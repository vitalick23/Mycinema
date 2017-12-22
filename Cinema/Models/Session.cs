using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cinema.HtmlHelpers;

namespace Cinema.Models
{
    public class Session
    {
        [Key]
        public int IdSession { get; set; }

        [ForeignKey("Film")]
        [Column(Order = 0)]
        public int IdFilms { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        [ValidatonDate("ReleaseDate","Old Date")]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime ReleaseTime { get; set; }

        [Range(0,2000, ErrorMessage= "Недопустимое количество билетов")]
        [Required]
        public int CountTicket { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Недопустимая цена")]
        [Required]
        public double Price { get; set; }

        [Required]
        public Films Film { get; set; }

    }
}