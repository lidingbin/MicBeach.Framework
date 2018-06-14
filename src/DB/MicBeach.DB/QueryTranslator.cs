using MicBeach.Develop.Command;
using MicBeach.Develop.CQuery;
using MicBeach.Develop.CQuery.Translator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DB
{
    /// <summary>
    /// translator query object
    /// </summary>
    public static class QueryTranslator
    {
        /// <summary>
        /// translate query object
        /// </summary>
        /// <param name="query">query object</param>
        /// <param name="serverInfo">database server</param>
        /// <returns></returns>
        public static TranslateResult Translate(IQuery query, ServerInfo serverInfo)
        {
            var translator = GetTranslator(serverInfo);
            if (translator == null)
            {
                return TranslateResult.Empty;
            }
            return translator.Translate(query);
        }

        /// <summary>
        /// get translator
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        public static IQueryTranslator GetTranslator(ServerInfo server)
        {
            if (server == null)
            {
                return null;
            }
            IQueryTranslator translator = null;
            switch (server.ServerType)
            {
                case ServerType.SQLServer:
                    translator = new SqlServerQueryTranslator();
                    break;
                case ServerType.MySQL:
                    translator = new MySqlQueryTranslator();
                    break;
            }
            return translator;
        }
    }
}
