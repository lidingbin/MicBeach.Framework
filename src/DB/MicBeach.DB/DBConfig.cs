using MicBeach.Develop.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DB
{
    /// <summary>
    /// DBConfig
    /// </summary>
    public static class DBConfig
    {
        static Dictionary<ServerType, IDbEngine> _dbEngines = new Dictionary<ServerType, IDbEngine>();

        #region Propertys

        /// <summary>
        /// get or set get database servers method
        /// </summary>
        public static Func<ICommand, IEnumerable<ServerInfo>> GetDBServerMethod
        {
            get; set;
        }

        /// <summary>
        /// db engines
        /// </summary>
        public static Dictionary<ServerType, IDbEngine> DbEngines
        {
            get
            {
                return _dbEngines;
            }
        }

        #endregion
    }
}
