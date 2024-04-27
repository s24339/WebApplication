﻿using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.DTOs
{
    public class UpdateAnimal
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
    
        [MaxLength(200)]
        public string? Description { get; set; } 
    
        [Required]
        [MaxLength(200)]
        public string Category { get; set; } = string.Empty;
    
        [Required]
        [MaxLength(200)]
        public string Area { get; set; } = string.Empty;
    }
}