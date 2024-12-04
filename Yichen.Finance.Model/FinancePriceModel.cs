namespace Yichen.Finance.Model
{
    /// <summary>
    /// 折扣价格信息
    /// </summary>
    public class DiscountPriceModel
    {
        /// <summary>
        /// 是否引用专业组折扣
        /// </summary>
        public bool groupDiscountState { get; set; } = false;
        /// <summary>
        /// 标准价格
        /// </summary>
        public decimal standerPirce { get; set; } = 0;
        /// <summary>
        /// 结算价格
        /// </summary>
        public decimal settlementPirce { get; set; } = 0;
        /// <summary>
        /// 收费价格
        /// </summary>
        public decimal chargePice { get; set; } = 0;
        /// <summary>
        /// 收费类型编号
        /// </summary>
        public string? chargeTypeNO { get; set; } = "0";

    }
}
