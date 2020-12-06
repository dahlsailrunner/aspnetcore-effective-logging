using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace CoreFlogger
{
    public class PerfTracker
    {
        private readonly Stopwatch _sw;
        private readonly FlogDetail _infoToLog;

        public PerfTracker(FlogDetail details)
        {
            _sw = Stopwatch.StartNew();
            _infoToLog = details;            

            var beginTime = DateTime.Now;
            if (_infoToLog.AdditionalInfo == null)
                _infoToLog.AdditionalInfo = new Dictionary<string, object>()
                {
                    { "Started", beginTime.ToString(CultureInfo.InvariantCulture) }
                };
            else
                _infoToLog.AdditionalInfo.Add(
                    "Started", beginTime.ToString(CultureInfo.InvariantCulture));
        }
        public PerfTracker(string name, string userId, string userName,
                   string location, string product, string layer)
        {            
            _infoToLog = new FlogDetail()
            {
                Message = name,
                UserId = userId,
                UserName = userName,
                Product = product,
                Layer = layer,
                Location = location,
                Hostname = Environment.MachineName
            };
            
            var beginTime = DateTime.Now;
            _infoToLog.AdditionalInfo = new Dictionary<string, object>()
            {
                { "Started", beginTime.ToString(CultureInfo.InvariantCulture)  }
            };
        }

        public PerfTracker(string name, string userId, string userName,
                   string location, string product, string layer,
                   Dictionary<string, object> perfParams)
            : this(name, userId, userName, location, product, layer)
        {
            foreach (var item in perfParams)
                _infoToLog.AdditionalInfo.Add("input-" + item.Key, item.Value);
        }

        public void Stop()
        {
            _sw.Stop();
            _infoToLog.ElapsedMilliseconds = _sw.ElapsedMilliseconds;
            Flogger.WritePerf(_infoToLog);
        }
    }
}
