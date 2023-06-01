using CriadorCaes.Data;
using CriadorCaes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CriadorCaes.Controllers
{
    public class AnimaisController : Controller
    {
        private readonly ApplicationDbContext _bd;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public AnimaisController(
           ApplicationDbContext context,
           IWebHostEnvironment webHostEnvironment
           )
        {
            _bd = context;
            _webHostEnvironment = webHostEnvironment;
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
            /* procurar, na base de dados, o animal com ID igual ao parâmetro fornecido
            * SELECT *
            * FROM Animais a INNER JOIN Criadores c ON a.CriadorFK = c.Id
            *                INNER JOIN Racas r ON a.RacaFK = r.Id
            * WHERE a.Id = id 
            */
            var animais = await _bd.Animais
                .Include(a => a.Criadores)
                .Include(a => a.Raca)
                .Include(a=>a.ListaFotografias)
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
        public async Task<IActionResult> Create([Bind("Id,Nome,DataNascimento,DataCompra,Sexo,NumLOP,RacaFk,CriadorFK")] Animais animal, IFormFile imagemAnimal)
        {
            //vars. auxiliares
            String nomeFoto = "";
            bool existeFoto = false;
            //avaliar se temos condições para tentar adicionar o animal
            //testa se a Raça do animal != 0 e Criador =/=0
            if (animal.RacaFk == 0)
            {
                //não escolhi Raça
                ModelState.AddModelError("", "É obrigatório escolher uma raça");

            }
            else
            {
                //escolhi uma Raça.Mas, escolhi um Criador?
                if (animal.CriadorFK == 0)
                {
                    //não escolhi criador
                    ModelState.AddModelError("", "É obrigatório escolher um criador");
                }
                else
                {
                    //se cheguei aqui, escolhi Raça e Criador
                    //será que escolhi Imagem? vamos avaliar...
                    if (imagemAnimal == null)
                    {
                        //o utilizador não fez upload de uma imagem
                        //vamos adicionar uma imagem predefinida ao animal
                        animal.ListaFotografias.Add(new Fotografias
                        {
                            Data = DateTime.Now,
                            Local = "NoImage",
                            NomeFicheiro = "catty.jpg"
                        });
                    }
                    else
                    {
                        //há ficheiro. Mas será que é uma imagem?
                        if (imagemAnimal.ContentType != "image/jpeg" &&
                            imagemAnimal.ContentType != "image/png")
                        {
                            //o ficheiro carregado não é uma imagem
                            //o que fazer?
                            //vamos fazer o mesmo que quando o utilizador não fornece uma imagem
                            animal.ListaFotografias.Add(
                                new Fotografias
                                {
                                    Data = DateTime.Now,
                                    Local = "NoImage",
                                    NomeFicheiro = "catty.jpg"
                                });
                        }
                        else
                        {
                            //há imagem!!!
                            //determinar o nome da imagem
                            Guid g = Guid.NewGuid();
                            nomeFoto = g.ToString();
                            //obter a extensão do ficheiro
                            string extensaoNomeFoto = Path.GetExtension(imagemAnimal.FileName).ToLower();
                            nomeFoto += extensaoNomeFoto;
                            animal.ListaFotografias.Add(
                                new Fotografias
                                {
                                    Data = DateTime.Now,
                                    Local = "",
                                    NomeFicheiro = nomeFoto
                                });
                            //informar a aplicação que há um ficheiro
                            //(imagem) para guardar no disco rigido
                            existeFoto = true;

                        }//if (imagemAnimal.ContentType!="image/jpeg" ||imagemAnimal.ContentType != "image/png")
                    }//if (imagemAnimal == null)
                }//if(animal.RacaFk == 0)

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

                        // se cheguei aqui, já foram guardados os dados 
                        //do animal na BD. Já posso guardar a imagem
                        //no disco rígido do servidor
                        if (existeFoto)
                        {
                            //determinar onde guardar a imagem
                            string nomeLocImgs = _webHostEnvironment.WebRootPath;
                            nomeLocImgs = Path.Combine(nomeLocImgs, "Imagens");
                            //e a pasta onde se pretende guardar a imagem existe?
                            if (!Directory.Exists(nomeLocImgs))
                            {
                                Directory.CreateDirectory(nomeLocImgs);
                            }
                            //informar o servidor do nome do ficheiro

                            string nomeFicheiro = Path.Combine(nomeLocImgs, nomeFoto);
                            //guardar o ficheiro
                            using var stream = new FileStream(nomeFicheiro,FileMode.Create);
                            await imagemAnimal.CopyToAsync(stream);
                        }

                        //devolver o controlo da app para a página de ínicio
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", "Ocorreu um erro com a adição dos dados do(a)" + animal.Nome);
                        //throw;
                    }

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
