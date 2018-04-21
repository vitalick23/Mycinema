using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cinema.Models;

namespace Cinema.Service
{
    public class FilmService
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public List<Films> GetFilms()
        {
            return _db.Films.ToList();
        }
    }
}