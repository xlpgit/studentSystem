using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.SessionState;

namespace studentSystem
{
    /// <summary>
    /// teamAdd 的摘要说明
    /// </summary>
    public class teamAdd : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string name = context.Request["name"];
            string classroom = context.Request["classroom"];
            string teacher_id = context.Request["teacher"];
            if (context.Session["Admin"] != null)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    try
                    {
                        SqlHelper.ExecuteNonQuery("insert into class(NAME,CLASSROME,TEACHER_ID) values(@NAME,@CLASSROME,@TEACHER_ID)"
                            , new SqlParameter("@NAME", name)
                            , new SqlParameter("@CLASSROME", classroom)
                            , new SqlParameter("@TEACHER_ID", teacher_id));
                    }
                    catch (Exception e)
                    {
                        context.Response.Write("访问数据库错误!");
                    }
                    context.Response.Redirect("teamMain.ashx");

                }
                else
                {
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from teacher");
                    var Data = new { teachers = dt.Rows };
                    string html = CommonHelper.RenderHtml("addteam.html", Data);
                    context.Response.Write(html);

                    //string html = CommonHelper.RenderHtml("addteam.html", "");
                    //context.Response.Write(html);
                }
            }
            else
            {
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