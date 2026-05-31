namespace MultiTrack.Models
{
    public class KitapTakip
    {
        public int Id { get; set; }
        public string KitapAdi { get; set; } = string.Empty;
        public int ToplamSayfa { get; set; }
        public int KalinanSayfa { get; set; }
        public string KullaniciId { get; set; } = string.Empty;
    }
}
