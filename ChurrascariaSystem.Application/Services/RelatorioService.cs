using ChurrascariaSystem.Application.Interfaces;
using ChurrascariaSystem.Domain.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;
using QuestPDF;

namespace ChurrascariaSystem.Application.Services
{
    /// <summary>
    /// Serviço de geração de relatórios em PDF usando QuestPDF
    /// </summary>
    public class RelatorioService : IRelatorioService
    {
        private readonly IMesaService _mesaService;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public RelatorioService(
            IMesaService mesaService,
            IPedidoRepository pedidoRepository,
            IUsuarioRepository usuarioRepository)
        {
            _mesaService = mesaService;
            _pedidoRepository = pedidoRepository;
            _usuarioRepository = usuarioRepository;
        }

        /// <summary>
        /// Gera PDF da conta do cliente
        /// </summary>
        public async Task<byte[]> GerarContaClienteAsync(int mesaId)
        {
            var conta = await _mesaService.GetContaMesaAsync(mesaId);

            if (conta == null)
                throw new Exception("Conta não encontrada");

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);

                    page.Header().Column(column =>
                    {
                        column.Item().AlignCenter().Text("🍖 Sistema de Churrasquinho")
                            .FontSize(20)
                            .Bold()
                            .FontColor(Colors.Red.Medium);

                        column.Item().AlignCenter().Text("Conta do Cliente")
                            .FontSize(16)
                            .SemiBold();

                        column.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                    });

                    // Content
                    page.Content().Column(column =>
                    {
                        // Informações da Mesa
                        column.Item().PaddingVertical(10).Row(row =>
                        {
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text($"Mesa: {conta.MesaNumero}").FontSize(14).Bold();
                                col.Item().Text($"Data: {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(10);
                            });

                            row.RelativeItem().Column(col =>
                            {
                                col.Item().AlignRight().Text($"Total de Pedidos: {conta.QuantidadePedidos}").FontSize(10);
                            });
                        });

                        column.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        // Tabela de Itens
                        column.Item().Table(table =>
                        {
                            // Define colunas
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3); // Produto
                                columns.ConstantColumn(60); // Qtd
                                columns.ConstantColumn(80); // Preço Unit
                                columns.ConstantColumn(100); // Subtotal
                            });

                            // Header da tabela
                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Red.Medium)
                                    .Padding(5).Text("Produto").FontColor(Colors.White).FontSize(11).Bold();

                                header.Cell().Background(Colors.Red.Medium)
                                    .Padding(5).Text("Qtd").FontColor(Colors.White).FontSize(11).Bold();

                                header.Cell().Background(Colors.Red.Medium)
                                    .Padding(5).Text("Preço Unit.").FontColor(Colors.White).FontSize(11).Bold();

