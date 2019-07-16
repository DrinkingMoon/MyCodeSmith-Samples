using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;

namespace DataLayer
{
    /// <summary>
    /// 查询接口
    /// </summary>
    public interface ISelectDataLayer
    {
        /// <summary>
        /// 从数据集获取实体对象列表
        /// </summary>
        /// <typeparam name="T">要获取数据的类型</typeparam>
        /// <param name="model">实体集</param>
        /// <returns>成功返回获取到的实体对象列表，失败则抛出异常</returns>
        T GetItem<T>(T model);

        /// <summary>
        /// 获取通过查询条件获取到的实体集记录
        /// </summary>
        /// <typeparam name="T">要获取数据的类型</typeparam>
        /// <param name="strWhere">查询条件</param>
        /// <returns>成功则返回获取到的数据记录，失败则抛出异常</returns>
        T GetItem<T>(string strWhere);

        /// <summary>
        /// 获取所有实体集记录
        /// </summary>
        /// <typeparam name="T">要获取数据的类型</typeparam>
        /// <param name="model">实体集</param>
        /// <param name="list">查询集</param>
        /// <returns>成功返回获取到的实体对象列表，失败则抛出异常</returns>
        List<T> GetItems<T>(T model, List<T> list);

        /// <summary>
        /// 获取所有实体集记录（按默认排序）
        /// </summary>
        /// <typeparam name="T">要获取数据的类型</typeparam>
        /// <returns>成功返回获取到的实体对象列表，失败则抛出异常</returns>
        List<T> GetItems<T>();

        /// <summary>
        /// 从数据集获取实体对象列表
        /// </summary>
        /// <typeparam name="T">要获取数据的类型</typeparam>
        /// <param name="model">实体集</param>
        /// <returns>成功返回获取到的实体对象列表，失败则抛出异常</returns>
        List<T> GetItems<T>(T model);

        /// <summary>
        /// 通过查询条件获取实体对象列表
        /// </summary>
        /// <typeparam name="T">要获取数据的类型</typeparam>
        /// <param name="strWhere">查询条件</param>
        /// <returns>成功返回获取到的实体对象列表，失败则抛出异常</returns>
        List<T> GetItems<T>(string strWhere);

        /// <summary>
        /// 通过查询条件获取实体对象列表
        /// </summary>
        /// <typeparam name="T">要获取数据的类型</typeparam>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrderBy">排序</param>
        /// <returns>成功返回获取到的实体对象列表，失败则抛出异常</returns>
        List<T> GetItems<T>(string strWhere, string strOrderBy);

        /// <summary>
        /// 获取所有记录（按默认排序）
        /// </summary>
        /// <typeparam name="T">要获取数据的类型</typeparam>
        /// <returns>成功则返回获取到的数据记录，失败则抛出异常</returns>
        DataSet GetItemsDataSet<T>();
        
        /// <summary>
        /// 获取所有记录（按默认排序）
        /// </summary>
        /// <typeparam name="T">要获取数据的类型</typeparam>
        /// <param name="model">实体集</param>
        /// <returns>成功则返回获取到的数据记录，失败则抛出异常</returns>
        DataSet GetItemsDataSet<T>(T model);

        /// <summary>
        /// 获取通过查询条件获取到的所有记录
        /// </summary>
        /// <typeparam name="T">要获取数据的类型</typeparam>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrderBy">排序</param>
        /// <returns>成功则返回获取到的数据记录，失败则抛出异常</returns>
        DataSet GetItemsDataSet<T>(string strWhere, string strOrderBy);

        /// <summary>
        /// 从数据集获取实体对象列表
        /// </summary>
        /// <typeparam name="T">要获取数据的类型</typeparam>
        /// <param name="queryParameter">查询参数实体集列表</param>
        /// <returns>成功返回获取到的实体对象列表，失败则抛出异常</returns>
        List<T> GetItems<T>(List<ServiceEntity.QueryParameters> queryParameter);
        
        /// <summary>
        /// 通过查询语句实体集获得SQL查询条件字符串
        /// </summary>
        /// <param name="queryParameter">查询语句实体集列表</param>
        /// <returns>返回字符串</returns>
        string GetSqlWhereString(List<ServiceEntity.QueryParameters> queryParameter);

    }
}
