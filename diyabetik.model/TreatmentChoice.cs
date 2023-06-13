using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diyabetik.model
{
    public class TreatmentChoice
    {
        public string Id { get; set; }
        public Boolean Insulin { get; set; }
        public Boolean Pump { get; set; }
        public Boolean OralAntidiabetic { get; set; }
        public Boolean InsulinAntidiabetic { get; set; }
    }
}