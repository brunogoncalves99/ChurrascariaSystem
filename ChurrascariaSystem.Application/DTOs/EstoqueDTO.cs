using System.ComponentModel.DataAnnotations;

namespace ChurrascariaSystem.Application.DTOs
{
    public class EstoqueDTO
    {
        public int idEstoque { get; set; }

        [Required(ErrorMessage = "Produto é obrigatório")]
        public int idProduto { get; set; }

        public string? NomeProduto { get; set; }

        [Required(ErrorMessage = "Quantidade atual é obrigatória")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantidade deve ser maior ou igual a zero")]
        public int QuantidadeAtual { get; set; }

        [Required(ErrorMessage = "Quantidade mínima é obrigatória")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantidade mínima deve ser maior ou igual a zero")]
        public int QuantidadeMinima { get; set; }

        public DateTime UltimaAtualizacao { get; set; }
        public bool EstoqueBaixo { get; set; }
        public bool EstoqueEsgotado { get; set; }
    }

    public class MovimentacaoEstoqueDTO
    {
        public int idMovimentacao { get; set; }

        [Required(ErrorMessage = "Produto é obrigatório")]
        public int idProduto { get; set; }

        public string? NomeProduto { get; set; }

        [Required(ErrorMessage = "Tipo de movimentação é obrigatório")]
        public string TipoMovimentacao { get; set; } = string.Empty;

        [Required(ErrorMessage = "Quantidade é obrigatória")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser maior que zero")]
        public int Quantidade { get; set; }
        public int QuantidadeAnterior { get; set; }
        public int QuantidadeNova { get; set; }

        [Required(ErrorMessage = "Motivo é obrigatório")]
        public string Motivo { get; set; } = string.Empty; 

        public int? idPedido { get; set; }
        public int? idUsuario { get; set; }
        public string? UsuarioNome { get; set; }
        public DateTime DataMovimentacao { get; set; } = DateTime.Now;

        [StringLength(500, ErrorMessage = "Observação deve ter no máximo 500 caracteres")]
        public string? Observacao { get; set; }
    }

    public class EntradaEstoqueDTO
    {
        [Required(ErrorMessage = "Produto é obrigatório")]
        public int idProduto { get; set; }

        [Required(ErrorMessage = "Quantidade é obrigatória")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser maior que zero")]
        public int Quantidade { get; set; }

        [StringLength(500, ErrorMessage = "Observação deve ter no máximo 500 caracteres")]
        public string? Observacao { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }

}
