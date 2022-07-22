
using System;
using Shared;

namespace Model
{
    namespace Processing
    {
        static class ControlPass
        {

            private static string password = null;
            public static string Password { get { return password; } }
            static string message = "Придумайте пароль. Пароль должен быть не менее " + Limits.passwordMin + " символов, и не более " + Limits.passwordMax + ". Содержать: цифры; прописные и строчные буквы.";

            static string GetPassFile()
            {
                // получаем текст пароля из файла
                return DataBaseConnect.GetContentFile(Config.file_Pass);
            }

            // проверяет есть ли в файле пароль
            static public bool PassExists()
            {

                // проверяем есть ли файл pass
                if (DataBaseConnect.FileExists(Config.file_Pass) && !String.IsNullOrEmpty(GetPassFile()))
                        return true;
                else
                {
                    Message.Warning = message;
                    return false;
                }


            }



            // требуем ввести пароль при входе
            static public bool PassLogin(string pass)
            {
                // проверяем есть ли пароль, и сравниваем закодированный
                if (!String.IsNullOrEmpty(pass) && PassExists() && GetPassFile() == Crypt.GetHash(pass))
                {
                    password = pass;

                    Message.Success = "Вы успешно вошли";
                    Message.Error = null;
                    Message.Warning = null;

                    return true;
                }
                else 
                    Message.Error = "Пароль введен не верно, введите еще раз";

                return false;
            }


            // добавление пароля в первый раз
            static public bool AddPass(string pass, byte tip = 0)
            {                
                string message = "Ошибка программы";

                //if (tip == 0) message = "Придумайте свой пароль";
                //else if (tip == 1) message = "Придумайте свой новый пароль";


                bool ok = ControlAddPass(pass);

                if (tip == 0) message = "Пароль успешно сохранен. Ведите его еще раз для авторизации";
                else if (tip == 1) message = "Пароль успешно изменен. Ведите его еще раз для авторизации";

                if (ok)
                {
                    // кодируем и добавляем пароль в файл
                    pass = Crypt.GetHash(pass);

                    if (DataBaseConnect.SetContentFile(Config.file_Pass, pass))
                    {
                        Message.Success = message;
                        return true;
                    }
                    else
                    {
                        Message.Error = "Не удалось сохранить пароль, попробуйте повторить.";
                        return false;
                    }
                }                           
                return false;
            }


            static bool ControlAddPass(string pass)
            {

                if (pass == null)
                {
                    Message.Error = "Вы ввели пустой пароль";
                    return false;
                }

                bool is_Letter = false, is_LetterUp = false, is_Digit = false, is_LengthMin = false, is_LengthMax = false, is_Space = true;

                if (pass.Length >= Limits.passwordMin) is_LengthMin = true;
                if (pass.Length <= Limits.passwordMax) is_LengthMax = true;

                foreach (char c in pass)
                {                   

                    if(Char.IsWhiteSpace(c)) is_Space = false;

                    if ((Char.IsLetter(c)) && (Char.IsLower(c))) is_Letter = true;

                    if ((Char.IsLetter(c)) && (Char.IsUpper(c))) is_LetterUp = true;

                    if (Char.IsDigit(c)) is_Digit = true;

                }

                if (is_Letter && is_LetterUp && is_Digit && is_LengthMin && is_LengthMax && is_Space)
                {
                    return true;
                }
                else
                {
                    message = "Пароль не соответствует:";
                    if (!is_LengthMax) message += " должно быть не более " + Limits.passwordMax + " символов;";
                    if (!is_LengthMin) message += " должно быть не менее " + Limits.passwordMin + " символов;";
                    if (!is_Letter) message += " должны содержаться строчные буквы;";
                    if (!is_LetterUp) message += " должны содержаться прописные буквы;";
                    if (!is_Digit) message += " должны содержаться цифры;";
                    if (!is_Space) message += " не должно содержаться пробелов;";
                    Message.Error = message;
                    return false;
                }

            }


        }
       
    }    
}

