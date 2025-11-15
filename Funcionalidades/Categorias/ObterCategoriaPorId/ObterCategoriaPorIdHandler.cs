using Microsoft.EntityFrameworkCore;
using AprendizadoVerticalSlice.Infraestrutura;

namespace AprendizadoVerticalSlice.Funcionalidades.Categorias.ObterCategoriaPorId;

/// <summary>
/// Resposta contendo os dados de uma categoria
/// </summary>
public record CategoriaResposta(
    int Id,
    string Nome,
    string? Descricao
);

/// <summary>
/// Handler responsável por obter uma categoria específica por ID
/// </summary>
public class ObterCategoriaPorIdHandler
{
    private readonly BancoDeDados _bancoDeDados;

    public ObterCategoriaPorIdHandler(BancoDeDados bancoDeDados)
    {
        _bancoDeDados = bancoDeDados;
    }

    /// <summary>
    /// Executa a busca de uma categoria pelo seu ID.
    /// </summary>
    /// <param name="id">ID da categoria</param>
    /// <returns>CategoriaResposta ou null caso não seja encontrada</returns>
    public async Task<CategoriaResposta?> Executar(int id)
    {
        var categoria = await _bancoDeDados.Categorias
            .Where(c => c.Id == id)
            .Select(c => new CategoriaResposta(
                c.Id,
                c.Nome,
                c.Descricao
            ))
            .FirstOrDefaultAsync();

        return categoria;
    }
}
