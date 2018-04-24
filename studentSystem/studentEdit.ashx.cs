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
    /// studentEdit 的摘要说明
    /// </summary>
    public class studentEdit : IHttpHandler, IRequiresSessionState
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
                        DataTable dt = SqlHelper.ExecuteDataTable("select * from student where STUDENT_ID=@STUDENT_ID", new SqlParameter("@STUDENT_ID", id));
                        DataTable dts = SqlHelper.ExecuteDataTable("select * from class");
                        var Data = new { student = dt.Rows[0], teams = dts.Rows };
                        string html = CommonHelper.RenderHtml("editstudent.html", Data);
                        context.Response.Write(html);
                    }
                }
                else if (!string.IsNullOrEmpty(context.Request["UpdateID"]))
                {
                    string name = context.Request["name"];
                    string sex = context.Request["sex"];
                    string birth = context.Request["birth"];
                    string height = context.Request["height"];
                    string special = context.Request["special"];
                    string team_id = context.Request["team"];
                    try
                    {
                        SqlHelper.ExecuteNonQuery("update  student set NAME=@NAME,SEX=@SEX,BIRTHDAY=@BIRTHDAY,HEIGHT=@HEIGHT,CLASS_ID=@CLASS_ID,IS_SPECIAL=@IS_SPECIAL where STUDENT_ID=@STUDENT_ID",
                            new SqlParameter("@NAME", name)
                            , new SqlParameter("@SEX", sex)
                            , new SqlParameter("@BIRTHDAY", birth)
                            , new SqlParameter("@HEIGHT", height)
                            , new SqlParameter("@CLASS_ID", team_id)
                            , new SqlParameter("@IS_SPECIAL", special)
                            , new SqlParameter("@STUDENT_ID", context.Request["UpdateID"]));
                    }
                    catch (Exception e)
                    {
                        context.Response.Write("访问数据库错误！");
                    }
                    context.Response.Redirect("studentMain.ashx");
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