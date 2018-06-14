﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace MicBeach.DB.SQLServer
{
    /// <summary>
    /// Db Server Factory
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
            IDbConnection conn = conn = new SqlConnection(server.ConnectionString);
            return conn;
        }

        #endregion
    }
}
