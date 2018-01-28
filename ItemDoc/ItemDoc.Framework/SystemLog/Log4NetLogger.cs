using log4net;
using log4net.Config;
using ItemDoc.Framework.Environment;
using System;
using System.IO;
using ItemDoc.Framework.FileStore;

namespace ItemDoc.Framework.SystemLog
{
  /// <summary>
  /// 系统日志处理【基于Log4Net写日志的原则】
  /// 1、在catch后,把异常写入日志.
  /// 2、在调用第三方控件、第三方接口的开始和结束处.
  /// 3、在连接数据库的开始结束处.
  /// 4、除非必要,不要在循环体中加入日志,否则一旦出问题可能导致日志暴增.
  /// 5、在自己认为很重要的逻辑处写入日志.
  /// </summary>
  public class Log4NetLogger
  {
    /// <summary>
    /// log4net
    /// </summary>
    private static ILog _mlog;

    /// <summary>
    /// 初始化日志系统
    /// 在系统运行开始初始化,不存在创建目录文件
    /// </summary>
    /// <param name="configFilename">
    ///     <remarks>
    ///         <para>log4net配置文件路径，支持以下格式：</para>
    ///         <list type="bullet">
    ///             <item>~/config/log4net.config</item>
    ///             <item>~/web.config</item>
    ///             <item>c:\abc\log4net.config</item>
    ///         </list>
    ///     </remarks>
    /// </param>
    public Log4NetLogger(string configFilename = "")
    {
      XmlConfigurator.Configure();
      if (string.IsNullOrEmpty(configFilename))
      {
       
        string filePath = FileUtility.GetDiskFilePath("~/App_Data/");

        var files = Directory.GetFiles(filePath, "log4net.config");

        //foreach (var file in files)
        //  Console.WriteLine(file);
      }

      string file = FileUtility.GetDiskFilePath(configFilename);
      FileInfo configFileInfo = new FileInfo(file);
      if (!configFileInfo.Exists)
        throw new ApplicationException(string.Format("log4net配置文件 {0} 未找到", configFileInfo.FullName));

      if (RunningEnvironment.IsFullTrust())
        XmlConfigurator.ConfigureAndWatch(configFileInfo);
      else
        XmlConfigurator.Configure(configFileInfo);

    }

    /// <summary>
    /// 写入日志
    /// </summary>
    /// <param name="message">日志信息</param>
    /// <param name="messageType">日志类型</param>
    /// <param name="ex">异常</param>
    /// <param name="type"></param>
    public static void Write(object message, LogLevel messageType, Exception ex, Type type)
    {
      _mlog = LogManager.GetLogger(type);
      switch (messageType)
      {
        case LogLevel.Debug:
          _mlog.Debug(message, ex);
          break;
        case LogLevel.Info:
          _mlog.Info(message, ex);
          break;
        case LogLevel.Warn:
          _mlog.Warn(message, ex);
          break;
        case LogLevel.Error:
          _mlog.Error(message, ex);
          break;
        case LogLevel.Fatal:
          _mlog.Fatal(message, ex);
          break;
      }

    }

    /// <summary>
    /// 关闭log4net
    /// </summary>
    public static void ShutDown()
    {
      LogManager.Shutdown();

    }
  }
}
