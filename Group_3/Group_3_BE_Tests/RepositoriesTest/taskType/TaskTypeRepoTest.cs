using Group_3_BE.Entities;
using Group_3_BE.Enums;
using Group_3_BE.Models;
using Group_3_BE.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace Group_3_BE_Tests.RepositoriesTest.taskType
{
    [TestFixture]
    public partial class TaskTypeRepoTest : CommonTests
    {
        ITaskTypeRepository repository;
        public TaskTypeRepoTest() : base()
        {

        }

        //[SetUp]
        //public async System.Threading.Tasks.Task Setup()
        //{
        //    Init();
        //    await Clean();
        //    repository = new TaskTypeRepository(DataContext);

        //    DataContext.Statuses.Add(new StatusDAO
        //    {
        //        //Id = StatusEnum.ACTIVE.Id,
        //        Code = StatusEnum.ACTIVE.Code,
        //        Name = StatusEnum.ACTIVE.Name,
        //    });
        //    DataContext.Statuses.Add(new StatusDAO
        //    {
        //        //Id = StatusEnum.ACTIVE.Id,
        //        Code = StatusEnum.INACTIVE.Code,
        //        Name = StatusEnum.INACTIVE.Name,
        //    });

        //    DataContext.SaveChanges();
        //}

        //Create
        [Test]
        public async System.Threading.Tasks.Task TaskType_Create_ReturnTrue()
        {
            TaskType Input = new TaskType
            {
                Id = 0,
                Code = "Type1",
                Name = "Loai cong viec 1",
                Description = "Mo ta",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                StatusId = StatusEnum.ACTIVE.Id,
            };
            await repository.Create(Input);
            Init();
            var Output = DataContext.TaskTypes.Where(x => x.Id == Input.Id).FirstOrDefault();
            Assert.AreEqual(Input.Id, Output.Id);
            Assert.AreEqual(Input.Code, Output.Code);
            Assert.AreEqual(Input.Name, Output.Name);
            Assert.AreEqual(Input.Description, Output.Description);
            Assert.AreEqual(Input.CreatedAt.ToString("dd-MM-yyyy HH:mm:ss"), Output.CreatedAt.ToString("dd-MM-yyyy HH:mm:ss"));
            Assert.AreEqual(Input.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"), Output.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"));
            Assert.AreEqual(Input.StatusId, Output.StatusId);
        }

        //Update
        [Test]
        public async System.Threading.Tasks.Task TaskType_Update_ReturnTrue()
        {
            TaskType Input = new TaskType
            {
                Id = 0,
                Code = "Type1",
                Name = "Loai cong viec 1",
                Description = "Mo ta",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                StatusId = StatusEnum.ACTIVE.Id,
            };
            await repository.Create(Input);
            Init();
            var Created = DataContext.TaskTypes.Where(x => x.Id == Input.Id).FirstOrDefault();

            Input = new TaskType
            {
                Id = Created.Id,
                Code = "Type2",
                Name = "cong viec moi",
                Description = "Mo ta moi",
                UpdatedAt = DateTime.Now,
                StatusId = StatusEnum.ACTIVE.Id,
            };
            await repository.Update(Input);
            Init();
            var Output = DataContext.TaskTypes.Where(x => x.Id == Input.Id).FirstOrDefault();
            Assert.AreEqual(Input.Id, Output.Id);
            Assert.AreEqual(Input.Code, Output.Code);
            Assert.AreEqual(Input.Name, Output.Name);
            Assert.AreEqual(Input.Description, Output.Description);
            Assert.AreEqual(Input.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"), Output.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"));
            Assert.AreEqual(Input.StatusId, Output.StatusId);
        }

        //Delete
        [Test]
        public async System.Threading.Tasks.Task TaskType_Delete_ReturnTrue()
        {
            TaskType Input = new TaskType
            {
                Id = 0,
                Code = "Type1",
                Name = "Loai cong viec 1",
                Description = "Mo ta",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                StatusId = StatusEnum.ACTIVE.Id,
            };
            await repository.Create(Input);
            Init();
            var Created = DataContext.TaskTypes.Where(x => x.Id == Input.Id).FirstOrDefault();
            Init();
            Input = new TaskType
            {
                Id = Created.Id,
            };
            await repository.Delete(Input);
            Init();
            var Output = DataContext.TaskTypes.Where(x => x.Id == Input.Id).FirstOrDefault();
            //Assert.AreNotEqual(Created.DeletedAt, Output.DeletedAt);
            Assert.AreNotEqual(null, Output.DeletedAt);
        }

        // Count
        [Test]
        public async System.Threading.Tasks.Task TaskType_Count_ReturnTrue()
        {
            List<TaskType> InputData = new List<TaskType>
            {
                new TaskType
                {
                    Id = 0,
                    Code = "Type1",
                    Name = "Loai cong viec 1",
                    Description = "Mo ta 1",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    StatusId = StatusEnum.ACTIVE.Id,
                },
                new TaskType
                {
                    Id = 0,
                    Code = "Type2",
                    Name = "Loai cong viec 2",
                    Description = "Mo ta 2",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    StatusId = StatusEnum.ACTIVE.Id,
                },
            };
            await repository.BulkMerge(InputData);

            Init();

            TaskTypeFilter Input = new TaskTypeFilter
            {
               //Id = new IdFilter { Equal = 1 }
            };

            int Output = await repository.Count(Input);
            Assert.AreEqual(2, Output);
        }


        // BulkDelete
        [Test]
        public async System.Threading.Tasks.Task TaskType_BulkDelete_ReturnTrue()
        {
            TaskType Input1 = new TaskType
            {
                Id = 0,
                Code = "Type1",
                Name = "Loai cong viec 1",
                Description = "Mo ta",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                StatusId = StatusEnum.ACTIVE.Id,
            };
            await repository.Create(Input1);
            
            TaskType Input2 = new TaskType
            {
                Id = 0,
                Code = "Type2",
                Name = "Loai cong viec 2",
                Description = "Mo ta",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                StatusId = StatusEnum.ACTIVE.Id,
            };
            await repository.Create(Input2);

            List<TaskType> Input = new List<TaskType>
            {
                new TaskType { Id = 1 },
                new TaskType {Id = 2 },
            };
            await repository.BulkDelete(Input);

            Init();
            //List<TaskType> Outs = await repository.List(new List<long> { 1, 2 });
            //foreach (var Out in Outs)
            //{
            //    Console.WriteLine(Out.DeletedAt);
            //}
        }
    }
}
