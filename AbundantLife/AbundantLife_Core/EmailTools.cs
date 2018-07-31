using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbundantLife_Core
{
    internal class EmailTools : IEmailTools
    {
        public bool SendEmail(string to, string body)
        {
            throw new NotImplementedException();
        }
    }

    internal class FakeEmailTools : IEmailTools
    {
        public bool SendEmail(string to, string body)
        {
            return true;
        }
    }

    internal interface IEmailTools
    {
        bool SendEmail(string to, string body);
    }
}
