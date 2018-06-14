using MicBeach.Util;
using MicBeach.Util.Serialize;
using MicBeach.ViewModel.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWeb.Controllers.Base;
using MicBeach.Util.Extension;
using MicBeach.Develop.CQuery;
using MicBeach.DTO.Task.Cmd;
using MicBeach.ServiceInterface.Task;
using MicBeach.Util.Paging;
using MicBeach.Web.Mvc;
using MicBeach.DTO.Task.Query;
using MicBeach.ViewModel.Task.Filter;
using MicBeach.DTO.Task.Query.Filter;
using MicBeach.CTask;
using MicBeach.CTask.Client.RemoteCommand;
using MicBeach.CTask.Job;
using MicBeach.CTask.Service;
using MicBeach.CTask.Client;
using MicBeach.ViewModel.Common;
using MicBeach.Util.Response;
using System.IO;

namespace TestWeb.Controllers
{
    public class TaskController : WebBaseController
    {
        IJobGroupService jobGroupService = null;
        IServerNodeService serverNodeService = null;
        IJobService jobService = null;
        IJobServerHostService jobServerHostService = null;
        ITriggerService triggerService = null;
        ITriggerServerService triggerServerService = null;
        IExecuteLogService executeLogService = null;
        IErrorLogService errorLogService = null;
        IJobFileService jobFileService = null;
        public TaskController()
        {
            jobGroupService = this.Instance<IJobGroupService>();
            serverNodeService = this.Instance<IServerNodeService>();
            jobService = this.Instance<IJobService>();
            jobServerHostService = this.Instance<IJobServerHostService>();
            triggerService = this.Instance<ITriggerService>();
            triggerServerService = this.Instance<ITriggerServerService>();
            executeLogService = this.Instance<IExecuteLogService>();
            errorLogService = this.Instance<IErrorLogService>();
            jobFileService = this.Instance<IJobFileService>();
        }

        #region 工作分组管理

        #region 工作分组列表

        public ActionResult JobGroupList()
        {
            Tuple<string, string, string> jobGroupDatas = InitJobGroupTreeData("");
            ViewBag.AllJobGroup = jobGroupDatas.Item3;
            ViewBag.AllNodes = jobGroupDatas.Item1;
            ViewBag.SelectNodes = jobGroupDatas.Item2;
            return View();
        }

        #endregion

        #region 编辑/添加工作分组

        public ActionResult EditJobGroup(JobGroupViewModel jobGroup)
        {
            if (IsPost)
            {
                var result = jobGroupService.SaveJobGroup(jobGroup.MapTo<JobGroupCmdDto>());
                var initResult = new Result() { Success = result.Success, Message = result.Message };
                if (result.Success)
                {
                    InitJobGroupResultData(initResult);
                }
                return Json(initResult);
            }
            else if (!(jobGroup.Code.IsNullOrEmpty()))
            {
                jobGroup = jobGroupService.GetJobGroup(new JobGroupFilterDto()
                {
                    Codes = new List<string>()
                    {
                        jobGroup.Code
                    }
                }).MapTo<JobGroupViewModel>();
            }
            return View(jobGroup);
        }

        #endregion

        #region 删除工作分组

        public ActionResult DeleteJobGroup(string codes)
        {
            IEnumerable<string> codeArray = codes.LSplit(",");
            Result result = jobGroupService.DeleteJobGroup(new DeleteJobGroupCmdDto()
            {
                JobGroupIds = codeArray
            });
            InitJobGroupResultData(result);
            return Json(result);
        }

        #endregion

        #region 修改工作分组排序

        [HttpPost]
        public ActionResult ChangeJobGroupSort(string code, int sort)
        {
            Result result = null;
            //result = jobGroupService.ModifySort(new ModifyJobGroupSortCmdDto()
            //{
            //    Code = code,
            //    NewSort = sort
            //});
            if (result.Success)
            {
                InitJobGroupResultData(result);
            }
            return Json(result);
        }

        #endregion

        #region 获取下级数据

        /// <summary>
        /// 获取下级数据
        /// </summary>
        /// <param name="parentId">上级编号</param>
        /// <returns></returns>
        public ActionResult LoadChildJobGroups(string parentId)
        {
            List<JobGroupViewModel> childJobGroups = null;
            if (!parentId.IsNullOrEmpty())
            {
                childJobGroups = jobGroupService.GetJobGroupList(new JobGroupFilterDto()
                {
                    Parent = parentId
                }).Select(c => c.MapTo<JobGroupViewModel>()).OrderBy(r => r.Sort).ToList();
            }
            childJobGroups = childJobGroups ?? new List<JobGroupViewModel>(0);
            List<TreeNode> treeNodeList = childJobGroups.Select(c => JobGroupToTreeNode(c)).ToList();
            string nodesString = JsonSerialize.ObjectToJson<List<TreeNode>>(treeNodeList);
            string jobGroupsData = JsonSerialize.ObjectToJson(childJobGroups.ToDictionary(c => c.Code.ToString()));
            return Json(new
            {
                ChildNodes = nodesString,
                JobGroupData = jobGroupsData
            });
        }

        #endregion

        #region 数据序列化

        string JobGroupListToJsonString(IEnumerable<JobGroupViewModel> jobGroupList)
        {
            List<TreeNode> nodeList = JobGroupListToTreeNodes(jobGroupList.ToList());
            return JsonSerialize.ObjectToJson(nodeList);
        }

        List<TreeNode> JobGroupListToTreeNodes(List<JobGroupViewModel> jobGroupList)
        {
            if (jobGroupList.IsNullOrEmpty())
            {
                return new List<TreeNode>(0);
            }
            List<TreeNode> nodeList = new List<TreeNode>(jobGroupList.Count);
            var levelOneJobGroups = jobGroupList.Where(c => c.Level == 1).OrderBy(c => c.Sort);
            foreach (var jobGroup in levelOneJobGroups)
            {
                TreeNode node = JobGroupToTreeNode(jobGroup);
                AppendChildNodes(node, jobGroup.Code, jobGroupList);
                nodeList.Add(node);
            }
            return nodeList;
        }

        void AppendChildNodes(TreeNode parentNode, string parentCode, IEnumerable<JobGroupViewModel> allJobGroups)
        {
            var childJobGroups = allJobGroups.Where(c => c.Parent != null && c.Parent.Code == parentCode && c.Code != c.Parent.Code && !(c.Parent.Code.IsNullOrEmpty())).OrderBy(c => c.Sort);
            if (childJobGroups.IsNullOrEmpty())
            {
                return;
            }
            foreach (var jobGroup in childJobGroups)
            {
                TreeNode node = JobGroupToTreeNode(jobGroup);
                parentNode.Children.Add(node);
                AppendChildNodes(node, jobGroup.Code, allJobGroups);
            }
        }

        TreeNode JobGroupToTreeNode(JobGroupViewModel jobGroup)
        {
            return new TreeNode()
            {
                Value = jobGroup.Code.ToString(),
                Text = jobGroup.Name,
                Children = new List<TreeNode>(),
                IsParent = true
            };
        }

        void InitJobGroupResultData(Result res)
        {
            Tuple<string, string, string> jobGroupDatas = InitJobGroupTreeData("");
            res.Data = new
            {
                AllNode = jobGroupDatas.Item1,
                SelectNode = jobGroupDatas.Item2,
                AllJobGroup = jobGroupDatas.Item3
            };
        }

        Tuple<string, string, string> InitJobGroupTreeData(string parentId)
        {
            JobGroupFilterDto groupFilter = new JobGroupFilterDto()
            {
                Level = 1
            };
            if (!parentId.IsNullOrEmpty())
            {
                groupFilter.Parent = parentId;
            }
            List<JobGroupViewModel> allJobGroups = jobGroupService.GetJobGroupList(groupFilter).Select(c => c.MapTo<JobGroupViewModel>()).ToList();
            string allNodesString = JobGroupListToJsonString(allJobGroups);
            JobGroupViewModel[] copyJobGroups = new JobGroupViewModel[allJobGroups.Count];
            allJobGroups.CopyTo(copyJobGroups);
            List<JobGroupViewModel> selectJobGroups = copyJobGroups.ToList();
            selectJobGroups.Insert(0, new JobGroupViewModel()
            {
                Name = "一级分组",
                Code = "",
                Level = 1
            });
            string selectNodesString = JobGroupListToJsonString(selectJobGroups);
            return new Tuple<string, string, string>(allNodesString, selectNodesString, JsonSerialize.ObjectToJson(allJobGroups.ToDictionary(c => c.Code.ToString())));
        }

