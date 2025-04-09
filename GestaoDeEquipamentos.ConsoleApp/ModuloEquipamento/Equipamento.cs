using GestaoDeEquipamentos.ConsoleApp.ModuloFabricante;
using System.Drawing;
using System.Globalization;

namespace GestaoDeEquipamentos.ConsoleApp.ModuloEquipamento;

public class Equipamento
{
    public int Id;
    public string Nome;
    public Fabricante Fabricante;
    public decimal PrecoAquisicao;
    public DateTime DataFabricacao;

    public Equipamento(string nome, Fabricante fabricante, decimal precoAquisicao, DateTime dataFabricacao)
    {
        Nome = nome;
        Fabricante = fabricante;
        PrecoAquisicao = precoAquisicao;
        DataFabricacao = dataFabricacao;
    }

    public string Validar()
    {
        string erros = "";

        if (string.IsNullOrWhiteSpace(Nome))
        {
            erros += "O campo 'Nome' é obrigatório.\n";
        }

        if (Nome.Length < 3)
        {
            erros += "O campo 'Nome' deve ter pelo menos 3 caracteres.\n";
        }

        if (Fabricante == null)
        {
            erros += "O campo 'Fabricante' é obrigatório.\n";
        }

        if (PrecoAquisicao <= 0)
        {
            erros += "O campo 'Preço de Aquisição' deve ser maior que zero.\n";
        }

        if (DataFabricacao == null)
        {
            erros += "O campo 'Data de Fabricação' é obrigatório.\n";
        }

        if (DataFabricacao > DateTime.Now)
        {
            erros += "A data de fabricação não pode ser futura.\n";
        }

        return erros;
    }

    public string ObterNumeroSerie()
    {
        string tresPrimeirosCaracteres = Nome.Substring(0, 3).ToUpper();

        return $"{tresPrimeirosCaracteres}-{Id}";
    }
}
