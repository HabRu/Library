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
using Org.BouncyCastle.Asn1.X509.Qualified;
using BusinessLayer.Services.LibraryParser.Model;
using BusinessLayer.Services.LibraryParser.Jobs;
using BusinessLayer.Services.LibraryParser;
using BusinessLayer.Services.LibraryParser.ParserInterfaces;
using System.Collections.Generic;
using Parser;

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
            services.AddHostedService<StartParserService>();

            //Передаем конфигурацию 'EmailServiceSettings' через IOptions
            services.Configure<Settings>(Configuration.GetSection("EmailServiceSettings"));
            services.Configure<ParserSettings>(Configuration.GetSection("ParserServiceSettings"));

            // Add Quartz services
            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            services.AddTransient<IParser<IEnumerable<string>>, ListBooksParse>();
            services.AddTransient<IParser<BookParserModel>, BookParser>();

            // Add our job
            services.AddSingleton<CheckJob>();
            services.AddSingleton(new EmailScheduler(
                jobType: typeof(CheckJob),
                cronExpression: $"0 0 0 1/{Configuration.GetValue<int>("EmailServiceSettings:RunInterval")} * ?"));

            services.AddTransient<AddDataFromParserJob>();
            services.AddSingleton(new ParserScheduler(
                jobType: typeof(AddDataFromParserJob),
                cronExpression: $"0 0/30 * 1/1 * ? *"));


            services.AddAutoMapper(typeof(Startup));

            services.AddDbContext<ApplicationContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Library")));

            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddTransient<DbContext, ApplicationContext>();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            
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
