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
    }
}
