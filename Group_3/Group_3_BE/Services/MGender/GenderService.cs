using Group_3_BE.Entities;
using Group_3_BE.Helpers;
using Group_3_BE.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace Group_3_BE.Services.MGender
{
    public interface IGenderService : IServiceScoped
    {
        Task<int> Count(GenderFilter GenderFilter);
        Task<List<Gender>> List(GenderFilter GenderFilter);
    }
    public class GenderService : IGenderService
    {
        private IUOW UOW;
        private ILogging Logging;
        public GenderService
        (
            IUOW UOW,
            ILogging Logging
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
        }
        public async Task<int> Count(GenderFilter GenderFilter)
        {
            try
            {
                int result = await UOW.GenderRepository.Count(GenderFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(GenderService));
            }
            return 0;
        }

        public async Task<List<Gender>> List(GenderFilter GenderFilter)
        {
            try
            {
                List<Gender> Genders = await UOW.GenderRepository.List(GenderFilter);
                return Genders;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(GenderService));
            }
            return null;
        }
    }
}
