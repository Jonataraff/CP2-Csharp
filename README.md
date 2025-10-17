# CP2-Csharp

# 🛍️ Sistema de Loja — C# + SQL Server

## 📘 Descrição do Projeto

Este projeto implementa um **sistema de gerenciamento de loja**, permitindo cadastrar **produtos, categorias, clientes e pedidos**.
Ele se conecta a um **SQL Server local** usando `Microsoft.Data.SqlClient` e suporta **operações completas de CRUD** (Create, Read, Update, Delete) para produtos, além de **gerenciamento de pedidos** com transações seguras.

### ⚙️ Funcionalidades Extras

* Listar produtos com **estoque baixo**
* **Busca** por nome de produto (`LIKE`)
* **Total de vendas por período**
* **Listagem completa** de produtos por categoria

---

## 🧩 Requisitos

* [.NET 7 SDK](https://dotnet.microsoft.com/download) ou superior
* [SQL Server 2022](https://hub.docker.com/_/microsoft-mssql-server) (pode ser executado via Docker)
* `Microsoft.Data.SqlClient` (já incluído no projeto)

---

## 🐋 Setup do Banco de Dados (Docker)

Execute o container do SQL Server:

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=SqlServer2024!" -p 1433:1433 --name sqlserver2022 -d mcr.microsoft.com/mssql/server:2022-latest
```

Depois, execute o script `Lab12_Setup.sql` no SQL Server (via SQL Server Management Studio ou Azure Data Studio).
Esse script cria:

* Banco de dados **LojaDB**
* Tabelas: `Clientes`, `Categorias`, `Produtos`, `Pedidos`, `PedidoItens`
* Views: `vw_ProdutosCompleto`, `vw_PedidosCompleto`
* Stored Procedures:

  * `sp_ProdutosEstoqueBaixo`
  * `sp_TotalVendasPeriodo`
  * `sp_ProdutosMaisVendidos`
* Dados de teste para categorias, produtos, clientes e pedidos

**Confirme:** o banco deve estar acessível em `localhost,1433` com:

* Usuário: `sa`
* Senha: `SqlServer2024!`

---

## ▶️ Como Rodar o Projeto

Abra o terminal na pasta do projeto e execute:

```bash
dotnet restore
dotnet build
dotnet run --project SistemaLoja
```

O **menu interativo** será exibido no console, permitindo:

* 📦 Listar, inserir, atualizar e deletar produtos
* 🗂️ Listar produtos por categoria
* 🧾 Criar novos pedidos
* 👤 Consultar pedidos de clientes e detalhes
* 🔍 Buscar produtos por nome
* ⚠️ Consultar produtos com estoque baixo
* 💰 Consultar total de vendas por período

---

## 🧠 Observações Importantes

* Todos os IDs (**Clientes**, **Produtos**, **Categorias**) devem existir antes de criar pedidos.
* A criação de pedidos é feita em **transação**, garantindo que, se algo falhar (ex: estoque insuficiente), nenhuma alteração seja confirmada.
* Campos monetários utilizam `decimal(10,2)` no banco.

---

## 🗂️ Estrutura do Projeto

```
SistemaLoja/
├─ Program.cs                # Menu e fluxo principal
├─ ProdutoRepository.cs      # CRUD e métodos extras de produtos
├─ PedidoRepository.cs       # Gerenciamento de pedidos
├─ DatabaseConnection.cs     # Conexão com SQL Server
├─ Models/                   # Classes Produto, Pedido, PedidoItem
└─ Lab12_Setup.sql           # Script de criação do banco e dados de teste
```

---

## 🧑‍💻 Alunos

**Jonata Rafael: RM552939**
**Vinicius Silva - RM553240**


---

