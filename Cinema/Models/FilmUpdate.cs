using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cinema.Models
{
    public class FilmUpdate
    {
        public Films Film { get; set; }
        public string NewName { get; set; }
    }
}