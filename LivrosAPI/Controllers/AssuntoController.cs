using LivrosAPI.Data;
using LivrosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LivrosAPI.Controllers
{
    [Route("livrosapi/[controller]")]
    [ApiController]
    public class AssuntoController : ControllerBase
    {
        private readonly LivrosDbContext dbContext;
        public AssuntoController(LivrosDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await dbContext.Assunto.ToListAsync());
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

                var result = await dbContext.Assunto.FindAsync(cod);
                return result != null ? Ok(result) : NotFound("Registro não encontrado");
            }
            catch (Exception ex)
            {
                return Problem(title: "Ocorreu um erro ao consultar lista de registros.",
                               detail: ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert(Assunto assunto)
        {
            try
            {
                var validationResponse = ValidateBeforeSave(assunto);
                if (validationResponse is not OkResult)
                    return validationResponse;

                var result = await dbContext.Assunto.AddAsync(assunto);
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
        public async Task<IActionResult> Update(Assunto assunto)
        {
            try
            {
                var validationResponse = ValidateBeforeSave(assunto);
                if (validationResponse is not OkResult)
                    return validationResponse;

                dbContext.Assunto.Update(assunto);
                await dbContext.SaveChangesAsync();

                return Ok(assunto);
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

                var entity = await dbContext.Assunto.FindAsync(cod);
                if (entity == null)
                    return BadRequest("Codigo informado para exclusão não encontrado.");
                else
                    dbContext.Assunto.Remove(entity);
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
        public IActionResult ValidateBeforeSave(Assunto assunto)
        {
            try
            {
                if (string.IsNullOrEmpty(assunto.Descricao))
                    return BadRequest("Informe a descrição do assunto.");
                else if (dbContext.Assunto.Any(a => a.CodAs != assunto.CodAs && a.Descricao.ToLower() == assunto.Descricao.ToLower()))
                    return Conflict("O assunto informado já foi cadastrado.");
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
