﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBudget.Models
{
    class Category
    {
        public Category()
        {
            this.Items = new ObservableCollection<Item>();
        }

        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ObservableCollection<Item> Items { get; set; }
    }
}
