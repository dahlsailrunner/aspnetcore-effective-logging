using System.Diagnostics;
using Serilog;

namespace Flogger.Serilog
{
    public class PerfTracker
    {
        private readonly string _whatsBeingTracked;
        private readonly Stopwatch _tracker;

        /// <summary>
        /// Creates a new PerfTracker object to track performance.  The constructor starts the
        /// clock ticking.
        /// </summary>
        /// <param name="whatsBeingTracked">The name of the thing you're tracking performance for --
        /// like API method name, procname, or whatever.</param>
        public PerfTracker(string whatsBeingTracked)
        {
            _whatsBeingTracked = whatsBeingTracked;
            _tracker = new Stopwatch();
            _tracker.Start();
        }

        public void Stop()
        {
            if (_tracker == null) return;
            _tracker.Stop();
            Log.Information("{PerfItem} took {ElapsedMilliseconds} milliseconds",
                _whatsBeingTracked, _tracker.ElapsedMilliseconds);
        }
    }
}
