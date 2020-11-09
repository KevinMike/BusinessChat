using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BusinessChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessChat.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<ChatMessage> ChatMessages { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
