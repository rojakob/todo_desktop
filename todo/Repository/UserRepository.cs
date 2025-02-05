using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using todo.ApplicationData;

namespace todo.Repository
{
    internal class UserRepository
    {

        public static bool UserHasTasks(int userId)
        {
            using (var context = new todoEntities())
            {
                // Проверяем, есть ли задачи у пользователя с указанным ID
                return context.TaskModel.Any(t => t.Id_usera == userId);
            }
        }

        private static UserRepository UserRepositoryInstance;

        public static UserRepository GetInstance()
        {
            if (UserRepositoryInstance == null)
            {
                UserRepositoryInstance = new UserRepository();
            }

            return UserRepositoryInstance;
        }

        public static UserModel currentUser;

        public UserModel GetUserByEmail(string email)
        {
            using (var context = new todoEntities())
            {
                return context.UserModel.FirstOrDefault(user => user.Email == email);
            }
        }

        public UserModel Register(UserModel user, string pass2)
        {
            if (!Validator.IsMatchPass(user.Pass, pass2))
            {
                throw new Exception("Пароли не совпадают!");
            }

            if (!Validator.IsValidEmail(user.Email))
            {
                throw new Exception("Неверный формат Email!");
            }

            if (!Validator.IsValidPassword(user.Pass))
            {
                throw new Exception("Недопустимый пароль!");
            }

            if (!Validator.IsValidName(user.Name))
            {
                throw new Exception("Недопустимое имя!");
            }

            if (GetUserByEmail(user.Email) != null)
            {
                throw new Exception("Пользователь с таким e-mail уже существует!");
            }

            using (var context = new todoEntities())
            {
                context.UserModel.Add(user);
                context.SaveChanges();
            }

            currentUser = user;
            return user;
        }

        public UserModel Login(string email, string password)
        {
            var userResult = GetUserByEmail(email);

            if (userResult == null)
            {
                throw new Exception("Такого пользователя нет в базе!");
            }

            if (userResult.Pass != password)
            {
                throw new Exception("Неверный пароль!");
            }

            currentUser = userResult;
            return userResult;
        }
    }
}
