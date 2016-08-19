using System;
using System.Data;
using StackExchange.Profiling.Data;

namespace SqlProfiler
{
    /// <summary>
    ///     Profiler event arguments
    /// </summary>
    public sealed class DbProfilerEventArgs
    {
        /// <summary>
        ///     Profiler event arguments.
        /// </summary>
        /// <param name="command">Db command.</param>
        /// <param name="executeType">Execute type.</param>
        /// <param name="exception">Exception (if exists).</param>
        public DbProfilerEventArgs(IDbCommand command, SqlExecuteType executeType, Exception exception = null)
        {
            Command = command;
            ExecuteType = executeType;
            Exception = exception;
        }

        /// <summary>
        ///     Db command.
        /// </summary>
        public IDbCommand Command { get; private set; }

        /// <summary>
        ///     Execute type.
        /// </summary>
        public SqlExecuteType ExecuteType { get; private set; }

        /// <summary>
        ///     Exception.
        /// </summary>
        public Exception Exception { get; private set; }
    }
}