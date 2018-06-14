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
using MicBeach.Domain.Task.Service;
using MicBeach.DTO.Task.Query.Filter;
using MicBeach.Util.Response;

namespace MicBeach.Business.Task
{
    /// <summary>
    /// 工作分组业务
    /// </summary>
    public class JobGroupBusiness : IJobGroupBusiness
    {
        public JobGroupBusiness()
        {
        }

        #region 保存工作分组

        /// <summary>
        /// 保存工作分组
        /// </summary>
        /// <param name="jobGroupInfo">工作分组对象</param>
        /// <returns>执行结果</returns>
        public Result<JobGroupDto> SaveJobGroup(JobGroupCmdDto jobGroupInfo)
        {
            using (var businessWork = UnitOfWork.Create())
            {
                if (jobGroupInfo == null)
                {
                    return Result<JobGroupDto>.FailedResult("分组信息不完整");
                }
                var jobGroup = jobGroupInfo.MapTo<JobGroup>();
                JobGroupService.SaveJobGroup(jobGroup);
                var commitResult = businessWork.Commit();
                Result<JobGroupDto> result = null;
                if (commitResult.ExecutedSuccess)
                {
                    result = Result<JobGroupDto>.SuccessResult("保存成功");
                    result.Data = jobGroup.MapTo<JobGroupDto>();
                }
                else
                {
                    result = Result<JobGroupDto>.FailedResult("保存失败");
                }
                return result;
            }
        }

        #endregion

        #region 获取工作分组

        /// <summary>
        /// 获取工作分组
        /// </summary>
        /// <param name="filter">筛选信息</param>
        /// <returns></returns>
        public JobGroupDto GetJobGroup(JobGroupFilterDto filter)
        {
            var jobGroup = JobGroupService.GetJobGroup(CreateQueryObject(filter));
            return jobGroup.MapTo<JobGroupDto>();
        }

        #endregion

        #region 获取工作分组列表

        /// <summary>
        /// 获取工作分组列表
        /// </summary>
        /// <param name="filter">筛选信息</param>
        /// <returns></returns>
        public List<JobGroupDto> GetJobGroupList(JobGroupFilterDto filter)
        {
            var jobGroupList = JobGroupService.GetJobGroupList(CreateQueryObject(filter));
            return jobGroupList.Select(c => c.MapTo<JobGroupDto>()).ToList();
        }

        #endregion

        #region 获取工作分组分页

        /// <summary>
        /// 获取工作分组分页
        /// </summary>
        /// <param name="filter">筛选信息</param>
        /// <returns></returns>
        public IPaging<JobGroupDto> GetJobGroupPaging(JobGroupFilterDto filter)
        {
            var jobGroupPaging = JobGroupService.GetJobGroupPaging(CreateQueryObject(filter));
            return jobGroupPaging.ConvertTo<JobGroupDto>();
        }

        #endregion

        #region 删除工作分组

        /// <summary>
        /// 删除工作分组
        /// </summary>
        /// <param name="deleteInfo">删除信息</param>
        /// <returns>执行结果</returns>
        public Result DeleteJobGroup(DeleteJobGroupCmdDto deleteInfo)
        {
            using (var businessWork = UnitOfWork.Create())
            {
                #region 参数判断

                if (deleteInfo == null || deleteInfo.JobGroupIds.IsNullOrEmpty())
                {
                    return Result.FailedResult("没有指定要删除的工作分组");
                }

                #endregion

                //删除逻辑
                JobGroupService.DeleteJobGroup(deleteInfo.JobGroupIds);
                var commitResult = businessWork.Commit();
                return commitResult.ExecutedSuccess ? Result.SuccessResult("删除成功") : Result.FailedResult("删除失败");
            }
        }

        #endregion

        #region 修改工作分组排序

        /// <summary>
        /// 修改分组排序
        /// </summary>
        /// <param name="sortInfo">排序信息</param>
        /// <returns>执行结果</returns>
        public Result ModifySortIndex(ModifyJobGroupSortCmdDto sortInfo)
        {
            return null;
        }

        #endregion

        #region 根据查询条件生成查询对象

        /// <summary>
        /// 根据查询条件生成查询对象
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        IQuery CreateQueryObject(JobGroupFilterDto filter)
        {
            if (filter == null)
            {
                return null;
            }
            IQuery query = QueryFactory.Create();
            if (!filter.Codes.IsNullOrEmpty())
            {
                query.In<JobGroupQuery>(c => c.Code, filter.Codes);
            }
            if (!filter.Name.IsNullOrEmpty())
            {
                query.Equal<JobGroupQuery>(c => c.Name, filter.Name);
            }
            if (filter.Sort.HasValue)
            {
                query.Equal<JobGroupQuery>(c => c.Sort, filter.Sort.Value);
            }
            if (!filter.Parent.IsNullOrEmpty())
            {
                query.Equal<JobGroupQuery>(c => c.Parent, filter.Parent);
            }
            if (!filter.Root.IsNullOrEmpty())
            {
                query.Equal<JobGroupQuery>(c => c.Root, filter.Root);
            }
            if (filter.Level.HasValue)
            {
                query.Equal<JobGroupQuery>(c => c.Level, filter.Level.Value);
            }
            if (!filter.Remark.IsNullOrEmpty())
            {
                query.Equal<JobGroupQuery>(c => c.Remark, filter.Remark);
            }
            return query;
        }

        #endregion
    }
}
