using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using todo.ApplicationData;
using static todo.Repository.UserRepository;

namespace todo.Task
{
    public class TaskRepository
    {

        public static todoEntities context = new todoEntities();

        public class ApiResponse
        {
            public List<TaskResponse> data { get; set; }
        }

        public class TaskResponse
        {
            public string id { get; set; }
            public string category { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public int date { get; set; }
            public bool isCompleted { get; set; }
            public CoordinateResponse coordinate { get; set; }
        }

        public class TaskRequest
        {
            public string Category { get; set; }       // Категория задачи
            public string Title { get; set; }          // Заголовок задачи
            public string Description { get; set; }    // Описание задачи
            public long Date { get; set; }             // Дата (временная метка)
            public Coordinate Coordinate { get; set; } // Координаты
        }

        public class Coordinate
        {
            public string Longitude { get; set; } // Долгота
            public string Latitude { get; set; }  // Широта
        }

        public class CoordinateResponse
        {
            public string longitude { get; set; } // Долгота
            public string latitude { get; set; }  // Широта
        }

        public class LoginResponse
        {
            public Data data { get; set; }
        }

        public class Data
        {
            public string id { get; set; }
        }

        public static async Task<bool> AddTaskAPI(string name, string description, string category)
        {
            // URL для авторизации
            string apiUrl = "http://45.144.64.179/api/todos";

            // Создаем тело запроса в формате JSON
            var requestBody = new
            {
                Category = category,
                Title = name,
                Description = description,
                Date = 0, // Временная метка (например, Unix-время)
                Coordinate = new Coordinate
                {
                    Longitude = "50.4501",
                    Latitude = "30.5234"
                }
            };

            // Сериализуем тело запроса в JSON
            string json = JsonSerializer.Serialize(requestBody, new JsonSerializerOptions { WriteIndented = true });

            string token = currentUser.Token;

            // Создаем HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Устанавливаем заголовок авторизации 
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Устанавливаем заголовок Content-Type для JSON
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // Создаем содержимое запроса
                var content = new StringContent(json, Encoding.UTF8, "application/json");

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

                        Console.WriteLine(responseData);

                        // Получаем токен из ответа
                        string id = responseObject.data.id;

                        // Сохраняем id задачи в базу данных
                        using (var context = new todoEntities())
                        {
                            var taskForSetId = context.TaskModel.FirstOrDefault(t => t.Name == name);
                            taskForSetId.Id_Task = id;
                            context.SaveChanges();
                        }

                        return true; // Задача добавлена успешно
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: " + response.StatusCode);
                        return false; // Ошибка добавления задачи
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Исключение: " + ex.Message);
                    return false; // Ошибка при выполнении запроса
                }
            }
        }


        // Метод для добавления задачи в базу данных
        public static void AddTask(string name, string description, string category, string date, string time, bool completed, int idUsera)
        {

            // Создаем новый объект TaskModel
            var newTask = new TaskModel
            {
                Id_Task = "что-то",
                Name = name,
                Description = description,
                Category = category,
                Date = date,
                Time = time,
                Completed = completed,
                Id_usera = idUsera
            };

            // Добавляем задачу в таблицу TaskModel
            context.TaskModel.Add(newTask);
            // Сохраняем изменения в базе данных
            context.SaveChanges();

        }

        // Метод для удаления задачи из базы данных по названию
        public async static void DeleteTaskByName(string taskName)
        {
            using (var context = new todoEntities())
            {
                // Находим задачу по названию
                var taskToDelete = context.TaskModel.FirstOrDefault(t => t.Name == taskName);

                // Проверяем, найдена ли задача
                if (taskToDelete != null)
                {
                    // Удаляем задачу из таблицы TaskModel
                    context.TaskModel.Remove(taskToDelete);
                    // Сохраняем изменения в базе данных
                    context.SaveChanges();
                }
                else
                {
                    // Если задача не найдена, выбрасываем исключение или выводим сообщение
                    throw new Exception("Задача с указанным названием не найдена!");
                }
                string apiUrl = "http://45.144.64.179/api/todos/" + taskToDelete.Id_Task;

                string token = currentUser.Token;

                using (HttpClient client = new HttpClient())
                {
                    // Устанавливаем заголовок авторизации 
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    try
                    {
                        HttpResponseMessage response = await client.DeleteAsync(apiUrl);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Исключение: " + ex.Message);
                    }

                    
                }
            }
        }

        public async static void TaskIsDoneAPI(string taskName)
        {
            using (var context = new todoEntities())
            {
                // Находим задачу по названию
                var taskToDone = context.TaskModel.FirstOrDefault(t => t.Name == taskName);

                // Создаем юрл по которому будет отправлен пут запрос на отметку задачи 
                string apiUrl = "http://45.144.64.179/api/todos/mark/" + taskToDone.Id_Task;

                //Берем токен текущего юзера, чтобы пихнуть его в хедер авторизации
                string token = currentUser.Token;

                using (HttpClient client = new HttpClient())
                {
                    // Устанавливаем заголовок авторизации 
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var jsonString = "";
                    var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    try
                    {
                        HttpResponseMessage response = await client.PutAsync(apiUrl, httpContent);

                        if (response.IsSuccessStatusCode)
                        {
                            // Читаем ответ как строку
                            string responseData = await response.Content.ReadAsStringAsync();
                            Console.WriteLine("Удалось сделать отметку о выполнении" + responseData);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Исключение: " + ex.Message);
                    }
                }
            }
        }

        public async static Task<List<TaskResponse>> GetAllUserTasks()
        {
            string token = currentUser.Token;

            string url = "http://45.144.64.179/api/todos";

            using (HttpClient client = new HttpClient())
            {
                // Устанавливаем хедер авторизации
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                try
                {
                    // Создаем объект класса HttpResponseMessage который будет содержать в себе ответ от сервера после гет запроса 
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        // Читаем ответ как строку
                        string responseData = await response.Content.ReadAsStringAsync();

                        //Выведем ответ в консоль, чтобы чекнуть ответ самостоятельно
                        Console.WriteLine(responseData);

                        var apiResponse = JsonSerializer.Deserialize<ApiResponse>(responseData);

                        // Возвращаем список задач
                        return apiResponse.data;


                    }
                    else
                    {
                        Console.WriteLine("Ошибка: " + response.StatusCode);
                        return null; // Возвращаем null в случае ошибки
                    }

                   
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Исключение: " + ex.Message);
                    return null; // Возвращаем null в случае исключения
                }
            }
        }
    }
}
