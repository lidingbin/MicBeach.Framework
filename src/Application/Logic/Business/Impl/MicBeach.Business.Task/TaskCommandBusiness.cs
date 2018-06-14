using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using MicBeach.Develop.CQuery;
using MicBeach.Query.Task;
using MicBeach.Domain.Task.Model;
using MicBeach.Domain.Task.Repository;
using MicBeach.BusinessInterface.Task;
using MicBeach.Util.IoC;
using MicBeach.CTask;
using MicBeach.CTask.Client;
using MicBeach.CTask.Job;
using MicBeach.CTask.Service;
using MicBeach.CTask.Trigger;

namespace MicBeach.Business.Task
{
    /// <summary>
    /// 任务命令逻辑
    /// </summary>
    public static class TaskCommandBusiness
    {
        static IJobRepository jobRepository = ContainerManager.Container.Resolve<IJobRepository>();
        static IJobServerHostRepository jobServerHostRepository = ContainerManager.Container.Resolve<IJobServerHostRepository>();
        static IServerNodeRepository serverNodeRepository = ContainerManager.Container.Resolve<IServerNodeRepository>();
        static ITriggerRepository triggerRepository = ContainerManager.Container.Resolve<ITriggerRepository>();
        static ITriggerServerRepository triggerServerRepository = ContainerManager.Container.Resolve<ITriggerServerRepository>();

        #region 工作任务

        #region 刷新工作

        /// <summary>
        /// 刷新工作
        /// </summary>
        /// <param name="jobIds">要刷新的工作编号</param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task RefreshJobAsync(params string[] jobIds)
        {
            await System.Threading.Tasks.Task.Run(async () =>
            {
                if (jobIds.IsNullOrEmpty())
                {
                    return;
                }
                List<Job> jobList = jobRepository.GetList(QueryFactory.Create<JobQuery>(c => jobIds.Contains(c.Id)));
                List<Job> remoteContinueJobs = jobList.Where(c => c.Type == JobApplicationType.远程任务 && c.RunType == JobRunType.持续运行).ToList();
                //刷新远程持续任务
                await RefreshRemoteContinueJobAsync(remoteContinueJobs.ToArray()).ConfigureAwait(false);
                //刷新服务调度任务
                List<Job> scheduleJobs = jobList.Where(c => !(c.Type == JobApplicationType.远程任务 && c.RunType == JobRunType.持续运行)).ToList();
                await RefreshScheduleJobAsync(scheduleJobs.ToArray()).ConfigureAwait(false);

            }).ConfigureAwait(false);
        }

        /// <summary>
        /// 刷新远程持续任务
        /// </summary>
        /// <param name="jobs">任务信息</param>
        /// <returns></returns>
        static async System.Threading.Tasks.Task RefreshRemoteContinueJobAsync(params Job[] jobs)
        {
            if (jobs.IsNullOrEmpty())
            {
                return;
            }
            await ModifyRemoteContinueJobRunStateAsync(jobs.ToArray()).ConfigureAwait(false);//刷新远程任务只需要修改任务状态
        }

        /// <summary>
        /// 刷新调度任务
        /// </summary>
        /// <param name="jobs">任务信息</param>
        /// <returns></returns>
        static async System.Threading.Tasks.Task RefreshScheduleJobAsync(params Job[] jobs)
        {
            if (jobs.IsNullOrEmpty())
            {
                return;
            }
            var jobIds = jobs.Select(c => c.Id);
            var jobServerHostQuery = QueryFactory.Create<JobServerHostQuery>(c => jobIds.Contains(c.Job));
            var jobServerHostList = jobServerHostRepository.GetList(jobServerHostQuery);
            var serverIds = jobServerHostList.Select(c => c.Server?.Id).Distinct();
            var serverQuery = QueryFactory.Create<ServerNodeQuery>(c => serverIds.Contains(c.Id));
            var serverList = serverNodeRepository.GetList(serverQuery);
            foreach (var server in serverList)
            {
                var nowJobIds = jobServerHostList.Where(c => c.Server?.Id == server.Id).Select(c => c.Job?.Id);
                if (nowJobIds.IsNullOrEmpty())
                {
                    continue;
                }
                var nowJobs = jobs.Where(c => nowJobIds.Contains(c.Id)).ToList();
                if (nowJobs.IsNullOrEmpty())
                {
                    continue;
                }
                var taskServer = server.MapTo<TaskService>();
                var taskJobs = nowJobs.Select(c => c.MapTo<TaskJob>()).ToList();
                await TaskClientManager.AddScheduleJobCommandAsync(taskServer, taskJobs, TaskCommandType.刷新调度工作任务).ConfigureAwait(false);
            }
        }

        #endregion

        #region 修改任务运行状态

