using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAC.Model
{
    public class SPreProjectcs
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
        /// 路条批复文件
        /// </summary>
        public string LApprove { get; set; }

        /// <summary>
        /// 路条批复日期
        /// </summary>
        public string LAppTime { get; set; }

        /// <summary>
        /// 光伏电站核准批复文件
        /// </summary>
        public string SApprove { get; set; }

        /// <summary>
        /// 光伏电站核准批复日期
        /// </summary>
        public string SAppTime { get; set; }

        /// <summary>
        /// 接入系统批复文件
        /// </summary>
        public string JoinApprove { get; set; }

        /// <summary>
        /// 接入电网点
        /// </summary>
        public string JoinGrid { get; set; }

        /// <summary>
        /// 接入电网电压等级
        /// </summary>
        public string JoinVoltage { get; set; }

        /// <summary>
        /// 太阳能规划装机容量
        /// </summary>
        public string PlanCapacity { get; set; }

        /// <summary>
        /// 本期工程装机容量
        /// </summary>
        public string PeriodCapacity { get; set; }

        /// <summary>
        /// 年平均辐射照度
        /// </summary>
        public string YearAvgRadiation { get; set; }

        /// <summary>
        /// 设计利用小时数
        /// </summary>
        public string DesignHours { get; set; }

        /// <summary>
        /// 工程静态总投资
        /// </summary>
        public string StaticInvest { get; set; }

        /// <summary>
        /// 工程动态总投资
        /// </summary>
        public string DynamicInvest { get; set; }

        /// <summary>
        /// 核准批复上网电价
        /// </summary>
        public string Price { get; set; }
    }
}
