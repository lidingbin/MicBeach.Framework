using MicBeach.Util.Paging;
using MicBeach.DTO.Task.Cmd;
using MicBeach.DTO.Task.Query;
using MicBeach.DTO.Task.Query.Filter;
using MicBeach.Util.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.ServiceInterface.Task
{
    /// <summary>
    /// 任务工作文件服务接口
    /// </summary>
    public interface IJobFileService
    {
        #region 保存任务工作文件

        /// <summary>
        /// 保存任务工作文件
        /// </summary>
        /// <param name="saveInfo">保存信息</param>
        /// <returns>执行结果</returns>
        Result<JobFileDto> SaveJobFile(SaveJobFileCmdDto saveInfo);

        #endregion

        #region 获取任务工作文件

        /// <summary>
        /// 获取任务工作文件
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        JobFileDto GetJobFile(JobFileFilterDto filter);

        #endregion

        #region 获取任务工作文件列表

        /// <summary>
        /// 获取任务工作文件列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        List<JobFileDto> GetJobFileList(JobFileFilterDto filter);

        #endregion

        #region 获取任务工作文件分页

        /// <summary>
        /// 获取任务工作文件分页
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        IPaging<JobFileDto> GetJobFilePaging(JobFileFilterDto filter);

        #endregion

        #region 删除任务工作文件

        /// <summary>
        /// 删除任务工作文件
        /// </summary>
        /// <param name="deleteInfo">删除信息</param>
        /// <returns>执行结果</returns>
        Result DeleteJobFile(DeleteJobFileCmdDto deleteInfo);

        #endregion
    }
}
