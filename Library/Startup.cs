using Library.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Library.Services.EmailServices;
using Library.Services.CheckServices;
using Library.Services.CheckServicesQuartz;
using Quartz.Spi;
using Quartz;
using Quartz.Impl;
using AutoMapper;
using Library.Services.BookContorlServices;
using Library.Services.ReservationControlServices;
using BusinessLayer.InrefacesRepository;
using BusinessLayer.ImplementationsRepository;

namespace Library
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
            services.AddTransient<EmailService>();
            services.AddTransient<Settings>();
            services.AddTransient<MessageForm>();
            services.AddHostedService<StartEmailService>();

            //Передаем конфигурацию 'EmailServiceSettings' через IOptions
            services.Configure<Settings>(Configuration.GetSection("EmailServiceSettings"));

            // Add Quartz services
            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            // Add our job
            services.AddSingleton<CheckJob>();
            services.AddSingleton(new EmailScheduler(
                jobType: typeof(CheckJob),
                cronExpression: $"0 0 0 1/{Configuration.GetValue<int>("EmailServiceSettings:RunInterval")} * ?"));


            services.AddAutoMapper(typeof(Startup));

            services.AddDbContext<ApplicationContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddTransient<DbContext, ApplicationContext>();
            services.AddTransient<IRepository<Book>, Repository<Book>>();
            services.AddTransient<IRepository<Tracking>, Repository<Tracking>>();
            services.AddTransient<IRepository<Reservation>, Repository<Reservation>>();
            services.AddTransient<IRepository<Comment>, Repository<Comment>>();
            services.AddTransient<IRepository<Evaluation>, Repository<Evaluation>>();
            services.AddTransient<IBooksRepository, EFBooksRepository>();
            services.AddTransient<IReservationRepository, EFReservationsRepository>();
            services.AddTransient<ITrackingsRepository, EFTrackingsRepository>();
            
            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceScopeFactory serviceScope)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();


            app.UseRouting(); // используем систему маршрутизации
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            app.AddUsersAndRole(serviceScope);
        }
    }
}
