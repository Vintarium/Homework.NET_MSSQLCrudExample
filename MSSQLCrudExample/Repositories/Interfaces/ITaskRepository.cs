using System;
using MSSQLCrudExample.Models;

namespace MSSQLCrudExample.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        void CreateTable();
        List<TaskItem> GetAll();
        TaskItem? GetById(int id);
        void Add(TaskItem task);
        void Update(TaskItem task);
        void Delete(int id);
        void MarkAsCompleted(int id, bool isCompleted);
    }
}
