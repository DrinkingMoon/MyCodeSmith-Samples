using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ConfigEntity;
using GlobalObject;

namespace DataLayer
{
    /// <summary>
    /// 数据层操作接口
    /// </summary>
    public interface IDataLayer : ISelectDataLayer
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="T">操作数据的类型</typeparam>
        /// <param name="item">要保存的数据信息</param>
        /// <param name="error">错误信息</param>
        bool Insert<T>(T item, out string error);
        
        /// <summary>
        /// 向事务中添加插入存储过程
        /// </summary>
        /// <typeparam name="T">操作数据的类型</typeparam>
        /// <param name="items">实体列表</param>
        /// <param name="accessParameter">数据层参数对象</param>
        /// <param name="error">错误信息</param>
        bool Insert<T>(List<T> items, AccessParameter accessParameter, out string error);
        
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <typeparam name="T">操作数据的类型</typeparam>
        /// <param name="item">要保存的数据信息</param>
        /// <param name="error">错误信息</param>
        bool Modify<T>(T item, out string error);
        
        /// <summary>
        /// 向事务中添加更新存储过程
        /// </summary>
        /// <typeparam name="T">操作数据的类型</typeparam>
        /// <param name="items">实体列表</param>
        /// <param name="accessParameter">数据层参数对象</param>
        /// <param name="error">错误信息</param>
        bool Modify<T>(List<T> items, AccessParameter accessParameter, out string error);
        
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T">操作数据的类型</typeparam>
        /// <param name="item">要删除的数据信息</param>
        /// <param name="error">错误信息</param>
        bool Delete<T>(T item, out string error);
        
        /// <summary>
        /// 向事务中添加更新存储过程
        /// </summary>
        /// <typeparam name="T">操作数据的类型</typeparam>
        /// <param name="items">实体列表</param>
        /// <param name="accessParameter">数据层参数对象</param>
        /// <param name="error">错误信息</param>
        bool Delete<T>(List<T> items, AccessParameter accessParameter, out string error);

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="accessParameter">数据层参数对象</param>
        /// <returns>成功则返回True，失败则返回False</returns>
        bool RunTransaction(AccessParameter accessParameter);
        
        /// <summary>
        /// 获得数据库操作参数实体集列表
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="listInfo">实体集列表</param>
        /// <param name="dataOperator">数据库操作类型</param>
        /// <returns>返回数据库操作参数实体集列表</returns>
        List<ServiceEntity.DataBaseOperatorParmeters> GetOperatorInfo<T>(List<T> listInfo, 数据库操作 dataOperator);
    }
}

