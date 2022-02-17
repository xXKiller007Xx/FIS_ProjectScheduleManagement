using Group_3_BE.Entities;
using Group_3_BE.Enums;
using Group_3_BE.Helpers;
using Group_3_BE.Models;
using Group_3_BE.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace Group_3_BE_Tests.RepositoriesTest
{
    [TestFixture]
    public class TaskRepoTest : CommonTests
    {
        ITaskRepository repository;
        public TaskRepoTest() : base()
        {

        }

        [SetUp]
        public async System.Threading.Tasks.Task Setup()
        {
            Init();
            await Clean();
            repository = new TaskRepository(DataContext);

            DataContext.Statuses.Add(new StatusDAO
            {
                //Id = StatusEnum.ACTIVE.Id,
                Code = StatusEnum.ACTIVE.Code,
                Name = StatusEnum.ACTIVE.Name,
            });
            DataContext.Statuses.Add(new StatusDAO
            {
                //Id = StatusEnum.ACTIVE.Id,
                Code = StatusEnum.INACTIVE.Code,
                Name = StatusEnum.INACTIVE.Name,
            });

            DataContext.SaveChanges();

            DataContext.TaskTypes.Add(new TaskTypeDAO
            {
                Id = 0,
                Code = "type1",
                Name = "loai cong viec 1",
                Description = "Mo ta 1",
                CreatedAt = StaticParams.DateTimeNow,
                UpdatedAt = StaticParams.DateTimeNow,
                StatusId = StatusEnum.ACTIVE.Id,
            });
            DataContext.TaskTypes.Add(new TaskTypeDAO
            {
                //Id = StatusEnum.ACTIVE.Id,
                Code = "type2",
                Name = "loai cong viec 2",
                Description = "Mo ta 2",
                CreatedAt = StaticParams.DateTimeNow,
                UpdatedAt = StaticParams.DateTimeNow,
                StatusId = StatusEnum.ACTIVE.Id,
            });

            DataContext.Projects.Add(new ProjectDAO
            {
                Id = 0,
                Code = "Project1",
                Name = "Project 1",
                Description = "mo ta prj 1",
                StartDate = StaticParams.DateTimeNow,
                FinishDate = new DateTime(2022, 12, 12),
                Percentage = 0,
                CreatedAt = StaticParams.DateTimeNow,
                UpdatedAt = StaticParams.DateTimeNow,
                Used = false,
                StatusId = StatusEnum.ACTIVE.Id,
            });

            DataContext.SaveChanges();
        }

        // Create
        [Test]
        public async System.Threading.Tasks.Task Task_Create_ReturnTrue()
        {
            Task Input = new Task
            {
                Id = 0,
                Code = "Task1",
                Name = "Cong viec 1",
                Description = "Mo ta cv 1",
                StartDate = DateTime.Now,
                FinishDate = new DateTime(2022, 12, 12),
                Percentage = 0,
                TaskTypeId = 1,
                ProjectId = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                StatusId = StatusEnum.ACTIVE.Id,
                Used = false,
            };
            await repository.Create(Input);

            var Output = DataContext.Tasks.Where(x => x.Id == Input.Id).FirstOrDefault();
            Assert.AreEqual(Input.Id, Output.Id);
            Assert.AreEqual(Input.Code, Output.Code);
            Assert.AreEqual(Input.Name, Output.Name);
            Assert.AreEqual(Input.Description, Output.Description);
            Assert.AreEqual(Input.StartDate.ToString("dd-MM-yyyy HH:mm:ss"), Output.StartDate.ToString("dd-MM-yyyy HH:mm:ss"));
            Assert.AreEqual(Input.FinishDate.ToString("dd-MM-yyyy HH:mm:ss"), Output.FinishDate.ToString("dd-MM-yyyy HH:mm:ss"));
            Assert.AreEqual(Input.Percentage, Output.Percentage);
            Assert.AreEqual(Input.ProjectId, Output.ProjectId);
            Assert.AreEqual(Input.TaskTypeId, Output.TaskTypeId);
            Assert.AreEqual(Input.CreatedAt.ToString("dd-MM-yyyy HH:mm:ss"), Output.CreatedAt.ToString("dd-MM-yyyy HH:mm:ss"));
            Assert.AreEqual(Input.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"), Output.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"));
            Assert.AreEqual(Input.Used, Output.Used);
            Assert.AreEqual(Input.StatusId, Output.StatusId);
        }

        //Update
        [Test]
        public async System.Threading.Tasks.Task Task_Update_ReturnTrue()
        {
            Task Input = new Task
            {
                Id = 0,
                Code = "Task1",
                Name = "Cong viec 1",
                Description = "Mo ta cv 1",
                StartDate = DateTime.Now,
                FinishDate = new DateTime(2022, 12, 12),
                Percentage = 0,
                TaskTypeId = 1,
                ProjectId = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                StatusId = StatusEnum.ACTIVE.Id,
                Used = false,
            };
            await repository.Create(Input);
            Init();

            var Created = DataContext.Tasks.Where(x => x.Id == Input.Id).FirstOrDefault();

            Input = new Task
            {
                Id = Created.Id,
                Code = "TaskUpdate",
                Name = "Cong viec 1 update",
                Description = "Mo ta cv 1 update",
                StartDate = DateTime.Now,
                FinishDate = new DateTime(2022, 12, 22),
                Percentage = 0,
                TaskTypeId = 1,
                ProjectId = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                StatusId = StatusEnum.ACTIVE.Id,
                Used = false,
            };
            await repository.Update(Input);
            Init();
            var Output = DataContext.Tasks.Where(x => x.Id == Input.Id).FirstOrDefault();

            Assert.AreEqual(Input.Id, Output.Id);
            Assert.AreEqual(Input.Code, Output.Code);
            Assert.AreEqual(Input.Name, Output.Name);
            Assert.AreEqual(Input.Description, Output.Description);
            Assert.AreEqual(Input.StartDate.ToString("dd-MM-yyyy HH:mm:ss"), Output.StartDate.ToString("dd-MM-yyyy HH:mm:ss"));
            Assert.AreEqual(Input.FinishDate.ToString("dd-MM-yyyy HH:mm:ss"), Output.FinishDate.ToString("dd-MM-yyyy HH:mm:ss"));
            Assert.AreEqual(Input.Percentage, Output.Percentage);
            Assert.AreEqual(Input.ProjectId, Output.ProjectId);
            Assert.AreEqual(Input.TaskTypeId, Output.TaskTypeId);
            Assert.AreEqual(Input.CreatedAt.ToString("dd-MM-yyyy HH:mm:ss"), Output.CreatedAt.ToString("dd-MM-yyyy HH:mm:ss"));
            Assert.AreEqual(Input.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"), Output.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"));
            Assert.AreEqual(Input.Used, Output.Used);
            Assert.AreEqual(Input.StatusId, Output.StatusId);
        }

    }
}
