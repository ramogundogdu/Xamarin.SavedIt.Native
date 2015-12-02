using System;
using System.Collections.Generic;
using System.Text;
using SQLite.Net.Attributes;

namespace SavedIt.Portable.Models
{
   public class SavedItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

    }
}
