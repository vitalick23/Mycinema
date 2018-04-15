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
    [RequireHttps]
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public int pageSize = 5;

        public ActionResult SessionView(int page = 1, string serh="")
        {
            // var model = db.Sessions;
            SessionListViewModels model = new SessionListViewModels
            {
                Session = db.Sessions
                  .OrderBy(x => x.ReleaseDate)
                  .OrderBy(x => x.ReleaseTime)
                  .Where(x => x.Film.Name.Contains(serh))
                  //.Where(x => x.ReleaseDate.Day >= DateTime.Now.Day)
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

        [HttpGet]
        public ActionResult Index(int page = 1, string serh = "")
        {
            // var model = db.Sessions;
            FilmsListViewModel model = new FilmsListViewModel
            {
                Films = db.Films
                  .OrderBy(x=>x.Name)
                  .Where(x => x.Name.Contains(serh))
                  .Skip((page - 1) * pageSize)
                  .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = db.Sessions.Count()
                }
            };
            
            return View(model);

        }
        [HttpPost]
        public ActionResult Index(string serh,int page = 1 )
        {
            FilmsListViewModel model = new FilmsListViewModel
            {
                Films = db.Films
                  .OrderBy(x => x.Name)
                  .Where(x => x.Name.Contains(serh))
                  .Skip((page - 1) * pageSize)
                  .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = db.Sessions.Count()
                }
            };

            return PartialView(model);

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
            ViewBag.Date = DateTime.Now.Date;
       //     ViewBag.Min = new SelectList(new string[]{"00","15","30","45"},"Min");
       //     ViewBag.Clock = new SelectList(new string[] { "00", "01", "02", "03","04", "05", "06", "07", "08", "09",
        //       "10","11","12","13","14","15","16","17","19","20","21","22","23",},"clock");
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateSession(Session model)
        {

            if (!ModelState.IsValid)
            {
                model.Film = db.Films.Find(model.IdFilms);
                //model.ReleaseDate = DateTime.Now.AddHours(2); 
                db.Entry(model).State = EntityState.Added;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    InfoMessenger infoMessenger = new InfoMessenger
                    {
                        title = "Seseion created fail. Check data",
                        information = " |"
                    };
                    return RedirectToAction("InfoMessenger", "Home", infoMessenger);
                }
                InfoMessenger models = new InfoMessenger
                {
                    title = "Session created successfully",
                    information = "Movie name: " + model.Film.Name +
                    "|Genre: " + model.Film.genre +
                    "|Date of release: " + model.ReleaseDate.Day+"."
                    +model.ReleaseDate.Month + "."+model.ReleaseDate.Year + 
                    " " + model.ReleaseTime.TimeOfDay +
                    "|Price: " + model.Price + "|"

                };

                return RedirectToAction("InfoMessenger", "Home",models);

            }
            return View(model);

        }


        [Authorize(Roles = "admin")]
        public ActionResult CreateFilms()
        {
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
                InfoMessenger models = new InfoMessenger
                {
                    title = "Movie created successfully",
                    information = "Movie name: " + model.Name +
                          "|Genre: " + model.genre + "|"
                };

                return RedirectToAction("InfoMessenger", "Home",models);

            }
            return View(model);
                
        }


        [Authorize(Roles = "admin")]
        public ActionResult DeleteFilm()
        {
            SelectList Films = new SelectList(db.Films, "IdFilms", "Name");
            ViewBag.Films = Films;
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteFilm(Films model, HttpPostedFileBase uploadImage)
        {
            
            db = new ApplicationDbContext();
            List<Session> ses = db.Sessions.Where(x => x.IdFilms == model.idFilms).ToList();
            if (ses.Count > 0)
            {
                db.Dispose();
                InfoMessenger models = new InfoMessenger
                {
                    title = "Delete Film " + model.Name + " fail, Film have sessions",
                    information = " |"
                };
                return RedirectToAction("InfoMessenger", "Home", models);
            }
            else
            {
                Films film = db.Films.Find(model.idFilms);
                db.Films.Remove(film);
                db.SaveChanges();
                db.Dispose();

                InfoMessenger models = new InfoMessenger
                {
                    title = "Delete Film " + model.Name + " successfully",
                    information = "  | "
                };
                return RedirectToAction("InfoMessenger", "Home", models);
            } 
        }

        [Authorize(Roles = "admin")]
        public ActionResult EditFilm()
        {
            SelectList Films = new SelectList(db.Films, "IdFilms", "Name");
            ViewBag.Films = Films;
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditFilm(FilmUpdate model, HttpPostedFileBase uploadImage)
        {
            try
            {
                Films film = db.Films.Where(x => x.Name == model.Film.Name).First();
                film.genre = model.Film.genre;
                film.Name = model.NewName;
                db.Entry(film).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception e)
            {
                return RedirectToAction("InfoMessenger", "Home", new InfoMessenger { title = "Error", information =" |" });
            }
                
            
            InfoMessenger models = new InfoMessenger
            {
                title = "Update Film " + model.NewName + " successfully",
                information = "  | "
            };
            return RedirectToAction("InfoMessenger", "Home", models);
        }

        [Authorize(Roles = "admin")]
        public ActionResult DeleteSession(int idSession)
        {
            try
            {
                Session mod = db.Sessions.Find(idSession);
            }
            catch(Exception e)
            {
                return RedirectToAction("InfoMessenger", "Home", new InfoMessenger { title ="Error",information =" |" });
            }
            Session model = db.Sessions.Find(idSession);
            List<Basket> basket = db.Baskets.Where(x => x.IdSession == model.IdSession).ToList();
            if (basket.Count > 0)
            {
                TimerModule tm = new TimerModule();
                string mes = "Sorry the session for the movie " + db.Films.Find(model.IdFilms).Name +
                    " Time: " + model.ReleaseDate.Day + "." +
                    model.ReleaseDate.Month + "." +
                    model.ReleaseDate.Year + " " +
                    model.ReleaseTime.TimeOfDay +
                    " was canceled";
                foreach (var b in basket)
                {
                    tm.Send(db.Users.Find(b.IdUsers).Email, db.Users.Find(b.IdUsers).UserName, mes);
                    db.Baskets.Remove(b);
                }
            }
            db.Sessions.Remove(model);
            db.SaveChanges();
            db.Dispose();
            InfoMessenger info = new InfoMessenger
            {
                title = "Session Delete",
                information = " |"
            };
            return RedirectToAction("InfoMessenger", "Home",info);
        }

        [Authorize(Roles = "admin")]
        public ActionResult EditSession(int idSession)
        {
            Session model = db.Sessions.Find(idSession);
            return View(model);
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditSession(Session model)
        {
            bool flag = false;
            string mess = "", mesCansel = "";
            try
            {
                Session ses = db.Sessions.Find(model.IdSession);
                ses.Film = db.Films.Find(ses.IdFilms);
                mesCansel = "sorry, due to the change in the number of seats, your movie " +
                    ses.Film.Name +" Time: "+
                    model.ReleaseDate.Day + "." +
                    ses.ReleaseDate.Month + "." +
                    ses.ReleaseDate.Year + "." +
                    ses.ReleaseTime.TimeOfDay +
                    " tickets are canceled";
                if (ses.ReleaseDate != model.ReleaseDate || ses.ReleaseTime != model.ReleaseTime)
                {
                    flag = true;
                    mess = "In connection with changing the session for the film "
                            + ses.Film.Name + " Time: " + ses.ReleaseDate.Day + "." +
                            +ses.ReleaseDate.Month + "." +
                            +ses.ReleaseDate.Year + "." +
                            ses.ReleaseTime.TimeOfDay + " Moved to: " +
                            model.ReleaseDate.Day + "." +
                            model.ReleaseDate.Month + "." +
                            model.ReleaseDate.Year + "." +
                            model.ReleaseTime.TimeOfDay;
                }

                ses.Price = model.Price;
                ses.CountTicket = model.CountTicket;
                ses.ReleaseDate = model.ReleaseDate;
                ses.ReleaseTime = model.ReleaseTime;
                db.Entry(ses).State = EntityState.Modified;

                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception ex)
            {
                InfoMessenger infoMessenger = new InfoMessenger
                {
                    title = "Seseion created fail. Check data",
                    information = " |"
                };
                return RedirectToAction("InfoMessenger", "Home", infoMessenger);
            }
            db = new ApplicationDbContext();
            List<Basket> basket = db.Baskets.Where(x => x.IdSession == model.IdSession).ToList();
            int BuyTicket = 0;
            if (basket.Count > 0)
            {
                for (int i = 0; i < basket.Count(); i++)
                {
                    BuyTicket += basket.ElementAt(i).CoutTicket;
                }
            }
            TimerModule tm = new TimerModule();
            if (flag )
            {
                if (basket.Count > 0)
                {

                    if (BuyTicket > model.CountTicket)
                    {
                        for (int i = 0; i < basket.Count; i++)
                        {
                            var user = db.Users.Find(basket.ElementAt(i).IdUsers);
                            tm.Send(user.Email, user.UserName, mesCansel);
                            db.Baskets.Remove(basket.ElementAt(i));

                        }
                        db.SaveChanges();
                    }
                    else
                    {
                        Session ses = db.Sessions.Find(model.IdSession);
                        ses.CountTicket -= BuyTicket;
                        ses.Film = db.Films.Find(ses.IdFilms);
                        db.Entry(ses).State = EntityState.Modified;
                        db.SaveChanges();

                        for (int i = 0; i < basket.Count; i++)
                        {
                            var user = db.Users.Find(basket.ElementAt(i).IdUsers);
                            tm.Send(user.Email, user.UserName, mess);
                        }
                    }
                }
            }
            else
            {
                if(basket.Count() > 0)
                {
                    if (BuyTicket > model.CountTicket)
                    {
                        for (int i = 0; i < basket.Count; i++)
                        {
                            var user = db.Users.Find(basket.ElementAt(i).IdUsers);
                            tm.Send(user.Email, user.UserName, mesCansel);
                            db.Baskets.Remove(basket.ElementAt(i));

                        }
                        db.SaveChanges();

                    }
                    else
                    {
                        Session ses = db.Sessions.Find(model.IdSession);
                        ses.CountTicket -= BuyTicket;
                        ses.Film = db.Films.Find(ses.IdFilms);
                        db.Entry(ses).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                }

            }
           
            db.Dispose();
            //return View();
            InfoMessenger info = new InfoMessenger
            {
                title = "Sucсess sesion update",
                information = " |"
            };
            return RedirectToAction("InfoMessenger", "Home",info);
        }

        

        [Authorize]
        public ActionResult InfoMessenger(InfoMessenger model)
        {
             return View(model);
        }
         
    }
}