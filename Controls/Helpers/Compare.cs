using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controls.Helpers
{
    class Compare : IEqualityComparer<String>
    {
        public bool Equals(String x, String y)
        {
            if (x.ToLower() == y.ToLower())
            {
                return true;
            }

            return false;
        }

        public int GetHashCode(String codeh)
        {
            return 0;
        }
    }
}
