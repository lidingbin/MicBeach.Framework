using System;
using MicBeach.Develop.Domain.Aggregation;
using MicBeach.Util;
using MicBeach.Domain.Task.Repository;
using MicBeach.Util.Extension;
using MicBeach.CTask;
using MicBeach.Application.Task;
using MicBeach.Util.Data;
using MicBeach.Develop.CQuery;
using MicBeach.Query.Task;
using MicBeach.Domain.Task.Service;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;
using MicBeach.Util.ExpressionUtil;
using MicBeach.Util.Code;

namespace MicBeach.Domain.Task.Model
{
    /// <summary>
    /// 任务执行计划
    /// </summary>
    public class Trigger : AggregationRoot<Trigger>
    {
        ITriggerRepository triggerRepository = null;

        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        protected string _id;

        /// <summary>
        /// 名称
        /// </summary>
        protected string _name;

        /// <summary>
        /// 说明
        /// </summary>
        protected string _description;

        /// <summary>
        /// 所属任务
        /// </summary>
        protected LazyMember<Job> _job;

        /// <summary>
        /// 应用到对象
        /// </summary>
        protected TaskTriggerApplyTo _applyTo;

        /// <summary>
        /// 上次执行时间
        /// </summary>
        protected DateTime _prevFireTime;

        /// <summary>
        /// 下次执行时间
        /// </summary>
        protected DateTime _nextFireTime;

        /// <summary>
        /// 优先级
        /// </summary>
        protected int _priority;

        /// <summary>
        /// 状态
        /// </summary>
        protected TaskTriggerState _state;

        /// <summary>
        /// 类型
        /// </summary>
        protected TaskTriggerType _type;

        /// <summary>
        /// 额外条件类型
        /// </summary>
        protected LazyMember<TriggerCondition> _condition;

        /// <summary>
        /// 开始时间
        /// </summary>
        protected DateTime _startTime;

        /// <summary>
        /// 结束时间
        /// </summary>
        protected DateTime _endTime;

        /// <summary>
        /// 执行失败操作类型
        /// </summary>
        protected TaskTriggerMisFireType _misFireType;

        /// <summary>
        /// 总触发次数
        /// </summary>
        protected int _fireTotalCount;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化任务执行计划对象
        /// </summary>
        /// <param name="id">编号</param>
        internal Trigger(string id = "")
        {
            _id = id;
            _job = new LazyMember<Job>(LoadJob);
            _condition = new LazyMember<TriggerCondition>(LoadCondition);
            triggerRepository = this.Instance<ITriggerRepository>();
        }

        #endregion

        #region	属性

