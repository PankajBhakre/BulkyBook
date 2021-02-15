﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BulkyBook.Models
{
    public class CoverType
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name ="Cover Type")]
        public string NAME { get; set; }

    }
}
