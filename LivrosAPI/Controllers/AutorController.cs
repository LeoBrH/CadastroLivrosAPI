using LivrosAPI.Data;
using LivrosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LivrosAPI.Controllers
{
    [Route("livrosapi/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        private readonly LivrosDbContext dbContext;
        public AutorController(LivrosDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await dbContext.Autor.ToListAsync());
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
                    return BadRequest("Informe o código para realizar a busca");

                var result = await dbContext.Autor.FindAsync(cod);
                return result != null ? Ok(result) : NotFound("Registro não encontrado");
            }
            catch (Exception ex)
            {
                return Problem(title: "Ocorreu um erro ao consultar lista de registros.",
                               detail: ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert(Autor autor)
        {
            try
            {
                var validationResponse = ValidateBeforeSave(autor);
                if (validationResponse is not OkResult)
                    return validationResponse;

                var result = await dbContext.Autor.AddAsync(autor);
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
        public async Task<IActionResult> Update(Autor autor)
        {
            try
            {
                var validationResponse = ValidateBeforeSave(autor);
                if (validationResponse is not OkResult)
                    return validationResponse;

                dbContext.Autor.Update(autor);
                await dbContext.SaveChangesAsync();

                return Ok(autor);
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

                var entity = await dbContext.Autor.FindAsync(cod);
                if (entity == null)
                    return BadRequest("Codigo informado para exclusão não encontrado.");
                else
                    dbContext.Autor.Remove(entity);
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
        public IActionResult ValidateBeforeSave(Autor autor)
        {
            try
            {
                if (string.IsNullOrEmpty(autor.Nome))
                    return BadRequest("Informe o nome do autor.");
                else if (dbContext.Autor.Any(a => a.CodAu != autor.CodAu && a.Nome.ToLower() == autor.Nome.ToLower()))
                    return Conflict("O autor informado já foi cadastrado.");
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
