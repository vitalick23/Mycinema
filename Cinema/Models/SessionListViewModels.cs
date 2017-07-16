using Cinema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cinema.Models
{
    public class SessionListViewModels
    {
        public IEnumerable<Session> Session { get; set; }
        public PagingInfo PagingInfo { get; set; }

        
    }
}