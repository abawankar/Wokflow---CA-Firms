using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserInterface.Helpers
{
    public class TokenHelper
    {
        public static string GetToken(string name)
        {
            // Edit this to add your own salt !
            return string.Format("{0}&{1}={2}", name, (name.Length * 97), new string(name.Reverse().ToArray()));
        }

        public static bool VerifyToken(string name, string token)
        {
            return MD5Helper.VerifyMd5Hash(TokenHelper.GetToken(name), token);
        }
    }
}