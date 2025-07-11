using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using INTEGRACION_SANJUAN.Data;
using INTEGRACION_SANJUAN.Models;

namespace INTEGRACION_SANJUAN.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CursosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /api/cursos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Curso>>> GetCursos()
        {
            return await _context.Cursos
                .Include(c => c.Docente)
                .ToListAsync();
        }

        // GET: /api/cursos/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Curso>> GetCursoPorId(int id)
        {
            var curso = await _context.Cursos
                .Include(c => c.Docente)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (curso == null)
                return NotFound();

            return curso;
        }

        // GET: /api/cursos/ciclo/2024-I
        [HttpGet("ciclo/{ciclo}")]
        public async Task<ActionResult<IEnumerable<Curso>>> GetCursosPorCiclo(string ciclo)
        {
            var cursos = await _context.Cursos
                .Where(c => c.ciclo == ciclo)
                .Include(c => c.Docente)
                .ToListAsync();

            return cursos;
        }

        // POST: /api/cursos
        [HttpPost]
        public async Task<ActionResult<Curso>> CrearCurso(Curso curso)
        {
            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCursoPorId), new { id = curso.Id }, curso);
        }

        // PUT: /api/cursos/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> ActualizarCurso(int id, Curso curso)
        {
            if (id != curso.Id)
                return BadRequest();

            _context.Entry(curso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CursoExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: /api/cursos/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> EliminarCurso(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
                return NotFound();

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CursoExists(int id)
        {
            return _context.Cursos.Any(c => c.Id == id);
        }
    }
}
