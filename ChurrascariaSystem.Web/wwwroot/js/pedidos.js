// ============================================
// PEDIDOS.JS - Funções JavaScript para Pedidos
// ============================================

// Variáveis globais
let carrinho = [];
let mesaSelecionada = null;

// ============================================
// INICIALIZAÇÃO
// ============================================
$(document).ready(function() {
    console.log('Pedidos.js carregado');
    inicializarEventos();
});

// ============================================
// EVENTOS
// ============================================
function inicializarEventos() {
    // Seleção de mesa
    $('#mesaSelect').on('change', selecionarMesa);
    
    // Filtro por tipo de produto
    $('#tipoSelect').on('change', filtrarProdutosPorTipo);
    
    // Adicionar produto ao carrinho
    $(document).on('click', '.btn-adicionar', adicionarAoCarrinho);
    
    // Remover item do carrinho
    $(document).on('click', '.btn-remover', removerDoCarrinho);
    
    // Finalizar pedido
    $('#btnFinalizar').on('click', finalizarPedido);
    
    // Atualizar status do pedido
    $(document).on('click', '.btn-status', atualizarStatusPedido);
    
    // Busca de produtos
    $('#searchProduto').on('keyup', buscarProdutos);
}

// ============================================
// SELEÇÃO DE MESA
// ============================================
function selecionarMesa() {
    mesaSelecionada = $(this).val();
    validarFinalizacao();
    
    if (mesaSelecionada) {
        mostrarNotificacao('Mesa selecionada', 'success');
    }
}

// ============================================
// FILTROS
// ============================================
function filtrarProdutosPorTipo() {
    const tipoId = $(this).val();
    
    if (tipoId === '') {
        $('.produto-card').show();
    } else {
        $('.produto-card').hide();
        $(`.produto-card[data-tipo="${tipoId}"]`).show();
    }
}

function buscarProdutos() {
    const termo = $(this).val().toLowerCase();
    
    $('.produto-card').each(function() {
        const nome = $(this).find('.card-title').text().toLowerCase();
        if (nome.includes(termo)) {
            $(this).show();
        } else {
            $(this).hide();
        }
    });
}

// ============================================
// CARRINHO DE COMPRAS
// ============================================
function adicionarAoCarrinho() {
    const btn = $(this);
    const input = btn.closest('.input-group').find('.quantidade');
    const produtoId = parseInt(btn.data('id'));
    const quantidade = parseInt(input.val());
    const nome = input.data('nome');
    const preco = parseFloat(input.data('preco'));
    
    if (quantidade < 1) {
        mostrarNotificacao('Quantidade inválida', 'error');
        return;
    }
    
    // Verifica se o produto já está no carrinho
    const existente = carrinho.find(item => item.produtoId === produtoId);
    
    if (existente) {
        existente.quantidade += quantidade;
        mostrarNotificacao(`Quantidade atualizada: ${nome}`, 'success');
    } else {
        carrinho.push({
            produtoId: produtoId,
            nome: nome,
            quantidade: quantidade,
            precoUnitario: preco
        });
        mostrarNotificacao(`Adicionado: ${nome}`, 'success');
    }
    
    atualizarCarrinho();
    input.val(1); // Reset quantidade
    
    // Animação
    btn.addClass('btn-success').removeClass('btn-primary');
    setTimeout(() => {
        btn.removeClass('btn-success').addClass('btn-primary');
    }, 500);
}

function removerDoCarrinho() {
    const index = $(this).data('index');
    const nomeRemovido = carrinho[index].nome;
    
    carrinho.splice(index, 1);
    atualizarCarrinho();
    
    mostrarNotificacao(`Removido: ${nomeRemovido}`, 'info');
}

function atualizarCarrinho() {
    const container = $('#carrinhoItens');
    container.empty();
    
    if (carrinho.length === 0) {
        container.html(`
            <div class="text-center text-muted p-3">
                <i class="fas fa-shopping-cart fa-3x mb-2"></i>
                <p>Carrinho vazio</p>
            </div>
        `);
        $('#valorTotal').text('R$ 0,00');
        validarFinalizacao();
        return;
    }
    
    let total = 0;
    
    carrinho.forEach((item, index) => {
        const subtotal = item.quantidade * item.precoUnitario;
        total += subtotal;
        
        const html = `
            <div class="card mb-2 carrinho-item">
                <div class="card-body p-2">
                    <div class="d-flex justify-content-between align-items-start">
                        <div class="flex-grow-1">
                            <h6 class="mb-1">${item.nome}</h6>
                            <small class="text-muted">
                                ${item.quantidade}x R$ ${item.precoUnitario.toFixed(2)}
                            </small>
                        </div>
                        <button class="btn btn-sm btn-danger btn-remover" data-index="${index}">
                            <i class="fas fa-times"></i>
                        </button>
                    </div>
                    <div class="text-end mt-2">
                        <strong class="text-success">R$ ${subtotal.toFixed(2)}</strong>
                    </div>
                </div>
            </div>
        `;
        container.append(html);
    });
    
    $('#valorTotal').text(`R$ ${total.toFixed(2)}`);
    validarFinalizacao();
}

