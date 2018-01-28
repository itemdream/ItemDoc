using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Newtonsoft.Json.Converters;
using PetaPoco;

namespace ItemDoc.Framework.Repositories
{
    /// <summary>
    /// 数据仓储接口
    /// </summary>
    /// <typeparam name="T">仓储对应的实体</typeparam>
    public partial interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        T GetById(object id);
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>返回id主键</returns>
        void Insert(T entity);
        /// <summary>
        /// 添加实体集合
        /// </summary>
        /// <param name="entities">实体集合</param>
        void Insert(IEnumerable<T> entities);
        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity">实体</param>
        void Update(T entity);
        /// <summary>
        /// 修改实体集合
        /// </summary>
        /// <param name="entities">实体集合</param>
        void Update(IEnumerable<T> entities);
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">实体</param>
        void Delete(T entity);
        /// <summary>
        /// 删除实体集合
        /// </summary>
        /// <param name="entities">实体集合</param>
        void Delete(IEnumerable<T> entities);
        /// <summary>
        /// 获取集合
        /// </summary>
        IQueryable<T> Table { get; }
       

        /// <summary>
        /// 执行给定的命令
        /// </summary>
        /// <param name="sql">命令字符串</param>
        /// <param name="parameters">要应用于命令字符串的参数</param>
        /// <returns>执行命令后由数据库返回的结果</returns>
        int Execute(string sql, params object[] parameters);
        /// <summary>
        /// 创建一个原始 SQL 查询，该查询将返回给定泛型类型的元素。
        /// </summary>
        /// <typeparam name="T">查询所返回对象的类型</typeparam>
        /// <param name="sql">SQL 查询字符串</param>
        /// <param name="parameters">要应用于 SQL 查询字符串的参数</param>
        /// <returns></returns>
        IQueryable<T> SqlQuery(string sql, params object[] parameters);

 
    }
}