        #endregion

        #region 分组单选

        /// <summary>
        /// 操作分组单选
        /// </summary>
        /// <returns></returns>
        public ActionResult JobGroupSingleSelect()
        {
            var result = InitJobGroupTreeData("");
            ViewBag.AllJobGroup = result.Item3;
            ViewBag.AllNodes = result.Item1;
            return View();
        }

        #endregion

        #endregion

        #region 服务节点管理

        #region 服务节点列表

        public ActionResult ServerNodeList()
        {
            return View();
        }

        public ActionResult SearchServerNode(ServerNodeFilterViewModel filter)
        {
            IPaging<ServerNodeViewModel> serverNodePager = serverNodeService.GetServerNodePaging(filter.MapTo<ServerNodeFilterDto>()).ConvertTo<ServerNodeViewModel>();
            object objResult = new
            {
                TotalCount = serverNodePager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(serverNodePager.ToList())
            };
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 编辑/添加服务节点

        public ActionResult EditServerNode(ServerNodeViewModel serverNode)
        {
            if (IsPost)
            {
                SaveServerNodeCmdDto saveInfo = new SaveServerNodeCmdDto()
                {
                    ServerNode = serverNode.MapTo<ServerNodeCmdDto>()
                };
                var result = serverNodeService.SaveServerNode(saveInfo);
                return Json(result);
            }
            else if (!(serverNode.Id.IsNullOrEmpty()))
            {
                ServerNodeFilterDto filterDto = new ServerNodeFilterDto()
                {
                    Ids = new List<string>()
                    {
                        serverNode.Id
                    }
                };
                serverNode = serverNodeService.GetServerNode(filterDto).MapTo<ServerNodeViewModel>();
            }
            return View(serverNode);
        }

        #endregion

        #region 删除服务节点

        public ActionResult DeleteServerNode(string ids)
        {
            IEnumerable<string> idArray = ids.LSplit(",");
            Result result = serverNodeService.DeleteServerNode(new DeleteServerNodeCmdDto()
            {
                ServerNodeIds = idArray
            });
            return Json(result);
        }

        #endregion

        #region 服务详情

        public ActionResult ServerNodeDetail(string id)
        {
            ServerNodeViewModel server = null;
            if (!id.IsNullOrEmpty())
            {
                ServerNodeFilterDto filterDto = new ServerNodeFilterDto()
                {
                    Ids = new List<string>()
                    {
                        id
                    }
                };
                server = serverNodeService.GetServerNode(filterDto).MapTo<ServerNodeViewModel>();
            }
            if (server == null)
            {
                return Content("信息获取失败");
            }
            return View(server);
        }

        #endregion

        #region 服务节点多选

        public ActionResult ServerNodeMultipleSelect()
        {
            return View();
        }

        public ActionResult ServerNodeMultipleSelectSearch(ServerNodeFilterViewModel filter)
        {
            ServerNodeFilterDto filterDto = filter.MapTo<ServerNodeFilterDto>();
            IPaging<ServerNodeViewModel> serverNodePager = serverNodeService.GetServerNodePaging(filterDto).ConvertTo<ServerNodeViewModel>();
            object objResult = new
            {
                TotalCount = serverNodePager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(serverNodePager.ToList())
            };
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 修改服务运行状态

        [HttpPost]
        public ActionResult ModifyServiceRunState(ServerNodeViewModel service)
        {
            ModifyServerNodeRunStateCmdDto cmd = new ModifyServerNodeRunStateCmdDto()
            {
                Servers = new List<ServerNodeCmdDto>()
                {
                    service.MapTo<ServerNodeCmdDto>()
                }
            };
            return Json(serverNodeService.ModifyRunState(cmd));
        }

        #endregion

        #endregion

        #region 工作任务管理

        #region 工作任务列表

        public ActionResult JobList()
        {
            return View();
        }

        public ActionResult SearchJob(JobFilterViewModel filter)
        {
            var filterDto = filter.MapTo<JobFilterDto>();
            IPaging<JobViewModel> jobPager = jobService.GetJobPaging(filterDto).ConvertTo<JobViewModel>();

            #region 分组

            IEnumerable<string> groupCodes = jobPager.Select(c => c.Group?.Code).Distinct().ToList();
            JobGroupFilterDto groupFilter = new JobGroupFilterDto()
            {
                Codes = groupCodes.ToList()
            };
            IEnumerable<JobGroupViewModel> jobGroupList = jobGroupService.GetJobGroupList(groupFilter).Select(c => c.MapTo<JobGroupViewModel>());
            foreach (var job in jobPager)
            {
                job.Group = jobGroupList.FirstOrDefault(c => c.Code == job.Group?.Code);
            }

            #endregion

            string viewContent = this.RenderViewContent("Partial/_JobList", jobPager, "", true);
            object objResult = new
            {
                TotalCount = jobPager.TotalCount,
                View = viewContent
            };
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 任务详情

        public ActionResult JobDetail(string id)
        {
            var jobFilter = new JobFilterDto()
            {
                Ids = new List<string>()
                {
                    id
                },
                LoadGroup = true
            };
            var nowJob = jobService.GetJob(jobFilter).MapTo<JobViewModel>();
            if (nowJob == null)
            {
                return Content("获取数据失败");
            }
            return View(nowJob);
        }

        #endregion

        #region 编辑/添加工作任务

        public ActionResult EditJob(JobViewModel job)
        {
            if (IsPost)
            {
                var result = jobService.SaveJob(new SaveJobCmdDto()
                {
                    Job = job.MapTo<JobCmdDto>()
                });
                return Json(result);
            }
            else if (!(job.Id.IsNullOrEmpty()))
            {
                var jobFilter = new JobFilterDto()
                {
                    Ids = new List<string>()
                {
                    job.Id
                },
                    LoadGroup = true
                };
                job = jobService.GetJob(jobFilter).MapTo<JobViewModel>();
            }
            else if (job.Group?.Code.IsNullOrEmpty() ?? false)
            {
                job.Group = jobGroupService.GetJobGroup(new JobGroupFilterDto()
                {
                    Codes = new List<string>()
                    {
                        job.Group.Code
                    }
                }).MapTo<JobGroupViewModel>();
            }
            return View(job);
        }

        #endregion

        #region 删除工作任务

        public ActionResult DeleteJob(IEnumerable<string> jobIds)
        {
            Result result = jobService.DeleteJob(new DeleteJobCmdDto()
            {
                JobIds = jobIds
            });
            return Json(result);
        }

        #endregion

        #region 工作任务多选

        public ActionResult JobMultiSelect()
        {
            var result = InitJobGroupTreeData("");
            ViewBag.AllNodes = result.Item1;
            return View();
        }

        [HttpPost]
        public ActionResult JobMultiSelectSearch(JobFilterViewModel filter)
        {
            var filterDto = filter.MapTo<JobFilterDto>();
            List<JobViewModel> jobList = jobService.GetJobList(filterDto).Select(c => c.MapTo<JobViewModel>()).ToList();
            var result = new
            {
                Datas = JsonSerialize.ObjectToJson(jobList)
            };
            return Json(result);
        }

        #endregion

        #region 加载分组任务


        [HttpPost]
        public ActionResult GetGroupJobs(string groupId, string key)
        {
            JobFilterDto filter = new JobFilterDto()
            {
                Group = groupId,
                Name = key
            };
            List<JobViewModel> jobList = jobService.GetJobList(filter).Select(c => c.MapTo<JobViewModel>()).ToList();
            var result = new
            {
                Datas = JsonSerialize.ObjectToJson(jobList)
            };
            return Json(result);
        }

        #endregion

        #region 修改任务运行状态

        [HttpPost]
        public ActionResult ModifyJobRunState(JobViewModel job)
        {
            ModifyJobRunStateCmdDto stateInfo = new ModifyJobRunStateCmdDto()
            {
                Jobs = new List<JobCmdDto>()
                {
                    job.MapTo<JobCmdDto>()
                }
            };
            return Json(jobService.ModifyJobRunState(stateInfo));
        }

        #endregion

        #region 上传工作文件

        public ActionResult UploadJobFile(string jobId)
        {
            string jobFolderPath = Path.Combine("Job", jobId??string.Empty);
            var uploader = new UploadController();
            return null;

        }

        #endregion

        #endregion

        #region 任务&服务承载

        #region 获取任务承载服务

        public ActionResult GetJobServerHostByJob(JobServerHostFilterViewModel filter)
        {
            JobServerHostFilterDto filterDto = filter.MapTo<JobServerHostFilterDto>();
            var serverHostPager = jobServerHostService.GetJobServerHostPaging(filterDto);
            //服务信息
            List<string> serverIds = serverHostPager.Select(c => c.Server?.Id).Distinct().ToList();
            ServerNodeFilterDto serverFilter = new ServerNodeFilterDto()
            {
                Ids = serverIds
            };
            var serverList = serverNodeService.GetServerNodeList(serverFilter);
            foreach (var serverHost in serverHostPager)
            {
                serverHost.Server = serverList.FirstOrDefault(c => c.Id == serverHost.Server?.Id);
            }
            var serverHostViewPager = serverHostPager.ConvertTo<JobServerHostViewModel>();
            object objResult = new
            {
                TotalCount = serverHostViewPager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(serverHostViewPager.ToList())
            };
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetJobServerHostByServer(JobServerHostFilterViewModel filter)
        {
            JobServerHostFilterDto filterDto = filter.MapTo<JobServerHostFilterDto>();
            var serverHostPager = jobServerHostService.GetJobServerHostPaging(filterDto);
            //任务信息
            List<string> jobIds = serverHostPager.Select(c => c.Job?.Id).Distinct().ToList();
            JobFilterDto jobFilter = new JobFilterDto()
            {
                Ids = jobIds
            };
            var jobList = jobService.GetJobList(jobFilter);
            foreach (var serverHost in serverHostPager)
            {
                serverHost.Job = jobList.FirstOrDefault(c => c.Id == serverHost.Job?.Id);
            }
            var serverHostViewPager = serverHostPager.ConvertTo<JobServerHostViewModel>();
            object objResult = new
            {
                TotalCount = serverHostViewPager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(serverHostPager.ToList())
            };
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 保存任务承载服务

        [HttpPost]
        public ActionResult SaveJobServerHost(IEnumerable<JobServerHostViewModel> serverHosts)
        {
            if (serverHosts.IsNullOrEmpty())
            {
                return Json(Result.FailedResult("没有指定任何要保存的信息"));
            }
            SaveJobServerHostCmdDto saveInfo = new SaveJobServerHostCmdDto()
            {
                JobServerHosts = serverHosts.Select(c => { c.RunState = JobServerState.启用; return c.MapTo<JobServerHostCmdDto>(); }).ToList()
            };
            var result = jobServerHostService.SaveJobServerHost(saveInfo);
            return Json(result);
        }

        #endregion

        #region 修改任务承载运行状态

        [HttpPost]
        public ActionResult ModifyJobServerHostRunState(IEnumerable<JobServerHostViewModel> serverHosts)
        {
            ModifyJobServerHostRunStateCmdDto modifyInfo = new ModifyJobServerHostRunStateCmdDto()
            {
                JobServerHosts = serverHosts.Select(c => c.MapTo<JobServerHostCmdDto>()).ToList()
            };
            var result = jobServerHostService.ModifyRunState(modifyInfo);
            return Json(result);
        }

        #endregion

        #region 删除任务承载服务

        [HttpPost]
        public ActionResult DeleteJobServerHost(IEnumerable<JobServerHostViewModel> serverHosts)
        {
            DeleteJobServerHostCmdDto deleteInfo = new DeleteJobServerHostCmdDto()
            {
                JobServerHosts = serverHosts.Select(c => c.MapTo<JobServerHostCmdDto>()).ToList()
            };
            var result = jobServerHostService.DeleteJobServerHost(deleteInfo);
            return Json(result);
        }

        #endregion

        #endregion

        #region 执行计划

        public ActionResult EditTrigger(TriggerViewModel trigger)
        {
            if (IsPost)
            {
                #region 初始化计划基本信息

                if (trigger.Type == TaskTriggerType.简单)
                {
                    Result<SimpleTriggerViewModel> simpleDataResult = InitSimpleTriggerValue(trigger);
                    if (!simpleDataResult.Success)
                    {
                        return Json(simpleDataResult);
                    }
                    trigger = simpleDataResult.Object;
                }
                else
                {
                    Result<ExpressionTriggerViewModel> expressionDataResult = InitExpressionTriggerValue(trigger);
                    if (!expressionDataResult.Success)
                    {
                        return Json(expressionDataResult);
                    }
                    trigger = expressionDataResult.Object;
                }

                #endregion

                #region 计划附加条件

                var conditionResult = InitTriggerCondition(trigger);
                if (!conditionResult.Success)
                {
                    return Json(conditionResult);
                }
                trigger.Condition = conditionResult.Object;

                #endregion

                //#region 应用对象

                //var serverResult = InitTriggerServer(trigger);
                //if (!serverResult.Success)
                //{
                //    return Json(serverResult);
                //}

                //#endregion

                SaveTriggerCmdDto saveInfo = new SaveTriggerCmdDto()
                {
                    Trigger = trigger.MapTo<TriggerCmdDto>(),
                    //TriggerServers = serverResult.Data?.Select(c => c.MapTo<TriggerServerCmdDto>()).ToList()
                };
                return Json(triggerService.SaveTrigger(saveInfo));
            }
            if (!trigger.Id.IsNullOrEmpty())
            {
                TriggerFilterDto filter = new TriggerFilterDto()
                {
                    Ids = new List<string>()
                    {
                        trigger.Id
                    },
                    LoadCondition = true
                };
                trigger = triggerService.GetTrigger(filter).MapTo<TriggerViewModel>();
                if (trigger == null)
                {
                    return Content("获取信息失败");
                }
            }
            if (trigger.Job == null || trigger.Job.Id.IsNullOrEmpty())
            {
                return Content("请指定要添加计划的任务");
            }
            JobFilterDto jobFilter = new JobFilterDto()
            {
                Ids = new List<string>()
                    {
                        trigger.Job.Id
                    }
            };
            var job = jobService.GetJob(jobFilter);
            if (job == null)
            {
                return Content("请指定要添加计划的任务");
            }
            trigger.Job = job.MapTo<JobViewModel>();
            return View(trigger);
        }

        public ActionResult TriggerDetail(string id)
        {
            if (id.IsNullOrEmpty())
            {
                return Content("信息获取失败");
            }
            TriggerFilterDto filter = new TriggerFilterDto()
            {
                Ids = new List<string>()
                {
                    id
                },
                LoadJob = true,
                LoadCondition = true
            };
            TriggerViewModel trigger = triggerService.GetTrigger(filter).MapTo<TriggerViewModel>();
            if (trigger == null)
            {
                return Content("信息获取失败");
            }
            return View(trigger);
        }

        /// <summary>
        /// 获取工作任务的执行计划
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetJobTrigger(TriggerFilterViewModel filter)
        {
            IPaging<TriggerViewModel> triggerPager = triggerService.GetTriggerPaging(filter.MapTo<TriggerFilterDto>()).ConvertTo<TriggerViewModel>();
            object objResult = new
            {
                TotalCount = triggerPager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(triggerPager.ToList())
            };
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RemoveTrigger(IEnumerable<string> triggerIds)
        {
            DeleteTriggerCmdDto deleteInfo = new DeleteTriggerCmdDto()
            {
                TriggerIds = triggerIds
            };
            return Json(triggerService.DeleteTrigger(deleteInfo));
        }

        [HttpPost]
        public ActionResult ModifyTriggerState(IEnumerable<TriggerViewModel> triggers)
        {
            ModifyTriggerStateCmdDto stateInfo = new ModifyTriggerStateCmdDto()
            {
                Triggers = triggers.Select(c => c.MapTo<TriggerCmdDto>()).ToList()
            };
            return Json(triggerService.ModifyTriggerState(stateInfo));
        }

        #region 初始执行计划信息

        Result<SimpleTriggerViewModel> InitSimpleTriggerValue(TriggerViewModel trigger)
        {
            if (trigger == null)
            {
                return Result<SimpleTriggerViewModel>.FailedResult("执行计划信息为空");
            }
            bool repeatForever = false;
            bool.TryParse(Request["RepeatForever"].LSplit(",")[0], out repeatForever);
            int repeatCount = 0;
            int.TryParse(Request["RepeatCount"], out repeatCount);
            int repeatInterval = 0;
            int.TryParse(Request["RepeatInterval"], out repeatInterval);
            if (!repeatForever)
            {
                if (repeatCount <= 0)
                {
                    return Result<SimpleTriggerViewModel>.FailedResult("请指定任务要重复执行的次数");
                }
            }
            else
            {
                repeatCount = 0;
            }
            if (repeatInterval <= 0)
            {
                return Result<SimpleTriggerViewModel>.FailedResult("请指定任务执行的间隔毫秒数");
            }
            var result = Result<SimpleTriggerViewModel>.SuccessResult("数据初始化成功");
            result.Data = trigger.MapTo<SimpleTriggerViewModel>();
            result.Object.RepeatForever = repeatForever;
            result.Object.RepeatCount = repeatCount;
            result.Object.RepeatInterval = repeatInterval;
            return result;
        }

        Result<ExpressionTriggerViewModel> InitExpressionTriggerValue(TriggerViewModel trigger)
        {
            if (trigger == null)
            {
                return Result<ExpressionTriggerViewModel>.FailedResult("执行计划数据为空");
            }
            var options = Enum.GetValues(typeof(TaskTriggerExpressionItem));
            List<ExpressionItemViewModel> expressionItems = new List<ExpressionItemViewModel>();
            foreach (TaskTriggerExpressionItem item in options)
            {
                string valueTypeKey = string.Format("Item_ValueType_{0}", (int)item);
                int valueTypeVal = 0;
                int.TryParse(Request[valueTypeKey], out valueTypeVal);
                if (!Enum.IsDefined(typeof(TaskTriggerExpressionItemConfigType), valueTypeVal))
                {
                    continue;
                }
                TaskTriggerExpressionItemConfigType valueType = (TaskTriggerExpressionItemConfigType)valueTypeVal;
                ExpressionItemViewModel expressionItem = new ExpressionItemViewModel()
                {
                    ValueType = valueType,
                    Option = item,
                };
                switch (valueType)
                {
                    case TaskTriggerExpressionItemConfigType.集合值:
                        List<string> arrayValues = new List<string>();
                        if (item == TaskTriggerExpressionItem.星期)
                        {
                            arrayValues = Request["Item_Week_ArrayVal"]?.Trim().LSplit(",").ToList();
                        }
                        else
                        {
                            arrayValues = Request["Item_ArrayValue_" + (int)item]?.Trim().LSplit(",").ToList();
                        }
                        if (arrayValues.IsNullOrEmpty())
                        {
                            return Result<ExpressionTriggerViewModel>.FailedResult(string.Format("选项【{0}】集合值为空", item.ToString()));
                        }
                        expressionItem.BeginValue = 0;
                        expressionItem.EndValue = 0;
                        expressionItem.ArrayValue = arrayValues;
                        break;
                    case TaskTriggerExpressionItemConfigType.范围值:
                        int beginValue = 0;
                        int endValue = 0;
                        string beginVal = Request["Item_BeginValue_" + (int)item]?.Trim();
                        string endVal = Request["Item_EndValue_" + (int)item]?.Trim();
                        if (!int.TryParse(beginVal, out beginValue) || !int.TryParse(endVal, out endValue))
                        {
                            return Result<ExpressionTriggerViewModel>.FailedResult(string.Format("请填写选项【{0}】正确的范围值", item.ToString()));
                        }
                        if (beginValue > endValue)
                        {
                            return Result<ExpressionTriggerViewModel>.FailedResult(string.Format("选项【{0}】范围起始值不能大于结束值", item.ToString()));
                        }
                        if (item == TaskTriggerExpressionItem.星期)
                        {
                            Type weekType = typeof(TaskWeek);
                            if (!Enum.IsDefined(weekType, beginValue) || !Enum.IsDefined(weekType, endValue))
                            {
                                return Result<ExpressionTriggerViewModel>.FailedResult(string.Format("请指定选项【{0}】正确的范围值", item.ToString()));
                            }
                        }
                        expressionItem.BeginValue = beginValue;
                        expressionItem.EndValue = endValue;
                        break;
                    case TaskTriggerExpressionItemConfigType.开始_间隔:
                        int intervalBeginValue = 0;
                        int intervalValue = 0;
                        string intervalBeginVal = Request["Item_IntervalBeginValue_" + (int)item]?.Trim();
                        string intervalVal = Request["Item_IntervalSplitValue_" + (int)item]?.Trim();
                        if (!int.TryParse(intervalBeginVal, out intervalBeginValue) || !int.TryParse(intervalVal, out intervalValue))
                        {
                            return Result<ExpressionTriggerViewModel>.FailedResult(string.Format("请填写选项【{0}】正确的起始/间隔值", item.ToString()));
                        }
                        if (intervalValue <= 0)
                        {
                            return Result<ExpressionTriggerViewModel>.FailedResult(string.Format("请填写选项【{0}】正确的执行间隔值", item.ToString()));
                        }
                        expressionItem.BeginValue = intervalBeginValue;
                        expressionItem.EndValue = intervalValue;
                        break;
                    case TaskTriggerExpressionItemConfigType.每月倒数第N天:
                        int monthEndDay = 0;
                        string monthEndDayVal = Request["Item_MonthEndDay_" + (int)item]?.Trim();
                        if (!int.TryParse(monthEndDayVal, out monthEndDay) || monthEndDay <= 0 || monthEndDay > 31)
                        {
                            return Result<ExpressionTriggerViewModel>.FailedResult(string.Format("请填写选项【{0}】正确的倒数天数", item.ToString()));
                        }
                        expressionItem.BeginValue = monthEndDay;
                        break;
                    case TaskTriggerExpressionItemConfigType.每月个最后一个星期N:
                        int monthLastWeekDay = 0;
                        string monthLastWeekDayVal = Request["Item_MonthLastWeekDay_" + (int)item]?.Trim();
                        if (!int.TryParse(monthLastWeekDayVal, out monthLastWeekDay) || !Enum.IsDefined(typeof(TaskWeek), monthLastWeekDay))
                        {
                            return Result<ExpressionTriggerViewModel>.FailedResult(string.Format("请指定选项【{0}】正确的值", item.ToString()));
                        }
                        expressionItem.BeginValue = monthLastWeekDay;
                        break;
                    case TaskTriggerExpressionItemConfigType.本月第M个星期N:
                        int monthWhichWeekDayNumber = 0;
                        int monthWhichWeekDay = 0;
                        string monthWhichWeekDayNumberVal = Request["Item_MonthWhichWeekDay_Num_" + (int)item]?.Trim();
                        string monthWhichWeekDayVal = Request["Item_MonthWhichWeekDay_" + (int)item]?.Trim();
                        if (!int.TryParse(monthWhichWeekDayNumberVal, out monthWhichWeekDayNumber) || !int.TryParse(monthWhichWeekDayVal, out monthWhichWeekDay) || !Enum.IsDefined(typeof(TaskWeek), monthWhichWeekDay) || monthWhichWeekDayNumber <= 0)
                        {
                            return Result<ExpressionTriggerViewModel>.FailedResult(string.Format("请指定选项【{0}】正确的值", item.ToString()));
                        }
                        expressionItem.BeginValue = monthWhichWeekDayNumber;
                        expressionItem.EndValue = monthWhichWeekDay;
                        break;
                }
                expressionItems.Add(expressionItem);
            }
            ExpressionTriggerViewModel expressionTrigger = trigger.MapTo<ExpressionTriggerViewModel>();
            expressionTrigger.ExpressionItems = expressionItems;
            var result = Result<ExpressionTriggerViewModel>.SuccessResult("初始化成功");
            result.Data = expressionTrigger;
            return result;
        }

        #endregion

        #region 初始化执行计划条件

        Result<TriggerConditionViewModel> InitTriggerCondition(TriggerViewModel trigger)
        {
            if (trigger == null || trigger.Condition == null)
            {
                return Result<TriggerConditionViewModel>.FailedResult("执行计划信息为空");
            }
            Result<TriggerConditionViewModel> conditionResult = null;
            switch (trigger.Condition.Type)
            {
                case TaskTriggerConditionType.不限制:
                default:
                    conditionResult = Result<TriggerConditionViewModel>.SuccessResult("没有条件限制");
                    break;
                case TaskTriggerConditionType.固定日期:
                    conditionResult = InitFullDateCondition(trigger);
                    break;
                case TaskTriggerConditionType.星期配置:
                    conditionResult = InitWeeklyCondition(trigger);
                    break;
                case TaskTriggerConditionType.每天时间段:
                    conditionResult = InitDaylyCondition(trigger);
                    break;
                case TaskTriggerConditionType.每年日期:
                    conditionResult = InitAnnualCondition(trigger);
                    break;
                case TaskTriggerConditionType.每月日期:
                    conditionResult = InitMontylyCondition(trigger);
                    break;
                case TaskTriggerConditionType.自定义:
                    conditionResult = InitExpressionCondition(trigger);
                    break;
            }
            return conditionResult;
        }

        /// <summary>
        /// 初始化年度计划
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        Result<TriggerConditionViewModel> InitAnnualCondition(TriggerViewModel trigger)
        {
            if (trigger == null)
            {
                return Result<TriggerConditionViewModel>.FailedResult("执行计划信息为空");
            }
            string[] yearMonthKeys = Request.Form.AllKeys.Where(c => c.StartsWith("Condition_YearDay_Month")).ToArray();
            List<AnnualConditionDayViewModel> dayList = new List<AnnualConditionDayViewModel>();
            foreach (var monthKey in yearMonthKeys)
            {
                string[] monthKeyArray = monthKey.LSplit("-");
                int keyIndex = 0;
                if (monthKeyArray.Length != 2 || !int.TryParse(monthKeyArray[1], out keyIndex) || keyIndex <= 0)
                {
                    continue;
                }
                int month = 0;
                int day = 0;
                bool enable = false;
                if (!int.TryParse(Request[monthKey], out month) || month <= 0 || month > 12)
                {
                    return Result<TriggerConditionViewModel>.FailedResult("请设置正确的月份");
                }
                if (!int.TryParse(Request["Condition_YearDay_Day-" + keyIndex], out day) || day <= 0 || day > 31)
                {
                    return Result<TriggerConditionViewModel>.FailedResult("请设置正确的日期");
                }
                if (!bool.TryParse(Request["Condition_YearDay_Enable-" + keyIndex], out enable))
                {
                    return Result<TriggerConditionViewModel>.FailedResult("请设置正确的日期执行方式");
                }
                dayList.Add(new AnnualConditionDayViewModel()
                {
                    Month = month,
                    Day = day,
                    Include = enable
                });
            }
            if (dayList.IsNullOrEmpty())
            {
                return Result<TriggerConditionViewModel>.FailedResult("请至少设置一条日期信息");
            }
            TriggerAnnualConditionViewModel annualCondition = new TriggerAnnualConditionViewModel()
            {
                Type = TaskTriggerConditionType.每年日期,
                Days = dayList
            };
            var result = Result<TriggerConditionViewModel>.SuccessResult("初始化成功");
            result.Data = annualCondition;
            return result;
        }

        /// <summary>
        /// 初始化月份计划
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        Result<TriggerConditionViewModel> InitMontylyCondition(TriggerViewModel trigger)
        {
            if (trigger == null)
            {
                return Result<TriggerConditionViewModel>.FailedResult("执行计划信息为空");
            }
            string[] monthDayKeys = Request.Form.AllKeys.Where(c => c.StartsWith("Condition_MonthDay_Day")).ToArray();
            List<MonthConditionDayViewModel> days = new List<MonthConditionDayViewModel>();
            foreach (string dayKey in monthDayKeys)
            {
                if (dayKey.IsNullOrEmpty())
                {
                    continue;
                }
                string[] keyArray = dayKey.LSplit("-");
                int keyIndex = 0;
                if (keyArray.Length != 2 || !int.TryParse(keyArray[1], out keyIndex) || keyIndex <= 0)
                {
                    continue;
                }
                int day = 0;
                bool enable = false;
                if (!int.TryParse(Request[dayKey], out day) || day <= 0 || day > 31)
                {
                    return Result<TriggerConditionViewModel>.FailedResult("请设置正确的日期");
                }
                if (!bool.TryParse(Request["Condition_MonthDay_Enable-" + keyIndex], out enable))
                {
                    return Result<TriggerConditionViewModel>.FailedResult("请设置正确的日期执行方式");
                }
                days.Add(new MonthConditionDayViewModel()
                {
                    Day = day,
                    Include = enable
                });
            }
            if (days.IsNullOrEmpty())
            {
                return Result<TriggerConditionViewModel>.FailedResult("请至少设置一条日期信息");
            }
            TriggerMonthlyConditionViewModel monthlyCondition = new TriggerMonthlyConditionViewModel()
            {
                Days = days,
                Type = TaskTriggerConditionType.每月日期
            };
            var result = Result<TriggerConditionViewModel>.SuccessResult("初始化成功");
            result.Data = monthlyCondition;
            return result;
        }

        /// <summary>
        /// 初始化固定日期计划
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        Result<TriggerConditionViewModel> InitFullDateCondition(TriggerViewModel trigger)
        {
            if (trigger == null)
            {
                return Result<TriggerConditionViewModel>.FailedResult("执行计划信息为空");
            }
            string[] dayKeys = Request.Form.AllKeys.Where(c => c.StartsWith("Condition_FullDate_Date")).ToArray();
            List<FullDateConditionDateViewModel> days = new List<FullDateConditionDateViewModel>();
            foreach (var dayKey in dayKeys)
            {
                string[] keyArray = dayKey.LSplit("-");
                int keyIndex = 0;
                if (keyArray.Length != 2 || !int.TryParse(keyArray[1], out keyIndex) || keyIndex <= 0)
                {
                    continue;
                }
                DateTime date = DateTime.Now;
                bool enable = false;
                if (!DateTime.TryParse(Request[dayKey], out date))
                {
                    return Result<TriggerConditionViewModel>.FailedResult("请设置正确的日期");
                }
                if (!bool.TryParse(Request["Condition_FullDate_Enable-" + keyIndex], out enable))
                {
                    return Result<TriggerConditionViewModel>.FailedResult("请设置正确的日期执行方式");
                }
                days.Add(new FullDateConditionDateViewModel()
                {
                    Date = date,
                    Include = enable
                });
            }
            TriggerFullDateConditionViewModel fullDateCondition = new TriggerFullDateConditionViewModel()
            {
                Dates = days,
                Type = TaskTriggerConditionType.固定日期
            };
            var result = Result<TriggerConditionViewModel>.SuccessResult("初始化成功");
            result.Data = fullDateCondition;
            return result;
        }

        /// <summary>
        /// 初始化每天时间段计划
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        Result<TriggerConditionViewModel> InitDaylyCondition(TriggerViewModel trigger)
        {
            if (trigger == null)
            {
                return Result<TriggerConditionViewModel>.FailedResult("执行计划信息为空");
            }
            string beginTime = Request["Condition_TimeRange_BeginTime"];
            string endTime = Request["Condition_TimeRange_EndTime"];
            if (beginTime.IsNullOrEmpty() || endTime.IsNullOrEmpty())
            {
                return Result<TriggerConditionViewModel>.FailedResult("请设置完整的时间区间");
            }
            bool inversion = false;
            bool.TryParse(Request["Condition_TimeRange_Inversion"], out inversion);
            TriggerDailyConditionViewModel daylyCondition = new TriggerDailyConditionViewModel()
            {
                BeginTime = beginTime,
                EndTime = endTime,
                Inversion = inversion,
                Type = TaskTriggerConditionType.每天时间段
            };
            var result = Result<TriggerConditionViewModel>.SuccessResult("初始化成功");
            result.Data = daylyCondition;
            return result;
        }

        /// <summary>
        /// 初始化星期计划
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        Result<TriggerConditionViewModel> InitWeeklyCondition(TriggerViewModel trigger)
        {
            if (trigger == null)
            {
                return Result<TriggerConditionViewModel>.FailedResult("执行计划信息为空");
            }
            string[] weekDayKeys = Request.Form.AllKeys.Where(c => c.StartsWith("Condition_Week_Day")).ToArray();
            List<WeeklyConditionDayViewModel> days = new List<WeeklyConditionDayViewModel>();
            foreach (var dayKey in weekDayKeys)
            {
                if (dayKey.IsNullOrEmpty())
                {
                    continue;
                }
                string[] keyArray = dayKey.LSplit("-");
                int keyIndex = 0;
                if (keyArray.Length != 2 || !int.TryParse(keyArray[1], out keyIndex) || keyIndex <= 0)
                {
                    continue;
                }
                int weekDay = 0;
                bool enable = false;
                if (!int.TryParse(Request[dayKey], out weekDay) || !Enum.IsDefined(typeof(TaskWeek), weekDay))
                {
                    return Result<TriggerConditionViewModel>.FailedResult("请选择正确的日期");
                }
                if (!bool.TryParse(Request["Condition_Week_Enable-" + keyIndex], out enable))
                {
                    return Result<TriggerConditionViewModel>.FailedResult("请设置正确的日期执行方式");
                }
                days.Add(new WeeklyConditionDayViewModel()
                {
                    Day = weekDay,
                    Include = enable
                });
            }
            if (days.IsNullOrEmpty())
            {
                return Result<TriggerConditionViewModel>.FailedResult("请至少设置一条日期信息");
            }
            TriggerWeeklyConditionViewModel weeklyCondition = new TriggerWeeklyConditionViewModel()
            {
                Days = days,
                Type = TaskTriggerConditionType.星期配置
            };
            var result = Result<TriggerConditionViewModel>.SuccessResult("初始化成功");
            result.Data = weeklyCondition;
            return result;
        }

        /// <summary>
        /// 初始化自定义计划
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        Result<TriggerConditionViewModel> InitExpressionCondition(TriggerViewModel trigger)
        {
            if (trigger == null)
            {
                return Result<TriggerConditionViewModel>.FailedResult("执行计划信息为空");
            }
            var options = Enum.GetValues(typeof(TaskTriggerExpressionItem));
            List<ExpressionItemViewModel> expressionItems = new List<ExpressionItemViewModel>();
            foreach (TaskTriggerExpressionItem item in options)
            {
                string valueTypeKey = string.Format("Condition_ValueType_{0}", (int)item);
                int valueTypeVal = 0;
                int.TryParse(Request[valueTypeKey], out valueTypeVal);
                if (!Enum.IsDefined(typeof(TaskTriggerExpressionItemConfigType), valueTypeVal))
                {
                    continue;
                }
                TaskTriggerExpressionItemConfigType valueType = (TaskTriggerExpressionItemConfigType)valueTypeVal;
                ExpressionItemViewModel expressionItem = new ExpressionItemViewModel()
                {
                    ValueType = valueType,
                    Option = item,
                };
                switch (valueType)
                {
                    case TaskTriggerExpressionItemConfigType.集合值:
                        List<string> arrayValues = new List<string>();
                        if (item == TaskTriggerExpressionItem.星期)
                        {
                            arrayValues = Request["Condition_Week_ArrayVal"]?.Trim().LSplit(",").ToList();
                        }
                        else
                        {
                            arrayValues = Request["Condition_ArrayValue_" + (int)item]?.Trim().LSplit(",").ToList();
                        }
                        if (arrayValues.IsNullOrEmpty())
                        {
                            return Result<TriggerConditionViewModel>.FailedResult(string.Format("选项【{0}】集合值为空", item.ToString()));
                        }
                        expressionItem.BeginValue = 0;
                        expressionItem.EndValue = 0;
                        expressionItem.ArrayValue = arrayValues;
                        break;
                    case TaskTriggerExpressionItemConfigType.范围值:
                        int beginValue = 0;
                        int endValue = 0;
                        string beginVal = Request["Condition_BeginValue_" + (int)item]?.Trim();
                        string endVal = Request["Condition_EndValue_" + (int)item]?.Trim();
                        if (!int.TryParse(beginVal, out beginValue) || !int.TryParse(endVal, out endValue))
                        {
                            return Result<TriggerConditionViewModel>.FailedResult(string.Format("请填写选项【{0}】正确的范围值", item.ToString()));
                        }
                        if (beginValue > endValue)
                        {
                            return Result<TriggerConditionViewModel>.FailedResult(string.Format("选项【{0}】范围起始值不能大于结束值", item.ToString()));
                        }
                        if (item == TaskTriggerExpressionItem.星期)
                        {
                            Type weekType = typeof(TaskWeek);
                            if (!Enum.IsDefined(weekType, beginValue) || !Enum.IsDefined(weekType, endValue))
                            {
                                return Result<TriggerConditionViewModel>.FailedResult(string.Format("请指定选项【{0}】正确的范围值", item.ToString()));
                            }
                        }
                        expressionItem.BeginValue = beginValue;
                        expressionItem.EndValue = endValue;
                        break;
                    case TaskTriggerExpressionItemConfigType.开始_间隔:
                        int intervalBeginValue = 0;
                        int intervalValue = 0;
                        string intervalBeginVal = Request["Condition_IntervalBeginValue_" + (int)item]?.Trim();
                        string intervalVal = Request["Condition_IntervalSplitValue_" + (int)item]?.Trim();
                        if (!int.TryParse(intervalBeginVal, out intervalBeginValue) || !int.TryParse(intervalVal, out intervalValue))
                        {
                            return Result<TriggerConditionViewModel>.FailedResult(string.Format("请填写选项【{0}】正确的起始/间隔值", item.ToString()));
                        }
                        if (intervalValue <= 0)
                        {
                            return Result<TriggerConditionViewModel>.FailedResult(string.Format("请填写选项【{0}】正确的执行间隔值", item.ToString()));
                        }
                        expressionItem.BeginValue = intervalBeginValue;
                        expressionItem.EndValue = intervalValue;
                        break;
                    case TaskTriggerExpressionItemConfigType.每月倒数第N天:
                        int monthEndDay = 0;
                        string monthEndDayVal = Request["Condition_MonthEndDay_" + (int)item]?.Trim();
                        if (!int.TryParse(monthEndDayVal, out monthEndDay) || monthEndDay <= 0 || monthEndDay > 31)
                        {
                            return Result<TriggerConditionViewModel>.FailedResult(string.Format("请填写选项【{0}】正确的倒数天数", item.ToString()));
                        }
                        expressionItem.BeginValue = monthEndDay;
                        break;
                    case TaskTriggerExpressionItemConfigType.每月个最后一个星期N:
                        int monthLastWeekDay = 0;
                        string monthLastWeekDayVal = Request["Condition_MonthLastWeekDay_" + (int)item]?.Trim();
                        if (!int.TryParse(monthLastWeekDayVal, out monthLastWeekDay) || !Enum.IsDefined(typeof(TaskWeek), monthLastWeekDay))
                        {
                            return Result<TriggerConditionViewModel>.FailedResult(string.Format("请指定选项【{0}】正确的值", item.ToString()));
                        }
                        expressionItem.BeginValue = monthLastWeekDay;
                        break;
                    case TaskTriggerExpressionItemConfigType.本月第M个星期N:
                        int monthWhichWeekDayNumber = 0;
                        int monthWhichWeekDay = 0;
                        string monthWhichWeekDayNumberVal = Request["Condition_MonthWhichWeekDay_Num_" + (int)item]?.Trim();
                        string monthWhichWeekDayVal = Request["Condition_MonthWhichWeekDay_" + (int)item]?.Trim();
                        if (!int.TryParse(monthWhichWeekDayNumberVal, out monthWhichWeekDayNumber) || !int.TryParse(monthWhichWeekDayVal, out monthWhichWeekDay) || !Enum.IsDefined(typeof(TaskWeek), monthWhichWeekDay) || monthWhichWeekDayNumber <= 0)
                        {
                            return Result<TriggerConditionViewModel>.FailedResult(string.Format("请指定选项【{0}】正确的值", item.ToString()));
                        }
                        expressionItem.BeginValue = monthWhichWeekDayNumber;
                        expressionItem.EndValue = monthWhichWeekDay;
                        break;
                }
                expressionItems.Add(expressionItem);
            }
            TriggerExpressionConditionViewModel expressionCondition = new TriggerExpressionConditionViewModel()
            {
                Type = TaskTriggerConditionType.自定义,
                ExpressionItems = expressionItems
            };
            var result = Result<TriggerConditionViewModel>.SuccessResult("初始化成功");
            result.Data = expressionCondition;
            return result;
        }

        #endregion

        #region 应用对象信息

        /// <summary>
        /// 初始化应用对象信息
        /// </summary>
        /// <param name="trigger">执行计划</param>
        /// <returns></returns>
        Result<List<TriggerServerViewModel>> InitTriggerServer(TriggerViewModel trigger)
        {
            if (trigger == null)
            {
                return Result<List<TriggerServerViewModel>>.FailedResult("执行计划信息为空");
            }
            if (trigger.ApplyTo != TaskTriggerApplyTo.服务)
            {
                return Result<List<TriggerServerViewModel>>.SuccessResult("初始化成功");
            }
            string[] serverNodeKeys = Request.Form.AllKeys.Where(c => c.StartsWith("Trigger_Server_") && !c.EndsWith("State")).ToArray();
            List<TriggerServerViewModel> serverList = new List<TriggerServerViewModel>();
            foreach (string serverKey in serverNodeKeys)
            {
                string serverId = Request[serverKey];
                if (serverId.IsNullOrEmpty())
                {
                    continue;
                }
                int runState = 0;
                if (!int.TryParse(Request[serverKey + "_State"], out runState) || !Enum.IsDefined(typeof(TaskTriggerServerRunState), runState))
                {
                    return Result<List<TriggerServerViewModel>>.FailedResult("请执行正确的运行状态");
                }
                serverList.Add(new TriggerServerViewModel()
                {
                    Server = new ServerNodeViewModel()
                    {
                        Id = serverId
                    },
                    RunState = (TaskTriggerServerRunState)runState
                });
            }
            if (serverList.IsNullOrEmpty())
            {
                return Result<List<TriggerServerViewModel>>.FailedResult("请至少设置一条服务信息");
            }
            var result = Result<List<TriggerServerViewModel>>.SuccessResult("初始化成功");
            result.Data = serverList;
            return result;
        }

        #endregion

        #region 任务&服务执行计划

        public ActionResult ServerScheduleTriggerList(string jobId, string serverCode)
        {
            ViewBag.JobId = jobId;
            ViewBag.ServerCode = serverCode;
            return View();
        }

        [HttpPost]
        public ActionResult SearchServerScheduleTrigger(string jobId, string serverCode)
        {
            ServerScheduleTriggerFilterDto filter = new ServerScheduleTriggerFilterDto()
            {
                Job = jobId,
                ServerFilter = new ServerNodeFilterDto()
                {
                    Ids = new List<string>()
                    {
                        serverCode
                    }
                }
            };
            List<TriggerViewModel> triggerList = triggerService.GetTriggerList(filter).Select(c => c.MapTo<TriggerViewModel>()).ToList();

            #region 调度信息

            List<TriggerServerViewModel> triggerServerList = null;
            if (!triggerList.IsNullOrEmpty())
            {
                triggerServerList = triggerServerService.GetTriggerServerList(new TriggerServerFilterDto()
                {
                    Triggers = triggerList.Select(c => c.Id).ToList(),
                    Servers = new List<string>()
                {
                    serverCode
                },

                }).Select(c => c.MapTo<TriggerServerViewModel>()).ToList();
                triggerServerList.ForEach(c =>
                {
                    c.Trigger = triggerList.FirstOrDefault(t => t.Id == c.Trigger?.Id);
                });
            }

            #endregion

            var result = new
            {
                Datas = JsonSerialize.ObjectToJson(triggerServerList)
            };
            return Json(result);
        }

        public ActionResult TriggerHostServerList(string triggerId)
        {
            if (triggerId.IsNullOrEmpty())
            {
                return Content("获取信息失败");
            }
            var trigger = triggerService.GetTrigger(new TriggerFilterDto()
            {
                Ids = new List<string>()
                {
                    triggerId
                }
            }).MapTo<TriggerViewModel>();
            if (trigger == null)
            {
                return Content("获取信息失败");
            }
            return View(trigger);
        }

        [HttpPost]
        public ActionResult SearchTriggerHostServer(TriggerServerFilterViewModel filter)
        {
            IEnumerable<TriggerServerViewModel> triggerServerList = triggerServerService.GetTriggerServerList(filter.MapTo<TriggerServerFilterDto>()).Select(c => c.MapTo<TriggerServerViewModel>()).ToList();
            IEnumerable<string> serverIds = triggerServerList.Select(c => c.Server?.Id).Distinct();
            ServerNodeFilterDto serverFilter = new ServerNodeFilterDto()
            {
                Ids = serverIds.ToList()
            };
            List<ServerNodeViewModel> serverList = serverNodeService.GetServerNodeList(serverFilter).Select(c => c.MapTo<ServerNodeViewModel>()).ToList();
            if (!serverList.IsNullOrEmpty())
            {
                foreach (var triggerServer in triggerServerList)
                {
                    triggerServer.Server = serverList.FirstOrDefault(c => c.Id == triggerServer.Server?.Id);
                }
            }
            object objResult = new
            {
                Datas = JsonSerialize.ObjectToJson(triggerServerList)
            };
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SearchTriggerHostServerByJob(JobServerHostFilterViewModel filter)
        {
            JobServerHostFilterDto filterDto = filter.MapTo<JobServerHostFilterDto>();
            var serverHostPager = jobServerHostService.GetJobServerHostPaging(filterDto);
            //服务信息
            List<string> serverIds = serverHostPager.Select(c => c.Server?.Id).Distinct().ToList();
            ServerNodeFilterDto serverFilter = new ServerNodeFilterDto()
            {
                Ids = serverIds
            };
            var serverList = serverNodeService.GetServerNodeList(serverFilter);
            var triggerServerList = serverList.Select(c => new TriggerServerViewModel()
            {
                RunState = TaskTriggerServerRunState.运行,
                Server = c.MapTo<ServerNodeViewModel>()
            });
            object objResult = new
            {
                Datas = JsonSerialize.ObjectToJson(triggerServerList)
            };
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 执行计划多选

        public ActionResult TriggerMultipleSelect(TriggerFilterViewModel filter)
        {
            ViewBag.TriggerFilter = filter;
            return View();
        }

        public ActionResult TriggerMultipleSelectSearch(TriggerFilterViewModel filter)
        {
            filter.Ids = filter.Ids?.Where(c => !c.IsNullOrEmpty()).ToList();
            IPaging<TriggerViewModel> triggerPager = triggerService.GetTriggerPaging(filter.MapTo<TriggerFilterDto>()).ConvertTo<TriggerViewModel>();
            object objResult = new
            {
                TotalCount = triggerPager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(triggerPager.ToList())
            };
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region 计划服务

        [HttpPost]
        public ActionResult GetTriggerServers(TriggerServerFilterViewModel filter)
        {
            IPaging<TriggerServerViewModel> triggerServerPager = triggerServerService.GetTriggerServerPaging(filter.MapTo<TriggerServerFilterDto>()).ConvertTo<TriggerServerViewModel>();
            IEnumerable<string> serverIds = triggerServerPager.Select(c => c.Server?.Id).Distinct();
            ServerNodeFilterDto serverFilter = new ServerNodeFilterDto()
            {
                Ids = serverIds.ToList()
            };
            List<ServerNodeViewModel> serverList = serverNodeService.GetServerNodeList(serverFilter).Select(c => c.MapTo<ServerNodeViewModel>()).ToList();
            if (!serverList.IsNullOrEmpty())
            {
                foreach (var triggerServer in triggerServerPager)
                {
                    triggerServer.Server = serverList.FirstOrDefault(c => c.Id == triggerServer.Server?.Id);
                }
            }
            object objResult = new
            {
                TotalCount = triggerServerPager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(triggerServerPager.ToList())
            };
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetTriggerServersByJob(JobServerHostFilterViewModel filter)
        {
            JobServerHostFilterDto filterDto = filter.MapTo<JobServerHostFilterDto>();
            var serverHostPager = jobServerHostService.GetJobServerHostPaging(filterDto);
            //服务信息
            List<string> serverIds = serverHostPager.Select(c => c.Server?.Id).Distinct().ToList();
            ServerNodeFilterDto serverFilter = new ServerNodeFilterDto()
            {
                Ids = serverIds
            };
            var serverList = serverNodeService.GetServerNodeList(serverFilter);
            var triggerServerList = serverList.Select(c => new TriggerServerViewModel()
            {
                RunState = TaskTriggerServerRunState.运行,
                Server = c.MapTo<ServerNodeViewModel>()
            });
            object objResult = new
            {
                Datas = JsonSerialize.ObjectToJson(triggerServerList.ToList())
            };
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteTriggerServers(IEnumerable<TriggerServerViewModel> triggerServers)
        {
            DeleteTriggerServerCmdDto deleteInfo = new DeleteTriggerServerCmdDto()
            {
                TriggerServers = triggerServers.Select(c => c.MapTo<TriggerServerCmdDto>()).ToList()
            };
            return Json(triggerServerService.DeleteTriggerServer(deleteInfo));
        }

        [HttpPost]
        public ActionResult ModifyTriggerServerState(IEnumerable<TriggerServerViewModel> triggerServers)
        {
            if (triggerServers.IsNullOrEmpty())
            {
                return Json(Result.FailedResult("没有指定要操作的信息"));
            }
            ModifyTriggerServerRunStateCmdDto stateInfo = new ModifyTriggerServerRunStateCmdDto()
            {
                TriggerServers = triggerServers.Select(c => c.MapTo<TriggerServerCmdDto>()).ToList()
            };
            return Json(triggerServerService.ModifyRunState(stateInfo));
        }

        [HttpPost]
        public ActionResult SaveTriggerServer(IEnumerable<TriggerServerViewModel> triggerServers)
        {
            if (triggerServers.IsNullOrEmpty())
            {
                return Json(Result.FailedResult("没有指定任何要保存的信息"));
            }
            return Json(triggerServerService.SaveTriggerServer(new SaveTriggerServerCmdDto()
            {
                TriggerServers = triggerServers.Select(c => c.MapTo<TriggerServerCmdDto>()).ToList()
            }));
        }

        #endregion

        #region 执行日志

        [HttpPost]
        public ActionResult SearchJobExecuteLog(ExecuteLogFilterViewModel filter)
        {
            filter.LoadServer = true;
            filter.LoadTrigger = true;
            IPaging<ExecuteLogViewModel> executeLogPager = executeLogService.GetExecuteLogPaging(filter.MapTo<ExecuteLogFilterDto>()).ConvertTo<ExecuteLogViewModel>();
            object objResult = new
            {
                TotalCount = executeLogPager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(executeLogPager.ToList())
            };
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SearchTriggerExecuteLog(ExecuteLogFilterViewModel filter)
        {
            filter.LoadServer = true;
            IPaging<ExecuteLogViewModel> executeLogPager = executeLogService.GetExecuteLogPaging(filter.MapTo<ExecuteLogFilterDto>()).ConvertTo<ExecuteLogViewModel>();
            object objResult = new
            {
                TotalCount = executeLogPager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(executeLogPager.ToList())
            };
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SearchServerExecuteLog(ExecuteLogFilterViewModel filter)
        {
            filter.LoadTrigger = true;
            filter.LoadJob = true;
            IPaging<ExecuteLogViewModel> executeLogPager = executeLogService.GetExecuteLogPaging(filter.MapTo<ExecuteLogFilterDto>()).ConvertTo<ExecuteLogViewModel>();
            object objResult = new
            {
                TotalCount = executeLogPager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(executeLogPager.ToList())
            };
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 错误日志

        [HttpPost]
        public ActionResult SearchJobErrorLog(ErrorLogFilterViewModel filter)
        {
            filter.LoadServer = true;
            IPaging<ErrorLogViewModel> errorLogPager = errorLogService.GetErrorLogPaging(filter.MapTo<ErrorLogFilterDto>()).ConvertTo<ErrorLogViewModel>();
            object objResult = new
            {
                TotalCount = errorLogPager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(errorLogPager.ToList())
            };
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SearchServerErrorLog(ErrorLogFilterViewModel filter)
        {
            filter.LoadJob = true;
            IPaging<ErrorLogViewModel> errorLogPager = errorLogService.GetErrorLogPaging(filter.MapTo<ErrorLogFilterDto>()).ConvertTo<ErrorLogViewModel>();
            object objResult = new
            {
                TotalCount = errorLogPager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(errorLogPager.ToList())
            };
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ErrorLogDetail(long id)
        {
            if (id <= 0)
            {
                return Content("获取信息失败");
            }
            ErrorLogFilterDto filter = new ErrorLogFilterDto()
            {
                Ids = new List<long>()
                {
                    id
                },
                LoadJob = true,
                LoadServer = true
            };
            ErrorLogViewModel log = errorLogService.GetErrorLog(filter).MapTo<ErrorLogViewModel>();
            if (log == null)
            {
                return Content("获取信息失败");
            }
            return View(log);
        }


        public ActionResult ErrorLogList()
        {
            return View();
        }

        public ActionResult SearchErrorLog(ErrorLogFilterViewModel filter)
        {
            filter.LoadJob = true;
            filter.LoadServer = true;
            IPaging<ErrorLogViewModel> errorLogPager = errorLogService.GetErrorLogPaging(filter.MapTo<ErrorLogFilterDto>()).ConvertTo<ErrorLogViewModel>();
            object objResult = new
            {
                TotalCount = errorLogPager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(errorLogPager.ToList())
            };
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 工作文件

        public ActionResult SearchJobFile(JobFileFilterViewModel filter)
        {
            IPaging<JobFileViewModel> jobFilePager = jobFileService.GetJobFilePaging(filter.MapTo<JobFileFilterDto>()).ConvertTo<JobFileViewModel>();
            object objResult = new
            {
                TotalCount = jobFilePager.TotalCount,
                Datas = JsonSerialize.ObjectToJson(jobFilePager.ToList())
            };
            return Json(objResult, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}