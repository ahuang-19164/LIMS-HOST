﻿namespace Yichen.Per.Model
{

    /// <summary>
    /// 接收信息查询
    /// </summary>
    public class ReceiveSelectModel
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 用户密钥
        /// </summary>
        public string? UserToken { get; set; }
        ///// <summary>
        ///// 录入表明
        ///// </summary>
        //public string? TableName { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string? startTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string? endTime { get; set; }
        ///// <summary>
        ///// 样本状态
        ///// </summary>
        //public string? sampleState { get; set; }
        /// <summary>
        /// 专业组编号
        /// </summary>
        public string? groupNO { get; set; }



        public string? barcode { get; set; }
        public string? hosbarcode { get; set; }

    }


    /// <summary>
    /// 接收信息对象
    /// </summary>
    public class ReceiveInfoModel
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 用户密钥
        /// </summary>
        public string? UserToken { get; set; }
        /// <summary>
        /// 接收到时间
        /// </summary>
        public string? ReceiveTime { get; set; }
        /// <summary>
        /// 检验接收信息集合
        /// </summary>

        public List<ReceiveSampleInfoModel> ReceiveInfos { get; set; }


    }
    public class ReceiveSampleInfoModel
    {
        public int perid { get; set; } = 0;
        public int testid { get; set; } = 0;
        public string? sampleid { get; set; } = "";
        /// <summary>
        /// 专业组编号
        /// </summary>
        public string? groupNO { get; set; }
        /// <summary>
        /// 专业组名称
        /// </summary>
        public string? groupName { get; set; }
        public string? barcode { get; set; }
        public string? testNo { get; set; }
        public string? frameNo { get; set; }
    }
}
