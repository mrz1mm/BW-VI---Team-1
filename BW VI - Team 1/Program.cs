using BW_VI___Team_1.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BW_VI___Team_1.Services;
using BW_VI___Team_1.Interfaces;
using Microsoft.Extensions.FileProviders;

namespace BW_VI___Team_1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
                options.AccessDeniedPath = "/Home/Index";
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.Pharmacist, policy => policy.RequireRole("Pharmacist"));
                options.AddPolicy(Policies.Veterinarian, policy => policy.RequireRole("Veterinarian"));
                options.AddPolicy("Admin", policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c =>
                            (c.Type == ClaimTypes.Role && (c.Value == "Pharmacist" || c.Value == "Veterinarian")))));
            });

            builder.Services.AddDbContext<LifePetDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services
            .AddScoped<IAnimalSvc, AnimalSvc>()
            .AddScoped<IAuthSvc, AuthSvc>()
            .AddScoped<IImageSvc>(x => new ImageSvc(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images")))
            .AddScoped<ILockerSvc, LockerSvc>()
            .AddScoped<IOrderSvc, OrderSvc>()
            .AddScoped<IOwnerSvc, OwnerSvc>()
            .AddScoped<IProductSvc, ProductSvc>()
            .AddScoped<IRecoverySvc, RecoverySvc>()
            .AddScoped<ISupplierSvc, SupplierSvc>()
            .AddScoped<IUsageSvc, UsageSvc>()
            .AddScoped<IVisitSvc, VisitSvc>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                // Mappare la pagina importata come la rotta predefinita
                endpoints.MapGet("/", async context =>
                {
                    context.Response.ContentType = "text/html";
                    await context.Response.SendFileAsync(Path.Combine(app.Environment.WebRootPath, "importedProject/Homepage/index.html"));
                });

                // Mappare la pagina importata anche a una rotta specifica (opzionale)
                endpoints.MapGet("/imported-project", async context =>
                {
                    context.Response.ContentType = "text/html";
                    await context.Response.SendFileAsync(Path.Combine(app.Environment.WebRootPath, "importedProject/Homepage/index.html"));
                });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            app.Run();
        }
    }
}
