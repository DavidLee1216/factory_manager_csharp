using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace factory_manager
{
    public class PlacementInfo
    {
        public int id { get; set; }
        public string chinese_name { get; set; }
        public string korean_name { get; set; }
        public string place { get; set; }
        public string in_date { get; set; }
        public int count { get; set; }
        public double price { get; set; }
        public string date { get; set; }
        public string model { get; set; }
        public int asset_id { get; set; }
        public string remark { get; set; }
        public PlacementInfo(int asset_id, int id, string chinese_name, string korean_name, string place, string in_date, string date, string model, int count, double price, string remark)
        {
            this.asset_id = asset_id;
            this.id = id;
            this.chinese_name = chinese_name;
            this.korean_name = korean_name;
            this.place = place;
            this.in_date = in_date;
            this.date = date;
            this.model = model;
            this.count = count;
            this.price = price;
            this.remark = remark;
        }
    }
    public class SearchPlacementInfo
    {
        public int id { get; set; }
        public string chinese_name { get; set; }
        public string korean_name { get; set; }
        public string place { get; set; }
        public string in_date { get; set; }
        public int count { get; set; }
        public double price { get; set; }
        public string date { get; set; }
        public string model { get; set; }
        public int place_id { get; set; }
        public string remark { get; set; }
        public SearchPlacementInfo(int id, int place_id, string chinese_name, string korean_name, string place, string in_date, string date, string model, int count, double price, string remark)
        {
            this.place_id = place_id;
            this.id = id;
            this.chinese_name = chinese_name;
            this.korean_name = korean_name;
            this.place = place;
            this.in_date = in_date;
            this.date = date;
            this.model = model;
            this.count = count;
            this.price = price;
            this.remark = remark;
        }
    }
    public class SearchMovementInfo
    {
        public int id { get; set; }
        public string chinese_name { get; set; }
        public string korean_name { get; set; }
        public string old_place { get; set; }
        public string new_place { get; set; }
        public string in_date { get; set; }
        public int count { get; set; }
        public double price { get; set; }
        public string date { get; set; }
        public string model { get; set; }
        public int place_id { get; set; }
        public string remark { get; set; }
        public SearchMovementInfo(int id, int place_id, string chinese_name, string korean_name, string old_place, string new_place, string in_date, string date, string model, int count, double price)
        {
            this.place_id = place_id;
            this.id = id;
            this.chinese_name = chinese_name;
            this.korean_name = korean_name;
            this.old_place = old_place;
            this.new_place = new_place;
            this.in_date = in_date;
            this.date = date;
            this.model = model;
            this.count = count;
            this.price = price;
            this.remark = remark;
        }
    }
    public class SearchBackInfo
    {
        public int id { get; set; }
        public string chinese_name { get; set; }
        public string korean_name { get; set; }
        public string in_date { get; set; }
        public double price { get; set; }
        public string model { get; set; }
        public int place_id { get; set; }
        public string remark { get; set; }
        public string back_place { get; set; }
        public string back_date { get; set; }
        public int back_count { get; set; }
        public SearchBackInfo(int id, int place_id, string chinese_name, string korean_name, string in_date, string model, double price, string remark, int back_count, string back_date, string back_place)
        {
            this.place_id = place_id;
            this.id = id;
            this.chinese_name = chinese_name;
            this.korean_name = korean_name;
            this.in_date = in_date;
            this.model = model;
            this.price = price;
            this.remark = remark;
            this.back_count = back_count;
            this.back_date = back_date;
            this.back_place = back_place;
        }
    }
    public class SearchExhaustInfo
    {
        public int id { get; set; }
        public string chinese_name { get; set; }
        public string korean_name { get; set; }
        public string in_date { get; set; }
        public double price { get; set; }
        public string model { get; set; }
        public int place_id { get; set; }
        public string remark { get; set; }
        public string exhaust_place { get; set; }
        public string exhaust_date { get; set; }
        public int exhaust_count { get; set; }
        public SearchExhaustInfo(int id, int place_id, string chinese_name, string korean_name, string in_date, string model, double price, string remark, int exhaust_count, string exhaust_date, string exhaust_place)
        {
            this.place_id = place_id;
            this.id = id;
            this.chinese_name = chinese_name;
            this.korean_name = korean_name;
            this.in_date = in_date;
            this.model = model;
            this.price = price;
            this.remark = remark;
            this.exhaust_count = exhaust_count;
            this.exhaust_date = exhaust_date;
            this.exhaust_place = exhaust_place;
        }
    }
}
