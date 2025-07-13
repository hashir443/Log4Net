using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogKeeper.Enums
{
    public enum LogTarget
    {
        None = 0,
        Database = 1,
        File = 2,
        CloudWatch = 4,
        All = Database | File | CloudWatch
    }
}
