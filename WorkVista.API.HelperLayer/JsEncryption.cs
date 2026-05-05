using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WorkVista.API.HelperLayer
{
    public static class JsEncryption
    {
        public static string EncryptStringAESToModel(object anonymousTypeModel, string jsEncrytionKey)
        {
            try
            {
                string cipherText = JsonConvert.SerializeObject(anonymousTypeModel, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                string encryptedValue = JsEncryption.EncryptStringAES(cipherText, jsEncrytionKey);

                return encryptedValue;
            }
            catch
            {
                throw new Exception("Invalid data!");
            }
        }

        public static TModel DecryptStringAESToModelDecode<TModel>(string cipherText, string jsEncrytionKey)
        {
            return DecryptStringAESToModel<TModel>(cipherText, jsEncrytionKey);
        }

        public static TModel DecryptStringAESToModel<TModel>(string cipherText, string jsEncrytionKey)
        {
            try
            {
                string decryptedValue = JsEncryption.DecryptStringAES(cipherText, jsEncrytionKey);
                TModel model = JsonConvert.DeserializeObject<TModel>(decryptedValue);

                return model;
            }
            catch
            {
                throw new Exception("Invalid data!");
            }
        }

        public static TAnonymousTypeModel DecryptStringAESToAnonymousTypeModel<TAnonymousTypeModel>(string cipherText, TAnonymousTypeModel anonymousTypeModel, string jsEncrytionKey)
        {
            try
            {
                string decryptedValue = JsEncryption.DecryptStringAES(cipherText, jsEncrytionKey);
                TAnonymousTypeModel model = JsonConvert.DeserializeAnonymousType(decryptedValue, anonymousTypeModel);

                return model;
            }
            catch
            {
                throw new Exception("Invalid data!");
            }
        }

        public static string EncryptStringAES(string cipherText, string jsEncrytionKey)
        {
            try
            {
                string encryptionKey = jsEncrytionKey;


                var keybytes = Encoding.UTF8.GetBytes(encryptionKey);
                var iv = Encoding.UTF8.GetBytes(encryptionKey);

                //string _cipherText = HttpUtility.UrlDecode(cipherText).Replace(" ", "+");
                string _cipherText = cipherText;

                byte[] encriptedFromJavascript = EncryptStringToBytes(_cipherText, keybytes, iv);

                string _encriptedFromJavascript = Convert.ToBase64String(encriptedFromJavascript);

                return _encriptedFromJavascript;
            }
            catch
            {
                return null;
            }
        }

        private static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }

        public static string DecryptStringAES(string cipherText, string jsEncrytionKey)
        {
            try
            {
                string encryptionKey = jsEncrytionKey;


                var keybytes = Encoding.UTF8.GetBytes(encryptionKey);
                var iv = Encoding.UTF8.GetBytes(encryptionKey);

                string _cipherText = HttpUtility.UrlDecode(cipherText).Replace(" ", "+");

                var encrypted = Convert.FromBase64String(_cipherText);
                var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
                //return string.Format(decriptedFromJavascript);
                return decriptedFromJavascript;
            }
            catch
            {
                return null;
            }
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.  
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold  
            // the decrypted text.  
            string plaintext = null;

            // Create an RijndaelManaged object  
            // with the specified key and IV.  
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings  
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.  
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption.  
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream  
                                // and place them in a string.  
                                plaintext = srDecrypt.ReadToEnd();

                            }

                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }
    }
}
