using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SampleAPI.Contracts;
using SampleAPI.Filters;
using SampleAPI.Services;
using System.Threading;

namespace SampleAPI
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
            services.AddControllers(options =>
                options.Filters.Add(new HttpResponseExceptionFilter()));

            services.Configure<AppSettings>(Configuration.GetSection("ApplicationSettings"));

            services.AddSingleton<IRedisFactory, RedisFactory>();
            services.AddSingleton<ICacheService>(x =>
                new CacheService(
                    x.GetRequiredService<IRedisFactory>(),
                    x.GetRequiredService<ILogger<CacheService>>()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // set min threads to reduce chance for Redis timeoutes
            // https://docs.microsoft.com/en-us/azure/azure-cache-for-redis/cache-troubleshoot-client#traffic-burst
            ThreadPool.SetMinThreads(200, 200);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
