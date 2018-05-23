using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace WorkshopScheduler.iOS
{
    public class LinkerPleaseInclude
    {
        public void Include(UITabBarItem item)
        {
            item.BadgeColor = UIColor.Red;
            item.BadgeValue = "badge";
        }
    }
}
