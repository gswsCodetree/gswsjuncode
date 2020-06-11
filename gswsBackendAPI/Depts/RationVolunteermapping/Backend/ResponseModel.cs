using gswsBackendAPI.Depts.PensionVolunteerMapping.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.Depts.RationVolunteermapping.Backend
{
    public class ResponseModel
    {

        public class rationMembersModel
        {
            public string EXISTING_RC_NUMBER { get; set; }
            public List<memberData> memberDatas { get; set; }

        }

        public class memberData
        {
            public string MOBILE_NUMBER { get; set; }
            public string MEMBER_NAME_EN { get; set; }
            public string GT_GENDER { get; set; }
            public string SEC_ID { get; set; }
            public string RS_NAME_EN { get; set; }
        }

        public class RationInputs
        {
            public string ptype { get; set; }
            public int psec_id { get; set; }

            public string pration_id { get; set; }

            public string pvv_id { get; set; }

            public DateTime pUPDATED_ON { get; set; }

            public List<UserData> user_data { get; set; }

            public string pUPDATED_BY { get; set; }

            public string pvv_name { get; set; }

            public string pCLUSTER_ID { get; set; }

            public string pCLUSTER_NAME { get; set; }

        }




        public class UserData
        {
            public string EXISTING_RC_NUMBER { get; set; }
            public string MEMBER_NAME_EN { get; set; }
            public string GT_GENDER { get; set; }
            public string SEC_ID { get; set; }
            public string DISTRICT_STATUS { get; set; }
        }

        public class pensionModel
        {
            public string vv_name { get; set; }
            public string cluster_id { get; set; }
            public string cluster_name { get; set; }
            public string type { get; set; }
            public string gsws_code { get; set; }
            public List<UserData> user_data { get; set; }
            public string vv_id { get; set; }
            public string updated_by { get; set; }
        }

    }
}