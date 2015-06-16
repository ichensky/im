using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controls.Helpers
{
    public static class StringExtensions
    {
        public static string RemoveDiacritics(this String s)
        {
            byte[] tempBytes = Encoding.GetEncoding("ISO-8859-8").GetBytes(s);
            return Encoding.UTF8.GetString(tempBytes, 0, tempBytes.Length);
        }
    }
}
