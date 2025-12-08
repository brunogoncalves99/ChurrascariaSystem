$(document).ready(function () {
    carregarProdutos();

    // Aplicar filtros
    $('#filtroCategoria, #filtroDisponibilidade').on('change', function () {
        carregarProdutos();
    });

    $('#filtroBusca').on('keyup', debounce(function () {
        carregarProdutos();
    }, 500));
});

// Carregar produtos com filtros
function carregarProdutos() {
    const filtros = {
        categoria: $('#filtroCategoria').val(),
        disponivel: $('#filtroDisponibilidade').val(),
        busca: $('#filtroBusca').val()
    };

    $.ajax({
        url: '/Produto/ObterProdutos',
        method: 'GET',
        data: filtros,
        success: function (response) {
            renderizarProdutos(response);
        },
        error: function (xhr) {
            console.error('Erro ao carregar produtos:', xhr);
            Swal.fire({
                icon: 'error',
                title: 'Erro',
                text: 'Não foi possível carregar os produtos.',
                confirmButtonColor: '#dc3545'
            });
        }
    });
}

// Renderizar cards de produtos
function renderizarProdutos(produtos) {
    const container = $('#listaProdutosContainer');
    container.empty();

    if (produtos.length === 0) {
        container.html(`
            <div class="col-12">
                <div class="alert alert-info text-center">
                    <i class="fas fa-info-circle fa-2x mb-2"></i>
                    <p class="mb-0">Nenhum produto encontrado com os filtros aplicados.</p>
                </div>
            </div>
        `);
        return;
    }

    const categorias = {
        1: { nome: 'Carnes', icone: 'fa-drumstick-bite', cor: 'danger' },
        2: { nome: 'Bebidas', icone: 'fa-glass-cheers', cor: 'info' },
        3: { nome: 'Acompanhamentos', icone: 'fa-utensils', cor: 'warning' },
        4: { nome: 'Sobremesas', icone: 'fa-ice-cream', cor: 'success' }
    };

    produtos.forEach(function (produto) {
        const categoria = categorias[produto.categoria];
        const statusBadge = produto.disponivel
            ? '<span class="badge bg-success">Disponível</span>'
            : '<span class="badge bg-secondary">Indisponível</span>';

        const card = `
            <div class="col-md-6 col-lg-4 col-xl-3 mb-4">
                <div class="card h-100 shadow-sm">
                    <div class="card-header bg-${categoria.cor} text-white">
                        <div class="d-flex justify-content-between align-items-center">
                            <h6 class="mb-0">
                                <i class="fas ${categoria.icone}"></i> ${categoria.nome}
                            </h6>
                            ${statusBadge}
                        </div>
                    </div>
                    <div class="card-body">
                        <h5 class="card-title">${produto.nome}</h5>
                        <p class="card-text text-muted small">
                            ${produto.descricao || 'Sem descrição'}
                        </p>
                        <hr>
                        <h4 class="text-${categoria.cor} mb-0">
                            <i class="fas fa-dollar-sign"></i> ${formatarMoeda(produto.preco)}
                        </h4>
                    </div>
                    <div class="card-footer bg-light">
                        <button class="btn btn-primary btn-sm w-100 mb-2" onclick="editarProduto('${produto.id}')">
                            <i class="fas fa-edit"></i> Editar
                        </button>
                        <button class="btn btn-${produto.disponivel ? 'warning' : 'success'} btn-sm w-100" 
                                onclick="alternarDisponibilidade('${produto.id}', ${produto.disponivel})">
                            <i class="fas fa-${produto.disponivel ? 'eye-slash' : 'eye'}"></i> 
                            ${produto.disponivel ? 'Desativar' : 'Ativar'}
                        </button>
                    </div>
                </div>
            </div>
        `;

        container.append(card);
    });
}

// Abrir modal de novo produto
function abrirModalNovoProduto() {
    $('#formProduto')[0].reset();
    $('#produtoId').val('');
    $('#tituloModalProduto').html('<i class="fas fa-plus"></i> Novo Produto');
    $('#disponivel').prop('checked', true);
    $('#modalProduto').modal('show');
}

// Editar produto
function editarProduto(produtoId) {
    $.ajax({
        url: `/Produto/ObterPorId/${produtoId}`,
        method: 'GET',
        success: function (produto) {
            $('#produtoId').val(produto.id);
            $('#nomeProduto').val(produto.nome);
            $('#descricaoProduto').val(produto.descricao);
            $('#categoriaProduto').val(produto.categoria);
            $('#precoProduto').val(produto.preco);
            $('#disponivel').prop('checked', produto.disponivel);

            $('#tituloModalProduto').html('<i class="fas fa-edit"></i> Editar Produto');
            $('#modalProduto').modal('show');
        },
        error: function (xhr) {
            Swal.fire({
                icon: 'error',
                title: 'Erro',
                text: 'Não foi possível carregar o produto.',
                confirmButtonColor: '#dc3545'
            });
        }
    });
}

