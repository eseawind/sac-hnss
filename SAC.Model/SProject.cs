using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAC.Model
{
    public class SProject
    {
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

        public DateTime CheckTime
        {
            get { return DateTime.Now; }
            set { CheckTime = value; }
        }

        /// <summary>
        /// 光伏电站工程名称
        /// </summary>
        public string SName { get; set; }

        /// <summary>
        /// 首个光伏发电单元基础开始施工
        /// </summary>
        public string FirstConstruct { get; set; }

        /// <summary>
        /// 首个光伏发电单元开始安装
        /// </summary>
        public string FirstSetup { get; set; }

        /// <summary>
        /// 升压站安装调试完成并反送电
        /// </summary>
        public string StationPower { get; set; }

        /// <summary>
        /// 首个光伏发电单元发电
        /// </summary>
        public string FirstUnitPower { get; set; }

        /// <summary>
        /// 全部光伏发电单元整套启动开始72小时并网试运行
        /// </summary>
        public string AllUnitPower { get; set; }

        /// <summary>
        /// 项目整体正式移交生产
        /// </summary>
        public string ProjectToProduct { get; set; }

        /// <summary>
        /// 当月新增安装容量
        /// </summary>
        public string AddCapacity { get; set; }

        /// <summary>
        /// 当月底累积正式运行容量
        /// </summary>
        public string TotalCapacity { get; set; }

        /// <summary>
        /// 当月工程建设投资数
        /// </summary>
        public string MonthInvest { get; set; }

        /// <summary>
        /// 累计工程投资数
        /// </summary>
        public string TotalInvest { get; set; }
    }
}


