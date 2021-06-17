using System;
using System.Collections.Generic;
using System.Text;

namespace DailyNewsServer.Core.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string Description { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