        /// <summary>
        ///  修改任务运行状态
        /// </summary>
        /// <param name="jobIds">任务编号</param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task ModifyJobRunStateAsync(params string[] jobIds)
        {
            await System.Threading.Tasks.Task.Run(async () =>
            {
                if (jobIds.IsNullOrEmpty())
                {
                    return;
                }
                List<Job> jobList = jobRepository.GetList(QueryFactory.Create<JobQuery>(c => jobIds.Contains(c.Id)));
                List<Job> remoteContinueJobs = jobList.Where(c => c.Type == JobApplicationType.远程任务 && c.RunType == JobRunType.持续运行).ToList();
                //修改远程持续任务运行状态
                await ModifyRemoteContinueJobRunStateAsync(remoteContinueJobs.ToArray()).ConfigureAwait(false);
                //修改调度任务运行状态
                List<Job> scheduleJobs = jobList.Where(c => !(c.Type == JobApplicationType.远程任务 && c.RunType == JobRunType.持续运行)).ToList();
                await ModifyScheduleJobRunStateAsync(scheduleJobs.ToArray()).ConfigureAwait(false);

            }).ConfigureAwait(false);
        }

        #region 修改远程持续任务运行状态

        /// <summary>
        /// 修改远程持续任务运行状态
        /// </summary>
        /// <param name="jobs">工作任务</param>
        /// <returns></returns>
        static async System.Threading.Tasks.Task ModifyRemoteContinueJobRunStateAsync(params Job[] jobs)
        {
            if (jobs.IsNullOrEmpty())
            {
                return;
            }
            foreach (var job in jobs)
            {
                if (job.State == JobState.停止)
                {
                    await TaskClientManager.AddRemoteContinueJobCommandAsync(job.MapTo<TaskJob>(), TaskCommandType.关闭远程持续任务).ConfigureAwait(false);
                }
                else
                {
                    await TaskClientManager.AddRemoteContinueJobCommandAsync(job.MapTo<TaskJob>(), TaskCommandType.开启远程持续任务).ConfigureAwait(false);
                }
            }
        }

        #endregion

        #region 修改调度任务运行状态

        /// <summary>
        /// 修改调度任务运行状态
        /// </summary>
        /// <param name="jobs">工作任务</param>
        /// <returns></returns>
        static async System.Threading.Tasks.Task ModifyScheduleJobRunStateAsync(params Job[] jobs)
        {
            if (jobs.IsNullOrEmpty())
            {
                return;
            }
            var jobIds = jobs.Select(c => c.Id);
            var jobServerHostQuery = QueryFactory.Create<JobServerHostQuery>(c => jobIds.Contains(c.Job));
            var jobServerHostList = jobServerHostRepository.GetList(jobServerHostQuery);
            var serverIds = jobServerHostList.Select(c => c.Server?.Id).Distinct();
            var serverQuery = QueryFactory.Create<ServerNodeQuery>(c => serverIds.Contains(c.Id));
            var serverList = serverNodeRepository.GetList(serverQuery);
            foreach (var server in serverList)
            {
                var nowJobIds = jobServerHostList.Where(c => c.Server?.Id == server.Id).Select(c => c.Job?.Id);
                if (nowJobIds.IsNullOrEmpty())
                {
                    continue;
                }
                var nowJobs = jobs.Where(c => nowJobIds.Contains(c.Id));
                if (nowJobs.IsNullOrEmpty())
                {
                    continue;
                }
                var taskServer = server.MapTo<TaskService>();
                var startJobs = nowJobs.Where(c => c.State == JobState.运行中);
                if (!startJobs.IsNullOrEmpty())
                {
                    await TaskClientManager.AddScheduleJobCommandAsync(taskServer, startJobs.Select(c => c.MapTo<TaskJob>()), TaskCommandType.开始调度工作任务).ConfigureAwait(false);
                }
                var stopJobs = nowJobs.Where(c => c.State == JobState.停止);
                if (!stopJobs.IsNullOrEmpty())
                {
                    await TaskClientManager.AddScheduleJobCommandAsync(taskServer, stopJobs.Select(c => c.MapTo<TaskJob>()), TaskCommandType.停止调度工作任务).ConfigureAwait(false);
                }
            }
        }

        #endregion

        #endregion

        #region 删除任务

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="jobIds">任务编号</param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task DeleteJobAsync(params string[] jobIds)
        {
            await System.Threading.Tasks.Task.Run(async () =>
            {
                if (jobIds.IsNullOrEmpty())
                {
                    return;
                }
                List<Job> jobList = jobRepository.GetList(QueryFactory.Create<JobQuery>(c => jobIds.Contains(c.Id)));
                jobList.ForEach(c => c.State = JobState.停止);
                List<Job> remoteContinueJobs = jobList.Where(c => c.Type == JobApplicationType.远程任务 && c.RunType == JobRunType.持续运行).ToList();
                //停止远程任务
                await ModifyRemoteContinueJobRunStateAsync(remoteContinueJobs.ToArray()).ConfigureAwait(false);
                //刷新服务调度任务
                List<Job> scheduleJobs = jobList.Where(c => !(c.Type == JobApplicationType.远程任务 && c.RunType == JobRunType.持续运行)).ToList();
                await DeleteScheduleJobAsync(scheduleJobs.ToArray()).ConfigureAwait(false);

            }).ConfigureAwait(false);
        }

