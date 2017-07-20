using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cinema.Models
{
    public class Basket
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Sessions")]
        [Column(Order = 1)]
        public int IdSession { get; set; }

        //[ForeignKey("user")]
        //[Column(Order = 0)]
        public string IdUsers { get; set; }

        public int CoutTicket { get; set; }

        public DateTime DateBuy { get; set; }
        
        public Session Sessions { get; set; }

      //  public ApplicationUser user { get; set; }
    }
}