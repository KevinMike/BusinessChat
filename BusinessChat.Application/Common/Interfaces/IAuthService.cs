using System;
using System.Threading.Tasks;
using BusinessChat.Application.Common.Models;

namespace BusinessChat.Application.Common.Interfaces
{
    public interface IAuthService
    {
        public string UserId { get; }
    }
}
