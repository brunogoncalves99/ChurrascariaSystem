using ChurrascariaSystem.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ChurrascariaSystem.Application.Services
{

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _emailRemetente;
        private readonly string _nomeRemetente;
        private readonly string _emailDestinatario;
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUsuario;
        private readonly string _smtpSenha;
        private readonly bool _habilitarSsl;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;

            _emailRemetente = _configuration["EmailSettings:EmailRemetente"] ?? "";
            _nomeRemetente = _configuration["EmailSettings:NomeRemetente"] ?? "Sistema Churrascaria";
            _emailDestinatario = _configuration["EmailSettings:EmailDestinatario"] ?? "";
            _smtpHost = _configuration["EmailSettings:SmtpHost"] ?? "";
            _smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
            _smtpUsuario = _configuration["EmailSettings:SmtpUsuario"] ?? "";
            _smtpSenha = _configuration["EmailSettings:SmtpSenha"] ?? "";
            _habilitarSsl = bool.Parse(_configuration["EmailSettings:HabilitarSsl"] ?? "true");
        }

        /// <summary>
        /// Envia email simples (texto)
        /// </summary>
        public async Task<bool> EnviarEmailAsync(string destinatario, string assunto, string corpo)
        {
            try
            {
                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(_emailRemetente, _nomeRemetente);
                    message.To.Add(new MailAddress(destinatario));
                    message.Subject = assunto;
                    message.Body = corpo;
                    message.IsBodyHtml = false;

                    return await EnviarAsync(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar email: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Envia email em HTML
        /// </summary>
        public async Task<bool> EnviarEmailHtmlAsync(string destinatario, string assunto, string corpoHtml)
        {
            try
            {
                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(_emailRemetente, _nomeRemetente);
                    message.To.Add(new MailAddress(destinatario));
                    message.Subject = assunto;
                    message.Body = corpoHtml;
                    message.IsBodyHtml = true;

                    return await EnviarAsync(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar email HTML: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Envia alerta de estoque baixo (produto individual)
        /// </summary>
        public async Task<bool> EnviarAlertaEstoqueBaixoAsync(string produtoNome, int quantidadeAtual, int quantidadeMinima)
        {
            var assunto = $"⚠️ ALERTA: Estoque Baixo - {produtoNome}";
            var corpoHtml = GerarHtmlEstoqueBaixo(produtoNome, quantidadeAtual, quantidadeMinima);

            return await EnviarEmailHtmlAsync(_emailDestinatario, assunto, corpoHtml);
        }

        /// <summary>
        /// Envia alerta de estoque esgotado
        /// </summary>
        public async Task<bool> EnviarAlertaEstoqueEsgotadoAsync(string produtoNome)
        {
            var assunto = $"🔴 URGENTE: Estoque Esgotado - {produtoNome}";
            var corpoHtml = GerarHtmlEstoqueEsgotado(produtoNome);

            return await EnviarEmailHtmlAsync(_emailDestinatario, assunto, corpoHtml);
        }

        /// <summary>
        /// Envia resumo de múltiplos produtos com estoque baixo
        /// </summary>
        public async Task<bool> EnviarResumoEstoqueBaixoAsync(List<(string Produto, int Atual, int Minimo)> produtos)
        {
            var assunto = $"📊 Relatório de Estoque - {produtos.Count} produto(s) com alerta";
            var corpoHtml = GerarHtmlResumoEstoque(produtos);

            return await EnviarEmailHtmlAsync(_emailDestinatario, assunto, corpoHtml);
        }

        /// <summary>
        /// Método privado para enviar email via SMTP
        /// </summary>
        private async Task<bool> EnviarAsync(MailMessage message)
        {
            try
            {
                using (var smtpClient = new SmtpClient(_smtpHost, _smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(_smtpUsuario, _smtpSenha);
                    smtpClient.EnableSsl = _habilitarSsl;

                    await smtpClient.SendMailAsync(message);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro no SMTP: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gera HTML para alerta de estoque baixo
        /// </summary>
        private string GerarHtmlEstoqueBaixo(string produtoNome, int quantidadeAtual, int quantidadeMinima)
        {
            var html = new StringBuilder();
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine("    <meta charset='utf-8'>");
            html.AppendLine("    <style>");
            html.AppendLine("        body { font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 20px; }");
            html.AppendLine("        .container { max-width: 600px; margin: 0 auto; background-color: white; border-radius: 10px; overflow: hidden; box-shadow: 0 4px 6px rgba(0,0,0,0.1); }");
            html.AppendLine("        .header { background-color: #ff9800; color: white; padding: 30px; text-align: center; }");
            html.AppendLine("        .header h1 { margin: 0; font-size: 24px; }");
            html.AppendLine("        .content { padding: 30px; }");
            html.AppendLine("        .alert-box { background-color: #fff3cd; border-left: 4px solid #ff9800; padding: 15px; margin: 20px 0; }");
            html.AppendLine("        .info-table { width: 100%; border-collapse: collapse; margin: 20px 0; }");
            html.AppendLine("        .info-table td { padding: 10px; border-bottom: 1px solid #ddd; }");
            html.AppendLine("        .info-table td:first-child { font-weight: bold; width: 40%; }");
            html.AppendLine("        .footer { background-color: #f8f9fa; padding: 20px; text-align: center; color: #666; font-size: 12px; }");
            html.AppendLine("        .btn { display: inline-block; background-color: #28a745; color: white; padding: 12px 30px; text-decoration: none; border-radius: 5px; margin-top: 20px; }");
            html.AppendLine("    </style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");
            html.AppendLine("    <div class='container'>");
            html.AppendLine("        <div class='header'>");
            html.AppendLine("            <h1>⚠️ Alerta de Estoque Baixo</h1>");
            html.AppendLine("        </div>");
            html.AppendLine("        <div class='content'>");
            html.AppendLine("            <h2>Atenção Necessária!</h2>");
            html.AppendLine("            <div class='alert-box'>");
            html.AppendLine($"                <strong>{produtoNome}</strong> está com estoque abaixo do mínimo recomendado.");
            html.AppendLine("            </div>");
            html.AppendLine("            <table class='info-table'>");
            html.AppendLine($"                <tr><td>Produto:</td><td><strong>{produtoNome}</strong></td></tr>");
            html.AppendLine($"                <tr><td>Quantidade Atual:</td><td><strong style='color: #ff9800;'>{quantidadeAtual}</strong></td></tr>");
            html.AppendLine($"                <tr><td>Quantidade Mínima:</td><td>{quantidadeMinima}</td></tr>");
            html.AppendLine($"                <tr><td>Status:</td><td><span style='color: #ff9800; font-weight: bold;'>⚠️ ESTOQUE BAIXO</span></td></tr>");
            html.AppendLine("            </table>");
            html.AppendLine("            <p><strong>Ação Recomendada:</strong> Realizar reposição do estoque o mais breve possível para evitar falta no atendimento.</p>");
            html.AppendLine("        </div>");
            html.AppendLine("        <div class='footer'>");
            html.AppendLine($"            <p>Sistema de Gestão Churrascaria</p>");
            html.AppendLine($"            <p>Email enviado em {DateTime.Now:dd/MM/yyyy HH:mm}</p>");
            html.AppendLine("        </div>");
            html.AppendLine("    </div>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");

            return html.ToString();
        }

        /// <summary>
        /// Gera HTML para alerta de estoque esgotado
        /// </summary>
        private string GerarHtmlEstoqueEsgotado(string produtoNome)
        {
            var html = new StringBuilder();
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine("    <meta charset='utf-8'>");
            html.AppendLine("    <style>");
            html.AppendLine("        body { font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 20px; }");
            html.AppendLine("        .container { max-width: 600px; margin: 0 auto; background-color: white; border-radius: 10px; overflow: hidden; box-shadow: 0 4px 6px rgba(0,0,0,0.1); }");
            html.AppendLine("        .header { background-color: #dc3545; color: white; padding: 30px; text-align: center; }");
            html.AppendLine("        .header h1 { margin: 0; font-size: 24px; }");
            html.AppendLine("        .content { padding: 30px; }");
            html.AppendLine("        .alert-box { background-color: #f8d7da; border-left: 4px solid #dc3545; padding: 15px; margin: 20px 0; }");
            html.AppendLine("        .footer { background-color: #f8f9fa; padding: 20px; text-align: center; color: #666; font-size: 12px; }");
            html.AppendLine("    </style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");
            html.AppendLine("    <div class='container'>");
            html.AppendLine("        <div class='header'>");
            html.AppendLine("            <h1>🔴 URGENTE: Estoque Esgotado</h1>");
            html.AppendLine("        </div>");
            html.AppendLine("        <div class='content'>");
            html.AppendLine("            <h2>Ação Imediata Necessária!</h2>");
            html.AppendLine("            <div class='alert-box'>");
            html.AppendLine($"                <strong>{produtoNome}</strong> está com estoque ZERADO!");
            html.AppendLine("            </div>");
            html.AppendLine($"            <p><strong>Produto:</strong> {produtoNome}</p>");
            html.AppendLine($"            <p><strong>Quantidade Atual:</strong> <span style='color: #dc3545; font-weight: bold; font-size: 18px;'>0 (ZERO)</span></p>");
            html.AppendLine("            <p><strong>Ação Urgente:</strong> Realizar compra imediatamente para evitar interrupção no atendimento!</p>");
            html.AppendLine("        </div>");
            html.AppendLine("        <div class='footer'>");
            html.AppendLine($"            <p>Sistema de Gestão Churrascaria</p>");
            html.AppendLine($"            <p>Email enviado em {DateTime.Now:dd/MM/yyyy HH:mm}</p>");
            html.AppendLine("        </div>");
            html.AppendLine("    </div>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");

            return html.ToString();
        }

        /// <summary>
        /// Gera HTML para resumo de múltiplos produtos
        /// </summary>
        private string GerarHtmlResumoEstoque(List<(string Produto, int Atual, int Minimo)> produtos)
        {
            var html = new StringBuilder();
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine("    <meta charset='utf-8'>");
            html.AppendLine("    <style>");
            html.AppendLine("        body { font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 20px; }");
            html.AppendLine("        .container { max-width: 600px; margin: 0 auto; background-color: white; border-radius: 10px; overflow: hidden; box-shadow: 0 4px 6px rgba(0,0,0,0.1); }");
            html.AppendLine("        .header { background-color: #007bff; color: white; padding: 30px; text-align: center; }");
            html.AppendLine("        .header h1 { margin: 0; font-size: 24px; }");
            html.AppendLine("        .content { padding: 30px; }");
            html.AppendLine("        .produtos-table { width: 100%; border-collapse: collapse; margin: 20px 0; }");
            html.AppendLine("        .produtos-table th { background-color: #007bff; color: white; padding: 12px; text-align: left; }");
            html.AppendLine("        .produtos-table td { padding: 10px; border-bottom: 1px solid #ddd; }");
            html.AppendLine("        .esgotado { background-color: #f8d7da; }");
            html.AppendLine("        .baixo { background-color: #fff3cd; }");
            html.AppendLine("        .footer { background-color: #f8f9fa; padding: 20px; text-align: center; color: #666; font-size: 12px; }");
            html.AppendLine("    </style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");
            html.AppendLine("    <div class='container'>");
            html.AppendLine("        <div class='header'>");
            html.AppendLine("            <h1>📊 Relatório de Estoque</h1>");
            html.AppendLine("        </div>");
            html.AppendLine("        <div class='content'>");
            html.AppendLine($"            <h2>Alerta: {produtos.Count} Produto(s) Necessitam Atenção</h2>");
            html.AppendLine("            <table class='produtos-table'>");
            html.AppendLine("                <thead>");
            html.AppendLine("                    <tr>");
            html.AppendLine("                        <th>Produto</th>");
            html.AppendLine("                        <th>Atual</th>");
            html.AppendLine("                        <th>Mínimo</th>");
            html.AppendLine("                        <th>Status</th>");
            html.AppendLine("                    </tr>");
            html.AppendLine("                </thead>");
            html.AppendLine("                <tbody>");

            foreach (var produto in produtos.OrderBy(p => p.Atual))
            {
                var cssClass = produto.Atual == 0 ? "esgotado" : "baixo";
                var status = produto.Atual == 0 ? "🔴 ESGOTADO" : "⚠️ BAIXO";

                html.AppendLine($"                    <tr class='{cssClass}'>");
                html.AppendLine($"                        <td><strong>{produto.Produto}</strong></td>");
                html.AppendLine($"                        <td><strong>{produto.Atual}</strong></td>");
                html.AppendLine($"                        <td>{produto.Minimo}</td>");
                html.AppendLine($"                        <td>{status}</td>");
                html.AppendLine("                    </tr>");
            }

            html.AppendLine("                </tbody>");
            html.AppendLine("            </table>");
            html.AppendLine("            <p><strong>Ação Recomendada:</strong> Realizar reposição dos produtos listados acima.</p>");
            html.AppendLine("        </div>");
            html.AppendLine("        <div class='footer'>");
            html.AppendLine($"            <p>Sistema de Gestão Churrascaria</p>");
            html.AppendLine($"            <p>Email enviado em {DateTime.Now:dd/MM/yyyy HH:mm}</p>");
            html.AppendLine("        </div>");
            html.AppendLine("    </div>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");

            return html.ToString();
        }
    }
}
