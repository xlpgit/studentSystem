using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.SessionState;

namespace studentSystem
{
    /// <summary>
    /// addstudent 的摘要说明
    /// </summary>
    public class addstudent : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
             if (context.Session["Admin"] != null)
            {
                    string name = context.Request["name"];
                    string phone = context.Request["phone"];
                    string email = context.Request["email"];
                    string birthday = context.Request["birthday"];
                    if (!string.IsNullOrEmpty(name))
                    {
                        try
                        {
                            SqlHelper.ExecuteNonQuery("insert into teacher(NAME,PHONE,EMAIL,BIRTHDAY) values(@NAME,@PHONE,@EMAIL,@BIRTHDAY)"
                                , new SqlParameter("@NAME", name)
                                , new SqlParameter("@PHONE", phone)
                                , new SqlParameter("@EMAIL", email)
                                , new SqlParameter("@BIRTHDAY", birthday));
                        }
                        catch (Exception e)
                        {
                            context.Response.Write("访问数据库错误!");
                        }
                        context.Response.Redirect("teacherMain.ashx");

                    }
                    else
                    {
                        string html = CommonHelper.RenderHtml("addteacher.html", "");
                        context.Response.Write(html);
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