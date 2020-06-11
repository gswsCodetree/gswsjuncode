using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gswsBackendAPI.Depts.Energy
{
    public class EnergyModel
    {
    }

    public class AppStatus
    {
        public string CSC_REGNO { get; set; }
        public string PS_TXN_ID { get; set; }
    }

    public class APEPDCLStatus
    {
        public string REQUEST_NO { get; set; }
        public string MOBILE { get; set; }
    }

    public class APEPDCLHistory
    {
        public string REG_NO { get; set; }
        
    }
}