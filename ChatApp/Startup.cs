using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using ChatApp.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ChatApp.Hubs;

namespace ChatApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public static readonly ILoggerFactory LogFactory = LoggerFactory.Create(b =>
        {
            b
            .AddFilter((c, l) =>
                c == DbLoggerCategory.Database.Command.Name
            )
            .AddConsole();

        });

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"))
                .UseLoggerFactory(LogFactory));
            services.AddDefaultIdentity<Models.ChatUser>(options =>
            {
                //Set password requirements
                options.Password = new PasswordOptions()
                {
                    RequiredLength = 6,

                };
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddScoped<MessageService>();
            services.AddSignalR(options =>
            {
                options.ClientTimeoutInterval = TimeSpan.FromMinutes(1);
                options.KeepAliveInterval = TimeSpan.FromSeconds(30);
            });

            services.AddSingleton<IUserStatusStorageService, UserStatusStorageService>();
            services.AddSingleton<IUserStatusMonitorService, UserStatusMonitorService>();

            services.AddHostedService<ChatHubKeepAlive>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapHub<ChatHub>("/chathub");
            });
        }
    }
}
