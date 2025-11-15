using Microsoft.AspNetCore.Mvc;

namespace AprendizadoVerticalSlice.Funcionalidades.Categorias.ObterCategoriaPorId;

/// <summary>
/// Endpoint para obter uma categoria específica por ID
/// </summary>
public static class ObterCategoriaPorIdEndpoint
{
    /// <summary>
    /// Mapeia o endpoint GET /api/categorias/{id:int}
    /// </summary>
    public static IEndpointRouteBuilder MapObterCategoriaPorId(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/categorias/{id:int}", async (
            [FromRoute] int id,
            [FromServices] ObterCategoriaPorIdHandler handler) =>
        {
            var categoria = await handler.Executar(id);

            if (categoria is null)
            {
                return Results.NotFound(new { mensagem = $"Categoria com ID {id} não encontrada." });
            }

            return Results.Ok(categoria);
        })
        .WithName("ObterCategoriaPorId")
        .WithTags("Categorias")
        .Produces<CategoriaResposta>(200)
        .Produces(404);

        return endpoints;
    }
}
