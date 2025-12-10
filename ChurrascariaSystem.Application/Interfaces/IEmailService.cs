namespace ChurrascariaSystem.Application.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Envia email simples
        /// </summary>
        Task<bool> EnviarEmailAsync(string destinatario, string assunto, string corpo);

        /// <summary>
        /// Envia email em HTML
        /// </summary>
        Task<bool> EnviarEmailHtmlAsync(string destinatario, string assunto, string corpoHtml);

        /// <summary>
        /// Envia alerta de estoque baixo
        /// </summary>
        Task<bool> EnviarAlertaEstoqueBaixoAsync(string produtoNome, int quantidadeAtual, int quantidadeMinima);

        /// <summary>
        /// Envia alerta de estoque esgotado
        /// </summary>
        Task<bool> EnviarAlertaEstoqueEsgotadoAsync(string produtoNome);

        /// <summary>
        /// Envia resumo de múltiplos produtos com estoque baixo
        /// </summary>
        Task<bool> EnviarResumoEstoqueBaixoAsync(List<(string Produto, int Atual, int Minimo)> produtos);
    }
}
