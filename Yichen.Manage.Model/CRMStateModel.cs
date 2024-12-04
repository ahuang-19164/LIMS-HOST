namespace Yichen.Manage.Model
{
    /// <summary>
    /// 检验信息处理对象
    /// </summary>
    public class CRMStateModel
    {
        /// <summary>
        /// 信息状态 0，成功 1，失败
        /// </summary>
        public int code { get; set; } = 0;
        /// <summary>
        /// 处理状态值
        /// </summary>
        public int handleState { get; set; }
        /// <summary>
        /// 检验状态值
        /// </summary>
        public int testStateNO { get; set; }
        /// <summary>
        /// 处理消息
        /// </summary>

        public string msg { get; set; }

    }
}
