using LivrosAPI.Data;
using LivrosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LivrosAPI.Controllers
{
    [Route("livrosapi/[controller]")]
    [ApiController]
    public class LivroController : ControllerBase
    {
        private readonly LivrosDbContext dbContext;
        public LivroController(LivrosDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await dbContext.Livro.ToListAsync());
            }
            catch (Exception ex)
            {
                return Problem(title: "Ocorreu um erro ao consultar lista de registros.",
                               detail: ex.Message);
            }
        }

        [HttpGet("{cod}")]
        public async Task<IActionResult> GetByCod(int cod)
        {
            try
            {
                if (cod < 1)
                    return BadRequest("Informe um código para pesquisar.");

                var result = await dbContext.Livro.FindAsync(cod);
                return result != null ? Ok(result) : NotFound("Registro não encontrado");
            }
            catch (Exception ex)
            {
                return Problem(title: "Ocorreu um erro ao consultar lista de registros.",
                               detail: ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert(Livro livro)
        {
            try
            {
                var validationResponse = ValidateBeforeSave(livro);
                if (validationResponse is not OkResult)
                    return validationResponse;

                var result = await dbContext.Livro.AddAsync(livro);
                await dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(Insert), result.Entity);
            }
            catch (Exception ex)
            {
                return Problem(title: "Ocorreu um erro ao salvar o registro.",
                               detail: ex.Message);
            }
        }

        [HttpPut("{cod}")]
        public async Task<IActionResult> Update(int cod, Livro livro)
        {
            try
            {
                var validationResponse = ValidateBeforeSave(livro);
                if (validationResponse is not OkResult)
                    return validationResponse;

                dbContext.Livro.Update(livro);
                await dbContext.SaveChangesAsync();

                return Ok(livro);
            }
            catch (Exception ex)
            {
                return Problem(title: "Ocorreu um erro ao atualizar o registro.",
                               detail: ex.Message);
            }
        }

        [HttpDelete("{cod}")]
        public async Task<IActionResult> Delete(int cod)
        {
            try
            {
                if (cod < 1)
                    return BadRequest("Informe o código para excluir.");

                var entity = await dbContext.Livro.FindAsync(cod);
                if (entity == null)
                    return NotFound("Codigo informado para exclusão não encontrado.");
                else
                    dbContext.Livro.Remove(entity);
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
        public IActionResult ValidateBeforeSave(Livro livro)
        {
            try
            {
                if (livro.Edicao < 1)
                    return BadRequest("Numero de edição inválida.");

                else if (dbContext.Livro.Any(l => l.Codl != livro.Codl
                                             && l.Titulo == livro.Titulo
                                             && l.Editora == livro.Editora
                                             && l.Edicao == livro.Edicao
                                             && l.AnoPublicacao == livro.AnoPublicacao))
                    return Conflict("O livro informado já foi cadastrado.");

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
