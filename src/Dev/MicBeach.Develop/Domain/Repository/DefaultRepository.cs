using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Develop.CQuery;
using MicBeach.Util.Paging;
using MicBeach.Develop.Domain.Aggregation;
using MicBeach.Util.Extension;
using MicBeach.Develop.Command;
using MicBeach.Util.IoC;
using MicBeach.Util;
using MicBeach.Develop.DataAccess;

namespace MicBeach.Develop.Domain.Repository
{
    /// <summary>
    /// 默认存储
    /// </summary>
    public abstract class DefaultRepository<DT, ET, DAI> : BaseRepository<DT> where DT : IAggregationRoot<DT> where ET : CommandEntity<ET> where DAI : IDataAccess<ET>
    {
        protected IDataAccess<ET> dataAccess = ContainerManager.Resolve<DAI>();

        public DefaultRepository()
        {
            BindEvent();
        }

        #region 属性

        public event DataChange<DT> RemoveEvent;//移除事件
        public event DataChange<DT> SaveEvent; //保存事件
        public event QueryData<DT> QueryEvent;//查询事件

        #endregion

        #region 接口方法

        /// <summary>
        /// 保存对象集合
        /// </summary>
        /// <param name="objList">对象信息</param>
        public sealed override void Save(params DT[] objDatas)
        {
            #region 参数验证

            if (objDatas == null || objDatas.Length <= 0)
            {
                throw new Exception("objDatas is null or empty");
            }
            foreach (var obj in objDatas)
            {
                if (obj == null)
                {
                    throw new Exception("save object data is null");
                }
                if (!obj.CanBeSave)
                {
                    throw new Exception("object data cann't to be save");
                }
            }

            #endregion

            ExecuteSave(objDatas);//执行保存

            SaveEvent?.Invoke(objDatas);//保存事件

            #region 保存后数据操作

            foreach (var data in objDatas)
            {
                data.MarkStored();//标记为已保存
            }

            #endregion
        }

        /// <summary>
        /// 移除对象集合
        /// </summary>
        /// <param name="objDatas">对象信息</param>
        public sealed override void Remove(params DT[] objDatas)
        {
            #region 参数验证

            if (objDatas == null || objDatas.Length <= 0)
            {
                throw new Exception("objDatas is null or empty");
            }
            foreach (var obj in objDatas)
            {
                if (obj == null)
                {
                    throw new Exception("remove object data is null");
                }
                if (!obj.CanBeRemove)
                {
                    throw new Exception("object data cann't to be remove");
                }
            }

            #endregion

            ExecuteRemove(objDatas);//执行移除操作
            RemoveEvent?.Invoke(objDatas);//移除事件

            #region 移除后数据操作

            foreach (var data in objDatas)
            {
                data.MarkRemove();//标记为已移除
            }

            #endregion
        }

        /// <summary>
        /// 根据条件移除对象
        /// </summary>
        /// <param name="query">查询对象</param>
        public sealed override void Remove(IQuery query)
        {
            ExecuteRemove(query);
        }

