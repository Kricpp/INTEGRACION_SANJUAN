using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using INTEGRACION_SANJUAN.Data;
using INTEGRACION_SANJUAN.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace INTEGRACION_SANJUAN.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MesaPartesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public MesaPartesController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/mesapartes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MesaPartes>>> GetRegistros()
        {
            return await _context.MesaPartes.ToListAsync();
        }

        // GET: api/mesapartes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MesaPartes>> GetRegistro(int id)
        {
            var registro = await _context.MesaPartes.FindAsync(id);
            if (registro == null)
                return NotFound();

            return registro;
        }

        // POST: api/mesapartes
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<ActionResult<MesaPartes>> PostRegistro([FromForm] MesaPartes model, IFormFile archivo, List<IFormFile> anexos)
        {
            // Subida del documento principal
            if (archivo != null && archivo.Length > 0)
            {
                var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsPath);
                var archivoPath = Path.Combine(uploadsPath, archivo.FileName);
                using (var stream = new FileStream(archivoPath, FileMode.Create))
                {
                    await archivo.CopyToAsync(stream);
                }

                model.DocumentoPrincipal = "/uploads/" + archivo.FileName;
            }

            // Subida de anexos
            for (int i = 0; i < anexos.Count && i < 3; i++)
            {
                if (anexos[i] != null && anexos[i].Length > 0)
                {
                    var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadsPath);
                    var anexoPath = Path.Combine(uploadsPath, anexos[i].FileName);
                    using (var stream = new FileStream(anexoPath, FileMode.Create))
                    {
                        await anexos[i].CopyToAsync(stream);
                    }

                    string rutaAnexo = "/uploads/" + anexos[i].FileName;
                    if (i == 0) model.Anexo1 = rutaAnexo;
                    if (i == 1) model.Anexo2 = rutaAnexo;
                    if (i == 2) model.Anexo3 = rutaAnexo;
                }
            }

            _context.MesaPartes.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRegistro), new { id = model.Id }, model);
        }

        // PUT: api/mesapartes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegistro(int id, MesaPartes model)
        {
            if (id != model.Id)
                return BadRequest();

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.MesaPartes.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/mesapartes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistro(int id)
        {
            var registro = await _context.MesaPartes.FindAsync(id);
            if (registro == null)
                return NotFound();

            _context.MesaPartes.Remove(registro);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

