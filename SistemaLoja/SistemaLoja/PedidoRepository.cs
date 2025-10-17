using System;
using Microsoft.Data.SqlClient;
using SistemaLoja.Lab12_ConexaoSQLServer;

namespace SistemaLoja
{
    public class PedidoRepository
    {
        public void CriarPedido(Pedido pedido, List<PedidoItem> itens)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                // TODO: Inicie a transação
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Get products info
                    string sqlProdutos = "SELECT Id, Estoque, Preco FROM Produtos WHERE Id IN ";

                    List<Produto> produtos = new List<Produto>();

                    if (itens.Count == 0)
                        throw new Exception("Nenhum item para processar no pedido.");


                    List<string> param = new List<string>();
                    for (int i = 0; i < itens.Count; i++)
                    {
                        param.Add($"@Id{i}");
                    }

                    sqlProdutos += "(" + string.Join(", ", param) + ")";

                    using (SqlCommand cmd = new SqlCommand(sqlProdutos, conn, transaction))
                    {
                        for (int i = 0; i < itens.Count; i++)
                        {
                            cmd.Parameters.AddWithValue($"@Id{i}", itens[i].ProdutoId);
                        }

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                produtos.Add(new Produto()
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Preco = Convert.ToDecimal(reader["Preco"]),
                                    Estoque = Convert.ToInt32(reader["Estoque"])
                                });
                            }
                        }
                    }

                    // Process PedidoItems

                    foreach (PedidoItem item in itens)
                    {
                        Produto? produto = produtos.FirstOrDefault(p => p.Id == item.ProdutoId);

                        if (produto == null)
                        {
                            throw new Exception($"Produto com Id {item.ProdutoId} não encontrado.");
                        }

                        if (produto.Estoque < item.Quantidade)
                        {
                            throw new Exception($"Estoque insuficiente para o produto {produto.Nome}. Disponível: {produto.Estoque}, Requerido: {item.Quantidade}");
                        }

                        // Update stock
                        string sqlUpdateEstoque = "UPDATE Produtos SET Estoque = Estoque - @Quantidade WHERE Id = @ProdutoId";
                        using (SqlCommand cmd = new SqlCommand(sqlUpdateEstoque, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Quantidade", item.Quantidade);
                            cmd.Parameters.AddWithValue("@ProdutoId", item.ProdutoId);
                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected == 0)
                            {
                                throw new Exception($"Falha ao atualizar estoque do produto {produto.Nome}.");
                            }
                        }

                        item.PrecoUnitario = produto.Preco;
                    }

                    pedido.ValorTotal = itens.Sum(i => i.Quantidade * i.PrecoUnitario);

                    // Inseting Pedido
                    string sqlPedido = "INSERT INTO Pedidos (ClienteId, ValorTotal) " +
                                    "OUTPUT INSERTED.Id " +
                                    "VALUES (@ClienteId, @ValorTotal)";

                    int pedidoId = 0;
                    using (SqlCommand cmd = new SqlCommand(sqlPedido, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@ClienteId", pedido.ClienteId);
                        cmd.Parameters.AddWithValue("@ValorTotal", pedido.ValorTotal);

                        pedidoId = (int)cmd.ExecuteScalar()!;
                        if (pedidoId <= 0)
                            throw new Exception("Falha ao inserir pedido.");
                    }

                    // Inserting PedidoItems

                    string sqlPedidoItem = "INSERT INTO PedidoItens (PedidoId, ProdutoId, Quantidade, PrecoUnitario) " +
                                        "VALUES (@PedidoId, @ProdutoId, @Quantidade, @PrecoUnitario)";

                    foreach (PedidoItem item in itens)
                    {
                        item.PedidoId = pedidoId;
                        using (SqlCommand cmd = new SqlCommand(sqlPedidoItem, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@PedidoId", item.PedidoId);
                            cmd.Parameters.AddWithValue("@ProdutoId", item.ProdutoId);
                            cmd.Parameters.AddWithValue("@Quantidade", item.Quantidade);
                            cmd.Parameters.AddWithValue("@PrecoUnitario", item.PrecoUnitario);
                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected == 0)
                            {
                                throw new Exception($"Falha ao inserir item do pedido para o produto Id {item.ProdutoId}.");
                            }
                        }
                    }
                    transaction.Commit();
                    Console.WriteLine("Pedido criado com sucesso!");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Erro ao criar pedido: {ex.Message}");
                    throw;
                }
            }
        }

        public void ListarPedidosDeCliente(int clienteId)
        {
            using var conn = DatabaseConnection.GetConnection();
            conn.Open();

            string sql = "SELECT Id, DataPedido, ValorTotal FROM Pedidos WHERE ClienteId=@c";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@c", clienteId);
            using var reader = cmd.ExecuteReader();

            Console.WriteLine($"\n=== PEDIDOS DO CLIENTE {clienteId} ===");
            while (reader.Read())
            {
                Console.WriteLine($"Pedido: {reader["Id"]} | Data: {reader["DataPedido"]} | Total: {reader["ValorTotal"]}");
            }
        }

        public void DetalhesDoPedido(int pedidoId)
        {
            using var conn = DatabaseConnection.GetConnection();
            conn.Open();

            string sql = @"SELECT p.Id, p.DataPedido, p.ValorTotal, pr.Nome, i.Quantidade, i.PrecoUnitario
                           FROM Pedidos p
                           INNER JOIN PedidoItens i ON p.Id = i.PedidoId
                           INNER JOIN Produtos pr ON pr.Id = i.ProdutoId
                           WHERE p.Id=@id";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", pedidoId);
            using var reader = cmd.ExecuteReader();

            Console.WriteLine($"\n=== DETALHES DO PEDIDO {pedidoId} ===");
            while (reader.Read())
            {
                Console.WriteLine($"Produto: {reader["Nome"]} | Quantidade: {reader["Quantidade"]} | Unitário: {reader["PrecoUnitario"]}");
            }
        }
        public void TotalVendasPeriodo(DateTime inicio, DateTime fim)
        {
            using var conn = DatabaseConnection.GetConnection();
            conn.Open();
            using var cmd = new SqlCommand("EXEC sp_TotalVendasPeriodo @DataInicio, @DataFim", conn);
            cmd.Parameters.AddWithValue("@DataInicio", inicio);
            cmd.Parameters.AddWithValue("@DataFim", fim);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Console.WriteLine("\n=== TOTAL DE VENDAS ===");
                Console.WriteLine($"Total de pedidos: {reader["TotalPedidos"]}");
                Console.WriteLine($"Valor total: {reader["ValorTotal"]}");
                Console.WriteLine($"Ticket médio: {reader["TicketMedio"]}");
            }
            else
            {
                Console.WriteLine("Nenhuma venda registrada neste período.");
            }
        }
    }
}