namespace MultiTrack.Models
{
    public class SporAntrenman
    {
        public int Id { get; set; }
        public string Gun { get; set; } = "";
        public string AntrenmanAdi { get; set; } = "";
        public string KullaniciId { get; set; } = "rad";
        public bool IsCompleted { get; set; }
    }
}