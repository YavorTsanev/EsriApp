namespace EsriApi
{
    using Data;
    using Hangfire;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Services;
    using Services.Contracts;
    using System.Threading.Tasks;

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

            services.AddCors(policy =>
            {
                policy.AddPolicy("CorsPolicy", opt => opt
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
            services.AddHangfireServer();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EsriApi", Version = "v1" });
            });

            services.AddHttpClient();
            services
                .AddDbContext<ApplicationDbContext>(options => options
                    .UseSqlServer(connectionString));

            //Custom Services
            services.AddTransient<IUsaCountiesService, UsaStatesService>();
            services.AddTransient<BackgroundProcessing>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRecurringJobManager recurringJob, BackgroundProcessing processing)
        {
            app.UseCors("CorsPolicy");

            app.UseHangfireDashboard();

            _ = SeedHangFireJobs(recurringJob);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EsriApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            processing.RecurringUpdateDb().GetAwaiter().GetResult();

            async Task SeedHangFireJobs(IRecurringJobManager recurringJob)
            {
                recurringJob.AddOrUpdate<BackgroundProcessing>("BackgroundProcessing", x => x.RecurringUpdateDb(), Cron.Minutely);
            }
        }
    }
}
