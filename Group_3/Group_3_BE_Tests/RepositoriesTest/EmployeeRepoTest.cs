using Group_3_BE.Entities;
using Group_3_BE.Enums;
using Group_3_BE.Models;
using Group_3_BE.Repositories;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Group_3_BE_Tests.RepositoriesTest
{
    [TestFixture]
    public class EmployeeRepoTest : CommonTests
    {
        IEmployeeRepository repository;
        public EmployeeRepoTest() : base()
        {

        }
        [SetUp]
        public async System.Threading.Tasks.Task Setup()
        {
            Init();
            await Clean();
            repository = new EmployeeRepository(DataContext);
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
        //Create
        [Test]
        public async System.Threading.Tasks.Task Employee_Create_ReturnTrue()
        {
            Employee Input = new Employee
            {
                Id = 0,
                Code = "NV1",
                Name = "Nguyễn Trung Thành",
                GenderId = GenderEnum.MALE.Id,
                JobId = JobEnum.EMPLOYEE.Id,
                DateOfBirth = DateTime.Parse("05/12/2000"),
                Address = "Bắc Giang",
                Phone = "0868959613",
                Email = "trungthanhtdln@gmail.com",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                StatusId = StatusEnum.ACTIVE.Id,
                Used = false,
            };
            await repository.Create(Input);
            Init();
            var Output = DataContext.Employees.Where(x => x.Id == Input.Id).FirstOrDefault();
            Assert.AreEqual(Input.Id, Output.Id);
            Assert.AreEqual(Input.Code, Output.Code);
            Assert.AreEqual(Input.Name, Output.Name);
            Assert.AreEqual(Input.GenderId, Output.GenderId);
            Assert.AreEqual(Input.JobId, Output.JobId);
            Assert.AreEqual(Input.DateOfBirth.ToString("dd-MM-yyyy"), Output.DateOfBirth.ToString("dd-MM-yyyy"));
            Assert.AreEqual(Input.Address, Output.Address);
            Assert.AreEqual(Input.Phone, Output.Phone);
            Assert.AreEqual(Input.Email, Output.Email);
            Assert.AreEqual(Input.StatusId, Output.StatusId);
            Assert.AreEqual(Input.Used, Output.Used);
            Assert.AreEqual(Input.CreatedAt.ToString("dd-MM-yyyy HH:mm:ss"), Output.CreatedAt.ToString("dd-MM-yyyy HH:mm:ss"));
            Assert.AreEqual(Input.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"), Output.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"));
        }

        //Update
        [Test]
        public async System.Threading.Tasks.Task Employee_Update_ReturnTrue()
        {
            Employee InputCreate = new Employee
            {
                Id = 0,
                Code = "NV1",
                Name = "Nguyễn Trung Thành",
                GenderId = GenderEnum.MALE.Id,
                JobId = JobEnum.EMPLOYEE.Id,
                DateOfBirth = DateTime.Parse("05/12/2000"),
                Address = "Bắc Giang",
                Phone = "0868959613",
                Email = "trungthanhtdln@gmail.com",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                StatusId = StatusEnum.ACTIVE.Id,
                Used = false,
            };
            await repository.Create(InputCreate);
            Init();
            EmployeeDAO employee = DataContext.Employees.Where(x => x.Id == InputCreate.Id).FirstOrDefault();
            Employee InputUpdate = new Employee
            {
                Id = employee.Id,
                Code = employee.Code,
                Name = "Trương Hoàng Sơn",
                GenderId = GenderEnum.MALE.Id,
                JobId = JobEnum.DIRECTOR.Id,
                DateOfBirth = DateTime.Parse("05/02/2001"),
                Address = "Hà Nội",
                Phone = "0868959613",
                Email = "trungthanhtd7a@gmail.com",
                UpdatedAt = DateTime.Now,
                StatusId = StatusEnum.ACTIVE.Id,
            };
            await repository.Update(InputUpdate);
            Init();
            var Output = DataContext.Employees.Where(x => x.Id == InputUpdate.Id).FirstOrDefault();
            Assert.AreEqual(InputUpdate.Id, Output.Id);
            Assert.AreEqual(InputUpdate.Code, Output.Code);
            Assert.AreEqual(InputUpdate.Name, Output.Name);
            Assert.AreEqual(InputUpdate.GenderId, Output.GenderId);
            Assert.AreEqual(InputUpdate.JobId, Output.JobId);
            Assert.AreEqual(InputUpdate.DateOfBirth.ToString("dd-MM-yyyy"), Output.DateOfBirth.ToString("dd-MM-yyyy"));
            Assert.AreEqual(InputUpdate.Address, Output.Address);
            Assert.AreEqual(InputUpdate.Phone, Output.Phone);
            Assert.AreEqual(InputUpdate.Email, Output.Email);
            Assert.AreEqual(InputUpdate.StatusId, Output.StatusId);
            Assert.AreEqual(InputUpdate.Used, Output.Used);
            Assert.AreEqual(InputUpdate.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"), Output.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"));
        }
        //Delete
        [Test]
        public async System.Threading.Tasks.Task Employee_Delete_ReturnTrue()
        {
            Employee InputCreate = new Employee
            {
                Id = 0,
                Code = "NV1",
                Name = "Nguyễn Trung Thành",
                GenderId = GenderEnum.MALE.Id,
                JobId = JobEnum.EMPLOYEE.Id,
                DateOfBirth = DateTime.Parse("05/12/2000"),
                Address = "Bắc Giang",
                Phone = "0868959613",
                Email = "trungthanhtdln@gmail.com",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                StatusId = StatusEnum.ACTIVE.Id,
                Used = false,
            };
            await repository.Create(InputCreate);
            Init();
            EmployeeDAO employee = DataContext.Employees.Where(x => x.Id == InputCreate.Id).FirstOrDefault();
            Employee InputDelete = new Employee
            {
                Id = employee.Id,
                DeletedAt = DateTime.Now,
            };
            await repository.Delete(InputDelete);
            Init();
            var Output = DataContext.Employees.Where(x => x.Id == InputDelete.Id).FirstOrDefault();
            Assert.AreEqual(InputDelete.DeletedAt.ToString(), Output.DeletedAt.ToString());
        }
    }
}