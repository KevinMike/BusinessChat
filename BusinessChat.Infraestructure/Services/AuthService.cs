using System;
using BusinessChat.Application.Common.Interfaces;

namespace BusinessChat.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        public AuthService()
        {
        }

        public string UserId => throw new NotImplementedException();
    }
}
