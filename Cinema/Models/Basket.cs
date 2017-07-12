﻿using System;
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

        [ForeignKey("Film")]
        [Column(Order = 0)]
        public int IdFilms { get; set; }

        //[ForeignKey("user")]
        //[Column(Order = 0)]
        public int IdUsers { get; set; }

        public int CoutTicket { get; set; }

        public DateTime DateBuy { get; set; }

        public int Status { get; set; }

        public Films Film { get; set; }

      //  public ApplicationUser user { get; set; }
    }
}