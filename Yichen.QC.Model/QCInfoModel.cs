namespace Yichen.QC.Model
{
    /// <summary>
    /// 新增QC信息
    /// </summary>
    public class QCAddModel
    {
        public bool dstate { get; set; }
        public bool resultState { get; set; }
        public bool state { get; set; }
        //public DateTime createTime { get; set; }
        public string? qcTime { get; set; }
        public int id { get; set; }
        public int sort { get; set; } = 0;
        //public string? creater { get; set; }
        public string? itemNO { get; set; }
        public string? itemResult { get; set; }
        public string? planGradeid { get; set; }
        public string? planid { get; set; }
        public string? planItemid { get; set; }
        public string? remark { get; set; }
        public string? resultRule { get; set; }
        public string? resultType { get; set; }
        public string? zVlaue { get; set; }
    }

    /// <summary>
    /// 查询QC相关信息
    /// </summary>
    public class QCSelectValueModel
    {

        /// <summary>
        /// 质控计划id
        /// </summary>
        public string? planid { get; set; }
        /// <summary>
        /// 质控品id
        /// </summary>
        public string? planGradeid { get; set; }
        /// <summary>
        /// 计划项目id
        /// </summary>
        public string? planItemid { get; set; }
        /// <summary>
        /// 项目编号
        /// </summary>
        public string? itemNO { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string? startTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string? endTime { get; set; }
    }
}
