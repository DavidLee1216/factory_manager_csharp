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

using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.IO;

namespace factory_manager
{
    /// <summary>
    /// Interaction logic for placeAssets.xaml
    /// </summary>
    public partial class placeAssets : Window
    {
        public MySqlConnection con;
        public int asset_id = 0;
        public placeAssets(int id)
        {
            InitializeComponent();
            change_label_strings();
            con = DatabaseConnection.getDBConnection();
            this.asset_id = id;
            place_date.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            set_remain_count();
        }
        private void set_remain_count()
        {
            int back_cnt = DatabaseConnection.get_back_count(asset_id);
            int exhaust_cnt = DatabaseConnection.get_exhaust_count(asset_id);
            int placed_cnt = DatabaseConnection.get_placed_count(asset_id);
            int asset_cnt = DatabaseConnection.get_asset_count(asset_id);
            remain_count.Text = (asset_cnt - back_cnt - exhaust_cnt - placed_cnt).ToString();
        }
        private bool check_inputs()
        {
            if (place.Text == "")
            {
                common.show_message("请填写摆放工程。", "배치공정을 입력십시오.");
                return false;
            }
            if (count.Text == "")
            {
                common.show_message("请填写摆放数量。", "배치수량을 입력하십시오.");
                return false;
            }
            else
            {
                if (common.check_int_number(count.Text) == false)
                {
                    common.show_message("请输入数字。", "수자를 입력하십시오.");
                    return false;
                }
                int remain_cnt = Convert.ToInt32(remain_count.Text);
                int cnt = Convert.ToInt32(count.Text);
                if(remain_cnt < cnt)
                {
                    common.show_message("摆放数量不能大于未处理数量。", "배치수량은 미처리수량보다 크지 말아야 합니다.");
                    return false;
                }
            }
//             if (remark.Text == "")
//             {
//                 MessageBox.Show("请填写备注。");
//                 return false;
//             }
            DateTime? date = place_date.SelectedDate;
            if (date.HasValue == false)
            {
                common.show_message("选择摆放日期。", "배치날자를 입력하십시오.");
                return false;
            }
            return true;
        }
        private void place_button_Click(object sender, RoutedEventArgs e)
        {
            if (check_inputs())
            {
                int id = DatabaseConnection.get_last_id_from_table("placement_info") + 1;
                string sql = "insert into placement_info (id, asset_id, place, count, date, remark) values(@id, @asset_id, @place, @count, @date, @remark)";
                var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@asset_id", asset_id);
                cmd.Parameters.AddWithValue("@place", place.Text);
                cmd.Parameters.AddWithValue("@count", count.Text);
                DateTime? selectedDate = place_date.SelectedDate;
                string date;
                if (selectedDate.HasValue)
                {
                    date = selectedDate.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    cmd.Parameters.AddWithValue("@date", date);
                }
                cmd.Parameters.AddWithValue("@remark", remarkCombo.Text);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                common.show_message("摆放成功", "성공적으로 입력되였습니다.");
                this.Close();
            }
        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public void change_label_strings()
        {
            if(common.lang==0)
            {
                this.Title = "摆放";
                label_non_process_count.Content = "未处理数量";
                label_place.Content = "摆放工程";
                label_place_count.Content = "摆放数量";
                label_place_date.Content = "摆放日期";
                label_remark.Content = "备注";
                place_button.Content = "摆放";
                cancel_button.Content = "取消";
            }
            else
            {
                this.Title = "배치";
                label_non_process_count.Content = "미배치수량";
                label_place.Content = "배치공정";
                label_place_count.Content = "배치수량";
                label_place_date.Content = "배치날자";
                label_remark.Content = "비고";
                place_button.Content = "배치";
                cancel_button.Content = "취소";
            }
        }
    }
}
