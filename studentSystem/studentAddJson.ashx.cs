using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.Web.SessionState;

namespace studentSystem
{
    /// <summary>
    /// studentAddJson 的摘要说明
    /// </summary>
    public class studentAddJson : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string IsPostBack = context.Request["IsPostBack"];
            if (context.Session["Admin"] != null)
            {
                if (string.IsNullOrEmpty("IsPostBack")) //第一次访问
                {
                    var Data = new { IsPostBack = "1" };
                    string html = CommonHelper.RenderHtml("addstudent.html", Data);
                    context.Response.Write(html);
                }
                else
                {
                    string json = context.Request["json"];   //获取前台传过来的json值,json以参数的形式传过来的
                    JavaScriptSerializer jsss = new JavaScriptSerializer();  //构建反序列化实体
                    student stu = jsss.Deserialize<student>(json);
                    SqlHelper.ExecuteNonQuery("insert into student (NAME,SEX,BIRTHDAY,HEIGHT,IS_SPECIAL,CLASS_ID) values(@NAME,@SEX,@BIRTHDAY,@HEIGHT,@IS_SPECIAL,@CLASS_ID)",
                           new SqlParameter("@NAME", stu.name)
                           , new SqlParameter("@SEX", stu.sex)
                           , new SqlParameter("@BIRTHDAY", stu.birthday)
                           , new SqlParameter("@HEIGHT", stu.height)
                           , new SqlParameter("@IS_SPECIAL", stu.isSpecial)
                           , new SqlParameter("@CLASS_ID", stu.classId)
                          );
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