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
using Microsoft.Owin;
using Owin;
using Demo.AspNetCore.ServerSentEvents.Services.Bots;

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
            services.AddSingleton<IChatService, ChatService>();
            services.AddSingleton<IChatRoomService ,ChatRoomService>();
            services.AddSingleton<IChatBot, ChatBot>();
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
            IAppBuilder appb = serviceProvider.GetService<IAppBuilder>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //.UseHttpsRedirection();
            app.UseResponseCompression()
                .MapServerSentEvents("/sse-notifications", serviceProvider.GetService<NotificationsServerSentEventsService>())
                .MapServerSentEvents("/sse-cti-event", serviceProvider.GetService<NotificationsServerSentEventsService>())
                .UseStaticFiles();

                //appb.MapSignalR();
                app.UseMvc(routes =>
                {
                    routes.MapRoute(name: "default", template: "{controller=Basic}/{action=Index}");
                });
        }
        #endregion
    }
}
