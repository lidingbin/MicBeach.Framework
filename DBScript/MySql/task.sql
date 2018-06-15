
--
-- Table structure for table `task_errorlog`
--

DROP TABLE IF EXISTS `task_errorlog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `task_errorlog` (
  `id` bigint(20) NOT NULL COMMENT '编号',
  `Server` varchar(50) DEFAULT NULL COMMENT '服务节点',
  `Job` varchar(50) DEFAULT NULL COMMENT '工作任务',
  `Trigger` varchar(50) DEFAULT NULL COMMENT '执行计划',
  `Message` varchar(1000) DEFAULT NULL COMMENT '错误消息',
  `Description` text COMMENT '错误描述',
  `Type` int(11) NOT NULL COMMENT '错误类型',
  `Date` datetime NOT NULL COMMENT '时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk COMMENT='任务异常日志';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `task_eventlog`
--

DROP TABLE IF EXISTS `task_eventlog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `task_eventlog` (
  `Id` bigint(20) NOT NULL COMMENT '编号',
  `Job` varchar(50) DEFAULT NULL COMMENT '工作',
  `Trigger` varchar(50) DEFAULT NULL COMMENT '触发器',
  `Server` varchar(50) DEFAULT NULL COMMENT '服务',
  `Message` varchar(200) NOT NULL,
  `Description` text,
  `CreateTime` datetime NOT NULL COMMENT '时间',
  `LogType` int(11) NOT NULL COMMENT '日志类型',
  `Level` int(11) NOT NULL COMMENT '等级',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk COMMENT='任务事件日志';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `task_executelog`
--

DROP TABLE IF EXISTS `task_executelog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `task_executelog` (
  `Id` bigint(20) NOT NULL COMMENT '编号',
  `Job` varchar(50) NOT NULL COMMENT '工作任务',
  `Trigger` varchar(50) NOT NULL COMMENT '触发器',
  `Server` varchar(50) NOT NULL COMMENT '执行节点',
  `BeginTime` datetime NOT NULL COMMENT '执行开始时间',
  `EndTime` datetime NOT NULL COMMENT '执行结束时间',
  `RecordTime` datetime NOT NULL COMMENT '记录时间',
  `State` int(11) NOT NULL COMMENT '执行状态',
  `Message` varchar(200) DEFAULT NULL COMMENT '执行消息',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk COMMENT='执行日志';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `task_job`
--

DROP TABLE IF EXISTS `task_job`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `task_job` (
  `Id` varchar(50) NOT NULL COMMENT '工作编号',
  `Group` varchar(50) NOT NULL COMMENT '工作分组',
  `Name` varchar(50) NOT NULL COMMENT '工作名称',
  `Type` int(11) NOT NULL COMMENT '任务类型',
  `RunType` int(11) DEFAULT '0' COMMENT '执行类型',
  `State` int(11) NOT NULL,
  `Description` varchar(100) DEFAULT NULL COMMENT '工作说明',
  `UpdateDate` datetime NOT NULL COMMENT '更新时间',
  `JobPath` varchar(500) DEFAULT NULL COMMENT '任务路径',
  `JobFileName` varchar(100) DEFAULT NULL COMMENT '工作文件名称',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk COMMENT='工作任务';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `task_jobgroup`
--

DROP TABLE IF EXISTS `task_jobgroup`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `task_jobgroup` (
  `Code` varchar(50) NOT NULL COMMENT '分组编码',
  `Name` varchar(50) NOT NULL COMMENT '分组名称',
  `Sort` int(11) NOT NULL DEFAULT '0' COMMENT '排序',
  `Parent` varchar(50) DEFAULT NULL COMMENT '上级',
  `Root` varchar(50) DEFAULT NULL COMMENT '根组',
  `Level` int(11) NOT NULL DEFAULT '0' COMMENT '等级',
  `Remark` varchar(50) DEFAULT NULL COMMENT '说明',
  PRIMARY KEY (`Code`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk COMMENT='工作分组';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `task_jobserverhost`
--

DROP TABLE IF EXISTS `task_jobserverhost`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `task_jobserverhost` (
  `Server` varchar(50) NOT NULL COMMENT '服务节点',
  `Job` varchar(50) NOT NULL COMMENT '任务',
  `RunState` int(11) DEFAULT '0' COMMENT '运行状态',
  PRIMARY KEY (`Server`,`Job`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk COMMENT='任务承载';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `task_servernode`
--

DROP TABLE IF EXISTS `task_servernode`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `task_servernode` (
  `Id` varchar(50) NOT NULL COMMENT '节点编号',
  `InstanceName` varchar(50) NOT NULL COMMENT '实例名称',
  `Name` varchar(50) NOT NULL COMMENT '名称',
  `State` int(11) NOT NULL COMMENT '状态',
  `Host` varchar(200) NOT NULL COMMENT '主机地址',
  `ThreadCount` int(11) NOT NULL COMMENT '线程数量',
  `ThreadPriority` varchar(45) DEFAULT NULL COMMENT '线程优先级',
  `Description` varchar(100) DEFAULT NULL COMMENT '说明',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk COMMENT='服务节点';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `task_trigger`
--

DROP TABLE IF EXISTS `task_trigger`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `task_trigger` (
  `Id` varchar(50) NOT NULL COMMENT '编号',
  `Name` varchar(50) NOT NULL COMMENT '名称',
  `Description` varchar(500) DEFAULT NULL COMMENT '说明',
  `Job` varchar(50) NOT NULL COMMENT '所属任务',
  `ApplyTo` int(11) NOT NULL COMMENT '引用对象',
  `PrevFireTime` datetime DEFAULT NULL COMMENT '上次执行时间',
  `NextFireTime` datetime DEFAULT NULL COMMENT '下次执行时间',
  `Priority` int(11) NOT NULL COMMENT '优先级',
  `State` int(11) NOT NULL COMMENT '状态',
  `Type` int(11) NOT NULL COMMENT '类型',
  `ConditionType` int(11) NOT NULL COMMENT '条件类型',
  `StartTime` datetime NOT NULL COMMENT '开始时间',
  `EndTime` datetime DEFAULT NULL COMMENT '结束时间',
  `MisFireType` int(11) NOT NULL COMMENT '触发失败操作类型',
  `FireTotalCount` int(11) NOT NULL COMMENT '执行次数',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk COMMENT='任务执行计划';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `task_triggerannualcondition`
--

DROP TABLE IF EXISTS `task_triggerannualcondition`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `task_triggerannualcondition` (
  `TriggerId` varchar(50) NOT NULL COMMENT '编号',
  `Month` int(11) NOT NULL COMMENT '月份',
  `Day` int(11) NOT NULL COMMENT '日期',
  `Include` bit(1) NOT NULL COMMENT '包含',
  PRIMARY KEY (`TriggerId`,`Month`,`Day`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk COMMENT='计划年度条件';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `task_triggerdailycondition`
--

DROP TABLE IF EXISTS `task_triggerdailycondition`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `task_triggerdailycondition` (
  `TriggerId` varchar(50) NOT NULL COMMENT '编号',
  `BeginTime` varchar(50) NOT NULL COMMENT '开始时间',
  `EndTime` varchar(50) NOT NULL COMMENT '结束时间',
  `Inversion` bit(1) NOT NULL COMMENT '排除设定日期',
  PRIMARY KEY (`TriggerId`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk COMMENT='计划时间计划条件';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `task_triggerexpression`
--

DROP TABLE IF EXISTS `task_triggerexpression`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `task_triggerexpression` (
  `TriggerId` varchar(50) NOT NULL COMMENT '编号',
  `Option` int(11) NOT NULL COMMENT '选项',
  `ValueType` int(11) NOT NULL COMMENT '值类型',
  `BeginValue` int(11) DEFAULT NULL COMMENT '其实值',
  `EndValue` int(11) DEFAULT NULL COMMENT '结束值',
  `ArrayValue` varchar(1000) DEFAULT NULL COMMENT '集合值',
  PRIMARY KEY (`TriggerId`,`Option`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk COMMENT='自定义触发计划';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `task_triggerexpressioncondition`
--

DROP TABLE IF EXISTS `task_triggerexpressioncondition`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `task_triggerexpressioncondition` (
  `TriggerId` varchar(50) NOT NULL COMMENT '编号',
  `ConditionOption` int(11) NOT NULL COMMENT '条件项',
  `ValueType` int(11) NOT NULL COMMENT '值类型',
  `BeginValue` int(11) DEFAULT NULL COMMENT '起始值',
  `EndValue` int(11) DEFAULT NULL COMMENT '结束值',
  `ArrayValue` varchar(50) DEFAULT NULL COMMENT '集合值',
  PRIMARY KEY (`TriggerId`,`ConditionOption`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk COMMENT='计划表达式附加条件';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `task_triggerfulldatecondition`
--

DROP TABLE IF EXISTS `task_triggerfulldatecondition`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `task_triggerfulldatecondition` (
  `TriggerId` varchar(50) NOT NULL COMMENT '编号',
  `Date` datetime NOT NULL COMMENT '日期',
  `Include` bit(1) NOT NULL COMMENT '包含日期',
  PRIMARY KEY (`TriggerId`,`Date`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk COMMENT='计划完整日期条件';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `task_triggermonthlycondition`
--

DROP TABLE IF EXISTS `task_triggermonthlycondition`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `task_triggermonthlycondition` (
  `TriggerId` varchar(50) NOT NULL COMMENT '编号',
  `Day` int(11) NOT NULL COMMENT '日期',
  `Include` bit(1) DEFAULT NULL COMMENT '包含当前日期',
  PRIMARY KEY (`TriggerId`,`Day`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk COMMENT='计划月份附加条件';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `task_triggerserver`
--

DROP TABLE IF EXISTS `task_triggerserver`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `task_triggerserver` (
  `Trigger` varchar(50) NOT NULL COMMENT '执行计划',
  `Server` varchar(50) NOT NULL COMMENT '服务节点',
  `RunState` int(11) NOT NULL COMMENT '运行状态',
  `LastFireDate` datetime DEFAULT NULL COMMENT '上次触发时间',
  `NextFireDate` datetime DEFAULT NULL COMMENT '下次触发事件',
  PRIMARY KEY (`Trigger`,`Server`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk COMMENT='执行计划服务节点';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `task_triggersimple`
--

DROP TABLE IF EXISTS `task_triggersimple`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `task_triggersimple` (
  `TriggerId` varchar(50) NOT NULL COMMENT '触发编号',
  `RepeatCount` int(11) NOT NULL COMMENT '0',
  `RepeatInterval` bigint(20) NOT NULL COMMENT '触发间隔',
  `RepeatForever` bit(1) NOT NULL DEFAULT b'0' COMMENT '持续运行',
  PRIMARY KEY (`TriggerId`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk COMMENT='简单执行计划';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `task_triggerweeklycondition`
--

DROP TABLE IF EXISTS `task_triggerweeklycondition`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `task_triggerweeklycondition` (
  `TriggerId` varchar(50) NOT NULL COMMENT '编号',
  `Day` int(11) NOT NULL COMMENT '日期',
  `Include` bit(1) DEFAULT NULL COMMENT '包含当前日期',
  PRIMARY KEY (`TriggerId`,`Day`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk COMMENT='计划星期条件';
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2017-12-11 16:45:19