                                header.Cell().Background(Colors.Red.Medium)
                                    .Padding(5).Text("Subtotal").FontColor(Colors.White).FontSize(11).Bold();
                            });

                            // Agrupar itens por produto
                            var itensAgrupados = conta.Pedidos
                                .SelectMany(p => p.Itens)
                                .GroupBy(i => i.ProdutoNome)
                                .Select(g => new
                                {
                                    Produto = g.Key,
                                    Quantidade = g.Sum(i => i.Quantidade),
                                    PrecoUnitario = g.First().PrecoUnitario,
                                    Subtotal = g.Sum(i => i.Subtotal)
                                })
                                .OrderBy(i => i.Produto);

                            // Linhas da tabela
                            foreach (var item in itensAgrupados)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                    .Padding(5).Text(item.Produto).FontSize(10);

                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                    .Padding(5).AlignCenter().Text($"{item.Quantidade}x").FontSize(10);

                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                    .Padding(5).AlignRight().Text($"R$ {item.PrecoUnitario:N2}").FontSize(10);

                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                    .Padding(5).AlignRight().Text($"R$ {item.Subtotal:N2}").FontSize(10).Bold();
                            }
                        });

                        // Total
                        column.Item().PaddingTop(20).AlignRight().Row(row =>
                        {
                            row.ConstantItem(200).Background(Colors.Red.Lighten4)
                                .Padding(10).Column(col =>
                                {
                                    col.Item().Text("TOTAL A PAGAR").FontSize(12).SemiBold();
                                    col.Item().Text($"R$ {conta.ValorTotal:N2}")
                                        .FontSize(20)
                                        .Bold()
                                        .FontColor(Colors.Red.Darken2);
                                });
                        });

                        // Observações
                        column.Item().PaddingTop(30).AlignCenter().Text("Obrigado pela preferência!")
                            .FontSize(10)
                            .Italic();
                    });

                    // Footer
                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Gerado em: ");
                        text.Span($"{DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(8);
                    });
                });
            }).GeneratePdf();
        }

        /// <summary>
        /// Gera relatório de vendas diárias
        /// </summary>
        public async Task<byte[]> GerarRelatorioVendasDiariaAsync(DateTime data)
        {
            var pedidos = await _pedidoRepository.GetAllAsync();
            var pedidosDia = pedidos
                .Where(p => p.DataPedido.Date == data.Date && p.Pago)
                .OrderBy(p => p.DataPedido)
                .ToList();

            var totalVendas = pedidosDia.Sum(p => p.ValorTotal);
            var quantidadePedidos = pedidosDia.Count;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);

                    // Header
                    page.Header().Column(column =>
                    {
                        column.Item().AlignCenter().Text("🍖 Sistema de Churrasquinho")
                            .FontSize(20)
                            .Bold()
                            .FontColor(Colors.Red.Medium);

                        column.Item().AlignCenter().Text($"Relatório de Vendas - {data:dd/MM/yyyy}")
                            .FontSize(16)
                            .SemiBold();

                        column.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                    });

                    // Content
                    page.Content().Column(column =>
                    {
                        // Resumo
                        column.Item().PaddingVertical(10).Row(row =>
                        {
                            row.RelativeItem().Background(Colors.Blue.Lighten4)
                                .Padding(10).Column(col =>
                                {
                                    col.Item().Text("Total de Pedidos").FontSize(10);
                                    col.Item().Text($"{quantidadePedidos}").FontSize(16).Bold();
                                });

                            row.ConstantItem(20);

                            row.RelativeItem().Background(Colors.Green.Lighten4)
                                .Padding(10).Column(col =>
                                {
                                    col.Item().Text("Faturamento Total").FontSize(10);
                                    col.Item().Text($"R$ {totalVendas:N2}").FontSize(16).Bold();
                                });

                            row.ConstantItem(20);

                            row.RelativeItem().Background(Colors.Orange.Lighten4)
                                .Padding(10).Column(col =>
                                {
                                    col.Item().Text("Ticket Médio").FontSize(10);
                                    col.Item().Text($"R$ {(quantidadePedidos > 0 ? totalVendas / quantidadePedidos : 0):N2}")
                                        .FontSize(16).Bold();
                                });
                        });

                        column.Item().PaddingVertical(15).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        // Tabela de Pedidos
                        if (pedidosDia.Any())
                        {
                            column.Item().Text("Detalhamento de Pedidos").FontSize(14).Bold();

                            column.Item().PaddingVertical(10).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(60);  // Pedido
                                    columns.ConstantColumn(80);  // Hora
                                    columns.ConstantColumn(70);  // Mesa
                                    columns.RelativeColumn();    // Garçom
                                    columns.ConstantColumn(100); // Forma Pgto
                                    columns.ConstantColumn(90);  // Valor
                                });

                                // Header
                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Grey.Darken2)
                                        .Padding(5).Text("Pedido").FontColor(Colors.White).FontSize(10).Bold();
                                    header.Cell().Background(Colors.Grey.Darken2)
                                        .Padding(5).Text("Hora").FontColor(Colors.White).FontSize(10).Bold();
                                    header.Cell().Background(Colors.Grey.Darken2)
                                        .Padding(5).Text("Mesa").FontColor(Colors.White).FontSize(10).Bold();
                                    header.Cell().Background(Colors.Grey.Darken2)
                                        .Padding(5).Text("Garçom").FontColor(Colors.White).FontSize(10).Bold();
                                    header.Cell().Background(Colors.Grey.Darken2)
                                        .Padding(5).Text("Forma Pgto").FontColor(Colors.White).FontSize(10).Bold();
                                    header.Cell().Background(Colors.Grey.Darken2)
                                        .Padding(5).Text("Valor").FontColor(Colors.White).FontSize(10).Bold();
                                });

                                // Dados
                                foreach (var pedido in pedidosDia)
                                {
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                        .Padding(5).Text($"#{pedido.idPedido}").FontSize(9);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                        .Padding(5).Text(pedido.DataPedido.ToString("HH:mm")).FontSize(9);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                        .Padding(5).Text(pedido.Mesa?.Numero ?? "").FontSize(9);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                        .Padding(5).Text(pedido.Usuario?.Nome ?? "").FontSize(9);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                        .Padding(5).Text(pedido.FormaPagamento ?? "-").FontSize(9);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                        .Padding(5).AlignRight().Text($"R$ {pedido.ValorTotal:N2}").FontSize(9).Bold();
                                }
                            });
                        }
                        else
                        {
                            column.Item().PaddingVertical(20).AlignCenter()
                                .Text("Nenhuma venda realizada nesta data")
                                .FontSize(12)
                                .Italic()
                                .FontColor(Colors.Grey.Medium);
                        }
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Página ");
                        text.CurrentPageNumber();
                        text.Span(" de ");
                        text.TotalPages();
                        text.Span($" | Gerado em {DateTime.Now:dd/MM/yyyy HH:mm}");
                    });
                });
            }).GeneratePdf();
        }

        /// <summary>
        /// Gera relatório de produtos mais vendidos
        /// </summary>
        public async Task<byte[]> GerarRelatorioProdutosMaisVendidosAsync(DateTime dataInicio, DateTime dataFim)
        {
            var pedidos = await _pedidoRepository.GetAllAsync();
            var pedidosPeriodo = pedidos
                .Where(p => p.DataPedido.Date >= dataInicio.Date
                         && p.DataPedido.Date <= dataFim.Date
                         && p.Pago)
                .ToList();

            var produtosMaisVendidos = pedidosPeriodo
                .SelectMany(p => p.Itens)
                .GroupBy(i => i.Produto.Nome)
                .Select(g => new
                {
                    Produto = g.Key,
                    Quantidade = g.Sum(i => i.Quantidade),
                    Faturamento = g.Sum(i => i.Subtotal),
                    QuantidadePedidos = g.Select(i => i.idPedido).Distinct().Count()
                })
                .OrderByDescending(p => p.Quantidade)
                .Take(20)
                .ToList();

            var totalGeral = produtosMaisVendidos.Sum(p => p.Faturamento);

            return QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);

                    // Header
                    page.Header().Column(column =>
                    {
                        column.Item().AlignCenter().Text("🍖 Sistema de Churrasquinho")
                            .FontSize(20)
                            .Bold()
                            .FontColor(Colors.Red.Medium);

                        column.Item().AlignCenter().Text("Produtos Mais Vendidos")
                            .FontSize(16)
                            .SemiBold();

                        column.Item().AlignCenter().Text($"Período: {dataInicio:dd/MM/yyyy} a {dataFim:dd/MM/yyyy}")
                            .FontSize(12);

                        column.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                    });

                    // Content
                    page.Content().Column(column =>
                    {
                        if (produtosMaisVendidos.Any())
                        {
                            // Resumo
                            column.Item().PaddingVertical(10).Background(Colors.Green.Lighten4)
                                .Padding(10).Row(row =>
                                {
                                    row.RelativeItem().Text($"Total de Produtos: {produtosMaisVendidos.Count}").FontSize(12);
                                    row.RelativeItem().AlignRight().Text($"Faturamento Total: R$ {totalGeral:N2}").FontSize(12).Bold();
                                });

                            column.Item().PaddingVertical(15);

                            // Tabela
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(40);  // Posição
                                    columns.RelativeColumn(3);   // Produto
                                    columns.ConstantColumn(80);  // Qtd Vendida
                                    columns.ConstantColumn(80);  // Qtd Pedidos
                                    columns.ConstantColumn(100); // Faturamento
                                    columns.ConstantColumn(80);  // % do Total
                                });

                                // Header
                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Blue.Medium)
                                        .Padding(5).Text("Pos.").FontColor(Colors.White).FontSize(10).Bold();
                                    header.Cell().Background(Colors.Blue.Medium)
                                        .Padding(5).Text("Produto").FontColor(Colors.White).FontSize(10).Bold();
                                    header.Cell().Background(Colors.Blue.Medium)
                                        .Padding(5).Text("Qtd Vendida").FontColor(Colors.White).FontSize(10).Bold();
                                    header.Cell().Background(Colors.Blue.Medium)
                                        .Padding(5).Text("Pedidos").FontColor(Colors.White).FontSize(10).Bold();
                                    header.Cell().Background(Colors.Blue.Medium)
                                        .Padding(5).Text("Faturamento").FontColor(Colors.White).FontSize(10).Bold();
                                    header.Cell().Background(Colors.Blue.Medium)
                                        .Padding(5).Text("% Total").FontColor(Colors.White).FontSize(10).Bold();
                                });

                                // Dados
                                int posicao = 1;
                                foreach (var produto in produtosMaisVendidos)
                                {
                                    var percentual = (produto.Faturamento / totalGeral) * 100;
                                    var bgColor = posicao <= 3 ? Colors.Yellow.Lighten4 : Colors.White;

                                    table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                        .Padding(5).AlignCenter().Text($"{posicao}º").FontSize(9).Bold();
                                    table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                        .Padding(5).Text(produto.Produto).FontSize(9);
                                    table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                        .Padding(5).AlignCenter().Text($"{produto.Quantidade}").FontSize(9).Bold();
                                    table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                        .Padding(5).AlignCenter().Text($"{produto.QuantidadePedidos}").FontSize(9);
                                    table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                        .Padding(5).AlignRight().Text($"R$ {produto.Faturamento:N2}").FontSize(9).Bold();
                                    table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                        .Padding(5).AlignRight().Text($"{percentual:N1}%").FontSize(9);

                                    posicao++;
                                }
                            });

                            // Legenda Top 3
                            column.Item().PaddingTop(15).Background(Colors.Yellow.Lighten4)
                                .Padding(5).Text("⭐ Top 3 produtos destacados em amarelo")
                                .FontSize(9)
                                .Italic();
                        }
                        else
                        {
                            column.Item().PaddingVertical(20).AlignCenter()
                                .Text("Nenhuma venda realizada no período")
                                .FontSize(12)
                                .Italic()
                                .FontColor(Colors.Grey.Medium);
                        }
                    });

                    // Footer
                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span($"Gerado em {DateTime.Now:dd/MM/yyyy HH:mm}");
                    });
                });
            }).GeneratePdf();
        }

        /// <summary>
        /// Gera relatório de faturamento mensal
        /// </summary>
        public async Task<byte[]> GerarRelatorioFaturamentoMensalAsync(int mes, int ano)
        {
            var pedidos = await _pedidoRepository.GetAllAsync();
            var pedidosMes = pedidos
                .Where(p => p.DataPedido.Month == mes
                         && p.DataPedido.Year == ano
                         && p.Pago)
                .ToList();

            var faturamentoDiario = pedidosMes
                .GroupBy(p => p.DataPedido.Date)
                .Select(g => new
                {
                    Data = g.Key,
                    Quantidade = g.Count(),
                    Faturamento = g.Sum(p => p.ValorTotal)
                })
                .OrderBy(f => f.Data)
                .ToList();

            var totalMes = pedidosMes.Sum(p => p.ValorTotal);
            var totalPedidos = pedidosMes.Count;
            var ticketMedio = totalPedidos > 0 ? totalMes / totalPedidos : 0;

            var nomeMes = new DateTime(ano, mes, 1).ToString("MMMM/yyyy");

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);

                    // Header
                    page.Header().Column(column =>
                    {
                        column.Item().AlignCenter().Text("🍖 Sistema de Churrasquinho")
                            .FontSize(20)
                            .Bold()
                            .FontColor(Colors.Red.Medium);

                        column.Item().AlignCenter().Text($"Relatório de Faturamento - {nomeMes}")
                            .FontSize(16)
                            .SemiBold();

                        column.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                    });

                    // Content
                    page.Content().Column(column =>
                    {
                        // Resumo do mês
                        column.Item().PaddingVertical(10).Row(row =>
                        {
                            row.RelativeItem().Background(Colors.Blue.Lighten4)
                                .Padding(10).Column(col =>
                                {
                                    col.Item().Text("Faturamento Total").FontSize(10);
                                    col.Item().Text($"R$ {totalMes:N2}").FontSize(16).Bold();
                                });

                            row.ConstantItem(15);

                            row.RelativeItem().Background(Colors.Green.Lighten4)
                                .Padding(10).Column(col =>
                                {
                                    col.Item().Text("Total de Pedidos").FontSize(10);
                                    col.Item().Text($"{totalPedidos}").FontSize(16).Bold();
                                });

                            row.ConstantItem(15);

                            row.RelativeItem().Background(Colors.Orange.Lighten4)
                                .Padding(10).Column(col =>
                                {
                                    col.Item().Text("Ticket Médio").FontSize(10);
                                    col.Item().Text($"R$ {ticketMedio:N2}").FontSize(16).Bold();
                                });
                        });

                        column.Item().PaddingVertical(15).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        // Faturamento diário
                        if (faturamentoDiario.Any())
                        {
                            column.Item().Text("Faturamento Diário").FontSize(14).Bold();

                            column.Item().PaddingVertical(10).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(100); // Data
                                    columns.RelativeColumn();    // Dia da Semana
                                    columns.ConstantColumn(100); // Qtd Pedidos
                                    columns.ConstantColumn(120); // Faturamento
                                    columns.ConstantColumn(100); // Ticket Médio
                                });

                                // Header
                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Grey.Darken2)
                                        .Padding(5).Text("Data").FontColor(Colors.White).FontSize(10).Bold();
                                    header.Cell().Background(Colors.Grey.Darken2)
                                        .Padding(5).Text("Dia da Semana").FontColor(Colors.White).FontSize(10).Bold();
                                    header.Cell().Background(Colors.Grey.Darken2)
                                        .Padding(5).Text("Pedidos").FontColor(Colors.White).FontSize(10).Bold();
                                    header.Cell().Background(Colors.Grey.Darken2)
                                        .Padding(5).Text("Faturamento").FontColor(Colors.White).FontSize(10).Bold();
                                    header.Cell().Background(Colors.Grey.Darken2)
                                        .Padding(5).Text("Ticket Médio").FontColor(Colors.White).FontSize(10).Bold();
                                });

                                // Dados
                                foreach (var dia in faturamentoDiario)
                                {
                                    var diaSemana = dia.Data.ToString("dddd");
                                    var ticketDia = dia.Faturamento / dia.Quantidade;

                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                        .Padding(5).Text(dia.Data.ToString("dd/MM/yyyy")).FontSize(9);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                        .Padding(5).Text(diaSemana).FontSize(9);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                        .Padding(5).AlignCenter().Text($"{dia.Quantidade}").FontSize(9);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                        .Padding(5).AlignRight().Text($"R$ {dia.Faturamento:N2}").FontSize(9).Bold();
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                        .Padding(5).AlignRight().Text($"R$ {ticketDia:N2}").FontSize(9);
                                }
                            });
                        }
                        else
                        {
                            column.Item().PaddingVertical(20).AlignCenter()
                                .Text("Nenhuma venda realizada neste mês")
                                .FontSize(12)
                                .Italic()
                                .FontColor(Colors.Grey.Medium);
                        }
                    });

                    // Footer
                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Página ");
                        text.CurrentPageNumber();
                        text.Span(" de ");
                        text.TotalPages();
                        text.Span($" | Gerado em {DateTime.Now:dd/MM/yyyy HH:mm}");
                    });
                });
            }).GeneratePdf();
        }

        /// <summary>
        /// Gera relatório de vendas por forma de pagamento
        /// </summary>
        public async Task<byte[]> GerarRelatorioFormasPagamentoAsync(DateTime dataInicio, DateTime dataFim)
        {
            var pedidos = await _pedidoRepository.GetAllAsync();
            var pedidosPeriodo = pedidos
                .Where(p => p.DataPedido.Date >= dataInicio.Date
                         && p.DataPedido.Date <= dataFim.Date
                         && p.Pago)
                .ToList();

            var vendasPorFormaPagamento = pedidosPeriodo
                .GroupBy(p => p.FormaPagamento ?? "Não Informado")
                .Select(g => new
                {
                    FormaPagamento = g.Key,
                    Quantidade = g.Count(),
                    Faturamento = g.Sum(p => p.ValorTotal)
                })
                .OrderByDescending(v => v.Faturamento)
                .ToList();

            var totalGeral = vendasPorFormaPagamento.Sum(v => v.Faturamento);
            var totalPedidos = vendasPorFormaPagamento.Sum(v => v.Quantidade);

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);

                    // Header
                    page.Header().Column(column =>
                    {
                        column.Item().AlignCenter().Text("🍖 Sistema de Churrasquinho")
                            .FontSize(20)
                            .Bold()
                            .FontColor(Colors.Red.Medium);

                        column.Item().AlignCenter().Text("Relatório por Forma de Pagamento")
                            .FontSize(16)
                            .SemiBold();

                        column.Item().AlignCenter().Text($"Período: {dataInicio:dd/MM/yyyy} a {dataFim:dd/MM/yyyy}")
                            .FontSize(12);

                        column.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                    });

                    // Content
                    page.Content().Column(column =>
                    {
                        if (vendasPorFormaPagamento.Any())
                        {
                            // Resumo
                            column.Item().PaddingVertical(10).Row(row =>
                            {
                                row.RelativeItem().Background(Colors.Green.Lighten4)
                                    .Padding(10).Column(col =>
                                    {
                                        col.Item().Text("Faturamento Total").FontSize(10);
                                        col.Item().Text($"R$ {totalGeral:N2}").FontSize(16).Bold();
                                    });

                                row.ConstantItem(20);

                                row.RelativeItem().Background(Colors.Blue.Lighten4)
                                    .Padding(10).Column(col =>
                                    {
                                        col.Item().Text("Total de Pedidos").FontSize(10);
                                        col.Item().Text($"{totalPedidos}").FontSize(16).Bold();
                                    });
                            });

                            column.Item().PaddingVertical(15);

                            // Tabela
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);   // Forma de Pagamento
                                    columns.ConstantColumn(100); // Qtd Pedidos
                                    columns.ConstantColumn(120); // Faturamento
                                    columns.ConstantColumn(80);  // % do Total
                                    columns.ConstantColumn(100); // Ticket Médio
                                });

                                // Header
                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Purple.Medium)
                                        .Padding(5).Text("Forma de Pagamento").FontColor(Colors.White).FontSize(10).Bold();
                                    header.Cell().Background(Colors.Purple.Medium)
                                        .Padding(5).Text("Qtd Pedidos").FontColor(Colors.White).FontSize(10).Bold();
                                    header.Cell().Background(Colors.Purple.Medium)
                                        .Padding(5).Text("Faturamento").FontColor(Colors.White).FontSize(10).Bold();
                                    header.Cell().Background(Colors.Purple.Medium)
                                        .Padding(5).Text("% Total").FontColor(Colors.White).FontSize(10).Bold();
                                    header.Cell().Background(Colors.Purple.Medium)
                                        .Padding(5).Text("Ticket Médio").FontColor(Colors.White).FontSize(10).Bold();
                                });

                                // Dados
                                foreach (var forma in vendasPorFormaPagamento)
                                {
                                    var percentual = (forma.Faturamento / totalGeral) * 100;
                                    var ticketMedio = forma.Faturamento / forma.Quantidade;

                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                        .Padding(5).Text(forma.FormaPagamento).FontSize(9).Bold();
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                        .Padding(5).AlignCenter().Text($"{forma.Quantidade}").FontSize(9);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                        .Padding(5).AlignRight().Text($"R$ {forma.Faturamento:N2}").FontSize(9).Bold();
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                        .Padding(5).AlignRight().Text($"{percentual:N1}%").FontSize(9);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                        .Padding(5).AlignRight().Text($"R$ {ticketMedio:N2}").FontSize(9);
                                }
                            });
                        }
                        else
                        {
                            column.Item().PaddingVertical(20).AlignCenter()
                                .Text("Nenhuma venda realizada no período")
                                .FontSize(12)
                                .Italic()
                                .FontColor(Colors.Grey.Medium);
                        }
                    });

                    // Footer
                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span($"Gerado em {DateTime.Now:dd/MM/yyyy HH:mm}");
                    });
                });
            }).GeneratePdf();
        }

        /// <summary>
        /// Gera relatório de performance dos garçons
        /// </summary>
        public async Task<byte[]> GerarRelatorioPerformanceGarcomAsync(DateTime dataInicio, DateTime dataFim)
        {
            var pedidos = await _pedidoRepository.GetAllAsync();
            var pedidosPeriodo = pedidos
                .Where(p => p.DataPedido.Date >= dataInicio.Date
                         && p.DataPedido.Date <= dataFim.Date
                         && p.Pago)
                .ToList();

            var performanceGarcom = pedidosPeriodo
                .GroupBy(p => p.Usuario.Nome)
                .Select(g => new
                {
                    Garcom = g.Key,
                    QuantidadePedidos = g.Count(),
                    Faturamento = g.Sum(p => p.ValorTotal),
                    TicketMedio = g.Average(p => p.ValorTotal)
                })
                .OrderByDescending(p => p.Faturamento)
                .ToList();

            var totalGeral = performanceGarcom.Sum(p => p.Faturamento);
            var totalPedidos = performanceGarcom.Sum(p => p.QuantidadePedidos);

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);

                    // Header
                    page.Header().Column(column =>
                    {
                        column.Item().AlignCenter().Text("🍖 Sistema de Churrasquinho")
                            .FontSize(20)
                            .Bold()
                            .FontColor(Colors.Red.Medium);

                        column.Item().AlignCenter().Text("Relatório de Performance dos Garçons")
                            .FontSize(16)
                            .SemiBold();

                        column.Item().AlignCenter().Text($"Período: {dataInicio:dd/MM/yyyy} a {dataFim:dd/MM/yyyy}")
                            .FontSize(12);

                        column.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                    });

                    // Content
                    page.Content().Column(column =>
                    {
                        if (performanceGarcom.Any())
                        {
                            // Resumo
                            column.Item().PaddingVertical(10).Row(row =>
                            {
                                row.RelativeItem().Background(Colors.Green.Lighten4)
                                    .Padding(10).Column(col =>
                                    {
                                        col.Item().Text("Faturamento Total").FontSize(10);
                                        col.Item().Text($"R$ {totalGeral:N2}").FontSize(16).Bold();
                                    });

                                row.ConstantItem(20);

                                row.RelativeItem().Background(Colors.Blue.Lighten4)
                                    .Padding(10).Column(col =>
                                    {
                                        col.Item().Text("Total de Pedidos").FontSize(10);
                                        col.Item().Text($"{totalPedidos}").FontSize(16).Bold();
                                    });

                                row.ConstantItem(20);

                                row.RelativeItem().Background(Colors.Orange.Lighten4)
                                    .Padding(10).Column(col =>
                                    {
                                        col.Item().Text("Total de Garçons").FontSize(10);
                                        col.Item().Text($"{performanceGarcom.Count}").FontSize(16).Bold();
                                    });
                            });

                            column.Item().PaddingVertical(15);

                            // Tabela
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(40);  // Posição
                                    columns.RelativeColumn(2);   // Garçom
                                    columns.ConstantColumn(100); // Qtd Pedidos
                                    columns.ConstantColumn(120); // Faturamento
                                    columns.ConstantColumn(100); // Ticket Médio
                                    columns.ConstantColumn(80);  // % do Total
                                });

                                // Header
                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Teal.Medium)
                                        .Padding(5).Text("Pos.").FontColor(Colors.White).FontSize(10).Bold();
                                    header.Cell().Background(Colors.Teal.Medium)
                                        .Padding(5).Text("Garçom").FontColor(Colors.White).FontSize(10).Bold();
                                    header.Cell().Background(Colors.Teal.Medium)
                                        .Padding(5).Text("Qtd Pedidos").FontColor(Colors.White).FontSize(10).Bold();
                                    header.Cell().Background(Colors.Teal.Medium)
                                        .Padding(5).Text("Faturamento").FontColor(Colors.White).FontSize(10).Bold();
                                    header.Cell().Background(Colors.Teal.Medium)
                                        .Padding(5).Text("Ticket Médio").FontColor(Colors.White).FontSize(10).Bold();
                                    header.Cell().Background(Colors.Teal.Medium)
                                        .Padding(5).Text("% Total").FontColor(Colors.White).FontSize(10).Bold();
                                });

                                // Dados
                                int posicao = 1;
                                foreach (var garcom in performanceGarcom)
                                {
                                    var percentual = (garcom.Faturamento / totalGeral) * 100;
                                    var bgColor = posicao == 1 ? Colors.Blue.Lighten4 : Colors.White;

                                    table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                        .Padding(5).AlignCenter().Text($"{posicao}º").FontSize(9).Bold();
                                    table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                        .Padding(5).Text(garcom.Garcom).FontSize(9).Bold();
                                    table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                        .Padding(5).AlignCenter().Text($"{garcom.QuantidadePedidos}").FontSize(9);
                                    table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                        .Padding(5).AlignRight().Text($"R$ {garcom.Faturamento:N2}").FontSize(9).Bold();
                                    table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                        .Padding(5).AlignRight().Text($"R$ {garcom.TicketMedio:N2}").FontSize(9);
                                    table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
                                        .Padding(5).AlignRight().Text($"{percentual:N1}%").FontSize(9);

                                    posicao++;
                                }
                            });

                            // Legenda
                            column.Item().PaddingTop(15).Background(Colors.Blue.Lighten4)
                                .Padding(5).Text("🏆 1º lugar destacado em dourado")
                                .FontSize(9)
                                .Italic();
                        }
                        else
                        {
                            column.Item().PaddingVertical(20).AlignCenter()
                                .Text("Nenhuma venda realizada no período")
                                .FontSize(12)
                                .Italic()
                                .FontColor(Colors.Grey.Medium);
                        }
                    });

                    // Footer
                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span($"Gerado em {DateTime.Now:dd/MM/yyyy HH:mm}");
                    });
                });
            }).GeneratePdf();
        }

        public async Task<byte[]> GerarRelatorioCustomAsync(DateTime data)
        {
            var pedidos = await _pedidoRepository.GetAllAsync();

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);
                    page.Header().Text("Meu Relatório Custom");
                    page.Content().Text($"Data: {data:dd/MM/yyyy}");
                    page.Footer().Text($"Gerado em {DateTime.Now}");
                });
            }).GeneratePdf();
        }
    }
}
