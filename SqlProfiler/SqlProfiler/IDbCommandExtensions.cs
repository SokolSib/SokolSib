using System.Data;
using System.Data.Common;
using System.Text;

namespace SqlProfiler
{
    public static class DbCommandExtensions
    {
        /// <summary>
        ///     Command to text.
        /// </summary>
        /// <param name="command">Command.</param>
        /// <returns></returns>
        public static string ToText(this IDbCommand command)
        {
            var sb = new StringBuilder();

            sb.AppendFormat("CommandType: {0}, CommandText: {1}", command.CommandType, command.CommandText);

            if (command.Parameters.Count > 0)
            {
                sb.AppendLine();
                sb.AppendLine("Parameters:");

                foreach (DbParameter parameter in command.Parameters)
                {
                    sb.AppendFormat("{0} = {1}", parameter.ParameterName, parameter.Value);
                    sb.AppendLine();
                }
            }

            return sb.AppendLine().ToString();
        }
    }
}