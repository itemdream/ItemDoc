using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using PetaPoco;

namespace ItemDoc.Framework.Repositories.PetaPoco
{
  /// <summary>
  /// 对PetaPoco封装，用于数据访问
  /// 1、使用linq查询
  /// </summary>
  /// <typeparam name="T">实体类型</typeparam>
  public partial class PocoRepository<T> : IRepository<T> where T : BaseEntity
  {
    /// <summary>
    /// 数据库database对象
    /// </summary>
    private Database _Database;
    /// <summary>
    /// 
    /// </summary>
    private IMapper _defaultMapper;


    /// <summary>
    /// 
    /// </summary>
    private readonly object _lock = new object();
    /// <summary>
    /// 
    /// </summary>
    public PocoRepository()
    {
      _Database = new Database();
      _defaultMapper = _defaultMapper ?? new ConventionMapper();
    }




    /// <summary>
    /// 默认PetaPocoDatabase实例,为空是默认使用第一个connectionStringName
    /// </summary>
    /// <returns>Database</returns>
    public virtual Database Database(string connectionStringName = null)
    {

      int connectionStringsCount = ConfigurationManager.ConnectionStrings.Count;
      if (connectionStringName != null && connectionStringsCount > 0)
      {
        lock (_lock)
        {
          int value = (connectionStringsCount - 1) > 0 ? (connectionStringsCount - 1) : 0;
          string connectionStringsName = ConfigurationManager.ConnectionStrings[value].Name;
          _Database = new Database(connectionStringsName);
        }
      }
      else
      {
        _Database = new Database(connectionStringName);
      }

      return _Database;
    }




    /// <summary>
    /// 获取单个实体
    /// </summary>
    /// <param name="id">主键ID</param>
    /// <returns>Entity</returns>
    public virtual T GetById(object id)
    {
      T entity = Database().SingleOrDefault<T>(id);
      return entity;
    }
    /// <summary>
    /// 添加实体
    /// </summary>
    /// <param name="entity">实体</param>
    public virtual void Insert(T entity)
    {
      try
      {
        if (entity == null)
          throw new ArgumentNullException("entity is null");
        Database().Insert(entity);
      }
      catch (Exception e)
      {
        throw new ArgumentException(e.Message);
      }

    }
    /// <summary>
    /// 添加实体集合
    /// </summary>
    /// <param name="entities">实体集合</param>
    public virtual void Insert(IEnumerable<T> entities)
    {
      try
      {
        if (entities == null)
          throw new ArgumentNullException("entities is null");
        foreach (var entity in entities)
          Database().Insert(entity);

      }
      catch (Exception e)
      {
        throw new ArgumentException(e.Message);
      }

    }

    /// <summary>
    /// 把实体entiy更新到数据库
    /// </summary>
    /// <param name="entity">实体</param>
    public virtual void Update(T entity)
    {
      try
      {
        if (entity == null)
          throw new ArgumentNullException("entity is null");
        Database().Update(entity);
      }
      catch (Exception e)
      {
        throw new ArgumentException(e.Message);
      }
    }

    /// <summary>
    /// 修改实体集合
    /// </summary>
    /// <param name="entities">实体集合</param>
    public virtual void Update(IEnumerable<T> entities)
    {
      try
      {
        if (entities == null)
          throw new ArgumentNullException("entities is null");
        foreach (var entity in entities)
          Database().Update(entity);

      }
      catch (Exception e)
      {
        throw new ArgumentException(e.Message);
      }
    }
    /// <summary>
    /// 删除实体
    /// </summary>
    /// <param name="entity">实体</param>
    public virtual void Delete(T entity)
    {
      try
      {
        if (entity == null)
          throw new ArgumentNullException("entity is null");
        Database().Delete(entity);
      }
      catch (Exception e)
      {
        throw new ArgumentException(e.Message);
      }
    }

    /// <summary>
    /// 删除实体集合
    /// </summary>
    /// <param name="entities">实体集合</param>
    public virtual void Delete(IEnumerable<T> entities)
    {
      try
      {
        if (entities == null)
          throw new ArgumentNullException("entities is null");
        foreach (var entity in entities)
          Database().Delete(entity);

      }
      catch (Exception e)
      {
        throw new ArgumentException(e.Message);
      }
    }


    /// <summary>
    /// 获取所有实体（仅用于数据量少的情况）
    /// </summary>
    /// <returns>返回所有实体集合</returns>
    public virtual IQueryable<T> Table
    {
      get
      {
        try
        {
          string tableName = null;

          try
          {


            PocoData pocoData = PocoData.ForType(typeof(T), _defaultMapper);
            if (pocoData != null)
              tableName = pocoData.TableInfo.TableName;
          }
          catch
          {
            if (tableName == null)
              return null;
          }
          if (tableName == null)
            return null;
          var sql = Sql.Builder
          .Select("TOP 1000 *")
          .From(tableName)
          ;

          var sda = sql.ToString();
          IEnumerable<T> entitylists = Database().Query<T>(sql);
          return entitylists.AsQueryable();
        }
        catch (Exception e)
        {
          throw new ArgumentException(e.Message);
        }
      }
    }
    /// <summary>
    /// 执行一个SQL命令
    /// </summary>
    /// <param name="sql">要执行的SQL语句</param>
    /// <param name="args">在SQL参数任何嵌入参数</param>
    public virtual int Execute(string sql, params object[] args)
    {
      try
      {
        return Database().Execute(sql, args);
      }
      catch (Exception e)
      {
        throw new ArgumentException(e.Message);
      }

    }

    /// <summary>
    /// 创建一个原始 SQL 查询，该查询将返回给定泛型类型的元素。
    /// </summary>
    /// <typeparam name="T">查询所返回对象的类型</typeparam>
    /// <param name="sql">SQL 查询字符串</param>
    /// <param name="parameters">要应用于 SQL 查询字符串的参数</param>
    /// <returns></returns>
    public virtual IQueryable<T> SqlQuery(string sql, params object[] parameters)
    {
      try
      {
        var entitylists = Database().Query<T>(sql, parameters);
        return entitylists.AsQueryable();


      }
      catch (Exception e)
      {
        throw new ArgumentException(e.Message);
      }
    }
  }
}