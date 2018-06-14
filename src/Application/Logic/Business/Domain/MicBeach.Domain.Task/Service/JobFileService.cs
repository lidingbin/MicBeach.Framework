using MicBeach.Domain.Task.Model;
using MicBeach.Domain.Task.Repository;
using MicBeach.Util;
using MicBeach.Util.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using MicBeach.Develop.CQuery;
using MicBeach.Util.Paging;
using MicBeach.Util.Response;

namespace MicBeach.Domain.Task.Service
{
    /// <summary>
    /// 任务工作文件服务
    /// </summary>
    public static class JobFileService
    {
        static IJobFileRepository jobFileRepository = ContainerManager.Container.Resolve<IJobFileRepository>();

        #region 保存

        /// <summary>
        /// 保存任务工作文件
        /// </summary>
        /// <param name="jobFile">任务工作文件信息</param>
        /// <returns></returns>
        public static Result<JobFile> SaveJobFile(JobFile jobFile)
        {
            return null;
        }

        #endregion

        #region 获取任务工作文件

        /// <summary>
        /// 获取任务工作文件
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public static JobFile GetJobFile(IQuery query)
        {
            var jobFile = jobFileRepository.Get(query);
            return jobFile;
        }

        #endregion

        #region 获取任务工作文件列表

        /// <summary>
        /// 获取任务工作文件列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public static List<JobFile> GetJobFileList(IQuery query)
        {
            var jobFileList = jobFileRepository.GetList(query);
            return jobFileList;
        }

        #endregion

        #region 获取任务工作文件分页

        /// <summary>
        /// 获取任务工作文件分页
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public static IPaging<JobFile> GetJobFilePaging(IQuery query)
        {
            var jobFilePaging = jobFileRepository.GetPaging(query);
            return jobFilePaging;
        }

        #endregion

        #region 删除任务工作文件

        /// <summary>
        /// 删除任务工作文件
        /// </summary>
        /// <param name="jobFiles">要删出的信息</param>
        /// <returns>执行结果</returns>
        public static Result DeleteJobFile(IEnumerable<JobFile> jobFiles)
        {
            #region 参数判断

            if (jobFiles.IsNullOrEmpty())
            {
                return Result.FailedResult("没有指定要删除的任务工作文件");
            }

            #endregion

            //删除逻辑
            return Result.SuccessResult("删除成功");
        }

        #endregion
    }
}
