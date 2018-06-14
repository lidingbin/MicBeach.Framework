using System;
using MicBeach.Develop.Domain.Aggregation;
using MicBeach.Domain.Task.Repository;
using MicBeach.Util.Extension;
using MicBeach.Util.Data;
using MicBeach.Develop.CQuery;
using MicBeach.Query.Task;
using System.Collections.Generic;
using MicBeach.Util.CustomerException;

namespace MicBeach.Domain.Task.Model
{
    /// <summary>
    /// 工作分组
    /// </summary>
    public class JobGroup : AggregationRoot<JobGroup>
    {
        IJobGroupRepository jobGroupRepository = null;

        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        protected string _code;

        /// <summary>
        /// 名称
        /// </summary>
        protected string _name;

        /// <summary>
        /// 排序
        /// </summary>
        protected int _sort;

        /// <summary>
        /// 上级
        /// </summary>
        protected LazyMember<JobGroup> _parent;

        /// <summary>
        /// 根节点
        /// </summary>
        protected LazyMember<JobGroup> _root;

        /// <summary>
        /// 等级
        /// </summary>
        protected int _level;

        /// <summary>
        /// 说明
        /// </summary>
        protected string _remark;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化工作分组对象
        /// </summary>
        /// <param name="code">编号</param>
        internal JobGroup(string code = "")
        {
            _code = code;
            _parent = new LazyMember<JobGroup>(LoadParentGroup);
            _root = new LazyMember<JobGroup>(LoadRootGroup);
            jobGroupRepository = this.Instance<IJobGroupRepository>();
        }

        #endregion

        #region	属性

