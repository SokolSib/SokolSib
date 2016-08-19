using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using SqlProfiler;

namespace ConsoleExample
{
    internal class Program
    {
        private static readonly string ScriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts.txt");

        private static void Main(string[] args)
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["MsSqlConnection"].ToString();
                var profiledConnection = new DbProfilerConnection(new SqlConnection(connectionString), ProfilerEvent);

                int countOfRowsInTable;
                using (var connection = profiledConnection.CreateConnection())
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT count(*) FROM information_schema.tables WHERE TABLE_SCHEMA = @tableSchema";

                        var parameter = command.CreateParameter();
                        parameter.ParameterName = "@tableSchema";
                        parameter.Value = "dbo";
                        command.Parameters.Add(parameter);

                        countOfRowsInTable = (int) command.ExecuteScalar();
                    }
                }

                Console.WriteLine("Test success. Result = {0}. Press any key...", countOfRowsInTable);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Test failed. Exception = {0}. Press any key...", ex);
            }

            Console.ReadKey();
        }

        private static void ProfilerEvent(DbProfilerEventArgs e)
        {
            var commandText = e.Command.ToText();
            File.AppendAllText(ScriptPath, commandText);
            Console.Write(commandText);
        }
    }
}