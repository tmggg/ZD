using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgS.Helper
{
    public class LogHelper
    {
        private static LogHelper _helper = null;

        private static NLog.Logger logger = null;

        public static LogHelper instance()
        {
            if (_helper == null)
            {
                _helper = new LogHelper();
                logger = NLog.LogManager.GetCurrentClassLogger();
            }
            return _helper;
        }

        public void Warn(string msg, params object[] args)
        {
            logger.Warn(msg, args);
        }

        public void Warn(string msg, Exception err)
        {
            //logger.Warn(msg, err);
        }

        public void Debug(string msg, params object[] args)
        {
            logger.Debug(msg, args);
        }

        public void Debug(string msg, Exception err)
        {
            //logger.Debug(msg, err);
        }

        public void Info(string msg, params object[] args)
        {
            logger.Info(msg, args);
        }

        public void Info(string msg, Exception err)
        {
            //logger.Info(msg, err);
        }

        public void Trace(string msg, params object[] args)
        {
            logger.Trace(msg, args);
        }

        public void Trace(string msg, Exception err)
        {
            //logger.Trace(msg, err);
        }

        public void Error(string msg, params object[] args)
        {
            logger.Error(msg, args);
        }

        public void Error(string msg, Exception err)
        {
            //logger.Error(msg, err);
        }

        public void Fatal(string msg, params object[] args)
        {
            logger.Fatal(msg, args);
        }

        public void Fatal(string msg, Exception err)
        {
            //logger.Fatal(msg, err);
        }

    }

}
