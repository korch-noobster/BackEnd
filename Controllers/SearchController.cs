using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BackEnd.Context;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers
{
    [Produces("application/json")]
    [Route("api/Search")]
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

    
        [HttpGet]
        public JsonResult Search([FromQuery]string q)
        {
            Search result = new Search()
            {


                Articles = _context.Articles.Include(p => p.Images)
                    .Where(a => (a.Title.ToLower().Contains(" " + q.ToLower()))
                        || (a.Title.ToLower().StartsWith(q.ToLower())))
                   .ToList(),
                Discounts = _context.Discounts.Where(a => (a.Title.ToLower().Contains(" " + q.ToLower())) || (a.Title.ToLower().StartsWith(q.ToLower()))).ToList(),
                Services = _context.Servicies.Include(p => p.Images).Where(a => (a.Title.ToLower().Contains(" " + q.ToLower())) || (a.Title.ToLower().StartsWith(q.ToLower()))).ToList(),
            };
     
            return Json(result);
        }
    }
}