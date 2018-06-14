using MicBeach.Develop.CQuery;
using MicBeach.Util.Paging;
using MicBeach.DTO.Task.Cmd;
using MicBeach.DTO.Task.Query;
using MicBeach.DTO.Task.Query.Filter;
using MicBeach.Query.Task;
using MicBeach.Domain.Task.Repository;
using MicBeach.BusinessInterface.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using MicBeach.Util;
using MicBeach.Domain.Task.Model;
using MicBeach.Develop.UnitOfWork;
using MicBeach.Util.Response;
using MicBeach.Domain.Task.Service;

namespace MicBeach.Business.Task
{
    /// <summary>
    /// 任务工作文件业务
    /// </summary>
    public class JobFileBusiness : IJobFileBusiness
    {
        public JobFileBusiness()
        {
        }

        #region 保存任务工作文件

        /// <summary>
        /// 保存任务工作文件
        /// </summary>
        /// <param name="saveInfo">保存信息</param>
        /// <returns>执行结果</returns>
        public Result<JobFileDto> SaveJobFile(SaveJobFileCmdDto saveInfo)
        {
            if (saveInfo == null)
            {
                return Result<JobFileDto>.FailedResult("没有指定任何要保持的信息");
            }
            using (var businessWork = UnitOfWork.Create())
            {
                var saveResult = JobFileService.SaveJobFile(saveInfo.JobFile.MapTo<JobFile>());
                if (!saveResult.Success)
                {
                    return Result<JobFileDto>.FailedResult(saveResult.Message);
                }
                var commitResult = businessWork.Commit();
                Result<JobFileDto> result = null;
                if (commitResult.ExecutedSuccess)
                {
                    result = Result<JobFileDto>.SuccessResult("保存成功");
                    result.Data = saveResult.Data.MapTo<JobFileDto>();
                }
                else
                {
                    result = Result<JobFileDto>.FailedResult("保存失败");
                }
                return result;
            }
        }

        #endregion

        #region 获取任务工作文件

        /// <summary>
        /// 获取任务工作文件
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public JobFileDto GetJobFile(JobFileFilterDto filter)
        {
            var jobFile = JobFileService.GetJobFile(CreateQueryObject(filter));
            return jobFile.MapTo<JobFileDto>();
        }

        #endregion

        #region 获取任务工作文件列表

        /// <summary>
        /// 获取任务工作文件列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public List<JobFileDto> GetJobFileList(JobFileFilterDto filter)
        {
            var jobFileList = JobFileService.GetJobFileList(CreateQueryObject(filter));
            return jobFileList.Select(c => c.MapTo<JobFileDto>()).ToList();
        }

        #endregion

        #region 获取任务工作文件分页

        /// <summary>
        /// 获取任务工作文件分页
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public IPaging<JobFileDto> GetJobFilePaging(JobFileFilterDto filter)
        {
            var jobFilePaging = JobFileService.GetJobFilePaging(CreateQueryObject(filter));
            return jobFilePaging.ConvertTo<JobFileDto>();
        }

        #endregion

        #region 删除任务工作文件

        /// <summary>
        /// 删除任务工作文件
        /// </summary>
        /// <param name="deleteInfo">删除信息</param>
        /// <returns>执行结果</returns>
        public Result DeleteJobFile(DeleteJobFileCmdDto deleteInfo)
        {
            #region 参数判断

            if (deleteInfo == null || deleteInfo.JobFileIds.IsNullOrEmpty())
            {
                return Result.FailedResult("没有指定要删除的任务工作文件");
            }

            #endregion

            using (var businessWork = UnitOfWork.Create())
            {
                var jobFiles = deleteInfo.JobFileIds.Select(c => JobFile.CreateJobFile(c));
                var deleteResult = JobFileService.DeleteJobFile(jobFiles);
                if (!deleteResult.Success)
                {
                    return deleteResult;
                }
                var commitResult = businessWork.Commit();
                return commitResult.ExecutedSuccess ? Result.SuccessResult("删除成功") : Result.FailedResult("删除失败");
            }
        }

        #endregion


        #region 根据查询条件生成查询对象

        /// <summary>
        /// 根据查询条件生成查询对象
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        IQuery CreateQueryObject(JobFileFilterDto filter)
        {
            if (filter == null)
            {
                return null;
            }
            IQuery query = QueryFactory.Create<JobFileQuery>(filter);
            if (!filter.Ids.IsNullOrEmpty())
            {
                query.In<JobFileQuery>(c => c.Id, filter.Ids);
            }
            if (!filter.Job.IsNullOrEmpty())
            {
                query.Equal<JobFileQuery>(c => c.Job, filter.Job);
            }
            if (!filter.FileName.IsNullOrEmpty())
            {
                query.Equal<JobFileQuery>(c => c.FileName, filter.FileName);
            }
            if (!filter.FilePath.IsNullOrEmpty())
            {
                query.Equal<JobFileQuery>(c => c.FilePath, filter.FilePath);
            }
            if (filter.CreateDate.HasValue)
            {
                query.Equal<JobFileQuery>(c => c.CreateDate, filter.CreateDate.Value);
            }
            return query;
        }

        #endregion
    }
}
