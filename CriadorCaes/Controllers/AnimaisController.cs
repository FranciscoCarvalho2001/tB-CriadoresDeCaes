using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CriadorCaes.Data;
using CriadorCaes.Models;

namespace CriadorCaes.Controllers
{
    public class AnimaisController : Controller
    {
        private readonly ApplicationDbContext _bd;

        public AnimaisController(ApplicationDbContext context)
        {
            _bd = context;
        }

        // GET: Animais
        public async Task<IActionResult> Index()
        {
            /*procurar, na base de dados, a lista dos animais existentes
             * SELECT *
             * FROM Animais a INNER JOIN Criadore c ON a.CriadoresFK = c.id
             *                INNER JOIN Racas r ON a.RacaFK = r.id
             * WHERE 
             */
            var listaAnimais = _bd.Animais.Include(a => a.Criadores).Include(a => a.Raca);

            //invoca a view 
            return View(await listaAnimais.ToListAsync());
        }

        // GET: Animais/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _bd.Animais == null)
            {
                return NotFound();
            }

            var animais = await _bd.Animais
                .Include(a => a.Criadores)
                .Include(a => a.Raca)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animais == null)
            {
                return NotFound();
            }

            return View(animais);
        }

        // GET: Animais/Create
        /// <summary>
        /// Criar as condições para a view de criação de um animal
        /// ser enviada para o browser
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            //prepara os dados a serem apresentados na dropdown
            ViewData["CriadorFK"] = new SelectList(_bd.Criadores, "Id", "Email");
            ViewData["RacaFk"] = new SelectList(_bd.Racas, "Id", "Nome");
            return View();
        }

        // POST: Animais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// reage aos dados fornecidos pelo browser
        /// </summary>
        /// <param name="animal">dados do novo animal</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,DataNascimento,DataCompra,Sexo,NumLOP,RacaFk,CriadorFK")] Animais animal)
        {
            //se os dados recebidos respeitarem as classes
            //os dados podem ser adicionados
            if (ModelState.IsValid)
            {
                try
                {
                    //adicionar os dados á BD
                    _bd.Add(animal);
                    //COMIT da ação anterior
                    await _bd.SaveChangesAsync();
                    //devolver o controlo da app para a página de ínicio
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocorreu um erro com a adição dos dados do(a)" + animal.Nome);
                    //throw;
                }
                
            }
            //preparar estes dados, para quando os dados a introduzir na BD não estão bons
            ViewData["CriadorFK"] = new SelectList(_bd.Criadores, "Id", "Nome", animal.CriadorFK);
            ViewData["RacaFk"] = new SelectList(_bd.Racas, "Id", "Nome", animal.RacaFk);
            //devolver á view
            return View(animal);
        }

        // GET: Animais/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _bd.Animais == null)
            {
                return NotFound();
            }

            var animais = await _bd.Animais.FindAsync(id);
            if (animais == null)
            {
                return NotFound();
            }
            ViewData["CriadorFK"] = new SelectList(_bd.Criadores, "Id", "Email", animais.CriadorFK);
            ViewData["RacaFk"] = new SelectList(_bd.Racas, "Id", "Id", animais.RacaFk);
            return View(animais);
        }

        // POST: Animais/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,DataNascimento,DataCompra,Sexo,NumLOP,RacaFk,CriadorFK")] Animais animais, IFormFile imagemAnimal)
        {
            if (id != animais.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _bd.Update(animais);
                    await _bd.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimaisExists(animais.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CriadorFK"] = new SelectList(_bd.Criadores, "Id", "Email", animais.CriadorFK);
            ViewData["RacaFk"] = new SelectList(_bd.Racas, "Id", "Id", animais.RacaFk);
            return View(animais);
        }

        // GET: Animais/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _bd.Animais == null)
            {
                return NotFound();
            }

            var animais = await _bd.Animais
                .Include(a => a.Criadores)
                .Include(a => a.Raca)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animais == null)
            {
                return NotFound();
            }

            return View(animais);
        }

        // POST: Animais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_bd.Animais == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Animais'  is null.");
            }
            var animais = await _bd.Animais.FindAsync(id);
            if (animais != null)
            {
                _bd.Animais.Remove(animais);
            }
            
            await _bd.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnimaisExists(int id)
        {
          return _bd.Animais.Any(e => e.Id == id);
        }
    }
}
