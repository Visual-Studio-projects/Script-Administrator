using System;
using System.Security;
using System.Security.Cryptography;

namespace DocScript.Scripts
{
    class Security
    {
        /// <summary>
        /// https://weblogs.asp.net/jongalloway/encrypting-passwords-in-a-net-app-config-file
        /// </summary>
        readonly static byte[] entropy = System.Text.Encoding.Unicode.GetBytes("Code never lies, comments sometimes do.");

        /// <summary>
        /// To encrypt a string
        /// <example>
        /// <code lang="C#">
        /// Properties.Settings.Default.Script_Password = Scripts.Security.EncryptString(ToSecureString(txtPassword.Password));
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="input">string to be encrypted</param>
        /// <returns>encrypted string</returns>
        public static string EncryptString(System.Security.SecureString input)
        {
            byte[] encryptedData = ProtectedData.Protect(
                System.Text.Encoding.Unicode.GetBytes(ToInsecureString(input)),
                entropy,
                DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// To decrypt a string
        /// <example>
        /// <code lang="C#">
        /// SecureString password = DecryptString(Properties.Settings.Default.Script_Password);
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="encryptedData">encrypted string</param>
        /// <returns>secure string decrypted</returns>
        public static SecureString DecryptString(string encryptedData)
        {
            try
            {
                byte[] decryptedData = ProtectedData.Unprotect(
                    Convert.FromBase64String(encryptedData),
                    entropy,
                    DataProtectionScope.CurrentUser);
                return ToSecureString(System.Text.Encoding.Unicode.GetString(decryptedData));
            }
            catch
            {
                return new SecureString();
            }
        }

        /// <summary>
        /// Change a string to a secured string
        /// </summary>
        /// <param name="input">string to secure</param>
        /// <returns>Returns a secured string</returns>
        public static SecureString ToSecureString(string input)
        {
            SecureString secure = new SecureString();
            foreach (char c in input)
            {
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }

        /// <summary>
        /// Change a secured string to a string
        /// </summary>
        /// <param name="input">encrypted string</param>
        /// <returns>secure string decrypted</returns>
        public static string ToInsecureString(SecureString input)
        {
            string returnValue = string.Empty;
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(input);
            try
            {
                returnValue = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
            return returnValue;
        }

    }
}