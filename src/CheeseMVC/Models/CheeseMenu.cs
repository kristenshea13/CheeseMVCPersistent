﻿using System.Collections.Generic;

namespace CheeseMVC.Models
{
    public class CheeseMenu
    {
        public int MenuID { get; set; }
        public Menu Menu { get; set; }

        public int CheeseID { get; set; }
        public Cheese Cheese { get; set; }

        public IList<CheeseMenu> CheeseMenus { get; set; }
    }
}