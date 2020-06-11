using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;
using static gswsBackendAPI.Depts.RationVolunteermapping.Backend.ResponseModel;
using static gswsBackendAPI.Depts.RationVolunteermapping.Backend.RationVolunteerSPHelper;
namespace gswsBackendAPI.Depts.RationVolunteermapping.Backend
{
    public class RationVolunteerHelper
    {
        RationVolunteerSPHelper _RSP = new RationVolunteerSPHelper();

        public List<string> rationIds = new List<string>();
        public List<rationMembersModel> listrationMembersModel = new List<rationMembersModel>();
        public dynamic loadRationMembers(RationInputs obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.ptype = "1";
                DataTable dt = _RSP.RationVolunteerMappingProc(obj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string rationId = dr["EXISTING_RC_NUMBER"].ToString();
                        if (rationIdChecker(rationId))
                            rationIds.Add(rationId);
                    }


                    for (int i = 0; i < rationIds.Count; i++)
                    {
                        rationMembersModel objrationMembersModel = new rationMembersModel();
                        List<memberData> listMemberData = new List<memberData>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            string rationId = dr["EXISTING_RC_NUMBER"].ToString();
                            if (rationIds[i] == rationId)
                            {
                                memberData objMemberData = new memberData();
                                objMemberData.GT_GENDER = dr["GT_GENDER"].ToString();
                                objMemberData.MEMBER_NAME_EN = dr["MEMBER_NAME_EN"].ToString();
                                objMemberData.RS_NAME_EN = dr["RS_NAME_EN"].ToString();
                                objMemberData.SEC_ID = dr["SEC_ID"].ToString();
                                objMemberData.MOBILE_NUMBER = dr["MOBILENO"].ToString();
                                listMemberData.Add(objMemberData);
                            }
                        }

                        objrationMembersModel.EXISTING_RC_NUMBER = rationIds[i];
                        objrationMembersModel.memberDatas = listMemberData;
                        listrationMembersModel.Add(objrationMembersModel);
                    }

                    objdata.status = 200;
                    objdata.result = listrationMembersModel;
                }
                else
                {
                    objdata.status = 400;
                    objdata.result = "Rice cards not Available for this secretariat or all rice cards in this secretariat are mapped !!!";
                }
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        public bool rationIdChecker(string rationId)
        {
            for (int i = 0; i < rationIds.Count; i++)
            {
                if (rationIds[i] == rationId)
                    return false;
            }
            return true;

        }


        public dynamic loadClusters(RationInputs obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {

                obj.ptype = "2";
                DataTable dt = _RSP.RationVolunteerMappingProc(obj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    objdata.status = 200;
                    objdata.result = dt;
                }
                else
                {
                    objdata.status = 400;
                    objdata.result = "No Data Available";
                }
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        public dynamic assignRiceCardToCluster(RationInputs obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.ptype = "10";
                int count = _RSP.RationVolunteerMappingProc1(obj);
                if (count > 0)
                {
                    objdata.status = 200;
                    objdata.result = "Records Updated Successfully !!!";
                }
                else
                {
                    objdata.status = 400;
                    objdata.result = "Failed to update records, Please try again !!!";
                }
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        public dynamic reqRiceCardToCluster(RationInputs obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.ptype = "12";
                obj.pration_id = obj.user_data[0].EXISTING_RC_NUMBER;

                DataTable dt = _RSP.RationVolunteerMappingProc(obj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    objdata.status = 200;
                    objdata.result = "Rice card adding request submitted Successfully !!!,Rice card added to cluster after approved by the JOINT COLLECTOR";
                }
                else
                {
                    objdata.status = 400;
                    objdata.result = "Failed to submit request, Please try again !!!";
                }
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }



        public dynamic assignRationToCluster(RationInputs obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.ptype = "3";
                if (obj.user_data[0].DISTRICT_STATUS != "1")
                {
                    int count = _RSP.RationVolunteerMappingProc1(obj);
                    if (count > 0)
                    {
                        objdata.status = 200;
                        objdata.result = "Records Updated Successfully !!!";
                    }
                    else
                    {
                        objdata.status = 400;
                        objdata.result = "Failed to update records, Please try again !!!";
                    }
                }
                else
                {
                    objdata.status = 400;
                    objdata.result = "Failed to update records, Please try again !!!";
                }

            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        public dynamic unassignRationToCluster(RationInputs obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {

                obj.ptype = "8";
                int count = _RSP.RationVolunteerMappingProc1(obj);
                if (count > 0)
                {
                    objdata.status = 200;
                    objdata.result = "Records Updated Successfully !!!";
                }
                else
                {
                    objdata.status = 400;
                    objdata.result = "Failed to update records, Please try again !!!";
                }
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        public dynamic secList(RationInputs obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {

                obj.ptype = "7";
                DataTable dt = _RSP.RationVolunteerMappingProc(obj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    objdata.status = 200;
                    objdata.result = dt;
                }
                else
                {
                    objdata.status = 400;
                    objdata.result = "No Data Available";
                }
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }

        public dynamic SearchRiceCard(RationInputs obj)
        {
            dynamic objdata = new ExpandoObject();
            try
            {
                obj.ptype = "15";
                obj.pration_id = obj.user_data[0].EXISTING_RC_NUMBER;
                DataTable dt = _RSP.RationVolunteerMappingProc(obj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["STATUS"].ToString() == "0")
                    {
                        objdata.status = 200;
                        objdata.result = dt;
                    }
                    else
                    {
                        objdata.status = 400;
                        objdata.result = "Rice card already assigned to " + dt.Rows[0]["DISTRICT_NAME"].ToString() + " District, " + dt.Rows[0]["MANDAL_NAME"].ToString() + " Mandal, " + dt.Rows[0]["SECRETARIAT_NAME"].ToString() + " Sachivalayam, " + dt.Rows[0]["CLUSTER_NAME"].ToString() + " cluster";
                    }
                }
                else
                {
                    objdata.status = 400;
                    objdata.result = "No Data Available";
                }
            }
            catch (Exception ex)
            {
                objdata.status = 500;
                objdata.result = ex.Message.ToString();
            }
            return objdata;
        }


    }
}