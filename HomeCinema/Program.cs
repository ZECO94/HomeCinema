using HomeCinema.DataAccess;
using HomeCinema.DataAccess.Repository;
using HomeCinema.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using HomeCinema.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;

namespace HomeCinema
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>
            (options=>options.UseSqlServer
            (builder.Configuration.GetConnectionString("DefaultConnection")));
            // For Identity
            builder.Services.AddRazorPages();
            builder.Services.AddIdentity<IdentityUser,IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            // Handel Pathes
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Accout/Login";
                options.LogoutPath = $"/Identity/Accout/Logout";
                options.AccessDeniedPath = $"/Identity/Accout/AccessDenied";
            });
            //Unit Of Work
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
            builder.Services.AddScoped<IEmailSender,EmailSender>();
            //Stripe
            builder.Services.Configure<StripeSetting>(builder.Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
