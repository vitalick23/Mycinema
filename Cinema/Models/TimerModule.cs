using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web;

namespace Cinema.Models
{
    public class TimerModule : IHttpModule
    {
        static Timer timer;
        long interval = 3600000; //60 мин
        static object synclock = new object();
        static bool sent = false;

        public void Init(HttpApplication app)
        {
            timer = new Timer(new TimerCallback(SendEmail), null, 0, interval);
        }

        private void SendEmail(object obj)
        {
            lock (synclock)
            {
                if (!sent)
                {
                    ApplicationDbContext db = new ApplicationDbContext();
                    List<Session> session = db.Sessions.ToList<Session>();
                    db.Dispose();
                    foreach (var s in session)
                    {
                        if ((s.ReleaseTime - DateTime.Now).Hours < 1 && (s.ReleaseTime - DateTime.Now).Hours >= 0)
                        {
                            db = new ApplicationDbContext();
                            List<Basket> basket = db.Baskets.Where(x => x.IdSession == s.IdSession).ToList();

                            if (basket != null)
                            {
                                foreach (var b in basket)
                                {
                                    ApplicationUser user = db.Users.Find(b.IdUsers);
                                    Films film = db.Films.Find(s.IdFilms);
                                    string mailstr = "We remind you bought a ticket for today's movie (" + film.Name + ") session. " +
                                    "Time: " + s.ReleaseTime.TimeOfDay + " Number of ticket(s): " + b.CoutTicket;
                                    Send(user.Email, user.UserName, mailstr);
                                }
                            }
                            db.Dispose();
                        }
                        if(s.ReleaseDate.Date < DateTime.Now.Date && s.ReleaseTime.TimeOfDay < DateTime.Now.TimeOfDay )
                        {
                            db = new ApplicationDbContext();
                            List<Basket> basket = db.Baskets.Where(x => x.IdSession == s.IdSession).ToList();
                            foreach(var b in basket)
                            {
                                db.Baskets.Remove(b);
                            }
                            Session ses = db.Sessions.Find(s.IdSession);
                            db.Sessions.Remove(ses);
                            db.SaveChanges();
                            db.Dispose();
                        }
                    }
                    sent = true;
                }
                else
                {
                    sent = false;
                }
            }
        }
        public void Dispose()
        { }

        public void Send(string email, string userName, string mailstr)
        {
            var from = new MailAddress("shegod9i2@gmail.com", "Cinema");
            const string from_psvd = "s03an92qaz";
            var to = new MailAddress(email, userName);
            string sub = "Reminder";
            SmtpClient smtpcl = new SmtpClient();
            smtpcl.Host = "smtp.gmail.com";
            smtpcl.Port = 587;

            smtpcl.DeliveryMethod = SmtpDeliveryMethod.Network;

            smtpcl.UseDefaultCredentials = false;
            smtpcl.Credentials = new NetworkCredential(from.Address, from_psvd);
            //smtpcl.UseDefaultCredentials = true;
            smtpcl.EnableSsl = true;
            smtpcl.Timeout = 60000;


            MailMessage mail = new MailMessage(from, to);
            mail.Subject = sub;
            mail.Body = mailstr;

            smtpcl.Send(mail);
        }
    }
}