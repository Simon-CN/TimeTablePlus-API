using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetablePlus_API.Utility
{
    public class TokenUtil
    {
        public static string CreateToken(int userId)
        {
            string str = userId + "," + DateTime.Now.ToLongTimeString();
            Encoding encode = Encoding.ASCII;
            byte[] bytedata = encode.GetBytes(str);
            return Convert.ToBase64String(bytedata);
        }

        public static string ParseToken(string token)
        {
            byte[] bytedata = Convert.FromBase64String(token);
            return ASCIIEncoding.Default.GetString(bytedata);
        }

        public static int GetUserId(string token)
        {
            byte[] bytedata = Convert.FromBase64String(token);
            return Convert.ToInt32(ASCIIEncoding.Default.GetString(bytedata).Split(',')[0]);
        }

        public static DateTime GetLoginTime(string token)
        {
            byte[] bytedata = Convert.FromBase64String(token);
            return DateTime.Parse(ASCIIEncoding.Default.GetString(bytedata).Split(',')[1]);
        }
    }
}
