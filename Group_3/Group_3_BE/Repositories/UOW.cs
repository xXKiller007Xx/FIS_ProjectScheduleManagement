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
    }
    public class UOW : IUOW
    {
        private DataContext DataContext;
        public IStatusRepository StatusRepository { get; private set; }

        public IGenderRepository GenderRepository { get; private set; }

        public IJobRepository JobRepository { get; private set; }

        public IEmployeeRepository EmployeeRepository { get; private set; }
        public UOW(DataContext DataContext)
        {
            this.DataContext = DataContext;

            StatusRepository = new StatusRepository(DataContext);
            GenderRepository = new GenderRepository(DataContext);
            JobRepository = new JobRepository(DataContext);
            EmployeeRepository = new EmployeeRepository(DataContext);
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
