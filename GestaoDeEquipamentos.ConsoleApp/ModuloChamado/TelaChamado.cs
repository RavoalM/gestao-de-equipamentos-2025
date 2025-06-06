﻿using GestaoDeEquipamentos.ConsoleApp.Compartilhado;
using GestaoDeEquipamentos.ConsoleApp.ModuloEquipamento;

namespace GestaoDeEquipamentos.ConsoleApp.ModuloChamado;
public class TelaChamado
{
    public RepositorioEquipamento repositorioEquipamento;
    public RepositorioChamado repositorioChamado;

    public TelaChamado(RepositorioChamado repositorioChamado, RepositorioEquipamento repositorioEquipamento)
    {
        this.repositorioEquipamento = repositorioEquipamento;
        this.repositorioChamado = repositorioChamado;
    }

    public char ApresentarMenu()
    {
        ExibirCabecalho();

        Console.WriteLine("Escolha a operação desejada:");
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("1 - Cadastrar Chamado");
        Console.WriteLine("2 - Editar Chamado");
        Console.WriteLine("3 - Excluir Chamado");
        Console.WriteLine("4 - Visualizar Chamados");

        Console.WriteLine("S - Voltar");

        Console.WriteLine();

        Console.Write("Escolha uma das opções: ");
        char operacaoEscolhida = Convert.ToChar(Console.ReadLine()!);

        return operacaoEscolhida;
    }

    public void CadastrarChamado()
    {
        ExibirCabecalho();

        Console.WriteLine("Cadastrando Chamado...");
        Console.WriteLine("--------------------------------------------");

        Chamado novoChamado = ObterDadosChamado();

        string erros = novoChamado.Validar();

        if (erros.Length > 0)
        {
            Notificador.ExibirMensagem(erros, ConsoleColor.Red);

            CadastrarChamado();
            return;
        }

        repositorioChamado.CadastrarChamado(novoChamado);

        Console.WriteLine();
        Notificador.ExibirMensagem("O chamado foi cadastrado com sucesso!", ConsoleColor.Green);
    }

    public void EditarChamado()
    {
        ExibirCabecalho();

        Console.WriteLine("Editando Chamado...");
        Console.WriteLine("--------------------------------------------");

        VisualizarChamados(false);

        Console.Write("Digite o ID do chamado que deseja editar: ");
        int idChamado = Convert.ToInt32(Console.ReadLine());

        Chamado chamadoEditado = ObterDadosChamado();

        string erros = chamadoEditado.Validar();

        if (erros.Length > 0)
        {
            Notificador.ExibirMensagem(erros, ConsoleColor.Red);

            EditarChamado();
            return;
        }

        bool conseguiuEditar = repositorioChamado.EditarChamado(idChamado, chamadoEditado);

        if (!conseguiuEditar)
        {
            Notificador.ExibirMensagem("Houve um erro durante a edição de um registro...", ConsoleColor.Red);
            return;
        }

        Console.WriteLine();
        Notificador.ExibirMensagem("O chamado foi editado com sucesso!", ConsoleColor.Green);
    }

    public void ExcluirChamado()
    {
        ExibirCabecalho();

        Console.WriteLine("Excluindo Chamado...");
        Console.WriteLine("--------------------------------------------");

        VisualizarChamados(false);

        Console.Write("Digite o ID do chamado que deseja excluir: ");
        int idChamado = Convert.ToInt32(Console.ReadLine());

        bool conseguiuExcluir = repositorioChamado.ExcluirChamado(idChamado);

        if (!conseguiuExcluir)
        {
            Notificador.ExibirMensagem("Houve um erro durante a exclusão de um registro...", ConsoleColor.Red);
            return;
        }

        Console.WriteLine();
        Notificador.ExibirMensagem("O chamado foi excluído com sucesso!", ConsoleColor.Green);
    }

    public void VisualizarChamados(bool exibirTitulo)
    {
        if (exibirTitulo)
        {
            ExibirCabecalho();

            Console.WriteLine("Visualizando Chamados...");
            Console.WriteLine("--------------------------------------------");
        }

        Console.WriteLine();

        Console.WriteLine(
            "{0, -6} | {1, -12} | {2, -15} | {3, -30} | {4, -15} | {5, -15}",
            "Id", "Data de Abertura", "Título", "Descrição", "Equipamento", "Tempo Decorrido"
        );

        Chamado[] chamadosCadastrados = repositorioChamado.SelecionarChamados();

        for (int i = 0; i < chamadosCadastrados.Length; i++)
        {
            Chamado c = chamadosCadastrados[i];

            if (c == null)
                continue;

            string tempoDecorrido = $"{c.ObterTempoDecorrido()} dia(s)";

            Console.WriteLine(
                "{0, -6} | {1, -12} | {2, -15} | {3, -30} | {4, -15} | {5, -15}",
                c.Id, c.DataAbertura.ToShortDateString(), c.Titulo, c.Descricao, c.Equipamento.Nome, tempoDecorrido
            );
        }

        Console.WriteLine();

        Notificador.ExibirMensagem("Pressione ENTER para continuar...", ConsoleColor.DarkYellow);

    }

    public void VisualizarEquipamentos()
    {
        Console.WriteLine("Visualizando Equipamentos...");
        Console.WriteLine("--------------------------------------------");

        Console.WriteLine();

        Console.WriteLine(
            "{0, -10} | {1, -15} | {2, -11} | {3, -15} | {4, -15} | {5, -10}",
            "Id", "Nome", "Num. Série", "Fabricante", "Preço", "Data de Fabricação"
        );

        Equipamento[] equipamentosCadastrados = repositorioEquipamento.SelecionarEquipamentos();

        for (int i = 0; i < equipamentosCadastrados.Length; i++)
        {
            Equipamento e = equipamentosCadastrados[i];

            if (e == null) continue;

            Console.WriteLine(
                "{0, -10} | {1, -15} | {2, -11} | {3, -15} | {4, -15} | {5, -10}",
                e.Id, e.Nome, e.ObterNumeroSerie(), e.Fabricante, e.PrecoAquisicao.ToString("C2"), e.DataFabricacao.ToShortDateString()
            );
        }

        Console.WriteLine();

        Notificador.ExibirMensagem("Pressione ENTER para continuar...", ConsoleColor.DarkYellow);
    }

    public void ExibirCabecalho()
    {
        Console.Clear();
        Console.WriteLine("--------------------------------------------");
        Console.WriteLine("Controle de Chamados");
        Console.WriteLine("--------------------------------------------");
    }

    public Chamado ObterDadosChamado()
    {
        Console.Write("Digite o título do chamado: ");
        string titulo = Console.ReadLine()!.Trim();

        Console.Write("Digite o descrição do chamado: ");
        string descricao = Console.ReadLine()!.Trim();

        VisualizarEquipamentos();

        Console.Write("Digite o ID do equipamento que deseja selecionar: ");
        int idEquipamento = Convert.ToInt32(Console.ReadLine()!.Trim());

        Equipamento equipamentoSelecionado = repositorioEquipamento.SelecionarEquipamentoPorId(idEquipamento);

        Chamado novoChamado = new Chamado(titulo, descricao, equipamentoSelecionado);

        return novoChamado;
    }
}