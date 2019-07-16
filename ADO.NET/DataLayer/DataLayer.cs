using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using OperateDatabase;
using DataEntity;
using GlobalObject;
using System.Diagnostics;
using System.Runtime.Remoting;
using ConfigEntity;
using ServiceEntity;

namespace DataLayer
{
    /// <summary>
    /// 数据层类厂
    /// </summary>
    public class DataLayer : SelectDataLayer, IDataLayer
    {
        /// <summary>
        /// 数据库名称
        /// </summary>
        private string m_dbName;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public DataLayer()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbName">数据库名称</param>
        public DataLayer(string dbName):base(dbName)
        {
            Debug.Assert(!string.IsNullOrEmpty(dbName), "数据库名称不允许为空");

            m_dbName = dbName;


        }

        /// <summary>
        /// 获取服务组件
        /// </summary>
        /// <typeparam name="T">操作数据的类型</typeparam>
        /// <returns>返回组件接口</returns>
        static IDataLayer GetObject<T>()
        {
            string name = typeof(T).ToString();

            int indexOfLastPoint = name.LastIndexOf('.');

            string className;

            if (indexOfLastPoint == -1)
            {
                className = name.Substring(1);
            }
            else
            {
                className = "DataLayer." + name.Substring(indexOfLastPoint + 1);
            }

            ObjectHandle objectHandle = Activator.CreateInstance(null, className);

            return (IDataLayer)objectHandle.Unwrap();
        }

