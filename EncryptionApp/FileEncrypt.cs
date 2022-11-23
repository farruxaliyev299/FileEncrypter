using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionApp
{
    public static class FileEncrypt
    {
        public static byte[] EncryptOrDecrypt(byte[] text, byte[] key)
        {
            byte[] xor = new byte[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                xor[i] = (byte)(text[i] ^ key[i % key.Length]);
            }
            return xor;
        }
    }
}
