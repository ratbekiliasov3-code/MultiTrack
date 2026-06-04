using Microsoft.EntityFrameworkCore;

namespace MultiTrack.Models
{
    public class MultiTrackDbContext : DbContext
    {
        public MultiTrackDbContext(DbContextOptions<MultiTrackDbContext> options)
            : base(options)
        {
        }

        public DbSet<Kullanici> Kullanici { get; set; }

        public DbSet<Gorev> Gorevler { get; set; }
        public DbSet<SuTakibi> Sular { get; set; }
        public DbSet<KitapTakip> Kitaplar { get; set; }
        public DbSet<SporAntrenman> Sporlar { get; set; }
        public DbSet<Harcama> Harcamalar { get; set; }
    }
}