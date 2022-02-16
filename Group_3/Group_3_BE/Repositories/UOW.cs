using Group_3_BE.Models;
using Microsoft.Extensions.Configuration;
using System;
using TrueSight.Common;

namespace Group_3_BE.Repositories
{
    public interface IUOW : IServiceScoped, IDisposable
    {
        IStatusRepository StatusRepository { get; }
        IGenderRepository GenderRepository { get; }
        IJobRepository JobRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IProjectRepository ProjectRepository { get; }
        ITaskTypeRepository TaskTypeRepository { get; }
        ITaskRepository TaskRepository { get; }
    }
    public class UOW : IUOW
    {
        private DataContext DataContext;
        public IStatusRepository StatusRepository { get; private set; }
        public IGenderRepository GenderRepository { get; private set; }
        public IJobRepository JobRepository { get; private set; }
        public IEmployeeRepository EmployeeRepository { get; private set; }
        public IProjectRepository ProjectRepository { get; private set; }
        public ITaskTypeRepository TaskTypeRepository { get; private set; }
        public ITaskRepository TaskRepository { get; private set; }
        public UOW(DataContext DataContext)
        {
            this.DataContext = DataContext;

            StatusRepository = new StatusRepository(DataContext);
            GenderRepository = new GenderRepository(DataContext);
            JobRepository = new JobRepository(DataContext);
            EmployeeRepository = new EmployeeRepository(DataContext);
            ProjectRepository = new ProjectRepository(DataContext);
            TaskTypeRepository = new TaskTypeRepository(DataContext);
            TaskRepository = new TaskRepository(DataContext);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }
            if (this.DataContext == null)
            {
                return;
            }
            this.DataContext.Dispose();
            this.DataContext = null;
        }
    }
}
