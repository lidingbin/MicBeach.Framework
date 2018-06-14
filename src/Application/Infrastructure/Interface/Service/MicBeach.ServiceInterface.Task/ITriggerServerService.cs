﻿using MicBeach.Develop.CQuery;
using MicBeach.Util.Paging;
using MicBeach.DTO.Task.Cmd;
using MicBeach.DTO.Task.Query;
using MicBeach.DTO.Task.Query.Filter;
using MicBeach.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Response;

namespace MicBeach.ServiceInterface.Task
{
    /// <summary>
    /// 服务节点执行计划服务接口
    /// </summary>
    public interface ITriggerServerService
    {
        #region 获取服务节点执行计划

        /// <summary>
        /// 获取服务节点执行计划
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        TriggerServerDto GetTriggerServer(TriggerServerFilterDto filter);

        #endregion

        #region 获取服务节点执行计划列表

        /// <summary>
        /// 获取服务节点执行计划列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        List<TriggerServerDto> GetTriggerServerList(TriggerServerFilterDto filter);

        #endregion

        #region 获取服务节点执行计划分页

        /// <summary>
        /// 获取服务节点执行计划分页
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        IPaging<TriggerServerDto> GetTriggerServerPaging(TriggerServerFilterDto filter);

        #endregion

        #region 删除服务节点执行计划

        /// <summary>
        /// 删除服务节点执行计划
        /// </summary>
        /// <param name="deleteInfo">删除信息</param>
        /// <returns>执行结果</returns>
        Result DeleteTriggerServer(DeleteTriggerServerCmdDto deleteInfo);

        #endregion

        #region 修改计划服务运行状态

        /// <summary>
        /// 修改计划服务运行状态
        /// </summary>
        /// <param name="stateInfo">状态信息</param>
        /// <returns></returns>
        Result ModifyRunState(ModifyTriggerServerRunStateCmdDto stateInfo);

        #endregion

        #region 保存服务节点执行计划

        /// <summary>
        /// 保存服务节点执行计划
        /// </summary>
        /// <param name="saveInfo">保存信息</param>
        /// <returns></returns>
        Result SaveTriggerServer(SaveTriggerServerCmdDto saveInfo);

        #endregion
    }
}
