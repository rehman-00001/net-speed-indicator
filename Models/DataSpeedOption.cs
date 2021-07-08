using System;
using System.Collections.Generic;
using System.Text;

namespace net_speed_indicator.Models
{
    public class DataSpeedOption
    {
        public int Id { get; set;  }
        public string Name { get; set; }
        public DataSpeedOption(int Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }
    }
}
