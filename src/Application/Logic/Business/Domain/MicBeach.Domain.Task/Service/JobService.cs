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
using MicBeach.Query.Task;
using MicBeach.Util.Paging;
using MicBeach.Util;
using MicBeach.Util.CustomerException;

namespace MicBeach.Domain.Task.Service
{
    /// <summary>
    /// 任务服务
    /// </summary>
    public static class JobService
    {
        static IJobRepository jobRepository = ContainerManager.Resolve<IJobRepository>();

        #region 删除工作任务

        /// <summary>
        /// 删除工作任务
        /// </summary>
        /// <param name="jobs">工作信息</param>
        public static void DeleteJob(IEnumerable<Job> jobs)
        {
            if (jobs.IsNullOrEmpty())
            {
                return;
            }
            jobRepository.Remove(jobs.ToArray());
        }

        /// <summary>
        /// 删除工作任务
        /// </summary>
        /// <param name="jobIds">任务编号</param>
        public static void DeleteJob(IEnumerable<string> jobIds)
        {
            if (jobIds.IsNullOrEmpty())
            {
                return;
            }
            DeleteJob(jobIds.Select(c => Job.CreateJob(c)));
        }

        #endregion

        #region 修改工作任务运行状态

        /// <summary>
        /// 修改工作任务运行状态
        /// </summary>
        /// <param name="jobs">工作信息</param>
        public static void ModifyJobRunState(IEnumerable<Job> jobs)
        {
            if (jobs.IsNullOrEmpty())
            {
                return;
            }
            var jobIds = jobs.Select(c => c.Id).ToList();
            var jobQuery = QueryFactory.Create<JobQuery>(c => jobIds.Contains(c.Id));
            var nowJobList = jobRepository.GetList(jobQuery);
            if (nowJobList.IsNullOrEmpty())
            {
                return;
            }
            foreach (var job in nowJobList)
            {
                var newJob = jobs.FirstOrDefault(c => c.Id == job.Id);
                if (newJob == null)
                {
                    continue;
                }
                job.State = newJob.State;
                job.Save();
            }
        }

        #endregion

        #region 获取工作任务

        /// <summary>
        /// 获取工作任务
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public static Job GetJob(IQuery query)
        {
            return jobRepository.Get(query);
        }

        /// <summary>
        /// 根据工作编号获取工作任务
        /// </summary>
        /// <param name="id">工作编号</param>
        /// <returns>工作任务信息</returns>
        public static Job GetJob(string id)
        {
            if (id.IsNullOrEmpty())
            {
                return null;
            }
            IQuery jobQuery = QueryFactory.Create<JobQuery>(c => c.Id == id);
            return GetJob(jobQuery);
        }

        #endregion

        #region 获取工作任务列表

        /// <summary>
        /// 获取工作任务列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public static List<Job> GetJobList(IQuery query)
        {
            var jobList = jobRepository.GetList(query);
            jobList = LoadOtherObjectData(jobList, query);
            return jobList;
        }

        /// <summary>
        /// 根据工作编号获取工作任务
        /// </summary>
        /// <param name="ids">工作编号</param>
        /// <returns>工作任务信息</returns>
        public static List<Job> GetJobList(IEnumerable<string> ids)
        {
            if (ids.IsNullOrEmpty())
            {
                return new List<Job>(0);
            }
            IQuery jobQuery = QueryFactory.Create<JobQuery>(c => ids.Contains(c.Id));
            return GetJobList(jobQuery);
        }

        #endregion

        #region 获取工作任务分页

        /// <summary>
        /// 获取工作任务分页
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public static IPaging<Job> GetJobPaging(IQuery query)
        {
            var jobPaging = jobRepository.GetPaging(query);
            var jobList = LoadOtherObjectData(jobPaging, query);
            return new Paging<Job>(jobPaging.Page, jobPaging.PageSize, jobPaging.TotalCount, jobList);
        }

        #endregion

        #region 保存工作任务

        /// <summary>
        /// 保存工作任务
        /// </summary>
        /// <param name="job">任务信息</param>
        /// <returns>执行结果</returns>
        public static void SaveJob(Job job)
        {
            if (job == null)
            {
                return;
            }
            if (job.Id.IsNullOrEmpty())
            {
                AddJob(job);
            }
            else
            {
                UpdateJob(job);
            }
        }

        /// <summary>
        /// 添加工作任务
        /// </summary>
        /// <param name="job">任务信息</param>
        /// <returns>执行结果</returns>
        static void AddJob(Job job)
        {
            job.Save();//保存
        }

        /// <summary>
        /// 更新工作任务
        /// </summary>
        /// <param name="job">任务信息</param>
        /// <returns>执行结果</returns>
        static void UpdateJob(Job job)
        {
            Job nowJob = jobRepository.Get(QueryFactory.Create<JobQuery>(r => r.Id == job.Id));
            if (nowJob == null)
            {
                throw new AppException("没有指定要操作的任务信息");
            }
            //修改信息
            nowJob.Name = job.Name;
            nowJob.JobPath = job.JobPath;
            nowJob.JobFileName = job.JobFileName;
            nowJob.Description = job.Description;
            nowJob.State = job.State;
            nowJob.SetGroup(job.Group.MapTo<JobGroup>(), true);
            nowJob.Save();//保存
        }

        #endregion

        #region 加载其它数据

        /// <summary>
        /// 加载其它数据
        /// </summary>
        /// <param name="jobs">工作任务信息</param>
        /// <param name="query">筛选条件</param>
        /// <returns></returns>
        static List<Job> LoadOtherObjectData(IEnumerable<Job> jobs, IQuery query)
        {
            if (jobs.IsNullOrEmpty())
            {
                return new List<Job>(0);
            }
            if (query == null)
            {
                return jobs.ToList();
            }

            #region 任务分组

            List<JobGroup> jobGroupList = null;
            if (query.AllowLoad<Job>(c => c.Group))
            {
                var jobGroupIds = jobs.Select(c => c.Group?.Code).Distinct().ToList();
                jobGroupList = JobGroupService.GetJobGroupList(jobGroupIds);
            }

            #endregion

            foreach (var job in jobs)
            {
                if (job == null || job.Group == null)
                {
                    continue;
                }
                if (!jobGroupList.IsNullOrEmpty())
                {
                    job.SetGroup(jobGroupList.FirstOrDefault(c => c.Code == job.Group?.Code));
                }
            }
            return jobs.ToList();
        }

        #endregion
    }
}
