using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAC.Model
{
    public class ReportStyle
    {
        public ReportStyle()
        {   }

        /// <summary>
        /// 报表ID
        /// </summary>
        public string RptID
        {
            get;
            set;
        }

        /// <summary>
        /// 报表名称
        /// </summary>
        public string RptName
        {
            get;
            set;
        }

        /// <summary>
        /// 报表样式
        /// </summary>
        public string RptStyle
        {
            get;
            set;
        }

        /// <summary>
        /// 组织机构ID 
        /// </summary>
        public string OrgId
        { 
            get; 
            set;
        }

        /// <summary>
        /// 组织机构树ID 
        /// </summary>
        public string TreeId 
        {
            get;
            set; 
        }

        /// <summary>
        /// 报表时间类型
        /// </summary>
        public string styleType
        {
            get;
            set;
        }
    }
}
