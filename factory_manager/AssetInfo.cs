using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace factory_manager
{
    public class AssetInfo
    {
        public int id { get; set; }
        public string chinese_name { get; set; }
        public string korean_name { get; set; }
        public string in_date { get; set; }
        public int count { get; set; }
        public double price { get; set; }
        public string owner { get; set; }
        public string model { get; set; }
        public string pay_state { get; set; }
        public AssetInfo(int id, string chinese_name, string korean_name, string in_date, int count, double price, string owner, string model, bool pay_state)
        {
            this.id = id;
            this.chinese_name = chinese_name;
            this.korean_name = korean_name;
            this.in_date = in_date;
            this.count = count;
            this.price = price;
            this.owner = owner;
            this.model = model;
            this.price = price;
            this.pay_state = pay_state?"是":"不";
        }
    }
    public class DeletedAssetInfo
    {
        public int id { get; set; }
        public string chinese_name { get; set; }
        public string korean_name { get; set; }
        public string in_date { get; set; }
        public int count { get; set; }
        public double price { get; set; }
        public string owner { get; set; }
        public string model { get; set; }
        public string deleted_date { get; set; }
        public string deleted_user { get; set; }
        public DeletedAssetInfo(int id, string chinese_name, string korean_name, string in_date, int count, double price, string owner, string model, string delete_date, string deleted_user)
        {
            this.id = id;
            this.chinese_name = chinese_name;
            this.korean_name = korean_name;
            this.in_date = in_date;
            this.count = count;
            this.price = price;
            this.owner = owner;
            this.model = model;
            this.price = price;
            this.deleted_date = delete_date;
            this.deleted_user = deleted_user;
        }
    }
}
