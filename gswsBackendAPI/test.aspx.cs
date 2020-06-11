using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gswsBackendAPI
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
			string id=DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000, 9999).ToString();
			Response.Write(id);
		}
    }
}