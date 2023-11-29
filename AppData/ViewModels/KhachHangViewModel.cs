using AppData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels
{
    public class KhachHangViewModel
    {
        public Guid Id { get; set; }
        
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required, StringLength(30)]
        public string? Name { get; set; }
        [Required]
        public string? SDT { get; set; }
        [Required, StringLength(8)]
        public string Password { get; set; }

    }
}
