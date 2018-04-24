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
    /// teamEdit 的摘要说明
    /// </summary>
    public class teamEdit : IHttpHandler, IRequiresSessionState
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
                        DataTable dt = SqlHelper.ExecuteDataTable("select * from class where CLASS_ID=@CLASS_ID", new SqlParameter("@CLASS_ID", id));
                        DataTable dts = SqlHelper.ExecuteDataTable("select * from teacher");
                        var Data = new { team = dt.Rows[0], teachers = dts.Rows };
                        string html = CommonHelper.RenderHtml("editteam.html", Data);
                        context.Response.Write(html);
                        //DataTable dt = SqlHelper.ExecuteDataTable("select * from class_teacher where CLASS_ID=@CLASS_ID"
                        //    , new SqlParameter("@CLASS_ID", id));
                        //var Data = new { teachers = dt.Rows };
                        //string html = CommonHelper.RenderHtml("addteam.html", Data);
                        //context.Response.Write(html);
                    }
                }
                else if (!string.IsNullOrEmpty(context.Request["UpdateID"]))
                {
                    string name = context.Request["name"];
                    string classroom = context.Request["classroom"];
                    string teacher_id = context.Request["teacher"];
                    try
                    {
                        SqlHelper.ExecuteNonQuery("update  class set NAME=@NAME,CLASSROME=@CLASSROME,TEACHER_ID=@TEACHER_ID where CLASS_ID=@CLASS_ID",
                            new SqlParameter("@NAME", name)
                            , new SqlParameter("@CLASSROME", classroom)
                            , new SqlParameter("@TEACHER_ID", teacher_id)
                            , new SqlParameter("@CLASS_ID", context.Request["UpdateID"]));
                    }
                    catch (Exception e)
                    {
                        context.Response.Write("访问数据库错误！");
                    }
                    context.Response.Redirect("teamMain.ashx");
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