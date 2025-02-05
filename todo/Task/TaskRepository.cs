using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using todo.ApplicationData;

namespace todo.Task
{
    public static class TaskRepository
    {

        public static todoEntities context = new todoEntities();

        // Метод для добавления задачи в базу данных
        public static void AddTask(string name, string description, string category, string date, string time, bool completed, int idUsera)
        {

            // Создаем новый объект TaskModel
            var newTask = new TaskModel
            {
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
        public static void DeleteTaskByName(string taskName)
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
            }
        }
    }
}
