using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using OperateDatabase;
using DataEntity;
using ConfigEntity;
using GlobalObject;
using System.Diagnostics;
using System.Runtime.Remoting;

namespace DataLayer
{
    /// <summary>
    /// 数据层类厂
    /// </summary>
    public class SelectDataLayer : ISelectDataLayer
    {
        /// <summary>
        /// 数据库名称
        /// </summary>
        private string m_dbName;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SelectDataLayer()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbName">数据库名称</param>
        public SelectDataLayer(string dbName)
        {
            Debug.Assert(!string.IsNullOrEmpty(dbName), "数据库名称不允许为空");

            m_dbName = dbName;
        }

        #region ISelectDataLayer 成员

        /// <summary>
        /// 从数据集获取实体对象列表
        /// </summary>
        /// <typeparam name="T">要获取数据的类型</typeparam>
        /// <param name="model">实体集</param>
        /// <returns>成功返回获取到的实体对象列表，失败则抛出异常</returns>
        public T GetItem<T>(T model)
        {
            PropertyInfo[] listPi = model.GetType().GetProperties();

            string strWhere = "";

            foreach (PropertyInfo pi in listPi)
            {
                object obj = pi.GetValue(model, null);

                if (obj != null && !pi.PropertyType.Name.Contains("List"))
                {
                    strWhere = strWhere + " and " + pi.Name + " = '" + obj.ToString() + "'";
                }
            }

            if (strWhere.Length == 0)
            {
                return default(T);
            }

            strWhere = strWhere.Substring(4);

            List<T> list = GetItems<T>(strWhere);

            if (list == null || list.Count == 0)
            {
                return default(T);
            }
            else
            {
                return GetItems<T>(strWhere)[0];
            }
        }

        /// <summary>
        /// 获取通过查询条件获取到的实体集记录
        /// </summary>
        /// <typeparam name="T">要获取数据的类型</typeparam>
        /// <param name="strWhere">查询条件</param>
        /// <returns>成功则返回获取到的数据记录，失败则抛出异常</returns>
        public T GetItem<T>(string strWhere)
        {
            DataSet ds = GetItemsDataSet<T>(strWhere, "");

            if (ds == null || ds.Tables.Count == 0)
            {
                return default(T);
            }
            else
            {
                if (ds.Tables[0].Rows.Count == 0)
                {
                    return default(T);
                }
                else
                {
                    DataRow dr = GetItemsDataSet<T>(strWhere, "").Tables[0].Rows[0];
                    return GlobalObject.GeneralFunction.ReflectiveEntity<T>(dr);
                }
            }

        }

        /// <summary>
        /// 获取所有实体集记录（按默认排序）
        /// </summary>
        /// <typeparam name="T">要获取数据的类型</typeparam>
        /// <returns>成功返回获取到的实体对象列表，失败则抛出异常</returns>
        public List<T> GetItems<T>()
        {
            return GetItems<T>("", "");
        }

        /// <summary>
        /// 获取所有实体集记录
        /// </summary>
        /// <typeparam name="T">要获取数据的类型</typeparam>
        /// <param name="model">实体集</param>
        /// <param name="list">查询集</param>
        /// <returns>成功返回获取到的实体对象列表，失败则抛出异常</returns>
        public List<T> GetItems<T>(T model, List<T> list)
        {
            PropertyInfo[] listPi = model.GetType().GetProperties();

            foreach (PropertyInfo pi in listPi)
            {
                object obj = pi.GetValue(model, null);

                if (obj != null && !pi.PropertyType.Name.Contains("List"))
                {
                    list = (from a in list
                            where a.GetType().GetProperty(pi.Name).GetValue(a,null).ToString() == obj.ToString()
                           select a).ToList();
                }
            }

            return list;
        }

        /// <summary>
        /// 从数据集获取实体对象列表
        /// </summary>
        /// <typeparam name="T">要获取数据的类型</typeparam>
        /// <param name="model">实体集</param>
        /// <returns>成功返回获取到的实体对象列表，失败则抛出异常</returns>
        public List<T> GetItems<T>(T model)
        {
            PropertyInfo[] listPi = model.GetType().GetProperties();

            string strWhere = "";

            foreach (PropertyInfo pi in listPi)
            {
                object obj = pi.GetValue(model, null);

                if (obj != null && !pi.PropertyType.Name.Contains("List"))
                {
                    #region
                    //if (obj.GetType().Name.Contains("List"))
                    //{
                    //    IList listTemp = (IList)obj;

                    //    if (listTemp.Count == 0)
                    //    {
                    //        continue;
                    //    }
                    //}
                    #endregion

                    strWhere = strWhere + " and " + pi.Name + " = '" + obj.ToString() + "'";
                }
            }

            if (strWhere.Length == 0)
            {
                return null;
            }

            strWhere = strWhere.Substring(4);

            return GetItems<T>(strWhere);
        }

