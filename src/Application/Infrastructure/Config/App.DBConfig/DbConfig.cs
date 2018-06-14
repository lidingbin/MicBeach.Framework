using MicBeach.DB;
using MicBeach.DB.MySQL;
using MicBeach.DB.SQLServer;
using MicBeach.Develop.Command;
using MicBeach.Develop.CQuery;
using MicBeach.Entity.Task;
using MicBeach.Query.Task;
using MicBeach.Util.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DBConfig
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public class DbConfig
    {
        public static void Init()
        {
            DataBaseEngineConfig();//数据库执行器
            MicBeach.DB.DBConfig.GetDBServerMethod = GetServerInfo;//获取数据连接信息方法
            DataBaseNameConfig();//数据库名设置
            PrimaryKeyConfig();//主键信息配置
            CommandExecuteManager.ExectEngine = new DBCommandEngine();
            RefreshFieldsConfig();//刷新字段
        }

        /// <summary>
        /// 数据库执行器配置
        /// </summary>
        static void DataBaseEngineConfig()
        {
            MicBeach.DB.DBConfig.DbEngines.Add(ServerType.SQLServer, new SqlServerEngine());
            MicBeach.DB.DBConfig.DbEngines.Add(ServerType.MySQL, new MySqlEngine());
        }

        /// <summary>
        /// 获取数据库服务器
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        static List<ServerInfo> GetServerInfo(ICommand command)
        {
            List<ServerInfo> servers = new List<ServerInfo>();
            servers.Add(new ServerInfo()
            {
                ServerType = ServerType.SQLServer,
                ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString
            });
            return servers;
        }

        /// <summary>
        /// 数据库名设置
        /// </summary>
        static void DataBaseNameConfig()
        {
            #region Task

            QueryConfig.SetObjectName("Task_JobGroup", typeof(JobGroupEntity), typeof(JobGroupQuery));
            QueryConfig.SetObjectName("Task_ServerNode", typeof(ServerNodeEntity), typeof(ServerNodeQuery));
            QueryConfig.SetObjectName("Task_Job", typeof(JobEntity), typeof(JobQuery));
            QueryConfig.SetObjectName("Task_JobServerHost", typeof(JobServerHostEntity), typeof(JobServerHostQuery));

            QueryConfig.SetObjectName("Task_TriggerServer", typeof(TriggerServerEntity), typeof(TriggerServerQuery));
            QueryConfig.SetObjectName("Task_Trigger", typeof(TriggerEntity), typeof(TriggerQuery));
            QueryConfig.SetObjectName("Task_TriggerSimple", typeof(TriggerSimpleEntity), typeof(TriggerSimpleQuery));
            QueryConfig.SetObjectName("Task_TriggerExpression", typeof(TriggerExpressionEntity), typeof(TriggerExpressionQuery));
            QueryConfig.SetObjectName("Task_TriggerAnnualCondition", typeof(TriggerAnnualConditionEntity), typeof(TriggerAnnualConditionQuery));
            QueryConfig.SetObjectName("Task_TriggerDailyCondition", typeof(TriggerDailyConditionEntity), typeof(TriggerDailyConditionQuery));
            QueryConfig.SetObjectName("Task_TriggerExpressionCondition", typeof(TriggerExpressionConditionEntity), typeof(TriggerExpressionConditionQuery));
            QueryConfig.SetObjectName("Task_TriggerFullDateCondition", typeof(TriggerFullDateConditionEntity), typeof(TriggerFullDateConditionQuery));
            QueryConfig.SetObjectName("Task_TriggerMonthlyCondition", typeof(TriggerMonthlyConditionEntity), typeof(TriggerMonthlyConditionQuery));
            QueryConfig.SetObjectName("Task_TriggerWeeklyCondition", typeof(TriggerWeeklyConditionEntity), typeof(TriggerWeeklyConditionQuery));
            QueryConfig.SetObjectName("Task_ExecuteLog", typeof(ExecuteLogEntity), typeof(ExecuteLogQuery));
            QueryConfig.SetObjectName("Task_ErrorLog", typeof(ErrorLogEntity), typeof(ErrorLogQuery));
            QueryConfig.SetObjectName("Task_JobFile", typeof(JobFileEntity), typeof(JobFileQuery));
            //QueryConfig.SetObjectName("Task_Trigger", typeof(TriggerEntity), typeof(TriggerQuery));

            #endregion
        }

        /// <summary>
        /// 主键配置
        /// </summary>
        static void PrimaryKeyConfig()
        {
            #region Task

            QueryConfig.SetPrimaryKey<JobGroupEntity>(u => u.Code);
            QueryConfig.SetPrimaryKey<ServerNodeEntity>(u => u.Id);
            QueryConfig.SetPrimaryKey<JobEntity>(u => u.Id);
            QueryConfig.SetPrimaryKey<JobServerHostEntity>(u => u.Server, u => u.Job);
            QueryConfig.SetPrimaryKey<TriggerServerEntity>(u => u.Trigger, u => u.Server);
            QueryConfig.SetPrimaryKey<TriggerEntity>(u => u.Id);
            QueryConfig.SetPrimaryKey<TriggerSimpleEntity>(u => u.TriggerId);
            QueryConfig.SetPrimaryKey<TriggerExpressionEntity>(u => u.TriggerId, u => u.Option);
            QueryConfig.SetPrimaryKey<TriggerAnnualConditionEntity>(u => u.TriggerId, u => u.Month, u => u.Day);
            QueryConfig.SetPrimaryKey<TriggerDailyConditionEntity>(u => u.TriggerId);
            QueryConfig.SetPrimaryKey<TriggerExpressionConditionEntity>(u => u.TriggerId, u => u.ConditionOption);
            QueryConfig.SetPrimaryKey<TriggerFullDateConditionEntity>(u => u.TriggerId, u => u.Date);
            QueryConfig.SetPrimaryKey<TriggerMonthlyConditionEntity>(u => u.TriggerId, u => u.Day);
            QueryConfig.SetPrimaryKey<TriggerWeeklyConditionEntity>(u => u.TriggerId, u => u.Day);
            QueryConfig.SetPrimaryKey<ExecuteLogEntity>(u => u.Id);
            QueryConfig.SetPrimaryKey<ErrorLogEntity>(u => u.Id);
            QueryConfig.SetPrimaryKey<JobFileEntity>(u => u.Id);
            //QueryConfig.SetPrimaryKey<TriggerEntity>(u => u.Id); 

            #endregion
        }

        /// <summary>
        /// 刷新字段配置
        /// </summary>
        static void RefreshFieldsConfig()
        {
            #region Task

            QueryConfig.SetRefreshDateField<JobEntity>(c => c.UpdateDate);

            #endregion
        }
    }
}
