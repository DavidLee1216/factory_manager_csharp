using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace factory_manager
{
    public class BackInfo
    {
        public int id { get; set; }
        public int place_id { get; set; }
        public int back_id { get; set; }
        public string place { get; set; }
        public string date { get; set; }
        public int count { get; set; }
        public BackInfo(int id, int place_id, int back_id, string place, string date, int count)
        {
            this.id = id;
            this.place_id = place_id;
            this.back_id = back_id;
            this.place = place;
            this.date = date;
            this.count = count;
        }
    }
}