        /// <summary>
        /// 通过查询条件获取实体对象列表
        /// </summary>
        /// <typeparam name="T">要获取数据的类型</typeparam>
        /// <param name="strWhere">查询条件</param>
        /// <returns>成功返回获取到的实体对象列表，失败则抛出异常</returns>
        public List<T> GetItems<T>(string strWhere)
        {
            return GetItems<T>(strWhere, ""); ;
        }

        /// <summary>
        /// 通过查询条件获取实体对象列表
        /// </summary>
        /// <typeparam name="T">要获取数据的类型</typeparam>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrderBy">排序</param>
        /// <returns>成功返回获取到的实体对象列表，失败则抛出异常</returns>
        public List<T> GetItems<T>(string strWhere, string strOrderBy)
        {
            return GlobalObject.GeneralFunction.ReflectiveListEntity<T>(GetItemsDataSet<T>(strWhere, strOrderBy));
        }

        /// <summary>
        /// 从数据集获取实体对象列表
        /// </summary>
        /// <typeparam name="T">要获取数据的类型</typeparam>
        /// <param name="queryParameter">查询参数实体集列表</param>
        /// <returns>成功返回获取到的实体对象列表，失败则抛出异常</returns>
        public List<T> GetItems<T>(List<ServiceEntity.QueryParameters> queryParameter)
        {
            if (queryParameter == null)
            {
                return null;
            }

            string name = typeof(T).ToString();
            int indexOfLastPoint = name.LastIndexOf('.');
            string className = name.Substring(indexOfLastPoint + 1);

            string strSql = " select * from dbo." + className + " where " + GetSqlWhereString(queryParameter);

            DataSet ds = DataBuilder.GetDBOperate(m_dbName).ExcuteQuery(strSql.ToString());

            return GlobalObject.GeneralFunction.ReflectiveListEntity<T>(ds);
        }

        /// <summary>
        /// 获取所有记录（按默认排序）
        /// </summary>
        /// <typeparam name="T">要获取数据的类型</typeparam>
        /// <returns>成功则返回获取到的数据记录，失败则抛出异常</returns>
        public DataSet GetItemsDataSet<T>()
        {
            return GetItemsDataSet<T>("", "");
        }

