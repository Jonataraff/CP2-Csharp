using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using SistemaLoja.Lab12_ConexaoSQLServer;

namespace SistemaLoja
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
        public int CategoriaId { get; set; }
    }

    public class ProdutoRepository
    {
        public void ListarTodosProdutos()
        {
            using var conn = DatabaseConnection.GetConnection();
            conn.Open();

            string sql = "SELECT Id, Nome, Preco, Estoque, CategoriaId FROM Produtos";
            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            Console.WriteLine("\n=== LISTA DE PRODUTOS ===");
            while (reader.Read())
            {
                Console.WriteLine($"ID: {reader["Id"]} | Nome: {reader["Nome"]} | Preço: {reader["Preco"]} | Estoque: {reader["Estoque"]} | Categoria: {reader["CategoriaId"]}");
            }
        }

        public void InserirProduto(Produto p)
        {
            using var conn = DatabaseConnection.GetConnection();
            conn.Open();

            string sql = "INSERT INTO Produtos (Nome, Preco, Estoque, CategoriaId) VALUES (@n, @p, @e, @c)";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@n", p.Nome);
            cmd.Parameters.AddWithValue("@p", p.Preco);
            cmd.Parameters.AddWithValue("@e", p.Estoque);
            cmd.Parameters.AddWithValue("@c", p.CategoriaId);
            cmd.ExecuteNonQuery();

            Console.WriteLine("✅ Produto inserido com sucesso!");
        }

        public void AtualizarProduto(Produto p)
        {
            using var conn = DatabaseConnection.GetConnection();
            conn.Open();

            string sql = "UPDATE Produtos SET Nome=@n, Preco=@p, Estoque=@e, CategoriaId=@c WHERE Id=@id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@n", p.Nome);
            cmd.Parameters.AddWithValue("@p", p.Preco);
            cmd.Parameters.AddWithValue("@e", p.Estoque);
            cmd.Parameters.AddWithValue("@c", p.CategoriaId);
            cmd.Parameters.AddWithValue("@id", p.Id);
            int linhas = cmd.ExecuteNonQuery();

            Console.WriteLine(linhas > 0 ? "✅ Produto atualizado com sucesso!" : "⚠️ Produto não encontrado.");
        }

        public void DeletarProduto(int id)
        {
            using var conn = DatabaseConnection.GetConnection();
            conn.Open();

            string sql = "DELETE FROM Produtos WHERE Id=@id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            int linhas = cmd.ExecuteNonQuery();

            Console.WriteLine(linhas > 0 ? "🗑️ Produto excluído com sucesso!" : "⚠️ Produto não encontrado.");
        }

        public void ListarPorCategoria(int categoriaId)
        {
            using var conn = DatabaseConnection.GetConnection();
            conn.Open();

            string sql = "SELECT Id, Nome, Preco FROM Produtos WHERE CategoriaId=@c";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@c", categoriaId);
            using var reader = cmd.ExecuteReader();

            Console.WriteLine($"\n=== PRODUTOS DA CATEGORIA {categoriaId} ===");
            while (reader.Read())
            {
                Console.WriteLine($"ID: {reader["Id"]} | Nome: {reader["Nome"]} | Preço: {reader["Preco"]}");
            }
        }
        public void BuscarProdutosPorNome(string nome)
        {
            using var conn = DatabaseConnection.GetConnection();
            conn.Open();
            string sql = "SELECT Id, Nome, Preco, Estoque FROM Produtos WHERE Nome LIKE @nome";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@nome", "%" + nome + "%");
            using var reader = cmd.ExecuteReader();

            Console.WriteLine($"\n=== RESULTADOS PARA '{nome}' ===");
            while (reader.Read())
            {
                Console.WriteLine($"ID: {reader["Id"]} | Nome: {reader["Nome"]} | Preço: {reader["Preco"]} | Estoque: {reader["Estoque"]}");
            }
        }

        public void ListarProdutosEstoqueBaixo(int quantidadeMinima = 30)
        {
            using var conn = DatabaseConnection.GetConnection();
            conn.Open();
            string sql = "EXEC sp_ProdutosEstoqueBaixo @QuantidadeMinima";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@QuantidadeMinima", quantidadeMinima);
            using var reader = cmd.ExecuteReader();

            Console.WriteLine($"\n=== PRODUTOS COM ESTOQUE < {quantidadeMinima} ===");
            while (reader.Read())
            {
                Console.WriteLine($"ID: {reader["Id"]} | Nome: {reader["Nome"]} | Estoque: {reader["Estoque"]} | Categoria: {reader["Categoria"]}");
            }
        }
    }
}