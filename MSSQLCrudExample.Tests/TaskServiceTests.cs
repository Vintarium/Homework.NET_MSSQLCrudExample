using Moq;
using MSSQLCrudExample.Models;
using MSSQLCrudExample.Repositories.Interfaces;
using MSSQLCrudExample.Services;

namespace MSSQLCrudExample.Tests
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _mockRepo;
        private readonly TaskServiсe _taskServise;

        public TaskServiceTests()
        {

            _mockRepo = new Mock<ITaskRepository>();
            _taskServise = new TaskServiсe(_mockRepo.Object);

        }

        [Fact]
        public void GetAllTasksWhenNoTasks()
        {
            // ARRANGE
            _mockRepo.Setup(repo => repo.GetAll()).Returns(new List<TaskItem>());

            // ACT
            _taskServise.ShowAllTasks();

            // ASSERT
            _mockRepo.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public void ShowAllTasks_WithTasks_ShouldDisplayCorrectInfo()
        {
            // ARRANGE
            var testTasks = new List<TaskItem>
            {
                new TaskItem
                {
                    Id = 1,
                    Title = "Помыть посуду",
                    Description = "Помыть всю посуду на кухне",
                    IsCompleted = false,
                    CreatedAt = new DateTime(2024, 1, 15, 10, 30, 0)
                }
            };

            _mockRepo.Setup(repo => repo.GetAll()).Returns(testTasks);

            // ACT
            string output = CaptureConsoleOutput(() => _taskServise.ShowAllTasks());

            // ASSERT
            _mockRepo.Verify(repo => repo.GetAll(), Times.Once);
            Assert.Contains("ID: 1", output);
            Assert.Contains("Помыть посуду", output);
            Assert.Contains("В РАБОТЕ", output);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(999, false)]
        public void GetById_ShouldReturnCorrectResult(int taskId, bool shouldExist)
        {
            // ARRANGE
            var expectedTask = shouldExist ?
                new TaskItem { Id = taskId, Title = "Тест задача" } :
                null;

            _mockRepo.Setup(repo => repo.GetById(taskId)).Returns(expectedTask);

            // ACT
            var result = _mockRepo.Object.GetById(taskId);

            // ASSERT
            if (shouldExist)
            {
                Assert.NotNull(result);
                Assert.Equal(taskId, result.Id);
            }
            else
            {
                Assert.Null(result);
            }

            _mockRepo.Verify(repo => repo.GetById(taskId), Times.Once);
        }

        private string CaptureConsoleOutput(Action action)
        {
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            action();
            return stringWriter.ToString();
        }
    }
}