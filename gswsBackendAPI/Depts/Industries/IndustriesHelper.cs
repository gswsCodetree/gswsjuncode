using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.Depts.Industries
{
    public class IndustriesHelper:IndustriesSPHelper
    {
        #region YSR Navodayam
        public dynamic GetMethod(string url)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                var val = GetData(url);
                obj.Status = 100;
                obj.Reason = "Data Loaded Successfully";
                obj.Data = val;

                return obj;
            }
            catch (Exception ex)
            {
                obj.Status = 102;
                obj.Reason = ThirdpartyMessage;
                return obj;
            }

        }

        public dynamic SaveOTRReg_helper(NavodayamOTR root)
        {
            dynamic obj = new ExpandoObject();
            try
            {
                var val = PostData("http://125.17.121.166:8080/SDPIntegrationServices/YSRNavodayamService/SaveOTRDetails", root);
                var data = GetSerialzedData<dynamic>(val);

                obj.Status = 100;
                obj.Reason = "Data Loaded Successfully";
                obj.Data = data;

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