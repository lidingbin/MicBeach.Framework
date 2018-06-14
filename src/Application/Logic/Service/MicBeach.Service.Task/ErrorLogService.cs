using MicBeach.ServiceInterface.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util;
using MicBeach.BusinessInterface.Task;
using MicBeach.Util.Response;
using MicBeach.DTO.Task.Cmd;
using MicBeach.DTO.Task.Query;
using MicBeach.DTO.Task.Query.Filter;
using MicBeach.Util.Paging;

namespace MicBeach.Service.Task
{
    /// <summary>
    /// 任务异常日志服务
    /// </summary>
    public class ErrorLogService : IErrorLogService
    {
        IErrorLogBusiness errorLogBusiness = null;

        public ErrorLogService(IErrorLogBusiness errorLogBusiness)
        {
            this.errorLogBusiness = errorLogBusiness;
        }

        #region 保存任务异常日志

        /// <summary>
        /// 保存任务异常日志
        /// </summary>
        /// <param name="saveInfo">保存信息</param>
        /// <returns>执行结果</returns>
        public Result SaveErrorLog(SaveErrorLogCmdDto saveInfo)
        {
            return errorLogBusiness.SaveErrorLog(saveInfo);
        }

        #endregion

        #region 获取任务异常日志

        /// <summary>
        /// 获取任务异常日志
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public ErrorLogDto GetErrorLog(ErrorLogFilterDto filter)
        {
            return errorLogBusiness.GetErrorLog(filter);
        }

        #endregion

        #region 获取任务异常日志列表

        /// <summary>
        /// 获取任务异常日志列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public List<ErrorLogDto> GetErrorLogList(ErrorLogFilterDto filter)
        {
            return errorLogBusiness.GetErrorLogList(filter);
        }

        #endregion

        #region 获取任务异常日志分页

        /// <summary>
        /// 获取任务异常日志分页
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public IPaging<ErrorLogDto> GetErrorLogPaging(ErrorLogFilterDto filter)
        {
            return errorLogBusiness.GetErrorLogPaging(filter);
        }

        #endregion

    }
}
