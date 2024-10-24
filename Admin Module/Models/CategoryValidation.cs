﻿using System.ComponentModel.DataAnnotations;

namespace Admin_Module.Models
{
    public class CategoryValidation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide category name")]
        public string Name { get; set; }


        [Display(Name = "Active")]
        public bool isActive { get; set; }
        
        public string Description { get; set; }
    }
}
