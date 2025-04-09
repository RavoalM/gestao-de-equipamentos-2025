using GestaoDeEquipamentos.ConsoleApp.Compartilhado;

namespace GestaoDeEquipamentos.ConsoleApp.ModuloFabricante;

public class TelaFabricante
{
    public RepositorioFabricante repositorioFabricante;

    public TelaFabricante(RepositorioFabricante repositorioFabricante)
    {
        this.repositorioFabricante = new RepositorioFabricante();
    }

    public char ApresentarMenu()
    {
        ExibirCabecalho();

        Console.WriteLine("Escolha a operação desejada:");
        Console.WriteLine("1 - Cadastro de Fabricantes");
        Console.WriteLine("2 - Edição de Fabricantes");
        Console.WriteLine("3 - Exclusão de Fabricantes");
        Console.WriteLine("4 - Visualização de Fabricantes");
        Console.WriteLine("--------------------------------------------");

        Console.Write("Digite um opção válida: ");
        char opcaoEscolhida = Console.ReadLine()[0];

        return opcaoEscolhida;
    }

    public void CadastrarFabricante()
    {
        ExibirCabecalho();

        Console.WriteLine("Cadastrando Fabricante...");
        Console.WriteLine("--------------------------------------------");

        Fabricante novoFabricante = ObterDadosFabricante();

        string erros = novoFabricante.Validar();

        if (erros.Length > 0)
        {
            Notificador.ExibirMensagem(erros, ConsoleColor.Red);

            CadastrarFabricante();
            return;
        }

        repositorioFabricante.CadastrarFabricante(novoFabricante);

        Console.WriteLine();
        Notificador.ExibirMensagem("O fabricante foi cadastrado com sucesso!", ConsoleColor.Green);
    }

    public void EditarFabricante()
    {
        ExibirCabecalho();

        Console.WriteLine("Editando Fabricante...");
        Console.WriteLine("--------------------------------------------");

        VisualizarFabricantes(false);

        Console.Write("Digite o ID do fabricante que deseja selecionar: ");
        int idSelecionado = Convert.ToInt32(Console.ReadLine());

        Fabricante fabricanteEditado = ObterDadosFabricante();

        string erros = fabricanteEditado.Validar();

        if (erros.Length > 0)
        {
            Notificador.ExibirMensagem(erros, ConsoleColor.Red);

            EditarFabricante();
            return;
        }

        bool conseguiuEditar = repositorioFabricante.EditarFabricante(idSelecionado, fabricanteEditado);

        Console.WriteLine();
        Notificador.ExibirMensagem("O Fabricante foi editado com sucesso!", ConsoleColor.Green);
    }

    public void ExcluirFabricante()
    {
        ExibirCabecalho();

        Console.WriteLine("Excluindo Fabricante...");
        Console.WriteLine("--------------------------------------------");

        VisualizarFabricantes(false);

        Console.Write("Digite o ID do registro que deseja selecionar: ");
        int idSelecionado = Convert.ToInt32(Console.ReadLine());

        bool conseguiuExcluir = repositorioFabricante.ExcluirFabricante(idSelecionado);

        if (!conseguiuExcluir)
        {
            Notificador.ExibirMensagem("Houve um erro durante a exclusão de um registro...", ConsoleColor.Red);
            return;
        }

        Console.WriteLine();
        Notificador.ExibirMensagem("O fabricante foi excluído com sucesso!", ConsoleColor.Green);
    }

    public void VisualizarFabricantes(bool exibirTitulo)
    {
        if (exibirTitulo)
        {
            ExibirCabecalho();

            Console.WriteLine("Visualizando Fabricantes...");
            Console.WriteLine("--------------------------------------------");
        }

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

    public void ExibirCabecalho()
    {
        Console.Clear();
        Console.WriteLine("--------------------------------------------");
        Console.WriteLine("Gestão de Fabricantes");
        Console.WriteLine("--------------------------------------------");
    }

    public Fabricante ObterDadosFabricante()
    {
        Console.Write("Digite o nome do fabricante: ");
        string nome = Console.ReadLine()!.Trim();

        Console.Write("Digite o email do fabricante: ");
        string email = Console.ReadLine()!.Trim();

        Console.Write("Digite o telefone do fabricante ");
        string telefone = Console.ReadLine()!.Trim();

        Fabricante novoFabricante = new Fabricante(nome, email, telefone);

        return novoFabricante;
    }
}
