﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cinema.Models
{

        public class Films
        {
            [System.ComponentModel.DataAnnotations.Key]
            public int idFilms { get; set; }

            public string genre { get; set; }

            public string Name { get; set; }

            public byte[] Image { get; set; } 
        }
    
}