using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbundantLife_Model
{
    public class UserGroups
    {
        public int UserGroupID { get; set; }
        public string GroupName { get; set; }
        public int GroupLevel { get; set; }
        public bool Active { get; set; }
    }
}
