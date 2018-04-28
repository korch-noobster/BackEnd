using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Context;
using BackEnd.Models;
using Microsoft.AspNetCore.Authorization;

namespace BackEnd.Controllers
{
    [Produces("application/json")]
    [Route("api/Articles")]
    
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArticlesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Articles
        [HttpGet]
        public IEnumerable<Article> GetArticles()
        {
            return _context.Articles.Include(p=>p.Images);
        }

        // GET: api/Articles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticle([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var article = await _context.Articles.Include(p=>p.Images).SingleOrDefaultAsync(m => m.Id == id);

            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
        }

        // PUT: api/Articles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticle([FromRoute] int id, [FromBody] ViewModel.Article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != article.Id)
            {
                return BadRequest();
            }

            Article temp = _context.Articles.FirstOrDefault(a => a.Id == article.Id);
            temp.Content = article.Content;
            temp.Title = article.Title;
            foreach (var item in article.Images)
            {
                _context.SetOfImages.Add(new SetOfImages
                {
                    Image = item,
                    Article = new Article { Id = temp.Id }
                });

            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Articles
        [HttpPost]
        public async Task<IActionResult> PostArticle([FromBody] ViewModel.Article article)
        {
            Article temp = _context.Articles.Add(new Article
            {
                Content = article.Content,
                Title = article.Title
            }).Entity;

            foreach (var item in article.Images)
            {
                _context.SetOfImages.Add(new SetOfImages
                {
                    Image = item,
                    Article = new Article { Id = temp.Id }
                });
            }

            await _context.SaveChangesAsync();
            return CreatedAtAction("GetArticle", new { id = article.Id }, article);
        }

        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var article = await _context.Articles.SingleOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return Ok(article);
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
}