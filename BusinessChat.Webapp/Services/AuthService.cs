using System;
using System.Security.Claims;
using BusinessChat.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusinessChat.Webapp.Services
{
    public class AuthService : IAuthService
    {
        public string UserId { get; }

        public AuthService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
