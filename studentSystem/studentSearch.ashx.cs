using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.SessionState;

namespace studentSystem
{
    /// <summary>
    /// studentSearch 的摘要说明
    /// </summary>
    public class studentSearch : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            //string searchText = context.Request["searchText"];//只对姓名进行查询

            //if (!string.IsNullOrEmpty(searchText))
            //{
            //    DataTable dt = SqlHelper.ExecuteDataTable("select * from student where NAME  like @NAME", new SqlParameter("@NAME", searchText));
            //    var Data = new { students = dt.Rows };
            //    string html = CommonHelper.RenderHtml("student.html", Data);
            //    context.Response.Write(html);
            //}
            if (context.Session["Admin"] != null)
            {
                string search = context.Request["search"];
                string searchText = context.Request["searchText"];
                if (!string.IsNullOrEmpty(searchText))
                {
                    if (search == "姓名")
                    {
                        DataTable dt = SqlHelper.ExecuteDataTable("select * from student_class where S_NAME  like @S_NAME", new SqlParameter("@S_NAME", "%" + searchText + "%"));
                        var Data = new { students = dt.Rows };
                        string html = CommonHelper.RenderHtml("student.html", Data);
                        context.Response.Write(html);
                    }
                    else if (search == "性别")
                    {
                        DataTable dt = SqlHelper.ExecuteDataTable("select * from student_class where SEX  like @SEX", new SqlParameter("@SEX", "%" + searchText + "%"));
                        var Data = new { students = dt.Rows };
                        string html = CommonHelper.RenderHtml("student.html", Data);
                        context.Response.Write(html);
                    }
                }
                else
                {
                    DataTable dt = new DataTable();
                    try
                    {
                        dt = SqlHelper.ExecuteDataTable("select * from student_class");
                    }
                    catch (Exception e)
                    {

                    }
                    var Data = new { students = dt.Rows };
                    string html = CommonHelper.RenderHtml("student.html", Data);
                    context.Response.Write(html);
                }
            }
            else {
                string html = CommonHelper.RenderHtml("login.html", null);
                context.Response.Write(html);
            }
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