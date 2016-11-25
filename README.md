# Automatically CacheLibrary 

You can use this library to cache your methods result to the MemoryCache automatically or not.

To automatically save a method result to the MemoryCache you should use the AutomaticCache attribute in the method, it will be executing automatically and saving the data in the MemoryCache using the CacheProvider API.

All methods invocation should be done using the CacheLibrary Execute function to get the data from the cache or save new data when there is no cache hit.

## Main use

```
[AutomaticCache("GetDataCacheKey", 1 /*Minutes*/)]

public static string GetData() {...}
```

The GetData function will execute every minute and save the data in the CacheProvider.



To get the data from the cache you should execute:

```
var result = CacheManager.Execute(GetData, "GetDataCacheKey");
```



For methods with multiple parameters you must use lambda expressions:

```
var result = CacheManager.Execute(() =>
{
	return GetWithParameter(p1, p2);
}, "GetWithParameterCacheKey" + string.Format("-{0}-{1}", p1, p2));
```


## CacheProvider API:


void Init(Assembly executingAssembly, int defaultSaveTime)
- Load the assembly to find the methods with AutomaticCache attribute (reflection).

object GetFromCache(string key)
- Get a item from the MemoryCache using the "key"

void DeleteFromCache(string key)
- Delete a item from the MemoryCache using the "key"

List<CacheAutomaticRegisterItem> GetFullDynamicListRegister()
- Get list of methods with the AutomaticCache attribute

CacheAutomaticRegisterItem GetDynamicListRegisterByKey(string key)
- Get methods with the AutomaticCache attribute by "key"

void ForceDynamicListRegisterUpdateByKey(string key)
- Force method with the AutomaticCache attribute to execute

T Execute<T>(Func<T> method, CacheKeyItem cacheExecutionItem)
- Executes method and saves result in the cache. Use cacheExecutionItem to define the save time and the key

async Task<T> ExecuteAsync<T>(Func<Task<T>> method, CacheKeyItem cacheExecutionItem)
- Executes async method and saves result in the cache. Use cacheExecutionItem to define the save time and the key

T Execute<T>(Func<T> method, string key)
- Executes method and saves result in the cache. Uses the default cache save time of the Init method

Task<T> ExecuteAsync<T>(Func<Task<T>> method, string key)
- Executes async method and saves result in the cache. Uses the default cache save time of the Init method




## Demo 1:
Shows a simple use of the CacheLibrary


## Demo 2:
Implements a advanced use of the cache library using a CacheKeysRegister to ensure that there is no repeated cache keys.
This demo also uses a enum with some default cache refresh times (RefreshTimeEnum)


