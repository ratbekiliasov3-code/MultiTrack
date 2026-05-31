using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiTrack.Models
{
    public class Gorev
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Baslik { get; set; } = string.Empty;

        [Required]
        public DateTime Tarih { get; set; }

        public bool IsCompleted { get; set; } = false;

        // Foreign Key İlişkisi
        [Required]
        public int KullaniciId { get; set; }

        [ForeignKey("KullaniciId")]
        public Kullanici? Kullanici { get; set; }
    }
}