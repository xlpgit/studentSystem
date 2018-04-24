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
    /// studentAdd 的摘要说明
    /// </summary>
    public class studentAdd : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType ="text/html";
            string name=context.Request["name"];
            string sex=context.Request["sex"];
            string birth=context.Request["birth"];
            string height = context.Request["height"];
            string spe=context.Request["special"];//得到的是option中的value的值
            string class_id = context.Request["class_name"];
            if (context.Session["Admin"] != null)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    try
                    {
                        SqlHelper.ExecuteNonQuery("insert into student (NAME,SEX,BIRTHDAY,HEIGHT,IS_SPECIAL,CLASS_ID) values(@NAME,@SEX,@BIRTHDAY,@HEIGHT,@IS_SPECIAL,@CLASS_ID)",
                           new SqlParameter("@NAME", name)
                           , new SqlParameter("@SEX", sex)
                           , new SqlParameter("@BIRTHDAY", birth)
                           , new SqlParameter("@HEIGHT", height)
                           , new SqlParameter("@IS_SPECIAL", spe)
                           , new SqlParameter("@CLASS_ID", class_id)
                          );
                    }
                    catch (Exception e)
                    {
                        context.Response.Write("访问数据库错误！");
                    }
                    context.Response.Redirect("studentMain.ashx");
                }
                else
                {
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from class");
                    var Data = new { teams = dt.Rows };
                    string html = CommonHelper.RenderHtml("addstudent.html", Data);
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