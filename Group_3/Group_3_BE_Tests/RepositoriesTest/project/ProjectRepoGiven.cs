using Group_3_BE.Entities;
using Group_3_BE.Enums;
using Group_3_BE.Helpers;
using Group_3_BE.Models;
using Group_3_BE.Repositories;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Group_3_BE_Tests.RepositoriesTest.project
{
    public partial class ProjectRepoTest : CommonTests
    {
        [SetUp]
        public async System.Threading.Tasks.Task Setup()
        {
            Init();
            await Clean();
            repository = new ProjectRepository(DataContext);

            StatusGiven();
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
                Id = 0,
                Code = StatusEnum.INACTIVE.Code,
                Name = StatusEnum.INACTIVE.Name,
            });
            DataContext.SaveChanges();
        }
    }
}
