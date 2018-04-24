using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.Data;
using System.Web.SessionState;

namespace studentSystem
{
    /// <summary>
    /// teacherAddJson 的摘要说明
    /// </summary>
    public class teacherAddJson : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string IsPostBack = context.Request["IsPostBack"];
            if (context.Session["Admin"] != null)
            {
                if (IsPostBack == null) //第一次访问
                {
                    //DataTable dt = SqlHelper.ExecuteDataTable("select * from Comment");
                    //DataTable dtselect = SqlHelper.ExecuteDataTable("select Name from Comment");
                    //var Data = new { comments = dt.Rows, options = dtselect.Rows };
                    var Data = new { IsPostBack = "1" };
                    string html = CommonHelper.RenderHtml("addteacher.html", Data);
                    context.Response.Write(html);
                }
                else
                {
                    string json = context.Request["json"];   //获取前台传过来的json值,json以参数的形式传过来的
                    JavaScriptSerializer jsss = new JavaScriptSerializer();  //构建反序列化实体
                    teacher tea = jsss.Deserialize<teacher>(json);
                    SqlHelper.ExecuteNonQuery("insert into teacher(NAME,PHONE,EMAIL,BIRTHDAY) values(@NAME,@PHONE,@EMAIL,@BIRTHDAY)"
                                    , new SqlParameter("@NAME", tea.name)
                                    , new SqlParameter("@PHONE", tea.phone)
                                    , new SqlParameter("@EMAIL", tea.email)
                                    , new SqlParameter("@BIRTHDAY", tea.birthday));   //不能为空，否则会出问题
                    //string jso = "{\"name\":\"" + tea.name + "\",\"phone\":\"" + tea.phone + "\",\"email\":\"" + tea.email + "\",\"birthday\":\"" + tea.birthday + "\"}";             
                    //context.Response.Write(jso);        //返回给客户端浏览器,现在这也不用管
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