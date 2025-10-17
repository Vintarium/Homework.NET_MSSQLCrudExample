using System;
using System.Data;
using Microsoft.Data.SqlClient;
using MSSQLCrudExample;
using MSSQLCrudExample.Config;
using MSSQLCrudExample.Models;
using MSSQLCrudExample.Repositories.Interfaces;
using Dapper;

namespace MSSQLCrudExample.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly string _connectionString;

        public TaskRepository()
        {
            _connectionString = DatabaseConfig.ConnectionString;
        }

        public void CreateTable()
        {
            using var connection = new SqlConnection(_connectionString);

            var createDbQuery = @"
                IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'TaskManager')
                 BEGIN
                     CREATE DATABASE TaskManager;
                END";

            var createTableQuery = @"
                IF NOT EXISTS (SELECT * FROM TaskManager.sys.tables WHERE name = 'Tasks')
                 BEGIN
                 CREATE TABLE TaskManager.dbo.Tasks (
                     Id INT IDENTITY(1,1) PRIMARY KEY,
                     Title NVARCHAR(100) NOT NULL,
                     Description NVARCHAR(100) NOT NULL,
                     IsCompleted BIT NOT NULL DEFAULT 0,
                     CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
                    )
                END";

            connection.Execute(createDbQuery);
            connection.Execute(createTableQuery);
        }

        public void Add(TaskItem task)
        {
            using var connection = new SqlConnection(_connectionString);

            var query = @"INSERT INTO TaskManager.dbo.Tasks (Title, Description, IsCompleted) 
                            VALUES (@Title, @Description, @IsCompleted)";

            connection.Execute(query, new
            {
                task.Title,
                task.Description,
                task.IsCompleted
            });

        }

        public List<TaskItem> GetAll()
        {
            var tasks = new List<TaskItem>();

            using var connection = new SqlConnection(_connectionString);

            var query = @"SELECT * FROM TaskManager.dbo.Tasks ORDER BY CreatedAt DESC";

            return connection.Query<TaskItem>(query).ToList();
        }

        public TaskItem? GetById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "SELECT * FROM TaskManager.dbo.Tasks WHERE Id = @Id";

            return connection.QueryFirstOrDefault<TaskItem>(query, new { Id = id });
        }
        public void Update(TaskItem task)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"UPDATE TaskManager.dbo.Tasks SET Title = @Title, Description = @Description,
                            IsCompleted = @IsCompleted WHERE Id = @Id";

            connection.Execute(query, new
            {
                task.Id,
                task.Title,
                task.Description,
                task.IsCompleted
            });
        }

        public void Delete(int id)
        {
            var connection = new SqlConnection(_connectionString);
            var query = @"DELETE FROM TaskManager.dbo.Tasks WHERE Id = @Id";
            connection.Execute(query, new { Id = id });
        }

        public void MarkAsCompleted(int id, bool isCompleted)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "UPDATE TaskManager.dbo.Tasks SET IsCompleted = @IsCompleted WHERE Id = @Id";

            connection.Execute(query, new { Id = id, IsCompleted = isCompleted });
        }
    }
}
