using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace todo.Repository
{
    internal class UserRepository
    {
        private static UserRepository UserRepositoryInstance;

        
        public static UserRepository GetInstance()
        {
            if (UserRepositoryInstance == null)
            {
                UserRepositoryInstance = new UserRepository();
            }

            return UserRepositoryInstance;
        }

        private List<UserModel> users = new List<UserModel>();



        public static UserModel currentUser;



        public UserModel GetUserByEmail(string email)
        {
            return users.Find((user) => user.Email == email);
        }

        public UserModel Register(UserModel user, string pass2)
        {
            if (!Validator.IsMatchPass(user.Pass,pass2))
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
            
            users.Add(user);
            currentUser = user;
            return user;
        }

        public UserModel Login(string email, string password)
        {
            var userResult = GetUserByEmail(email);

            if (userResult == null)
            {
                throw new Exception("Неверный e-mail!");
            }

            if (userResult.Pass != password)
            {
                throw new Exception("Неверный пароль!");
            }

            return userResult;
        }

    }
}
