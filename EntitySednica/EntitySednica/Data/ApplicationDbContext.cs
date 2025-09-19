using Microsoft.EntityFrameworkCore;
using EntitySednica.Models;

namespace EntitySednica.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Pozicija> Pozicije { get; set; }
        public DbSet<Stranka> Stranke { get; set; }
        public DbSet<Lice> Lica { get; set; }
        public DbSet<Saziv> Sazivi { get; set; }
        public DbSet<Mandat> Mandati { get; set; }
        public DbSet<TipZasedanja> TipoviZasedanja { get; set; }
        public DbSet<Zasedanje> Zasedanja { get; set; }
        public DbSet<Sednica> Sednice { get; set; }
        public DbSet<DnevniRed> DnevniRedovi { get; set; }
        public DbSet<Pitanje> Pitanja { get; set; }
        public DbSet<Glasanje> Glasnja { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and constraints
            modelBuilder.Entity<Lice>()
                .HasOne(l => l.Pozicija)
                .WithMany()
                .HasForeignKey(l => l.PozicijaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Lice>()
                .HasOne(l => l.Stranka)
                .WithMany()
                .HasForeignKey(l => l.StrankaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Mandat>()
                .HasOne(m => m.Lice)
                .WithMany()
                .HasForeignKey(m => m.LiceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Mandat>()
                .HasOne(m => m.Saziv)
                .WithMany()
                .HasForeignKey(m => m.SazivId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Mandat>()
                .HasOne(m => m.Stranka)
                .WithMany()
                .HasForeignKey(m => m.StrankaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Zasedanje>()
                .HasOne(z => z.TipZasedanja)
                .WithMany()
                .HasForeignKey(z => z.TipZasedanjaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Zasedanje>()
                .HasOne(z => z.Saziv)
                .WithMany()
                .HasForeignKey(z => z.SazivId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Sednica>()
                .HasOne(s => s.Zasedanje)
                .WithMany()
                .HasForeignKey(s => s.ZasedanjeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DnevniRed>()
                .HasOne(d => d.Sednica)
                .WithMany()
                .HasForeignKey(d => d.SednicaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pitanje>()
                .HasOne(p => p.DnevniRed)
                .WithMany()
                .HasForeignKey(p => p.DnevniRedId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Glasanje>()
                .HasOne(g => g.Pitanje)
                .WithMany()
                .HasForeignKey(g => g.PitanjeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Glasanje>()
                .HasOne(g => g.Lice)
                .WithMany()
                .HasForeignKey(g => g.LiceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
