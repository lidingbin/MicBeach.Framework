using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Util.ObjectMap
{
    /// <summary>
    /// object map manager
    /// </summary>
    public static class ObjectMapManager
    {
        static IObjectMap _objectMapper;

        /// <summary>
        /// Object Mapper
        /// </summary>
        public static IObjectMap ObjectMapper
        {
            get
            {
                return _objectMapper;
            }
            set
            {
                _objectMapper = value;
            }
        }

    }
}
