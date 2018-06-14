using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Util.Net
{
    /// <summary>
    /// File Filter
    /// </summary>
    public class FileFilter
    {
        /// <summary>
        /// suffix list
        /// </summary>
        public IEnumerable<string> Suffixs
        {
            get; set;
        }

        /// <summary>
        /// path list
        /// </summary>
        public IEnumerable<string> Paths
        {
            get; set;
        }

        /// <summary>
        /// search child folder
        /// </summary>
        public bool SearchChildFolder
        {
            get; set;
        }
    }
}