        /// <summary>
        /// 删除调度任务
        /// </summary>
        /// <param name="jobs">任务信息</param>
        /// <returns></returns>
        static async System.Threading.Tasks.Task DeleteScheduleJobAsync(params Job[] jobs)
        {
            if (jobs.IsNullOrEmpty())
            {
                return;
            }
            var jobIds = jobs.Select(c => c.Id);
            var jobServerHostQuery = QueryFactory.Create<JobServerHostQuery>(c => jobIds.Contains(c.Job));
            var jobServerHostList = jobServerHostRepository.GetList(jobServerHostQuery);
            var serverIds = jobServerHostList.Select(c => c.Server?.Id).Distinct();
            var serverQuery = QueryFactory.Create<ServerNodeQuery>(c => serverIds.Contains(c.Id));
            var serverList = serverNodeRepository.GetList(serverQuery);
            foreach (var server in serverList)
            {
                var nowJobIds = jobServerHostList.Where(c => c.Server?.Id == server.Id).Select(c => c.Job?.Id);
                if (nowJobIds.IsNullOrEmpty())
                {
                    continue;
                }
                var nowJobs = jobs.Where(c => nowJobIds.Contains(c.Id));
                if (nowJobs.IsNullOrEmpty())
                {
                    continue;
                }
                var taskServer = server.MapTo<TaskService>();
                await TaskClientManager.AddScheduleJobCommandAsync(taskServer, nowJobs.Select(c => c.MapTo<TaskJob>()), TaskCommandType.移除调度工作任务).ConfigureAwait(false);
            }
        }

        #endregion

        #endregion

        #region 工作&服务承载

        /// <summary>
        /// 添加服务任务承载
        /// </summary>
        /// <param name="serverJobs">服务任务承载信息（键：服务编号，值：要承载的任务信息）</param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task AddServiceJobHostAsync(Dictionary<string, List<string>> serverJobs)
        {
            await System.Threading.Tasks.Task.Run(async () =>
            {
                #region 刷新承载任务

                if (serverJobs == null || serverJobs.Count <= 0)
                {
                    return;
                }
                IEnumerable<string> serverIds = serverJobs.Keys;
                var serverQuery = QueryFactory.Create<ServerNodeQuery>(c => serverIds.Contains(c.Id));
                var servers = serverNodeRepository.GetList(serverQuery);
                if (servers.IsNullOrEmpty())
                {
                    return;
                }
                List<string> jobIds = new List<string>();
                foreach (var serverJob in serverJobs)
                {
                    jobIds.AddRange(serverJob.Value);
                }
                if (jobIds.IsNullOrEmpty())
                {
                    return;
                }
                jobIds = jobIds.Distinct().ToList();
                var jobQuery = QueryFactory.Create<JobQuery>(c => jobIds.Contains(c.Id));
                var jobs = jobRepository.GetList(jobQuery);
                if (jobs.IsNullOrEmpty())
                {
                    return;
                }
                foreach (var serverJob in serverJobs)
                {
                    var nowServer = servers.FirstOrDefault(c => c.Id == serverJob.Key);
                    if (nowServer == null)
                    {
                        continue;
                    }
                    var nowJobs = jobs.Where(c => serverJob.Value.Contains(c.Id)).ToList();
                    if (nowJobs.IsNullOrEmpty())
                    {
                        continue;
                    }
                    await TaskClientManager.AddScheduleJobCommandAsync(nowServer.MapTo<TaskService>(), nowJobs.Select(c => c.MapTo<TaskJob>()), TaskCommandType.刷新调度工作任务).ConfigureAwait(false);
                }

                #endregion

                #region 任务执行计划

                var triggerQuery = QueryFactory.Create<TriggerQuery>(c => jobIds.Contains(c.Job));
                triggerQuery.AddQueryFields<TriggerQuery>(c => c.Id);
                var triggers = triggerRepository.GetList(triggerQuery);
                if (triggers.IsNullOrEmpty())
                {
                    return;
                }
                await RefreshTriggerAsync(triggers.Select(c => c.Id), serverIds).ConfigureAwait(false);

                #endregion
            });
        }

