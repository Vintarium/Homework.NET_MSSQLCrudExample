using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSSQLCrudExample.Services;

namespace MSSQLCrudExample.Controllers
{
    public class MenuController
    {
        private readonly TaskService _taskService;
        public MenuController(TaskService taskService)
        {
            _taskService = taskService;
        }

        public void Run()
        {
            Console.WriteLine("=== СИСТЕМА УПРАВЛЕНИЯ ЗАДАЧАМИ ===");
            _taskService.CreateDB_And_Table(); 

            bool manager = true;

            while (manager)
            {
                ShowMenu();
                var command = Console.ReadLine();
                switch (command)
                {
                    case "1": _taskService.ShowAllTasks(); break;
                    case "2": _taskService.AddNewTask(); break;
                    case "3": _taskService.UpdateTaskStatus(); break;
                    case "4": _taskService.DeleteTask(); break;
                    case "0": manager = false; Console.WriteLine("Спасибо что использовали наш сервис!"); break;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("Неверный выбор!"); break;
                }
            }
        }

        private void ShowMenu()
        {
            Console.WriteLine("\n=== МЕНЮ ===");
            Console.WriteLine("1 - Показать все задачи");
            Console.WriteLine("2 - Добавить задачу");
            Console.WriteLine("3 - Обновить статус задачи");
            Console.WriteLine("4 - Удалить задачу");
            Console.WriteLine("0 - Выход");
            Console.WriteLine();
            Console.Write("Выберите действие: ");
        }
    }
}