using MicBeach.Develop.CQuery;
using MicBeach.Util.Paging;
using MicBeach.Develop.Domain.Aggregation;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.Domain.Repository
{
    /// <summary>
    /// 对象仓储接口
    /// </summary>
    /// <typeparam name="T">聚合对象类型</typeparam>
    public interface IRepository<T> where T : IAggregationRoot<T>
    {
        #region 属性

        /// <summary>
        /// 数据移除事件
        /// </summary>
        event DataChange<T> RemoveEvent;

        /// <summary>
        /// 数据保存事件
        /// </summary>
        event DataChange<T> SaveEvent;

        /// <summary>
        /// 查询事件
        /// </summary>
        event QueryData<T> QueryEvent;

        #endregion

        #region 方法

        /// <summary>
        /// 保存对象集合
        /// </summary>
        /// <param name="objList">对象信息</param>
        void Save(params T[] objDatas);

        /// <summary>
        /// 移除对象集合
        /// </summary>
        /// <param name="objDatas">对象信息</param>
        void Remove(params T[] objDatas);

        /// <summary>
        /// 根据条件移除对象
        /// </summary>
        /// <param name="query">查询对象</param>
        void Remove(IQuery query);

        /// <summary>
        /// 更新权限分组
        /// </summary>
        /// <param name="expression">更新表达式</param>
        /// <param name="query">更新条件</param>
        void Modify(IModify expression, IQuery query);

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns>对象</returns>
        T Get(IQuery query);

        /// <summary>
        /// 获取对象列表
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns>对象列表</returns>
        List<T> GetList(IQuery query);

        /// <summary>
        /// 获取对象分页
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns>对象列表</returns>
        IPaging<T> GetPaging(IQuery query);

        /// <summary>
        /// 是否存在指定的数据
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        bool Exist(IQuery query);

        /// <summary>
        /// 数据量
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        long Count(IQuery query);

        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="DT">返回数据类型</typeparam>
        /// <param name="query">查询条件</param>
        /// <returns>最大值</returns>
        DT Max<DT>(IQuery query);

        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="DT">返回数据类型</typeparam>
        /// <param name="query"></param>
        /// <returns>最小值</returns>
        DT Min<DT>(IQuery query);

        /// <summary>
        /// 求和
        /// </summary>
        /// <typeparam name="DT">返回数据类型</typeparam>
        /// <param name="query">查询条件</param>
        /// <returns>总和</returns>
        DT Sum<DT>(IQuery query);

        /// <summary>
        /// 平均值
        /// </summary>
        /// <typeparam name="DT">返回数据类型</typeparam>
        /// <param name="query">查询条件</param>
        /// <returns>平均值</returns>
        DT Avg<DT>(IQuery query);

        #endregion
    }
}
