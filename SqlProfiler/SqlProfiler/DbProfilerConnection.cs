using System.Data;
using System.Data.Common;
using StackExchange.Profiling.Data;

namespace SqlProfiler
{
    /// <summary>
    ///     Profiled connection.
    /// </summary>
    public class DbProfilerConnection : IDbConnection
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
            _profiler.DbProfilerEvent += dbProfilerEvent;
            _connection = new ProfiledDbConnection((DbConnection) connection, _profiler);
        }

        #region IDbConnection members

        public void Dispose()
        {
            _connection.Dispose();
        }

        public IDbTransaction BeginTransaction()
        {
            return _connection.BeginTransaction();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return _connection.BeginTransaction(il);
        }

        public void Close()
        {
            _connection.Close();
        }

        public void ChangeDatabase(string databaseName)
        {
            _connection.ChangeDatabase(databaseName);
        }

        public IDbCommand CreateCommand()
        {
            return _connection.CreateCommand();
        }

        public void Open()
        {
            _connection.Open();
        }

        public string ConnectionString
        {
            get { return _connection.ConnectionString; }
            set { _connection.ConnectionString = value; }
        }

        public int ConnectionTimeout
        {
            get { return _connection.ConnectionTimeout; }
        }

        public string Database
        {
            get { return _connection.Database; }
        }

        public ConnectionState State
        {
            get { return _connection.State; }
        }

        #endregion
    }
}