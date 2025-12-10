using ChurrascariaSystem.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ChurrascariaSystem.Web.Services
{
    /// <summary>
    /// Serviço em background que verifica o estoque periodicamente
    /// e envia alertas por email quando necessário
    /// </summary>
    public class MonitoramentoEstoqueService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MonitoramentoEstoqueService> _logger;
        private readonly TimeSpan _intervaloVerificacao;

        public MonitoramentoEstoqueService(
            IServiceProvider serviceProvider,
            ILogger<MonitoramentoEstoqueService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;

            _intervaloVerificacao = TimeSpan.FromMinutes(30);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Serviço de Monitoramento de Estoque iniciado.");

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await VerificarEstoqueAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao verificar estoque.");
                }

                // Aguardar próxima verificação
                await Task.Delay(_intervaloVerificacao, stoppingToken);
            }

            _logger.LogInformation("Serviço de Monitoramento de Estoque finalizado.");
        }

        private async Task VerificarEstoqueAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var estoqueService = scope.ServiceProvider.GetRequiredService<IEstoqueService>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                _logger.LogInformation("Iniciando verificação de estoque...");

                // Busca produtos com estoque baixo e esgotado
                var estoqueBaixo = await estoqueService.GetEstoqueBaixoAsync();
                var estoqueEsgotado = await estoqueService.GetEstoqueEsgotadoAsync();

                if (estoqueBaixo.Any() || estoqueEsgotado.Any())
                {
                    _logger.LogWarning($"Encontrados {estoqueBaixo.Count()} produtos com estoque baixo e {estoqueEsgotado.Count()} esgotados.");

                    var produtosAlerta = new List<(string Produto, int Atual, int Minimo)>();

                    // Adicionar produtos com estoque baixo (mas não esgotados)
                    foreach (var produto in estoqueBaixo.Where(p => !p.EstoqueEsgotado))
                    {
                        produtosAlerta.Add((produto.NomeProduto, produto.QuantidadeAtual, produto.QuantidadeMinima));
                    }

                    foreach (var produto in estoqueEsgotado)
                    {
                        produtosAlerta.Add((produto.NomeProduto, 0, produto.QuantidadeMinima));
                    }

                    if (produtosAlerta.Any())
                    {
                        var enviado = await emailService.EnviarResumoEstoqueBaixoAsync(produtosAlerta);

                        if (enviado)
                        {
                            _logger.LogInformation("Email de alerta de estoque enviado com sucesso!");
                        }
                        else
                        {
                            _logger.LogError("Falha ao enviar email de alerta de estoque.");
                        }
                    }
                }
                else
                {
                    _logger.LogInformation("Todos os estoques estão OK.");
                }
            }
        }
    }
}
