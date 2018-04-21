using Cinema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cinema.Service;

namespace Cinema.Controllers
{
    [AllowAnonymous]
    public class MyController : ApiController
    {
        // GET api/<controller>
        [AllowAnonymous]
        public List<Films> Get()
        {
            return new FilmService().GetFilms();
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}