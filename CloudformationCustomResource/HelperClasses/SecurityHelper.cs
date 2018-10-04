using System;
using System.Security.Cryptography;
using System.Text;

namespace CloudformationCustomResource.HelperClasses
{
    class SecurityHelper
    {
        public static string GetSha256Hash(string input)
        {
            byte[] tmpSource;
            byte[] tmpHash;

            //Create a byte array from source data.
            tmpSource = ASCIIEncoding.ASCII.GetBytes(input);
            // Convert the input string to a byte array and compute the hash.
            tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);

            return ByteArrayToString(tmpHash);
        }

        private static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }
    }
}
