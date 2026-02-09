using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using  Microsoft.EntityFrameworkCore;


namespace api.Data
{
    public class AppDbContext: IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions) //? pass dbcontext up to inherited
        {
            
        }
        // ADDING TABLES
        public new DbSet<User> Users{get;set;}
        public DbSet<EntityConfig> Configurations{get;set;}
        // So you can inherit ....

        public DbSet<Base> Bases{get;set;}
        public DbSet<Comment> Comments{get;set;}
        public DbSet<SSD> SSDs {get;set;} // name here is name in dbs
        public DbSet<GPU> GPUs{get;set;}
        public DbSet<CPU> CPUs{get;set;}
        public DbSet<Mobo> Motherboards{get;set;}
        public DbSet<Pcu> Pcus{get;set;}
        public DbSet<Ram> Rams{get; set;}
        
        // build it yourself
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ----------GASP inheritanceeeee fr this time...
            builder.Entity<Base>()
            .HasKey(b => b.Id);

            // ----------TPT
            builder.Entity<Base>().ToTable("Bases");

            builder.Entity<SSD>().ToTable("SSDs");
            builder.Entity<GPU>().ToTable("GPUs");
            builder.Entity<CPU>().ToTable("CPUs");
            builder.Entity<Mobo>().ToTable("Motherboards");
            builder.Entity<Pcu>().ToTable("Pcu");
            builder.Entity<Ram>().ToTable("Rams");

            // ----------Roles
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
            };
            builder.Entity<IdentityRole>().HasData(roles);

            // ----------Confguration
            // composite key
            builder.Entity<EntityConfig>()
            .HasKey(c=> new {c.Username, c.ConfigName});

            // -----------User + Configuration 
            
            // foreign key to be sure
            // O2M 1 user Many configs
            builder.Entity<EntityConfig>()
            .HasOne(c => c.User) // config remmebers user
            .WithMany(u => u.Configs) // user remembers config
            .HasForeignKey(c => c.Username)
            .HasPrincipalKey(u => u.UserName);

            builder.Entity<EntityConfig>()
            .HasOne(c => c.Ssd) // config remembers ssd
            .WithMany() // ssd doesnt care
            .HasForeignKey(c => c.SsdId)
            .OnDelete(DeleteBehavior.NoAction);  // cascade == set null in sqls eyes so now we 
            //                                      cant remove component if it belongs to config

            builder.Entity<EntityConfig>()
            .HasOne(c => c.Cpu) // config remembers ssd
            .WithMany() // ssd doesnt care
            .HasForeignKey(c => c.CpuId)
            .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<EntityConfig>()
            .HasOne(c => c.Gpu) // config remembers ssd
            .WithMany() // ssd doesnt care
            .HasForeignKey(c => c.GpuId)
            .OnDelete(DeleteBehavior.NoAction);

             builder.Entity<EntityConfig>()
            .HasOne(c => c.Ram) // config remembers ssd
            .WithMany() // ssd doesnt care
            .HasForeignKey(c => c.RamId)
            .OnDelete(DeleteBehavior.NoAction);

             builder.Entity<EntityConfig>()
            .HasOne(c => c.Mobo) // config remembers ssd
            .WithMany() // ssd doesnt care
            .HasForeignKey(c => c.MoboId)
            .OnDelete(DeleteBehavior.NoAction);
            
             builder.Entity<EntityConfig>()
            .HasOne(c => c.Pcu) // config remembers ssd
            .WithMany() // ssd doesnt care
            .HasForeignKey(c => c.PcuId)
            .OnDelete(DeleteBehavior.NoAction);

            // -------------comments
            // 
            builder.Entity<Comment>()
            .HasKey(c => c.Id);

            builder.Entity<Comment>()
            .HasOne(c => c.Base) 
            .WithMany(b => b.comments)
            .HasForeignKey(c => c.BaseId);

            builder.Entity<Comment>()
            .HasOne(c => c.User) // comment remmebers user
            .WithMany() // user doesnt care
            .HasForeignKey(c => c.Username)
            .HasPrincipalKey(u => u.UserName);
            


            
        }


    }
}