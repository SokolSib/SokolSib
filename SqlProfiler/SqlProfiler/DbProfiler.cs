using System;
using System.Data;
using System.Data.Common;
using StackExchange.Profiling.Data;

namespace SqlProfiler
{
    internal sealed class DbProfiler : IDbProfiler
    {
        /// <summary>
        ///     Lazy initializer for DbProfiler.
        /// </summary>
        private static readonly Lazy<DbProfiler> LazyInitializer = new Lazy<DbProfiler>(() => new DbProfiler());

        public static DbProfiler Current
        {
            get { return LazyInitializer.Value; }
        }

        public void ExecuteStart(IDbCommand profiledDbCommand, SqlExecuteType executeType)
        {
            // not usage
        }

        public void ExecuteFinish(IDbCommand profiledDbCommand, SqlExecuteType executeType, DbDataReader reader)
        {
            if (DbProfilerEvent != null)
                DbProfilerEvent(new DbProfilerEventArgs(profiledDbCommand, executeType));
        }

        public void ReaderFinish(IDataReader reader)
        {
            // not usage
        }

        public void OnError(IDbCommand profiledDbCommand, SqlExecuteType executeType, Exception exception)
        {
            if (DbProfilerEvent != null)
                DbProfilerEvent(new DbProfilerEventArgs(profiledDbCommand, executeType, exception));
        }

        public bool IsActive
        {
            // not usage
            get { return true; }
        }

        /// <summary>
        ///     Declare the event.
        /// </summary>
        public event DbProfilerConnection.ProfilerEventHandler DbProfilerEvent;
    }
}