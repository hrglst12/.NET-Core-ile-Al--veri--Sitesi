using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Site.Data;
using Site.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Site
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()//options => options.SignIn.RequireConfirmedAccount = true bunu kaldýrdým
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddSingleton<IEmailSender, EmailSender>();
            services.Configure<EmailOptions>(Configuration);
            services.AddControllersWithViews().AddRazorRuntimeCompilation();//AddRazorRuntimeCompilation() frontend tarafýnda yaptýðýn deðiiklikleri anýnda tarayýcý tarafýnda görmeyi saðlar
            services.AddRazorPages();

            //kulanýcý yetkisi yoksa kullanýcý login sayfasýna yönlendirmek için
            services.ConfigureApplicationCookie(options =>
             {
                 options.LoginPath = $"/Identity/Account/Login";
                 options.LogoutPath = $"/Identity/Account/Logout";
                 options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });
            //facebook ile ilgili servisi burda açtým
            services.AddAuthentication().AddFacebook(options =>
            {
                options.AppId = "613721980424023";
                options.AppSecret = "99d384b795de48b3e6083e340f16eb69";
            });
            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "552963290475-o1iksbi4a78aenvd727n23rhvv204rt3.apps.googleusercontent.com";
                options.ClientSecret = "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";
            });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
