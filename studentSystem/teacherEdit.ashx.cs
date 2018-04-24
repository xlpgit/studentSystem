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
    /// teacherEditashx 的摘要说明
    /// </summary>
    public class teacherEditashx : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            if (context.Session["Admin"] != null)
            {
                if (!string.IsNullOrEmpty(context.Request["EditID"]))
                {
                    string id = context.Request["EditID"];
                    if (!string.IsNullOrEmpty(id))
                    {
                        DataTable dt = SqlHelper.ExecuteDataTable("select * from teacher where TEACHER_ID=@TEACHER_ID", new SqlParameter("@TEACHER_ID", id));
                        var Data = new { teacher = dt.Rows[0] };
                        string html = CommonHelper.RenderHtml("editteacher.html", Data);
                        context.Response.Write(html);
                    }
                }
                else if (!string.IsNullOrEmpty(context.Request["UpdateID"]))
                {
                    string name = context.Request["NAME"];
                    string phone = context.Request["phone"];
                    string email = context.Request["email"];
                    string birthday = context.Request["birthday"];
                    try
                    {
                        SqlHelper.ExecuteNonQuery("update  teacher set NAME=@NAME,PHONE=@PHONE,EMAIL=@EMAIL,BIRTHDAY=@BIRTHDAY where TEACHER_ID=@TEACHER_ID",
                            new SqlParameter("@NAME", name)
                            , new SqlParameter("@PHONE", phone)
                            , new SqlParameter("@EMAIL", email)
                            , new SqlParameter("@BIRTHDAY", birthday)
                            , new SqlParameter("@TEACHER_ID", context.Request["UpdateID"]));
                    }
                    catch (Exception e)
                    {
                        context.Response.Write("访问数据库错误！");
                    }
                    context.Response.Redirect("teacherMain.ashx");
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