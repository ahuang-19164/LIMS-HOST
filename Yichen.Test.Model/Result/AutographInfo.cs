﻿namespace Yichen.Test.Model.Result
{
    /// <summary>
    /// 代审核对象
    /// </summary>
    public class AutographInfo
    {
        /// <summary>
        /// 样本代审核状态false 不代审核，true 代审核
        /// </summary>
        public bool delState { get; set; } = false;
        /// <summary>
        /// 是否使用待审核信息
        /// </summary>
        public bool state { get; set; }
        /// <summary>
        /// 检验者
        /// </summary>
        public string? tester { get; set; }
        /// <summary>
        /// 检测时间
        /// </summary>
        public DateTime? testTime { get; set; }
        /// <summary>
        /// 初审者
        /// </summary>
        public string? reTester { get; set; }
        /// <summary>
        /// 初审时间
        /// </summary>
        public DateTime? reTestTime { get; set; }
        /// <summary>
        /// 审核者
        /// </summary>
        public string? checker { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? checkTime { get; set; }
        /// <summary>
        /// 检验备注信息
        /// </summary>
        public string? testRemark { get; set; }
    }
}
