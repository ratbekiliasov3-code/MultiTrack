using System;
using System.ComponentModel.DataAnnotations;

namespace MultiTrack.Models
{
    public class Harcama
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Tarih { get; set; }

        [Required]
        public string Aciklama { get; set; } = string.Empty;

        public double Tutar { get; set; }

        [Required]
        public string KullaniciId { get; set; } = string.Empty;
    }
}