        #region IDataLayer 成员

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="T">操作数据的类型</typeparam>
        /// <param name="item">要保存的数据信息</param>
        /// <param name="error">错误信息</param>
        public bool Insert<T>(T item,out string error)
        {
            error = null;
            try
            {
                if (!GetObject<T>().Insert<T>(item, out error))
                {
                    return false;
                }

                if (!typeof(T).ToString().Contains("PRJ_SystemLog"))
                {
                    List<T> list = new List<T>();

                    list.Add(item);

                    if (!SystemLog<T>(list, 数据库操作.添加, out error))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 向事务中添加插入存储过程
        /// </summary>
        /// <typeparam name="T">操作数据的类型</typeparam>
        /// <param name="items">实体列表</param>
        /// <param name="accessParameter">数据层参数对象</param>
        /// <param name="error">错误信息</param>
        public bool Insert<T>(List<T> items, AccessParameter accessParameter, out string error)
        {
            error = null;

            try
            {
                if (!GetObject<T>().Insert<T>(items, accessParameter, out error))
                {
                    return false;
                }

                if (!typeof(T).ToString().Contains("PRJ_SystemLog"))
                {
                    if (!SystemLog<T>(accessParameter,items, 数据库操作.添加, out error))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <typeparam name="T">操作数据的类型</typeparam>
        /// <param name="item">要保存的数据信息</param>
        /// <param name="error">错误信息</param>
        public bool Modify<T>(T item,out string error)
        {
            error = null;

            try
            {
                if (!GetObject<T>().Modify<T>(item,out error))
                {
                    return false;
                }

                if (!typeof(T).ToString().Contains("PRJ_SystemLog"))
                {
                    List<T> list = new List<T>();

                    list.Add(item);

                    if (!SystemLog<T>(list, 数据库操作.修改, out error))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }

        }

        /// <summary>
        /// 向事务中添加更新存储过程
        /// </summary>
        /// <typeparam name="T">操作数据的类型</typeparam>
        /// <param name="items">实体列表</param>
        /// <param name="accessParameter">数据层参数对象</param>
        /// <param name="error">错误信息</param>
        public bool Modify<T>(List<T> items, AccessParameter accessParameter,out string error)
        {
            error = null;

            try
            {
                if(!GetObject<T>().Modify<T>(items, accessParameter, out error))
                {
                    return false;
                }

                if (!typeof(T).ToString().Contains("PRJ_SystemLog"))
                {
                    if (!SystemLog<T>(accessParameter, items, 数据库操作.修改, out error))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T">操作数据的类型</typeparam>
        /// <param name="item">要删除的数据信息</param>
        /// <param name="error">错误信息</param>
        public bool Delete<T>(T item, out string error)
        {
            error = null;

            try
            {
                if (!GetObject<T>().Delete<T>(item, out error))
                {
                    return false;
                }

                if (!typeof(T).ToString().Contains("PRJ_SystemLog"))
                {
                    List<T> list = new List<T>();

                    list.Add(item);

                    if (!SystemLog<T>(list, 数据库操作.删除, out error))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 向事务中添加更新存储过程
        /// </summary>
        /// <typeparam name="T">操作数据的类型</typeparam>
        /// <param name="items">实体列表</param>
        /// <param name="accessParameter">数据层参数对象</param>
        /// <param name="error">错误信息</param>
        public bool Delete<T>(List<T> items, AccessParameter accessParameter, out string error)
        {
            error = null;

            try
            {
                if (!GetObject<T>().Delete<T>(items, accessParameter, out error))
                {
                    return false;
                }

                if (!typeof(T).ToString().Contains("PRJ_SystemLog"))
                {
                    if (!SystemLog<T>(accessParameter, items, 数据库操作.删除, out error))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="accessParameter">数据层参数对象</param>
        /// <returns>成功则返回True，失败则返回False</returns>
        public bool RunTransaction(AccessParameter accessParameter)
        {
            Hashtable[] parms = null;

            return DataBuilder.GetDBOperate(m_dbName)
                .RunTransaction(accessParameter.StoredProcedures, accessParameter.StoredParams, ref parms);
        }

        /// <summary>
        /// 获得主键列表
        /// </summary>
        /// <typeparam name="T">表名与数据类型名相同</typeparam>
        /// <returns>返回字符串列表</returns>
        List<string> GetPrimaryKey<T>()
        {
            string name = typeof(T).ToString();
            int indexOfLastPoint = name.LastIndexOf('.');
            string strTableName = name.Substring(indexOfLastPoint + 1);

            Hashtable parameters = new Hashtable();

            parameters.Add("@table_name", strTableName);
            parameters.Add("@table_owner", "dbo");
            parameters.Add("@table_qualifier", "TaskManagement");

            DataSet ds = OperateDatabase.DataBuilder.GetDBOperate("TaskManagement").RunProcedure("sp_pkeys", parameters);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }

            List<string> listPKeys = new List<string>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                listPKeys.Add(dr["COLUMN_NAME"].ToString());
            }

            return listPKeys;
        }

        /// <summary>
        /// 获取带查询字符串的字典
        /// </summary>
        /// <typeparam name="T">操作数据类型</typeparam>
        /// <param name="listInfo">实体集列表</param>
        /// <returns>返回字典</returns>
        Dictionary<T, string> GetWhereDictionary<T>(List<T> listInfo)
        {
            Dictionary<T, string> result = new Dictionary<T, string>();

            List<string> listPKeys = GetPrimaryKey<T>();

            if (listPKeys == null)
            {
                return null;
            }

            foreach (T model in listInfo)
            {
                string strWhere = "";

                foreach (string pKeyName in listPKeys)
                {
                    string strValue = "";

                    if (model.GetType().GetProperty(pKeyName).GetValue(model, null) != null)
                    {
                        strValue = model.GetType().GetProperty(pKeyName).GetValue(model, null).ToString();
                    }

                    strWhere += " " + pKeyName + " = '" + strValue + "' and";
                }

                if (strWhere.Length != 0)
                {
                    strWhere = strWhere.Substring(0, strWhere.Length - 3);
                }

                result.Add(model, strWhere);
            }

            return result;
        }

        /// <summary>
        /// 获得数据库操作参数实体集列表
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="listInfo">实体集列表</param>
        /// <param name="dataOperator">数据库操作类型</param>
        /// <returns>返回数据库操作参数实体集列表</returns>
        public List<ServiceEntity.DataBaseOperatorParmeters> GetOperatorInfo<T>(List<T> listInfo, 数据库操作 dataOperator)
        {
            if (listInfo == null || listInfo.Count == 0)
            {
                return null;
            }

            Dictionary<string, Type> dic = GlobalObject.GeneralFunction.GetAttribute<T>();

            string name = typeof(T).ToString();
            int indexOfLastPoint = name.LastIndexOf('.');
            string strTableName = name.Substring(indexOfLastPoint + 1);

            Dictionary<T, string> dicSelect = GetWhereDictionary<T>(listInfo);

            if (dicSelect == null)
            {
                return null;
            }
            else
            {
                Dictionary<T, T> dicCompare = new Dictionary<T, T>();

                foreach (KeyValuePair<T, string> dicItemSelect in dicSelect)
                {
                    dicCompare.Add(dicItemSelect.Key, GetItem<T>(dicItemSelect.Value.ToString()));
                }

                List<ServiceEntity.DataBaseOperatorParmeters> listOpertaor = new List<ServiceEntity.DataBaseOperatorParmeters>();

                foreach (KeyValuePair<T, T> compareItem in dicCompare)
                {
                    ServiceEntity.DataBaseOperatorParmeters dbOperatorModel = new ServiceEntity.DataBaseOperatorParmeters();
                    
                    dbOperatorModel.PrimaryKey = new Dictionary<string, object>();
                    dbOperatorModel.OperatorInfo = new Dictionary<string, DataValueType>();
                    dbOperatorModel.TableName = strTableName;
                    dbOperatorModel.DataOperator = dataOperator;

                    T oldModel = compareItem.Value;
                    T newModel = compareItem.Key;

                    foreach (KeyValuePair<string, Type> column in dic)
                    {

                        if (column.Value.FullName.Contains("List"))
                        {
                            continue;
                        }

                        string strColumnName = column.Key.ToString();

                        object objOldValue = null;
                        object objNewValue = null;

                        if (oldModel != null)
                        {
                            objOldValue = oldModel.GetType().GetProperty(strColumnName).GetValue(oldModel, null);
                        }

                        if (newModel != null)
                        {
                            objNewValue = newModel.GetType().GetProperty(strColumnName).GetValue(newModel, null);
                        }

                        DataValueType modelType = new DataValueType();
                        modelType.NewValue = objNewValue;
                        modelType.OldValue = objOldValue;

                        switch (dbOperatorModel.DataOperator)
                        {
                            case 数据库操作.添加:
                                dbOperatorModel.OperatorInfo.Add(column.Key, modelType);
                                break;
                            case 数据库操作.修改:
                                if (objOldValue != objNewValue)
                                {
                                    string strOld = objOldValue == null ? "" : objOldValue.ToString();
                                    string strNew = objNewValue == null ? "" : objNewValue.ToString();

                                    if (strOld != strNew)
                                    {
                                        dbOperatorModel.OperatorInfo.Add(column.Key, modelType);
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    List<string> listPKeys = GetPrimaryKey<T>();

                    foreach (string primaryKeyName in listPKeys)
                    {
                        object objValue = null;

                        if (newModel != null)
                        {
                            objValue = newModel.GetType().GetProperty(primaryKeyName).GetValue(newModel, null);
                        }

                        dbOperatorModel.PrimaryKey.Add(primaryKeyName, objValue);
                    }

                    listOpertaor.Add(dbOperatorModel);
                }

                return listOpertaor;
            }
 
        }

        /// <summary>
        /// 操作任务日志
        /// </summary>
        /// <typeparam name="T">操作数据类型</typeparam>
        /// <param name="listInfo">实体集列表</param>
        /// <param name="dataOperator">数据库操作类型</param>
        /// <param name="error">错误信息</param>
        bool SystemLog<T>(List<T> listInfo, 数据库操作 dataOperator, out string error)
        {
            error = null;

            if (listInfo == null || listInfo.Count == 0)
            {
                error = "操作失败： 数据为空";
                return false;
            }

            try
            {
                AccessParameter acc = new AccessParameter();

                if (!SystemLog<T>(acc, listInfo, dataOperator, out error))
                {
                    return false;
                }

                if (!RunTransaction(acc))
                {
                    error = "操作失败：存储过程操作失败";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 操作任务日志
        /// </summary>
        /// <typeparam name="T">操作数据类型</typeparam>
        /// <typeparam name="accessParameter">数据上下文</typeparam>
        /// <param name="listInfo">实体集列表</param>
        /// <param name="dataOperator">数据库操作类型</param>
        /// <param name="error">错误信息</param>
        bool SystemLog<T>(AccessParameter accessParameter, List<T> listInfo, 数据库操作 dataOperator , out string error)
        {
            error = null;

            if (listInfo == null || listInfo.Count == 0)
            {
                error = "操作失败： 数据为空";
                return false; ;
            }

            try
            {

                List<ServiceEntity.DataBaseOperatorParmeters> listOperator = GetOperatorInfo<T>(listInfo, dataOperator);

                foreach (ServiceEntity.DataBaseOperatorParmeters item in listOperator)
                {

                    string strPrimary = "";
                    string strWhere = "主键:";

                    foreach (KeyValuePair<string, object> primaryKey in item.PrimaryKey)
                    {
                        string strValue = "";

                        if (primaryKey.Value != null)
                        {
                            strValue = primaryKey.Value.ToString();
                        }

                        strWhere += " 【 " + primaryKey.Key + " 字段， 值: " + strValue + " 】";
                    }

                    foreach (KeyValuePair<string, DataValueType> info in item.OperatorInfo)
                    {
                        if (info.Key != null)
                        {
                            strWhere += "【 " + info.Key + " 字段，";

                            string strOld = info.Value.OldValue == null ? "" : info.Value.OldValue.ToString();
                            string strNew = info.Value.NewValue == null ? "" : info.Value.NewValue.ToString();

                            if (info.Value.NewValue != null)
                            {
                                strWhere += " 值: " + strNew;
                            }

                            if (item.DataOperator == 数据库操作.修改)
                            {
                                strWhere += " 原值: " + strNew;
                            }

                            strWhere += " 】";
                        }
                    }

                    DataEntity.PRJ_SystemLog sysLog = new DataEntity.PRJ_SystemLog();

                    sysLog.Content = "操作表: {[" + item.TableName + "]},操作类型:{" + item.DataOperator.ToString() + "}, {"
                        + strPrimary + "}, 操作内容: {" + strWhere + "}";
                    sysLog.RecordTime = DateTime.Now;
                    sysLog.Recorder = "";

                    List<DataEntity.PRJ_SystemLog> list = new List<DataEntity.PRJ_SystemLog>();
                    list.Add(sysLog);

                    if (!Insert<DataEntity.PRJ_SystemLog>(list, accessParameter, out error))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }

        }

        #endregion
    }
}