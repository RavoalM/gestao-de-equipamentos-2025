using GestaoDeEquipamentos.ConsoleApp.ModuloEquipamento;
using GestaoDeEquipamentos.ConsoleApp.ModuloFabricante;
using System.Net.Mail;

namespace GestaoDeEquipamentos.ConsoleApp.ModuloChamado;

public class Chamado
{
    public int Id;
    public string Titulo;
    public string Descricao;
    public Equipamento Equipamento;
    public DateTime DataAbertura;

    public Chamado(string titulo, string descricao, Equipamento equipamento)
    {
        Titulo = titulo;
        Descricao = descricao;
        Equipamento = equipamento;
        DataAbertura = DateTime.Now;
    }

    public string Validar()
    {
        string erros = "";

        if (string.IsNullOrWhiteSpace(Titulo))
        {
            erros += "O campo 'Nome' é obrigatório.\n";
        }

        if (Titulo.Length < 3)
        {
            erros += "O campo 'Nome' deve ter pelo menos 3 caracteres.\n";
        }

        if (string.IsNullOrWhiteSpace(Descricao))
        {
            erros += "O campo 'Nome' é obrigatório.\n";
        }

        if (Equipamento == null)
        {
            erros += "O campo 'Fabricante' é obrigatório.\n";
        }

        if (DataAbertura == null)
        {
            erros += "O campo 'Data de Fabricação' é obrigatório.\n";
        }

        if (DataAbertura > DateTime.Now)
        {
            erros += "A data de fabricação não pode ser futura.\n";
        }

        return erros;
    }

    public int ObterTempoDecorrido()
    {
        TimeSpan diferencaTempo = DateTime.Now.Subtract(DataAbertura);

        return diferencaTempo.Days;
    }
}