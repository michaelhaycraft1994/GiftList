using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftList.DTO
{
    public class SearchCriteriaDTO
    {
        public int personID { get; set; }
        public int categoryID { get; set; }
        public double? maxMoney { get; set; }
        public bool? sortA { get; set; }
        public bool? sortZ { get; set; }
        public bool? sortHighValue { get; set; }
        public bool? sortLowValue { get; set; }
    }
}
