namespace Yichen.Manage.Model
{
    /// <summary>
    /// 提交检验项目信息
    /// </summary>
    public class CRMInfohandleModel
    {
        public int id { get; set; } = 0;
        public int perid { get; set; } = 0;
        public int testid { get; set; } = 0;
        /// <summary>
        /// true 正常处理；false取消处理
        /// </summary>
        public bool? infoState { get; set; }
        public string? barcode { get; set; }
        public bool dstate { get; set; } = false;
        public bool state { get; set; } = true;
        public DateTime handleTime { get; set; } = DateTime.Now;
        public DateTime createTime { get; set; } = DateTime.Now;


        public string contact { get; set; }

        public string creater { get; set; }

        public string handler { get; set; }

        public string applyCodes { get; set; }

        public string applyNames { get; set; }

        public string contactMode { get; set; }

        public string handleResult { get; set; }

        public string handleTypeNO { get; set; }

        public string pleasLevel { get; set; }

        public string recordTypeNO { get; set; }

        public string recordValue { get; set; }

        public string remark { get; set; }

        public string submitItemCodes { get; set; }
        public string submitItemNames { get; set; }

        //public bit state { get; set; }
    }
}
