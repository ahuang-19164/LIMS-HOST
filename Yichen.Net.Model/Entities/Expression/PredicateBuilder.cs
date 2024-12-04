/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System;
using System.Linq;
using System.Linq.Expressions;

namespace Yichen.Net.Model.Entities.Expression
{
    public static class PredicateBuilder
    {
        /// <summary>
        /// 机关函数应用True时：单个AND有效，多个AND有效；单个OR无效，多个OR无效；混应时写在AND后的OR有效  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>()
        {
            return f => true;
        }
        /// <summary>
        /// 机关函数应用False时：单个AND无效，多个AND无效；单个OR有效，多个OR有效；混应时写在OR后面的AND有效  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>()
        {
            return f => false;
        }

        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second,
            Func<System.Linq.Expressions.Expression, System.Linq.Expressions.Expression,
                System.Linq.Expressions.Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)  
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first  
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression   
            return System.Linq.Expressions.Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        /// <summary>
        ///     扩展啦
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            return first.Compose(second, System.Linq.Expressions.Expression.AndAlso);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            return first.Compose(second, System.Linq.Expressions.Expression.OrElse);
        }

        #region 使用案例

        ///// <summary>
        ///// 多条件查询
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    using (LinqDBDataContext db = new LinqDBDataContext())
        //    {
        //        var list = db.StuInfo;
        //        var where = PredicateBuilder.True<StuInfo>();
        //        if (this.txtName.Text.Trim().Length != 0)
        //        {
        //            where = where.And(p => p.StuName.Contains(this.txtName.Text.Trim()));
        //        }
        //        if (this.txtAge.Text.Trim().Length != 0)
        //        {
        //            where = where.And(p => p.StuAge == Convert.ToInt32(this.txtAge.Text.Trim()));
        //        }
        //        var result = list.Where(where).ToList();
        //        this.repStuInfo.DataSource = result;
        //        this.repStuInfo.DataBind();
        //    }
        //}

        #endregion
    }
}