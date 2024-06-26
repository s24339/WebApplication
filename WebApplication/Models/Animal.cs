﻿namespace WebApplication.Models
{
    public class Animal
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } 
        public string Category { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
    }
}