namespace Yichen.Report.Model
{
    /// <summary>
    /// 获取报告信息
    /// </summary>
    public class GetReportModel
    {
        /// <summary>
        /// 用户名
        /// </summary>

        public string userName { get; set; }

        /// <summary>
        /// 用户验证
        /// </summary>
        public string? userToken { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? ClientPower { get; set; }
        /// <summary>
        ///查询类型 0.报告端查询，1，客户端查询
        /// </summary>
        public int reportType { get; set; } = 0;
        /// <summary>
        /// 检验中信息id
        /// </summary>

        public List<int> infoID { get; set; }

    }

    /// <summary>
    /// 上传报告
    /// </summary>
    public class UpLoadReportModel
    {

        public string userName { get; set; }

        public string? userToken { get; set; }
        /// <summary>
        /// 0,上传操作，1清空操作
        /// </summary>
        public int upState { get; set; } = 0;

        /// <summary>
        /// 录入id
        /// </summary>
        public int perid { get; set; }
        /// <summary>
        /// 检验id
        /// </summary>
        public int testid { get; set; }
        /// <summary>
        /// 条码号
        /// </summary>

        public string barcode { get; set; }

        /// <summary>
        /// 医院编号
        /// </summary>

        public string hospitalNo { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string? FileName { get; set; }
        /// <summary>
        /// 文件string
        /// </summary>

        public string FileString { get; set; }

    }
}
