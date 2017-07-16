using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cinema.Models;
using System.Threading.Tasks;
using System.Data.Entity;
using System.IO;

namespace Cinema.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public int pageSize = 2;
        public ActionResult Index(int page = 1)
        {
            // var model = db.Sessions;
            SessionListViewModels model = new SessionListViewModels
            {
                Session = db.Sessions
                  .OrderBy(game => game.ReleaseDate)
                  .Skip((page - 1) * pageSize)
                  .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = db.Sessions.Count()
                }
            };
            //var tmp = db.Sessions.ToList();
            for (int i = 0; i < model.Session.Count(); i++)
            {
                Films t = db.Films.Find(model.Session.ElementAt(i).IdFilms);
            }

            return View(model);

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult CreateSession()
        {
            // ViewBag.Message = "Your contact page.";
            SelectList Films = new SelectList(db.Films, "IdFilms", "Name");
            ViewBag.Films = Films;
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateSession(Session model)
        {

            if (!ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Added;
                model.ReleaseDate = DateTime.Now;                
                db.SaveChanges();
                return RedirectToAction("CreateSession", "Home");

            }
            return View(model);

        }


        [Authorize(Roles = "admin")]
        public ActionResult CreateFilms()
        {
           // ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateFilms(Films model, HttpPostedFileBase uploadImage)
        {
            if (!ModelState.IsValid)
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                model.Image = imageData;
                db.Entry(model).State = EntityState.Added;
                db.SaveChanges();
                return RedirectToAction("CreateFilms", "Home");

            }
            return View(model);
                
        }

        public RedirectToRouteResult AddToSession(Session ss)
        {
          //  Session game = repository.Games
            //    .FirstOrDefault(g => g.GameId == gameId);

            if (ss != null)
            {
               // GetCart().AddItem(game, 1);
            }
            return RedirectToAction("Index");
        }


    }
}