using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Demo.AspNetCore.ServerSentEvents.Model;
using Demo.AspNetCore.ServerSentEvents.Services;
using Newtonsoft.Json;

namespace Demo.AspNetCore.ServerSentEvents.Controllers
{
    public class NotificationsController : Controller
    {
        #region Fields
        private INotificationsService _notificationsService;
        #endregion

        #region Constructor
        public NotificationsController(INotificationsService notificationsService)
        {
            _notificationsService = notificationsService;
        }
        #endregion

        #region Actions
        [ActionName("sse-notifications-receiver")]
        [AcceptVerbs("GET")]
        public IActionResult Receiver()
        {
            return View("Receiver");
        }

        [ActionName("index")]
        [AcceptVerbs("GET")]
        public IActionResult Index()
        {
            return View("Index");
        }

        [ActionName("sse-notifications-sender")]
        [AcceptVerbs("GET")]
        public IActionResult Sender()
        {
            return View("Sender", new NotificationsSenderViewModel());
        }

        [ActionName("cti-realtime")]
        [AcceptVerbs("GET")]
        public IActionResult CTI()
        {
            return View("CTIEvent", new SSEventModel());
        }


        [ActionName("sse-cti-sender")]
        [AcceptVerbs("GET")]
        public IActionResult CTISender()
        {
            SSEventModel vm = new SSEventModel();
            vm.Contacts = new List<Contact>
            {
                new Contact { Id = "001", FullName = "John Smith", PhoneNumber = "(123)456-7890" },
                new Contact { Id = "002", FullName = "Jan Perrier", PhoneNumber = "(800)456-7891" },
                new Contact { Id = "003", FullName = "Kim Loo", PhoneNumber = "(777)456-7892" },
                new Contact { Id = "004", FullName = "Sam Uncle", PhoneNumber = "(888)456-7893" },

            };
            vm.CallingDN = "1234567890";
            vm.cti_event = "EventRinging";
            vm.callid = "00000001";
            return View("CTISender", vm);
        }

        [ActionName("sse-notifications-sender")]
        [AcceptVerbs("POST")]
        public async Task<IActionResult> Sender(NotificationsSenderViewModel viewModel)
        {
            if (!String.IsNullOrEmpty(viewModel.Notification))
            {
                await _notificationsService.SendNotificationAsync(viewModel.Notification, viewModel.Alert);
            }

            ModelState.Clear();

            return View("Sender", new NotificationsSenderViewModel());
        }

        [ActionName("sse-cti-sender")]
        [AcceptVerbs("POST")]
        public async Task<IActionResult> Sender(SSEventModel viewModel)
        {
             if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(viewModel.cti_event))
                {
                    if (!String.IsNullOrEmpty(viewModel.selectedDN))
                    {
                        if (viewModel.selectedDN == "Please select contact")
                            viewModel.selectedDN = "+1(123)456-7890";
                        viewModel.CallingDN = viewModel.selectedDN;
                    }
                    viewModel.sequance_id++;
                    string ss = JsonConvert.SerializeObject(viewModel);
                    await _notificationsService.SendNotificationAsync("cti", ss);
                }
            }
            SSEventModel vm = viewModel;
            vm.Contacts = new List<Contact>
            {
                new Contact { Id = "001", FullName = "John Smith", PhoneNumber = "(123)456-7890" },
                new Contact { Id = "002", FullName = "Jan Perrier", PhoneNumber = "(800)456-7891" },
                new Contact { Id = "003", FullName = "Kim Loo", PhoneNumber = "(777)456-7892" },
                new Contact { Id = "004", FullName = "Sam Uncle", PhoneNumber = "(888)456-7893" },

            };
            vm.CallingDN = "1234567890";
            return View("CTISender", vm);
        }
        [ActionName("sse-cti-sender-call")]
        [AcceptVerbs("POST")]
        public async Task<IActionResult> Call(SSEventModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.CallingDN = viewModel.selectedDN;
                if (true)
                {
                    viewModel.CallingDN = "+1(123)456-7890";
                    viewModel.Contacts = new List<Contact>() { new Contact() { FullName = "John Smith", PhoneNumber = "+1(123)456-7890", Id = "fbd0001" } };
                    viewModel.callid = new Guid().ToString();
                    viewModel.data = "No Data";
                }
                string[] events = { "EventRinging", "EventEstablished", "EventHeld", "EventResumed", "EventReleased" };
                for (int i = 0; i < 5; i++)
                {
                    viewModel.sequance_id++;
                    viewModel.cti_event = events[i];
                    string ss = JsonConvert.SerializeObject(viewModel);
                    await _notificationsService.SendNotificationAsync("cti", ss);
                    await Task.Delay(3000);
                }
            }
            SSEventModel vm = viewModel;
            vm.Contacts = new List<Contact>
            {
                new Contact { Id = "001", FullName = "John Smith", PhoneNumber = "(123)456-7890" },
                new Contact { Id = "002", FullName = "Jan Perrier", PhoneNumber = "(800)456-7891" },
                new Contact { Id = "003", FullName = "Kim Loo", PhoneNumber = "(777)456-7892" },
                new Contact { Id = "004", FullName = "Sam Uncle", PhoneNumber = "(888)456-7893" },

            };
            vm.CallingDN = "1234567890";
            return View("CTISender", vm);
        }

        public IActionResult Resume()
        {
            return View("Resume");
        }

        #endregion
    }
}
