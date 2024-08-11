using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace OsData
{
    public enum EncryptionType
    {
        DES,
        RC2,
        Rijndael,
        TripleDES,
        AES
    }


    public enum EncryptionTextFormat
    {
        Base64,
        UTF8,
        URL,
        Ascii,
        Hex
    }

    public enum EncryptionDirection
    {
        Encrypt,
        Decrypt
    }


    public class GeneralEncryption
    {
        public static string EncryptWithPassword(EncryptionDirection direction, EncryptionType encryptType, string str, string passPhrase, byte[] sault, EncryptionTextFormat inputEncoding, EncryptionTextFormat outputEncoding, char paddingCharacter)
        {
            SymmetricAlgorithm Provider;
            switch (encryptType)
            {
                case EncryptionType.TripleDES:
                    Provider = new TripleDESCryptoServiceProvider();
                    break;
                case EncryptionType.DES:
                    Provider = new DESCryptoServiceProvider();
                    break;
                case EncryptionType.RC2:
                    Provider = new RC2CryptoServiceProvider();
                    break;
                case EncryptionType.AES:
                    Provider = new AesCryptoServiceProvider();
                    Provider.Padding = PaddingMode.PKCS7;
                    break;
                case EncryptionType.Rijndael:
                default:
                    Provider = new RijndaelManaged();
                    break;
            }

            string NewKey = null;
            if (Provider.LegalKeySizes.Length > 0)
            {
                int MaxSize = Provider.LegalKeySizes[0].MaxSize;
                if (passPhrase.Length * 8 > MaxSize)
                    NewKey = passPhrase.Substring(0, MaxSize / 8);
                else
                {
                    int MoreKeySize = Provider.LegalKeySizes[0].MinSize;
                    while (passPhrase.Length * 8 > MoreKeySize)
                    {
                        MoreKeySize += Provider.LegalKeySizes[0].SkipSize;
                    }
                    NewKey = passPhrase.PadRight(MoreKeySize / 8, paddingCharacter);
                }
            }
            else
                NewKey = passPhrase;

            if (sault == null)
                sault = new byte[] { 0, 0, 0, 0, 0 };
            byte[] newSault = sault;

            if (Provider.LegalBlockSizes.Length > 0)
            {
                int MaxSaleSize = Provider.LegalBlockSizes[0].MaxSize;
                int NewSaltSize = NewKey.Length;
                if (NewSaltSize > MaxSaleSize)
                    NewSaltSize = MaxSaleSize;

                newSault = new byte[MaxSaleSize];
                for (int idx = 0; idx < NewSaltSize && idx < sault.Length; idx++)
                    newSault[idx] = sault[idx];
            }

            byte[] PlainText = null;
            switch (inputEncoding)
            {
                case EncryptionTextFormat.Base64:
                    PlainText = Convert.FromBase64String(str);
                    break;
                case EncryptionTextFormat.UTF8:
                    PlainText = System.Text.UTF8Encoding.UTF8.GetBytes(str);
                    break;
                case EncryptionTextFormat.URL:
                    PlainText = System.Web.HttpUtility.UrlDecodeToBytes(str);
                    break;
                case EncryptionTextFormat.Ascii:
                    PlainText = System.Text.ASCIIEncoding.ASCII.GetBytes(str);
                    break;
                case EncryptionTextFormat.Hex:
                    PlainText = DecryptFromHexa(str);
                    break;
                default:
                    PlainText = System.Text.UTF8Encoding.UTF8.GetBytes(str);
                    break;
            }

            ICryptoTransform Encryptor = (direction == EncryptionDirection.Encrypt) ?
                Provider.CreateEncryptor(System.Text.ASCIIEncoding.ASCII.GetBytes(NewKey), newSault) :
                Provider.CreateDecryptor(System.Text.ASCIIEncoding.ASCII.GetBytes(NewKey), newSault);

            MemoryStream memoryStream = new MemoryStream();
            //Defines a stream that links data streams to cryptographic transformations
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(PlainText, 0, PlainText.Length);
            //Writes the final state and clears the buffer
            cryptoStream.FlushFinalBlock();
            byte[] CipherBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();

            string EncryptedData = null;
            switch (outputEncoding)
            {
                case EncryptionTextFormat.Base64:
                    EncryptedData = Convert.ToBase64String(CipherBytes);
                    break;
                case EncryptionTextFormat.UTF8:
                    EncryptedData = System.Text.UTF8Encoding.UTF8.GetString(CipherBytes);
                    break;
                case EncryptionTextFormat.URL:
                    EncryptedData = System.Web.HttpUtility.UrlEncode(CipherBytes);
                    break;
                case EncryptionTextFormat.Ascii:
                    EncryptedData = Encoding.ASCII.GetString(CipherBytes);
                    break;
                case EncryptionTextFormat.Hex:
                    EncryptedData = EncryptAsHexa(CipherBytes);
                    break;
                default:
                    EncryptedData = System.Text.UTF8Encoding.UTF8.GetString(CipherBytes);
                    break;
            }
            return EncryptedData;
        }



        public static string EncryptAsHexa(byte[] text)
        {
            StringBuilder oBuilder = new StringBuilder();
            foreach (byte b in text)
                oBuilder.AppendFormat("{0:x2}", b);
            return oBuilder.ToString();
        }

        public static byte[] DecryptFromHexa(string text)
        {
            int NumberChars = text.Length;
            byte[] result = new byte[NumberChars / 2];
            for (int i = 0; i <= NumberChars - 1; i = i + 2)
                result[i / 2] = Convert.ToByte(text.Substring(i, 2), 16);
            return result;
        }
    }
}
