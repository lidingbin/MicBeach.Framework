using MicBeach.Domain.Task.Model;
using MicBeach.Domain.Task.Repository;
using MicBeach.Util.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using MicBeach.Develop.CQuery;
using MicBeach.Util.Paging;

namespace MicBeach.Domain.Task.Service
{
    /// <summary>
    /// 任务执行日志服务
    /// </summary>
    public static class ExecuteLogService
    {
        static IExecuteLogRepository executeLogRepository = ContainerManager.Container.Resolve<IExecuteLogRepository>();

        #region 保存执行日志

        /// <summary>
        /// 保存执行日志
        /// </summary>
        /// <param name="logs">日志信息</param>
        public static void SaveExecuteLog(IEnumerable<ExecuteLog> logs)
        {
            if (logs.IsNullOrEmpty())
            {
                return;
            }
            executeLogRepository.Save(logs.ToArray());
        }

        #endregion

        #region 获取任务执行日志

        /// <summary>
        /// 获取任务执行日志
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public static ExecuteLog GetExecuteLog(IQuery query)
        {
            return executeLogRepository.Get(query);
        }

        #endregion

        #region 获取任务执行日志列表

        /// <summary>
        /// 获取任务执行日志列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public static List<ExecuteLog> GetExecuteLogList(IQuery query)
        {
            var logList = executeLogRepository.GetList(query);
            logList = LoadOtherObjectData(logList, query);//加载其它附加属性数据
            return logList;
        }

        #endregion

        #region 获取任务执行日志分页

        /// <summary>
        /// 获取任务执行日志分页
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public static IPaging<ExecuteLog> GetExecuteLogPaging(IQuery query)
        {
            var logPager = executeLogRepository.GetPaging(query);
            var logList = LoadOtherObjectData(logPager, query);
            return new Paging<ExecuteLog>(logPager.Page, logPager.PageSize, logPager.TotalCount, logList);
        }

        #endregion

        #region 数据列表加载额外的对象数据

        /// <summary>
        /// 数据列表加载额外的对象数据
        /// </summary>
        /// <param name="datas">数据信息</param>
        /// <param name="query">查询对象</param>
        static List<ExecuteLog> LoadOtherObjectData(IEnumerable<ExecuteLog> datas, IQuery query)
        {
            #region 参数判断

            if (datas.IsNullOrEmpty())
            {
                return new List<ExecuteLog>(0);
            }
            if (query == null || query.LoadPropertys == null || query.LoadPropertys.Count <= 0)
            {
                return datas.ToList();
            }

            #endregion

            #region 服务信息

            List<ServerNode> serverList = null;
            if (query.AllowLoad<ExecuteLog>(c => c.Server))
            {
                List<string> serverIds = datas.Select(c => c.Server?.Id).Distinct().ToList();
                serverList = ServerNodeService.GetServerNodeList(serverIds);
            }

            #endregion

            #region 工作信息

            List<Job> jobList = null;
            if (query.AllowLoad<ExecuteLog>(c => c.Job))
            {
                List<string> jobIds = datas.Select(c => c.Job?.Id).Distinct().ToList();
                jobList = JobService.GetJobList(jobIds);
            }

            #endregion

            #region 执行计划

            List<Trigger> triggerList = null;
            if (query.AllowLoad<ExecuteLog>(c => c.Trigger))
            {
                List<string> triggerIds = datas.Select(c => c.Trigger?.Id).Distinct().ToList();
                triggerList = TriggerService.GetTriggerList(triggerIds);
            }

            #endregion

            foreach (var log in datas)
            {
                if (!serverList.IsNullOrEmpty())
                {
                    log.SetServer(serverList.FirstOrDefault(c => c.Id == log.Server?.Id));
                }
                if (!jobList.IsNullOrEmpty())
                {
                    log.SetJob(jobList.FirstOrDefault(c => c.Id == log.Job?.Id));
                }
                if (!triggerList.IsNullOrEmpty())
                {
                    log.SetTrigger(triggerList.FirstOrDefault(c => c.Id == log.Trigger?.Id));
                }
            }
            return datas.ToList();
        }

        #endregion
    }
}
