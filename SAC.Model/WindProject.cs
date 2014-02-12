using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAC.Model
{
    /// <summary>
    /// 风电工程建设项目对象
    /// </summary>
    public class WindProject
    {
        /// <summary>
        /// 风电场工程名称
        /// </summary>
        public string WindProjName { get; set; }

        /// <summary>
        /// 首台风电机组基础开始施工
        /// </summary>
        public string FirstConstruct { get; set; }

        /// <summary>
        /// 首台风电机组开始吊装
        /// </summary>
        public string FirstSetup { get; set; }

        /// <summary>
        /// 升压站安装调试完成并反送电
        /// </summary>
        public string StationPower { get; set; }

        /// <summary>
        /// 首台风机调试完成并网发电
        /// </summary>
        public string FirstCombinedToGrid { get; set; }

        /// <summary>
        /// 全部风电机组整套启动开始250小时试运行
        /// </summary>
        public string TestRun { get; set; }

        /// <summary>
        /// 项目整体正式移交生产
        /// </summary>
        public string TransferProduction { get; set; }

        /// <summary>
        /// 风电场拟安装台数
        /// </summary>
        public string WCount { get; set; }

        /// <summary>
        /// 当月新增安装台数
        /// </summary>
        public string AddCount { get; set; }

        /// <summary>
        /// 当月底累积正式运行台数
        /// </summary>
        public string SumCount { get; set; }

        /// <summary>
        /// 当月工程建设投资数
        /// </summary>
        public string MonthInvest { get; set; }

        /// <summary>
        /// 累计工程投资数
        /// </summary>
        public string TotalInvest { get; set; }

        public int KeyId { get; set; }

        public string OrgId { get; set; }

        public string TreeId { get; set; }

        /// <summary>
        /// 记录更新人
        /// </summary>
        public string UpdateUser { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime
        {
            get { return DateTime.Now; }
            set { UpdateTime = value; }
        }

        /// <summary>
        /// 审批状态
        /// </summary>
        public int CheckStatus { get; set; }

        public string CheckUser { get; set; }

        public DateTime CheckTime { get; set; }
    }

    public class WindProjectData
    {
        public int total { get; set; }
        public string Msg { get; set; }
        public IList<WindProject> rows { get; set; }
    }
}
