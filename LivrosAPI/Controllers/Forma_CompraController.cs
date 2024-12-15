using LivrosAPI.Data;
using LivrosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LivrosAPI.Controllers
{
    [Route("livrosapi/[controller]")]
    [ApiController]
    public class Forma_CompraController : ControllerBase
    {
        private readonly LivrosDbContext dbContext;
        public Forma_CompraController(LivrosDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await dbContext.Forma_Compra.ToListAsync());
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


                var result = await dbContext.Forma_Compra.FindAsync(cod);
                return result != null ? Ok(result) : NotFound("Registro não encontrado");
            }
            catch (Exception ex)
            {
                return Problem(title: "Ocorreu um erro ao consultar lista de registros.",
                               detail: ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert(Forma_Compra forma_compra)
        {
            try
            {
                var validationResponse = ValidateBeforeSave(forma_compra);
                if (validationResponse is not OkResult)
                    return validationResponse;

                var result = await dbContext.Forma_Compra.AddAsync(forma_compra);
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
        public async Task<IActionResult> Update(Forma_Compra forma_compra)
        {
            try
            {
                var validationResponse = ValidateBeforeSave(forma_compra);
                if (validationResponse is not OkResult)
                    return validationResponse;

                dbContext.Forma_Compra.Update(forma_compra);
                await dbContext.SaveChangesAsync();

                return Ok(forma_compra);
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

                var entity = await dbContext.Forma_Compra.FindAsync(cod);
                if (entity == null)
                    return BadRequest("Codigo informado para exclusão não encontrado.");
                else
                    dbContext.Forma_Compra.Remove(entity);
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
        public IActionResult ValidateBeforeSave(Forma_Compra forma_compra)
        {
            try
            {
                if (string.IsNullOrEmpty(forma_compra.Descricao))
                    return BadRequest("Informe a descrição da forma de compra.");
                else if (dbContext.Forma_Compra.Any(l => l.CodFC != forma_compra.CodFC && l.Descricao.ToLower() == forma_compra.Descricao.ToLower()))
                    return Conflict("A forma de compra informada já foi cadastrada.");
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
