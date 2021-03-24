﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MuslimFashion.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public virtual DbSet<Color> Color { get; set; }
        public virtual DbSet<HomeMenu> HomeMenu { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Registration> Registration { get; set; }
        public virtual DbSet<Size> Size { get; set; }
        public virtual DbSet<SubMenu> SubMenu { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ColorConfiguration());
            builder.ApplyConfiguration(new HomeMenuConfiguration());
            builder.ApplyConfiguration(new MenuConfiguration());
            builder.ApplyConfiguration(new RegistrationConfiguration());
            builder.ApplyConfiguration(new SubMenuConfiguration());
            builder.ApplyConfiguration(new SizeConfiguration());

            base.OnModelCreating(builder);
            builder.SeedAdminData();
        }
    }
}