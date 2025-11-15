using Microsoft.EntityFrameworkCore;
using AprendizadoVerticalSlice.Infraestrutura;
using Scalar.AspNetCore;

// Importações para os Handlers de Produtos
using AprendizadoVerticalSlice.Funcionalidades.Produtos.ObterTodosProdutos;
using AprendizadoVerticalSlice.Funcionalidades.Produtos.ObterProdutoPorId;
using AprendizadoVerticalSlice.Funcionalidades.Produtos.CriarProduto;
using AprendizadoVerticalSlice.Funcionalidades.Produtos.AtualizarProduto;
using AprendizadoVerticalSlice.Funcionalidades.Produtos.ExcluirProduto;

// Importações para os Handlers de Categorias
using AprendizadoVerticalSlice.Funcionalidades.Categorias.ObterTodasCategorias;
using AprendizadoVerticalSlice.Funcionalidades.Categorias.CriarCategoria;
using AprendizadoVerticalSlice.Funcionalidades.Categorias.ObterCategoriaPorId;

var builder = WebApplication.CreateBuilder(args);

// Registro do Handler de ObterCategoriaPorId
builder.Services.AddScoped<ObterCategoriaPorIdHandler>();

// Configuração do Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Configuração do banco de dados em memória
// Para usar SQLite, comente a linha abaixo e descomente a linha do SQLite
builder.Services.AddDbContext<BancoDeDados>(opcoes =>
    opcoes.UseInMemoryDatabase("BancoDeDadosVerticalSlice"));

// Configuração do banco de dados SQLite (comentada por padrão)
// builder.Services.AddDbContext<BancoDeDados>(opcoes =>
//     opcoes.UseSqlite("Data Source=aprendizado.db"));

// Registro dos Handlers de Produtos
// Na arquitetura Vertical Slice, cada handler é registrado individualmente
builder.Services.AddScoped<ObterTodosProdutosHandler>();
builder.Services.AddScoped<ObterProdutoPorIdHandler>();
builder.Services.AddScoped<CriarProdutoHandler>();
builder.Services.AddScoped<AtualizarProdutoHandler>();
builder.Services.AddScoped<ExcluirProdutoHandler>();

// Registro dos Handlers de Categorias
builder.Services.AddScoped<ObterTodasCategoriasHandler>();
builder.Services.AddScoped<CriarCategoriaHandler>();

var app = builder.Build();

// Inicializa o banco de dados com dados iniciais
using (var escopo = app.Services.CreateScope())
{
    var bancoDeDados = escopo.ServiceProvider.GetRequiredService<BancoDeDados>();
    bancoDeDados.Database.EnsureCreated();
}

// Configuração do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// Mapeamento dos endpoints de Produtos
// Cada endpoint é mapeado através de um método de extensão
app.MapObterTodosProdutos();
app.MapObterProdutoPorId();
app.MapCriarProduto();
app.MapAtualizarProduto();
app.MapExcluirProduto();

// Mapeamento dos endpoints de Categorias
app.MapObterTodasCategorias();
app.MapCriarCategoria();
app.MapObterCategoriaPorId();

app.Run();