function validarFinalizacao() {
    const mesaValida = mesaSelecionada && mesaSelecionada !== '';
    const carrinhoValido = carrinho.length > 0;
    $('#btnFinalizar').prop('disabled', !(mesaValida && carrinhoValido));
}

// ============================================
// FINALIZAR PEDIDO
// ============================================
function finalizarPedido() {
    if (!confirm('Confirmar criação do pedido?')) return;
    
    const pedido = {
        mesaId: parseInt(mesaSelecionada),
        observacao: $('#observacao').val(),
        itens: carrinho.map(item => ({
            produtoId: item.produtoId,
            quantidade: item.quantidade,
            observacao: ''
        }))
    };
    
    // Mostrar loading
    $('#btnFinalizar').prop('disabled', true).html('<i class="fas fa-spinner fa-spin"></i> Processando...');
    
    $.ajax({
        url: '/Pedido/Criar',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(pedido),
        success: function(response) {
            if (response.success) {
                mostrarNotificacao('Pedido criado com sucesso!', 'success');
                setTimeout(() => {
                    window.location.href = '/Pedido/Index';
                }, 1500);
            } else {
                mostrarNotificacao('Erro: ' + response.message, 'error');
                $('#btnFinalizar').prop('disabled', false).html('<i class="fas fa-check-circle"></i> Finalizar Pedido');
            }
        },
        error: function() {
            mostrarNotificacao('Erro ao criar pedido', 'error');
            $('#btnFinalizar').prop('disabled', false).html('<i class="fas fa-check-circle"></i> Finalizar Pedido');
        }
    });
}

// ============================================
// ATUALIZAR STATUS DO PEDIDO
// ============================================
function atualizarStatusPedido() {
    const btn = $(this);
    const pedidoId = btn.data('id');
    const novoStatus = btn.data('status');
    
    if (!confirm(`Alterar status para "${novoStatus}"?`)) return;
    
    btn.prop('disabled', true).html('<i class="fas fa-spinner fa-spin"></i>');
    
    $.ajax({
        url: '/Pedido/UpdateStatus',
        type: 'POST',
        data: { id: pedidoId, status: novoStatus },
        success: function(response) {
            if (response.success) {
                mostrarNotificacao('Status atualizado!', 'success');
                setTimeout(() => location.reload(), 1000);
            } else {
                mostrarNotificacao('Erro: ' + response.message, 'error');
                btn.prop('disabled', false);
            }
        },
        error: function() {
            mostrarNotificacao('Erro ao atualizar status', 'error');
            btn.prop('disabled', false);
        }
    });
}

// ============================================
// NOTIFICAÇÕES
// ============================================
function mostrarNotificacao(mensagem, tipo) {
    const tipos = {
        success: { classe: 'alert-success', icone: 'fa-check-circle' },
        error: { classe: 'alert-danger', icone: 'fa-exclamation-circle' },
        info: { classe: 'alert-info', icone: 'fa-info-circle' },
        warning: { classe: 'alert-warning', icone: 'fa-exclamation-triangle' }
    };
    
    const config = tipos[tipo] || tipos.info;
    
    const alerta = $(`
        <div class="alert ${config.classe} alert-dismissible fade show notificacao-flutuante" role="alert">
            <i class="fas ${config.icone}"></i> ${mensagem}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    `);
    
    $('#notificacoes').append(alerta);
    
    setTimeout(() => {
        alerta.alert('close');
    }, 3000);
}

// ============================================
// FORMATAÇÃO
// ============================================
function formatarMoeda(valor) {
    return valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
}

function formatarData(data) {
    const d = new Date(data);
    return d.toLocaleDateString('pt-BR') + ' ' + d.toLocaleTimeString('pt-BR', { hour: '2-digit', minute: '2-digit' });
}

// ============================================
// EXPORTAR FUNÇÕES GLOBAIS
// ============================================
window.PedidosJS = {
    adicionarAoCarrinho,
    removerDoCarrinho,
    finalizarPedido,
    atualizarStatusPedido,
    mostrarNotificacao,
    formatarMoeda,
    formatarData
};