        /// <summary>
        /// 获取所有记录（按默认排序）
        /// </summary>
        /// <typeparam name="T">要获取数据的类型</typeparam>
        /// <param name="model">实体集</param>
        /// <returns>成功则返回获取到的数据记录，失败则抛出异常</returns>
        public DataSet GetItemsDataSet<T>(T model)
        {
            PropertyInfo[] listPi = model.GetType().GetProperties();

            string strWhere = "";

            foreach (PropertyInfo pi in listPi)
            {
                object obj = pi.GetValue(model, null);

                if (obj != null && !pi.PropertyType.Name.Contains("List"))
                {
                    strWhere = strWhere + " and " + pi.Name + " = '" + obj.ToString() + "'";
                }
            }

            if (strWhere.Length == 0)
            {
                return null;
            }

            strWhere = strWhere.Substring(4);

            return GetItemsDataSet<T>(strWhere, "");
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <typeparam name="T">要获取数据的类型</typeparam>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrderBy">排序</param>
        /// <returns>成功则返回获取到的数据记录，失败则抛出异常</returns>
        public DataSet GetItemsDataSet<T>(string strWhere, string strOrderBy)
        {
            string name = typeof(T).ToString();

            int indexOfLastPoint = name.LastIndexOf('.');

            string className = name.Substring(indexOfLastPoint + 1);

            StringBuilder strSql = new StringBuilder();

            strSql.Append("select * ");
            strSql.Append(" FROM " + m_dbName + ".dbo." + className);

            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            if (strOrderBy.Trim() != "")
            {
                strSql.Append(" order by " + strOrderBy);
            }

            return DataBuilder.GetDBOperate(m_dbName).ExcuteQuery(strSql.ToString());
        }

        /// <summary>
        /// SWICH特殊操作符获得SQL字符串
        /// </summary>
        /// <param name="operatorInfo">特殊操作符</param>
        /// <param name="queryParam">参数实体集</param>
        /// <returns>返回字符串</returns>
        string SwichSpecialOperator(特殊操作符 operatorInfo, ServiceEntity.QueryParameters queryParam)
        {
            string result = "";

            switch (operatorInfo)
            {
                case 特殊操作符.空:
                    result = " 1 = 1 ";
                    break;
                case 特殊操作符.我参与的:
                    result = " (任务编号 in (select distinct TaskID from ( " +
                            " select TaskID from  TaskManagement.dbo.PRJ_TaskNotifyPeople where WorkID = '" + queryParam.参数值 + "'" +
                            " union all" +
                            " select TaskID from TaskManagement.dbo.PRJ_TaskObserver where WorkID = '" + queryParam.参数值 + "'" +
                            " union all" +
                            " select TaskID from TaskManagement.dbo.PRJ_TaskCooperativePeople where WorkID = '" + queryParam.参数值 + "'" +
                            " union all" +
                            " select TaskID from TaskManagement.dbo.PRJ_TaskNegotiatingMessage where WorkID = '" + queryParam.参数值 + "'" +
                            " ) as a ) or 发布人工号 = '" + queryParam.参数值 + "' or 负责人工号 = '" + queryParam.参数值 + "' or 督办人工号 = '" + queryParam.参数值 + "') ";
                    break;
                case 特殊操作符.关联责任部门:
                    result = " 责任部门 in (select b.DeptName from TaskManagement.dbo.fun_get_RecuresiveDept((select Top 1 DeptCode "+
                             " from DepotManagement.dbo.HR_Dept "+
                             " where DeptCode = '" + queryParam.参数值 + "' or DeptName = '" + queryParam.参数值 + "')) as a "+
                             " inner join DepotManagement.dbo.HR_Dept as b on " +
                             " a.DeptCode = b.DeptCode) ";
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// 判断逻辑表达符
        /// </summary>
        /// <param name="expression">逻辑表达符</param>
        /// <returns>返回字符串</returns>
        string SwichLogicalExpression(逻辑表达符 expression)
        {
            string strOut = "";

            switch (expression)
            {
                case 逻辑表达符.并且:
                    strOut = " and";
                    break;
                case 逻辑表达符.或者:
                    strOut = " or";
                    break;
                case 逻辑表达符.空:
                    strOut = " ";
                    break;
                default:
                    break;
            }

            return strOut;
        }

        /// <summary>
        /// 判断逻辑运算符
        /// </summary>
        /// <param name="expression">逻辑运算符</param>
        /// <returns>返回字符串</returns>
        string SwichLogicalOperator(逻辑运算符 operators)
        {
            string strOut = "";

            switch (operators)
            {
                case 逻辑运算符.等于:
                    strOut = " =";
                    break;
                case 逻辑运算符.不等于:
                    strOut = " <>";
                    break;
                case 逻辑运算符.模糊:
                case 逻辑运算符.左模糊:
                case 逻辑运算符.右模糊:
                    strOut = " like";
                    break;
                case 逻辑运算符.模糊不:
                    strOut = " not like";
                    break;
                case 逻辑运算符.大于:
                    strOut = " >";
                    break;
                case 逻辑运算符.大于等于:
                    strOut = " >=";
                    break;
                case 逻辑运算符.小于:
                    strOut = " <";
                    break;
                case 逻辑运算符.小于等于:
                    strOut = " <=";
                    break;
                case 逻辑运算符.不是:
                    strOut = " is not";
                    break;
                case 逻辑运算符.是:
                    strOut = " is";
                    break;
                case 逻辑运算符.包含:
                    strOut = " in";
                    break;
                case 逻辑运算符.不包含:
                    strOut = " not in";
                    break;
                default:
                    break;
            }

            return strOut;
        }

        /// <summary>
        /// 通过查询语句实体集获得SQL查询条件字符串
        /// </summary>
        /// <param name="queryParameter">查询语句实体集列表</param>
        /// <returns>返回字符串</returns>
        public string GetSqlWhereString(List<ServiceEntity.QueryParameters> queryParameter)
        {
            if (queryParameter == null)
            {
                return "";
            }

            string strSql = "";

            foreach (ServiceEntity.QueryParameters item in queryParameter)
            {
                strSql += SwichLogicalExpression(item.逻辑表达符);

                if (item.左括号 > 0)
                {
                    for (int i = 0; i < item.左括号; i++)
                    {
                        strSql += " (";
                    }
                }

                if (item.特殊操作符 == 特殊操作符.空)
                {
                    strSql += " " + item.字段名;
                    strSql += SwichLogicalOperator(item.逻辑运算符);

                    if (item.逻辑运算符.ToString().Contains("包含"))
                    {
                        strSql += " (";

                        if (item.参数值列表 != null && item.参数值列表.Count() != 0)
                        {
                            foreach (string str in item.参数值列表)
                            {
                                strSql += "'" + str + "',";
                            }

                            strSql = strSql.Substring(0, strSql.Length - 1);
                        }

                        strSql += ")";
                    }
                    else
                    {
                        if (item.逻辑运算符.ToString().Contains("模糊"))
                        {
                            switch (item.逻辑运算符)
                            {
                                case 逻辑运算符.模糊:
                                case 逻辑运算符.模糊不:
                                    strSql += item.参数值 == null ? " null" : " '%" + item.参数值.ToString() + "%'";
                                    break;
                                case 逻辑运算符.左模糊:
                                    strSql += item.参数值 == null ? " null" : " '%" + item.参数值.ToString() + "'";
                                    break;
                                case 逻辑运算符.右模糊:
                                    strSql += item.参数值 == null ? " null" : " '" + item.参数值.ToString() + "%'";
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            strSql += item.参数值 == null ? " null" : " '" + item.参数值.ToString() + "'";
                        }
                    }
                }
                else
                {
                    strSql += SwichSpecialOperator(item.特殊操作符, item);
                }

                if (item.右括号 > 0)
                {
                    for (int i = 0; i < item.右括号; i++)
                    {
                        strSql += " )";
                    }
                }
            }

            return strSql.ToString();
        }

        #endregion
    }
}