using MicBeach.DataValidation.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace App.Mapper
{
    /// <summary>
    /// 显示配置
    /// </summary>
    public static class DisplayConfig
    {
        public static void Init()
        {
            string folderPath = HttpContext.Current.Server.MapPath("~/App_Data/Config/Display");
            if (Directory.Exists(folderPath))
            {
                var files = Directory.GetFiles(folderPath).Where(c => Path.GetExtension(c).Trim('.').ToLower() == "disconfig").ToArray();
                MicBeach.DataValidation.Config.DisplayConfig.InitFromFiles(files);
            }

            //#region Sys

            //#region 角色

            //DisplayManager.Add<RoleViewModel>(r => r.Name, "名称");
            //DisplayManager.Add<RoleViewModel>(r => r.Remark, "说明");
            //DisplayManager.Add<RoleViewModel>(r => r.Status, "状态");
            //DisplayManager.Add<RoleViewModel>(r => r.Parent, "上级");
            //DisplayManager.Add<RoleViewModel>(r => r.Level, "等级");

            //#endregion

            //#endregion
        }
    }
}
