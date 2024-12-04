using SqlSugar.Extensions;
using System;

namespace Yichen.Net.Configuration
{
    /// <summary>
    /// 配置文件格式化
    /// </summary>
    public class AppSettingsConstVars
    {

        #region 全局地址================================================================================
        /// <summary>
        /// 系统后端地址
        /// </summary>
        public static readonly string AppConfigAppUrl = AppSettingsHelper.GetContent("AppConfig", "AppUrl");
        /// <summary>
        /// 系统接口地址
        /// </summary>
        public static readonly string AppConfigAppInterFaceUrl = AppSettingsHelper.GetContent("AppConfig", "AppInterFaceUrl");


        #endregion


        #region 服务文件

        public static readonly string[] servicefiles = AppSettingsHelper.GetContent("services", "servicesnames").Split(',');
        public static readonly string[] repositoryfiles = AppSettingsHelper.GetContent("services", "repositorynames").Split(',');

        #endregion


        #region 系统功能配置
        /// <summary>
        /// 是否开启接收功能
        /// </summary>
        public static readonly bool ReceiveState = Convert.ToBoolean(AppSettingsHelper.GetContent("systemfunction", "ReceiveState"));
        /// <summary>
        /// 是否开启分拣功能
        /// </summary>
        public static readonly bool SampleSort = Convert.ToBoolean(AppSettingsHelper.GetContent("systemfunction", "SampleSort"));
        /// <summary>
        /// 报告生成器数量
        /// </summary>
        public static readonly int ReportCount = Convert.ToInt32(AppSettingsHelper.GetContent("systemfunction", "ReportCount"));

        #endregion

        #region 文件路径
        /// <summary>
        /// 检验结果附件存储路径
        /// </summary>
        public static readonly string TestFilePath = AppSettingsHelper.GetContent("filepath", "testFilePath");
        /// <summary>
        /// 压缩程序地址
        /// </summary>
        public static readonly string RarApplictionPath = AppSettingsHelper.GetContent("filepath", "RarApplictionPath");
        /// <summary>
        /// 客户端升级文件地址
        /// </summary>
        public static readonly string ClientFilePath = AppSettingsHelper.GetContent("filepath", "ClientFilePath");
        /// <summary>
        /// 报告端升级文件地址
        /// </summary>
        public static readonly string ReportToolPath = AppSettingsHelper.GetContent("filepath", "ReportToolPath");
        /// <summary>
        /// 上传报告文件地址
        /// </summary>
        public static readonly string UpdateReportPath = AppSettingsHelper.GetContent("filepath", "UpdateReportPath");
        /// <summary>
        /// 报告临时文件存放地址
        /// </summary>
        public static readonly string ReportFilePath = AppSettingsHelper.GetContent("filepath", "ReportFilePath");
        /// <summary>
        /// 公共文件存放地址
        /// </summary>
        public static readonly string baseFilePath = AppSettingsHelper.GetContent("filepath", "baseFilePath");
        /// <summary>
        /// 流程文件存放地址
        /// </summary>
        public static readonly string flowFilePath = AppSettingsHelper.GetContent("filepath", "flowFilePath");
        /// <summary>
        /// 报告文件存放地址
        /// </summary>
        public static readonly string readReportFilePath = AppSettingsHelper.GetContent("filepath", "readReportFilePath");


        #endregion



        #region 配置密钥
        /// <summary>
        /// 登录密码密钥
        /// </summary>
        public static string SecretUser = AppSettingsHelper.GetContent("Secret:User");
        /// <summary>
        /// 数据密钥
        /// </summary>
        public static string SecretDB = AppSettingsHelper.GetContent("Secret:DB");
        /// <summary>
        /// redis密钥
        /// </summary>
        public static string SecretRedis = AppSettingsHelper.GetContent("Secret:Redis");
        /// <summary>
        /// Jwt密钥
        /// </summary>
        public static string SecretJWT = AppSettingsHelper.GetContent("Secret:JWT");
        /// <summary>
        /// 授权密钥
        /// </summary>
        public static string SecretAudience = AppSettingsHelper.GetContent("Secret:Audience");
        /// <summary>
        /// 发行密钥
        /// </summary>
        public static string SecretIssuer = AppSettingsHelper.GetContent("Secret:Issuer");
        /// <summary>
        /// 导出文件加密key
        /// </summary>
        public static string ExportFile = "C5ABA9E202D94C13A3CB66002BF77FAF";




        #endregion


        #region 数据库================================================================================
        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        public static readonly string DbSqlConnection = AppSettingsHelper.GetContent("ConnectionStrings", "SqlConnection");
        /// <summary>
        /// 获取数据库类型
        /// </summary>
        public static readonly string DbDbType = AppSettingsHelper.GetContent("ConnectionStrings", "DbType");

        #endregion

        #region redis================================================================================

        /// <summary>
        /// 获取redis连接字符串
        /// </summary>
        public static readonly string RedisConfigConnectionString = AppSettingsHelper.GetContent("RedisConfig", "ConnectionString");
        /// <summary>
        /// 报告分发Redis链接字符串
        /// </summary>
        public static readonly string RedisReportConnectionString = AppSettingsHelper.GetContent("RedisConfig", "ReportConnectionString");