        /// <summary>
        /// 更新权限分组
        /// </summary>
        /// <param name="expression">更新表达式</param>
        /// <param name="query">更新条件</param>
        public sealed override void Modify(IModify expression, IQuery query)
        {
            ExecuteModify(expression, query);
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns>对象</returns>
        public sealed override DT Get(IQuery query)
        {
            var data = GetData(query);
            if (QueryEvent == null)
            {
                return data;
            }
            IEnumerable<DT> dataList = new List<DT>() { data };
            QueryEvent?.Invoke(ref dataList);
            return dataList.FirstOrDefault();
        }

        /// <summary>
        /// 获取对象列表
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns>对象列表</returns>
        public sealed override List<DT> GetList(IQuery query)
        {
            IEnumerable<DT> datas = GetDataList(query);
            if (QueryEvent == null)
            {
                return datas.ToList();
            }
            QueryEvent?.Invoke(ref datas);
            return datas.ToList();
        }

        /// <summary>
        /// 获取对象分页
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns>对象列表</returns>
        public sealed override IPaging<DT> GetPaging(IQuery query)
        {
            var paging = GetDataPaging(query);
            if (QueryEvent == null)
            {
                return paging;
            }
            IEnumerable<DT> datas = paging;
            QueryEvent?.Invoke(ref datas);
            return new Paging<DT>(paging.Page, paging.PageSize, paging.TotalCount, datas);
        }

        /// <summary>
        /// 是否存在指定的数据
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public sealed override bool Exist(IQuery query)
        {
            return IsExist(query);
        }

        /// <summary>
        /// 数据量
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public sealed override long Count(IQuery query)
        {
            return CountValue(query);
        }

        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="DT">返回数据类型</typeparam>
        /// <param name="query">查询条件</param>
        /// <returns>最大值</returns>
        public sealed override VT Max<VT>(IQuery query)
        {
            return MaxValue<VT>(query);
        }

        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <typeparam name="DT">返回数据类型</typeparam>
        /// <param name="query"></param>
        /// <returns>最小值</returns>
        public sealed override VT Min<VT>(IQuery query)
        {
            return MinValue<VT>(query);
        }

        /// <summary>
        /// 求和
        /// </summary>
        /// <typeparam name="DT">返回数据类型</typeparam>
        /// <param name="query">查询条件</param>
        /// <returns>总和</returns>
        public sealed override VT Sum<VT>(IQuery query)
        {
            return SumValue<VT>(query);
        }

        /// <summary>
        /// 平均值
        /// </summary>
        /// <typeparam name="DT">返回数据类型</typeparam>
        /// <param name="query">查询条件</param>
        /// <returns>平均值</returns>
        public sealed override VT Avg<VT>(IQuery query)
        {
            return AvgValue<VT>(query);
        }

        /// <summary>
        /// 事件绑定
        /// </summary>
        protected virtual void BindEvent()
        {

        }

        #endregion

        #region 功能方法

        /// <summary>
        /// 执行保存
        /// </summary>
        /// <param name="objDatas">保存数据</param>
        protected virtual void ExecuteSave(params DT[] objDatas)
        {
            if (objDatas == null || objDatas.Length <= 0)
            {
                return;
            }
            foreach (var data in objDatas)
            {
                Save(data);
            }
        }

        /// <summary>
        /// 执行移除
        /// </summary>
        /// <param name="objDatas">移除数据</param>
        protected virtual void ExecuteRemove(params DT[] objDatas)
        {
            IEnumerable<ET> entityList = objDatas.Select(c => c.MapTo<ET>());
            Remove(entityList);
        }

        /// <summary>
        /// 执行移除
        /// </summary>
        /// <param name="query">移除条件</param>
        protected virtual void ExecuteRemove(IQuery query)
        {
            if (query == null)
            {
                query = QueryFactory.Create();
            }
            Type entityType = typeof(ET);
            var keys = QueryConfig.GetPrimaryKeys(entityType);
            if (keys.IsNullOrEmpty())
            {
                throw new Exception(string.Format("Type:{0} isn't set primary keys", entityType.FullName));
            }
            var dataList = GetList(query);
            if (dataList == null || dataList.Count <= 0)
            {
                return;
            }
            Remove(dataList.ToArray());
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        protected virtual DT GetData(IQuery query)
        {
            var entityData = dataAccess.Get(query);
            DT data = default(DT);
            if (entityData != null)
            {
                data = entityData.MapTo<DT>();
            }
            return data;
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        protected virtual List<DT> GetDataList(IQuery query)
        {
            var entityDataList = dataAccess.GetList(query);
            if (entityDataList.IsNullOrEmpty())
            {
                return new List<DT>(0);
            }
            return entityDataList.Select(c => c.MapTo<DT>()).ToList();
        }

        /// <summary>
        /// 获取数据分页
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        protected virtual IPaging<DT> GetDataPaging(IQuery query)
        {
            var entityPaging = dataAccess.GetPaging(query);
            return entityPaging.ConvertTo<DT>();
        }

        /// <summary>
        /// 检查数据是否存在
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        protected virtual bool IsExist(IQuery query)
        {
            return dataAccess.Exist(query);
        }

        /// <summary>
        /// 数据量
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        protected virtual long CountValue(IQuery query)
        {
            return dataAccess.Count(query);
        }

        /// <summary>
        /// 最大值
        /// </summary>
        /// <typeparam name="VT">数据类型</typeparam>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        protected virtual VT MaxValue<VT>(IQuery query)
        {
            return dataAccess.Max<VT>(query);
        }

        /// <summary>
        /// 最小值
        /// </summary>
        /// <typeparam name="VT">数据类型</typeparam>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        protected virtual VT MinValue<VT>(IQuery query)
        {
            return dataAccess.Min<VT>(query);
        }

        /// <summary>
        /// 求和
        /// </summary>
        /// <typeparam name="VT">数据类型</typeparam>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        protected virtual VT SumValue<VT>(IQuery query)
        {
            return dataAccess.Sum<VT>(query);
        }

        /// <summary>
        /// 平均值
        /// </summary>
        /// <typeparam name="VT">数据类型</typeparam>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        protected virtual VT AvgValue<VT>(IQuery query)
        {
            return dataAccess.Avg<VT>(query);
        }

        /// <summary>
        /// 执行修改
        /// </summary>
        /// <param name="expression">修改表达式</param>
        /// <param name="query">条件</param>
        protected virtual void ExecuteModify(IModify expression, IQuery query)
        {
            UnitOfWork.UnitOfWork.RegisterCommand(dataAccess.Modify(expression, query));
        }

        #endregion

        #region Util

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="data">数据</param>
        void Save(DT data)
        {
            if (data.SaveByAdd)
            {
                Add(data);
            }
            else
            {
                Modify(data);
            }
        }

        #region 添加

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="data">数据</param>
        protected void Add(DT data)
        {
            if (data.LifeStatus == Aggregation.LifeStatus.New || data.LifeStatus == Aggregation.LifeStatus.Remove)
            {
                ET entity = data.MapTo<ET>();
                Add(entity);
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="datas">数据</param>
        protected void Add(params ET[] datas)
        {
            UnitOfWork.UnitOfWork.RegisterCommand(dataAccess.Add(datas).ToArray());
        }

        #endregion

        #region 更新

        /// <summary>
        ///修改
        /// </summary>
        /// <param name="data">数据</param>
        protected void Modify(DT data)
        {
            if (data.LifeStatus == Aggregation.LifeStatus.Stored || data.LifeStatus == Aggregation.LifeStatus.Modify)
            {
                ET entity = data.MapTo<ET>();
                Modify(entity);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="datas">数据</param>
        protected void Modify(params ET[] datas)
        {
            Type entityType = typeof(ET);
            var keys = QueryConfig.GetPrimaryKeys(entityType);
            if (keys.IsNullOrEmpty())
            {
                throw new Exception(string.Format("Type:{0} is not set primary keys", entityType.FullName));
            }
            foreach (var data in datas)
            {
                IQuery query = QueryFactory.Create();
                foreach (var key in keys)
                {
                    query.Equal(key, data.GetPropertyValue(key));
                }
                UnitOfWork.UnitOfWork.RegisterCommand(dataAccess.Modify(data, query));
            }
        }

        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="entityList">数据</param>
        protected void Remove(IEnumerable<ET> entityList)
        {
            Type entityType = typeof(ET);
            var keys = QueryConfig.GetPrimaryKeys(entityType);
            if (keys.IsNullOrEmpty())
            {
                throw new Exception(string.Format("Type:{0} isn't set primary keys", entityType.FullName));
            }
            IQuery query = QueryFactory.Create();
            List<dynamic> keyValueList = new List<dynamic>();
            foreach (ET entity in entityList)
            {
                if (keys.Count == 1)
                {
                    keyValueList.Add(entity.GetPropertyValue(keys.ElementAt(0)));
                }
                else
                {
                    IQuery entityQuery = QueryFactory.Create();
                    foreach (var key in keys)
                    {
                        entityQuery.And(key, CriteriaOperator.Equal, entity.GetPropertyValue(key));
                    }
                    query.Or(entityQuery);
                }
            }
            if (keys.Count == 1)
            {
                query.In(keys.ElementAt(0), keyValueList);
            }
            UnitOfWork.UnitOfWork.RegisterCommand(dataAccess.Delete(query));
        }

        #endregion

        #endregion
    }
}
