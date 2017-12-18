using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Cinema.Models
{
    public class CreateSessionList
    {
        public Session Session { get; set; }
        public string NameFilm { get; set; }
    }
}