# 🎓 Roteiro de Aprendizagem: Arquitetura Vertical Slice com .NET 9

## 📚 Índice
1. [Introdução](#introdução)
2. [O que é Vertical Slice Architecture?](#o-que-é-vertical-slice-architecture)
3. [Estrutura do Projeto](#estrutura-do-projeto)
4. [Conceitos Fundamentais](#conceitos-fundamentais)
5. [Explorando o Código](#explorando-o-código)
6. [Atividade Prática](#atividade-prática)
7. [Entrega](#entrega)
8. [Referências Adicionais](#referências-adicionais)

---

## 🎯 Introdução

Bem-vindo ao roteiro de aprendizagem sobre **Vertical Slice Architecture**! Este projeto foi criado para ajudá-lo a entender e aplicar esta arquitetura moderna usando .NET 9 e C#.

### O que você vai aprender:
- ✅ Conceitos fundamentais da Vertical Slice Architecture
- ✅ Como organizar código por funcionalidades ao invés de camadas técnicas
- ✅ Como criar endpoints RESTful seguindo este padrão
- ✅ Como trabalhar com Entity Framework Core
- ✅ Como implementar novas funcionalidades do zero

### Pré-requisitos:
- .NET 9 SDK instalado
- Visual Studio 2022, VS Code ou Rider
- Conhecimentos básicos de C# e ASP.NET Core
- Conhecimentos básicos de APIs REST

---

## 🏗️ O que é Vertical Slice Architecture?

### Arquitetura Tradicional em Camadas (Layered Architecture)

Na arquitetura tradicional, organizamos o código por **camadas técnicas**:

```
📁 Projeto
├── 📁 Controllers
│   ├── ProdutoController.cs
│   └── CategoriaController.cs
├── 📁 Services
│   ├── ProdutoService.cs
│   └── CategoriaService.cs
├── 📁 Repositories
│   ├── ProdutoRepository.cs
│   └── CategoriaRepository.cs
└── 📁 Models
    ├── Produto.cs
    └── Categoria.cs
```

**Problema:** Quando precisamos adicionar ou modificar uma funcionalidade, precisamos mexer em arquivos espalhados por várias camadas diferentes.

### Vertical Slice Architecture

Na **Vertical Slice Architecture**, organizamos o código por **funcionalidades de negócio**:

```
📁 Projeto
├── 📁 Funcionalidades
│   ├── 📁 Produtos
│   │   ├── Produto.cs
│   │   ├── 📁 ObterTodosProdutos
│   │   │   ├── ObterTodosProdutosHandler.cs
│   │   │   └── ObterTodosProdutosEndpoint.cs
│   │   ├── 📁 CriarProduto
│   │   │   ├── CriarProdutoHandler.cs
│   │   │   └── CriarProdutoEndpoint.cs
│   │   └── ...
│   └── 📁 Categorias
│       ├── Categoria.cs
│       └── ...
└── 📁 Infraestrutura
    └── BancoDeDados.cs
```

**Vantagens:**

1. ✅ **Alta Coesão:** Todo código relacionado a uma funcionalidade fica junto
2. ✅ **Baixo Acoplamento:** Funcionalidades são independentes entre si
3. ✅ **Fácil Manutenção:** Para modificar uma funcionalidade, você sabe exatamente onde ir
4. ✅ **Facilita o Trabalho em Equipe:** Diferentes desenvolvedores podem trabalhar em diferentes slices sem conflitos
5. ✅ **Código mais Simples:** Cada slice resolve apenas um problema específico

### Quando usar?

✅ **Bom para:**
- Aplicações com muitas funcionalidades diferentes
- Times grandes trabalhando no mesmo projeto
- Quando as funcionalidades mudam com frequência
- Projetos que crescem com o tempo

❌ **Talvez não seja ideal para:**
- Aplicações muito pequenas e simples
- Quando há muita lógica compartilhada entre funcionalidades

---

## 📂 Estrutura do Projeto

Este projeto demonstra a Vertical Slice Architecture com um sistema simples de catálogo de produtos:

```
AprendizadoVerticalSlice/
├── 📁 Funcionalidades/
│   ├── 📁 Produtos/
│   │   ├── Produto.cs                                    # Entidade Produto
│   │   ├── 📁 ObterTodosProdutos/
│   │   │   ├── ObterTodosProdutosHandler.cs             # Lógica de negócio
│   │   │   └── ObterTodosProdutosEndpoint.cs            # Configuração do endpoint
│   │   ├── 📁 ObterProdutoPorId/
│   │   │   ├── ObterProdutoPorIdHandler.cs
│   │   │   └── ObterProdutoPorIdEndpoint.cs
│   │   ├── 📁 CriarProduto/
│   │   │   ├── CriarProdutoHandler.cs
│   │   │   └── CriarProdutoEndpoint.cs
│   │   ├── 📁 AtualizarProduto/
│   │   │   ├── AtualizarProdutoHandler.cs
│   │   │   └── AtualizarProdutoEndpoint.cs
│   │   └── 📁 ExcluirProduto/
│   │       ├── ExcluirProdutoHandler.cs
│   │       └── ExcluirProdutoEndpoint.cs
│   └── 📁 Categorias/
│       ├── Categoria.cs                                  # Entidade Categoria
│       ├── 📁 ObterTodasCategorias/
│       │   ├── ObterTodasCategoriasHandler.cs
│       │   └── ObterTodasCategoriasEndpoint.cs
│       └── 📁 CriarCategoria/
│           ├── CriarCategoriaHandler.cs
│           └── CriarCategoriaEndpoint.cs
├── 📁 Infraestrutura/
│   └── BancoDeDados.cs                                   # Contexto do EF Core
├── Program.cs                                            # Configuração da aplicação
└── appsettings.json                                      # Configurações
```

### Componentes de cada Slice:

1. **Entidade** (`Produto.cs`, `Categoria.cs`): Representa a entidade do domínio
2. **Handler** (`*Handler.cs`): Contém a lógica de negócio da funcionalidade
3. **Endpoint** (`*Endpoint.cs`): Define e configura o endpoint HTTP
4. **Records de Request/Response**: Definem os contratos de entrada e saída

---

## 💡 Conceitos Fundamentais

### 1. Handler (Manipulador)

O **Handler** é o coração de cada slice. Ele contém toda a lógica de negócio para executar uma funcionalidade específica.

**Características:**
- Tem uma responsabilidade única e bem definida
- Recebe as dependências via construtor (Injeção de Dependência)
- Retorna um resultado específico para aquela operação
- Pode fazer validações, transformações e chamadas ao banco de dados

**Exemplo:**
```csharp
public class CriarProdutoHandler
{
    private readonly BancoDeDados _bancoDeDados;

    public CriarProdutoHandler(BancoDeDados bancoDeDados)
    {
        _bancoDeDados = bancoDeDados;
    }

    public async Task<Resultado> Executar(CriarProdutoRequisicao requisicao)
    {
        // Toda a lógica para criar um produto fica aqui
        // Validações, transformações, persistência, etc.
    }
}
```

### 2. Endpoint

O **Endpoint** é responsável por configurar a rota HTTP que expõe a funcionalidade.

**Características:**
- Define o método HTTP (GET, POST, PUT, DELETE)
- Define a rota (URL)
- Mapeia parâmetros de entrada
- Chama o Handler apropriado
- Retorna a resposta HTTP adequada

**Exemplo:**
```csharp
public static class CriarProdutoEndpoint
{
    public static IEndpointRouteBuilder MapCriarProduto(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/produtos", async (
            [FromBody] CriarProdutoRequisicao requisicao,
            [FromServices] CriarProdutoHandler handler) =>
        {
            var resultado = await handler.Executar(requisicao);
            return Results.Created($"/api/produtos/{resultado.Id}", resultado);
        });

        return endpoints;
    }
}
```

### 3. Records de Request e Response

**Records** são estruturas imutáveis perfeitas para representar dados que trafegam pela API.

**Request Record:** Define o que o cliente envia
```csharp
public record CriarProdutoRequisicao(
    string Nome,
    decimal Preco,
    int CategoriaId
);
```

**Response Record:** Define o que a API retorna
```csharp
public record ProdutoResposta(
    int Id,
    string Nome,
    decimal Preco
);
```

### 4. Injeção de Dependência

Cada Handler é registrado no container de DI do ASP.NET Core:

```csharp
// No Program.cs
builder.Services.AddScoped<CriarProdutoHandler>();
```

Isso permite que:
- O framework crie instâncias automaticamente
- As dependências sejam injetadas
- O gerenciamento de ciclo de vida seja automático

---

## 🔍 Explorando o Código

### Slice Completa: Criar Produto

Vamos analisar como funciona a funcionalidade de **Criar Produto** passo a passo.

#### 1. O Handler (`CriarProdutoHandler.cs`)

```csharp
public class CriarProdutoHandler
{
    private readonly BancoDeDados _bancoDeDados;

    public CriarProdutoHandler(BancoDeDados bancoDeDados)
    {
        _bancoDeDados = bancoDeDados;
    }

    public async Task<(bool Sucesso, string? Erro, CriarProdutoResposta? Produto)> 
        Executar(CriarProdutoRequisicao requisicao)
    {
        // 1. Validações
        if (string.IsNullOrWhiteSpace(requisicao.Nome))
            return (false, "O nome do produto é obrigatório.", null);

        if (requisicao.Preco <= 0)
            return (false, "O preço deve ser maior que zero.", null);

        // 2. Validação de relacionamento
        var categoriaExiste = await _bancoDeDados.Categorias
            .AnyAsync(c => c.Id == requisicao.CategoriaId);

        if (!categoriaExiste)
            return (false, $"Categoria não encontrada.", null);

        // 3. Criação da entidade
        var novoProduto = new Produto
        {
            Nome = requisicao.Nome,
            Preco = requisicao.Preco,
            // ... outros campos
        };

        // 4. Persistência
        _bancoDeDados.Produtos.Add(novoProduto);
        await _bancoDeDados.SaveChangesAsync();

        // 5. Retorno
        var resposta = new CriarProdutoResposta(
            novoProduto.Id,
            novoProduto.Nome,
            novoProduto.Preco
        );

        return (true, null, resposta);
    }
}
```

**O que está acontecendo:**
1. ✅ Recebe o banco de dados via construtor
2. ✅ Valida os dados de entrada
3. ✅ Verifica se a categoria existe (regra de negócio)
4. ✅ Cria a entidade
5. ✅ Salva no banco de dados
6. ✅ Retorna o resultado

#### 2. O Endpoint (`CriarProdutoEndpoint.cs`)

```csharp
public static class CriarProdutoEndpoint
{
    public static IEndpointRouteBuilder MapCriarProduto(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/produtos", async (
            [FromBody] CriarProdutoRequisicao requisicao,
            [FromServices] CriarProdutoHandler handler) =>
        {
            var (sucesso, erro, produto) = await handler.Executar(requisicao);

            if (!sucesso)
                return Results.BadRequest(new { mensagem = erro });

            return Results.Created($"/api/produtos/{produto!.Id}", produto);
        })
        .WithName("CriarProduto")
        .WithTags("Produtos")
        .Produces<CriarProdutoResposta>(201)
        .Produces(400);

        return endpoints;
    }
}
```

**O que está acontecendo:**
1. ✅ Define uma rota POST em `/api/produtos`
2. ✅ Recebe a requisição do body
3. ✅ Chama o Handler
4. ✅ Trata o resultado (sucesso ou erro)
5. ✅ Retorna a resposta HTTP adequada (201 Created ou 400 Bad Request)

#### 3. Registro no Program.cs

```csharp
// Registra o Handler
builder.Services.AddScoped<CriarProdutoHandler>();

// Mapeia o Endpoint
app.MapCriarProduto();
```

### Comparando com Camadas Tradicionais

**Vertical Slice:**
```
CriarProduto/
├── CriarProdutoHandler.cs      ← Toda lógica aqui
└── CriarProdutoEndpoint.cs     ← Configuração HTTP aqui
```
✅ Tudo relacionado a "Criar Produto" está em um único lugar!

**Camadas Tradicionais:**
```
Controllers/ProdutoController.cs       ← Endpoint
Services/ProdutoService.cs             ← Lógica
Repositories/ProdutoRepository.cs      ← Acesso a dados
Validators/ProdutoValidator.cs         ← Validações
```
❌ Para entender "Criar Produto", preciso abrir 4+ arquivos diferentes!

---

## 🚀 Como Executar o Projeto

### 1. Clonar o Repositório
```bash
git clone <url-do-repositorio>
cd AprendizadoVerticalSlice
```

### 2. Restaurar Pacotes
```bash
dotnet restore
```

### 3. Executar a Aplicação
```bash
dotnet run
```

### 4. Acessar a Documentação da API
Abra seu navegador em:
```
http://localhost:5156/scalar/v1
```
(A porta pode variar - verifique no console)

A interface Scalar oferece uma documentação interativa moderna onde você pode:
- Ver todos os endpoints disponíveis
- Testar as requisições diretamente no navegador
- Ver exemplos de request e response

### 5. Testar os Endpoints

#### Obter todas as categorias:
```http
GET /api/categorias
```

#### Criar um produto:
```http
POST /api/produtos
Content-Type: application/json

{
  "nome": "Mouse Gamer",
  "descricao": "Mouse gamer RGB",
  "preco": 150.00,
  "quantidadeEstoque": 50,
  "categoriaId": 1
}
```

#### Obter todos os produtos:
```http
GET /api/produtos
```

#### Obter um produto específico:
```http
GET /api/produtos/1
```

#### Atualizar um produto:
```http
PUT /api/produtos/1
Content-Type: application/json

{
  "nome": "Mouse Gamer RGB",
  "descricao": "Mouse gamer RGB com 16.000 DPI",
  "preco": 180.00,
  "quantidadeEstoque": 45,
  "categoriaId": 1
}
```

#### Excluir um produto:
```http
DELETE /api/produtos/1
```

---

## 🎯 Atividade Prática

Agora é sua vez! Você vai implementar uma nova funcionalidade completa seguindo a Vertical Slice Architecture.

### 📋 Tarefa: Implementar "Obter Categoria Por ID"

Atualmente, o projeto possui funcionalidades para:
- ✅ Obter todas as categorias
- ✅ Criar uma categoria

**Você deve implementar:**
- ❌ Obter uma categoria específica por ID

### 🎓 Objetivo de Aprendizagem

Ao completar esta atividade, você será capaz de:
- Criar uma nova slice vertical do zero
- Implementar um Handler com lógica de negócio
- Criar um Endpoint RESTful
- Registrar o Handler e Endpoint corretamente
- Testar a funcionalidade

### 📝 Passo a Passo (SEM CÓDIGO!)

#### Passo 1: Criar a Estrutura de Pastas
1. Navegue até a pasta `Funcionalidades/Categorias/`
2. Crie uma nova pasta chamada `ObterCategoriaPorId`

#### Passo 2: Criar o Handler
1. Dentro da pasta `ObterCategoriaPorId/`, crie um arquivo `ObterCategoriaPorIdHandler.cs`
2. O Handler deve:
   - Receber o `BancoDeDados` via construtor
   - Ter um método `Executar` que recebe um `int id` como parâmetro
   - Buscar a categoria no banco de dados usando o ID
   - Retornar um `CategoriaResposta?` (pode ser nulo se não encontrar)

#### Passo 3: Criar o Record de Resposta
1. No mesmo arquivo do Handler, crie um record `CategoriaResposta`
2. O record deve ter:
   - Id (int)
   - Nome (string)
   - Descricao (string?)

#### Passo 4: Criar o Endpoint
1. Na pasta `ObterCategoriaPorId/`, crie um arquivo `ObterCategoriaPorIdEndpoint.cs`
2. O Endpoint deve:
   - Ser uma classe estática com um método estático de extensão
   - Mapear uma rota GET para `/api/categorias/{id:int}`
   - Receber o ID da rota e o Handler via injeção de dependência
   - Chamar o método `Executar` do Handler
   - Retornar 404 (NotFound) se a categoria não existir
   - Retornar 200 (Ok) com a categoria se ela existir
   - Configurar nome, tags e produces

#### Passo 5: Registrar o Handler
1. Abra o arquivo `Program.cs`
2. Na seção de registro de Handlers de Categorias, adicione:
   - Registro do novo Handler no container de DI

#### Passo 6: Mapear o Endpoint
1. Ainda no `Program.cs`
2. Na seção de mapeamento de endpoints de Categorias, adicione:
   - Chamada ao método de extensão que mapeia o endpoint

#### Passo 7: Testar
1. Execute a aplicação
2. Acesse o Swagger
3. Teste o novo endpoint:
   - Tente obter uma categoria que existe (ID 1, 2 ou 3)
   - Tente obter uma categoria que não existe (ID 999)
4. Verifique se os status codes estão corretos

### ✅ Critérios de Aceitação

Sua implementação está correta se:

1. ✅ A estrutura de pastas segue o padrão: `Funcionalidades/Categorias/ObterCategoriaPorId/`
2. ✅ Existe um Handler com lógica para buscar categoria por ID
3. ✅ Existe um Endpoint configurado corretamente
4. ✅ O Handler está registrado no container de DI
5. ✅ O Endpoint está mapeado no Program.cs
6. ✅ A aplicação compila sem erros
7. ✅ O endpoint retorna 200 com os dados quando a categoria existe
8. ✅ O endpoint retorna 404 quando a categoria não existe


### 💡 Dicas

- 📖 Consulte a implementação de `ObterProdutoPorId` como referência
- 🔍 Preste atenção aos namespaces
- 🧪 Teste cada passo antes de avançar
- ❓ Se tiver dúvidas, revise a seção "Explorando o Código"
- 🎯 Mantenha o foco: uma slice resolve UM problema apenas

### 🚫 O que NÃO fazer

- ❌ Não crie camadas de serviço ou repositório
- ❌ Não reutilize Handlers de outras funcionalidades
- ❌ Não adicione lógica em múltiplos lugares
- ❌ Não crie abstrações desnecessárias

### 🎁 Desafio Extra (Opcional)

Se você terminou a atividade principal e quer mais desafios:

1. **Implementar "Atualizar Categoria"**
   - Criar slice completa para PUT `/api/categorias/{id}`
   - Incluir validações

2. **Implementar "Excluir Categoria"**
   - Criar slice completa para DELETE `/api/categorias/{id}`
   - Verificar se existem produtos usando a categoria antes de excluir

3. **Adicionar Paginação**
   - Modificar "Obter Todos os Produtos" para suportar paginação
   - Adicionar parâmetros `pagina` e `tamanhoPagina`

---

## 📦 Entrega

### Como entregar sua atividade:

1. **Faça um Fork deste repositório**
   - Clique no botão "Fork" no GitHub
   - Isso criará uma cópia do repositório em sua conta

2. **Clone seu fork**
   ```bash
   git clone <url-do-seu-fork>
   cd 08-PraticaVerticalSlice
   ```

3. **Crie uma branch para sua implementação**
   ```bash
   git checkout -b minha-implementacao
   ```

4. **Implemente a funcionalidade**
   - Siga o passo a passo da atividade
   - Faça commits com mensagens descritivas
   ```bash
   git add .
   git commit -m "Implementa ObterCategoriaPorId"
   ```
5. **Demonstre a sua funcionalidae implementada**
   - Tira uma cópia da tela do scalar com a sua funcionalidade implementada (vide *Guia de Testes*)
   

7. **Envie para seu fork**
   ```bash
   git push origin minha-implementacao
   ```

8. **Submeta o link do seu repositório na atividade atribuída no TEAMS**
   - Envie o link do seu fork no GitHub
   - Formato: `https://github.com/seu-usuario/08-PraticaVerticalSlice`
   - Certifique-se de que o repositório está público
   - Envie uma cópia de tela do scalar mostrando a sua funcionaliade implementada




### Checklist de Entrega:

- [ ] Fork do repositório criado
- [ ] Funcionalidade "ObterCategoriaPorId" implementada
- [ ] Código compila sem erros
- [ ] Aplicação executa corretamente
- [ ] Endpoint testado via Swagger
- [ ] Commits com mensagens descritivas
- [ ] Código comentado em português
- [ ] README lido completamente
- [ ] Link do repositório enviado
- [ ] Cópia da tela mostrando a sua funcionalidade implementada

---

## 🗄️ Sobre o Banco de Dados

Este projeto usa **Entity Framework Core** com banco de dados **em memória** por padrão.

### Banco de Dados em Memória (Padrão)

```csharp
builder.Services.AddDbContext<BancoDeDados>(opcoes =>
    opcoes.UseInMemoryDatabase("BancoDeDadosVerticalSlice"));
```

**Vantagens:**
- ✅ Não precisa instalar nada
- ✅ Perfeito para aprendizado e testes
- ✅ Rápido e simples

**Desvantagens:**
- ❌ Os dados são perdidos quando a aplicação é fechada
- ❌ Não é para uso em produção

### Dados Iniciais

O banco é populado automaticamente com dados de exemplo:

**Categorias:**
- ID 1: Eletrônicos
- ID 2: Livros
- ID 3: Roupas

**Produtos:**
- ID 1: Notebook Dell (Categoria: Eletrônicos)
- ID 2: Clean Code (Categoria: Livros)

### Alternativa: SQLite (Comentado no Código)

Se quiser persistir os dados entre execuções, descomente no `Program.cs`:

```csharp
// builder.Services.AddDbContext<BancoDeDados>(opcoes =>
//     opcoes.UseSqlite("Data Source=aprendizado.db"));
```

Isso criará um arquivo `aprendizado.db` com os dados persistidos.

---

## 📚 Referências Adicionais

### Artigos e Documentação

1. **Vertical Slice Architecture**
   - [Vertical Slice Architecture - Jimmy Bogard](https://www.jimmybogard.com/vertical-slice-architecture/)
   - [Organize Code by Feature](https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/august/asp-net-single-page-applications-build-modern-responsive-web-apps-with-asp-net)

2. **ASP.NET Core**
   - [Documentação Oficial ASP.NET Core](https://docs.microsoft.com/pt-br/aspnet/core/)
   - [Minimal APIs](https://docs.microsoft.com/pt-br/aspnet/core/fundamentals/minimal-apis)
   - [Dependency Injection](https://docs.microsoft.com/pt-br/aspnet/core/fundamentals/dependency-injection)

3. **Entity Framework Core**
   - [Documentação EF Core](https://docs.microsoft.com/pt-br/ef/core/)
   - [InMemory Database Provider](https://docs.microsoft.com/pt-br/ef/core/providers/in-memory/)

### Vídeos (Inglês)

- [SOLID Principles](https://www.youtube.com/results?search_query=solid+principles+c%23)
- [Clean Architecture vs Vertical Slice](https://www.youtube.com/results?search_query=vertical+slice+architecture)

### Comparações com Outras Arquiteturas

| Aspecto | Camadas | Clean Architecture | Vertical Slice |
|---------|---------|-------------------|----------------|
| Organização | Por camada técnica | Por camada + domínio | Por funcionalidade |
| Complexidade | Baixa-Média | Alta | Baixa |
| Escalabilidade | Média | Alta | Alta |
| Curva de Aprendizado | Baixa | Alta | Média |
| Ideal para | Apps tradicionais | Sistemas complexos | Apps modernas |

### Padrões Relacionados

- **CQRS (Command Query Responsibility Segregation):** Separa operações de leitura e escrita
- **MediatR:** Biblioteca que implementa o padrão Mediator, muito usado com Vertical Slices
- **Feature Folders:** Organização de código por funcionalidade (similar ao Vertical Slice)

### Livros Recomendados

1. **"Clean Code" - Robert C. Martin**
   - Princípios de código limpo e boas práticas

2. **"Domain-Driven Design" - Eric Evans**
   - Design orientado a domínio

3. **"Patterns of Enterprise Application Architecture" - Martin Fowler**
   - Padrões de arquitetura de software

---

## 🤔 Perguntas Frequentes

### 1. Quando devo criar uma nova slice?

Sempre que você tiver uma **nova funcionalidade ou caso de uso**. Exemplos:
- Obter lista de produtos
- Criar um produto
- Atualizar um produto
- Excluir um produto

Cada uma dessas é uma slice separada!

### 2. Posso reutilizar código entre slices?

**Sim, mas com cuidado!** Se você tem lógica que é verdadeiramente compartilhada:
- Crie uma classe utilitária na pasta `Infraestrutura/`
- Ou crie um projeto separado de "Shared"

**Mas não force:** Se só duas slices usam algo, pode duplicar o código. Duplicação é melhor que acoplamento desnecessário.

### 3. E as validações, não devo ter uma camada separada?

**Não!** Na Vertical Slice Architecture:
- Validações ficam no Handler
- Se forem muito complexas, crie uma classe de validação **dentro da pasta da slice**

Exemplo:
```
CriarProduto/
├── CriarProdutoHandler.cs
├── CriarProdutoEndpoint.cs
└── CriarProdutoValidador.cs    ← Validador específico desta slice
```

### 4. Como fica o banco de dados?

O **contexto do banco de dados** fica na pasta `Infraestrutura/` pois é compartilhado.
Mas as **queries específicas** ficam em cada Handler.

### 5. Isso não gera muita duplicação de código?

Um pouco, sim. Mas lembre-se:
- **Duplicação não é o pior problema**
- **Acoplamento é pior que duplicação**
- É melhor ter código duplicado que seja fácil de mudar, do que código compartilhado que é difícil de mudar

### 6. Posso usar essa arquitetura com MediatR?

**Sim!** MediatR funciona muito bem com Vertical Slices:
- Cada Handler vira um `IRequestHandler`
- Os Endpoints disparam Commands/Queries via MediatR
- Fica ainda mais desacoplado

### 7. Como testo uma slice?

Cada slice pode ser testada independentemente:
- **Testes de Unidade:** Teste o Handler isoladamente
- **Testes de Integração:** Teste o Endpoint + Handler + Banco
- **Testes E2E:** Teste a API completa

### 8. Posso usar com Entity Framework tradicional (não Core)?

Sim, mas Entity Framework Core é recomendado para projetos .NET modernos.

---

## 🎯 Conclusão

Parabéns por chegar até aqui! Você aprendeu:

✅ O que é Vertical Slice Architecture
✅ Como ela difere de arquiteturas tradicionais em camadas
✅ Como organizar código por funcionalidades
✅ Como criar Handlers e Endpoints
✅ Como trabalhar com Entity Framework Core
✅ Como implementar uma funcionalidade completa do zero

### Próximos Passos

1. ✅ Complete a atividade prática
2. 📚 Explore as referências adicionais
3. 🚀 Tente os desafios extras
4. 💡 Aplique esses conceitos em seus próprios projetos
5. 🤝 Compartilhe seu aprendizado com outros desenvolvedores

### Feedback

Este é um projeto de aprendizado. Se você encontrou erros, tem sugestões de melhoria ou dúvidas:
- Abra uma Issue no GitHub
- Entre em contato com seu instrutor
- Contribua com melhorias via Pull Request

---

## 📄 Licença

Este projeto é de código aberto e está disponível para fins educacionais.

---

**Bom aprendizado! 🚀**
