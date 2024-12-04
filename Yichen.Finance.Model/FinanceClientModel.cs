namespace Yichen.Finance.Model
{
    /// <summary>
    /// 客户收费等级折扣信息
    /// </summary>
    public class FinanceClientModel
    {
        /// <summary>
        /// 客户编号
        /// </summary>
        public string? clientNo { get; set; }
        /// <summary>
        /// 收费等级
        /// </summary>
        public string? chargeLevelNO { get; set; }
        /// <summary>
        /// 折扣率
        /// </summary>
        public double discount { get; set; } = 1;
        /// <summary>
        /// 负责人
        /// </summary>
        public string? personNO { get; set; } = "";
    }
}
