using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUtil
{
    public static class Log4netLogger
    {
        public static void Info(Type caller, object message)
        {
            var log = log4net.LogManager.GetLogger(caller);
            log.Info(message);
        }
        public static void Info(Type caller, object message, Exception exception)
        {
            var log = log4net.LogManager.GetLogger(caller);
            log.Info(message, exception);
        }
        public static void Warn(Type caller, object message)
        {
            var log = log4net.LogManager.GetLogger(caller);
            log.Warn(message);
        }
        public static void Warn(Type caller, object message, Exception exception)
        {
            var log = log4net.LogManager.GetLogger(caller);
            log.Warn(message, exception);
        }
        public static void Error(Type caller, object message)
        {
            var log = log4net.LogManager.GetLogger(caller);
            log.Error(message);
        }
        public static void Error(Type caller, object message, Exception exception)
        {
            var log = log4net.LogManager.GetLogger(caller);
            log.Error(message, exception);
        }
        public static void Debug(Type caller, object message)
        {
            var log = log4net.LogManager.GetLogger(caller);
            log.Debug(message);
        }
        public static void Debug(Type caller, object message, Exception exception)
        {
            var log = log4net.LogManager.GetLogger(caller);
            log.Debug(message, exception);
        }
        public static void Fatal(Type caller, object message)
        {
            var log = log4net.LogManager.GetLogger(caller);
            log.Fatal(message);
        }
        public static void Fatal(Type caller, object message, Exception exception)
        {
            var log = log4net.LogManager.GetLogger(caller);
            log.Fatal(message, exception);
        }
    }
}
