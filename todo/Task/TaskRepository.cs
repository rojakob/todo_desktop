using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace todo.Task
{
    public static class TaskRepository
    {
        public static TaskRepos TaskRepo { get; set; } = new TaskRepos();
    }
}
