using LivrosAPI.Data;
using LivrosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LivrosAPI.Controllers
{
    [Route("livrosapi/[controller]")]
    public class Livro_AutorController : ControllerBase
    {
        private readonly LivrosDbContext dbContext;
        public Livro_AutorController(LivrosDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("{cod}")]
        public async Task<IActionResult> GetByCod(int cod)
        {
            try
            {
                if (cod < 1)
                    return BadRequest("Informe o código para realizar a busca");


                var result = await dbContext.Livro_Autor.Where(l => l.Livro_Codl == cod).ToListAsync();
                return result != null ? Ok(result) : NotFound("Registro não encontrado");
            }
            catch (Exception ex)
            {
                return Problem(title: "Ocorreu um erro ao consultar lista de registros.",
                               detail: ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert(Livro_Autor livro_autor)
        {
            try
            {
                var validationResponse = ValidateBeforeSave(livro_autor);
                if (validationResponse is not OkResult)
                    return validationResponse;

                var result = await dbContext.Livro_Autor.AddAsync(livro_autor);
                await dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(Insert), result.Entity);
            }
            catch (Exception ex)
            {
                return Problem(title: "Ocorreu um erro ao salvar o registro.",
                               detail: ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int codl, int codAu)
        {
            try
            {
                if (codl < 1 || codAu < 1)
                    return BadRequest("Informe os códigos para excluir.");

                var entity = await dbContext.Livro_Autor.FindAsync(codl, codAu);
                if (entity == null)
                    return BadRequest("Codigo informado para exclusão não encontrado.");
                else
                    dbContext.Livro_Autor.Remove(entity);
                await dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(title: "Ocorreu um erro ao consultar lista de registros.",
                               detail: ex.Message);
            }
        }

        [NonAction]
        public IActionResult ValidateBeforeSave(Livro_Autor livro_autor)
        {
            try
            {
                if (dbContext.Livro_Autor.Any(l => l.Livro_Codl == livro_autor.Livro_Codl && l.Autor_CodAu == livro_autor.Autor_CodAu))
                    return Conflict("O autor já foi vinculado à esse livro.");
                else
                    return Ok();
            }
            catch (Exception ex)
            {
                return Problem(title: "Ocorreu um erro durante a validação dos dados",
                               detail: ex.Message);
            }
        }
    }
}
