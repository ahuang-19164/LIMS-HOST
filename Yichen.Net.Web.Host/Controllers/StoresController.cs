/***********************************************************************
 *            Project: Yichen
 *        ProjectName: 屹辰智禾管理系统                                
 *                Web: https://www.zui51.com                 
 *             Author: 屹辰                                       
 *              Email: 499715561@qq.com                              
 *         CreateTime: 2023-11-16 11:56:59
 *        Description: 暂无
 ***********************************************************************/


using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Yichen.Net.Configuration;
using Yichen.Net.Model.Entities;
using Yichen.Net.Model.Entities.Expression;
using Yichen.Net.Model.FromBody;
using Yichen.Net.Filter;
using Yichen.Net.Loging;
using Yichen.Stores.IServices;
using Yichen.Net.Utility.Helper;
using Yichen.Net.Utility.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using SqlSugar;
using Yichen.Stores.Model;
using Yichen.Comm.Model.ViewModels.UI;
using Yichen.Comm.Model;

namespace Yichen.Net.Web.Admin.Controllers
{
    /// <summary>
    /// 
    ///</summary>
    [Description("")]
    [Route("api/[controller]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize]
    public class StoresController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly Isw_storesServices _sw_storesServices;

        /// <summary>
        /// 构造函数
        ///</summary>
        public StoresController(IWebHostEnvironment webHostEnvironment
            ,Isw_storesServices sw_storesServices
            )
        {
            _webHostEnvironment = webHostEnvironment;
            _sw_storesServices = sw_storesServices;
        }
        /// <summary>
        /// 获取财务流水号
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("GetShelfSerial")]
        [Authorize]
        public async Task<WebApiCallBack> GetShelfSerial()
        {
            return await _sw_storesServices.GetShelfSerial();
        }


        /// <summary>
        /// 存储标本处理
        /// </summary>
        /// <returns></returns>
        /// state=(1正常2已处理3已过期4其他)
        [HttpPost, Route("RecordHandle")]
        [Authorize]
        public async Task<WebApiCallBack> RecordHandle(commInfoModel<string> comminfo)
        {
            return await _sw_storesServices.RecordHandle(comminfo);
        }














        //      #region 获取列表============================================================
        //      // POST: Api/sw_stores/GetPageList
        //       /// <summary>
        //      /// 获取列表
        //      /// </summary>
        //      /// <returns></returns>
        //      [HttpPost]
        //      [Description("获取列表")]
        //      public async Task<WebApiCallBack> GetPageList()
        //      {
        //          var jm = new WebApiCallBack();
        //          //var aaaa= Request.Form["Authorization"].FirstOrDefault().ObjectToString();

        //          //Console.WriteLine(aaaa);
        //          var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
        //          var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
        //          var where = PredicateBuilder.True<sw_stores>();
        //          //获取排序字段
        //          var orderField = Request.Form["orderField"].FirstOrDefault();

        //          Expression<Func<sw_stores, object>> orderEx = orderField switch
        //          {
        //              "id" => p => p.id,"no" => p => p.no,"names" => p => p.names,"shortNames" => p => p.shortNames,"customCode" => p => p.customCode,"address" => p => p.address,"saveDay" => p => p.saveDay,"shoresRow" => p => p.shoresRow,"shoresCell" => p => p.shoresCell,"sampleType" => p => p.sampleType,"remark" => p => p.remark,"sore" => p => p.sore,"creater" => p => p.creater,"createTime" => p => p.createTime,"state" => p => p.state,"dstate" => p => p.dstate,
        //              _ => p => p.id
        //          };

        //          //设置排序方式
        //          var orderDirection = Request.Form["orderDirection"].FirstOrDefault();
        //          var orderBy = orderDirection switch
        //          {
        //              "asc" => OrderByType.Asc,
        //              "desc" => OrderByType.Desc,
        //              _ => OrderByType.Desc
        //          };
        //          //查询筛选

        //	//id int
        //	var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
        //          if (id > 0)
        //          {
        //              where = where.And(p => p.id == id);
        //          }
        //	//编号 varchar
        //	var no = Request.Form["no"].FirstOrDefault();
        //          if (!string.IsNullOrEmpty(no))
        //          {
        //              where = where.And(p => p.no.Contains(no));
        //          }
        //	//名称 varchar
        //	var names = Request.Form["names"].FirstOrDefault();
        //          if (!string.IsNullOrEmpty(names))
        //          {
        //              where = where.And(p => p.names.Contains(names));
        //          }
        //	//拼音缩写 varchar
        //	var shortNames = Request.Form["shortNames"].FirstOrDefault();
        //          if (!string.IsNullOrEmpty(shortNames))
        //          {
        //              where = where.And(p => p.shortNames.Contains(shortNames));
        //          }
        //	//自定编码 varchar
        //	var customCode = Request.Form["customCode"].FirstOrDefault();
        //          if (!string.IsNullOrEmpty(customCode))
        //          {
        //              where = where.And(p => p.customCode.Contains(customCode));
        //          }
        //	//存储地址 varchar
        //	var address = Request.Form["address"].FirstOrDefault();
        //          if (!string.IsNullOrEmpty(address))
        //          {
        //              where = where.And(p => p.address.Contains(address));
        //          }
        //	//储存天数 int
        //	var saveDay = Request.Form["saveDay"].FirstOrDefault().ObjectToInt(0);
        //          if (saveDay > 0)
        //          {
        //              where = where.And(p => p.saveDay == saveDay);
        //          }
        //	//默认行数 int
        //	var shoresRow = Request.Form["shoresRow"].FirstOrDefault().ObjectToInt(0);
        //          if (shoresRow > 0)
        //          {
        //              where = where.And(p => p.shoresRow == shoresRow);
        //          }
        //	//默认列数 int
        //	var shoresCell = Request.Form["shoresCell"].FirstOrDefault().ObjectToInt(0);
        //          if (shoresCell > 0)
        //          {
        //              where = where.And(p => p.shoresCell == shoresCell);
        //          }
        //	//存储样本类型 varchar
        //	var sampleType = Request.Form["sampleType"].FirstOrDefault();
        //          if (!string.IsNullOrEmpty(sampleType))
        //          {
        //              where = where.And(p => p.sampleType.Contains(sampleType));
        //          }
        //	//备注信息 varchar
        //	var remark = Request.Form["remark"].FirstOrDefault();
        //          if (!string.IsNullOrEmpty(remark))
        //          {
        //              where = where.And(p => p.remark.Contains(remark));
        //          }
        //	//排序 int
        //	var sore = Request.Form["sore"].FirstOrDefault().ObjectToInt(0);
        //          if (sore > 0)
        //          {
        //              where = where.And(p => p.sore == sore);
        //          }
        //	//创建人 varchar
        //	var creater = Request.Form["creater"].FirstOrDefault();
        //          if (!string.IsNullOrEmpty(creater))
        //          {
        //              where = where.And(p => p.creater.Contains(creater));
        //          }
        //	////创建时间 datetime2
        //	//var createTime = Request.Form["createTime"].FirstOrDefault();
        // //         if (!string.IsNullOrEmpty(createTime))
        // //         {
        // //             where = where.And(p => p.createTime.Contains(createTime));
        // //         }
        //	//是否启用 bit
        //	var state = Request.Form["state"].FirstOrDefault();
        //          if (!string.IsNullOrEmpty(state) && state.ToLowerInvariant() == "true")
        //          {
        //              where = where.And(p => p.state == true);
        //          }
        //          else if (!string.IsNullOrEmpty(state) && state.ToLowerInvariant() == "false")
        //          {
        //              where = where.And(p => p.state == false);
        //          }
        //	//是否删除 bit
        //	var dstate = Request.Form["dstate"].FirstOrDefault();
        //          if (!string.IsNullOrEmpty(dstate) && dstate.ToLowerInvariant() == "true")
        //          {
        //              where = where.And(p => p.dstate == true);
        //          }
        //          else if (!string.IsNullOrEmpty(dstate) && dstate.ToLowerInvariant() == "false")
        //          {
        //              where = where.And(p => p.dstate == false);
        //          }
        //          //获取数据
        //          var list = await _sw_storesServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize, true);
        //          //返回数据
        //          jm.data = list;
        //          jm.code = 0;
        //          jm.otherData = list.TotalCount;
        //          jm.msg = "数据调用成功!";
        //          return jm;
        //      }
        //      #endregion

        //      #region 首页数据============================================================
        //      // POST: Api/sw_stores/GetIndex
        //      /// <summary>
        //      /// 首页数据
        //      /// </summary>
        //      /// <returns></returns>
        //      [HttpPost]
        //      [Description("首页数据")]
        //      public WebApiCallBack GetIndex()
        //      {
        //          //返回数据
        //          var jm = new WebApiCallBack { code = 0 };
        //          return jm;
        //      }
        //      #endregion

        //      #region 创建数据============================================================
        //      // POST: Api/sw_stores/GetCreate
        //      /// <summary>
        //      /// 创建数据
        //      /// </summary>
        //      /// <returns></returns>
        //      [HttpPost]
        //      [Description("创建数据")]
        //      public WebApiCallBack GetCreate()
        //      {
        //          //返回数据
        //          var jm = new WebApiCallBack { code = 0 };
        //          return jm;
        //      }
        //      #endregion

        //      #region 创建提交============================================================
        //      // POST: Api/sw_stores/DoCreate
        //      /// <summary>
        //      /// 创建提交
        //      /// </summary>
        //      /// <param name="entity"></param>
        //      /// <returns></returns>
        //      [HttpPost]
        //      [Description("创建提交")]
        //      public async Task<WebApiCallBack> DoCreate([FromBody]sw_stores entity)
        //      {
        //          var jm = await _sw_storesServices.InsertAsync(entity);
        //          return jm;
        //      }
        //      #endregion

        //      #region 编辑数据============================================================
        //      // POST: Api/sw_stores/GetEdit
        //      /// <summary>
        //      /// 编辑数据
        //      /// </summary>
        //      /// <param name="entity"></param>
        //      /// <returns></returns>
        //      [HttpPost]
        //      [Description("编辑数据")]
        //      public async Task<WebApiCallBack> GetEdit([FromBody]FMIntId entity)
        //      {
        //          var jm = new WebApiCallBack();

        //          var model = await _sw_storesServices.QueryByIdAsync(entity.id, false);
        //          if (model == null)
        //          {
        //              jm.msg = "不存在此信息";
        //              return jm;
        //          }
        //          jm.code = 0;
        //          jm.data = model;

        //          return jm;
        //      }
        //      #endregion

        //      #region 编辑提交============================================================
        //      // POST: Api/sw_stores/Edit
        //      /// <summary>
        //      /// 编辑提交
        //      /// </summary>
        //      /// <param name="entity"></param>
        //      /// <returns></returns>
        //      [HttpPost]
        //      [Description("编辑提交")]
        //      public async Task<WebApiCallBack> DoEdit([FromBody]sw_stores entity)
        //      {
        //          var jm = await _sw_storesServices.UpdateAsync(entity);
        //          return jm;
        //      }
        //      #endregion

        //      #region 删除数据============================================================
        //      // POST: Api/sw_stores/DoDelete/10
        //      /// <summary>
        //      /// 单选删除
        //      /// </summary>
        //      /// <param name="entity"></param>
        //      /// <returns></returns>
        //      [HttpPost]
        //      [Description("单选删除")]
        //      public async Task<WebApiCallBack> DoDelete([FromBody]FMIntId entity)
        //      {
        //          var jm = new WebApiCallBack();

        //          var model = await _sw_storesServices.ExistsAsync(p => p.id == entity.id, true);
        //          if (!model)
        //          {
        //              jm.msg = GlobalConstVars.DataisNo;
        //		return jm;
        //          }
        //          jm = await _sw_storesServices.DeleteByIdAsync(entity.id);

        //          return jm;
        //      }
        //      #endregion

        //      #region 批量删除============================================================
        //      // POST: Api/sw_stores/DoBatchDelete/10,11,20
        //      /// <summary>
        //      /// 批量删除
        //      /// </summary>
        //      /// <param name="entity"></param>
        //      /// <returns></returns>
        //      [HttpPost]
        //      [Description("批量删除")]
        //      public async Task<WebApiCallBack> DoBatchDelete([FromBody]FMArrayIntIds entity)
        //      {
        //          var jm = await _sw_storesServices.DeleteByIdsAsync(entity.id);
        //          return jm;
        //      }

        //      #endregion

        //      #region 预览数据============================================================
        //      // POST: Api/sw_stores/GetDetails/10
        //      /// <summary>
        //      /// 预览数据
        //      /// </summary>
        //      /// <param name="entity"></param>
        //      /// <returns></returns>
        //      [HttpPost]
        //      [Description("预览数据")]
        //      public async Task<WebApiCallBack> GetDetails([FromBody]FMIntId entity)
        //      {
        //          var jm = new WebApiCallBack();

        //          var model = await _sw_storesServices.QueryByIdAsync(entity.id, false);
        //          if (model == null)
        //          {
        //              jm.msg = "不存在此信息";
        //              return jm;
        //          }
        //          jm.code = 0;
        //          jm.data = model;

        //          return jm;
        //      }
        //      #endregion

        //      #region 选择导出============================================================
        //      // POST: Api/sw_stores/SelectExportExcel/10
        //      /// <summary>
        //      /// 选择导出
        //      /// </summary>
        //      /// <param name="entity"></param>
        //      /// <returns></returns>
        //      [HttpPost]
        //      [Description("选择导出")]
        //      public async Task<WebApiCallBack> SelectExportExcel([FromBody]FMArrayIntIds entity)
        //      {
        //          var jm = new WebApiCallBack();

        //          //创建Excel文件的对象
        //          var book = new HSSFWorkbook();
        //          //添加一个sheet
        //          var mySheet = book.CreateSheet("Sheet1");
        //          //获取list数据
        //          var listModel = await _sw_storesServices.QueryListByClauseAsync(p => entity.id.Contains(p.id), p => p.id, OrderByType.Asc, true);
        //          //给sheet1添加第一行的头部标题
        //          var headerRow = mySheet.CreateRow(0);
        //          var headerStyle = ExcelHelper.GetHeaderStyle(book);

        //          var cell0 = headerRow.CreateCell(0);
        //          cell0.SetCellValue("id");
        //          cell0.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(0, 10 * 256);

        //          var cell1 = headerRow.CreateCell(1);
        //          cell1.SetCellValue("编号");
        //          cell1.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(1, 10 * 256);

        //          var cell2 = headerRow.CreateCell(2);
        //          cell2.SetCellValue("名称");
        //          cell2.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(2, 10 * 256);

        //          var cell3 = headerRow.CreateCell(3);
        //          cell3.SetCellValue("拼音缩写");
        //          cell3.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(3, 10 * 256);

        //          var cell4 = headerRow.CreateCell(4);
        //          cell4.SetCellValue("自定编码");
        //          cell4.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(4, 10 * 256);

        //          var cell5 = headerRow.CreateCell(5);
        //          cell5.SetCellValue("存储地址");
        //          cell5.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(5, 10 * 256);

        //          var cell6 = headerRow.CreateCell(6);
        //          cell6.SetCellValue("储存天数");
        //          cell6.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(6, 10 * 256);

        //          var cell7 = headerRow.CreateCell(7);
        //          cell7.SetCellValue("默认行数");
        //          cell7.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(7, 10 * 256);

        //          var cell8 = headerRow.CreateCell(8);
        //          cell8.SetCellValue("默认列数");
        //          cell8.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(8, 10 * 256);

        //          var cell9 = headerRow.CreateCell(9);
        //          cell9.SetCellValue("存储样本类型");
        //          cell9.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(9, 10 * 256);

        //          var cell10 = headerRow.CreateCell(10);
        //          cell10.SetCellValue("备注信息");
        //          cell10.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(10, 10 * 256);

        //          var cell11 = headerRow.CreateCell(11);
        //          cell11.SetCellValue("排序");
        //          cell11.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(11, 10 * 256);

        //          var cell12 = headerRow.CreateCell(12);
        //          cell12.SetCellValue("创建人");
        //          cell12.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(12, 10 * 256);

        //          var cell13 = headerRow.CreateCell(13);
        //          cell13.SetCellValue("创建时间");
        //          cell13.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(13, 10 * 256);

        //          var cell14 = headerRow.CreateCell(14);
        //          cell14.SetCellValue("是否启用");
        //          cell14.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(14, 10 * 256);

        //          var cell15 = headerRow.CreateCell(15);
        //          cell15.SetCellValue("是否删除");
        //          cell15.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(15, 10 * 256);

        //          headerRow.Height = 30 * 20;
        //          var commonCellStyle = ExcelHelper.GetCommonStyle(book);

        //          //将数据逐步写入sheet1各个行
        //          for (var i = 0; i < listModel.Count; i++)
        //          {
        //              var rowTemp = mySheet.CreateRow(i + 1);

        //                  var rowTemp0 = rowTemp.CreateCell(0);
        //                      rowTemp0.SetCellValue(listModel[i].id.ToString());
        //                      rowTemp0.CellStyle = commonCellStyle;

        //                  var rowTemp1 = rowTemp.CreateCell(1);
        //                      rowTemp1.SetCellValue(listModel[i].no.ToString());
        //                      rowTemp1.CellStyle = commonCellStyle;

        //                  var rowTemp2 = rowTemp.CreateCell(2);
        //                      rowTemp2.SetCellValue(listModel[i].names.ToString());
        //                      rowTemp2.CellStyle = commonCellStyle;

        //                  var rowTemp3 = rowTemp.CreateCell(3);
        //                      rowTemp3.SetCellValue(listModel[i].shortNames.ToString());
        //                      rowTemp3.CellStyle = commonCellStyle;

        //                  var rowTemp4 = rowTemp.CreateCell(4);
        //                      rowTemp4.SetCellValue(listModel[i].customCode.ToString());
        //                      rowTemp4.CellStyle = commonCellStyle;

        //                  var rowTemp5 = rowTemp.CreateCell(5);
        //                      rowTemp5.SetCellValue(listModel[i].address.ToString());
        //                      rowTemp5.CellStyle = commonCellStyle;

        //                  var rowTemp6 = rowTemp.CreateCell(6);
        //                      rowTemp6.SetCellValue(listModel[i].saveDay.ToString());
        //                      rowTemp6.CellStyle = commonCellStyle;

        //                  var rowTemp7 = rowTemp.CreateCell(7);
        //                      rowTemp7.SetCellValue(listModel[i].shoresRow.ToString());
        //                      rowTemp7.CellStyle = commonCellStyle;

        //                  var rowTemp8 = rowTemp.CreateCell(8);
        //                      rowTemp8.SetCellValue(listModel[i].shoresCell.ToString());
        //                      rowTemp8.CellStyle = commonCellStyle;

        //                  var rowTemp9 = rowTemp.CreateCell(9);
        //                      rowTemp9.SetCellValue(listModel[i].sampleType.ToString());
        //                      rowTemp9.CellStyle = commonCellStyle;

        //                  var rowTemp10 = rowTemp.CreateCell(10);
        //                      rowTemp10.SetCellValue(listModel[i].remark.ToString());
        //                      rowTemp10.CellStyle = commonCellStyle;

        //                  var rowTemp11 = rowTemp.CreateCell(11);
        //                      rowTemp11.SetCellValue(listModel[i].sore.ToString());
        //                      rowTemp11.CellStyle = commonCellStyle;

        //                  var rowTemp12 = rowTemp.CreateCell(12);
        //                      rowTemp12.SetCellValue(listModel[i].creater.ToString());
        //                      rowTemp12.CellStyle = commonCellStyle;

        //                  var rowTemp13 = rowTemp.CreateCell(13);
        //                      rowTemp13.SetCellValue(listModel[i].createTime.ToString());
        //                      rowTemp13.CellStyle = commonCellStyle;

        //                  var rowTemp14 = rowTemp.CreateCell(14);
        //                      rowTemp14.SetCellValue(listModel[i].state.ToString());
        //                      rowTemp14.CellStyle = commonCellStyle;

        //                  var rowTemp15 = rowTemp.CreateCell(15);
        //                      rowTemp15.SetCellValue(listModel[i].dstate.ToString());
        //                      rowTemp15.CellStyle = commonCellStyle;

        //          }
        //          // 导出excel
        //          string webRootPath = _webHostEnvironment.WebRootPath;
        //          string tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
        //          string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-sw_stores导出(选择结果).xls";
        //          string filePath = webRootPath + tpath;
        //          DirectoryInfo di = new DirectoryInfo(filePath);
        //          if (!di.Exists)
        //          {
        //              di.Create();
        //          }
        //          FileStream fileHssf = new FileStream(filePath + fileName, FileMode.Create);
        //          book.Write(fileHssf);
        //          fileHssf.Close();

        //          jm.code = 0;
        //          jm.msg = GlobalConstVars.ExcelExportSuccess;
        //          jm.data = tpath + fileName;

        //          return jm;
        //      }
        //      #endregion

        //      #region 查询导出============================================================
        //      // POST: Api/sw_stores/QueryExportExcel/10
        //      /// <summary>
        //      /// 查询导出
        //      /// </summary>
        //      /// <returns></returns>
        //      [HttpPost]
        //      [Description("查询导出")]
        //      public async Task<WebApiCallBack> QueryExportExcel()
        //      {
        //          var jm = new WebApiCallBack();

        //          var where = PredicateBuilder.True<sw_stores>();
        //              //查询筛选

        //	//id int
        //	var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
        //          if (id > 0)
        //          {
        //              where = where.And(p => p.id == id);
        //          }
        //	//编号 varchar
        //	var no = Request.Form["no"].FirstOrDefault();
        //          if (!string.IsNullOrEmpty(no))
        //          {
        //              where = where.And(p => p.no.Contains(no));
        //          }
        //	//名称 varchar
        //	var names = Request.Form["names"].FirstOrDefault();
        //          if (!string.IsNullOrEmpty(names))
        //          {
        //              where = where.And(p => p.names.Contains(names));
        //          }
        //	//拼音缩写 varchar
        //	var shortNames = Request.Form["shortNames"].FirstOrDefault();
        //          if (!string.IsNullOrEmpty(shortNames))
        //          {
        //              where = where.And(p => p.shortNames.Contains(shortNames));
        //          }
        //	//自定编码 varchar
        //	var customCode = Request.Form["customCode"].FirstOrDefault();
        //          if (!string.IsNullOrEmpty(customCode))
        //          {
        //              where = where.And(p => p.customCode.Contains(customCode));
        //          }
        //	//存储地址 varchar
        //	var address = Request.Form["address"].FirstOrDefault();
        //          if (!string.IsNullOrEmpty(address))
        //          {
        //              where = where.And(p => p.address.Contains(address));
        //          }
        //	//储存天数 int
        //	var saveDay = Request.Form["saveDay"].FirstOrDefault().ObjectToInt(0);
        //          if (saveDay > 0)
        //          {
        //              where = where.And(p => p.saveDay == saveDay);
        //          }
        //	//默认行数 int
        //	var shoresRow = Request.Form["shoresRow"].FirstOrDefault().ObjectToInt(0);
        //          if (shoresRow > 0)
        //          {
        //              where = where.And(p => p.shoresRow == shoresRow);
        //          }
        //	//默认列数 int
        //	var shoresCell = Request.Form["shoresCell"].FirstOrDefault().ObjectToInt(0);
        //          if (shoresCell > 0)
        //          {
        //              where = where.And(p => p.shoresCell == shoresCell);
        //          }
        //	//存储样本类型 varchar
        //	var sampleType = Request.Form["sampleType"].FirstOrDefault();
        //          if (!string.IsNullOrEmpty(sampleType))
        //          {
        //              where = where.And(p => p.sampleType.Contains(sampleType));
        //          }
        //	//备注信息 varchar
        //	var remark = Request.Form["remark"].FirstOrDefault();
        //          if (!string.IsNullOrEmpty(remark))
        //          {
        //              where = where.And(p => p.remark.Contains(remark));
        //          }
        //	//排序 int
        //	var sore = Request.Form["sore"].FirstOrDefault().ObjectToInt(0);
        //          if (sore > 0)
        //          {
        //              where = where.And(p => p.sore == sore);
        //          }
        //	//创建人 varchar
        //	var creater = Request.Form["creater"].FirstOrDefault();
        //          if (!string.IsNullOrEmpty(creater))
        //          {
        //              where = where.And(p => p.creater.Contains(creater));
        //          }
        //	////创建时间 datetime2
        //	//var createTime = Request.Form["createTime"].FirstOrDefault();
        // //         if (!string.IsNullOrEmpty(createTime))
        // //         {
        // //             where = where.And(p => p.createTime.Contains(createTime));
        // //         }
        //	//是否启用 bit
        //	var state = Request.Form["state"].FirstOrDefault();
        //          if (!string.IsNullOrEmpty(state) && state.ToLowerInvariant() == "true")
        //          {
        //              where = where.And(p => p.state == true);
        //          }
        //          else if (!string.IsNullOrEmpty(state) && state.ToLowerInvariant() == "false")
        //          {
        //              where = where.And(p => p.state == false);
        //          }
        //	//是否删除 bit
        //	var dstate = Request.Form["dstate"].FirstOrDefault();
        //          if (!string.IsNullOrEmpty(dstate) && dstate.ToLowerInvariant() == "true")
        //          {
        //              where = where.And(p => p.dstate == true);
        //          }
        //          else if (!string.IsNullOrEmpty(dstate) && dstate.ToLowerInvariant() == "false")
        //          {
        //              where = where.And(p => p.dstate == false);
        //          }
        //          //获取数据
        //          //创建Excel文件的对象
        //          var book = new HSSFWorkbook();
        //          //添加一个sheet
        //          var mySheet = book.CreateSheet("Sheet1");
        //          //获取list数据
        //          var listModel = await _sw_storesServices.QueryListByClauseAsync(where, p => p.id, OrderByType.Asc, true);
        //          //给sheet1添加第一行的头部标题
        //              var headerRow = mySheet.CreateRow(0);
        //          var headerStyle = ExcelHelper.GetHeaderStyle(book);

        //          var cell0 = headerRow.CreateCell(0);
        //          cell0.SetCellValue("id");
        //          cell0.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(0, 10 * 256);

        //          var cell1 = headerRow.CreateCell(1);
        //          cell1.SetCellValue("编号");
        //          cell1.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(1, 10 * 256);

        //          var cell2 = headerRow.CreateCell(2);
        //          cell2.SetCellValue("名称");
        //          cell2.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(2, 10 * 256);

        //          var cell3 = headerRow.CreateCell(3);
        //          cell3.SetCellValue("拼音缩写");
        //          cell3.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(3, 10 * 256);

        //          var cell4 = headerRow.CreateCell(4);
        //          cell4.SetCellValue("自定编码");
        //          cell4.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(4, 10 * 256);

        //          var cell5 = headerRow.CreateCell(5);
        //          cell5.SetCellValue("存储地址");
        //          cell5.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(5, 10 * 256);

        //          var cell6 = headerRow.CreateCell(6);
        //          cell6.SetCellValue("储存天数");
        //          cell6.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(6, 10 * 256);

        //          var cell7 = headerRow.CreateCell(7);
        //          cell7.SetCellValue("默认行数");
        //          cell7.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(7, 10 * 256);

        //          var cell8 = headerRow.CreateCell(8);
        //          cell8.SetCellValue("默认列数");
        //          cell8.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(8, 10 * 256);

        //          var cell9 = headerRow.CreateCell(9);
        //          cell9.SetCellValue("存储样本类型");
        //          cell9.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(9, 10 * 256);

        //          var cell10 = headerRow.CreateCell(10);
        //          cell10.SetCellValue("备注信息");
        //          cell10.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(10, 10 * 256);

        //          var cell11 = headerRow.CreateCell(11);
        //          cell11.SetCellValue("排序");
        //          cell11.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(11, 10 * 256);

        //          var cell12 = headerRow.CreateCell(12);
        //          cell12.SetCellValue("创建人");
        //          cell12.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(12, 10 * 256);

        //          var cell13 = headerRow.CreateCell(13);
        //          cell13.SetCellValue("创建时间");
        //          cell13.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(13, 10 * 256);

        //          var cell14 = headerRow.CreateCell(14);
        //          cell14.SetCellValue("是否启用");
        //          cell14.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(14, 10 * 256);

        //          var cell15 = headerRow.CreateCell(15);
        //          cell15.SetCellValue("是否删除");
        //          cell15.CellStyle = headerStyle;
        //          mySheet.SetColumnWidth(15, 10 * 256);


        //          headerRow.Height = 30 * 20;
        //          var commonCellStyle = ExcelHelper.GetCommonStyle(book);

        //          //将数据逐步写入sheet1各个行
        //          for (var i = 0; i < listModel.Count; i++)
        //          {
        //              var rowTemp = mySheet.CreateRow(i + 1);


        //          var rowTemp0 = rowTemp.CreateCell(0);
        //          rowTemp0.SetCellValue(listModel[i].id.ToString());
        //          rowTemp0.CellStyle = commonCellStyle;



        //          var rowTemp1 = rowTemp.CreateCell(1);
        //          rowTemp1.SetCellValue(listModel[i].no.ToString());
        //          rowTemp1.CellStyle = commonCellStyle;



        //          var rowTemp2 = rowTemp.CreateCell(2);
        //          rowTemp2.SetCellValue(listModel[i].names.ToString());
        //          rowTemp2.CellStyle = commonCellStyle;



        //          var rowTemp3 = rowTemp.CreateCell(3);
        //          rowTemp3.SetCellValue(listModel[i].shortNames.ToString());
        //          rowTemp3.CellStyle = commonCellStyle;



        //          var rowTemp4 = rowTemp.CreateCell(4);
        //          rowTemp4.SetCellValue(listModel[i].customCode.ToString());
        //          rowTemp4.CellStyle = commonCellStyle;



        //          var rowTemp5 = rowTemp.CreateCell(5);
        //          rowTemp5.SetCellValue(listModel[i].address.ToString());
        //          rowTemp5.CellStyle = commonCellStyle;



        //          var rowTemp6 = rowTemp.CreateCell(6);
        //          rowTemp6.SetCellValue(listModel[i].saveDay.ToString());
        //          rowTemp6.CellStyle = commonCellStyle;



        //          var rowTemp7 = rowTemp.CreateCell(7);
        //          rowTemp7.SetCellValue(listModel[i].shoresRow.ToString());
        //          rowTemp7.CellStyle = commonCellStyle;



        //          var rowTemp8 = rowTemp.CreateCell(8);
        //          rowTemp8.SetCellValue(listModel[i].shoresCell.ToString());
        //          rowTemp8.CellStyle = commonCellStyle;



        //          var rowTemp9 = rowTemp.CreateCell(9);
        //          rowTemp9.SetCellValue(listModel[i].sampleType.ToString());
        //          rowTemp9.CellStyle = commonCellStyle;



        //          var rowTemp10 = rowTemp.CreateCell(10);
        //          rowTemp10.SetCellValue(listModel[i].remark.ToString());
        //          rowTemp10.CellStyle = commonCellStyle;



        //          var rowTemp11 = rowTemp.CreateCell(11);
        //          rowTemp11.SetCellValue(listModel[i].sore.ToString());
        //          rowTemp11.CellStyle = commonCellStyle;



        //          var rowTemp12 = rowTemp.CreateCell(12);
        //          rowTemp12.SetCellValue(listModel[i].creater.ToString());
        //          rowTemp12.CellStyle = commonCellStyle;



        //          var rowTemp13 = rowTemp.CreateCell(13);
        //          rowTemp13.SetCellValue(listModel[i].createTime.ToString());
        //          rowTemp13.CellStyle = commonCellStyle;



        //          var rowTemp14 = rowTemp.CreateCell(14);
        //          rowTemp14.SetCellValue(listModel[i].state.ToString());
        //          rowTemp14.CellStyle = commonCellStyle;



        //          var rowTemp15 = rowTemp.CreateCell(15);
        //          rowTemp15.SetCellValue(listModel[i].dstate.ToString());
        //          rowTemp15.CellStyle = commonCellStyle;


        //          }
        //          // 写入到excel
        //          string webRootPath = _webHostEnvironment.WebRootPath;
        //          string tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
        //          string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-sw_stores导出(查询结果).xls";
        //          string filePath = webRootPath + tpath;
        //          DirectoryInfo di = new DirectoryInfo(filePath);
        //          if (!di.Exists)
        //          {
        //              di.Create();
        //          }
        //          FileStream fileHssf = new FileStream(filePath + fileName, FileMode.Create);
        //          book.Write(fileHssf);
        //          fileHssf.Close();

        //          jm.code = 0;
        //          jm.msg = GlobalConstVars.ExcelExportSuccess;
        //          jm.data = tpath + fileName;

        //          return jm;
        //      }
        //      #endregion


        //      #region 设置是否启用============================================================
        //      // POST: Api/sw_stores/DoSetstate/10
        //      /// <summary>
        //      /// 设置是否启用
        //      /// </summary>
        //      /// <param name="entity"></param>
        //      /// <returns></returns>
        //      [HttpPost]
        //      [Description("设置是否启用")]
        //      public async Task<WebApiCallBack> DoSetstate([FromBody]FMUpdateBoolDataByIntId entity)
        //      {
        //          var jm = new WebApiCallBack();

        //          var oldModel = await _sw_storesServices.QueryByIdAsync(entity.id, false);
        //          if (oldModel == null)
        //          {
        //              jm.msg = "不存在此信息";
        //              return jm;
        //          }
        //          oldModel.state = (bool)entity.data;

        //          var bl = await _sw_storesServices.UpdateAsync(p => new sw_stores() { state = oldModel.state }, p => p.id == oldModel.id);
        //          jm.code = bl ? 0 : 1;
        //          jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

        //          return jm;
        //}
        //      #endregion

        //      #region 设置是否删除============================================================
        //      // POST: Api/sw_stores/DoSetdstate/10
        //      /// <summary>
        //      /// 设置是否删除
        //      /// </summary>
        //      /// <param name="entity"></param>
        //      /// <returns></returns>
        //      [HttpPost]
        //      [Description("设置是否删除")]
        //      public async Task<WebApiCallBack> DoSetdstate([FromBody]FMUpdateBoolDataByIntId entity)
        //      {
        //          var jm = new WebApiCallBack();

        //          var oldModel = await _sw_storesServices.QueryByIdAsync(entity.id, false);
        //          if (oldModel == null)
        //          {
        //              jm.msg = "不存在此信息";
        //              return jm;
        //          }
        //          oldModel.dstate = (bool)entity.data;

        //          var bl = await _sw_storesServices.UpdateAsync(p => new sw_stores() { dstate = oldModel.dstate }, p => p.id == oldModel.id);
        //          jm.code = bl ? 0 : 1;
        //          jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

        //          return jm;
        //}
        //      #endregion


    }
}