        /// <summary>
        /// 移除服务任务承载
        /// </summary>
        /// <param name="serverJobs">服务任务承载信息（键：服务编号，值：要承载的任务信息）</param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task RemoveServiceJobHostAsync(Dictionary<string, List<string>> serverJobs)
        {
            await System.Threading.Tasks.Task.Run(async () =>
            {
                if (serverJobs == null || serverJobs.Count <= 0)
                {
                    return;
                }
                IEnumerable<string> serverIds = serverJobs.Keys;
                var serverQuery = QueryFactory.Create<ServerNodeQuery>(c => serverIds.Contains(c.Id));
                var servers = serverNodeRepository.GetList(serverQuery);
                if (servers.IsNullOrEmpty())
                {
                    return;
                }
                List<string> jobIds = new List<string>();
                foreach (var serverJob in serverJobs)
                {
                    jobIds.AddRange(serverJob.Value);
                }
                if (jobIds.IsNullOrEmpty())
                {
                    return;
                }
                jobIds = jobIds.Distinct().ToList();
                var jobQuery = QueryFactory.Create<JobQuery>(c => jobIds.Contains(c.Id));
                var jobs = jobRepository.GetList(jobQuery);
                if (jobs.IsNullOrEmpty())
                {
                    return;
                }
                foreach (var serverJob in serverJobs)
                {
                    var nowServer = servers.FirstOrDefault(c => c.Id == serverJob.Key);
                    if (nowServer == null)
                    {
                        continue;
                    }
                    var nowJobs = jobs.Where(c => serverJob.Value.Contains(c.Id)).ToList();
                    if (nowJobs.IsNullOrEmpty())
                    {
                        continue;
                    }
                    await TaskClientManager.AddScheduleJobCommandAsync(nowServer.MapTo<TaskService>(), nowJobs.Select(c => c.MapTo<TaskJob>()), TaskCommandType.移除调度工作任务).ConfigureAwait(false);
                }
            });
        }

        /// <summary>
        /// 修改服务承载运行状态
        /// </summary>
        /// <param name="serverJobs">服务任务承载信息（键：服务编号，值：要承载的任务信息）</param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task ModifyServiceJobHostRunStateAsync(Dictionary<string, List<string>> serverJobs)
        {
            await System.Threading.Tasks.Task.Run(async () =>
            {
                if (serverJobs == null || serverJobs.Count <= 0)
                {
                    return;
                }
                //服务信息
                IEnumerable<string> serverIds = serverJobs.Keys;
                var serverQuery = QueryFactory.Create<ServerNodeQuery>(c => serverIds.Contains(c.Id));
                var servers = serverNodeRepository.GetList(serverQuery);
                if (servers.IsNullOrEmpty())
                {
                    return;
                }
                //任务信息
                List<string> jobIds = new List<string>();
                var serverHostQuery = QueryFactory.Create<JobServerHostQuery>();
                foreach (var serverJob in serverJobs)
                {
                    serverHostQuery.Or<JobServerHostQuery>(c => c.Server == serverJob.Key && serverJob.Value.Contains(c.Job));
                    jobIds.AddRange(serverJob.Value);
                }
                if (jobIds.IsNullOrEmpty())
                {
                    return;
                }
                jobIds = jobIds.Distinct().ToList();
                var jobQuery = QueryFactory.Create<JobQuery>(c => jobIds.Contains(c.Id));
                var jobs = jobRepository.GetList(jobQuery);
                if (jobs.IsNullOrEmpty())
                {
                    return;
                }
                //承载信息
                var serverJobHostList = jobServerHostRepository.GetList(serverHostQuery);
                if (serverJobHostList.IsNullOrEmpty())
                {
                    return;
                }
                foreach (var serverJob in serverJobs)
                {
                    var nowServer = servers.FirstOrDefault(c => c.Id == serverJob.Key);
                    if (nowServer == null)
                    {
                        continue;
                    }
                    var nowJobs = jobs.Where(c => serverJob.Value.Contains(c.Id)).ToList();
                    if (nowJobs.IsNullOrEmpty())
                    {
                        continue;
                    }
                    var nowHostList = serverJobHostList.Where(c => c.Server?.Id == serverJob.Key && serverJob.Value.Contains(c.Job?.Id)).ToList();
                    var taskServer = nowServer.MapTo<TaskService>();
                    //开启的任务
                    var startJobs = nowJobs.Where(c => c.State == JobState.运行中 && nowHostList.Where(ch => ch.RunState == JobServerState.启用).Select(cs => cs.Job?.Id).Contains(c.Id));
                    if (!startJobs.IsNullOrEmpty())
                    {
                        await TaskClientManager.AddScheduleJobCommandAsync(taskServer, startJobs.Select(c => c.MapTo<TaskJob>()), TaskCommandType.开始调度工作任务).ConfigureAwait(false);
                    }
                    //停止的任务
                    var stopJobs = nowJobs.Where(c => c.State == JobState.停止 || nowHostList.Where(ch => ch.RunState == JobServerState.停用).Select(cs => cs.Job?.Id).Contains(c.Id));
                    if (!stopJobs.IsNullOrEmpty())
                    {
                        await TaskClientManager.AddScheduleJobCommandAsync(taskServer, stopJobs.Select(c => c.MapTo<TaskJob>()), TaskCommandType.停止调度工作任务).ConfigureAwait(false);
                    }
                }
            });
        }

