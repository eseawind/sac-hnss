using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAC.Model
{
    /// <summary>
    /// AJAX请求结果类
    /// </summary>
    public class ExecResult
    {
        /// <summary>
        /// 结果集
        /// </summary>
        public string ResultData { get; set; }

        /// <summary>
        /// 结果状态(true or false)
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrMessage { get; set; }

        /// <summary>
        /// 自定义数据，存放任意需要的数据
        /// </summary>
        public object CustomInfo { get; set; }
    }
}
