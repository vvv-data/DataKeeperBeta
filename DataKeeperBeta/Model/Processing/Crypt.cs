using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System;

namespace Model
{
    namespace Processing
    {
        static class Crypt
        {

            static public string key = null;



            //создаем длинный ключ на основе пароля и имени файла
            public static string SetKey(int length, string fileName, string catalog)
            {
                StringBuilder stringKey = new StringBuilder();
                StringBuilder stringSource = new StringBuilder(ControlPass.Password); // получаем исходный пароль в stringSource
                stringSource.Append(Config.Version); // прибавляем версию базы
                stringSource.Append(catalog); // прибавляем дирректорию файла
                for (int i = 0; stringKey.Length <= length; i++)
                {
                    for (int j = 0; j < i + 1; j++)
                    {
                        if (j % 2 == 0)
                        {
                            stringSource.Append(fileName);
                            if (j != 0)
                                stringSource.Remove(0, fileName.Length - 1);
                        }
                        else
                        {
                            stringSource.Insert(0, fileName);
                            if (j != 0)
                                stringSource.Remove(stringSource.Length - fileName.Length, fileName.Length - 1);
                            
                        }

                    }
                    stringKey.Append(GetHash(stringSource.ToString()));
                }
                key = stringKey.ToString();
                return key;
            }


            //метод шифрования/дешифровки
            private static string Crypto(string text, string key)
            {
                var currentKey = key;
                var res = string.Empty;
                for (var i = 0; i < text.Length; i++)
                {
                    res += ((char)(text[i] ^ currentKey[i])).ToString();
                }
                return res;
            }

            //шифрование и расшифровка текста 
            public static string Cryption(int id, string catalog, string str)
            {
                return Crypto(str, SetKey(str.Length, id.ToString(), catalog));
            }


            static public string GetHash(string pass)
            {
                // 2 раза в md5 и убираем символ ==
                var md5 = MD5.Create();
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(pass));
                var hash2 = Convert.ToBase64String(hash);

                // преобразуем в строку и удаляем символ ==
                return Regex.Replace(Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(hash2))), "==", "");
            }

        }
    }
}

