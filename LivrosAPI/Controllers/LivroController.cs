using LivrosAPI.Data;
using LivrosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.WebForms;

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

        #region CRUD
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
        #endregion

        #region Relacionamentos

        [HttpGet("{codl}/LivroAutor")]
        public async Task<IActionResult> GetLivro_AutorByCod(int codl)
        {
            try
            {
                if (codl < 1)
                    return BadRequest("Informe o código do livro para pesquisar.");

                var result = await dbContext.Livro_Autor
                                            .Where(la => la.Livro_Codl == codl)
                                            .Select(la => la.Autor)
                                            .ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(title: "Ocorreu um erro ao consultar lista de registros.",
                               detail: ex.Message);
            }
        }

        [HttpPost("{codl}/LivroAutor/{codAu}")]
        public async Task<IActionResult> InsertLivro_Autor(int Codl, int CodAu)
        {
            if (Codl < 1 || CodAu < 1)
                return BadRequest("Informe os códigos para inserir.");

            var livro = await dbContext.Livro.FindAsync(Codl);
            var autor = await dbContext.Autor.FindAsync(CodAu);

            if (livro == null || autor == null)
            {
                return NotFound();
            }

            var livroAutor = new Livro_Autor
            {
                Livro_Codl = Codl,
                Autor_CodAu = CodAu
            };

            await dbContext.Livro_Autor.AddAsync(livroAutor);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{codl}/LivroAutor/{codAu}")]
        public async Task<IActionResult> DeleteLivro_Autor(int Codl, int CodAu)
        {
            if (Codl < 1 || CodAu < 1)
                return BadRequest("Informe os códigos para excluir.");

            var entity = await dbContext.Livro_Autor.FindAsync(Codl, CodAu);
            if (entity == null)
                return NotFound("Vinculo para exclusão não encontrado.");
            else
                dbContext.Livro_Autor.Remove(entity);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{codl}/LivroAssunto")]
        public async Task<IActionResult> GetLivro_AssuntoByCod(int codl)
        {
            try
            {
                if (codl < 1)
                    return BadRequest("Informe o código do livro para pesquisar.");

                var result = await dbContext.Livro_Assunto
                                            .Where(la => la.Livro_Codl == codl)
                                            .Select(la => la.Assunto)
                                            .ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(title: "Ocorreu um erro ao consultar lista de registros.",
                               detail: ex.Message);
            }
        }

        [HttpPost("{codl}/LivroAssunto/{codAs}")]
        public async Task<IActionResult> InserirLivro_Assunto(int Codl, int CodAs)
        {
            if (Codl < 1 || CodAs < 1)
                return BadRequest("Informe os códigos para inserir.");

            var livro = await dbContext.Livro.FindAsync(Codl);
            var assunto = await dbContext.Assunto.FindAsync(CodAs);

            if (livro == null || assunto == null)
                return NotFound();

            var livroAssunto = new Livro_Assunto
            {
                Livro_Codl = Codl,
                Assunto_codAs = CodAs
            };

            await dbContext.Livro_Assunto.AddAsync(livroAssunto);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{codl}/LivroAssunto/{codAs}")]
        public async Task<IActionResult> DeleteLivro_Assunto(int Codl, int CodAs)
        {
            if (Codl < 1 || CodAs < 1)
                return BadRequest("Informe os códigos para excluir.");

            var entity = await dbContext.Livro_Assunto.FindAsync(Codl, CodAs);
            if (entity == null)
                return NotFound("Vinculo para exclusão não encontrado.");
            else
                dbContext.Livro_Assunto.Remove(entity);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{codl}/LivroValor")]
        public async Task<IActionResult> GetLivro_ValorByCod(int codl)
        {
            try
            {
                if (codl < 1)
                    return BadRequest("Informe o código do livro para pesquisar.");

                var result = await dbContext.Livro_Valor
                                            .Where(lv => lv.Livro_Codl == codl)
                                            .Select(lv => new { lv.Livro_Codl, lv.Forma_Compra_CodFC, FormaCompra = lv.Forma_Compra, lv.Valor })
                                            .ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(title: "Ocorreu um erro ao consultar lista de registros.",
                               detail: ex.Message);
            }
        }

        [HttpPost("{Codl}/LivroValor/{CodFC}")]
        public async Task<IActionResult> InserirLivro_Valor(int Codl, int CodFC, [FromBody] double valor)
        {
            if (Codl < 1 || CodFC < 1)
                return BadRequest("Informe os códigos para inserir.");

            var livro = await dbContext.Livro.FindAsync(Codl);
            var formaCompra = await dbContext.Forma_Compra.FindAsync(CodFC);

            if (livro == null || formaCompra == null)
            {
                return NotFound();
            }

            var livroValor = new Livro_Valor
            {
                Livro_Codl = Codl,
                Forma_Compra_CodFC = CodFC,
                Valor = valor
            };

            await dbContext.Livro_Valor.AddAsync(livroValor);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{codl}/LivroValor/{CodFC}")]
        public async Task<IActionResult> DeleteLivro_Valor(int Codl, int CodFC)
        {
            if (Codl < 1 || CodFC < 1)
                return BadRequest("Informe os códigos para excluir.");

            var entity = await dbContext.Livro_Valor.FindAsync(Codl, CodFC);
            if (entity == null)
                return NotFound("Vinculo para exclusão não encontrado.");
            else
                dbContext.Livro_Valor.Remove(entity);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
        #endregion

        #region Relatório
        [HttpGet("Relatorio")]
        public async Task<IActionResult> GetRelatorioLivrosAutor()
        {
            try
            {
                var data = await dbContext.View_LivrosPorAutor.ToListAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Problem(title: "Ocorreu um erro ao consultar o relatório.",
                               detail: ex.Message);
            }
        }
        #endregion
    }
}