using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VisualSmart.Util
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class MD5Util
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="plain"></param>
        /// <returns></returns>
        public static string Encrypt(string plain)
        {
            return Encrypt(Encoding.UTF8.GetBytes(plain));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Encrypt(object obj)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memory, obj);
                return Encrypt(memory.ToArray());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plain"></param>
        /// <returns></returns>
        public static string Encrypt(byte[] plain)
        {
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(plain);
            StringBuilder hash = new StringBuilder();
            foreach (byte b in bytes)
            {
                hash.Append(b.ToString("X2"));
            }

            md5.Initialize();
            bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(hash.ToString()));
            hash.Length = 0;
            foreach (byte b in bytes)
            {
                hash.Append(b.ToString("X2"));
            }

            return hash.ToString();
        }
    }
}
