using MSSQLCrudExample.Repositories;
using MSSQLCrudExample.Repositories.Interfaces;
using MSSQLCrudExample.Services;

class Program
{
    static void Main(string[] args)
    {

        ITaskRepository taskRepository = new TaskRepository();

        var taskServise = new TaskServiсe(taskRepository);
        taskServise.Run();
    }
}