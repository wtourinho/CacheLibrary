using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CacheLibrary
{
    public interface ICacheProvider
    {
        #region CACHING READ/WRITE

        object GetFromCache(string key);

        void InsertInCache(string key, object result, int time = RefreshTimeEnum.REFRESH_TIME_05MIN);
        
        void DeleteFromCache(string key);

        #endregion

        #region DYNAMIC LIST MANIPULATION

        List<CacheAutomaticRegisterItem> GetFullDynamicListRegister();

        CacheAutomaticRegisterItem GetDynamicListRegisterByKey(string key);

        void ForceDynamicListRegisterUpdateByKey(string key);

        #endregion

        #region EXECUTED BY METHODS

        T Execute<T>(Func<T> method, CacheKeyItem cacheExecutionItem);

        Task<T> ExecuteAsync<T>(Func<Task<T>> method, CacheKeyItem cacheExecutionItem);

        T Execute<T>(Func<T> method, string key);

        Task<T> ExecuteAsync<T>(Func<Task<T>> method, string key);

        #endregion

        #region CACHE ITEMS

        List<InMemoryCacheItem> GetFullCacheItemsList();

        void CleanAllCacheItems();

        void CleanCacheItemByKey(string key);

        InMemoryCacheItem GetCacheItemByKey(string key);

        #endregion

        #region INIT

        void Init(Assembly executingAssembly, int defaultSaveTime = RefreshTimeEnum.REFRESH_TIME_05MIN);

        #endregion
    }
}
