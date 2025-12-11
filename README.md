# 🍖 Sistema de Gerenciamento de Churrasquinho

<div align="center">

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-12.0-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)

**Sistema completo de gerenciamento de pedidos e estoque para restaurantes de churrasquinho**

[Sobre](#-sobre-o-projeto) •
[Funcionalidades](#-funcionalidades) •
[Tecnologias](#-tecnologias-utilizadas) •
[Arquitetura](#-arquitetura) •
[Instalação](#-instalação) •
[Autor](#-autor)

</div>

---

## 📋 Sobre o Projeto

Sistema web desenvolvido em **.NET 8** com **Clean Architecture** e **Domain-Driven Design (DDD)** para gerenciar pedidos, produtos, mesas, estoque e faturamento de restaurantes especializados em churrasquinho.

O projeto foi desenvolvido como um **estudo de caso** para demonstrar boas práticas de desenvolvimento, incluindo:
- ✅ Clean Architecture (4 camadas)
- ✅ Domain-Driven Design (DDD)
- ✅ Princípios SOLID
- ✅ Repository Pattern
- ✅ Dependency Injection
- ✅ Entity Framework Core
- ✅ Autenticação com Claims
- ✅ Autorização baseada em Roles
- ✅ Background Services
- ✅ Email Notifications
- ✅ Relatórios em PDF

---

## ✨ Funcionalidades

### 👨‍💼 Área Administrativa (Admin)

#### Dashboard
- ✅ Dashboard com estatísticas em tempo real
- ✅ Gráfico de faturamento dos últimos 7 dias (Chart.js)
- ✅ Ranking dos 5 produtos mais vendidos
- ✅ Monitoramento de mesas ocupadas e pedidos abertos
- ✅ Indicadores de estoque crítico

#### Gerenciamento de Produtos
- ✅ Gerenciamento completo de produtos (CRUD)
- ✅ Gerenciamento de tipos de produtos
- ✅ Cadastro com nome, tipo, preço e descrição
- ✅ Ativação/desativação de produtos

#### Gerenciamento de Estoque ⭐ **NOVO**
- ✅ Controle completo de estoque por produto
- ✅ Dashboard de estoque com indicadores visuais:
  - 📊 Total de produtos cadastrados
  - ✅ Produtos com estoque OK (verde)
  - ⚠️ Produtos com estoque baixo (amarelo)
  - ❌ Produtos sem estoque (vermelho)
- ✅ Registro de movimentações de estoque:
  - **Entradas**: Compra, Devolução, Transferência, Ajuste
  - **Saídas**: Venda (automática), Perda, Dano, Validade
- ✅ Histórico completo de todas as movimentações
- ✅ Alertas visuais de estoque baixo/zerado
- ✅ Configuração de quantidade mínima por produto
- ✅ Ajustes manuais de estoque (inventário)
- ✅ Página dedicada de alertas de estoque
- ✅ Integração automática com pedidos (baixa automática no estoque)

#### Sistema de Alertas por Email ⭐ **NOVO**
- ✅ Alertas automáticos de estoque baixo por email
- ✅ Notificações de produtos zerados
- ✅ Emails HTML profissionais com templates coloridos
- ✅ Monitoramento em background (a cada 1 hora)
- ✅ Resumo consolidado de múltiplos produtos críticos
- ✅ Configuração SMTP flexível (Gmail, Outlook, Yahoo, etc)
- ✅ Emails com tabelas, badges coloridos e timestamps

#### Relatórios ⭐ **NOVO**
- ✅ Relatórios de pedidos em PDF (QuestPDF)
- ✅ Relatórios de movimentação de estoque
- ✅ Relatórios de faturamento
- ✅ Design profissional com logo e cabeçalho
- ✅ Exportação e download direto

#### Configurações
- ✅ Configuração de mesas (número, capacidade, status)
- ✅ Configuração de usuários e perfis
- ✅ Visualização de todos os pedidos e faturamento

### 🍽️ Área do Garçom

#### Pedidos
- ✅ Criação de pedidos com carrinho interativo
- ✅ Seleção de mesa e produtos
- ✅ Filtro de produtos por categoria
- ✅ Cálculo automático de totais e subtotais
- ✅ Atualização de status dos pedidos
- ✅ Visualização de pedidos abertos
- ✅ **Validação automática de estoque disponível** ⭐ **NOVO**
- ✅ **Baixa automática no estoque ao criar pedido** ⭐ **NOVO**
- ✅ **Mensagens de erro quando estoque insuficiente** ⭐ **NOVO**

#### Mesas
- ✅ Gerenciamento visual de mesas (grid com cores por status)
- ✅ Identificação rápida de mesas disponíveis/ocupadas
- ✅ Atualização automática de status ao criar/fechar pedido
- ✅ Cores visuais: Verde (Livre), Vermelho (Ocupada), Azul (Reservada)

### 🔐 Autenticação e Segurança
- ✅ Login com CPF e senha
- ✅ Autenticação baseada em Claims
- ✅ Autorização por perfis (Admin/Garçom)
- ✅ Senhas criptografadas com BCrypt
- ✅ Sessões persistentes com cookies
- ✅ Máscaras de CPF no formulário de login

---

## 🛠 Tecnologias Utilizadas

### Backend
- **.NET 8** - Framework principal
- **C# 12** - Linguagem de programação
- **Entity Framework Core 8** - ORM
- **SQL Server** - Banco de dados
- **BCrypt.Net** - Criptografia de senhas
- **AutoMapper** - Mapeamento de objetos
- **QuestPDF** ⭐ - Geração de relatórios em PDF
- **System.Net.Mail** ⭐ - Envio de emails SMTP

### Frontend
- **ASP.NET Core MVC** - Framework web
- **Razor Pages** - Template engine
- **Bootstrap 5.3** - Framework CSS responsivo
- **jQuery 3.7** - Biblioteca JavaScript
- **Chart.js** - Gráficos interativos
- **Font Awesome 6** - Ícones modernos
- **jQuery Mask** - Máscaras de entrada (CPF, telefone)
- **AJAX** - Requisições assíncronas

### Padrões e Práticas
- **Clean Architecture** - Organização em 4 camadas
- **Domain-Driven Design (DDD)** - Modelagem de domínio rica
- **Repository Pattern** - Abstração de acesso a dados
- **Service Pattern** - Camada de lógica de negócio
- **SOLID Principles** - Princípios de design orientado a objetos
- **Dependency Injection** - Inversão de controle nativa do .NET
- **DTO Pattern** - Transferência segura de dados entre camadas
- **Background Services** ⭐ - Tarefas assíncronas em background (IHostedService)
- **Transaction Scope** ⭐ - Transações atômicas (tudo ou nada)

---

## 🏗 Arquitetura

O projeto segue a **Clean Architecture** com 4 camadas bem definidas:

```
┌─────────────────────────────────────────────────────────┐
│                   PRESENTATION LAYER                     │
│                  ChurrascariaSystem.WebUI                │
│    Controllers | Views | wwwroot | Helpers | Services   │
│    EstoqueController | MonitoramentoEstoqueService      │
└─────────────────────────────────────────────────────────┘
                            ↓ Depende de
┌─────────────────────────────────────────────────────────┐
│                   APPLICATION LAYER                      │
│              ChurrascariaSystem.Application              │
│         Services | DTOs | Interfaces | Validators       │
│    EstoqueService | EmailService | PedidoService         │
└─────────────────────────────────────────────────────────┘
                            ↓ Depende de
┌─────────────────────────────────────────────────────────┐
│                     DOMAIN LAYER                         │
│                ChurrascariaSystem.Domain                 │
│      Entities | Value Objects | Enums | Interfaces      │
│    Pedido | Produto | Mesa | Estoque | MovimentacaoEst  │
└─────────────────────────────────────────────────────────┘
                            ↑ Implementado por
┌─────────────────────────────────────────────────────────┐
│                  INFRASTRUCTURE LAYER                    │
│            ChurrascariaSystem.Infrastructure             │
│        Repositories | DbContext | Configurations        │
│    EstoqueRepository | MovimentacaoEstoqueRepository    │
└─────────────────────────────────────────────────────────┘
```

### Fluxo de Integração: Pedido → Estoque

```
1. Garçom cria pedido no sistema
2. PedidoService valida estoque disponível
3. Pedido é salvo no banco (Transaction Scope)
4. EstoqueService faz baixa automática
5. MovimentacaoEstoque registra a saída
6. MonitoramentoService verifica estoque (a cada 1h)
7. EmailService envia alerta se estoque baixo
```

---

## 🚀 Instalação

### Pré-requisitos

Antes de começar, você precisa ter instalado:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) ou superior
- [SQL Server 2019+](https://www.microsoft.com/sql-server/sql-server-downloads) ou SQL Server Express
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (recomendado) ou [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

### Passo a Passo

#### 1. Clone o repositório

```bash
git clone https://github.com/brunogoncalves99/churrasquinho-pedidos.git
cd churrasquinho-pedidos
```

#### 2. Configure a Connection String

Edite o arquivo `ChurrascariaSystem.WebUI/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ChurrasquinhoDB;Trusted_Connection=True;TrustServerCertificate=True;"
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

#### 3. Configure o Sistema de Email (Opcional)

Se quiser receber alertas de estoque por email, configure no `appsettings.json`:

```json
{
  "EmailSettings": {
    "EmailRemetente": "seu.email@gmail.com",
    "NomeRemetente": "Sistema Churrascaria",
    "EmailDestinatario": "destinatario@example.com",
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": "587",
    "SmtpUsuario": "seu.email@gmail.com",
    "SmtpSenha": "senha_de_app_gmail",
    "HabilitarSsl": "true"
  }
}
```

**Para gerar senha de app do Gmail:**
1. Acesse: https://myaccount.google.com/security
2. Ative "Verificação em duas etapas"
3. Vá em "Senhas de app" → Email
4. Copie a senha gerada (16 caracteres)

#### 4. Restaure os pacotes NuGet

```bash
dotnet restore
```

#### 5. Crie o banco de dados e aplique migrations

```bash
# Via CLI (.NET)
dotnet ef database update --project ChurrascariaSystem.Infrastructure --startup-project ChurrascariaSystem.WebUI

# Ou via Package Manager Console (Visual Studio)
Update-Database
```

#### 6. Execute a aplicação

```bash
cd ChurrascariaSystem.WebUI
dotnet run
```

Ou pressione **F5** no Visual Studio.

#### 7. Acesse no navegador

```
https://localhost:5001
ou
http://localhost:5000
```

---

## 👤 Usuários Padrão

Após rodar as migrations, o sistema cria usuários de teste:

### Administrador
- **CPF:** `12345678900`
- **Senha:** (definida no seed)
- **Perfil:** Admin
- **Acesso:** Todas as funcionalidades

### Garçom
- **CPF:** `98765432100`
- **Senha:** (definida no seed)
- **Perfil:** Garçom
- **Acesso:** Pedidos e Mesas

---

## 📱 Como Usar

### Como Administrador:

#### 1. Dashboard
- Acesse após login
- Visualize estatísticas de hoje
- Veja gráfico de faturamento dos últimos 7 dias
- Confira produtos mais vendidos
- Monitore alertas de estoque

#### 2. Gerenciar Produtos
- Menu → Produtos
- Clique em "Novo Produto"
- Preencha: Nome, Tipo, Preço, Descrição
- Salve e gerencie lista

#### 3. Controle de Estoque ⭐
- Menu → Estoque
- Visualize dashboard com 4 cards:
  - **Total** (azul)
  - **OK** (verde)
  - **Baixo** (amarelo)
  - **Zerado** (vermelho)
- **Grid de Produtos**: Cada linha mostra:
  - ID, Nome do Produto
  - Quantidade Atual vs Mínima
  - Badge de Status (OK/Baixo/Sem Estoque)
  - Última Atualização
  - **Botões de ação**:
    - 🟢 **[+] Entrada** - Adicionar estoque
    - ✏️ **Ajustar** - Ajuste manual (inventário)
    - 📊 **Histórico** - Ver movimentações
    - ⚙️ **Config** - Alterar quantidade mínima

#### 4. Dar Entrada no Estoque
- Clique no botão **[+]** do produto
- Modal abre com formulário:
  - Quantidade a adicionar
  - Motivo (Compra/Devolução/Transferência/Ajuste)
  - Observação (opcional)
- Clique em "Adicionar"
- Quantidade é atualizada automaticamente

#### 5. Ajustar Estoque (Inventário)
- Clique no botão **✏️ Ajustar**
- Modal abre para ajuste:
  - Nova quantidade (valor exato)
  - Motivo (Inventário/Perda/Dano/Validade)
  - Observação
- Sistema registra a movimentação

#### 6. Ver Histórico de Movimentações
- Clique no botão **📊 Histórico**
- Modal mostra tabela com:
  - Data/Hora da movimentação
  - Tipo (Entrada/Saída)
  - Quantidade movimentada
  - Usuário responsável
  - Pedido vinculado (se houver)

#### 7. Alertas de Estoque
- Menu → Estoque → **Alertas**
- Veja produtos com problemas:
  - **Produtos Sem Estoque** (vermelho)
  - **Produtos com Estoque Baixo** (amarelo)
- Botões de ação rápida
- Recomendações de compra

#### 8. Configurar Mesas
- Menu → Mesas
- Cadastre mesas com:
  - Número da mesa
  - Capacidade (pessoas)
  - Status inicial
- Gerencie lista de mesas

### Como Garçom:

#### 1. Criar Pedido
- Clique em "Novo Pedido"
- **Passo 1**: Selecione a mesa
- **Passo 2**: Adicione produtos ao carrinho
  - Filtre por categoria
  - Clique em "Adicionar"
  - Ajuste quantidades
  - **Sistema valida estoque automaticamente** ⭐
- **Passo 3**: Revise o pedido
  - Veja subtotais
  - Total calculado automaticamente
- Clique em "Finalizar Pedido"
- **Estoque é baixado automaticamente** ⭐

#### 2. Gerenciar Pedidos
- Menu → Pedidos
- Visualize todos os pedidos abertos
- Atualize status:
  - Aberto → Em Preparação
  - Em Preparação → Pronto
  - Pronto → Entregue
- Veja detalhes de cada pedido

#### 3. Visualizar Mesas
- Menu → Mesas
- Grid visual com cores:
  - 🟢 **Verde**: Mesa Livre
  - 🔴 **Vermelho**: Mesa Ocupada
  - 🔵 **Azul**: Mesa Reservada
- Clique para ver detalhes

---

## 🔄 Fluxo Completo: Pedido com Estoque

```
1. Garçom acessa "Novo Pedido"
2. Seleciona mesa
3. Adiciona produtos ao carrinho
   ↓
4. Sistema VALIDA estoque disponível ⭐
   ├─ Estoque OK → Permite adicionar
   └─ Estoque INSUFICIENTE → Mostra erro
   ↓
5. Garçom finaliza pedido
   ↓
6. Sistema cria pedido (Transaction Scope) ⭐
7. Sistema faz BAIXA AUTOMÁTICA no estoque ⭐
8. MovimentacaoEstoque registra:
   - Tipo: Saída
   - Motivo: Venda
   - Quantidade: quantidade do pedido
   - PedidoId: vinculado ao pedido
   - UsuarioId: garçom que fez o pedido
   ↓
9. Mesa muda para "Ocupada"
10. Pedido aparece na lista
    ↓
--- BACKGROUND (a cada 1 hora) ---
11. MonitoramentoEstoqueService verifica estoque ⭐
12. Se estoque < mínimo → EmailService envia alerta ⭐
13. Admin recebe email com produtos críticos
```

---

## 🧪 Dados de Exemplo (Seed)

O banco é populado automaticamente com:

- ✅ **2 Usuários** (Admin e Garçom)
- ✅ **4 Tipos de Produto** (Churrasquinhos, Bebidas, Acompanhamentos, Sobremesas)
- ✅ **10 Produtos** com estoque configurado ⭐
- ✅ **5 Mesas** (capacidade variada)
- ✅ **Estoque inicial** para todos os produtos ⭐
- ✅ **Movimentações de exemplo** ⭐

---

## 📝 Comandos Úteis

### Entity Framework

```bash
# Criar nova migration
dotnet ef migrations add NomeDaMigration --project ChurrascariaSystem.Infrastructure --startup-project ChurrascariaSystem.WebUI

# Aplicar migrations
dotnet ef database update --project ChurrascariaSystem.Infrastructure --startup-project ChurrascariaSystem.WebUI

# Remover última migration
dotnet ef migrations remove --project ChurrascariaSystem.Infrastructure --startup-project ChurrascariaSystem.WebUI

# Gerar script SQL
dotnet ef migrations script --project ChurrascariaSystem.Infrastructure --startup-project ChurrascariaSystem.WebUI --output script.sql

# Dropar banco de dados
dotnet ef database drop --force --project ChurrascariaSystem.Infrastructure --startup-project ChurrascariaSystem.WebUI
```

### Package Manager Console (Visual Studio)

```powershell
# Criar migration
Add-Migration NomeDaMigration

# Aplicar migrations
Update-Database

# Remover migration
Remove-Migration

# Ver migrations
Get-Migration

# Script SQL
Script-Migration
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
dotnet run --project ChurrascariaSystem.WebUI

# Executar em modo de produção
dotnet run --project ChurrascariaSystem.WebUI --configuration Release
```

---

## 🧪 Testes

### Testes Manuais

Execute a aplicação e teste os seguintes cenários:

**Login:**
- [ ] Login como Admin (CPF: 12345678900)
- [ ] Login como Garçom (CPF: 98765432100)
- [ ] Login com CPF formatado e sem formatação
- [ ] Tentativa de login com credenciais inválidas

**Estoque (Admin):**
- [ ] Visualizar dashboard de estoque
- [ ] Dar entrada em produto (compra)
- [ ] Ajustar estoque (inventário)
- [ ] Ver histórico de movimentações
- [ ] Configurar quantidade mínima
- [ ] Verificar alertas de estoque baixo
- [ ] Testar cores dos badges (OK/Baixo/Zerado)

**Pedidos com Estoque:**
- [ ] Criar pedido com produto em estoque
- [ ] Tentar criar pedido sem estoque (erro)
- [ ] Verificar baixa automática no estoque
- [ ] Confirmar registro de movimentação
- [ ] Ver pedido vinculado no histórico

**Alertas de Email:**
- [ ] Configurar email no appsettings.json
- [ ] Aguardar 30 segundos após iniciar app
- [ ] Verificar log do monitoramento
- [ ] Reduzir estoque abaixo do mínimo
- [ ] Aguardar próxima verificação (1 hora)
- [ ] Verificar recebimento de email

**Dashboard (Admin):**
- [ ] Visualizar estatísticas atualizadas
- [ ] Verificar gráfico de faturamento
- [ ] Conferir produtos mais vendidos
- [ ] Ver alertas de estoque

---

## 🔮 Melhorias Futuras

Funcionalidades planejadas para próximas versões:

- [ ] Sistema de reserva de mesas online
- [ ] Integração com impressora de comandas
- [ ] Notificações push em tempo real (SignalR)
- [ ] Multi-tenant (múltiplos restaurantes)
- [ ] Sistema de comissão para garçons
- [ ] Controle de caixa e fechamento
- [ ] App mobile para garçons
- [ ] Dashboard com mais métricas
- [ ] Relatórios avançados em Excel
- [ ] Integração com sistemas de pagamento

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
- Mantenha a Clean Architecture (não misture camadas)
- Escreva código limpo e legível
- Documente métodos complexos com XML Comments
- Use nomes descritivos para variáveis e métodos
- Adicione testes quando possível

---

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

## 👨‍💻 Autor

**Bruno Gonçalves**

- GitHub: [@brunogoncalves99](https://github.com/brunogoncalves99)
- LinkedIn: [Bruno Gonçalves](https://linkedin.com/in/brunogoncalveslemos)
- Email: bruno.goncalves1999@hotmail.com

---

## 🙏 Agradecimentos

- [Microsoft Docs](https://docs.microsoft.com/) - Documentação do .NET
- [Clean Architecture](https://blog.cleancoder.com/) - Robert C. Martin
- [Domain-Driven Design](https://domainlanguage.com/ddd/) - Eric Evans
- [Bootstrap](https://getbootstrap.com/) - Framework CSS
- [Chart.js](https://www.chartjs.org/) - Biblioteca de gráficos
- [QuestPDF](https://www.questpdf.com/) - Geração de PDFs
- [Font Awesome](https://fontawesome.com/) - Ícones

---

## 📞 Suporte

Se você encontrar algum problema ou tiver dúvidas:

1. Verifique a seção de [Issues](https://github.com/brunogoncalves99/churrasquinho-pedidos/issues)
2. Abra uma nova issue descrevendo o problema
3. Entre em contato através do email: bruno.goncalves1999@hotmail.com

---

<div align="center">

**Desenvolvido com ❤️ usando .NET 8 e Clean Architecture**

⭐ Se este projeto te ajudou, considere dar uma estrela no repositório!

[![GitHub stars](https://img.shields.io/github/stars/brunogoncalves99/churrasquinho-pedidos?style=social)](https://github.com/brunogoncalves99/churrasquinho-pedidos/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/brunogoncalves99/churrasquinho-pedidos?style=social)](https://github.com/brunogoncalves99/churrasquinho-pedidos/network/members)

</div>
