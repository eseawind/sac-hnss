using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using System.Data;
using System.Collections;
namespace BLL
{
    public class FormBLL
    {
        FormDAL dal = new FormDAL();

        #region 创建数据表
        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <param name="tableName">数据表名称</param>
        /// <returns></returns>
        public string createTable(string tableName)
        {
            return dal.createTable(tableName);
        }
        #endregion

        #region 获取填报数据信息
        /// <summary>
        /// 获取填报数据信息
        /// </summary>
        /// // <param name="formID">表单ID</param>
        /// <returns></returns>
        public DataTable GetCreateInfo(string formID, string type)
        {
            return dal.GetCreateInfo(formID, type);
        }
        #endregion

        #region 为数据库表添加列
        /// <summary>
        /// 为数据库表添加列
        /// </summary>
        /// <param name="tableName">数据库表名称</param>
        /// <param name="columns">列集合</param>
        /// <returns></returns>
        public bool CreateColumns(string tableName, string columns)
        {
            return dal.CreateColumns(tableName, columns);
        }
        #endregion

        #region 填报数据
        /// <summary>
        /// 填报数据
        /// </summary>
        /// <param name="table">数据存储表名称</param>
        /// <param name="time">数据填报时间</param>
        /// <param name="timeName">数据存储表  时间字段名称</param>
        /// <param name="time">组织机构编号</param>
        /// <param name="value">填报的数据集</param>
        /// <param name="formID">表单编号</param>
        /// <returns></returns>
        public bool UpZBData(string table, string time, string timeName, string orgId, string value, string formID)
        {
            return dal.UpZBData(table, time, timeName, orgId, value, formID);
        }
        #endregion

        #region 填报数据  组织机构维度数据
        /// <summary>
        /// 填报数据  组织机构维度数据
        /// </summary>
        /// <param name="table">数据存储表名称</param>
        /// <param name="time">填报时间</param>
        /// <param name="timeName">数据存储表时间字段名称</param>
        /// <param name="id">填报字段名称集合</param>
        /// <param name="orgId">组织维度集合</param>
        /// <param name="value">填报数据集合</param>
        /// <returns></returns>
        public bool UpData(string table, string time, string timeName, string id, string orgId, string value)
        {
            return dal.UpData(table, time, timeName, id, orgId, value);
        }
        #endregion

        #region 获取填报参数  数据填报
        /// <summary>
        /// 获取组织机构数据  数据填报
        /// </summary>
        /// <param name="formId">表单编号</param>
        /// <param name="type">组织维度</param>
        /// <returns></returns>
        public DataTable GetDataParameter(string formId, string key)
        {
            return dal.GetDataParameter(formId, key);
        }
        #endregion

        #region 查询填报数据  指标维度
        /// <summary>
        /// 查询填报数据  指标维度
        /// </summary>
        /// <param name="columns">查询字段</param>
        /// <param name="tableName">查询表名称</param>
        /// <param name="timeType">时间字段名称</param>
        /// <param name="time">查询时间</param>
        /// <param name="orgId">组织机构ID</param>
        /// <returns></returns>
        public DataTable GetCreateValueZB(string columns, string tableName, string timeType, string time, string orgId)
        {
            return dal.GetCreateValueZB(columns, tableName, timeType, time, orgId);
        }
        #endregion

        #region 查询填报数据
        /// <summary>
        /// 查询填报数据
        /// </summary>
        /// <param name="columns">查询字段</param>
        /// <param name="tableName">查询表名称</param>
        /// <param name="timeType">时间字段名称</param>
        /// <param name="time">查询时间</param>
        /// /// <param name="treeName">组织机构树名称</param>
        /// <param name="orgId">组织机构ID</param>
        /// <param name="fid">表单编号</param>
        /// <returns></returns>
        public DataTable GetCreateValue(string columns, string tableName, string timeType, string time, string orgId, string fid)
        {
            return dal.GetCreateValue(columns, tableName, timeType, time, orgId, fid);
        }
        #endregion

        #region 获取填报公式等级
        /// <summary>
        /// 获取填报公式等级
        /// </summary>
        /// <param name="orgId">组织机构编码</param>
        /// <param name="formID">表单编号</param>
        /// <returns></returns>
        public DataTable GetFormGrade(string formID)
        {
            return dal.GetFormGrade(formID);
        }
        #endregion

        #region 搜索公式
        /// <summary>
        /// 搜索公式
        /// </summary>
        /// <param name="formID">表单编号</param>
        /// <param name="grade">优先级</param>
        /// <returns></returns>
        public DataTable GetFormGradeList(string formID, string grade)
        {
            return dal.GetFormGradeList(formID, grade);
        }
        #endregion

        #region 获取组织机构数据  数据填报
        /// <summary>
        /// 获取组织机构数据  数据填报
        /// </summary>
        /// <param name="formId">表单编号</param>
        /// <param name="type">组织维度</param>
        /// <returns></returns>
        public DataTable GetDataOrgInfo(string formId, string type)
        {
            return dal.GetDataOrgInfo(formId, type);
        }
        #endregion

        #region 获取表单数据类型
        /// <summary>
        /// 获取表单数据类型
        /// </summary>
        /// <param name="formID"></param>
        /// <returns></returns>
        public DataTable GetDataType(string formID)
        {
            return dal.GetDataType(formID);
        }
        #endregion

        #region 获取风电场站监测数据（风速和功率）
        /// <summary>
        /// 获取风电场站监测数据
        /// </summary>
        /// <param name="formID"></param>
        /// <returns></returns>
        public DataTable GetStationMonitor()
        {
            return dal.GetStationMonitor();
        }

        #endregion

        #region 获取标杆风电场站监测数据
        /// <summary>
        /// 获取标杆风电场站监测数据
        /// </summary>
        /// <param name="formID"></param>
        /// <returns></returns>
        public DataTable GetBGStationMonitor(string periodId)
        {
            return dal.GetBGStationMonitor(periodId);

        }
        #endregion

        #region 获取给定的风电场站风机数量
        /// <summary>
        /// 获取给定的风电场站风机数量
        /// </summary>
        /// <param name="formID"></param>
        /// <returns></returns>
        public DataTable GetBGCountStationMonitor(string periodId)
        {
            return dal.GetBGCountStationMonitor(periodId);
        }
        #endregion

        
        /// <summary>
        /// 获取风速功率测点 通过 工期ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetPointCompanyByPeriodID(string id)
        {
            return dal.GetPointCompanyByPeriodID(id);
        }

        /// <summary>
        /// 获取风场坐标
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetPointByPeriodID(string id)
        {
            return dal.GetPointByPeriodID(id);
        }
    }
}
