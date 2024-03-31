﻿using karg.DAL.Models.Enums;

namespace karg.DAL.Models
{
    public class Rescuer
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public RescuerRole Role { get; set; } = RescuerRole.Employee;
        public string? Current_Password { get; set; }
        public string? Previous_Password { get; set; }
    }
}
