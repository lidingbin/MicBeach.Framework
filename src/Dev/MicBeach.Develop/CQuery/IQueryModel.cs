using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.CQuery
{
    /// <summary>
    /// QueryModel Base
    /// </summary>
    public interface IQueryModel<in T> where T : IQueryModel<T>
    {
    }
}
