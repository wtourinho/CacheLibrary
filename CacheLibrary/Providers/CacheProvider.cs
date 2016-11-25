using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;

namespace CacheLibrary
{
    public class CacheProvider : ICacheProvider
    {
        #region FIELDS AND PROPERTIES

        private Assembly ExecutingAssembly;

        private List<CacheAutomaticRegisterItem> DynamicListRegister = new List<CacheAutomaticRegisterItem>();
        private List<InMemoryCacheItem> InMemoryCacheItems = new List<InMemoryCacheItem>();
        public int DefaultCacheSaveTime { get; set; }
        #endregion

        #region CACHING READ/WRITE

        public object GetFromCache(string key)
        {
            ObjectCache cache = MemoryCache.Default;

            return cache[key];
        }

        public void InsertInCache(string key, object result, int time = RefreshTimeEnum.REFRESH_TIME_05MIN)
        {
            var added = this.InMemoryCacheItems.FirstOrDefault(item => item.Key == key);

            if (added == null)
            {
                this.InMemoryCacheItems.Add(new InMemoryCacheItem()
                {
                    Key = key,
                    RefreshTime = time
                });
            }

            ObjectCache cache = MemoryCache.Default;
            cache.Set(key, result, DateTime.Now.AddMinutes(time));
        }

        public void DeleteFromCache(string key)
        {
            ObjectCache cache = MemoryCache.Default;

            cache.Remove(key);
        }

        #endregion

        #region  AUTOMATIC METHODS, REGISTER AND EXECUTION
        
        private MethodInfo[] GetAutomaticCacheMethods()
        {
            Assembly assembly = this.ExecutingAssembly;

            var methods = assembly.GetTypes()
                      .SelectMany(t => t.GetMethods())
                      .Where(m => m.GetCustomAttributes(typeof(AutomaticCacheAttribute), false).Length > 0)
                      .ToArray();

            return methods;
        }
        
        private void RegisterAutomaticMethods()
        {
            var methods = GetAutomaticCacheMethods();

            for (var i = 0; i < methods.Length; i++)
            {
                var method = methods[i];

                var attr = (AutomaticCacheAttribute)method.GetCustomAttribute(typeof(AutomaticCacheAttribute), true);

                var key = attr.Key;
                var refreshTime = attr.RefreshTime;

                var registerLine = DynamicListRegister.FirstOrDefault(e => e.Key == key);

                if (registerLine != null)
                {
                    throw new Exception("Dynamic cache key already register, key: " + key);
                }

                DynamicListRegister.Add(new CacheAutomaticRegisterItem()
                {
                    Key = key,
                    RefreshTime = refreshTime,
                    Method = method
                });
            }

            DynamicListRegister.ForEach(register =>
            {
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        var result = register.Method.Invoke(null, null);
                        register.CurrentExecutionTime = DateTime.Now;
                        InsertInCache(register.Key, result, register.RefreshTime);

                        var sleeptime = (register.RefreshTime * 60 * 1000) - 30000;
                        register.NextExecutionTime = DateTime.Now.AddMilliseconds(sleeptime);

                        // executes 30s before cache ends
                        System.Threading.Thread.Sleep(sleeptime);
                    }
                });
            });
        }
        
        private void CheckCacheKeysClasses()
        {
            Assembly assembly = this.ExecutingAssembly;

            var cacheKeysClasses = assembly.GetTypes().Where(m => m.GetCustomAttributes(typeof(CacheKeysAttribute), false).Length > 0).ToArray();

            for (var i = 0; i < cacheKeysClasses.Length; i++)
            {
                var cacheKeyClass = cacheKeysClasses[i];
                var fieldsValue = new List<string>();
                foreach (FieldInfo field in cacheKeyClass.GetFields())
                {
                    var value = field.GetRawConstantValue().ToString();
                    if (fieldsValue.FirstOrDefault(s => s == value) != null)
                    {
                        throw new Exception("CacheKey with same name repeats, key: " + value);
                    }
                    else
                    {
                        fieldsValue.Add(value);
                    }
                }
            }
        }

        #endregion

        #region DYNAMIC LIST MANIPULATION
        
        public List<CacheAutomaticRegisterItem> GetFullDynamicListRegister()
        {
            return DynamicListRegister;
        }

        public CacheAutomaticRegisterItem GetDynamicListRegisterByKey(string key)
        {
            return DynamicListRegister.FirstOrDefault(d => d.Key == key);
        }

        public void ForceDynamicListRegisterUpdateByKey(string key)
        {
            var register = DynamicListRegister.FirstOrDefault(d => d.Key == key);

            if (register != null)
            {
                var result = register.Method.Invoke(null, null);

                register.CurrentExecutionTime = DateTime.Now;

                InsertInCache(register.Key, result, register.RefreshTime);
            }
        }

        #endregion

        #region EXECUTED BY METHODS

        public T Execute<T>(Func<T> method, CacheKeyItem cacheExecutionItem)
        {
            var result = GetFromCache(cacheExecutionItem.Key);

            if (result == null)
            {
                result = method();
                InsertInCache(cacheExecutionItem.Key, result, cacheExecutionItem.RefreshTime);
            }

            return (T)result;
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> method, CacheKeyItem cacheExecutionItem)
        {
            var result = GetFromCache(cacheExecutionItem.Key);

            if (result == null)
            {
                result = await method();
                InsertInCache(cacheExecutionItem.Key, result, cacheExecutionItem.RefreshTime);
            }

            return (T)result;
        }

        public T Execute<T>(Func<T> method, string key)
        {
            return this.Execute(method, new CacheKeyItem(key, this.DefaultCacheSaveTime));
        }

        public Task<T> ExecuteAsync<T>(Func<Task<T>> method, string key)
        {
            return this.ExecuteAsync(method, new CacheKeyItem(key, this.DefaultCacheSaveTime));
        }

        #endregion

        #region CACHE ITEMS

        public List<InMemoryCacheItem> GetFullCacheItemsList()
        {
            return InMemoryCacheItems.ToList();
        }

        public void CleanAllCacheItems()
        {
            InMemoryCacheItems.ForEach(item =>
            {
                DeleteFromCache(item.Key);
            });

            InMemoryCacheItems.Clear();
        }

        public void CleanCacheItemByKey(string key)
        {
            DeleteFromCache(key);

            var register = this.InMemoryCacheItems.FirstOrDefault(item => item.Key == key);

            if (register != null)
            {
                this.InMemoryCacheItems.Remove(register);
            }
        }

        public InMemoryCacheItem GetCacheItemByKey(string key)
        {
            var register = InMemoryCacheItems.Concat(DynamicListRegister).FirstOrDefault(item => item.Key == key);

            return register;
        }

        #endregion

        #region INIT

        public void Init(Assembly executingAssembly, int defaultSaveTime = RefreshTimeEnum.REFRESH_TIME_05MIN)
        {
            this.DefaultCacheSaveTime = defaultSaveTime;
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                RegisterAutomaticMethods();

                #if DEBUG
                CheckCacheKeysClasses();
                #endif
            });

            this.ExecutingAssembly = executingAssembly;
        }

        #endregion
    }
}