using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MultiTrack.Models
{
    public class Kullanici
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        // İlişki: Bir kullanıcının birden fazla görevi olabilir
        public ICollection<Gorev>? Gorevler { get; set; }
    }
}