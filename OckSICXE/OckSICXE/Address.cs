using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace OckSICXE
{
    internal class Address
    {
        static int Simple(int LocCounter = 0, int ProgramCounter = 0, bool Indexing = false, int TargetAddress = 0)
        {
            int location = 0;

            return location;
        }
        static int Indirect()
        {
            return 1;
        }
        static int Immediate(int TargetAddress)
        {
            return TargetAddress;
        }
    }
}
