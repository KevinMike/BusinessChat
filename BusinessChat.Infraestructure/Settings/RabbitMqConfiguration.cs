﻿using System;
namespace BusinessChat.Infrastructure.Settings
{
    public class RabbitMqConfiguration
    {
        public string Hostname { get; set; }

        public int Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
