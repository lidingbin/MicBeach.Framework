using MicBeach.Util;
using MicBeach.Util.CustomerException;
using MicBeach.Util.Response;
using MicBeach.Util.Serialize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWeb.Filter
{
    /// <summary>
    /// 应用程序异常
    /// </summary>
    public class ApplicationError :HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            //记录日志
            //Logger.WriteErrorAsync(filterContext.Exception.Message, filterContext.Exception);
            string errorMessage = "系统错误";
            if (filterContext.Exception is AppException)
            {
                errorMessage = filterContext.Exception.Message;
            }
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                Result result = new Result()
                {
                    Success = false,
                    Message = errorMessage
                };
                filterContext.Result = new JsonResult() { Data=result };
            }
            else
            {
                filterContext.Result = new RedirectResult(new UrlHelper(filterContext.RequestContext).Action("Error", "Home", new { msg = filterContext.Exception.Message }));
            }
        }
    }
}