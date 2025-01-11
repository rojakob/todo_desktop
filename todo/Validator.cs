using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace todo
{
    internal class Validator
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false; // Пустая строка не является валидным email
            }

            // Используем регулярное выражение для проверки паттерна "*@*.*"
            string pattern = @"^.+@.+\..+$";
            return Regex.IsMatch(email, pattern);
        }

        public static bool IsValidPassword(string pass)
        {
            if (pass.Length < 6)
            {
                return false; // Пустая строка не может быть паролем
            }

            return true;

        }
        public static bool IsValidName(string name)
        {
            if (name.Length < 3)
            {
                return false;
            }

            return true;
        }

        public static bool IsMatchPass(string pass1, string pass2)
        {
            if (pass1 != pass2)
            {
                return false;
            }

            return true;
        }
    }
}
