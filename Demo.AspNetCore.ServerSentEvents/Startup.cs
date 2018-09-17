using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Lib.AspNetCore.ServerSentEvents;
using Demo.AspNetCore.ServerSentEvents.Services;
using Microsoft.AspNetCore.Http;

namespace Demo.AspNetCore.ServerSentEvents
{

    public class Startup
    {
        #region Properties
        public IConfiguration Configuration { get; }
        #endregion

        #region Constructor
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #endregion

        #region Methods

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "text/event-stream" });
            });

            services.AddServerSentEvents();
            services.AddSingleton<IHostedService, HeartbeatService>();
            services.AddDbContext<MySqliteDBContext>();

            //services.AddDbContext<MySqliteDBContext>(options =>
             //                                        options.UseSqlite("Data Source=MySqliteDB.db"));

            services.AddServerSentEvents<INotificationsServerSentEventsService, NotificationsServerSentEventsService>();
            services.AddNotificationsService(Configuration);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //services.AddHttpsRedirection(options =>
            //{
            //    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
            //    options.HttpsPort = 5001;
            //});            

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           //app.UseHttpsRedirection();
            app.UseResponseCompression()
                .MapServerSentEvents("/sse-notifications", serviceProvider.GetService<NotificationsServerSentEventsService>())
               .MapServerSentEvents("/sse-cti-event", serviceProvider.GetService<NotificationsServerSentEventsService>())
                .UseStaticFiles()
                .UseMvc(routes =>
                {
                    routes.MapRoute(name: "default", template: "{controller=Basic}/{action=Index}");
                });
        }
        #endregion
    }
}
