using System;

namespace ApiLibrary.Common
{
    public static class ConnectionStringBuilder
    {
        public static string Create()
        {
            var serverName = Environment.GetEnvironmentVariable("DbServerName");
            var databaseName = Environment.GetEnvironmentVariable("DatabaseName");
            var user = Environment.GetEnvironmentVariable("DbUser");
            var password = Environment.GetEnvironmentVariable("DbPassword");

            var connectionString = $"Server={serverName};Database={databaseName};User={user};Password={password};";
            return connectionString;
        }
    }
}