namespace ChurrascariaSystem.Domain.ValueObjects
{
    public class Preco
    {
        public decimal Valor { get; private set; }

        public Preco(decimal valor)
        {
            if (valor < 0)
                throw new ArgumentException("O preço não pode ser negativo");

            Valor = valor;
        }

        public static implicit operator decimal(Preco preco) => preco.Valor;
        public static implicit operator Preco(decimal valor) => new Preco(valor);

        public override string ToString() => Valor.ToString("C");
    }
}