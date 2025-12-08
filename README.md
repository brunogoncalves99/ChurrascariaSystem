# 🍖 Sistema de Gerenciamento de Churrasquinho

<div align="center">

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-12.0-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)

**Sistema completo de gerenciamento de pedidos para restaurantes de churrasquinho**

[Sobre](#-sobre-o-projeto) •
[Funcionalidades](#-funcionalidades) •
[Tecnologias](#-tecnologias-utilizadas) •
[Arquitetura](#-arquitetura) •
[Instalação](#-instalação) •
[Autor](#-autor)

</div>

---

## 📋 Sobre o Projeto

Sistema web desenvolvido em **.NET 8** com **Clean Architecture** e **Domain-Driven Design (DDD)** para gerenciar pedidos, produtos, mesas e faturamento de restaurantes especializados em churrasquinho.

O projeto foi desenvolvido como um **estudo de caso** para demonstrar boas práticas de desenvolvimento, incluindo:
- ✅ Clean Architecture (4 camadas)
- ✅ Domain-Driven Design (DDD)
- ✅ Princípios SOLID
- ✅ Repository Pattern
- ✅ Dependency Injection
- ✅ Entity Framework Core
- ✅ Autenticação com Claims
- ✅ Autorização baseada em Roles

---

## ✨ Funcionalidades

### 👨‍💼 Área Administrativa (Admin)
- ✅ Dashboard com estatísticas em tempo real
- ✅ Gráfico de faturamento dos últimos 7 dias (Chart.js)
- ✅ Ranking dos 5 produtos mais vendidos
- ✅ Gerenciamento completo de produtos (CRUD)
- ✅ Gerenciamento de tipos de produtos
- ✅ Configuração de mesas (número, capacidade, status)
- ✅ Visualização de todos os pedidos e faturamento

### 🍽️ Área do Garçom
- ✅ Criação de pedidos com carrinho interativo
- ✅ Seleção de mesa e produtos
- ✅ Filtro de produtos por categoria
- ✅ Cálculo automático de totais
- ✅ Atualização de status dos pedidos
- ✅ Visualização de pedidos abertos
- ✅ Gerenciamento visual de mesas (grid com cores por status)

### 🔐 Autenticação e Segurança
- ✅ Login com CPF e senha
- ✅ Autenticação baseada em Claims
- ✅ Autorização por perfis (Admin/Garçom)
- ✅ Senhas criptografadas com BCrypt
- ✅ Sessões persistentes com cookies

---

## 🛠 Tecnologias Utilizadas

### Backend
- **.NET 8** - Framework principal
- **C# 12** - Linguagem de programação
- **Entity Framework Core 8** - ORM
- **SQL Server** - Banco de dados
- **BCrypt.Net** - Criptografia de senhas
- **AutoMapper** - Mapeamento de objetos

### Frontend
- **ASP.NET Core MVC** - Framework web
- **Razor Pages** - Template engine
- **Bootstrap 5.3** - Framework CSS
- **jQuery 3.7** - Biblioteca JavaScript
- **Chart.js** - Gráficos interativos
- **Font Awesome** - Ícones
- **jQuery Mask** - Máscaras de entrada

### Padrões e Práticas
- **Clean Architecture** - Organização em 4 camadas
- **Domain-Driven Design (DDD)** - Modelagem de domínio
- **Repository Pattern** - Abstração de acesso a dados
- **SOLID Principles** - Princípios de design
- **Dependency Injection** - Inversão de controle
- **DTO Pattern** - Transferência de dados

---

## 🏗 Arquitetura

O projeto segue a **Clean Architecture** com 4 camadas bem definidas:

```
┌─────────────────────────────────────────────────────────┐
│                   PRESENTATION LAYER                     │
│                  ChurrascariaSystem.Web                  │
│         Controllers | Views | wwwroot | Helpers         │
└─────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────┐
│                   APPLICATION LAYER                      │
│              ChurrascariaSystem.Application              │
│              Services | DTOs | Interfaces                │
└─────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────┐
│                     DOMAIN LAYER                         │
│                ChurrascariaSystem.Domain                │
│      Entities | Value Objects | Enums | Interfaces      │
└─────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────┐
│                  INFRASTRUCTURE LAYER                    │
│            ChurrascariaSystem.Infrastructure             │
│        Repositories | DbContext | Configurations        │
└─────────────────────────────────────────────────────────┘
```

---

## 🚀 Instalação

### Pré-requisitos

Antes de começar, você precisa ter instalado:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) ou superior
- [SQL Server 2019+](https://www.microsoft.com/sql-server/sql-server-downloads) ou SQL Server Express
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

### Passo a Passo

1. **Clone o repositório**

```bash
git clone https://github.com/seu-usuario/churrasquinho-pedidos.git
cd churrasquinho-pedidos
```

2. **Configure a Connection String**

Edite o arquivo `ChurrasquinhoPedidos.WebUI/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ChurrasquinhoDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**Opções de Connection String:**

```json
// SQL Server LocalDB (Visual Studio)
"Server=(localdb)\\mssqllocaldb;Database=ChurrasquinhoDB;Trusted_Connection=True;TrustServerCertificate=True;"

// SQL Server Local
"Server=localhost;Database=ChurrasquinhoDB;Trusted_Connection=True;TrustServerCertificate=True;"

// SQL Server com autenticação
"Server=localhost;Database=ChurrasquinhoDB;User Id=seu_usuario;Password=sua_senha;TrustServerCertificate=True;"
```

3. **Restaure os pacotes NuGet**

```bash
dotnet restore
```

4. **Crie o banco de dados**

```bash
cd ChurrasquinhoPedidos.WebUI
dotnet ef database update --project ../ChurrasquinhoPedidos.Infrastructure
```

5. **Execute a aplicação**

```bash
dotnet run
```

6. **Acesse no navegador**

```
https://localhost:5001
```

---

### 📱 Funcionalidades por Perfil

#### Como Administrador:

1. **Dashboard**
   - Visualize faturamento diário e mensal
   - Acompanhe gráfico de faturamento dos últimos 7 dias
   - Veja os 5 produtos mais vendidos
   - Monitore mesas ocupadas e pedidos abertos

2. **Gerenciar Produtos**
   - Acesse Menu → Produtos
   - Cadastre novos produtos com nome, tipo, preço e descrição
   - Edite produtos existentes
   - Ative/desative produtos temporariamente

3. **Configurar Mesas**
   - Acesse Menu → Mesas
   - Cadastre mesas com número e capacidade
   - Gerencie status (Livre/Ocupada/Reservada)

#### Como Garçom:

1. **Criar Pedido**
   - Acesse "Novo Pedido"
   - Selecione a mesa
   - Filtre produtos por categoria
   - Adicione produtos ao carrinho
   - Finalize o pedido

2. **Gerenciar Pedidos**
   - Visualize todos os pedidos abertos
   - Atualize status (Aberto → Em Preparação → Pronto → Entregue)
   - Veja detalhes de cada pedido

3. **Visualizar Mesas**
   - Veja status visual de todas as mesas
   - Identifique mesas disponíveis/ocupadas

---

## 🧪 Dados de Exemplo (Seed)

O banco é populado automaticamente com:

- ✅ **2 Usuários** (Admin e Garçom)
- ✅ **4 Tipos de Produto** (Churrasquinhos, Bebidas, Acompanhamentos, Sobremesas)
- ✅ **6 Produtos** (Picanha, Frango, Alcatra, Refrigerante, Farofa, Vinagrete)
- ✅ **5 Mesas** (Mesa 1 a Mesa 5)

---

## 📸 Screenshots

<!-- Adicione screenshots do seu projeto aqui -->

### Tela de Login
![Login](docs/screenshots/login.png)

### Dashboard Administrativo
![Dashboard](docs/screenshots/dashboard.png)

### Criar Pedido
![Criar Pedido](docs/screenshots/criar-pedido.png)

### Gerenciar Produtos
![Produtos](docs/screenshots/produtos.png)

> **Nota:** Adicione suas próprias screenshots na pasta `docs/screenshots/`

---

## 🧪 Testes

### Testes Manuais

Execute a aplicação e teste os seguintes cenários:

**Login:**
- [ ] Login com credenciais válidas (Admin e Garçom)
- [ ] Login com CPF mascarado e sem máscara
- [ ] Tentativa de login com credenciais inválidas

**Pedidos:**
- [ ] Criar pedido com múltiplos produtos
- [ ] Filtrar produtos por tipo
- [ ] Atualizar status do pedido
- [ ] Visualizar detalhes do pedido

**Produtos (Admin):**
- [ ] Cadastrar novo produto
- [ ] Editar produto existente
- [ ] Ativar/desativar produto
- [ ] Buscar e filtrar produtos

**Dashboard (Admin):**
- [ ] Visualizar estatísticas atualizadas
- [ ] Verificar gráfico de faturamento
- [ ] Conferir produtos mais vendidos

---

## 🔮 Melhorias Futuras

Funcionalidades planejadas para próximas versões:

- [ ] Impressão de comandas
- [ ] Relatórios em PDF
- [ ] Notificações em tempo real (SignalR)
- [ ] Multi-tenant (múltiplos restaurantes)
- [ ] Sistema de comissão para garçons
- [ ] Controle de estoque
- [ ] Reserva de mesas online

---

## 📝 Comandos Úteis

### Entity Framework

```bash
# Criar nova migration
dotnet ef migrations add NomeDaMigration --project ChurrasquinhoPedidos.Infrastructure --startup-project ChurrasquinhoPedidos.WebUI

# Aplicar migrations
dotnet ef database update --project ChurrasquinhoPedidos.Infrastructure --startup-project ChurrasquinhoPedidos.WebUI

# Remover última migration
dotnet ef migrations remove --project ChurrasquinhoPedidos.Infrastructure --startup-project ChurrasquinhoPedidos.WebUI

# Gerar script SQL
dotnet ef migrations script --project ChurrasquinhoPedidos.Infrastructure --startup-project ChurrasquinhoPedidos.WebUI --output script.sql

# Dropar banco de dados
dotnet ef database drop --project ChurrasquinhoPedidos.Infrastructure --startup-project ChurrasquinhoPedidos.WebUI
```

### Build e Publicação

```bash
# Limpar build
dotnet clean

# Build do projeto
dotnet build

# Build em Release
dotnet build --configuration Release

# Publicar aplicação
dotnet publish --configuration Release --output ./publish

# Executar em modo de desenvolvimento
dotnet run --project ChurrasquinhoPedidos.WebUI

# Executar em modo de produção
dotnet run --project ChurrasquinhoPedidos.WebUI --configuration Release
```

---

## 🤝 Contribuindo

Contribuições são sempre bem-vindas!

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona MinhaFeature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

### Padrões de Código

- Siga os princípios SOLID
- Mantenha a Clean Architecture
- Escreva código limpo e legível
- Documente métodos complexos
- Adicione comentários quando necessário

---

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

## 👨‍💻 Autor

**Bruno**

- GitHub: [@seu-usuario](https://github.com/brunogoncalves99)
- LinkedIn: [Seu Nome](https://linkedin.com/in/brunogoncalveslemos)
- Email: bruno.goncalves1999@hotmail.com

---

## 🙏 Agradecimentos

- [Microsoft Docs](https://docs.microsoft.com/) - Documentação do .NET
- [Clean Architecture](https://blog.cleancoder.com/) - Robert C. Martin
- [Domain-Driven Design](https://domainlanguage.com/ddd/) - Eric Evans
- [Bootstrap](https://getbootstrap.com/) - Framework CSS
- [Chart.js](https://www.chartjs.org/) - Biblioteca de gráficos

---

## 📞 Suporte

Se você encontrar algum problema ou tiver dúvidas:

1. Verifique a seção de [Issues](https://github.com/brunogoncalves99/churrasquinho-pedidos/issues)
2. Abra uma nova issue descrevendo o problema
3. Entre em contato através do email

---

<div align="center">

**Desenvolvido com ❤️ usando .NET 8**

⭐ Se este projeto te ajudou, considere dar uma estrela!

</div>
