using System.Data;
using System.Data.Common;
using StackExchange.Profiling.Data;

namespace SqlProfiler
{
    /// <summary>
    ///     Profiled connection.
    /// </summary>
    public class DbProfilerConnection
    {
        /// <summary>
        ///     Declare the delegate for event.
        /// </summary>
        public delegate void ProfilerEventHandler(DbProfilerEventArgs e);

        /// <summary>
        ///     Standart connection.
        /// </summary>
        private readonly IDbConnection _connection;

        /// <summary>
        ///     Db profiler.
        /// </summary>
        private readonly DbProfiler _profiler = new DbProfiler();

        /// <summary>
        ///     Db profiler connection
        /// </summary>
        /// <param name="connection">Standart connection.</param>
        /// <param name="dbProfilerEvent">Profiler event.</param>
        public DbProfilerConnection(IDbConnection connection, ProfilerEventHandler dbProfilerEvent)
        {
            _connection = connection;
            _profiler.DbProfilerEvent += dbProfilerEvent;
        }

        /// <summary>
        ///     Create profiled connection.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <returns>Standart connection.</returns>
        public IDbConnection CreateConnection(string connectionString = null)
        {
            return new ProfiledDbConnection((DbConnection) _connection, _profiler);
        }
    }
}