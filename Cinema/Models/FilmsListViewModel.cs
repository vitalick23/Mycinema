using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cinema.Models
{
    public class FilmsListViewModel
    {
        public IEnumerable<Films> Films { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string Serch { get; set; }
    }
}