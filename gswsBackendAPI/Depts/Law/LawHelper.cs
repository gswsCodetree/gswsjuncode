using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.Depts.Law
{
    public class LawHelper : LawSPHelper
    {
        dynamic obj = new ExpandoObject();

        #region Law
        public dynamic GetMethod(string url)
        {
            
            try
            {
                var val = GetData(url);

                obj.Status = 100;
                obj.Reason = "Data Loaded Successfully.";
                obj.Details = val;

                return obj;
            }
            catch (Exception ex)
            {
               
                obj.Status = 102;
                obj.Reason = ThirdpartyMessage;
                return obj;
            }

        }

        #endregion
    }
}