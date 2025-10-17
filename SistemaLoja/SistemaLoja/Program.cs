using System;
using SistemaLoja.Lab12_ConexaoSQLServer;

namespace SistemaLoja
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== SISTEMA DE LOJA - SQL SERVER ===\n");

            var produtoRepo = new ProdutoRepository();
            var pedidoRepo = new PedidoRepository();

            bool continuar = true;
            while (continuar)
            {
                MostrarMenu();
                string opcao = Console.ReadLine() ?? "";

                try
                {
                    switch (opcao)
                    {
                        case "1":
                            produtoRepo.ListarTodosProdutos();
                            break;

                        case "2":
                            InserirNovoProduto(produtoRepo);
                            break;

                        case "3":
                            AtualizarProdutoExistente(produtoRepo);
                            break;

                        case "4":
                            DeletarProdutoExistente(produtoRepo);
                            break;

                        case "5":
                            ListarPorCategoria(produtoRepo);
                            break;

                        case "6":
                            CriarNovoPedido(pedidoRepo);
                            break;

                        case "7":
                            ListarPedidosDeCliente(pedidoRepo);
                            break;

                        case "8":
                            DetalhesDoPedido(pedidoRepo);
                            break;

                        case "9":
                            BuscarProdutosPorNome(produtoRepo);
                            break;

                        case "10":
                            EstoqueBaixo(produtoRepo);
                            break;

                        case "11":
                            TotalVendasPorPeriodo(pedidoRepo);
                            break;


                        case "0":
                            continuar = false;
                            break;

                        default:
                            Console.WriteLine("⚠️ Opção inválida!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n❌ Erro: {ex.Message}");
                }

                if (continuar)
                {
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            Console.WriteLine("\nPrograma finalizado!");
        }

        static void MostrarMenu()
        {
            Console.WriteLine("\n╔════════════════════════════════════╗");
            Console.WriteLine("║        MENU PRINCIPAL              ║");
            Console.WriteLine("╠════════════════════════════════════╣");
            Console.WriteLine("║ 1 - Listar produtos                ║");
            Console.WriteLine("║ 2 - Inserir produto                ║");
            Console.WriteLine("║ 3 - Atualizar produto              ║");
            Console.WriteLine("║ 4 - Deletar produto                ║");
            Console.WriteLine("║ 5 - Listar produtos por categoria  ║");
            Console.WriteLine("║ 6 - Criar novo pedido              ║");
            Console.WriteLine("║ 7 - Listar pedidos de cliente      ║");
            Console.WriteLine("║ 8 - Detalhes de pedido             ║");
            Console.WriteLine("║ 9 - Buscar produto por nome        ║");
            Console.WriteLine("║ 10 - Produtos com estoque baixo    ║");
            Console.WriteLine("║ 11 - Total de vendas por período   ║");
            Console.WriteLine("║ 0 - Sair                           ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            Console.Write("\nEscolha uma opção: ");
        }

        static void InserirNovoProduto(ProdutoRepository repo)
        {
            Console.WriteLine("\n=== INSERIR NOVO PRODUTO ===");
            Console.Write("Nome: ");
            string nome = Console.ReadLine() ?? "";
            Console.Write("Preço: ");
            decimal preco = decimal.Parse(Console.ReadLine() ?? "0");
            Console.Write("Estoque: ");
            int estoque = int.Parse(Console.ReadLine() ?? "0");
            Console.Write("CategoriaId: ");
            int categoriaId = int.Parse(Console.ReadLine() ?? "0");

            var produto = new Produto
            {
                Nome = nome,
                Preco = preco,
                Estoque = estoque,
                CategoriaId = categoriaId
            };
            repo.InserirProduto(produto);
        }

        static void AtualizarProdutoExistente(ProdutoRepository repo)
        {
            Console.WriteLine("\n=== ATUALIZAR PRODUTO ===");
            Console.Write("ID do produto: ");
            int id = int.Parse(Console.ReadLine() ?? "0");
            Console.Write("Novo nome: ");
            string nome = Console.ReadLine() ?? "";
            Console.Write("Novo preço: ");
            decimal preco = decimal.Parse(Console.ReadLine() ?? "0");
            Console.Write("Novo estoque: ");
            int estoque = int.Parse(Console.ReadLine() ?? "0");
            Console.Write("Nova CategoriaId: ");
            int categoriaId = int.Parse(Console.ReadLine() ?? "0");

            var produto = new Produto
            {
                Id = id,
                Nome = nome,
                Preco = preco,
                Estoque = estoque,
                CategoriaId = categoriaId
            };
            repo.AtualizarProduto(produto);
        }

        static void DeletarProdutoExistente(ProdutoRepository repo)
        {
            Console.WriteLine("\n=== DELETAR PRODUTO ===");
            Console.Write("ID do produto: ");
            int id = int.Parse(Console.ReadLine() ?? "0");
            Console.Write("Tem certeza que deseja excluir (S/N)? ");
            string confirmacao = Console.ReadLine() ?? "";
            if (confirmacao.ToUpper() == "S")
                repo.DeletarProduto(id);
            else
                Console.WriteLine("❎ Operação cancelada.");
        }

        static void ListarPorCategoria(ProdutoRepository repo)
        {
            Console.WriteLine("\n=== PRODUTOS POR CATEGORIA ===");
            Console.Write("ID da categoria: ");
            int cat = int.Parse(Console.ReadLine() ?? "0");
            repo.ListarPorCategoria(cat);
        }
        static void BuscarProdutosPorNome(ProdutoRepository repo)
        {
            Console.WriteLine("\n=== BUSCAR PRODUTO POR NOME ===");
            Console.Write("Digite parte do nome: ");
            string nome = Console.ReadLine() ?? "";
            repo.BuscarProdutosPorNome(nome);
        }

        static void EstoqueBaixo(ProdutoRepository repo)
        {
            Console.WriteLine("\n=== PRODUTOS COM ESTOQUE BAIXO ===");
            Console.Write("Quantidade mínima: ");
            int min = int.Parse(Console.ReadLine() ?? "30");
            repo.ListarProdutosEstoqueBaixo(min);
        }

        static void CriarNovoPedido(PedidoRepository repo)
        {
            Console.WriteLine("\n=== CRIAR NOVO PEDIDO ===");

            Console.Write("ID do cliente: ");
            int clienteId = int.Parse(Console.ReadLine() ?? "0");

            Pedido pedido = new Pedido()
            {
                ClienteId = clienteId,
            };
            List<PedidoItem> itens = new List<PedidoItem>();

            while (true)
            {
                Console.Write("Produto ID (0 = continuar): ");
                int produtoID = int.Parse(Console.ReadLine() ?? string.Empty);
                if (produtoID == 0)
                {
                    break;
                }
                Console.Write("Quantidade: ");
                int produtoQnt = int.Parse(Console.ReadLine() ?? string.Empty);

                if (produtoQnt != 0)
                {
                    itens.Add(new PedidoItem()
                    {
                        ProdutoId = produtoID,
                        Quantidade = produtoQnt
                    });
                }
                else
                {
                    Console.WriteLine("A quantidade de produtos nao pode ser 0");
                }
            }

            if (itens.Count > 0)
            {
                repo.CriarPedido(pedido, itens);
                return;
            }
            Console.WriteLine("Não é possivel criar um pedido sem produtos");
        }

        static void ListarPedidosDeCliente(PedidoRepository repo)
        {
            Console.WriteLine("\n=== PEDIDOS DO CLIENTE ===");
            Console.Write("ID do cliente: ");
            int clienteId = int.Parse(Console.ReadLine() ?? "0");
            repo.ListarPedidosDeCliente(clienteId);
        }

        static void DetalhesDoPedido(PedidoRepository repo)
        {
            Console.WriteLine("\n=== DETALHES DO PEDIDO ===");
            Console.Write("ID do pedido: ");
            int pedidoId = int.Parse(Console.ReadLine() ?? "0");
            repo.DetalhesDoPedido(pedidoId);
        }
        static void TotalVendasPorPeriodo(PedidoRepository repo)
        {
            Console.WriteLine("\n=== TOTAL DE VENDAS POR PERÍODO ===");
            Console.Write("Data início (AAAA-MM-DD): ");
            DateTime inicio = DateTime.Parse(Console.ReadLine() ?? DateTime.Now.ToString());
            Console.Write("Data fim (AAAA-MM-DD): ");
            DateTime fim = DateTime.Parse(Console.ReadLine() ?? DateTime.Now.ToString());
            repo.TotalVendasPeriodo(inicio, fim);
        }
    }
}
