// ============================================
// DASHBOARD.JS - Funções para Dashboard Administrativo
// ============================================

// Variáveis globais
let faturamentoChart = null;
let intervaloAtualizacao = null;

// ============================================
// INICIALIZAÇÃO
// ============================================
$(document).ready(function() {
    console.log('Dashboard.js carregado');
    inicializarDashboard();
    inicializarAutoRefresh();
});

// ============================================
// INICIALIZAÇÃO DO DASHBOARD
// ============================================
function inicializarDashboard() {
    carregarDadosDashboard();
    inicializarGraficos();
    inicializarEventos();
}

function inicializarEventos() {
    // Botão de refresh manual
    $('#btnRefresh').on('click', function() {
        carregarDadosDashboard();
        animarRefresh($(this));
    });
    
    // Seletor de período
    $('#periodoSelect').on('change', function() {
        const periodo = $(this).val();
        atualizarGraficoPorPeriodo(periodo);
    });
}

// ============================================
// CARREGAR DADOS DO DASHBOARD
// ============================================
function carregarDadosDashboard() {
    $.ajax({
        url: '/Dashboard/Index',
        type: 'GET',
        dataType: 'json',
        success: function(data) {
            atualizarCards(data);
            atualizarPedidosAbertos(data.pedidosAbertos);
            atualizarProdutosMaisVendidos(data.produtosMaisVendidos);
        },
        error: function() {
            mostrarErro('Erro ao carregar dados do dashboard');
        }
    });
}

// ============================================
// ATUALIZAR CARDS DE ESTATÍSTICAS
// ============================================
function atualizarCards(data) {
    // Faturamento Dia
    $('#faturamentoDia').countTo({
        from: 0,
        to: data.faturamentoDia,
        speed: 1000,
        refreshInterval: 50,
        formatter: function(value) {
            return 'R$ ' + value.toFixed(2);
        }
    });
    
    // Faturamento Mês
    $('#faturamentoMes').countTo({
        from: 0,
        to: data.faturamentoMes,
        speed: 1000,
        refreshInterval: 50,
        formatter: function(value) {
            return 'R$ ' + value.toFixed(2);
        }
    });
    
    // Pedidos Dia
    $('#pedidosDia').text(data.totalPedidosDia);
    
    // Pedidos Mês
    $('#pedidosMes').text(data.totalPedidosMes);
    
    // Mesas Ocupadas
    $('#mesasOcupadas').text(data.totalMesasOcupadas);
    
    // Mesas Livres
    $('#mesasLivres').text(data.totalMesasLivres);
    
    // Pedidos Abertos
    $('#pedidosAbertos').text(data.pedidosAbertos.length);
}

// ============================================
// GRÁFICOS
// ============================================
function inicializarGraficos() {
    carregarGraficoFaturamento();
}

function carregarGraficoFaturamento() {
    $.ajax({
        url: '/Dashboard/GetFaturamentoChart',
        type: 'GET',
        dataType: 'json',
        success: function(data) {
            renderizarGraficoFaturamento(data);
        },
        error: function() {
            console.error('Erro ao carregar gráfico de faturamento');
        }
    });
}

function renderizarGraficoFaturamento(dados) {
    const ctx = document.getElementById('faturamentoChart');
    if (!ctx) return;
    
    // Destruir gráfico anterior se existir
    if (faturamentoChart) {
        faturamentoChart.destroy();
    }
    
    const labels = dados.map(d => formatarDataCurta(d.data));
    const valores = dados.map(d => d.valor);
    
    faturamentoChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'Faturamento (R$)',
                data: valores,
                borderColor: '#0d6efd',
                backgroundColor: 'rgba(13, 110, 253, 0.1)',
                tension: 0.4,
                fill: true,
                pointRadius: 5,
                pointHoverRadius: 7,
                pointBackgroundColor: '#0d6efd',
                pointBorderColor: '#fff',
                pointBorderWidth: 2
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: true,
                    position: 'top'
                },
                tooltip: {
                    backgroundColor: 'rgba(0, 0, 0, 0.8)',
                    padding: 12,
                    titleFont: {
                        size: 14
                    },
                    bodyFont: {
                        size: 13
                    },
                    callbacks: {
                        label: function(context) {
                            return 'Faturamento: R$ ' + context.parsed.y.toFixed(2);
                        }
                    }
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        callback: function(value) {
                            return 'R$ ' + value.toFixed(0);
                        }
                    },
                    grid: {
                        color: 'rgba(0, 0, 0, 0.05)'
                    }
                },
                x: {
                    grid: {
                        display: false
                    }
                }
            },
            interaction: {
                intersect: false,
                mode: 'index'
            }
        }
    });
}

function atualizarGraficoPorPeriodo(periodo) {
    // Implementar lógica para diferentes períodos
    console.log('Atualizar gráfico para período:', periodo);
}

