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

namespace Group_3_BE_Tests.RepositoriesTest.task
{
    public partial class TaskRepoTest : CommonTests
    {
        [SetUp]
        public async System.Threading.Tasks.Task Setup()
        {
            Init();
            await Clean();
            repository = new TaskRepository(DataContext);

            StatusGiven();
            ProjectGiven();
        }

        public void StatusGiven()
        {
            DataContext.Statuses.Add(new StatusDAO
            {
                Id = 0,
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

        public void ProjectGiven()
        {
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
    }
}
