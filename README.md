# CP2-Csharp

README - Sistema de Loja C# + SQL Server
Descrição do Projeto

Este projeto implementa um sistema de gerenciamento de uma loja, permitindo cadastrar produtos, categorias, clientes e pedidos. Ele se conecta a um SQL Server local usando Microsoft.Data.SqlClient e suporta operações de CRUD (Create, Read, Update, Delete) para produtos, além de gerenciamento de pedidos.

O sistema inclui funcionalidades extras (desafios opcionais) como:

Listar produtos com estoque baixo.

Busca por nome de produto usando LIKE.

Total de vendas por período.

Listagem completa de produtos por categoria.

Requisitos

.NET 7 SDK ou superior

SQL Server 2022 (pode ser em container Docker)

Microsoft.Data.SqlClient (já referenciado no projeto)

Setup do Banco de Dados

Se for usar Docker, execute o container do SQL Server:

docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=SqlServer2024!" -p 1433:1433 --name sqlserver2022 -d mcr.microsoft.com/mssql/server:2022-latest


Execute o script Lab12_Setup.sql no SQL Server (via SQL Server Management Studio ou Azure Data Studio). Esse script cria:

Banco de dados LojaDB

Tabelas: Clientes, Categorias, Produtos, Pedidos, PedidoItens

Views: vw_ProdutosCompleto, vw_PedidosCompleto

Stored Procedures: sp_ProdutosEstoqueBaixo, sp_TotalVendasPeriodo, sp_ProdutosMaisVendidos

Dados de teste para categorias, produtos, clientes e pedidos

Confirme se o banco está rodando e acessível em localhost,1433 com usuário sa e senha SqlServer2024!.

Como Rodar o Projeto

Abra o terminal ou prompt de comando na pasta do projeto.

Restaure os pacotes NuGet (se necessário):

dotnet restore


Compile o projeto:

dotnet build


Execute o projeto:

dotnet run --project SistemaLoja


O menu interativo será exibido no console, permitindo:

Listar produtos

Inserir/atualizar/deletar produtos

Listar produtos por categoria

Criar novos pedidos

Consultar pedidos de clientes e detalhes de pedidos

Buscar produtos por nome

Consultar produtos com estoque baixo

Consultar total de vendas por período

Observações

Todos os IDs (Clientes, Produtos, Categorias) devem existir no banco antes de serem utilizados ao criar pedidos.

A transação no cadastro de pedidos garante que, se ocorrer algum erro (como estoque insuficiente), todas as alterações são revertidas.

Valores monetários usam decimal(10,2) no banco.

Estrutura do Projeto
SistemaLoja/
├─ Program.cs                # Menu e fluxo principal
├─ ProdutoRepository.cs      # CRUD e métodos extras de produtos
├─ PedidoRepository.cs       # Gerenciamento de pedidos
├─ DatabaseConnection.cs     # Conexão com SQL Server
├─ Models/                   # Classes Produto, Pedido, PedidoItem
└─ Lab12_Setup.sql           # Script de criação do banco e dados de teste

Autor

Jonata Rafael - Engenharia de Software, RM552939
