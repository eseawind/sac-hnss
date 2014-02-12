using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;

namespace SAC.Helper
{
    public class LogHelper
    {
        /// <summary>
        /// 日志写入类
        /// </summary>
        public LogHelper()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 日志类别
        /// </summary>
        public enum EnLogType
        {
            /// <summary>
            /// 错误日志
            /// </summary>
            Error,
            /// <summary>
            /// 成功日志
            /// </summary>
            Success,
            /// <summary>
            /// 日常日志
            /// </summary>
            Common,
            /// <summary>
            /// 警告日志
            /// </summary>
            Warn,
            /// <summary>
            /// 运行日志
            /// </summary>
            Run
        }

        /// <summary>
        /// 记录错误日志至文本文件
        /// </summary>
        /// <param name="message">记录的内容</param>
        public static void WriteLog(string message)
        {
            WriteLog(EnLogType.Error, message);
        }

        private static object privateObjectLock = new object();

        /// <summary>
        /// 记录日志至文本文件
        /// </summary>
        /// <param name="logtype">日志类型</param>
        /// <param name="message">记录的内容</param>
        public static void WriteLog(EnLogType logtype, string message)
        {
            string filePrefix;
            switch (logtype)
            {
                case EnLogType.Common:
                    filePrefix = "Access_";
                    break;
                case EnLogType.Error:
                    filePrefix = "Error_";
                    break;
                case EnLogType.Success:
                    filePrefix = "Sucess_";
                    break;
                case EnLogType.Warn:
                    filePrefix = "Warn_";
                    break;
                case EnLogType.Run:
                    filePrefix = "Run_";
                    break;
                default:
                    filePrefix = "Error_";
                    break;
            }
            string path;
            try
            {
                //path = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationSettings.AppSettings["LogPath"] + "\\";
                //F:\Web-单云龙\复件 WH\WHweb\
                path = System.AppDomain.CurrentDomain.BaseDirectory + ConfigurationSettings.AppSettings["LogPath"]; //"c:\\Log\\";
            }
            catch (Exception e)
            {
                return;
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            lock (privateObjectLock)
            {
                try
                {
                    string FILE_NAME = path + filePrefix + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                    if (File.Exists(FILE_NAME))
                    {
                        StreamWriter sr = File.AppendText(FILE_NAME);
                        sr.WriteLine(DateTime.Now.ToString() + ": " + message);
                        sr.Close();
                    }
                    else
                    {
                        StreamWriter sr = File.CreateText(FILE_NAME);
                        sr.WriteLine(DateTime.Now.ToString() + ": " + message);
                        sr.Close();
                    }
                }
                catch { }
            }
        }
    }
}
