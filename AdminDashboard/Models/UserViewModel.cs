﻿namespace AdminDashboard.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public bool isConfirmed { get; set; } 
    }
}
