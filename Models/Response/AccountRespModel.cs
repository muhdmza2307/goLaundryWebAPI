using System;
using System.Collections.Generic;

namespace goLaundryWebAPI.Models.Response
{
    public class AccDetails
    {
        public int userId { get; set; }
        public int compId { get; set; }
        public string fullName { get; set; }
        public string token { get; set; }
        public DateTime tokenExpires { get; set; }
        public List<AccPermission> accPermList { get; set; }

    }

    public class AccPermission
    {
        public int permId { get; set; }
        public string permCode { get; set; }
        public string permName { get; set; }
        public bool isChecked { get; set; }

    }

    public class AccMenu : AccPermission
    {
        public int menuId { get; set; }
        public string menuName { get; set; }
        public bool isParentMenu { get; set; }
        public int parentMenuId { get; set; }
        public bool isShown { get; set; }
        public int menuSeq { get; set; }
        public string menuUrlLink { get; set; }
        public string menuIcon { get; set; }
    }
    public class MenuList
    {
        public List<AccMenu> parentMenuList { get; set; }
        public List<AccMenu> childMenuList { get; set; }

    }
    
}
