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
            ApplicationDbContext db = new ApplicationDbContext();
             
            for (int i = 0; i < cart.Lines.Count(); i++)
            {
                if (db.Sessions.Find(cart.Lines.ElementAt(i).session.IdSession).CountTicket < cart.Lines.ElementAt(i).Quantity)
                {

                }
                else
                {
                    var user = db.Users.Where(x => x.UserName.StartsWith(userName));
                    Basket basket = new Basket()
                    {
                        Sessions = cart.Lines.ElementAt(i).session,
                        CoutTicket = cart.Lines.ElementAt(i).Quantity,
                        DateBuy = System.DateTime.Now,
                        IdSession = cart.Lines.ElementAt(i).session.IdSession,
                        IdUsers = user.First().Id

                    };
                    db.Dispose();
                    db = new ApplicationDbContext();
                    Session session = db.Sessions.Find(cart.Lines.ElementAt(i).session.IdSession);
                    session.CountTicket -= basket.CoutTicket;
                    db.Entry(session).State = EntityState.Modified;
                    session.Film = db.Films.Find(session.IdFilms);
                    db.SaveChanges();
                    db.Dispose();
                    db = new ApplicationDbContext();
                    basket.Sessions = db.Sessions.Find(basket.IdSession);
                    basket.Sessions.Film = db.Films.Find(basket.Sessions.IdFilms);
                    db.Baskets.Add(basket);
                    db.SaveChanges();

                    RemoveFromCart(cart, cart.Lines.ElementAt(i).session.IdSession, returnUrl);
                }
            }
            InfoMessenger model = new InfoMessenger
            {
                title = "Purchase completed",
                information = " "
            };  
            
            return RedirectToAction("InfoMessenger", "Home",model);
        }

    }
}