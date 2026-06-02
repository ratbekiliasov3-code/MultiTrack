namespace MultiTrack.Models
{
    public class SporAntrenman
    {
        public int Id { get; set; }
        public string Gun { get; set; } = "";
        public string AntrenmanAdi { get; set; } = "";
       public string KullaniciId { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}