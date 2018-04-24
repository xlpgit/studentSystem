using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Web.SessionState;

namespace studentSystem
{
    /// <summary>
    /// teamAddJson 的摘要说明
    /// </summary>
    public class teamAddJson : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string IsPostBack = context.Request["IsPostBack"];//判别字段为什么还得用json传过来，否则获取不到值
            if (context.Session["Admin"] != null)
            {
                if (string.IsNullOrEmpty(IsPostBack)) //为空时，第一次访问
                {
                    var Data = new { IsPostBack = "1" };
                    string html = CommonHelper.RenderHtml("addteam.html", Data);
                    context.Response.Write(html);
                }
                else
                {
                    //获取前台传过来的json串，并反序列化json对象
                    string json = context.Request["json"];
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    team te = js.Deserialize<team>(json);
                    SqlHelper.ExecuteNonQuery("insert into class(NAME,CLASSROME,TEACHER_ID) values(@NAME,@CLASSROME,@TEACHER_ID)"
                           , new SqlParameter("@NAME", te.name)
                           , new SqlParameter("@CLASSROME", te.classRoom)
                           , new SqlParameter("@TEACHER_ID", te.teacherId));       //现在必须得选择班级
                    string jso = "{\"name\":\"" + te.name + "\",\"classroom\":\"" + te.classRoom + "\",\"teacherId\":\"" + te.teacherId + "\"}";//返回给前台的数据，如何不做处理的话可以随意
                    context.Response.Write(jso);
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