using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace todo.Task
{
    public class TaskRepos
    {
        public List<TaskModel> tasks = new List<TaskModel>();

        private int nextId = 0; // Для генерациии уникальных айди
        public void AddTask(string name, string description, string category, string date, string time)
        {

           bool completed = false;

            TaskModel newTask = new TaskModel
            {
                Id = nextId++,
                Name = name,
                Description = description,
                Category = category,
                Date = date, // Объединяем дату и время
                Time = time,
                Completed = completed
            };

            // 4. Добавляем новую задачу в список.
            tasks.Add(newTask);
        }


        public void DeleteTask(TaskModel task)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].Id == task.Id)
                {
                    tasks.RemoveAt(i);
                    return;
                }
            }
        }
    }
}
