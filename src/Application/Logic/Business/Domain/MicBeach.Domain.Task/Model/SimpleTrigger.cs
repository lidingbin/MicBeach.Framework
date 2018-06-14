using System;
using MicBeach.Develop.Domain.Aggregation;
using MicBeach.Util;
using MicBeach.Domain.Task.Repository;
using MicBeach.Util.Extension;
using MicBeach.CTask;
using System.Collections.Generic;

namespace MicBeach.Domain.Task.Model
{
    /// <summary>
    /// 简单类型执行计划
    /// </summary>
    public class SimpleTrigger : Trigger
    {
        #region	字段

        /// <summary>
        /// 重复次数
        /// </summary>
        protected int _repeatCount;

        /// <summary>
        /// 重复间隔
        /// </summary>
        protected long _repeatInterval;

        /// <summary>
        /// 一直重复执行
        /// </summary>
        protected bool _repeatForever;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化简单类型执行计划对象
        /// </summary>
        /// <param name="triggerId">编号</param>
        internal SimpleTrigger(string triggerId = ""):base(triggerId)
        {
            _type = TaskTriggerType.简单;
        }

        #endregion

        #region	属性

        /// <summary>
        /// 重复次数
        /// </summary>
        public int RepeatCount
        {
            get
            {
                return _repeatCount;
            }
            set
            {
                _repeatCount = value;
            }
        }

        /// <summary>
        /// 重复间隔
        /// </summary>
        public long RepeatInterval
        {
            get
            {
                return _repeatInterval;
            }
            set
            {
                _repeatInterval = value;
            }
        }

        /// <summary>
        /// 一直重复执行
        /// </summary>
        public bool RepeatForever
        {
            get
            {
                return _repeatForever;
            }
            set
            {
                _repeatForever = value;
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 保存
        /// </summary>
        public override void Save()
        {
            if (SaveByAdd)
            {
                StartTime = DateTime.Now;
                EndTime = DateTime.Now;
            }
            base.Save();
        }

        #region 根据给定的计划对象更新当前信息

        /// <summary>
        /// 根据给定的计划对象更新当前信息
        /// </summary>
        /// <param name="trigger">其它计划信息</param>
        public override void ModifyFromOtherTrigger(Trigger trigger,IEnumerable<string> excludePropertys= null)
        {
            base.ModifyFromOtherTrigger(trigger, excludePropertys);
            if (trigger == null||!(trigger is SimpleTrigger))
            {
                return;
            }
            var simpleTrigger = trigger as SimpleTrigger;
            _repeatCount = simpleTrigger.RepeatCount;
            _repeatForever = simpleTrigger._repeatForever;
            _repeatInterval = simpleTrigger.RepeatInterval;
        } 

        #endregion

        #endregion
    }
}