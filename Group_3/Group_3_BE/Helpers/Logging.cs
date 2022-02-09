using Group_3_BE.Entities;
using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;
using TrueSight.Common;

namespace Group_3_BE.Helpers
{
    public interface ILogging : IServiceScoped
    {
        void CreateAuditLog(object newData, object oldData, string className, [CallerMemberName] string methodName = "");
        void CreateSystemLog(Exception ex, string className, [CallerMemberName] string methodName = "");
    }
    public class Logging : ILogging
    {
        public void CreateAuditLog(object newData, object oldData, string className, [CallerMemberName] string methodName = "")
        {
            AuditLog AuditLog = new AuditLog
            {
                ClassName = className,
                MethodName = methodName,
                ModuleName = StaticParams.ModuleName,
                OldData = JsonConvert.SerializeObject(oldData),
                NewData = JsonConvert.SerializeObject(newData),
                Time = StaticParams.DateTimeNow,
            };
        }

        public void CreateSystemLog(Exception ex, string className, [CallerMemberName] string methodName = "")
        {
            SystemLog SystemLog = new SystemLog
            {
                ClassName = className,
                MethodName = methodName,
                ModuleName = StaticParams.ModuleName,
                Exception = ex.ToString(),
                Time = StaticParams.DateTimeNow,
            };
            throw new MessageException(ex);
        }
    }
}
