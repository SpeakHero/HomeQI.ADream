//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace HomeQI.ADream.EntityFrameworkCore.Extensions
//{
//    public static class DatabaseExtensions
//    {
//        private static ILoggingServiceProvider LoggingServiceProvider => EfCoreInterceptorDescriptor.Instance.ExposeLoggingServiceProvider;
//        private static EfCoreSinkOptions Settings => EfCoreInterceptorDescriptor.Instance.ExposeSettings;
//        private static Func<string, object> GlobalSimpleLoggingInterceptor => EfCoreInterceptorDescriptor.Instance.ExposeSettings.SimgleLoggingAction;
//        private static RendingConfiguration UpstreamRenderingOptions => EfCoreInterceptorDescriptor.Instance.ExposeSettings.GetRenderingOptions();

//        public static void UseCosmosLogging(this DbContext db, Func<string, object> loggerAct = null)
//        {
//            UseCosmosLogging(db, null, loggerAct);
//        }

//        public static void UseCosmosLogging(this DbContext db, Func<string, LogEventLevel, bool> filter, Func<string, object> loggerAct = null)
//        {
//            if (db == null) throw new ArgumentNullEx(nameof(db));

//            Func<string, LogEventLevel, bool> localFilter = (s, l) => (Settings?.Filter?.Invoke(s, l) ?? true) && (filter?.Invoke(s, l) ?? true);

//            var internalLogger = LoggingServiceProvider?.GetLogger<DbContext>(localFilter, LogEventSendMode.Automatic, UpstreamRenderingOptions);
//            if (internalLogger == null) return;

//            var localFunc = GlobalSimpleLoggingInterceptor;
//            localFunc += loggerAct;

//            var logger = new SimpleLogger(internalLogger, localFunc);

//            db.ConfigureLogging(s => logger.WriteLog(s, db.Database.GetDbConnection().Database), (s, l) => localFilter.Invoke(s, LogLevelSwitcher.Switch(l)));
//        }
//    }
//}