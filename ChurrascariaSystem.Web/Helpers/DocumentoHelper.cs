using System.Text.RegularExpressions;

namespace ChurrascariaSystem.Web.Helpers
{
    /// <summary>
    /// Classe para validar CPF
    /// </summary>
    public class DocumentoHelper
    {
        /// <summary>
        /// Remove máscara de CPF (pontos, traços e espaços)
        /// </summary>
        /// <param name="cpf">CPF com ou sem máscara</param>
        /// <returns>CPF apenas com números</returns>
        /// <example>
        /// "111.111.111-11" => "11111111111"
        /// "111 111 111 11" => "11111111111"
        /// "11111111111" => "11111111111"
        /// 
        public static string LimparCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return string.Empty;

            // Remove tudo que não for dígito
            return Regex.Replace(cpf, @"\D", string.Empty);
        }

        public static string LimparTelefone(string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone))
                return string.Empty;

            return Regex.Replace(telefone, @"\D", string.Empty);
        }

        public static string ApenasNumeros(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            return Regex.Replace(texto, @"\D", string.Empty);
        }

        public static bool CpfValido(string cpf)
        {
            var cpfLimpo = LimparCpf(cpf);
            return cpfLimpo.Length == 11;
        }

        public static string FormatarCpf(string cpf)
        {
            var cpfLimpo = LimparCpf(cpf);

            if (cpfLimpo.Length != 11)
                return cpf;

            return Convert.ToUInt64(cpfLimpo).ToString(@"000\.000\.000\-00");
        }
    }
}
