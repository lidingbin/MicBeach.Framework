using MicBeach.Develop.CQuery;
using MicBeach.Util.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.Domain.Repository
{
    /// <summary>
    /// 存储实现
    /// </summary>
    public abstract class BaseRepository<DT>
    {
        /// <summary>
        /// 保存对象集合
        /// </summary>
        /// <param name="objList">对象信息</param>
        public abstract void Save(params DT[] objDatas);

        /// <summary>
        /// 移除对象集合
        /// </summary>
        /// <param name="objDatas">对象信息</param>
        public abstract void Remove(params DT[] objDatas);

        /// <summary>
        /// 根据条件移除对象
        /// </summary>
        /// <param name="query">查询对象</param>
        public abstract void Remove(IQuery query);

        /// <summary>
        /// 更新权限分组
        /// </summary>
        /// <param name="expression">更新表达式</param>
        /// <param name="query">更新条件</param>
        public abstract void Modify(IModify expression, IQuery query);

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns>对象</returns>
        public abstract DT Get(IQuery query);

        /// <summary>
        /// 获取对象列表
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns>对象列表</returns>
        public abstract List<DT> GetList(IQuery query);

        /// <summary>
        /// 获取对象分页
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns>对象列表</returns>
        public abstract IPaging<DT> GetPaging(IQuery query);

        /// <summary>
        /// 是否存在指定的数据
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public abstract bool Exist(IQuery query);

        /// <summary>
        /// 数据量
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public abstract long Count(IQuery query);

        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="DT">返回数据类型</typeparam>
        /// <param name="query">查询条件</param>
        /// <returns>最大值</returns>
        public abstract VT Max<VT>(IQuery query);

        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="DT">返回数据类型</typeparam>
        /// <param name="query"></param>
        /// <returns>最小值</returns>
        public abstract VT Min<VT>(IQuery query);

        /// <summary>
        /// 求和
        /// </summary>
        /// <typeparam name="DT">返回数据类型</typeparam>
        /// <param name="query">查询条件</param>
        /// <returns>总和</returns>
        public abstract VT Sum<VT>(IQuery query);

        /// <summary>
        /// 平均值
        /// </summary>
        /// <typeparam name="DT">返回数据类型</typeparam>
        /// <param name="query">查询条件</param>
        /// <returns>平均值</returns>
        public abstract VT Avg<VT>(IQuery query);
    }
}
