using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diyabetik.model
{
    public class UserInformation
    {
         public string Id { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }  
        public string Waist { get; set; }     
        public string HbA1cPercent { get; set; }
    }
}