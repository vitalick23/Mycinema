using System.Linq;
using System.Web.Mvc;
using Cinema.Models;
using System.Collections.Generic;
using System.Data.Entity;

namespace Cinema.Controllers
{
    public interface ISessionRepository
    {
        IEnumerable<Session> Session { get; }
    }

    public class CartController : Controller
    {    ApplicationDbContext db = new ApplicationDbContext();
        public ViewResult Index(Cart cart,string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        private ISessionRepository repository;
        public CartController()
        {
            //repository = repo;
        }
        [Authorize]
        public RedirectToRouteResult AddToCart(Cart cart,int sessionId, string returnUrl, int quantity = 1)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            Session session = db.Sessions.Find(sessionId);
            session.Film = db.Films.Find(session.IdFilms);
            if (session != null)
            {
                for(int i = 0; i < cart.Lines.Count();i++)
                {
                    if (cart.Lines.ElementAt(i).session.IdSession == session.IdSession)
                       if (cart.Lines.ElementAt(i).Quantity + quantity > session.CountTicket)
                            quantity = session.CountTicket - cart.Lines.ElementAt(i).Quantity;
                        else break;
                }
                cart.AddItem(session, quantity);
                
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int SessionId, string returnUrl)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            Session session = db.Sessions.Find(SessionId);
            session.Film = db.Films.Find(session.IdFilms);
            if (Session != null)
            {
                cart.RemoveLine(session);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }
        public RedirectToRouteResult Buy(Cart cart,string returnUrl,string userName)
        {
                        string info = " ";
            ApplicationDbContext db = new ApplicationDbContext();
            var user = db.Users.Where(x => x.UserName == userName);
            
            if (user.First().EmailConfirmed == true)
            {
                
                int count = cart.Lines.Count();
                if (count == 0) return RedirectToAction("Index", "Cart"); ;
                    for (int i = 0; i < count; i++)
                {
                
                    if (db.Sessions.Find(cart.Lines.ElementAt(i).session.IdSession).CountTicket < cart.Lines.ElementAt(i).Quantity)
                    {
                        cart.Lines.ElementAt(i).session.Film = db.Films.Find(cart.Lines.ElementAt(i).session.IdFilms);
                        info += "Sorry, his number of tickets does not exist: Film " +
                            cart.Lines.ElementAt(i).session.Film.Name + " Time " + cart.Lines.ElementAt(i).session.ReleaseDate.Day + "." +
                            cart.Lines.ElementAt(i).session.ReleaseDate.Month + "." +
                            cart.Lines.ElementAt(i).session.ReleaseDate.Year + " " +
                            cart.Lines.ElementAt(i).session.ReleaseTime.TimeOfDay + "|";
                    }
                    else
                    {
                        Basket basket = new Basket()
                        {
                            Sessions = cart.Lines.ElementAt(i).session,
                            CoutTicket = cart.Lines.ElementAt(i).Quantity,
                            DateBuy = System.DateTime.Now,
                            IdSession = cart.Lines.ElementAt(i).session.IdSession,
                            IdUsers = user.First().Id

                        };
                        Session session = db.Sessions.Find(cart.Lines.ElementAt(i).session.IdSession);
                        session.CountTicket -= basket.CoutTicket;
                        db.Entry(session).State = EntityState.Modified;
                        session.Film = db.Films.Find(session.IdFilms);
                        
                        basket.Sessions = db.Sessions.Find(basket.IdSession);
                        basket.Sessions.Film = db.Films.Find(basket.Sessions.IdFilms);
                        db.Baskets.Add(basket);
                    
                        info += " Buy ticket on the Film " +
                            cart.Lines.ElementAt(i).session.Film.Name + " Time " + cart.Lines.ElementAt(i).session.ReleaseDate.Day + "." +
                            cart.Lines.ElementAt(i).session.ReleaseDate.Month + "." +
                            cart.Lines.ElementAt(i).session.ReleaseDate.Year + " " +
                            cart.Lines.ElementAt(i).session.ReleaseTime.TimeOfDay + "|";
                        
                    }
                }
                for (int i = 0; i < count; i++)
                    RemoveFromCart(cart, cart.Lines.ElementAt(0).session.IdSession, returnUrl);
            }
            else
            {
               
                db.Dispose();
                return RedirectToAction("DisplayEmail", "Account");
            }
            db.SaveChanges();
            db.Dispose();
            
            InfoMessenger model = new InfoMessenger
            {
                title = "Information",
                information = info
            };
            return RedirectToAction("InfoMessenger", "Home",model);
        }

    }
}