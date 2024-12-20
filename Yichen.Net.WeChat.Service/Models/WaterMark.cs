﻿/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/7/29 1:09:19
 *        Description: 暂无
 ***********************************************************************/


using System;
using Yichen.Net.WeChat.Service.Utilities;

namespace Yichen.Net.WeChat.Service.Models
{
    /// <summary>
    /// 水印
    /// </summary>
    [Serializable]
    public class Watermark
    {

        public string? appid { get; set; }

        public long timestamp { get; set; }

        public DateTimeOffset DateTimeStamp
        {
            get { return DateTimeHelper.GetDateTimeFromXml(timestamp); }
        }
    }
}
