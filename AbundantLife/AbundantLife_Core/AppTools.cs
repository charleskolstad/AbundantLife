using AbundantLife_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbundantLife_Core
{
    public class AppTools
    {
        internal static ISprocCalls InitSprocCalls(bool isTest)
        {
            if (isTest)
                return new FakeSprocCalls();

            return new SprocCalls();
        }

        internal static IEmailTools InitEmailTools(bool isTest)
        {
            if (isTest)
                return new FakeEmailTools();

            return new EmailTools();
        }

        internal static IMembershipTools InitMembershipTools(bool isTest)
        {
            if (isTest)
                return new FakeMembershipTools();

            return new MembershipTools();
        }
    }
}
