using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftList.DTO
{
    public class ItemDTO
    {
        public int personID { get; set; }
        public int categoryID { get; set; }
        public String itemName { get; set; }
        public Decimal? itemPrice { get; set; }
        public String description { get; set; }
    }
}
