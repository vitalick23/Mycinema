using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cinema.Models;

namespace Cinema.Controllers
{
    public class ApiFilmController : Controller
    {
        // GET: ApiFilm
        public List<Films> Get()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.Films.ToList();
        }
    }
}