using System.Collections.Generic;
using System.Linq;

namespace Cinema.Models
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Session session, int quantity)
        {
            CartLine line = lineCollection
                .Where(g => g.session.IdSession== session.IdSession)
                .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    session = session,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Session session)
        {
            lineCollection.RemoveAll(l => l.session.IdSession == session.IdSession);
        }

        public double ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.session.Price * e.Quantity);

        }
        public void Clear()
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }

    public class CartLine
    {
        public Session session { get; set; }
        public int Quantity { get; set; }
    }
}