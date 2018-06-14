using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MicBeach.DB.MySQL
{
    /// <summary>
    /// db server factory
    /// </summary>
    internal static class DbServerFactory
    {
        #region get db connection

        /// <summary>
        /// get sql server database connection
        /// </summary>
        /// <param name="server">database server</param>
        /// <returns>db connection</returns>
        public static IDbConnection GetConnection(ServerInfo server)
        {
            IDbConnection conn = conn = new MySqlConnection(server.ConnectionString);
            return conn;
        }

        #endregion
    }
}
