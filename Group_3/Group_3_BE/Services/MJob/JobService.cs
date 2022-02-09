using Group_3_BE.Entities;
using Group_3_BE.Helpers;
using Group_3_BE.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace Group_3_BE.Services.MJob
{
    public interface IJobService : IServiceScoped
    {
        Task<int> Count(JobFilter JobFilter);
        Task<List<Job>> List(JobFilter JobFilter);
    }
    public class JobService : IJobService
    {
        private IUOW UOW;
        private ILogging Logging;
        public JobService
        (
            IUOW UOW,
            ILogging Logging
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
        }
        public async Task<int> Count(JobFilter JobFilter)
        {
            try
            {
                int result = await UOW.JobRepository.Count(JobFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(JobService));
            }
            return 0;
        }

        public async Task<List<Job>> List(JobFilter JobFilter)
        {
            try
            {
                List<Job> Jobs = await UOW.JobRepository.List(JobFilter);
                return Jobs;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(JobService));
            }
            return null;
        }
    }
}
