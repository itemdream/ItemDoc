﻿using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Configuration;
using System.Net;
using System.Text;

namespace ItemDoc.Framework.Caching
{
  /// <summary>
  /// 使用Redis的缓存服务实现
  /// </summary>
  public partial class RedisCacheManager : ICacheManager
  { 
    #region private
    private static string _connectionString = null;
    private static volatile ConnectionMultiplexer _instance;
    private static readonly object Lock = new object();
    private static IDatabase _db;

    public static ConnectionMultiplexer Instance
    {
      get
      {
        if (_instance != null && _instance.IsConnected)
          return _instance;
        lock (Lock)
        {
          try
          {
            if (_instance != null && _instance.IsConnected)
              return _instance;

            if (string.IsNullOrEmpty(_connectionString))
              throw new Exception("Redis connection string is empty");

            _instance?.Dispose();
            _instance = ConnectionMultiplexer.Connect(_connectionString);
          }
          catch (Exception ex)
          {
            throw new Exception("Redis service is not started " + ex.Message);
          }

        }
        return _instance;
      }
    }


    public RedisCacheManager(string connectionString = null)
    {
      _connectionString = connectionString;

      _db = Instance.GetDatabase();
    }


    public IServer Server(EndPoint endPoint)
    {
      return Instance.GetServer(endPoint);
    }

    public EndPoint[] GetEndpoints()
    {
      return Instance.GetEndPoints();
    }

    public void FlushDb(int? db = null)
    {
      var endPoints = GetEndpoints();

      foreach (var endPoint in endPoints)
      {
        Server(endPoint).FlushDatabase(db ?? -1);
      }
    } 
    #endregion

    #region Methods


    public virtual T Get<T>(string key)
    {
      var value = _db.StringGetAsync(key);
      var obj = Instance.Wait(value);
      if (obj.IsNull)
        return default(T);
      return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(obj));
    }


    public virtual void Set(string key, object data, int cacheTime)
    {
      if (data == null)
        return;

      var entryBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
      var expiresIn = TimeSpan.FromMinutes(cacheTime);

      //_db.StringSet(key, entryBytes, expiresIn);
      _db.StringSetAsync(key, entryBytes, expiresIn, flags: CommandFlags.FireAndForget);
    }

    public void Set(string key, object value, TimeSpan timeSpan)
    {
      if (value == null)
        return;
      var entryBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
      //_db.StringSet(key, entryBytes, timeSpan);
      _db.StringSetAsync(key, entryBytes, timeSpan, flags: CommandFlags.FireAndForget);
    }

    public virtual bool IsSet(string key)
    {
      return _db.KeyExists(key);
    }


    public virtual void Remove(string key)
    {
      _db.KeyDeleteAsync(key, CommandFlags.FireAndForget);
    }


    public virtual void RemoveByPattern(string pattern)
    {
      foreach (var ep in GetEndpoints())
      {
        var server = Server(ep);
        var keys = server.Keys(pattern: "*" + pattern + "*");
        foreach (var key in keys)
          _db.KeyDelete(key);
      }
    }


    public virtual void Clear()
    {
      foreach (var ep in GetEndpoints())
      {
        var server = Server(ep);
        //we can use the code below (commented)
        //but it requires administration permission - ",allowAdmin=true"
        //server.FlushDatabase();

        //that's why we simply interate through all elements now
        var keys = server.Keys();
        foreach (var key in keys)
          _db.KeyDelete(key);
      }
    }

    
    public virtual void Dispose()
    {
       
    }


    #endregion

  }
}