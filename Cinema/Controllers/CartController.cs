using System.Linq;
using System.Web.Mvc;
using Cinema.Models;
using System.Collections.Generic;

namespace Cinema.Controllers
{
    public interface ISessionRepository
    {
        IEnumerable<Session> Session { get; }
    }

    public class CartController : Controller
    {    ApplicationDbContext db = new ApplicationDbContext();
        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = GetCart(),
                ReturnUrl = returnUrl
            });
        }

        private ISessionRepository repository;
        public CartController()
        {
            //repository = repo;
        }

        public RedirectToRouteResult AddToCart(int sessionId, string returnUrl, int quantity)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            Session session = db.Sessions.Find(sessionId);
            session.Film = db.Films.Find(session.IdFilms);
            if (session != null)
            {
                GetCart().AddItem(session, quantity);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(int SessionId, string returnUrl)
        {
            Session Session = repository.Session
                .FirstOrDefault(g => g.IdSession == SessionId);

            if (Session != null)
            {
                GetCart().RemoveLine(Session);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
    }
}