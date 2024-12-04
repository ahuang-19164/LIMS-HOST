using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Yichen.Net.Caching
{
    public class RedisHelper
    {
        public static ConnectionMultiplexer redisconn { get; set; }
        public static IDatabase redisDB { get; set; }
        public RedisHelper(string connection)
        {
            redisconn = ConnectionMultiplexer.Connect(connection);
            redisDB = redisconn.GetDatabase();
        }
        /// <summary>
        /// 增加/修改 string 数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>

        public static bool SetStringValue(string key, string value)
        {
            return redisDB.StringSet(key, value);
        }

        public static bool SetListValue(string key, string value)
        {
            redisDB.KeyDelete("leftlistkey");
            for (int i = 0; i < 10; i++)
            {
                var getobjlist = redisDB.ListLeftPush("leftlistkey", (i + 1).ToString());
            }
            string[] a = redisDB.ListRange("leftlistkey").ToStringArray();

            return true;
        }

        ///// <summary>
        ///// 增加/修改 string 数据
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>

        //public static bool SetListValue<T>(string key, List<T> value )
        //{
        //    DataTable dt = ConvertClass<T>.FillDataTable(value);
        //    //DataTable dt = ConvertClass<T>.FillDataTable(value);
        //    return redisDB.StringSetAsync(key,value);
        //}


        /// <summary>
        /// 增加/修改 table 数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>

        public static bool SetDataTableValue(string key, DataTable value)
        {
            return redisDB.StringSet(key, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// 在列表头部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public async Task<long> ListLeftPushAsync(string redisKey, string redisValue)
        {
            return await redisDB.ListLeftPushAsync(redisKey, redisValue);
        }

        /// <summary>
        /// 在列表头部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public async Task<long> ListLeftPushAsync(string redisKey, DataTable dataTable)
        {
            return await redisDB.ListLeftPushAsync(redisKey, JsonConvert.SerializeObject(dataTable));
        }




        /// <summary>
        /// 在列表尾部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public async Task<long> ListRightPushAsync(string redisKey, DataTable dataTable)
        {
            return await redisDB.ListRightPushAsync(redisKey, JsonConvert.SerializeObject(dataTable));
        }


        public static void keysexist(string key)
        {
            foreach (var endPoint in redisconn.GetEndPoints())
            {
                //获取指定服务器
                var server = redisconn.GetServer(endPoint);
                //在指定服务器上使用 keys 或者 scan 命令来遍历key
                foreach (var lookkey in server.Keys(-1, $"{key}*"))
                {
                    //获取key对于的值
                    //var val = redisDB.StringGet(key);
                    //Console.WriteLine($"key: {key}, value: {val}");
                    Console.WriteLine($"key: {key}");
                }
            }
        }

        //public long publish<T>(string topic, T message)
        //{
        //    var subscriber = redis.GetSubscriber();
        //    return subscriber.Publish(topic, SerializeHelper.SerializeString(message));
        //}
        /// <summary>
        /// 查询 获取string
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>

        public static string GetStringValue(string key)
        {
            return redisDB.StringGet(key);
        }
        /// <summary>
        /// 查询 获取dataTable
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>

        public static DataTable GetDataTableValue(string key)
        {
            return JsonConvert.DeserializeObject<DataTable>(redisDB.StringGet(key));
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>

        public static bool DeleteKey(string key)
        {
            return redisDB.KeyDelete(key);
        }

        /// <summary>
        /// 列表长度
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<long> ListLengthAsync(string redisKey)
        {
            return await redisDB.ListLengthAsync(redisKey);
        }

        /// <summary>
        /// 返回在该列表上键所对应的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> ListRangeAsync(string redisKey, int db = -1)
        {
            var result = await redisDB.ListRangeAsync(redisKey);
            return result.Select(o => o.ToString());
        }

        /// <summary>
        /// 根据索引获取指定位置数据
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> ListRangeAsync(string redisKey, int start, int stop)
        {
            var result = await redisDB.ListRangeAsync(redisKey, start, stop);
            return result.Select(o => o.ToString());
        }

        /// <summary>
        /// 删除List中的元素 并返回删除的个数
        /// </summary>
        /// <param name="redisKey">key</param>
        /// <param name="redisValue">元素</param>
        /// <param name="type">大于零 : 从表头开始向表尾搜索，小于零 : 从表尾开始向表头搜索，等于零：移除表中所有与 VALUE 相等的值</param>
        /// <returns></returns>
        public async Task<long> ListDelRangeAsync(string redisKey, string redisValue, long type = 0)
        {
            return await redisDB.ListRemoveAsync(redisKey, redisValue, type);
        }

        /// <summary>
        /// 清空List
        /// </summary>
        /// <param name="redisKey"></param>
        public async Task ListClearAsync(string redisKey)
        {
            await redisDB.ListTrimAsync(redisKey, 1, 0);
        }


        /// <summary>
        /// 有序集合/定时任务延迟队列用的多
        /// </summary>
        /// <param name="redisKey">key</param>
        /// <param name="redisValue">元素</param>
        /// <param name="score">分数</param>
        public async Task SortedSetAddAsync(string redisKey, string redisValue, double score)
        {
            await redisDB.SortedSetAddAsync(redisKey, redisValue, score);
        }

        /// <summary>
        /// 根据key获取RedisValue
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<RedisValue[]> ListRangeAsync(string redisKey)
        {
            return await redisDB.ListRangeAsync(redisKey);
        }

        /// <summary>
        /// 移除并返回存储在该键列表的第一个元素   
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<string> ListLeftPopStringAsync(string redisKey)
        {
            return await redisDB.ListLeftPopAsync(redisKey);
        }

        /// <summary>
        /// 移除并返回存储在该键列表的第一个元素并序列化为datatable   
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<DataTable> ListLeftPopDTAsync(string redisKey)
        {
            string infos = await redisDB.ListLeftPopAsync(redisKey);
            DataTable table = JsonConvert.DeserializeObject<DataTable>(infos);
            return table;
        }

        /// <summary>
        /// 移除并返回存储在该键列表的第一个元素  反序列化
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<T> ListLeftPopAsync<T>(string redisKey) where T : class
        {
            var cacheValue = await redisDB.ListLeftPopAsync(redisKey);
            if (string.IsNullOrEmpty(cacheValue)) return null;
            var res = JsonConvert.DeserializeObject<T>(cacheValue);
            return res;
        }

        /// <summary>
        /// 移除并返回存储在该键列表的最后一个元素   
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<string> ListRightPopAsync(string redisKey)
        {
            return await redisDB.ListRightPopAsync(redisKey);
        }


    }
}
