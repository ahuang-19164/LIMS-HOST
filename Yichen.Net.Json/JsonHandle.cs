using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Yichen.Net.Json
{
    public class JsonHandle
    {
        /// <summary>
        /// 将对象序列化为json格式
        /// </summary>
        /// <param name="obj">序列化对象</param>
        /// <returns>json字符串</returns>
        public static string SerializeObjct(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// 解析JSON字符串生成对象实体
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="json">JSON字符串</param>
        /// <returns></returns>
        public static T JsonConvertObject<T>(string json)
        {
            if (json != "")
            {

                return JsonConvert.DeserializeObject<T>(json);

            }
            else
            {

                return default(T);

            }

        }
        /// <summary>
        /// 解析JSON字符串生成对象实体
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <returns></returns>
        public static T DeserializeJsonToObject<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);

            object obj = serializer.Deserialize(new JsonTextReader(sr), typeof(T));


            T t = obj as T;


            return t;

        }
        /// <summary>
        /// 解析JSON数组生成对象实体集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数组</param>
        /// <returns>对象实体集合</returns>
        public static List<T> DeserializeJsonToList<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);

            object obj = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));


            List<T> list = obj as List<T>;


            return list;

        }
        /// <summary>
        /// 将JSON转数组
        /// 用法:jsonArr[0]["xxxx"]
        /// </summary>
        /// <param name="json">json字符串</param>
        /// <returns></returns>
        public static JArray GetToJsonList(string json)
        {

            JArray jsonArr = (JArray)JsonConvert.DeserializeObject(json);


            return jsonArr;

        }
        /// <summary>
        /// 将DataTable转换成实体类
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        public static List<T> DtConvertToModel<T>(DataTable dt) where T : new()
        {
            List<T> ts = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                foreach (PropertyInfo pi in t.GetType().GetProperties())
                {
                    if (dt.Columns.Contains(pi.Name))
                    {
                        if (!pi.CanWrite) continue;
                        var value = dr[pi.Name];
                        if (value != DBNull.Value)
                        {
                            switch (pi.PropertyType.FullName)
                            {
                                case "System.Decimal":
                                    pi.SetValue(t, decimal.Parse(value.ToString()), null);
                                    break;
                                case "System.String":
                                    pi.SetValue(t, value.ToString(), null);
                                    break;
                                case "System.Int32":
                                    pi.SetValue(t, int.Parse(value.ToString()), null);
                                    break;
                                default:
                                    pi.SetValue(t, value, null);
                                    break;
                            }
                        }
                    }
                }
                ts.Add(t);
            }
            return ts;
        }
        public static DataTable JsonToDataTable(string strJson)
        {
            //转换json格式
            strJson = strJson.Replace(",\"", "*\"").Replace("\":", "\"#").ToString();
            //取出表名   
            var rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
            string strName = rg.Match(strJson).Value;

            DataTable tb = null;

            //去除表名   
            strJson = strJson.Substring(strJson.IndexOf("[") + 1);
            strJson = strJson.Substring(0, strJson.IndexOf("]"));

            //获取数据   
            rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value;
                string[] strRows = strRow.Split('*');

                //创建表   
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = strName;
                    foreach (string str in strRows)
                    {
                        var dc = new DataColumn();
                        string[] strCell = str.Split('#');

                        if (strCell[0].Substring(0, 1) == "\"")
                        {
                            int a = strCell[0].Length;
                            dc.ColumnName = strCell[0].Substring(1, a - 2);
                        }
                        else
                        {
                            dc.ColumnName = strCell[0];
                        }
                        tb.Columns.Add(dc);
                    }
                    tb.AcceptChanges();
                }

                //增加内容   
                DataRow dr = tb.NewRow();
                for (int r = 0; r < strRows.Length; r++)
                {
                    try
                    {
                        string a = strRows[r].Split('#')[1].Trim();
                        if (a.Equals("null"))
                        {
                            dr[r] = "";
                        }
                        else
                        {
                            dr[r] = strRows[r].Split('#')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
                        }
                    }
                    catch (Exception e)
                    {

                        throw e;
                    }
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }

            try
            {
                if (tb != null)
                {
                    return tb;
                }
                else
                {
                    throw new Exception("解析错误");
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }




        #region 自定义字符串方法

        public static string TableToString(DataTable dataTable)
        {
            try
            {
                string datastr = string.Empty;
                if (dataTable != null)
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            datastr += column.ColumnName + "||,";
                        }
                        for (int r = 0; r < dataTable.Rows.Count; r++)
                        {
                            string rowvalue = string.Empty;
                            for (int c = 0; c < dataTable.Columns.Count; c++)
                            {
                                if (!(Convert.IsDBNull(dataTable.Rows[r][c])))
                                {
                                    rowvalue += dataTable.Rows[r][c].ToString() + "//,";
                                }
                                else
                                {
                                    rowvalue += "//,";
                                }
                            }
                            datastr += rowvalue + "/s/n";
                        }
                    }
                }
                return datastr;
            }
            catch
            {

                return null;

            }
        }
        public static DataTable StringToTable(string dataTableStr)
        {
            try
            {
                if (dataTableStr.Length > 0)
                {
                    DataTable dataTable = new DataTable();
                    dataTableStr = dataTableStr.Substring(0, dataTableStr.Length - 4);
                    string[] Cstr = dataTableStr.Split(new string[] { "||," }, StringSplitOptions.None);
                    for (int c = 0; c < Cstr.Length - 1; c++)
                    {
                        dataTable.Columns.Add(Cstr[c]);
                    }
                    string laststr = Cstr[Cstr.Length - 1];
                    string[] rows = Cstr[Cstr.Length - 1].Split(new string[] { "/s/n" }, StringSplitOptions.None);


                    for (int rs = 0; rs < rows.Length; rs++)
                    {
                        dataTable.Rows.Add(rows[rs].Substring(0, rows[rs].Length - 3).Split(new string[] { "//," }, StringSplitOptions.None));
                    }
                    return dataTable;
                }
                else
                {

                    return null;

                }
            }
            catch
            {

                return null;

            }


        }

        #endregion
    }
}
