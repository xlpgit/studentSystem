using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.SessionState;

namespace studentSystem
{
    /// <summary>
    /// teacherMain 的摘要说明
    /// </summary>
    public class teacherMain : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
           
        //    if (string.IsNullOrEmpty(context.Request["DelID"]))
        //    {
        //        ShowList(context);
        //    }
        //    else  
        //    {
        //        string id = context.Request["DelID"];
        //        SqlHelper.ExecuteNonQuery("delete from teacher where TEACHER_ID=@TEACHER_ID", new SqlParameter("@TEACHER_ID", id));
        //        ShowList(context);
        //    }
            if (context.Session["Admin"] != null)
            {
                string json = context.Request["json"];
                if (string.IsNullOrEmpty(json))
                {
                    ShowList(context);
                }
                else
                {
                    SqlHelper.ExecuteNonQuery("delete from teacher where TEACHER_ID=@TEACHER_ID", new SqlParameter("@TEACHER_ID", json));
                    ShowList(context);
                    context.Response.Write("ok");
                }
            }
            else {
                string html = CommonHelper.RenderHtml("login.html",null);
                context.Response.Write(html);
            }

           


        }


        private void ShowList(HttpContext context)
        {
            DataTable dt = new DataTable();


            int PageNumber = 1;  //表示当前时第几页
            int pagec = int.Parse(ConfigurationManager.AppSettings["pagecount"].ToString());//获取web.config中每页有几条记录
            string paGet = context.Request["PageNumber"];//通过参数形式，从前台传过来的PageNumber（当前第几页）
            if (paGet != null) 
            {
                //前后端用正则表达式的形式有点不一样，道理是一样的。
                Regex r = new Regex(@"^\d*$");   //正则表达式验证它输入的是正整数
                if (r.IsMatch(paGet))
                    PageNumber = int.Parse(paGet);
            }
            dt = SqlHelper.ExecuteDataTable("select * from (select *,ROW_NUMBER() over( order by TEACHER_ID asc) as num from teacher p) s where s.num>@Start and s.num<@End",
                new SqlParameter("@Start", (PageNumber - 1) * 9),
                new SqlParameter("@End", PageNumber * 10));
            int totalCount = (int)SqlHelper.ExecuteScalar("select count(*) from teacher");//一共有多少条数据
            //int pageCount = totalCount / pagec ;  //一共有多少页   这样直接写有问题
            int pageRem=totalCount%pagec;
            int pageCount;
            //int pageCount = (int)Math.Ceiling(totalCount / 10.0);
            if (pageRem == 0)
            {
                pageCount = totalCount / pagec;
            }
            else {
                pageCount = totalCount / pagec+1;
            }
            object[] pageData = new object[pageCount];   //每一页的数据
            for (int i = 0; i < pageCount; i++)
            {
                pageData[i] = new { Href = "teacherMain.ashx?PageNumber=" + (i + 1), Title = i + 1 };
            }
            var Data = new { teachers = dt.Rows, PageData = pageData, TotalCount = totalCount, PageNumber = PageNumber, PageCount = pageCount };

            //try
            //{
            //    dt = SqlHelper.ExecuteDataTable("select * from teacher");
            //}
            //catch (Exception e)
            //{

            //}
            //var Data = new { teachers = dt.Rows };
            string html = CommonHelper.RenderHtml("teacher.html", Data);
            context.Response.Write(html);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}