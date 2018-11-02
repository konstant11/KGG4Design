using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.AspNetCore.ServerSentEvents;

namespace Demo.AspNetCore.ServerSentEvents.Services
{
    internal abstract class NotificationsServiceBase
    {
        #region Fields
        private INotificationsServerSentEventsService _notificationsServerSentEventsService;
        #endregion

        #region Constructor
        protected NotificationsServiceBase(INotificationsServerSentEventsService notificationsServerSentEventsService)
        {
            _notificationsServerSentEventsService = notificationsServerSentEventsService;
        }
        #endregion

        #region Methods
        protected Task SendSseEventAsync(string notification, bool alert)
        {
            return _notificationsServerSentEventsService.SendEventAsync(new ServerSentEvent
            {
                Type = alert ? "alert" : null,
                Data = new List<string>(notification.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None))
            });
        }

        protected Task SendSseEventAsync(string eventType, string  jsonContent)
        {
            return _notificationsServerSentEventsService.SendEventAsync(new ServerSentEvent
            {
                Type = eventType,
                //Data = new List<string>(new string[] { string.Format("cti-event: {0}",  cti_event ),jsonContent });
                Data = new List<string>(new string[] { jsonContent })
               // Data = new List<string>(notification.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None))
            });
        }

        #endregion
    }
}
