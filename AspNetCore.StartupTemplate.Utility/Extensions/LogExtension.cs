using AspNetCore.StartUpTemplate.Configuration;
using NLog;

namespace AspNetCore.StartUpTemplate.Utility;

    public static class LogExtension
    {
        /// <summary>
        /// groupid 自定义进程号 此值在每次启动的时候会重新生成
        /// </summary>
        public static string IpAddress { get; set; } = NetworkHelper.GetAddressIpByDns();

        public static string Env { get; set; } = AppSettingsConstVars.EnvironmentMode;
        /// <summary>
        /// 日志输出到Elasticsearch
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        public static void LogErrorEx(this ILogger logger, object message)
        {
            logger.Error($"[{Env}]-[{IpAddress}]-[ERROR]-{message}");
        }
        /// <summary>
        /// 日志输出到Elasticsearch
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        public static void LogErrorEx(this NLog.ILogger logger, Exception ex)
        {
            logger.Error($"[{Env}]-[{IpAddress}]-[ERROR]-{ex.Message}-[Stack]-{ex}");
        }
        /// <summary>
        /// 日志输出到Elasticsearch
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        public static void LogErrorEx(this NLog.ILogger logger, object message, Exception ex)
        {
            logger.Error($"[{Env}]-[{IpAddress}]-[ERROR]-{message}-{ex.Message}-[Stack]-{ex}");
        }
        /// <summary>
        /// 日志输出到Elasticsearch
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        public static void LogDebugEx(this NLog.ILogger logger, object message)
        {
            logger.Debug($"[{Env}]-[{IpAddress}]-[DEBUG]-{message}");
        }
        /// <summary>
        /// 日志输出到Elasticsearch
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        public static void LogFatalEx(this NLog.ILogger logger, object message)
        {
            logger.Fatal($"[{Env}]-[{IpAddress}]-[FATAL]-{message}");
        }
        /// <summary>
        /// 日志输出到Elasticsearch
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        public static void LogInfoEx(this NLog.ILogger logger, object message)
        {
            logger.Info($"[{Env}]-[{IpAddress}]-[INFO]-{message}");
        }
        /// <summary>
        /// 日志输出到Elasticsearch
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        public static void LogWarnEx(this NLog.ILogger logger, object message)
        {
            logger.Info($"[{Env}]-[{IpAddress}]-[WARN]-{message}");
        }
        /// <summary>
        /// 日志输出到Elasticsearch
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        public static void LogWarnEx(this NLog.ILogger logger, Exception ex)
        {
            logger.Info($"[{Env}]-[{IpAddress}]-[ERROR]-{ex.Message}-[Stack]-{ex}");
        }
    }
