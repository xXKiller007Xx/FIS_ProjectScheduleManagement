using Group_3_BE.Entities;
using Group_3_BE.Enums;
using Group_3_BE.Models;
using Group_3_BE.Repositories;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Group_3_BE_Tests.RepositoriesTest.employee
{
    public partial class EmployeeRepoTest : CommonTests
    {
        [SetUp]
        public async System.Threading.Tasks.Task Setup()
        {
            Init();
            await Clean();
            repository = new EmployeeRepository(DataContext);

            StatusGiven();
            GenderGiven();
            JobGiven();
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

        public void GenderGiven()
        {
            DataContext.Genders.Add(new GenderDAO
            {
                Id = 0,
                Code = GenderEnum.MALE.Code,
                Name = GenderEnum.MALE.Name,
            });
            DataContext.Genders.Add(new GenderDAO
            {
                Id = 0,
                Code = GenderEnum.FEMALE.Code,
                Name = GenderEnum.FEMALE.Name,
            });
            DataContext.Genders.Add(new GenderDAO
            {
                Id = 0,
                Code = GenderEnum.OTHER.Code,
                Name = GenderEnum.OTHER.Name,
            });

            DataContext.SaveChanges();
        }

        public void JobGiven()
        {
            DataContext.Jobs.Add(new JobDAO
            {
                Id = 0,
                Code = JobEnum.LEADER.Code,
                Name = JobEnum.LEADER.Name,
            });
            DataContext.Jobs.Add(new JobDAO
            {
                Id = 0,
                Code = JobEnum.DIRECTOR.Code,
                Name = JobEnum.DIRECTOR.Name,
            });
            DataContext.Jobs.Add(new JobDAO
            {
                Id = 0,
                Code = JobEnum.EMPLOYEE.Code,
                Name = JobEnum.EMPLOYEE.Name,
            });
            DataContext.Jobs.Add(new JobDAO
            {
                Id = 0,
                Code = JobEnum.MANAGER.Code,
                Name = JobEnum.MANAGER.Name,
            });

            DataContext.SaveChanges();
        }
    }
}
