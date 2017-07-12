using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cinema.Models
{
    public class Session
    {
        [Key]
        public int IdSession { get; set; }

        [ForeignKey("Film")]
        [Column(Order = 0)]
        public int IdFilms { get; set; }

        public DateTime ReleaseDate { get; set; }

        public int CountTicket { get; set; }

        public double Price { get; set; }

        public Films Film { get; set; }


    }
}