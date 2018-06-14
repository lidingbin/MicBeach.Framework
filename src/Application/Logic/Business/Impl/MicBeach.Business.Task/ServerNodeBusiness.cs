using MicBeach.Develop.CQuery;
using MicBeach.Util.Paging;
using MicBeach.DTO.Task.Cmd;
using MicBeach.DTO.Task.Query;
using MicBeach.Domain.Task.Repository;
using MicBeach.BusinessInterface.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using MicBeach.Domain.Task.Model;
using MicBeach.Util;
using MicBeach.Develop.UnitOfWork;
using MicBeach.Query.Task;
using MicBeach.DTO.Task.Query.Filter;
using MicBeach.Domain.Task.Service;
using MicBeach.Util.Response;

namespace MicBeach.Business.Task
{
    /// <summary>
    /// 服务节点业务
    /// </summary>
    public class ServerNodeBusiness : IServerNodeBusiness
    {
        public ServerNodeBusiness()
        {
        }

        #region 保存服务节点

        /// <summary>
        /// 保存服务节点
        /// </summary>
        /// <param name="saveInfo">服务节点对象</param>
        /// <returns>执行结果</returns>
        public Result<ServerNodeDto> SaveServerNode(SaveServerNodeCmdDto saveInfo)
        {
            using (var businessWork = UnitOfWork.Create())
            {
                if (saveInfo == null || saveInfo.ServerNode == null)
                {
                    return Result<ServerNodeDto>.FailedResult("服务节点信息为空");
                }
                var serverNode = saveInfo.ServerNode.MapTo<ServerNode>();
                ServerNodeService.SaveServerNode(serverNode);
                var commitResult = businessWork.Commit();
                Result<ServerNodeDto> result = null;
                if (commitResult.ExecutedSuccess)
                {
                    result = Result<ServerNodeDto>.SuccessResult("保存成功");
                    result.Data = serverNode.MapTo<ServerNodeDto>();
                }
                else
                {
                    result = Result<ServerNodeDto>.FailedResult("保存失败");
                }

                return result;
            }
        }

        #endregion

        #region 获取服务节点

        /// <summary>
        /// 获取服务节点
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public ServerNodeDto GetServerNode(ServerNodeFilterDto filter)
        {
            var serverNode = ServerNodeService.GetServerNode(CreateQueryObject(filter));
            return serverNode.MapTo<ServerNodeDto>();
        }

        #endregion

        #region 获取服务节点列表

        /// <summary>
        /// 获取服务节点列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public List<ServerNodeDto> GetServerNodeList(ServerNodeFilterDto filter)
        {
            var serverNodeList = ServerNodeService.GetServerNodeList(CreateQueryObject(filter));
            return serverNodeList.Select(c => c.MapTo<ServerNodeDto>()).ToList();
        }

        #endregion

        #region 获取服务节点分页

        /// <summary>
        /// 获取服务节点分页
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public IPaging<ServerNodeDto> GetServerNodePaging(ServerNodeFilterDto filter)
        {
            var serverNodePaging = ServerNodeService.GetServerNodePaging(CreateQueryObject(filter));
            return serverNodePaging.ConvertTo<ServerNodeDto>();
        }

        #endregion

        #region 删除服务节点

        /// <summary>
        /// 删除服务节点
        /// </summary>
        /// <param name="deleteInfo">删除信息</param>
        /// <returns>执行结果</returns>
        public Result DeleteServerNode(DeleteServerNodeCmdDto deleteInfo)
        {
            using (var businessWork = UnitOfWork.Create())
            {
                #region 参数判断

                if (deleteInfo == null || deleteInfo.ServerNodeIds.IsNullOrEmpty())
                {
                    return Result.FailedResult("没有指定要删除的服务节点");
                }

                #endregion

                var nowServers =ServerNodeService.GetServerNodeList(QueryFactory.Create<ServerNodeQuery>(c => deleteInfo.ServerNodeIds.Contains(c.Id)));
                //删除逻辑
                ServerNodeService.DeleteServerNode(deleteInfo.ServerNodeIds);
                var commitResult = businessWork.Commit();

                return commitResult.ExecutedSuccess ? Result.SuccessResult("删除成功") : Result.FailedResult("删除失败");
            }
        }

        #endregion

        #region 修改服务节点运行状态

        /// <summary>
        /// 修改服务节点运行状态
        /// </summary>
        /// <param name="stateInfo">状态信息</param>
        /// <returns></returns>
        public Result ModifyRunState(ModifyServerNodeRunStateCmdDto stateInfo)
        {
            using (var businessWork = UnitOfWork.Create())
            {
                if (stateInfo == null || stateInfo.Servers.IsNullOrEmpty())
                {
                    return Result.FailedResult("没有指定要修改的服务信息");
                }
                var servers = stateInfo.Servers.Select(c => c.MapTo<ServerNode>());
                ServerNodeService.ModifyServerNodeRunState(servers);
                var commitResult = businessWork.Commit();

                return commitResult.ExecutedSuccess ? Result.SuccessResult("修改成功") : Result.FailedResult("修改失败");
            }
        }

        #endregion

        #region 根据查询条件生成查询对象

        /// <summary>
        /// 根据查询条件生成查询对象
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public IQuery CreateQueryObject(ServerNodeFilterDto filter)
        {
            if (filter == null)
            {
                return null;
            }
            IQuery query = QueryFactory.Create<ServerNodeQuery>(filter);
            if (!filter.Ids.IsNullOrEmpty())
            {
                query.In<ServerNodeQuery>(c => c.Id, filter.Ids);
            }
            if (!filter.InstanceName.IsNullOrEmpty())
            {
                query.Equal<ServerNodeQuery>(c => c.InstanceName, filter.InstanceName);
            }
            if (!filter.Name.IsNullOrEmpty())
            {
                query.Equal<ServerNodeQuery>(c => c.Name, filter.Name);
            }
            if (filter.State.HasValue)
            {
                query.Equal<ServerNodeQuery>(c => c.State, filter.State.Value);
            }
            if (!filter.Host.IsNullOrEmpty())
            {
                query.Equal<ServerNodeQuery>(c => c.Host, filter.Host);
            }
            if (filter.ThreadCount.HasValue)
            {
                query.Equal<ServerNodeQuery>(c => c.ThreadCount, filter.ThreadCount.Value);
            }
            if (!filter.ThreadPriority.IsNullOrEmpty())
            {
                query.Equal<ServerNodeQuery>(c => c.ThreadPriority, filter.ThreadPriority);
            }
            if (!filter.Description.IsNullOrEmpty())
            {
                query.Equal<ServerNodeQuery>(c => c.Description, filter.Description);
            }
            return query;
        }

        #endregion
    }
}