        /// <summary>
        /// 启用redis作为缓存选择
        /// </summary>
        public static readonly bool RedisUseCache = AppSettingsHelper.GetContent("RedisConfig", "UseCache").ObjToBool();
        /// <summary>
        /// 启用redis作为定时任务
        /// </summary>
        public static readonly bool RedisUseTimedTask = AppSettingsHelper.GetContent("RedisConfig", "UseTimedTask").ObjToBool();

        #endregion

        #region AOP================================================================================
        /// <summary>
        /// 事务切面开关
        /// </summary>
        public static readonly bool TranAopEnabled = AppSettingsHelper.GetContent("TranAOP", "Enabled").ObjToBool();

        #endregion

        #region Jwt授权配置================================================================================

        public static readonly string JwtConfigSecretKey = AppSettingsHelper.GetContent("JwtConfig", "SecretKey");
        public static readonly string JwtConfigIssuer = AppSettingsHelper.GetContent("JwtConfig", "Issuer");
        public static readonly string JwtConfigAudience = AppSettingsHelper.GetContent("JwtConfig", "Audience");
        #endregion

        #region Cors跨域设置================================================================================
        public static readonly string CorsPolicyName = AppSettingsHelper.GetContent("Cors", "PolicyName");
        public static readonly bool CorsEnableAllIPs = AppSettingsHelper.GetContent("Cors", "EnableAllIPs").ObjToBool();
        public static readonly string CorsIPs = AppSettingsHelper.GetContent("Cors", "IPs");
        #endregion

        #region Middleware中间件================================================================================
        /// <summary>
        /// Ip限流
        /// </summary>
        public static readonly bool MiddlewareIpLogEnabled = AppSettingsHelper.GetContent("Middleware", "IPLog", "Enabled").ObjToBool();
        /// <summary>
        /// 记录请求与返回数据
        /// </summary>
        public static readonly bool MiddlewareRequestResponseLogEnabled = AppSettingsHelper.GetContent("Middleware", "RequestResponseLog", "Enabled").ObjToBool();
        /// <summary>
        /// 用户访问记录-是否开启
        /// </summary>
        public static readonly bool MiddlewareRecordAccessLogsEnabled = AppSettingsHelper.GetContent("Middleware", "RecordAccessLogs", "Enabled").ObjToBool();
        /// <summary>
        /// 用户访问记录-过滤ip
        /// </summary>
        public static readonly string MiddlewareRecordAccessLogsIgnoreApis = AppSettingsHelper.GetContent("Middleware", "RecordAccessLogs", "IgnoreApis");

        #endregion

        #region 支付================================================================================

        /// <summary>
        /// 微信支付回调
        /// </summary>
        public static readonly string PayCallBackWeChatPayUrl = AppSettingsHelper.GetContent("PayCallBack", "WeChatPayUrl");
        /// <summary>
        /// 微信退款回调
        /// </summary>
        public static readonly string PayCallBackWeChatRefundUrl = AppSettingsHelper.GetContent("PayCallBack", "WeChatRefundUrl");
        /// <summary>
        /// 支付宝支付回调
        /// </summary>
        public static readonly string PayCallBackAlipayUrl = AppSettingsHelper.GetContent("PayCallBack", "AlipayUrl");
        /// <summary>
        /// 支付宝退款回调
        /// </summary>
        public static readonly string PayCallBackAlipayRefundUrl = AppSettingsHelper.GetContent("PayCallBack", "AlipayRefundUrl");
        #endregion

        #region 易联云打印机================================================================================

        /// <summary>
        /// 是否开启
        /// </summary>
        public static readonly bool YiLianYunConfigEnabled = AppSettingsHelper.GetContent("YiLianYunConfig", "Enabled").ObjToBool();
        /// <summary>
        /// 应用ID
        /// </summary>
        public static readonly string YiLianYunConfigClientId = AppSettingsHelper.GetContent("YiLianYunConfig", "ClientId");
        /// <summary>
        /// 应用密钥
        /// </summary>
        public static readonly string YiLianYunConfigClientSecret = AppSettingsHelper.GetContent("YiLianYunConfig", "ClientSecret");
        /// <summary>
        /// 打印机设备号
        /// </summary>
        public static readonly string YiLianYunConfigMachineCode = AppSettingsHelper.GetContent("YiLianYunConfig", "MachineCode");
        /// <summary>
        /// 打印机终端密钥
        /// </summary>
        public static readonly string YiLianYunConfigMsign = AppSettingsHelper.GetContent("YiLianYunConfig", "Msign");
        /// <summary>
        /// 打印机名称
        /// </summary>
        public static readonly string YiLianYunConfigPrinterName = AppSettingsHelper.GetContent("YiLianYunConfig", "PrinterName");
        /// <summary>
        /// 打印机设置联系方式
        /// </summary>
        public static readonly string YiLianYunConfigPhone = AppSettingsHelper.GetContent("YiLianYunConfig", "Phone");

        #endregion

        #region HangFire定时任务================================================================================
        /// <summary>
        /// 登录账号
        /// </summary>
        public static readonly string HangFireLogin = AppSettingsHelper.GetContent("HangFire", "Login");
        /// <summary>
        /// 登录密码
        /// </summary>
        public static readonly string HangFirePassWord = AppSettingsHelper.GetContent("HangFire", "PassWord");


        #endregion

    }
}