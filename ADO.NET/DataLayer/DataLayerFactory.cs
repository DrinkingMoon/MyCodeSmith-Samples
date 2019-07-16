/****************************************************************************************
 * 版权所有 (c) 2006-2013, 湖南江麓容大车辆传动股份有限公司
 * 
 * 文件名称:   DataLayerFactory.cs
 * 
 * 作者    :   夏石友
 * 
 * 版本:       V1.0.0511
 * 
 * 创建日期:   2009-05-11
 * 
 * 开发平台:   vs2005(c#)
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer
{
    /// <summary>
    /// 数据层接口类厂
    /// </summary>
    public class DataLayerFactory
    {
        /// <summary>
        /// 获取系统默认的数据层操作接口
        /// </summary>
        /// <returns>成功则返回获取到的操作接口，失败返回null</returns>
        public static IDataLayer GetDataLayer()
        {
            return new DataLayer("TaskManagement");
        }

        /// <summary>
        /// 获取系统默认的数据层操作接口
        /// </summary>
        /// <returns>成功则返回获取到的操作接口，失败返回null</returns>
        public static ISelectDataLayer GetSelectDataLayer()
        {
            return new SelectDataLayer("TaskManagement");
        }

        /// <summary>
        /// 获取数据层操作接口
        /// </summary>
        /// <param name="dbName">数据库名称</param>
        /// <returns>成功则返回获取到的操作接口，失败返回null</returns>
        public static IDataLayer GetDataLayer(string dbName)
        {
            switch (dbName)
            {
                case "TaskManagement":
                    return new DataLayer(dbName);
                case "DepotManagement":
                    return new DataLayer(dbName);
                default:
                    throw new Exception(string.Format("数据层不支持此数据库：{0}", dbName));
            }
        }

        /// <summary>
        /// 获取数据层操作接口
        /// </summary>
        /// <param name="dbName">数据库名称</param>
        /// <returns>成功则返回获取到的操作接口，失败返回null</returns>
        public static ISelectDataLayer GetSelectDataLayer(string dbName)
        {
            switch (dbName)
            {
                case "TaskManagement":
                    return new SelectDataLayer(dbName);
                case "DepotManagement":
                    return new SelectDataLayer(dbName);
                default:
                    throw new Exception(string.Format("数据层不支持此数据库：{0}", dbName));
            }
        }
    }
}
