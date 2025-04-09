using GestaoDeEquipamentos.ConsoleApp.Compartilhado;
using GestaoDeEquipamentos.ConsoleApp.ModuloFabricante;

namespace GestaoDeEquipamentos.ConsoleApp.ModuloEquipamento;

public class TelaEquipamento
{
    public RepositorioFabricante repositorioFabricante;
    public RepositorioEquipamento repositorioEquipamento;

    public TelaEquipamento(RepositorioEquipamento repositorioEquipamento, RepositorioFabricante repositorioFabricante)
    {
        this.repositorioFabricante = repositorioFabricante;
        this.repositorioEquipamento = repositorioEquipamento;
    }

    public char ApresentarMenu()
    {
        ExibirCabecalho();

        Console.WriteLine("Escolha a operação desejada:");
        Console.WriteLine("1 - Cadastro de Equipamento");
        Console.WriteLine("2 - Edição de Equipamento");
        Console.WriteLine("3 - Exclusão de Equipamento");
        Console.WriteLine("4 - Visualização de Equipamentos");
        Console.WriteLine("--------------------------------------------");

        Console.Write("Digite um opção válida: ");
        char opcaoEscolhida = Console.ReadLine()[0];

        return opcaoEscolhida;
    }

    public void CadastrarEquipamento()
    {
        ExibirCabecalho();

        Console.WriteLine("Cadastrando Equipamento...");
        Console.WriteLine("--------------------------------------------");

        Equipamento novoEquipamento = ObterDadosEquipamento();

        string erros = novoEquipamento.Validar();

        if (erros.Length > 0)
        {
            Notificador.ExibirMensagem(erros, ConsoleColor.Red);

            CadastrarEquipamento();
            return;
        }

        Fabricante fabricante = novoEquipamento.Fabricante;

        fabricante.AdicionarEquipamento(novoEquipamento);

        repositorioEquipamento.CadastrarEquipamento(novoEquipamento);

        Console.WriteLine();
        Notificador.ExibirMensagem("O equipamento foi cadastrado com sucesso!", ConsoleColor.Red);
    }

    public void EditarEquipamento()
    {
        ExibirCabecalho();

        Console.WriteLine("Editando Equipamento...");
        Console.WriteLine("--------------------------------------------");

        VisualizarEquipamentos(false);

        Console.Write("Digite o ID do registro que deseja selecionar: ");
        int idSelecionado = Convert.ToInt32(Console.ReadLine());

        Equipamento equipamentoAntigo = repositorioEquipamento.SelecionarEquipamentoPorId(idSelecionado);
        Fabricante fabricanteAntigo = equipamentoAntigo.Fabricante;

        Console.WriteLine();

        Equipamento equipamentoEditado = ObterDadosEquipamento();

        string erros = equipamentoEditado.Validar();

        if (erros.Length > 0)
        {
            Notificador.ExibirMensagem(erros, ConsoleColor.Red);

            EditarEquipamento();
            return;
        }

        Fabricante fabricanteEditado = equipamentoEditado.Fabricante;

        bool conseguiuEditar = repositorioEquipamento.EditarEquipamento(idSelecionado, equipamentoEditado);

        if (!conseguiuEditar)
        {
            Notificador.ExibirMensagem("Houve um erro durante a edição de um registro...", ConsoleColor.Red);
            return;
        }

        if (fabricanteAntigo != fabricanteEditado)
        {
            fabricanteAntigo.RemoverEquipamento(equipamentoAntigo);
            fabricanteEditado.AdicionarEquipamento(equipamentoEditado);
        }

        Notificador.ExibirMensagem("O equipamento foi editado com sucesso!", ConsoleColor.Green);
    }

    public void ExcluirEquipamento()
    {
        ExibirCabecalho();

        Console.WriteLine("Excluindo Equipamento...");
        Console.WriteLine("--------------------------------------------");

        VisualizarEquipamentos(false);

        Console.Write("Digite o ID do registro que deseja selecionar: ");
        int idSelecionado = Convert.ToInt32(Console.ReadLine());

        Equipamento equipamentoSelecionado = repositorioEquipamento.SelecionarEquipamentoPorId(idSelecionado);

        bool conseguiuExcluir = repositorioEquipamento.ExcluirEquipamento(idSelecionado);

        if (!conseguiuExcluir)
        {
            Console.WriteLine("Houve um erro durante a exclusão de um registro...");
            return;
        }

        Fabricante fabricanteSelecionado = equipamentoSelecionado.Fabricante;
        fabricanteSelecionado.RemoverEquipamento(equipamentoSelecionado);

        Notificador.ExibirMensagem("O equipamento foi excluído com sucesso!", ConsoleColor.Green);
    }

    public void VisualizarEquipamentos(bool exibirTitulo)
    {
        if (exibirTitulo)
        {
            ExibirCabecalho();

            Console.WriteLine("Visualizando Equipamentos...");
            Console.WriteLine("--------------------------------------------");
        }

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
                e.Id, e.Nome, e.ObterNumeroSerie(), e.Fabricante.Nome, e.PrecoAquisicao.ToString("C2"), e.DataFabricacao.ToShortDateString()
            );
        }

        Console.WriteLine();

        Notificador.ExibirMensagem("Pressione ENTER para continuar...", ConsoleColor.DarkYellow);
    }

    public void ExibirCabecalho()
    {
        Console.Clear();
        Console.WriteLine("--------------------------------------------");
        Console.WriteLine("Gestão de Equipamentos");
        Console.WriteLine("--------------------------------------------");
    }

    public void VisualizarFabricantes()
    {
        Console.WriteLine("Visualizando Equipamentos...");
        Console.WriteLine("--------------------------------------------");

        Console.WriteLine();

        Console.WriteLine(
          "{0, -10} | {1, -15} | {2, -21} | {3, -15} | {4, -15}",
            "Id", "Nome", "Email", "Telefone", "Qtd Equipamentos"
        );

        Fabricante[] fabricantesCadastrados = repositorioFabricante.SelecionarFabricantes();

        for (int i = 0; i < fabricantesCadastrados.Length; i++)
        {
            Fabricante f = fabricantesCadastrados[i];

            if (f == null) continue;

            Console.WriteLine(
                 "{0, -10} | {1, -15} | {2, -21} | {3, -15} | {4, -15}",
                f.Id, f.Nome, f.Email, f.Telefone, f.ObterQuantidadeEquipamentos()
            );
        }

        Console.WriteLine();

        Notificador.ExibirMensagem("Pressione ENTER para continuar...", ConsoleColor.DarkYellow);
    }

    public Equipamento ObterDadosEquipamento()
    {
        Console.Write("Digite o nome do equipamento: ");
        string nome = Console.ReadLine();

        Console.Write("Digite o preço de aquisição R$ ");
        decimal precoAquisicao = Convert.ToDecimal(Console.ReadLine());

        Console.Write("Digite a data de fabricação do equipamento (dd/MM/yyyy) ");
        DateTime dataFabricacao = Convert.ToDateTime(Console.ReadLine());
        VisualizarFabricantes();

        Console.Write("Digite o ID do fabricante que deseja selecionar: ");
        int idFabricante = Convert.ToInt32(Console.ReadLine()!.Trim());

        Fabricante fabricanteSelecionado = repositorioFabricante.SelecionarFabricantePorId(idFabricante);

        Equipamento novoEquipamento = new Equipamento(nome, fabricanteSelecionado, precoAquisicao, dataFabricacao);

        return novoEquipamento;
    }
}