        #endregion

        #region 执行计划

        /// <summary>
        /// 刷新执行计划
        /// </summary>
        /// <param name="triggerIds">执行计划编号</param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task RefreshTriggerAsync(IEnumerable<string> triggerIds, IEnumerable<string> services = null)
        {
            await System.Threading.Tasks.Task.Run(async () =>
            {
                try
                {
                    if (triggerIds.IsNullOrEmpty())
                    {
                        return;
                    }
                    //计划信息
                    var triggerQuery = QueryFactory.Create<TriggerQuery>(c => triggerIds.Contains(c.Id));
                    var triggerList = triggerRepository.GetList(triggerQuery);
                    if (triggerList.IsNullOrEmpty())
                    {
                        return;
                    }
                    //任务
                    var jobIds = triggerList.Select(c => c.Job?.Id).Distinct();
                    var jobList = jobRepository.GetList(QueryFactory.Create<JobQuery>(c => jobIds.Contains(c.Id)));
                    if (jobList.IsNullOrEmpty())
                    {
                        return;
                    }
                    //任务承载服务
                    var jobServerHostQuery = QueryFactory.Create<JobServerHostQuery>(c => jobIds.Contains(c.Job));
                    var jobServerHostList = jobServerHostRepository.GetList(jobServerHostQuery);
                    var serverIds = jobServerHostList.Select(c => c.Server?.Id).Distinct();
                    var serverQuery = QueryFactory.Create<ServerNodeQuery>(c => serverIds.Contains(c.Id));
                    if (services != null)//刷新指定的服务器
                    {
                        serverQuery.And<ServerNodeQuery>(c => services.Contains(c.Id));
                    }
                    var serverList = serverNodeRepository.GetList(serverQuery);
                    //计划应用的服务
                    var triggerServerQuery = QueryFactory.Create<TriggerServerQuery>(c => triggerIds.Contains(c.Trigger));
                    var triggerServerList = triggerServerRepository.GetList(triggerServerQuery);

                    //应用到所有的服务的计划
                    var applyToAllTriggerList = triggerList.Where(c => c.ApplyTo == TaskTriggerApplyTo.所有).ToList();
                    foreach (var server in serverList)
                    {
                        var serverJobList = jobList.Where(c => jobServerHostList.Where(ct => ct.Server?.Id == server.Id).Select(cs => cs.Job?.Id).Contains(c.Id)).ToList();
                        if (serverJobList.IsNullOrEmpty())
                        {
                            continue;
                        }
                        var nowServerJobIds = serverJobList.Select(c => c.Id);
                        var nowTriggers = triggerList.Where(c => nowServerJobIds.Contains(c.Job?.Id) && (c.ApplyTo == TaskTriggerApplyTo.所有 || triggerServerList.Exists(cs => cs.Server?.Id == server.Id && cs.Trigger?.Id == c.Id)));
                        var taskTriggers = nowTriggers.Select(c =>
                        {
                            var taskTrigger = c.MapTo<TaskTrigger>();
                            taskTrigger.Job = serverJobList.FirstOrDefault(j => j.Id == c.Job?.Id).MapTo<TaskJob>();
                            return taskTrigger;
                        });
                        await TaskClientManager.AddTriggerCommandAsync(server.MapTo<TaskService>(), taskTriggers, TaskCommandType.刷新触发器).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                }

            }).ConfigureAwait(false);
        }

        /// <summary>
        /// 移除执行计划
        /// </summary>
        /// <param name="triggerIds">计划编号</param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task RemoveTriggerAsync(params string[] triggerIds)
        {
            await System.Threading.Tasks.Task.Run(async () =>
            {
                try
                {
                    if (triggerIds.IsNullOrEmpty())
                    {
                        return;
                    }
                    //计划信息
                    var triggers = triggerRepository.GetList(QueryFactory.Create<TriggerQuery>(c => triggerIds.Contains(c.Id)));
                    if (triggers.IsNullOrEmpty())
                    {
                        return;
                    }
                    //服务信息
                    var jobIds = triggers.Select(c => c.Job?.Id);
                    var jobServerHostQuery = QueryFactory.Create<JobServerHostQuery>(c => jobIds.Contains(c.Job));
                    var jobServerHostList = jobServerHostRepository.GetList(jobServerHostQuery);
                    var serverIds = jobServerHostList.Select(c => c.Server?.Id).Distinct().ToList();
                    var serverQuery = QueryFactory.Create<ServerNodeQuery>(c => serverIds.Contains(c.Id));
                    var servers = serverNodeRepository.GetList(serverQuery);
                    if (servers.IsNullOrEmpty())
                    {
                        return;
                    }
                    //计划&服务承载
                    var triggerServerList = triggerServerRepository.GetList(QueryFactory.Create<TriggerServerQuery>(c => triggerIds.Contains(c.Trigger)));
                    foreach (var server in servers)
                    {
                        var serverJobIds = jobServerHostList.Select(c => c.Job?.Id);
                        var jobTriggers = triggers.Where(c => serverJobIds.Contains(c.Job?.Id) && (c.ApplyTo == TaskTriggerApplyTo.所有 || triggerServerList.Exists(cs => cs.Trigger?.Id == c.Id && cs.Server?.Id == server.Id))).ToList();
                        if (jobTriggers.IsNullOrEmpty())
                        {
                            continue;
                        }
                        await TaskClientManager.AddTriggerCommandAsync(server.MapTo<TaskService>(), jobTriggers.Select(c => c.MapTo<TaskTrigger>()), TaskCommandType.移除触发器).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {

                }

            }).ConfigureAwait(false);
        }

        /// <summary>
        /// 修改执行计划状态
        /// </summary>
        /// <param name="triggerIds">计划编号</param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task ModifyTriggerStateAsync(params string[] triggerIds)
        {
            await System.Threading.Tasks.Task.Run(async () =>
            {
                try
                {
                    if (triggerIds.IsNullOrEmpty())
                    {
                        return;
                    }
                    //计划信息
                    var triggers = triggerRepository.GetList(QueryFactory.Create<TriggerQuery>(c => triggerIds.Contains(c.Id)));
                    if (triggers.IsNullOrEmpty())
                    {
                        return;
                    }
                    //服务信息
                    var jobIds = triggers.Select(c => c.Job?.Id);
                    var jobServerHostQuery = QueryFactory.Create<JobServerHostQuery>(c => jobIds.Contains(c.Job));
                    var jobServerHostList = jobServerHostRepository.GetList(jobServerHostQuery);
                    var serverIds = jobServerHostList.Select(c => c.Server?.Id).Distinct().ToList();
                    var serverQuery = QueryFactory.Create<ServerNodeQuery>(c => serverIds.Contains(c.Id));
                    var servers = serverNodeRepository.GetList(serverQuery);
                    if (servers.IsNullOrEmpty())
                    {
                        return;
                    }
                    //计划&服务承载
                    var triggerServerList = triggerServerRepository.GetList(QueryFactory.Create<TriggerServerQuery>(c => triggerIds.Contains(c.Trigger)));
                    foreach (var server in servers)
                    {
                        var serverJobIds = jobServerHostList.Select(c => c.Job?.Id);
                        var jobTriggers = triggers.Where(c => serverJobIds.Contains(c.Job?.Id) && (c.ApplyTo == TaskTriggerApplyTo.所有 || triggerServerList.Exists(cs => cs.Trigger?.Id == c.Id && cs.Server?.Id == server.Id))).ToList();
                        if (jobTriggers.IsNullOrEmpty())
                        {
                            continue;
                        }
                        var taskServer = server.MapTo<TaskService>();
                        var startTriggers = jobTriggers.Where(c => c.State == TaskTriggerState.运行 && (!triggerServerList.Exists(t => t.Server.Id == server.Id && t.Trigger.Id == c.Id && t.RunState == TaskTriggerServerRunState.停止)));
                        if (!startTriggers.IsNullOrEmpty())
                        {
                            await TaskClientManager.AddTriggerCommandAsync(taskServer, startTriggers.Select(c => c.MapTo<TaskTrigger>()), TaskCommandType.开始触发器).ConfigureAwait(false);
                        }
                        var stopTriggers = jobTriggers.Where(c => c.State == TaskTriggerState.停止 || triggerServerList.Exists(t => t.Server.Id == server.Id && t.Trigger.Id == c.Id && t.RunState == TaskTriggerServerRunState.停止));
                        if (!stopTriggers.IsNullOrEmpty())
                        {
                            await TaskClientManager.AddTriggerCommandAsync(taskServer, stopTriggers.Select(c => c.MapTo<TaskTrigger>()), TaskCommandType.停止触发器).ConfigureAwait(false);
                        }
                    }
                }
                catch (Exception ex)
                {

                }

            }).ConfigureAwait(false);
        }

        #endregion

        #region 执行计划&服务承载

        /// <summary>
        /// 修改服务执行计划运行状态
        /// </summary>
        /// <param name="serviceTriggers">服务计划信息</param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task ModifyTriggerServiceRunStateAsync(Dictionary<string, List<string>> serviceTriggers)
        {
            await System.Threading.Tasks.Task.Run(async () =>
            {
                if (serviceTriggers == null || serviceTriggers.Count <= 0)
                {
                    return;
                }
                //服务信息
                var serverIds = serviceTriggers.Keys;
                var serverList = serverNodeRepository.GetList(QueryFactory.Create<ServerNodeQuery>(c => serverIds.Contains(c.Id)));
                if (serverList.IsNullOrEmpty())
                {
                    return;
                }
                //计划信息
                var triggerIds = new List<string>();
                foreach (var serviceItem in serviceTriggers)
                {
                    triggerIds.AddRange(serviceItem.Value);
                }
                triggerIds = triggerIds.Distinct().ToList();
                var triggerList = triggerRepository.GetList(QueryFactory.Create<TriggerQuery>(c => triggerIds.Contains(c.Id)));
                if (triggerList.IsNullOrEmpty())
                {
                    return;
                }
                //任务
                var jobIds = triggerList.Select(c => c.Job?.Id).Distinct();
                var jobList = jobRepository.GetList(QueryFactory.Create<JobQuery>(c => jobIds.Contains(c.Id)));
                if (jobList.IsNullOrEmpty())
                {
                    return;
                }
                //计划&服务承载
                var triggerServerList = triggerServerRepository.GetList(QueryFactory.Create<TriggerServerQuery>(c => triggerIds.Contains(c.Trigger)));
                foreach (var serverItem in serviceTriggers)
                {
                    var nowServer = serverList.FirstOrDefault(c => c.Id == serverItem.Key);
                    if (nowServer == null)
                    {
                        continue;
                    }
                    var nowTriggers = triggerList.Where(c => serverItem.Value.Contains(c.Id)).ToList();
                    if (nowTriggers.IsNullOrEmpty())
                    {
                        continue;
                    }
                    var taskServer = nowServer.MapTo<TaskService>();
                    var startTriggers = nowTriggers.Where(c => c.State == TaskTriggerState.运行 && (!triggerServerList.Exists(t => t.Server.Id == nowServer.Id && t.Trigger.Id == c.Id && t.RunState == TaskTriggerServerRunState.停止)));
                    if (!startTriggers.IsNullOrEmpty())
                    {
                        await TaskClientManager.AddTriggerCommandAsync(taskServer, startTriggers.Select(c =>
                        {
                            var taskTrigger = c.MapTo<TaskTrigger>();
                            taskTrigger.Job = jobList.FirstOrDefault(j => j.Id == c.Job?.Id).MapTo<TaskJob>();
                            return taskTrigger;
                        }
                        ), TaskCommandType.开始触发器).ConfigureAwait(false);
                    }
                    var stopTriggers = nowTriggers.Where(c => c.State == TaskTriggerState.停止 || triggerServerList.Exists(t => t.Server.Id == nowServer.Id && t.Trigger.Id == c.Id && t.RunState == TaskTriggerServerRunState.停止));
                    if (!stopTriggers.IsNullOrEmpty())
                    {
                        await TaskClientManager.AddTriggerCommandAsync(taskServer, stopTriggers.Select(c =>
                        {
                            var taskTrigger = c.MapTo<TaskTrigger>();
                            taskTrigger.Job = jobList.FirstOrDefault(j => j.Id == c.Job?.Id).MapTo<TaskJob>();
                            return taskTrigger;
                        }), TaskCommandType.停止触发器).ConfigureAwait(false);
                    }
                }
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// 移除服务计划承载信息
        /// </summary>
        /// <param name="serviceTriggers">服务计划计划信息</param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task RemoveTriggerServiceHostAsync(Dictionary<string, List<string>> serviceTriggers)
        {
            await System.Threading.Tasks.Task.Run(async () =>
            {
                if (serviceTriggers == null || serviceTriggers.Count <= 0)
                {
                    return;
                }
                if (serviceTriggers == null || serviceTriggers.Count <= 0)
                {
                    return;
                }
                //服务信息
                var serverIds = serviceTriggers.Keys;
                var serverList = serverNodeRepository.GetList(QueryFactory.Create<ServerNodeQuery>(c => serverIds.Contains(c.Id)));
                if (serverList.IsNullOrEmpty())
                {
                    return;
                }
                //计划信息
                var triggerIds = new List<string>();
                foreach (var serviceItem in serviceTriggers)
                {
                    triggerIds.AddRange(serviceItem.Value);
                }
                triggerIds = triggerIds.Distinct().ToList();
                var triggerList = triggerRepository.GetList(QueryFactory.Create<TriggerQuery>(c => triggerIds.Contains(c.Id)));
                if (triggerList.IsNullOrEmpty())
                {
                    return;
                }
                //任务
                var jobIds = triggerList.Select(c => c.Job?.Id).Distinct();
                var jobList = jobRepository.GetList(QueryFactory.Create<JobQuery>(c => jobIds.Contains(c.Id)));
                if (jobList.IsNullOrEmpty())
                {
                    return;
                }
                foreach (var serverItem in serviceTriggers)
                {
                    var nowServer = serverList.FirstOrDefault(c => c.Id == serverItem.Key);
                    if (nowServer == null)
                    {
                        continue;
                    }
                    var nowTriggers = triggerList.Where(c => serverItem.Value.Contains(c.Id));
                    if (nowTriggers.IsNullOrEmpty())
                    {
                        continue;
                    }
                    var taskServer = nowServer.MapTo<TaskService>();
                    await TaskClientManager.AddTriggerCommandAsync(taskServer, nowTriggers.Select(c =>
                    {
                        var taskTrigger = c.MapTo<TaskTrigger>();
                        taskTrigger.Job = jobList.FirstOrDefault(j => j.Id == c.Job?.Id).MapTo<TaskJob>();
                        return taskTrigger;
                    }), TaskCommandType.移除触发器).ConfigureAwait(false);
                }
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// 添加计划&服务承载信息
        /// </summary>
        /// <param name="serviceTriggers">服务计划计划信息</param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task AddTriggerServiceHostAsync(Dictionary<string, List<string>> serviceTriggers)
        {
            await System.Threading.Tasks.Task.Run(async () =>
            {
                if (serviceTriggers == null || serviceTriggers.Count <= 0)
                {
                    return;
                }
                if (serviceTriggers == null || serviceTriggers.Count <= 0)
                {
                    return;
                }
                //服务信息
                var serverIds = serviceTriggers.Keys;
                var serverList = serverNodeRepository.GetList(QueryFactory.Create<ServerNodeQuery>(c => serverIds.Contains(c.Id)));
                if (serverList.IsNullOrEmpty())
                {
                    return;
                }
                //计划信息
                var triggerIds = new List<string>();
                foreach (var serviceItem in serviceTriggers)
                {
                    triggerIds.AddRange(serviceItem.Value);
                }
                triggerIds = triggerIds.Distinct().ToList();
                var triggerList = triggerRepository.GetList(QueryFactory.Create<TriggerQuery>(c => triggerIds.Contains(c.Id)));
                if (triggerList.IsNullOrEmpty())
                {
                    return;
                }
                //任务
                var jobIds = triggerList.Select(c => c.Job?.Id).Distinct();
                var jobList = jobRepository.GetList(QueryFactory.Create<JobQuery>(c => jobIds.Contains(c.Id)));
                if (jobList.IsNullOrEmpty())
                {
                    return;
                }
                foreach (var serverItem in serviceTriggers)
                {
                    var nowServer = serverList.FirstOrDefault(c => c.Id == serverItem.Key);
                    if (nowServer == null)
                    {
                        continue;
                    }
                    var nowTriggers = triggerList.Where(c => serverItem.Value.Contains(c.Id));
                    if (nowTriggers.IsNullOrEmpty())
                    {
                        continue;
                    }
                    var taskServer = nowServer.MapTo<TaskService>();
                    await TaskClientManager.AddTriggerCommandAsync(taskServer, nowTriggers.Select(c =>
                    {
                        var taskTrigger = c.MapTo<TaskTrigger>();
                        taskTrigger.Job = jobList.FirstOrDefault(j => j.Id == c.Job?.Id).MapTo<TaskJob>();
                        return taskTrigger;
                    }), TaskCommandType.刷新触发器).ConfigureAwait(false);
                }
            }).ConfigureAwait(false);
        }

        #endregion

        #region 服务节点

        /// <summary>
        /// 刷新服务节点
        /// </summary>
        /// <param name="serverIds">服务编号</param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task RefreshServiceAsync(IEnumerable<string> serverIds)
        {
            await System.Threading.Tasks.Task.Run(async () =>
            {
                if (serverIds.IsNullOrEmpty())
                {
                    return;
                }
                var servers = serverNodeRepository.GetList(QueryFactory.Create<ServerNodeQuery>(c => serverIds.Contains(c.Id)));
                if (servers.IsNullOrEmpty())
                {
                    return;
                }
                foreach (var server in servers)
                {
                    await TaskClientManager.AddServiceCommandAsync(server.MapTo<TaskService>(), server.State == ServerState.运行 ? TaskCommandType.启动服务 : TaskCommandType.停止服务).ConfigureAwait(false);
                }
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// 移除服务节点
        /// </summary>
        /// <param name="servers">要移除的服务信息</param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task RemoveServiceAsync(IEnumerable<ServerNode> servers)
        {
            await System.Threading.Tasks.Task.Run(async ()=> 
            {
                if (servers.IsNullOrEmpty())
                {
                    return;
                }
                foreach (var server in servers)
                {
                    await TaskClientManager.AddServiceCommandAsync(server.MapTo<TaskService>(),TaskCommandType.停止服务).ConfigureAwait(false);
                }
            }).ConfigureAwait(false);
        }

        #endregion
    }
}
