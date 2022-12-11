using System.Reflection.Emit;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TangyRestaurantWebsite.Models;

namespace TangyRestaurantWebsite.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Category> Category { get; set; }
    public DbSet<SubCategory> SubCategory { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<Coupons> Coupons { get; set; }
    public DbSet<ApplicationUser> ApplicationUser { get; set; }
    public DbSet<ShoppingCart> ShoppingCart { get; set; }
    public DbSet<OrderHeader> OrderHeader { get; set; }
    public DbSet<OrderDetails> OrderDetail { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
        .HasKey(p => p.Id);

        modelBuilder.Entity<SubCategory>()
        .HasKey(o => o.Id);

        modelBuilder.Entity<SubCategory>()
            .HasOne(o => o.MyCategory)
            .WithMany()
            .HasForeignKey(o => o.CategoryId);


        modelBuilder.Entity<MenuItem>()
            .HasKey(s => s.Id);

        modelBuilder.Entity<MenuItem>()
                    .HasOne(m => m.Category)
                    .WithMany()
                    .HasForeignKey(m => m.CategoryId);

        modelBuilder.Entity<MenuItem>()
                    .HasOne(m => m.SubCategory)
                    .WithMany()
                    .HasForeignKey(m => m.SubCategoryId);

        base.OnModelCreating(modelBuilder);

    }

}

