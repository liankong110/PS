//-------------------------------------------------------------------------
// <copyright  company="同程软件有限公司" file="CacheHelper.cs">
//  Copyright (c) 版本 V1
//  所属部门：景区B项目部-系统项目组
//  所属项目：智慧旅游
//  作    者：李小洋
//  创建日期：2012-9-18 9:42:49
//  功能描述：请一定在此描述类用途
//  版本历史:
//      如有新增或修改请再次添加描述(格式：时间+作者+描述)
//</copyright>
//-------------------------------------------------------------------------
using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

namespace VisualSmart.Util
{
    public class CacheHelper
    {
        #region 用户自定义变量
        private static readonly Cache Cache;//缓存实例
        private static readonly int Hourfactor;
        #endregion

        #region 构造函数
        static CacheHelper()
        {
            Hourfactor = 3600;
            Cache = HttpRuntime.Cache;
        }

        private CacheHelper()
        {
        }
        #endregion

        #region 清除所有缓存
        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public static void Clear()
        {
            //要循环访问 Cache 对象的枚举数
            IDictionaryEnumerator enumerator = Cache.GetEnumerator();//检索用于循环访问包含在缓存中的键设置及其值的字典枚举数
            {
                while (enumerator.MoveNext())
                {
                    Cache.Remove(enumerator.Key.ToString());
                }
            }
        }

        #endregion

        #region 得到缓存实例
        /// <summary>
        /// 得到缓存实例
        /// </summary>
        /// <param name="key">缓存实例名称</param>
        /// <returns>返回缓存实例</returns>
        public static object GetCache(string key)
        {
            return Cache[key];
        }
        #endregion

        #region 缓存实例插入
        /// <summary>
        /// 缓存实例插入(默认缓存20分钟)
        /// </summary>
        /// <param name="key">缓存实例名称</param>
        /// <param name="obj">要缓存的对象</param>
        public static void Insert(string key, object obj)
        {
            CacheDependency dep = null;
            Insert(key, obj, dep, 20);
        }

        /// <summary>
        /// 缓存实例插入
        /// </summary>
        /// <param name="key">缓存实例名称</param>
        /// <param name="obj">要缓存的对象</param>
        /// <param name="seconds">缓存的时间</param>
        public static void Insert(string key, object obj, int seconds)
        {
            CacheDependency dep = null;
            Insert(key, obj, dep, seconds);
        }

        /// <summary>
        /// 缓存实例插入(缓存过期时间是一天)
        /// </summary>
        /// <param name="key">缓存实例名称</param>
        /// <param name="obj">要缓存的对象</param>
        /// <param name="dep">缓存的依赖项</param>
        public static void Insert(string key, object obj, CacheDependency dep)
        {
            Insert(key, obj, dep, Hourfactor * 12);
        }

        /// <summary>
        /// 缓存实例插入(缓存过期时间是一天)
        /// </summary>
        /// <param name="key">缓存实例名称</param>
        /// <param name="obj">要缓存的对象</param>
        /// <param name="xmlPath">缓存的依赖项xml文件的路径（绝对路径）</param>
        public static void Insert(string key, object obj, string xmlPath)
        {
            var dep = new CacheDependency(xmlPath);
            Insert(key, obj, dep, Hourfactor * 12);
        }

        /// <summary>
        /// 缓存实例插入
        /// </summary>
        /// <param name="key">用于引用该对象的缓存键</param>
        /// <param name="obj">要插入缓存中的对象</param>
        /// <param name="seconds">缓存的依赖项xml文件的路径(绝对路径)</param>
        /// <param name="priority">所插入对象将过期并被从缓存中移除的时间</param>
        public static void Insert(string key, object obj, int seconds, CacheItemPriority priority)
        {
            Insert(key, obj, null, seconds, priority);
        }

        /// <summary>
        /// 缓存实例插入
        /// </summary>
        /// <param name="key">用于引用该对象的缓存键</param>
        /// <param name="obj">要插入缓存中的对象</param>
        /// <param name="dep">该项的文件依赖项或缓存键依赖项。当任何依赖项更改时，该对象即无效，并从缓存中移除。如果没有依赖项，则此参数包含空引用（Visual Basic 中为 Nothing）</param>
        /// <param name="seconds">所插入对象将过期并被从缓存中移除的时间。</param>
        public static void Insert(string key, object obj, CacheDependency dep, int seconds)
        {
            Insert(key, obj, dep, seconds, CacheItemPriority.Normal);
        }

        /// <summary>
        /// 缓存实例插入
        /// </summary>
        /// <param name="key">用于引用该对象的缓存键</param>
        /// <param name="obj">要插入缓存中的对象</param>
        /// <param name="xmlPath">缓存的依赖项xml文件的路径（绝对路径）</param>
        /// <param name="seconds">所插入对象将过期并被从缓存中移除的时间。</param>
        public static void Insert(string key, object obj, string xmlPath, int seconds)
        {
            var dep = new CacheDependency(xmlPath);
            Insert(key, obj, dep, seconds, CacheItemPriority.Normal);
        }

        /// <summary>
        /// 缓存实例插入
        /// </summary>
        /// <param name="key">用于引用该对象的缓存键</param>
        /// <param name="obj">要插入缓存中的对象</param>
        /// <param name="dep">该项的文件依赖项或缓存键依赖项。当任何依赖项更改时，该对象即无效，并从缓存中移除。如果没有依赖项，则此参数包含空引用（Visual Basic 中为 Nothing）。</param>
        /// <param name="seconds">所插入对象将过期并被从缓存中移除的时间。</param>
        /// <param name="priority">该对象相对于缓存中存储的其他项的成本，由 CacheItemPriority 枚举表示。该值由缓存在退出对象时使用；具有较低成本的对象在具有较高成本的对象之前被从缓存移除。 </param>
        public static void Insert(string key, object obj, CacheDependency dep, int seconds, CacheItemPriority priority)
        {
            if (obj != null)
            {
                Cache.Insert(key, obj, dep, DateTime.Now.AddSeconds(seconds), TimeSpan.Zero, priority, null);
            }
        }
        #endregion

        /// <summary>
        /// 移出单个缓存
        /// </summary>
        /// <param name="key">缓存实例名称</param>
        public static void Remove(string key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        /// 设置缓存值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetCacheValue(string key, object value)
        {
            if (Cache[key] == null)
            {
                Insert(key, value);
            }
            else
            {
                Cache[key] = value;
            }
        }

        #region 得到所有使用的Cache键值
        /// <summary>
        /// 得到所有使用的Cache键值
        /// </summary>
        /// <returns>返回所有的Cache键值</returns>
        public static ArrayList GetAllCacheKey()
        {
            var arrList = new ArrayList();
            IDictionaryEnumerator enumerator = Cache.GetEnumerator();
            {
                while (enumerator.MoveNext())
                {
                    arrList.Add(enumerator.Key);
                }
            }
            return arrList;
        }
        #endregion

    }
}