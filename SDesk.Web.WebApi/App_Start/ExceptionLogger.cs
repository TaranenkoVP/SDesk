using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.ExceptionHandling;
using log4net;

namespace SDesk.Web.WebApi
{
    public class ExceptionLogger : IExceptionLogger
    {
        private static readonly ILog Log4Net = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            Trace.TraceError(context.ExceptionContext.Exception.ToString());
        
            return Task.Run(() =>
            {
                Log4Net.Error(
                    $"Unhandled exception thrown in {context.Request.Method} for request {context.Request.RequestUri}: {context.Exception}");
            }, cancellationToken);
        }
    }
}