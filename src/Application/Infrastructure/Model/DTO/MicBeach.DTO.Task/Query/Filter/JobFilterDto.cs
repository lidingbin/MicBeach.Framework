﻿using MicBeach.Util.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DTO.Task.Query.Filter
{
    /// <summary>
    /// 工作任务查询信息
    /// </summary>
    public class JobFilterDto : PagingFilter
    {
        #region	属性

        /// <summary>
        /// 编号
        /// </summary>
        public List<string> Ids
        {
            get;
            set;
        }

        /// <summary>
        /// 分组
        /// </summary>
        public string Group
        {
            get;
            set;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 任务类型
        /// </summary>
        public int? Type
        {
            get;
            set;
        }

        /// <summary>
        /// 执行类型
        /// </summary>
        public int? RunType
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        public int? State
        {
            get;
            set;
        }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 任务路径
        /// </summary>
        public string JobPath
        {
            get;
            set;
        }

        /// <summary>
        /// 任务文件名称
        /// </summary>
        public string JobFileName
        {
            get;
            set;
        }

        #endregion

        #region 数据加载

        /// <summary>
        /// 加载分组
        /// </summary>
        public bool LoadGroup
        {
            get;set;
        }

        #endregion
    }
}