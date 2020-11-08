using System;
namespace BusinessChat.Domain.Common
{
    public class BaseEntity
    {
        public int ID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
    }
}
