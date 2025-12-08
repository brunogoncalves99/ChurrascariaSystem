using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChurrascariaSystem.Domain.Entities
{
    public class TipoProduto
    {
        public int idTipoProduto { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public bool Ativo { get; set; } = true;

        // Navigation Property
        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }
}
