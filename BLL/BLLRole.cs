using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DAL;
using SAC.Helper;

namespace BLL
{
    public class BLLRole
    {
        DALRole dr = new DALRole();
        DateHelper pb = new DateHelper();
        #region 角色管理
        //获取所有角色信息
        public DataTable GetRoleList()
        {
            return dr.GetRoleList();
        }
        //返回所有角色信息
        public DataTable GetAllRole(int sCount, int eCount)
        {
            return dr.GetAllRole(sCount, eCount);
        }
        //返回所有角色信息的条数
        public int GetRoleCount()
        {
            return dr.GetRoleCount();
        }
        //保存新的角色信息
        public bool SaveRole(string rId, string rName, out string errMsg)
        {
            return dr.SaveRole(rId, rName, out errMsg);
        }
        //编辑原有的角色信息
        public bool UpDateRole(string OrId, string rId, string rName, out string errMsg)
        {
            return dr.UpDateRole(OrId, rId, rName, out errMsg);
        }
        //删除原有的角色信息
        public bool DeleteRole(string rId, out string errMsg)
        {
            return dr.DeleteRole(rId, out errMsg);
        }
        #endregion

        #region 权限管理
        
        public IList<Hashtable> GetMembers()
        {
            return pb.DataTableToList(dr.GetMembers());
        }
        //判断某个岗位下面是否存在人员
        public bool JudgMemberByORGId(string id)
        {
            return dr.JudgMemberByORGId(id);
        }
        #endregion

        #region 角色人员管理

        #region 根据每页显示多少条数据，角色ID返回用户信息
        public DataTable GetUserMenuByRole(string roleId, int sCount, int eCount)
        {
            return dr.GetUserMenuByRole(roleId, sCount, eCount);
        }
        #endregion
        #region 根据角色ID返回所有用户信息的条数
        public int GetUserCountByRole(string roleId)
        {
            return dr.GetUserCountByRole(roleId);
        }
        #endregion
        #region 根据用户名判断是否存在该人员
        public bool JudgMember(string userId)
        {
            return dr.JudgMember(userId);
        }
        #endregion
        #region 添加人员
        public bool AddMember(string id, string name, string pwd, byte[] img, string orgID)
        {
            return dr.AddMember(id, name, pwd, img, orgID);
        }
        #endregion
        #region 返回人员信息
        public IList<Hashtable> GetmemberInfo(string id, int i)
        {
            return pb.DataTableToList(dr.GetmemberInfo(id, i));
        }
        #endregion
        #region 编辑人员信息
        public bool EditMemberInfo(string userIDO, string userID, string userName, string pwd, byte[] img, string treeNodeId)
        {
            return dr.EditMemberInfo(userIDO, userID, userName, pwd, img, treeNodeId);
        }
        #endregion
        #region 删除人员
        /// <summary>
        /// 删除人员信息
        /// </summary>
        /// <param name="id">人员编码</param>
        /// <returns></returns>
        public bool RemoveMember(string id)
        {
            return dr.RemoveMember(id);
        }
        #endregion
        #endregion

        #region 菜单管理
        public bool UpdataWebMenuXml(byte[] fileBytes)
        {
            return dr.UpdataWebMenuXml(fileBytes);
        }
        public bool IsEmptyXml()
        {
            return dr.IsEmptyXml();
        }
        public bool DownLoadXml(string fileID, string filePath)
        {
            return dr.DownLoadXml(fileID, filePath);
        }
        #endregion

        #region 登陆使用
        public DataTable GetUserInfo(string userName)
        {
            return dr.GetUserInfo(userName);
        }
        public string GetRoleId(string userId)
        {
            return dr.GetRoleId(userId);
        }
        #endregion
    }
}