// ============================================
// PEDIDOS ABERTOS
// ============================================
function atualizarPedidosAbertos(pedidos) {
    const container = $('#pedidosAbertosLista');
    container.empty();
    
    if (pedidos.length === 0) {
        container.html(`
            <div class="text-center text-muted p-4">
                <i class="fas fa-clipboard-check fa-3x mb-2"></i>
                <p>Nenhum pedido aberto no momento</p>
            </div>
        `);
        return;
    }
    
    pedidos.forEach(pedido => {
        const statusBadge = getStatusBadge(pedido.status);
        const tempoDecorrido = calcularTempoDecorrido(pedido.dataPedido);
        
        const html = `
            <div class="pedido-card card mb-2 border-start border-${statusBadge.cor} border-4">
                <div class="card-body p-3">
                    <div class="d-flex justify-content-between align-items-start">
                        <div>
                            <h6 class="mb-1">
                                <i class="fas fa-chair"></i> Mesa ${pedido.mesaNumero}
                                <small class="text-muted">#${pedido.id}</small>
                            </h6>
                            <small class="text-muted">
                                <i class="fas fa-clock"></i> ${tempoDecorrido}
                            </small>
                        </div>
                        <div class="text-end">
                            <span class="badge bg-${statusBadge.cor} mb-1">${pedido.status}</span>
                            <h5 class="mb-0 text-success">R$ ${pedido.valorTotal.toFixed(2)}</h5>
                        </div>
                    </div>
                </div>
            </div>
        `;
        container.append(html);
    });
}

// ============================================
// PRODUTOS MAIS VENDIDOS
// ============================================
function atualizarProdutosMaisVendidos(produtos) {
    const container = $('#produtosMaisVendidos');
    container.empty();
    
    if (produtos.length === 0) {
        container.html('<p class="text-muted text-center">Nenhum produto vendido ainda</p>');
        return;
    }
    
    produtos.forEach((produto, index) => {
        const posicao = index + 1;
        const iconeMedalha = posicao === 1 ? '🥇' : posicao === 2 ? '🥈' : posicao === 3 ? '🥉' : '🏅';
        
        const html = `
            <div class="d-flex justify-content-between align-items-center mb-3 produto-ranking">
                <div>
                    <span class="fs-4 me-2">${iconeMedalha}</span>
                    <strong>${produto.nomeProduto}</strong>
                    <br>
                    <small class="text-muted ms-5">${produto.quantidade} unidades</small>
                </div>
                <span class="badge bg-success rounded-pill fs-6">
                    R$ ${produto.valorTotal.toFixed(2)}
                </span>
            </div>
        `;
        container.append(html);
    });
}

// ============================================
// AUTO-REFRESH
// ============================================
function inicializarAutoRefresh() {
    // Atualizar a cada 30 segundos
    intervaloAtualizacao = setInterval(function() {
        carregarDadosDashboard();
        console.log('Dashboard atualizado automaticamente');
    }, 30000);
}

function pararAutoRefresh() {
    if (intervaloAtualizacao) {
        clearInterval(intervaloAtualizacao);
        intervaloAtualizacao = null;
    }
}

// ============================================
// HELPERS
// ============================================
function getStatusBadge(status) {
    const badges = {
        'Aberto': { cor: 'warning', icone: 'fa-clock' },
        'Em Preparação': { cor: 'info', icone: 'fa-fire' },
        'Pronto': { cor: 'success', icone: 'fa-check' },
        'Entregue': { cor: 'primary', icone: 'fa-handshake' },
        'Cancelado': { cor: 'danger', icone: 'fa-times' }
    };
    return badges[status] || badges['Aberto'];
}

function calcularTempoDecorrido(dataPedido) {
    const agora = new Date();
    const pedido = new Date(dataPedido);
    const diff = Math.floor((agora - pedido) / 1000 / 60); // minutos
    
    if (diff < 1) return 'Agora mesmo';
    if (diff < 60) return `${diff} min atrás`;
    
    const horas = Math.floor(diff / 60);
    return `${horas}h atrás`;
}

function formatarDataCurta(data) {
    const d = new Date(data);
    return d.toLocaleDateString('pt-BR', { day: '2-digit', month: '2-digit' });
}

function animarRefresh(btn) {
    btn.addClass('fa-spin');
    setTimeout(() => {
        btn.removeClass('fa-spin');
    }, 1000);
}

function mostrarErro(mensagem) {
    console.error(mensagem);
    // Implementar notificação visual se necessário
}

// ============================================
// PLUGIN COUNTTO (Animação de números)
// ============================================
$.fn.countTo = function(options) {
    options = options || {};
    
    return $(this).each(function() {
        const $this = $(this);
        const from = options.from || 0;
        const to = options.to || 100;
        const speed = options.speed || 1000;
        const refreshInterval = options.refreshInterval || 50;
        const formatter = options.formatter || function(value) { return value; };
        
        const loops = Math.ceil(speed / refreshInterval);
        const increment = (to - from) / loops;
        
        let current = from;
        let loopCount = 0;
        
        const interval = setInterval(function() {
            current += increment;
            loopCount++;
            
            $this.text(formatter(current));
            
            if (loopCount >= loops) {
                clearInterval(interval);
                $this.text(formatter(to));
            }
        }, refreshInterval);
    });
};

// ============================================
// EXPORTAR FUNÇÕES GLOBAIS
// ============================================
window.DashboardJS = {
    carregarDadosDashboard,
    atualizarCards,
    carregarGraficoFaturamento,
    atualizarPedidosAbertos,
    atualizarProdutosMaisVendidos,
    pararAutoRefresh,
    inicializarAutoRefresh
};

// Limpar ao sair da página
$(window).on('beforeunload', function() {
    pararAutoRefresh();
});
