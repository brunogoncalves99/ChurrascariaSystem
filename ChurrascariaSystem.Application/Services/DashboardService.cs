using ChurrascariaSystem.Application.DTOs;
using ChurrascariaSystem.Application.Interfaces;
using ChurrascariaSystem.Domain.Interfaces;

namespace ChurrascariaSystem.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMesaRepository _mesaRepository;

        public DashboardService(IPedidoRepository pedidoRepository, IMesaRepository mesaRepository)
        {
            _pedidoRepository = pedidoRepository;
            _mesaRepository = mesaRepository;
        }

        public async Task<DashboardDTO> GetDashboardDataAsync()
        {
            var hoje = DateTime.Today;
            var inicioMes = new DateTime(hoje.Year, hoje.Month, 1);

            var dashboard = new DashboardDTO
            {
                FaturamentoDia = await GetFaturamentoDiaAsync(hoje),
                FaturamentoMes = await GetFaturamentoMesAsync(hoje.Month, hoje.Year),
                TotalPedidosDia = await GetTotalPedidosDiaAsync(hoje),
                TotalPedidosMes = await GetTotalPedidosMesAsync(hoje.Month, hoje.Year)
            };

            // Mesas ocupadas e livres
            var mesas = await _mesaRepository.GetAllActiveAsync();
            dashboard.TotalMesasOcupadas = mesas.Count(m => m.Status == Domain.Enums.StatusMesa.Ocupada);
            dashboard.TotalMesasLivres = mesas.Count(m => m.Status == Domain.Enums.StatusMesa.Livre);

            // Pedidos abertos
            var pedidosAbertos = await _pedidoRepository.GetPedidosAbertosAsync();
            dashboard.PedidosAbertos = pedidosAbertos.Select(p => new PedidoDTO
            {
                idPedido = p.idPedido,
                MesaNumero = p.Mesa?.Numero,
                DataPedido = p.DataPedido,
                Status = p.StatusPedidoValor,
                ValorTotal = p.ValorTotal
            }).ToList();

            // Produtos mais vendidos no mês
            var pedidosMes = await _pedidoRepository.GetPedidosByPeriodoAsync(inicioMes, hoje.AddDays(1));
            var produtosMaisVendidos = pedidosMes
                .SelectMany(p => p.Itens)
                .GroupBy(i => i.Produto.Nome)
                .Select(g => new ProdutoMaisVendidoDTO
                {
                    NomeProduto = g.Key,
                    Quantidade = g.Sum(i => i.Quantidade),
                    ValorTotal = g.Sum(i => i.Subtotal)
                })
                .OrderByDescending(p => p.Quantidade)
                .Take(5)
                .ToList();
            dashboard.ProdutosMaisVendidos = produtosMaisVendidos;

            // Faturamento dos últimos 7 dias
            var faturamentoUltimos7Dias = new List<FaturamentoDiarioDTO>();
            for (int i = 6; i >= 0; i--)
            {
                var data = hoje.AddDays(-i);
                var faturamento = await GetFaturamentoDiaAsync(data);
                faturamentoUltimos7Dias.Add(new FaturamentoDiarioDTO
                {
                    Data = data,
                    Valor = faturamento
                });
            }
            dashboard.FaturamentoUltimos7Dias = faturamentoUltimos7Dias;

            return dashboard;
        }

        public async Task<decimal> GetFaturamentoDiaAsync(DateTime data)
        {
            var pedidos = await _pedidoRepository.GetByDataAsync(data);
            var pedidosConcluidos = pedidos.Where(p => p.StatusPedidoValor == "Entregue");
            return pedidosConcluidos.Sum(p => p.ValorTotal);
        }

        public async Task<decimal> GetFaturamentoMesAsync(int mes, int ano)
        {
            var dataInicio = new DateTime(ano, mes, 1);
            var dataFim = dataInicio.AddMonths(1);
            var pedidos = await _pedidoRepository.GetPedidosByPeriodoAsync(dataInicio, dataFim);
            var pedidosConcluidos = pedidos.Where(p => p.StatusPedidoValor == "Entregue");
            return pedidosConcluidos.Sum(p => p.ValorTotal);
        }

        public async Task<int> GetTotalPedidosDiaAsync(DateTime data)
        {
            var pedidos = await _pedidoRepository.GetByDataAsync(data);
            return pedidos.Count();
        }

        public async Task<int> GetTotalPedidosMesAsync(int mes, int ano)
        {
            var dataInicio = new DateTime(ano, mes, 1);
            var dataFim = dataInicio.AddMonths(1);
            var pedidos = await _pedidoRepository.GetPedidosByPeriodoAsync(dataInicio, dataFim);
            return pedidos.Count();
        }
    }
}
