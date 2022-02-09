using Group_3_BE.Entities;
using Group_3_BE.Helpers;
using Group_3_BE.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace Group_3_BE.Services.MStatus
{
    public interface IStatusService : IServiceScoped
    {
        Task<int> Count(StatusFilter StatusFilter);
        Task<List<Status>> List(StatusFilter StatusFilter);
    }
    public class StatusService : IStatusService
    {
        private IUOW UOW;
        private ILogging Logging;
        public StatusService
        (
            IUOW UOW,
            ILogging Logging
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
        }
        public async Task<int> Count(StatusFilter StatusFilter)
        {
            try
            {
                int result = await UOW.StatusRepository.Count(StatusFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StatusService));
            }
            return 0;
        }

        public async Task<List<Status>> List(StatusFilter StatusFilter)
        {
            try
            {
                List<Status> Statuses = await UOW.StatusRepository.List(StatusFilter);
                return Statuses;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StatusService));
            }
            return null;
        }
    }
}
