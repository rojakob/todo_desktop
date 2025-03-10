using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using todo.ApplicationData;

namespace todo.Repository
{
    internal class UserRepository
    {

        public static UserModel GetUserByToken(string token)
        {
            using (var context = new todoEntities())
            {
                // Проверяем, есть ли задачи у пользователя с указанным ID
                UserModel userResult = context.UserModel.FirstOrDefault(user => user.Token == token);
                currentUser = userResult;
                return userResult;
            }
        }

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

        public async Task<bool> RegisterAPI(string name, string email, string password)
        {
            // URL для авторизации
            string apiUrl = "http://45.144.64.179/api/auth/registration";

            // Создаем тело запроса в формате JSON
            var requestBody = new
            {
                Name = name,
                Email = email,
                Password = password
            };

            // Сериализуем тело запроса в JSON
            string jsonBody = JsonSerializer.Serialize(requestBody);


            // Создаем HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Устанавливаем заголовок Content-Type для JSON
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // Создаем содержимое запроса
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                try
                {
                    // Выполняем POST-запрос
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    // Проверяем, успешен ли запрос
                    if (response.IsSuccessStatusCode)
                    {
                        // Читаем ответ как строку
                        string responseData = await response.Content.ReadAsStringAsync();



                        // Десериализуем ответ в объект
                        var responseObject = JsonSerializer.Deserialize<LoginResponse>(responseData);

                        // Получаем токен из ответа
                        string accessToken = responseObject.data.access_token;

                        string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                        string appFolder = System.IO.Path.Combine(localAppDataPath, "todo");
                        Directory.CreateDirectory(appFolder);
                        string filePath = System.IO.Path.Combine(appFolder, "token.json");
                        var tokenData = new { Token = accessToken };
                        string jsonAuthFile = JsonSerializer.Serialize(tokenData);
                        File.WriteAllText(filePath, jsonAuthFile);


                        // Сохраняем токен в базу данных
                        using (var context = new todoEntities())
                        {
                            var User = context.UserModel.FirstOrDefault(user => user.Email == email);
                            User.Token = accessToken.ToString();
                            context.SaveChanges();
                        }

                        return true; // Успешная авторизация
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: " + response.StatusCode);
                        return false; // Ошибка авторизации
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Исключение: " + ex.Message);
                    return false; // Ошибка при выполнении запроса
                }
            }
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

        // Класс для десериализации ответа от API
        public class LoginResponse
        {
            public Data data { get; set; }
        }

        public class Data
        {
            public string access_token { get; set; }
        }

        public async Task<bool> LoginAPI(string email, string password)
        {
            // URL для авторизации
            string apiUrl = "http://45.144.64.179/api/auth/login";

            // Создаем тело запроса в формате JSON
            var requestBody = new
            {
                Email = email,
                Password = password
            };

            // Сериализуем тело запроса в JSON
            string jsonBody = JsonSerializer.Serialize(requestBody);


            // Создаем HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Устанавливаем заголовок Content-Type для JSON
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // Создаем содержимое запроса
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                try
                {
                    // Выполняем POST-запрос
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    // Проверяем, успешен ли запрос
                    if (response.IsSuccessStatusCode)
                    {
                        // Читаем ответ как строку
                        string responseData = await response.Content.ReadAsStringAsync();



                        // Десериализуем ответ в объект
                        var responseObject = JsonSerializer.Deserialize<LoginResponse>(responseData);

                        // Получаем токен из ответа
                        string accessToken = responseObject.data.access_token;

                        string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                        string appFolder = System.IO.Path.Combine(localAppDataPath, "todo");
                        Directory.CreateDirectory(appFolder);
                        string filePath = System.IO.Path.Combine(appFolder, "token.json");
                        var tokenData = new { Token = accessToken };
                        string jsonAuthFile = JsonSerializer.Serialize(tokenData);
                        File.WriteAllText(filePath, jsonAuthFile);

                        // Сохраняем токен в базу данных
                        using (var context = new todoEntities())
                        {
                            var User = context.UserModel.FirstOrDefault(user => user.Email == email);
                            User.Token = accessToken.ToString();
                            context.SaveChanges();
                        }

                        return true; // Успешная авторизация
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: " + response.StatusCode);
                        return false; // Ошибка авторизации
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Исключение: " + ex.Message);
                    return false; // Ошибка при выполнении запроса
                }
            }
        }

        public async static void GetUserInfo()
        {
            string token = currentUser.Token;

            string url = "http://45.144.64.179/api/user";

            using (HttpClient client = new HttpClient())
            {
                // Устанавливаем хедер авторизации
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                try
                {
                    // Создаем объект класса HttpResponseMessage который будет содержать в себе ответ от сервера после гет запроса 
                    HttpResponseMessage response = await client.GetAsync(url);

                    // Читаем ответ как строку
                    string responseData = await response.Content.ReadAsStringAsync();

                    //Выведем ответ в консоль, чтобы чекнуть ответ самостоятельно
                    Console.WriteLine(responseData);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Исключение: " + ex.Message);
                }
            }
        }
    }



}

