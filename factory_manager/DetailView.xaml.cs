using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using MySql.Data.MySqlClient;

namespace factory_manager
{
    /// <summary>
    /// Interaction logic for DetailView.xaml
    /// </summary>
    public partial class DetailView : Window
    {
        int asset_id;
        private MySqlConnection con = null;
        public DetailView(int id)
        {
            InitializeComponent();
            this.asset_id = id;
            change_label_strings();
        }

        private void dataGrid1_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid1.Items.Clear();
            con = DatabaseConnection.getDBConnection();
            string sql = $"select b.id, a.id, b.name, b.kor_name, a.place, b.in_date, a.date, b.model, a.count, b.price, a.remark from placement_info as a, assets_info as b where b.id={asset_id} and a.asset_id=b.id and a.count > 0 order by a.id";
            var cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int asset_id = reader.GetInt32(0);
                int id = reader.GetInt32(1);
                string chinese_name = reader.GetString(2);
                string korean_name = reader.GetString(3);
                string place = reader.GetString(4);
                DateTime in_date = reader.GetDateTime(5);
                string str_in_date = $"{in_date.Year.ToString("D4")}-{in_date.Month.ToString("D2")}-{in_date.Day.ToString("D2")}";
                DateTime date = reader.GetDateTime(6);
                string str_date = $"{date.Year.ToString("D4")}-{date.Month.ToString("D2")}-{date.Day.ToString("D2")}";
                string model = reader.GetString(7);
                int count = reader.GetInt32(8);
                double price = reader.GetDouble(9);
                string remark = reader.GetString(10);
                dataGrid1.Items.Add(new PlacementInfo(asset_id, id, chinese_name, korean_name, place, str_in_date, str_date, model, count, price, remark));
            }
            reader.Close();
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dataGrid_back_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = $"select b.id, a.id, a.place, b.date, b.count from placement_info as a, back_info as b where a.asset_id={asset_id} and b.asset_id={asset_id} and a.id=b.place order by b.id";
            var cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            int i = 1;
            while (reader.Read())
            {
                int back_id = reader.GetInt32(0);
                int place_id = reader.GetInt32(1);
                string place = reader.GetString(2);
                DateTime back_date = reader.GetDateTime(3);
                string str_back_date = $"{back_date.Year.ToString("D4")}-{back_date.Month.ToString("D2")}-{back_date.Day.ToString("D2")}";
                int count = reader.GetInt32(4);
                dataGrid_back.Items.Add(new BackInfo(i, place_id, back_id, place, str_back_date, count));
                i++;
            }
            reader.Close();
        }

        private void dataGrid_back_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void dataGrid_exhaust_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = $"select b.id, a.id, a.place, b.date, b.count from placement_info as a, exhaust_info as b where a.asset_id={asset_id} and b.asset_id={asset_id} and a.id=b.place order by b.id";
            var cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            int i = 1;
            while (reader.Read())
            {
                int back_id = reader.GetInt32(0);
                int place_id = reader.GetInt32(1);
                string place = reader.GetString(2);
                DateTime back_date = reader.GetDateTime(3);
                string str_back_date = $"{back_date.Year.ToString("D4")}-{back_date.Month.ToString("D2")}-{back_date.Day.ToString("D2")}";
                int count = reader.GetInt32(4);
                dataGrid_exhaust.Items.Add(new BackInfo(i, place_id, back_id, place, str_back_date, count));
                i++;
            }
            reader.Close();
        }

        private void dataGrid_exhaust_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public void change_label_strings()
        {
            if(common.lang==0)
            {
                this.Title = "查看详情";
                label_place.Text = "摆放资料";
                grid1_label_asset_no.Header = "资产No";
                grid1_label_place_no.Header = "摆放No";
                grid1_label_chinese_name.Header = "品名";
                grid1_label_korean_name.Header = "品名(朝鲜语)";
                grid1_label_place.Header = "摆放工程";
                grid1_label_place_date.Header = "摆放日期";
                grid1_label_model.Header = "规格/型号";
                grid1_label_count.Header = "数量";
                grid1_label_remark.Header = "备注";
                label_back.Text = "遣返资料";
                grid_back_label_no.Header = "No";
                grid_back_label_back_place.Header = "遣返工程";
                grid_back_label_back_date.Header = "遣返日期";
                grid_back_label_back_count.Header = "数量";
                label_exhaust.Text = "处置资料";
                grid_exhaust_no.Header = "No";
                grid_exhaust_exhaust_place.Header = "处置工程";
                grid_exhaust_exhaust_date.Header = "处置日期";
                grid_exhaust_exhaust_count.Header = "数量";
            }
            else
            {
                this.Title = "상세보기";
                label_place.Text = "배치자료";
                grid1_label_asset_no.Header = "자산번호";
                grid1_label_place_no.Header = "배치번호";
                grid1_label_chinese_name.Header = "품명(중국어)";
                grid1_label_korean_name.Header = "품명(조선어)";
                grid1_label_place.Header = "배치공정";
                grid1_label_place_date.Header = "배치날자";
                grid1_label_model.Header = "규격/형번호";
                grid1_label_count.Header = "수량";
                grid1_label_remark.Header = "비고";
                label_back.Text = "퇴송자료";
                grid_back_label_no.Header = "번호";
                grid_back_label_back_place.Header = "퇴송공정";
                grid_back_label_back_date.Header = "퇴송날자";
                grid_back_label_back_count.Header = "수량";
                label_exhaust.Text = "페기자료";
                grid_exhaust_no.Header = "번호";
                grid_exhaust_exhaust_place.Header = "페기공정";
                grid_exhaust_exhaust_date.Header = "페기날자";
                grid_exhaust_exhaust_count.Header = "수량";
            }
        }
    }
}
