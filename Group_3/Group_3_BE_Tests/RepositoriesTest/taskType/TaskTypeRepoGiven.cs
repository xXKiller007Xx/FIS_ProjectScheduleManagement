using Group_3_BE.Entities;
using Group_3_BE.Enums;
using Group_3_BE.Models;
using Group_3_BE.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Group_3_BE_Tests.RepositoriesTest.taskType
{
    public partial class TaskTypeRepoTest : CommonTests
    {
        [SetUp]
        public async System.Threading.Tasks.Task Setup()
        {
            Init();
            await Clean();
            repository = new TaskTypeRepository(DataContext);

            StatusGiven();
            //TaskTypeGiven();
        }

        public void StatusGiven()
        {
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
        }

        public void TaskTypeGiven()
        {
            TaskTypeDAO Input1 = new TaskTypeDAO
            {
                Id = 0,
                Code = "Type1",
                Name = "Loai cong viec 1",
                Description = "Mo ta",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                StatusId = StatusEnum.ACTIVE.Id,
            };
            DataContext.TaskTypes.Add(Input1);

            TaskTypeDAO Input2 = new TaskTypeDAO
            {
                Id = 0,
                Code = "Type2",
                Name = "Loai cong viec 2",
                Description = "Mo ta",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                StatusId = StatusEnum.ACTIVE.Id,
            };
            DataContext.TaskTypes.Add(Input2);

            DataContext.SaveChanges();
        }
    }
}