        /// <summary>
        /// 编号
        /// </summary>
        public string Code
        {
            get
            {
                return _code;
            }
            protected set
            {
                _code = value;
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
        /// 排序
        /// </summary>
        public int Sort
        {
            get
            {
                return _sort;
            }
            protected set
            {
                _sort = value;
            }
        }

        /// <summary>
        /// 上级
        /// </summary>
        public JobGroup Parent
        {
            get
            {
                return _parent.Value;
            }
            protected set
            {
                _parent.SetValue(value, false);
            }
        }

        /// <summary>
        /// 根节点
        /// </summary>
        public JobGroup Root
        {
            get
            {
                return _root.Value;
            }
            protected set
            {
                _root.SetValue(value, false);
            }
        }

        /// <summary>
        /// 等级
        /// </summary>
        public int Level
        {
            get
            {
                return _level;
            }
            protected set
            {
                _level = value;
            }
        }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark
        {
            get
            {
                return _remark;
            }
            set
            {
                _remark = value;
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
            ValidateRootGroup();//根节点
            jobGroupRepository.Save(this);
        }

        #endregion

        #region	移除

        /// <summary>
        /// 移除
        /// </summary>
        public override void Remove()
        {
            jobGroupRepository.Remove(this);
        }

        #endregion

        #region 设置上级分组

        /// <summary>
        /// 设置上级分组
        /// </summary>
        /// <param name="parentGroup">上级分组</param>
        public void SetParentGroup(JobGroup parentGroup, bool init = true)
        {
            int parentLevel = 0;
            string parentCode = string.Empty;
            if (parentGroup != null)
            {
                parentLevel = parentGroup.Level;
                parentCode = parentGroup.Code;
            }
            if (parentCode == _code && !PrimaryValueIsNone())
            {
                throw new AppException("不能将分组本身设置为自己的上级分组");
            }
            //排序
            IQuery sortQuery = QueryFactory.Create<JobGroupQuery>(r => r.Parent == parentCode);
            sortQuery.AddQueryFields<JobGroupQuery>(c => c.Sort);
            int maxSortIndex = jobGroupRepository.Max<int>(sortQuery);
            _sort = maxSortIndex + 1;
            _parent.SetValue(parentGroup, true);
            //等级
            int newLevel = parentLevel + 1;
            bool modifyChild = newLevel != _level;
            _level = newLevel;
            if (modifyChild)
            {
                //修改所有子集信息
                ModifyChildGroup();
            }
        }

        #endregion

        /// <summary>
        /// 设置根组的值
        /// </summary>
        /// <param name="rootGroup">根组对象</param>
        /// <param name="init">是否初始化，初始化后将不再自动加载值</param>
        public void SetRootGroup(JobGroup rootGroup, bool init = true)
        {
            _root.SetValue(rootGroup, init);
        }

        #region 初始化标识信息

        /// <summary>
        /// 初始化标识信息
        /// </summary>
        public override void InitPrimaryValue()
        {
            base.InitPrimaryValue();
            _code = GenerateJobGroupId();
        }

        #endregion

        #endregion

        #region 内部方法

        #region 加载上级分组

        /// <summary>
        /// 加载上级分组
        /// </summary>
        /// <returns></returns>
        JobGroup LoadParentGroup()
        {
            if (!AllowLazyLoad(r => r.Parent))
            {
                return _parent.CurrentValue;
            }
            if (_level <= 1 || _parent.CurrentValue == null)
            {
                return _parent.CurrentValue;
            }
            IQuery parentQuery = QueryFactory.Create<JobGroupQuery>(r => r.Code == _parent.CurrentValue.Code);
            return jobGroupRepository.Get(parentQuery);
        }

        #endregion

        #region 加载根分组

        /// <summary>
        /// 加载根分组
        /// </summary>
        /// <returns></returns>
        JobGroup LoadRootGroup()
        {
            if (!AllowLazyLoad(r => r.Root))
            {
                return _root.CurrentValue;
            }
            if (_level <= 1 || _root.CurrentValue == null)
            {
                return _root.CurrentValue;
            }
            IQuery rootQuery = QueryFactory.Create<JobGroupQuery>(r => r.Code == _root.CurrentValue.Code);
            return jobGroupRepository.Get(rootQuery);
        }

        #endregion

        #region 修改下级分组

        /// <summary>
        /// 修改下级分组
        /// </summary>
        void ModifyChildGroup()
        {
            if (IsNew)
            {
                return;
            }
            IQuery query = QueryFactory.Create<JobGroupQuery>(r => r.Parent == Code);
            List<JobGroup> childGroupList = jobGroupRepository.GetList(query);
            foreach (var group in childGroupList)
            {
                group.SetParentGroup(this);
                group.Save();
            }
        }

        #endregion

        #region 验证根组

        /// <summary>
        /// 验证根组
        /// </summary>
        void ValidateRootGroup()
        {
            if (_root.CurrentValue == null || _root.CurrentValue.Code.IsNullOrEmpty())
            {
                var newRootGroup = CreateJobGroup(_code);
                _root.SetValue(newRootGroup, false);
            }
        }

        #endregion

        #region 验证对象标识信息是否未设置

        /// <summary>
        /// 判断对象标识信息是否未设置
        /// </summary>
        /// <returns></returns>
        protected override bool PrimaryValueIsNone()
        {
            return _code.IsNullOrEmpty();
        }

        #endregion

        #region 更新上级分组

        /// <summary>
        /// 更新上级分组
        /// </summary>
        void ModifyParentGroup()
        {
            //int parentLevel = 0;
            //string parentId = "";
            //if (parentGroup != null)
            //{
            //    if (_code == parentGroup.Code)
            //    {
            //        throw new Exception("不能将分组数据设置为自己的上级分组");
            //    }
            //    if (parentGroup.Root != null && parentGroup.Root.Code == _code)
            //    {
            //        throw new Exception("不能将当前分组的下级设置为上级分组");
            //    }
            //    parentLevel = parentGroup.Level;
            //    parentId = parentGroup.Code;
            //    _root.SetValue(parentGroup.Root, false);
            //}
            //else
            //{
            //    _root.SetValue(null, false);
            //}
            ////排序
            //IQuery sortQuery = QueryFactory.Create<JobGroupQuery>(r => r.Parent == parentId);
            //sortQuery.AddQueryFields<JobGroupQuery>(c => c.Sort);
            //int maxSortIndex = jobGroupRepository.Max<int>(sortQuery);
            //_sort = maxSortIndex + 1;
            //_parent.SetValue(parentGroup, true);
            ////等级
            //int newLevel = parentLevel + 1;
            //bool modifyChild = newLevel != _level;
            //_level = newLevel;
            //if (modifyChild)
            //{
            //    //修改所有子集信息
            //    ModifyChildGroup();
            //}
        }

        #endregion

        #endregion

        #region 静态方法

        #region 生成一个工作分组编号

        /// <summary>
        /// 生成一个工作分组编号
        /// </summary>
        /// <returns></returns>
        public static string GenerateJobGroupId()
        {
            return Guid.NewGuid().ToString();//RandomHelper.GenerateSerialNumber();
        }

        #endregion

        #region 创建工作分组

        /// <summary>
        /// 创建一个工作分组对象
        /// </summary>
        /// <param name="code">编号</param>
        /// <returns></returns>
        public static JobGroup CreateJobGroup(string code = "")
        {
            code = code.IsNullOrEmpty() ? GenerateJobGroupId() : code;
            return new JobGroup(code);
        }

        #endregion

        #endregion

        #endregion
    }
}