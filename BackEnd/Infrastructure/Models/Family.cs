using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class Family
    {
        public string? Id { get; set; }
        public required string Name { get; set; }
        public List<string> AdminUserIds { get; set; } = []; 
        public List<string> MemberUserIds { get; set; } = [];
    }
}
