using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftList.Domain
{
    public class Item
    {
        public int pk_ItemID { get; set; }
        public int fk_CategoryID { get; set; }
        public int fk_personID { get; set; }
        public Decimal itemPrice { get; set; }
        public String itemName { get; set; }
        public String description { get; set; }
    }
}
