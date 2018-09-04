using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Lib.AspNetCore.ServerSentEvents;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace Demo.AspNetCore.ServerSentEvents.Services
{
    internal class HeartbeatService : BackgroundService
    {
        #region Fields
        private const string HEARTBEAT_MESSAGE_FORMAT = "Demo.AspNetCore.ServerSentEvents Heartbeat ({0} UTC)";

        private readonly IServerSentEventsService _serverSentEventsService;
        #endregion

        #region Constructor
        public HeartbeatService(IServerSentEventsService serverSentEventsService)
        {
                _serverSentEventsService = serverSentEventsService;
        }
        #endregion

        #region Methods
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                ServerSentEvent  serverSentEvent = new ServerSentEvent()
                {
                    Id = "xxx",
                    Type = "heartbeat",
                    Data = new List<string>() { String.Format(HEARTBEAT_MESSAGE_FORMAT, DateTime.UtcNow) }
                };
                //await _serverSentEventsService.SendEventAsync(String.Format(HEARTBEAT_MESSAGE_FORMAT, DateTime.UtcNow));
                string toSend = JsonConvert.SerializeObject(serverSentEvent);
                await _serverSentEventsService.SendEventAsync(toSend);

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
        #endregion
    }

}
