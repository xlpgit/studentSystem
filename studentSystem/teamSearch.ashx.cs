using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.SessionState;

namespace studentSystem
{
    /// <summary>
    /// teamSearch 的摘要说明
    /// </summary>
    public class teamSearch : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string search = context.Request["search"];
            string searchText = context.Request["searchText"];
            if (context.Session["Admin"] != null)
            {
                if (!string.IsNullOrEmpty(searchText))
                {
                    if (search == "班级名称")
                    {
                        DataTable dt = SqlHelper.ExecuteDataTable("select * from class_teacher where C_NAME like @C_NAME", new SqlParameter("@C_NAME", "%" + searchText + "%"));
                        var Data = new { teams = dt.Rows };
                        string html = CommonHelper.RenderHtml("team.html", Data);
                        context.Response.Write(html);
                    }
                    else if (search == "教室号")
                    {
                        DataTable dt = SqlHelper.ExecuteDataTable("select * from class_teacher where CLASSROME  like @CLASSROME", new SqlParameter("@CLASSROME", "%" + searchText + "%"));
                        var Data = new { teams = dt.Rows };
                        string html = CommonHelper.RenderHtml("team.html", Data);
                        context.Response.Write(html);
                    }
                }

                else
                {
                    DataTable dt = new DataTable();
                    try
                    {
                        dt = SqlHelper.ExecuteDataTable("select * from class");
                    }
                    catch (Exception e)
                    {

                    }
                    var Data = new { teachers = dt.Rows };
                    string html = CommonHelper.RenderHtml("team.html", Data);
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