        /// <summary>
        /// 编号
        /// </summary>
        public string Id
        {
            get
            {
                return _id;
            }
            protected set
            {
                _id = value;
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        /// <summary>
        /// 所属任务
        /// </summary>
        public Job Job
        {
            get
            {
                return _job.Value;
            }
            protected set
            {
                _job.SetValue(value, false);
            }
        }

        /// <summary>
        /// 应用到对象
        /// </summary>
        public TaskTriggerApplyTo ApplyTo
        {
            get
            {
                return _applyTo;
            }
            set
            {
                _applyTo = value;
            }
        }

        /// <summary>
        /// 上次执行时间
        /// </summary>
        public DateTime PrevFireTime
        {
            get
            {
                return _prevFireTime;
            }
            protected set
            {
                _prevFireTime = value;
            }
        }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime NextFireTime
        {
            get
            {
                return _nextFireTime;
            }
            protected set
            {
                _nextFireTime = value;
            }
        }

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority
        {
            get
            {
                return _priority;
            }
            set
            {
                _priority = value;
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public TaskTriggerState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        /// <summary>
        /// 类型
        /// </summary>
        public TaskTriggerType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        /// <summary>
        /// 额外条件类型
        /// </summary>
        public TriggerCondition Condition
        {
            get
            {
                return _condition.Value;
            }
            protected set
            {
                _condition.SetValue(value, false);
            }
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime
        {
            get
            {
                return _startTime;
            }
            protected set
            {
                _startTime = value;
            }
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime
        {
            get
            {
                return _endTime;
            }
            protected set
            {
                _endTime = value;
            }
        }

        /// <summary>
        /// 执行失败操作类型
        /// </summary>
        public TaskTriggerMisFireType MisFireType
        {
            get
            {
                return _misFireType;
            }
            protected set
            {
                _misFireType = value;
            }
        }

        /// <summary>
        /// 总触发次数
        /// </summary>
        public int FireTotalCount
        {
            get
            {
                return _fireTotalCount;
            }
            protected set
            {
                _fireTotalCount = value;
            }
        }

        #endregion

        #region 方法

        #region 功能方法

        #region 保存

        /// <summary>
        /// 保存
        /// </summary>
        public override void Save()
        {
            if (SaveByAdd)
            {
                PrevFireTime = DateTime.Now;
                NextFireTime = DateTime.Now;
            }
            triggerRepository.Save(this);
        }

        #endregion

        #region	移除

        /// <summary>
        /// 移除
        /// </summary>
        public override void Remove()
        {
            triggerRepository.Remove(this);
        }

        #endregion

        #region 设置附加条件

        /// <summary>
        /// 设置执行计划附加条件
        /// </summary>
        /// <param name="condition">附加条件数据</param>
        /// <param name="init">是否初始化值（初始化后将不会再次执行自动加载）</param>
        public void SetCondition(TriggerCondition condition, bool init = true)
        {
            _condition.SetValue(condition, init);
        }

        #endregion

        #region 设置计划所属工作任务

        /// <summary>
        /// 设置执行计划所属工作任务
        /// </summary>
        /// <param name="job">工作任务信息</param>
        /// <param name="init">是否设置为已初始化（设置为初始化后将不会再自动加载值）</param>
        public void SetJob(Job job, bool init = true)
        {
            _job.SetValue(job, init);
        }

        #endregion

        #region 根据给定的计划对象更新当前信息

        /// <summary>
        /// 根据给定的计划对象更新当前信息
        /// </summary>
        /// <param name="trigger">其它计划信息</param>
        public virtual void ModifyFromOtherTrigger(Trigger trigger,IEnumerable<string> excludePropertys=null)
        {
            if (trigger == null)
            {
                return;
            }
            CopyDataFromSimilarObject<Trigger>(trigger, excludePropertys);
        }

        #endregion

        #region 初始化对象唯一标识值

        /// <summary>
        /// 初始化对象唯一标识值
        /// </summary>
        public override void InitPrimaryValue()
        {
            base.InitPrimaryValue();
            _id = GenerateTriggerId();
        }

        #endregion

        #endregion

        #region 内部方法

        #region 加载工作信息

        /// <summary>
        /// 加载工作信息
        /// </summary>
        /// <returns></returns>
        Job LoadJob()
        {
            if (!AllowLazyLoad(r => r.Job))
            {
                return _job.CurrentValue;
            }
            if (_job.CurrentValue == null)
            {
                return _job.CurrentValue;
            }
            IQuery jobQuery = QueryFactory.Create<JobQuery>(r => r.Id == _job.CurrentValue.Id);
            return this.Instance<IJobRepository>().Get(jobQuery);
        }

        #endregion

        #region 加载附加条件信息

        /// <summary>
        /// 加载附加条件信息
        /// </summary>
        /// <returns></returns>
        TriggerCondition LoadCondition()
        {
            if (!AllowLazyLoad(r => r.Condition))
            {
                return _condition.CurrentValue;
            }
            if (_condition.CurrentValue == null)
            {
                return _condition.CurrentValue;
            }
            return TriggerConditionService.GetTriggerConditionByTrigger(_id, _condition.CurrentValue.Type);
        }

        #endregion

        #region 从指定对象复制值

        /// <summary>
        /// 从指定对象复制值
        /// </summary>
        /// <typeparam name="DT">数据类型</typeparam>
        /// <param name="similarData">数据对象</param>
        /// <param name="excludePropertys">排除不复制的属性</param>
        protected override void CopyDataFromSimilarObject<DT>(DT similarData, IEnumerable<string> excludePropertys = null)
        {
            base.CopyDataFromSimilarObject<DT>(similarData, excludePropertys);
            if (similarData == null)
            {
                return;
            }
            excludePropertys = excludePropertys ?? new List<string>(0);
            #region 复制值

            if (!excludePropertys.Contains("Id"))
            {
                Id = similarData.Id;
            }
            if (!excludePropertys.Contains("ApplyTo"))
            {
                ApplyTo = similarData.ApplyTo;
            }
            if (!excludePropertys.Contains("Description"))
            {
                Description = similarData.Description;
            }
            if (!excludePropertys.Contains("EndTime"))
            {
                EndTime = similarData.EndTime;
            }
            if (!excludePropertys.Contains("FireTotalCount"))
            {
                FireTotalCount = similarData.FireTotalCount;
            }
            if (!excludePropertys.Contains("Job"))
            {
                Job = similarData.Job;
            }
            if (!excludePropertys.Contains("Name"))
            {
                Name = similarData.Name;
            }
            if (!excludePropertys.Contains("NextFireTime"))
            {
                NextFireTime = similarData.NextFireTime;
            }
            if (!excludePropertys.Contains("PrevFireTime"))
            {
                PrevFireTime = similarData.PrevFireTime;
            }
            if (!excludePropertys.Contains("Priority"))
            {
                Priority = similarData.Priority;
            }
            if (!excludePropertys.Contains("StartTime"))
            {
                StartTime = similarData.StartTime;
            }
            if (!excludePropertys.Contains("State"))
            {
                State = similarData.State;
            }
            if (!excludePropertys.Contains("MisFireType"))
            {
                MisFireType = similarData.MisFireType;
            }
            if (!excludePropertys.Contains("Condition"))
            {
                Condition = similarData.Condition;
                if (Condition != null)
                {
                    Condition.TriggerId = Id;
                }
            }

            #endregion
        }

        #endregion

        #region 判断对象标识信息是否未设置

        /// <summary>
        /// 判断对象标识信息是否未设置
        /// </summary>
        /// <returns></returns>
        protected override bool PrimaryValueIsNone()
        {
            return _id.IsNullOrEmpty();
        }

        #endregion

        #endregion

        #region 静态方法

        #region 生成一个任务执行计划编号

        /// <summary>
        /// 生成一个任务执行计划编号
        /// </summary>
        /// <returns></returns>
        public static string GenerateTriggerId()
        {
            return SerialNumber.GetSerialNumber(TaskApplicationUtil.GetIdGroupCode(TaskIdGroup.执行计划)).ToString();
        }

        #endregion

        #region 创建任务执行计划

        /// <summary>
        /// 创建一个任务执行计划对象
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        public static Trigger CreateTrigger(string id = "", TaskTriggerType type = TaskTriggerType.简单)
        {
            id = id.IsNullOrEmpty() ? GenerateTriggerId() : id;
            Trigger trigger = null;
            switch (type)
            {
                case TaskTriggerType.简单:
                    trigger = new SimpleTrigger(id);
                    break;
                case TaskTriggerType.自定义:
                    trigger = new ExpressionTrigger(id);
                    break;
            }
            return trigger;
        }

        #endregion

        #endregion

        #endregion
    }
}