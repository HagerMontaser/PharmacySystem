﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace PharmacySystem.Models
{
    public partial class Item
    {
        public Item()
        {
            Item_Invoices = new HashSet<Item_Invoice>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Quantity { get; set; }
        public double Price { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageName { get; set; }

        public virtual ICollection<Item_Invoice> Item_Invoices { get; set; }
    }
}