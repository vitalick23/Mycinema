using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Cinema.Models
{
    public class CreateSessionList
    {
        public Session session { get; set; }
        [DataType(DataType.Date)]
        public DateTime date { get; set; }
        [DataType(DataType.Time)]
        public DateTime time { get; set; }
        public string NameFilm { get; set; }
    }
}