﻿using Microsoft.AspNetCore.SignalR;

namespace SmartMass.Controller.Api.Hubs
{
    public class MessageHub : Hub
    {
        private readonly ILogger<MessageHub> logger;

        public MessageHub(ILogger<MessageHub> logger)
        {
            this.logger = logger;
        }

    }
}