// Salvar produto (criar ou editar)
function salvarProduto() {
    const form = $('#formProduto');

    if (!form[0].checkValidity()) {
        form[0].reportValidity();
        return;
    }

    const produtoId = $('#produtoId').val();
    const isEdicao = produtoId !== '';

    const dados = {
        id: produtoId || null,
        nome: $('#nomeProduto').val().trim(),
        descricao: $('#descricaoProduto').val().trim(),
        categoria: parseInt($('#categoriaProduto').val()),
        preco: parseFloat($('#precoProduto').val()),
        disponivel: $('#disponivel').is(':checked')
    };

    const url = isEdicao ? '/Produto/Atualizar' : '/Produto/Criar';
    const metodo = isEdicao ? 'PUT' : 'POST';

    $.ajax({
        url: url,
        method: metodo,
        contentType: 'application/json',
        data: JSON.stringify(dados),
        success: function (response) {
            $('#modalProduto').modal('hide');

            Swal.fire({
                icon: 'success',
                title: isEdicao ? 'Produto atualizado!' : 'Produto criado!',
                text: `${dados.nome} foi ${isEdicao ? 'atualizado' : 'cadastrado'} com sucesso.`,
                confirmButtonColor: '#dc3545'
            }).then(() => {
                carregarProdutos();
            });
        },
        error: function (xhr) {
            let mensagem = 'Erro ao salvar produto. Tente novamente.';

            if (xhr.responseJSON && xhr.responseJSON.mensagem) {
                mensagem = xhr.responseJSON.mensagem;
            }

            Swal.fire({
                icon: 'error',
                title: 'Erro',
                text: mensagem,
                confirmButtonColor: '#dc3545'
            });
        }
    });
}

// Alternar disponibilidade do produto
function alternarDisponibilidade(produtoId, disponivelAtual) {
    const acao = disponivelAtual ? 'desativar' : 'ativar';

    Swal.fire({
        title: `${acao.charAt(0).toUpperCase() + acao.slice(1)} produto?`,
        text: `Tem certeza que deseja ${acao} este produto?`,
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#dc3545',
        cancelButtonColor: '#6c757d',
        confirmButtonText: `Sim, ${acao}`,
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `/Produto/AlterarDisponibilidade/${produtoId}`,
                method: 'PATCH',
                contentType: 'application/json',
                data: JSON.stringify({ disponivel: !disponivelAtual }),
                success: function (response) {
                    carregarProdutos();

                    Swal.fire({
                        toast: true,
                        position: 'top-end',
                        icon: 'success',
                        title: `Produto ${acao === 'ativar' ? 'ativado' : 'desativado'}!`,
                        showConfirmButton: false,
                        timer: 2000
                    });
                },
                error: function (xhr) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Erro',
                        text: `Não foi possível ${acao} o produto.`,
                        confirmButtonColor: '#dc3545'
                    });
                }
            });
        }
    });
}

// Excluir produto (opcional - caso queira implementar)
function excluirProduto(produtoId) {
    Swal.fire({
        title: 'Excluir produto?',
        text: 'Esta ação não pode ser desfeita!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#dc3545',
        cancelButtonColor: '#6c757d',
        confirmButtonText: 'Sim, excluir',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `/Produto/Excluir/${produtoId}`,
                method: 'DELETE',
                success: function (response) {
                    carregarProdutos();

                    Swal.fire({
                        icon: 'success',
                        title: 'Produto excluído!',
                        text: 'O produto foi removido do sistema.',
                        confirmButtonColor: '#dc3545'
                    });
                },
                error: function (xhr) {
                    let mensagem = 'Não foi possível excluir o produto.';

                    if (xhr.responseJSON && xhr.responseJSON.mensagem) {
                        mensagem = xhr.responseJSON.mensagem;
                    }

                    Swal.fire({
                        icon: 'error',
                        title: 'Erro',
                        text: mensagem,
                        confirmButtonColor: '#dc3545'
                    });
                }
            });
        }
    });
}

// Funções auxiliares
function formatarMoeda(valor) {
    return new Intl.NumberFormat('pt-BR', {
        style: 'currency',
        currency: 'BRL'
    }).format(valor);
}

function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}