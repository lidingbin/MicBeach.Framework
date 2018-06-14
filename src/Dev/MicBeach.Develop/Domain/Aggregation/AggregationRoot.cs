using MicBeach.Develop.DataValidation;
using MicBeach.Util.ExpressionUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.Domain.Aggregation
{
    /// <summary>
    /// 聚合对象基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AggregationRoot<T> : IAggregationRoot<T> where T : AggregationRoot<T>
    {
        /// <summary>
        /// 新创建的对象
        /// </summary>
        protected LifeStatus _lifeStatus = LifeStatus.New;

        //允许自动加载数据的属性
        protected Dictionary<string, bool> _allowLoadPropertys = new Dictionary<string, bool>();

        /// <summary>
        /// 批量返回
        /// </summary>
        protected bool _batchReturn = false;

        /// <summary>
        /// 启用延迟加载数据
        /// </summary>
        protected bool _loadLazyMember = true;

        #region 属性

        /// <summary>
        /// 能否保存
        /// </summary>
        public bool CanBeSave
        {
            get
            {
                return SaveValidation();
            }
        }

        /// <summary>
        /// 能否移除
        /// </summary>
        public bool CanBeRemove
        {
            get
            {
                return RemoveValidation();
            }
        }

        /// <summary>
        /// 对象状态
        /// </summary>
        public LifeStatus LifeStatus
        {
            get
            {
                return _lifeStatus;
            }
            private set
            {
                _lifeStatus = value;
            }
        }

        /// <summary>
        /// 是否为新对象
        /// </summary>
        public bool IsNew
        {
            get
            {
                return _lifeStatus == LifeStatus.New;
            }
        }

        /// <summary>
        /// 移除
        /// </summary>
        public bool IsRemove
        {
            get
            {
                return _lifeStatus == LifeStatus.Remove;
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        public bool IsModify
        {
            get
            {
                return _lifeStatus == LifeStatus.Modify;
            }
        }

        /// <summary>
        /// 使用新增方法保存
        /// </summary>
        public bool SaveByAdd
        {
            get
            {
                return IsNew || IsRemove;
            }
        }

        /// <summary>
        /// 已经保存的值
        /// </summary>
        private T StoredData { get; set; } = default(T);

        /// <summary>
        /// 是否批量返回
        /// </summary>
        protected bool BatchReturn
        {

            get
            {
                return _batchReturn;
            }
            private set
            {
                _batchReturn = value;
                _loadLazyMember = !value;
            }
        }

        /// <summary>
        /// 是否允许加载延迟对象
        /// </summary>
        protected bool LoadLazyMember
        {

            get
            {
                return _loadLazyMember;
            }
        }

        /// <summary>
        /// 允许自动加载的属性
        /// </summary>
        protected Dictionary<string, bool> LoadPropertys
        {
            get
            {
                return _allowLoadPropertys;
            }
            private set
            {
                _allowLoadPropertys = value;
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 标记为新的对象
        /// </summary>
        public bool MarkNew()
        {
            MarkLifeStatus(LifeStatus.New);
            return true;
        }

        /// <summary>
        /// 标记为移除
        /// </summary>
        public bool MarkRemove()
        {
            MarkLifeStatus(LifeStatus.Remove);
            return true;
        }

        /// <summary>
        /// 标记为修改
        /// </summary>
        public bool MarkModify()
        {
            MarkLifeStatus(LifeStatus.Modify);
            return true;
        }

        /// <summary>
        /// 标记为保存
        /// </summary>
        /// <returns></returns>
        public bool MarkStored()
        {
            MarkLifeStatus(LifeStatus.Stored);
            return true;
        }

        /// <summary>
        /// 标记为新的状态
        /// </summary>
        protected void MarkLifeStatus(LifeStatus status)
        {
            _lifeStatus = status;
        }

        /// <summary>
        /// 保存方法
        /// </summary>
        public abstract void Save();

        /// <summary>
        /// 移除方法
        /// </summary>
        public abstract void Remove();

        /// <summary>
        /// 保存数据验证
        /// </summary>
        /// <returns></returns>
        protected virtual bool SaveValidation()
        {
            if (PrimaryValueIsNone())
            {
                if (SaveByAdd)
                {
                    InitPrimaryValue();
                    if (PrimaryValueIsNone())
                    {
                        throw new Exception("请初始化对象的标识值");
                    }
                }
                else
                {
                    throw new Exception("未指定要保存对象的标识值");
                }
            }
            //数据验证
            var verifyResults = ValidationManager.Validate(this);
            string[] errorMessages = verifyResults.GetErrorMessage();
            if (errorMessages != null && errorMessages.Length > 0)
            {
                throw new Exception(string.Join("\n", errorMessages));
            }
            return true;
        }

        /// <summary>
        /// 移除数据验证
        /// </summary>
        /// <returns></returns>
        protected virtual bool RemoveValidation()
        {
            if (PrimaryValueIsNone())
            {
                throw new Exception("未指定要移除数据的标识值");
            }
            return true;
        }

        /// <summary>
        /// 初始化对象唯一标识值
        /// </summary>
        public virtual void InitPrimaryValue()
        {

        }

        /// <summary>
        /// 验证对象主要标识信息是否未设置
        /// </summary>
        /// <returns></returns>
        protected abstract bool PrimaryValueIsNone();

        /// <summary>
        /// 从相似的对象拷贝数据
        /// </summary>
        /// <typeparam name="DT">数据类型</typeparam>
        /// <param name="similarObject">相识对象</param>
        /// <param name="excludePropertys">排除不复制的属性</param>
        protected virtual void CopyDataFromSimilarObject<DT>(DT similarObject, IEnumerable<string> excludePropertys = null) where DT : T
        {
        }

        /// <summary>
        /// 检查是否允许加载指定属性
        /// </summary>
        /// <param name="property">属性</param>
        /// <returns>是否允许加载属性</returns>
        protected bool AllowLazyLoad(string property)
        {
            if (!_loadLazyMember || _allowLoadPropertys == null || !_allowLoadPropertys.ContainsKey(property))
            {
                return false;
            }
            return _allowLoadPropertys[property];
        }

        /// <summary>
        /// 检查是否允许加载指定属性
        /// </summary>
        /// <param name="property">属性</param>
        /// <returns>是否允许加载属性</returns>
        protected bool AllowLazyLoad(Expression<Func<T, dynamic>> property)
        {
            if (property == null)
            {
                return false;
            }
            return AllowLazyLoad(ExpressionHelper.GetExpressionPropertyName(property.Body));
        }

        /// <summary>
        /// 根据其他相似对象初始化当前对象
        /// </summary>
        /// <typeparam name="DT">数据类型</typeparam>
        /// <param name="similarObject">相似对象</param>
        /// <returns></returns>
        public void InitFromSimilarObject<DT>(DT similarObject) where DT : AggregationRoot<T>, T
        {
            if (similarObject == null)
            {
                return;
            }
            MarkLifeStatus(similarObject.LifeStatus);
            CopyDataFromSimilarObject(similarObject);//复制数据
            //合并存储数据
            if (similarObject.StoredData != null)
            {
                if (StoredData == null)
                {
                    StoredData = similarObject.StoredData;
                }
                else
                {
                    StoredData.CopyDataFromSimilarObject(similarObject.StoredData);
                }
            }
        }

        #endregion
    }
}
