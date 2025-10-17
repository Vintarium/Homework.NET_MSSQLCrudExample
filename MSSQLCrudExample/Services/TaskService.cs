using System;
using MSSQLCrudExample.Repositories.Interfaces;
using MSSQLCrudExample.Repositories;
using MSSQLCrudExample.Models;


namespace MSSQLCrudExample.Services
{
    public class TaskService

    {
        private readonly ITaskRepository _taskRepository;
        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public void CreateDB_And_Table()
        {
            _taskRepository.CreateTable();
        }

        public void ShowAllTasks()
        {
            Console.WriteLine("\n--- СПИСОК ЗАДАЧ ---");
            Console.WriteLine();
            var tasks = _taskRepository.GetAll();
            if (tasks.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("Задач нет!");
                return;
            }
            foreach (var task in tasks)
            {
                var status = task.IsCompleted ? "ВЫПОЛНЕНА" : "В РАБОТЕ";
                Console.WriteLine($"ID: {task.Id} | {status} | {task.Title}");
                Console.WriteLine($"    Описание: {task.Description}");
                Console.WriteLine($"    Создана: {task.CreatedAt:dd.MM.yyyy HH:mm}");
                Console.WriteLine();
                Console.WriteLine("-----------------");
                Console.WriteLine();
            }
        }

        public void AddNewTask()
        {
            Console.Write("Введите заголовок задачи: ");
            var taskTitle = inputStringValidate();

            Console.Write("Введите описание задачи: ");
            var taskDescription = inputStringValidate();

            var newTask = new TaskItem
            {
                Title = taskTitle,
                Description = taskDescription,
                IsCompleted = false
            };
            _taskRepository.Add(newTask);
        }

        public void UpdateTaskStatus()
        {
            Console.WriteLine("\n--- ИЗМЕНЕНИЕ СТАТУСА ЗАДАЧИ ---");
            ShowAllTasks();

            while (true)
            {
                Console.Write("Введите id задачи :");
                var id = inputIdValidate();

                var taskId = _taskRepository.GetById(id);

                Console.Write("Задача выполнена? (y/n): ");
                var input = inputStringValidate().ToLower();

                if (input == "y" || input == "n")
                {
                    switch (input)
                    {
                        case "y":
                            _taskRepository.MarkAsCompleted(id, true);
                            Console.WriteLine($"Статус задачи обновлен на: 'ВЫПОЛНЕНА' )");
                            break;
                        case "n":
                            _taskRepository.MarkAsCompleted(id, false);
                            Console.WriteLine($"Статус задачи обновлен на: 'В РАБОТЕ' )");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Введен не кореектный ответ!");
                    Console.Write("Попробуйте еще раз: ");
                    continue;
                }
                break;
            }
        }

        public void DeleteTask()
        {
            Console.WriteLine("\n--- УДАЛЕНИЕ ЗАДАЧИ ---");
            ShowAllTasks();

            Console.Write("Введите id задачи :");
            var id = inputIdValidate();

            _taskRepository.Delete(id);
            Console.WriteLine();
            Console.WriteLine("Задача успешно удалена!");
            Console.WriteLine();
        }

        private string inputStringValidate()
        {
            while (true)
            {
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Введена пустая строка!");
                    Console.Write("Попробуйте еще раз: ");
                    continue;
                }

                if (input.Length > 100)
                {
                    Console.WriteLine("Максимальное количество символов для ввода : 100.");
                    Console.WriteLine("Вы превысили это ограничение.");
                    Console.Write("Попробуйте еще раз: ");
                    continue;
                }
                else
                {
                    return input.Trim();
                }
            }
        }
        private int inputIntValidate()
        {
            while (true)
            {
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out int id))
                {
                    Console.WriteLine("Ошибка: ID должен быть числом!");
                    continue;
                }
                else
                {
                    return id;
                }
            }
        }

        private int inputIdValidate()
        {
            while (true)
            {
                var id = inputIntValidate();
                var taskById = _taskRepository.GetById(id);
                if (taskById == null)
                {
                    Console.WriteLine("Ошибка: задачи с таким ID не существует!");
                    Console.Write("Попробуйте еще раз: ");
                    continue;
                }
                else
                {
                    return id;
                }
            }
        }
    }
}
