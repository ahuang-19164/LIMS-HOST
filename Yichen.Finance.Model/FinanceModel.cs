﻿namespace Yichen.Finance.Model
{

    #region  查询财务信息对象
    /// <summary>
    /// 财务信息查询
    /// </summary>
    public class SelectFinanceModel
    {
        /// <summary>
        /// 查询信息键值对
        /// </summary>
        public List<PairsFinanceModel>? PairsInfo { get; set; }
        /// <summary>
        /// 采样起始时间
        /// </summary>
        public string? sampleTimeStart { get; set; }
        /// <summary>
        /// 采样起始结束
        /// </summary>
        public string? sampleTimeEnd { get; set; }
        /// <summary>
        /// 物流接收起始时间
        /// </summary>
        public string? receiveTimeStart { get; set; }
        /// <summary>
        /// 物流接收结束时间
        /// </summary>
        public string? receiveTimeEnd { get; set; }
        /// <summary>
        /// 录入起始时间
        /// </summary>
        public string? perTimeStart { get; set; }
        /// <summary>
        /// 录入结束时间
        /// </summary>
        public string? perTimeEnd { get; set; }


    }
    /// <summary>
    /// 查询键值信息
    /// </summary>
    public class PairsFinanceModel
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string? keyName { get; set; }
        /// <summary>
        ///查询类型 0，=||1,in||2，like||3，<||4，>||5，<=||6，>=
        /// </summary>
        public string? type { get; set; }
        /// <summary>
        /// 查询id
        /// </summary>
        public string? keyNO { get; set; }
        /// <summary>
        /// 查询值
        /// </summary>
        public string? keyValue { get; set; }
    }
    #endregion




    #region 

    /// <summary>
    /// 价格修改
    /// </summary>
    public class priceChangeModel
    {
        /// <summary>
        /// 收费信息ID
        /// </summary>
        public string? financeID { get; set; }
        /// <summary>
        /// 条码号
        /// </summary>
        public string? barcode { get; set; }
        /// <summary>
        /// 修改价格
        /// </summary>
        public string? charge { get; set; }
    }

    #endregion






}
