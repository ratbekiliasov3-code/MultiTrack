using System;

namespace MultiTrack.Models
{
    public class SuTakibi
    {
        public int Id { get; set; }
        public DateTime Tarih { get; set; }
        public double Miktar { get; set; }
        public string KullaniciId { get; set; } = string.Empty;
    }
}