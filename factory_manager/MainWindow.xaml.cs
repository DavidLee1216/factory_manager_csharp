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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;

using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Globalization;

namespace factory_manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public static class DatabaseConnection
    {
        static MySqlConnection con = null;
        public static MySqlConnection getDBConnection()
        {
            if (con == null)
            {
                try
                {
                    //                 string connectionString = ConfigurationManager.ConnectionStrings["myDatabaseConnection"].ConnectionString;
                    string connectionString = common.connectionString;
                    con = new MySqlConnection(connectionString);
                    con.Open();
                }
                catch
                {
                    common.show_message("数据库连接错误。", "데이터베이스련결실패!");
                    con = null;
                }
            }
            return con;
        }
        public static int get_back_count(int asset_id)
        {
            string sql = $"select sum(count) from back_info where asset_id={asset_id}";
            var cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            int cnt = 0;
            if(reader.Read() && !reader.IsDBNull(0))
                cnt = reader.GetInt32(0);
            reader.Close();
            return cnt;
        }
        public static int get_exhaust_count(int asset_id)
        {
            string sql = $"select sum(count) from exhaust_info where asset_id={asset_id}";
            var cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            int cnt = 0;
            if (reader.Read() && !reader.IsDBNull(0))
                cnt = reader.GetInt32(0);
            reader.Close();
            return cnt;
        }
        public static int get_placed_count(int asset_id)
        {
            string sql = $"select sum(count) from placement_info where asset_id={asset_id}";
            var cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            int cnt = 0;
            if (reader.Read() && !reader.IsDBNull(0))
                cnt = reader.GetInt32(0);
            reader.Close();
            return cnt;
        }
        public static int get_asset_count(int asset_id)
        {
            string sql = $"select count from assets_info where id={asset_id}";
            var cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            int cnt = 0;
            if (reader.Read() && !reader.IsDBNull(0))
                cnt = reader.GetInt32(0);
            reader.Close();
            return cnt;
        }
        public static int get_last_id_from_table(string tablename)
        {
            string sql = $"select id from {tablename} order by id desc";
            var cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            int id = 0;
            if (reader.Read() && !reader.IsDBNull(0))
            {
                id = reader.GetInt32(0);
            }
            reader.Close();
            return id;
        }
        public static bool check_user_pass(string username, string password)
        {
            string sql = $"select password from user_info where username='{username}'";
            var cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            string pass = "";
            if (reader.Read() && !reader.IsDBNull(0))
            {
                pass = reader.GetString(0);
            }
            reader.Close();
            return pass==password;
        }
        public static bool check_user(string username)
        {
            string sql = $"select * from user_info where username='{username}'";
            var cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            bool bExist = false;
            if (reader.Read() && !reader.IsDBNull(0))
            {
                bExist = true;
            }
            reader.Close();
            return bExist;
        }
        public static void set_pass(string username, string password)
        {
            string sql = $"update user_info set password='{password}' where username='{username}'";
            var cmd = new MySqlCommand(sql, con);
            cmd.ExecuteNonQuery();
        }
        public static void modify_user(string old_user, string new_user)
        {
            string sql = $"update user_info set username='{new_user}' where username='{old_user}'";
            var cmd = new MySqlCommand(sql, con);
            cmd.ExecuteNonQuery();
        }
        public static void delete_user(string user)
        {
            string sql = $"delete from user_info where username='{user}'";
            var cmd = new MySqlCommand(sql, con);
            cmd.ExecuteNonQuery();
        }
    }
    public partial class MainWindow : Window
    {
        public MySqlConnection con;
        private bool bGridLoad1 = true;
        private bool bGridLoad2 = true;

        public MainWindow()
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;
//             AppDomain.CurrentDomain.AssemblyResolve += OnMysqlResolveAssembly;
//             AppDomain.CurrentDomain.AssemblyResolve += OnOpenXmlResolveAssembly;
            try
            {
                InitializeComponent();
                read_default_lang_from_file();
                read_server_setting_from_file();
                con = DatabaseConnection.getDBConnection();
                if (con == null)
                    return;
//                 label_chinese.IsChecked = true;
                change_language();
                var loginView = new login();
                double screenWidth = SystemParameters.PrimaryScreenWidth;
                double screenHeight = SystemParameters.PrimaryScreenHeight;
                double windowWidth = loginView.Width;
                double windowHeight = loginView.Height;
                loginView.Left = (screenWidth / 2) - (windowWidth / 2);
                loginView.Top = (screenHeight / 2) - (windowHeight / 2);
                loginView.ShowDialog();
                if (loginView.closed)
                    this.Close();
                pay_no.IsChecked = true;
                show_tabs();
                show_usernames();
                hide_user_manage_controls();
                set_dates();
                set_default_search_date();
                search_area_combo.SelectedIndex = 0;
                change_check_default_lang_png_visibility();
                if (common.lang == 0)
                {
                    label_chinese.IsChecked = true;
                    label_korean.IsChecked = false;
                }
                else
                {
                    label_korean.IsChecked = true;
                    label_chinese.IsChecked = false;
                }
                //             ConnectDB();
            }
            catch { 

            }
        }
        private static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = new AssemblyName(args.Name);

            var path = assemblyName.Name + ".dll";
            if (assemblyName.CultureInfo.Equals(CultureInfo.InvariantCulture) == false) path = String.Format(@"{0}\{1}", assemblyName.CultureInfo, path);

            using (Stream stream = executingAssembly.GetManifestResourceStream(path))
            {
                if (stream == null) return null;

                var assemblyRawBytes = new byte[stream.Length];
                stream.Read(assemblyRawBytes, 0, assemblyRawBytes.Length);
                return Assembly.Load(assemblyRawBytes);
            }
        }
        private static Assembly OnMysqlResolveAssembly(object sender, ResolveEventArgs args)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();

            var path = "EmbedAssembly.MySql.Data.dll";

            using (Stream stream = executingAssembly.GetManifestResourceStream(path))
            {
                if (stream == null) return null;

                var assemblyRawBytes = new byte[stream.Length];
                stream.Read(assemblyRawBytes, 0, assemblyRawBytes.Length);
                return Assembly.Load(assemblyRawBytes);
            }
        }
        private static Assembly OnOpenXmlResolveAssembly(object sender, ResolveEventArgs args)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();

            var path = "EmbedAssembly.DocumentFormat.OpenXml.dll";

            using (Stream stream = executingAssembly.GetManifestResourceStream(path))
            {
                if (stream == null) return null;

                var assemblyRawBytes = new byte[stream.Length];
                stream.Read(assemblyRawBytes, 0, assemblyRawBytes.Length);
                return Assembly.Load(assemblyRawBytes);
            }
        }
        private void set_dates()
        {
            purchase_date.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            date2.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            back_date.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            exhaust_date.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
        }
        private void set_default_search_date()
        {
            search_date_from.SelectedDate = new DateTime(DateTime.Today.Year-5, DateTime.Today.Month, DateTime.Today.Day);
            search_date_to.SelectedDate = DateTime.Today;
        }
        private void hide_user_manage_controls()
        {
            if(common.username != "admin")
            {
                username_text_setting.Visibility = Visibility.Hidden;
                username_add_button.Visibility = Visibility.Hidden;
                username_modify_button.Visibility = Visibility.Hidden;
                username_delete_button.Visibility = Visibility.Hidden;
            }
        }
        private void show_usernames()
        {
            string sql = $"select username from user_info order by id";
            var cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string name = reader.GetString(0);
                username_list.Items.Add(name);
            }
            reader.Close();
        }
        private void hide_user_pass_control()
        {
            group_user_manage.Visibility = Visibility.Hidden;
            group_pass_manage.Visibility = Visibility.Hidden;
        }
        private void show_tabs()
        {
            if (common.user_mode == false)
            {
                tab1.Visibility = Visibility.Collapsed;
                tab2.Visibility = Visibility.Collapsed;
                tab3.Visibility = Visibility.Visible;
                tab_control.SelectedIndex = 2;
                tab4.Visibility = Visibility.Visible;
                hide_user_pass_control();
            }
            else
            {
                tab1.Visibility = Visibility.Visible;
                tab2.Visibility = Visibility.Visible;
                tab3.Visibility = Visibility.Visible;
                tab4.Visibility = Visibility.Visible;
            }
        }
        private void ConnectDB()
        {
            string cs = @"server=localhost;userid=root;password=Quoted1216;database=factory";
            con = new MySqlConnection(cs);
            con.Open();
        }
        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (bGridLoad1 == false)
                return;
            dataGrid1.Items.Clear();
            string sql = "select * from assets_info order by id";
            var cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string chinese_name = reader.GetString(1);
                string korean_name = reader.GetString(2);
                DateTime in_date = reader.GetDateTime(3);
                string str_in_date = $"{in_date.Year.ToString("D4")}-{in_date.Month.ToString("D2")}-{in_date.Day.ToString("D2")}";
                int count = reader.GetInt32(4);
                string model = reader.GetString(5);
                double price = reader.GetDouble(6);
                string owner = reader.GetString(7);
                bool pay_state = reader.GetBoolean(8);
                dataGrid1.Items.Add(new AssetInfo(id, chinese_name, korean_name, str_in_date, count, price, owner, model, pay_state));
            }
            reader.Close();
//             var items = new List<Dog>();
//             items.Add(new Dog("Fido", 10));
//             items.Add(new Dog("Spark", 20));
//             items.Add(new Dog("Fluffy", 4));
// 
//             // ... Assign ItemsSource of DataGrid.
//             var grid = sender as DataGrid;
//             grid.ItemsSource = items;
        }
        private bool check_number(string str)
        {
            if (Regex.IsMatch(str, @"^[0-9]+[.]?[0-9]*$"))
                return true;
            else
                return false;
        }
        private bool check_inputs()
        {
            if(asset_chinese_name.Text=="")
            {
                common.show_message("请填写品名。", "품명(중국어)을 입력하십시오."); 
                return false;
            }
//             if(asset_korean_name.Text=="")
//             {
//                 common.show_message("请填写朝鲜语品名。", "품명(조선어)을 입력하십시오.");
//                 return false;
//             }
            if (model.Text == "")
            {
                common.show_message("请填写规格/型号。", "규격/형번호를 입력하십시오.");
                return false;
            }
            if (owner.Text == "")
            {
                common.show_message("请填写归属厂。", "소유공장명을 입력하십시오.");
                return false;
            }
            DateTime? date = purchase_date.SelectedDate;
            if(date.HasValue==false)
            {
                common.show_message("选择入库日期。", "입고날자를 입력하십시오.");
                return false;
            }
            if(count.Text=="")
            {
                common.show_message("请填写数量。", "수량을 입력하십시오.");
                return false;
            }
            else
            {
                count.Text = count.Text.Replace(",", string.Empty);
                if(check_number(count.Text)==false)
                {
                    common.show_message("填写数字(数量)。", "수자를 입력하십시오.");
                    return false;
                }
            }
            if (price.Text == "")
            {
                common.show_message("请填写单价。", "단가를 입력하십시오.");
                return false;
            }
            else
            {
                price.Text = price.Text.Replace(",", string.Empty);
                if (check_number(price.Text) == false)
                {
                    common.show_message("填写数字(单价)。", "수자를 입력하십시오.");
                    return false;
                }
            }
            return true;
        }
        private void add_asset(object sender, RoutedEventArgs e)
        {
            if (check_inputs() == false)
                return;
            string sql = "insert into assets_info (id, name, kor_name, in_date, count, model, price, owner, pay_state, exhaust, back) values(@id, @name, @korean_name, @in_date, @count, @model, @price, @owner, @pay_state, '0', '0')";
            var cmd = new MySqlCommand(sql, con);
            int id = DatabaseConnection.get_last_id_from_table("assets_info") + 1;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", asset_chinese_name.Text);
            cmd.Parameters.AddWithValue("@korean_name", asset_korean_name.Text);
            DateTime? selectedDate = purchase_date.SelectedDate;
            string date = "";
            if (selectedDate.HasValue)
            {
                date = selectedDate.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                cmd.Parameters.AddWithValue("@in_date", date);
            }
            cmd.Parameters.AddWithValue("@model", model.Text);
            cmd.Parameters.AddWithValue("@count", count.Text);
            cmd.Parameters.AddWithValue("@price", price.Text);
            cmd.Parameters.AddWithValue("@owner", owner.Text);
            bool? pp = pay_yes.IsChecked;
            Byte pay_byte = 0;
            if (pp == true)
                pay_byte = 1;
            cmd.Parameters.AddWithValue("@pay_state", pay_byte);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            dataGrid1.Items.Add(new AssetInfo(id, asset_chinese_name.Text, asset_korean_name.Text, date, Convert.ToInt32(count.Text), Convert.ToDouble(price.Text), owner.Text, model.Text, Convert.ToBoolean(pay_byte)));
        }
        private int get_id_from_grid(object Item)
        {
            string id = (dataGrid1.SelectedCells[0].Column.GetCellContent(Item) as TextBlock).Text;
            return Convert.ToInt32(id);
        }
        private void delete_asset(object sender, RoutedEventArgs e)
        {
            var selectedItem = dataGrid1.SelectedItem;
            if (selectedItem == null)
            {
                common.show_message("请选择要删除的资料。", "지우려는 자료를 선택하십시오.");
                return;
            }
            //             string id = (dataGrid1.SelectedCells[0].Column.GetCellContent(selectedItem) as TextBlock).Text;
            int id = get_id_from_grid(selectedItem);
            if (common.show_message_yesno("确定您要删除资料", "정말 자료를 지우겠습니까?"))
            {
                add_to_delete_history(id);
                string query = $"delete from assets_info where id='{id}'";
                var cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();
                query = $"delete from placement_info where asset_id='{id}'";
                cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();
                query = $"delete from back_info where asset_id='{id}'";
                cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();
                query = $"delete from movement_info where asset_id='{id}'";
                cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();
                query = $"delete from exhaust_info where asset_id='{id}'";
                cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();
                if (selectedItem != null)
                {
                    dataGrid1.Items.Remove(selectedItem);
                }
            }
        }
        private void add_to_delete_history(int id)
        {
            string sql = $"select * from assets_info where id='{id}'";
            var cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string chinese_name = reader.GetString(1);
                string kor_name = reader.GetString(2);
                DateTime in_date = reader.GetDateTime(3);
                string str_in_date = $"{in_date.Year.ToString("D4")}-{in_date.Month.ToString("D2")}-{in_date.Day.ToString("D2")}";
                string model = reader.GetString(5);
                int count = reader.GetInt32(4);
                double price = reader.GetDouble(6);
                DateTime today = DateTime.Today;
                string str_date = $"{today.Year.ToString("D4")}-{today.Month.ToString("D2")}-{today.Day.ToString("D2")}";
                string owner = reader.GetString(7);
                reader.Close();
                int delete_id = DatabaseConnection.get_last_id_from_table("delete_history_info") + 1;
                sql = $"insert into delete_history_info (id, name, kor_name, in_date, count, model, price, owner, delete_date, user) values('{delete_id}','{chinese_name}'," +
                    $"'{kor_name}', '{str_in_date}', '{count}', '{model}', '{price}', '{owner}', '{str_date}', '{common.username}')";
                cmd = new MySqlCommand(sql, con);
                cmd.ExecuteNonQuery();
            }
        }
        private void modify_asset(object sender, RoutedEventArgs e)
        {
            var selectedItem = dataGrid1.SelectedItem;
            if(selectedItem==null)
            {
                common.show_message("请选择要变更的资料。", "변경할 자료를 선택하십시오.");
                return;
            }
            if (check_inputs() == false)
                return;
            if (common.show_message_yesno("确定您要变更资料", "정말 자료를 변경겠습니까?"))
            {
                int id = get_id_from_grid(selectedItem);
                DateTime? selectedDate = purchase_date.SelectedDate;
                string date = "";
                if (selectedDate.HasValue)
                {
                    date = selectedDate.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                }
                bool? pp = pay_yes.IsChecked;
                Byte pay_byte = 0;
                if (pp == true)
                    pay_byte = 1;
                string query = $"update assets_info set name='{asset_chinese_name.Text}',kor_name='{asset_korean_name.Text}', in_date='{date}', count='{count.Text}', model='{model.Text}', price='{price.Text}', owner='{owner.Text}', pay_state='{pay_byte}' where id='{id}'";
                var cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();
                if (selectedItem != null)
                {
                    AssetInfo asset_info = selectedItem as AssetInfo;
                    asset_info.chinese_name = asset_chinese_name.Text;
                    asset_info.korean_name = asset_korean_name.Text;
                    asset_info.in_date = date;
                    asset_info.price = Convert.ToDouble(price.Text);
                    asset_info.model = model.Text;
                    asset_info.owner = owner.Text;
                    asset_info.pay_state = Convert.ToBoolean(pay_byte)?"是":"不";
                    dataGrid1.Items.Refresh();
                }
            }
        }
        private void place_asset(object sender, RoutedEventArgs e)
        {
            var selectedItem = dataGrid1.SelectedItem;
            if (selectedItem == null)
            {
                common.show_message("请选择要变更的资料。", "변경할 자료를 선택하십시오.");
                return;
            }
            int id = get_id_from_grid(selectedItem);
            var placeWindow = new placeAssets(id);
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = placeWindow.Width;
            double windowHeight = placeWindow.Height;
            placeWindow.Left = (screenWidth / 2) - (windowWidth / 2);
            placeWindow.Top = (screenHeight / 2) - (windowHeight / 2);
            placeWindow.ShowDialog();
        }
        private int get_remain_asset_count(int asset_id)
        {
            int back_cnt = DatabaseConnection.get_back_count(asset_id);
            int exhaust_cnt = DatabaseConnection.get_exhaust_count(asset_id);
            int placed_cnt = DatabaseConnection.get_placed_count(asset_id);
            int asset_cnt = DatabaseConnection.get_asset_count(asset_id);
            return asset_cnt - placed_cnt - exhaust_cnt - back_cnt;
        }
        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = dataGrid1.SelectedItem;
            if (selectedItem == null)
                return;
            else
            {
                var item = selectedItem as AssetInfo;
                asset_chinese_name.Text = item.chinese_name;
                asset_korean_name.Text = item.korean_name;
                model.Text = item.model;
                owner.Text = item.owner;
                purchase_date.SelectedDate = DateTime.Parse(item.in_date);
                count.Text = item.count.ToString();
                price.Text = item.price.ToString();
                pay_yes.IsChecked = (item.pay_state=="是");
                pay_no.IsChecked = !pay_yes.IsChecked;
            }
        }

        private void dataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var selectedItem = dataGrid1.SelectedItem;
                int id = get_id_from_grid(selectedItem);
                var detailView = new DetailView(id);
                double screenWidth = SystemParameters.PrimaryScreenWidth;
                double screenHeight = SystemParameters.PrimaryScreenHeight;
                double windowWidth = detailView.Width;
                double windowHeight = detailView.Height;
                detailView.Left = (screenWidth / 2) - (windowWidth / 2);
                detailView.Top = (screenHeight / 2) - (windowHeight / 2);
                detailView.ShowDialog();
            }
            catch (Exception excp)
            {
                MessageBox.Show(excp.Message);
            }
            return;
        }

        private void dataGrid2_Loaded(object sender, RoutedEventArgs e)
        {
            if (bGridLoad2 == false)
                return;
            dataGrid2.Items.Clear();
            string sql = "select b.id, a.id, b.name, b.kor_name, a.place, b.in_date, a.date, b.model, a.count, b.price, a.remark from placement_info as a, assets_info as b where a.asset_id=b.id and a.count > 0 order by a.asset_id";
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
                dataGrid2.Items.Add(new PlacementInfo(asset_id, id, chinese_name, korean_name, place, str_in_date, str_date, model, count, price, remark));
            }
            reader.Close();
        }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = dataGrid2.SelectedItem;
            if (selectedItem == null)
                return;
            else
            {
                var item = selectedItem as PlacementInfo;
                place2.Text = item.place;
                count2.Text = item.count.ToString();
                remarkCombo2.Text = item.remark;
                date2.SelectedDate = DateTime.Parse(item.date);
                int asset_id = item.asset_id;
                int place_id = item.id;
                Load_dataGrid_back(asset_id, place_id);
                Load_dataGrid_exhaust(asset_id, place_id);
            }
        }

        private void dataGrid2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void simple_search_Click(object sender, RoutedEventArgs e)
        {
            dataGrid2.Items.Clear();
            string sql = "select b.id, a.id, b.name, b.kor_name, a.place, b.in_date, a.date, b.model, a.count, b.price, a.remark from placement_info as a, assets_info as b where a.asset_id=b.id order by a.asset_id";
            if (simple_search_id.Text != "")
                sql = $"select b.id, a.id, b.name, b.kor_name, a.place, b.in_date, a.date, b.model, a.count, b.price, a.remark from placement_info as a, assets_info as b where a.asset_id=b.id and b.id={simple_search_id.Text} order by a.asset_id";
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
                dataGrid2.Items.Add(new PlacementInfo(asset_id, id, chinese_name, korean_name, place, str_in_date, str_date, model, count, price, remark));
            }
            reader.Close();

        }
        private void modify_place(object sender, RoutedEventArgs e)
        {
            var selectedItem = dataGrid2.SelectedItem;
            if (selectedItem == null)
            {
                common.show_message("请选择要变更的资料。", "변경할 자료를 선택하십시오.");
                return;
            }
            PlacementInfo info = selectedItem as PlacementInfo;
            int id = info.id;
            if (place2.Text == "")
            {
                common.show_message("填写变更/移动工程名。", "변경/이동할 공정명을 입력하십시오.");
                return;
            }
            DateTime? selectedDate = date2.SelectedDate;
            string date = "";
            if (selectedDate.HasValue)
            {
                date = selectedDate.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            if(count2.Text=="")
            {
                common.show_message("填写变更/移动数量。", "변경/이동할 수량을 입력하십시오.");
                return;
            }
            else
            {
                int cnt = Convert.ToInt32(count2.Text);
                int remain_cnt = get_remain_asset_count(info.asset_id);
                if (cnt - info.count > remain_cnt)
                {
                    common.show_message("审查填写数量。超过资产入库数量。", "입력수량을 다시 검토하십시오.재산입고수량을 초과했습니다.");
                    return;
                }
            }
            if (common.show_message_yesno("确定您要变更。", "정말 변경하겠습니까?"))
            {
                try
                {
                    string query = $"update placement_info set date='{date}', count='{count2.Text}', place='{place2.Text}' where id='{id}'";
                    var cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    if (selectedItem != null)
                    {
                        info.count = Convert.ToInt32(count2.Text);
                        info.place = place2.Text;
                        info.date = date;
                        dataGrid2.Items.Refresh();
                        common.show_message("操作成功", "조작성공");
                    }
                }
                catch
                {
                    common.show_message("操作失败", "조작실패");
                }
            }
        }

        private void delete_place(object sender, RoutedEventArgs e)
        {
            var selectedItem = dataGrid2.SelectedItem;
            if (selectedItem == null)
            {
                common.show_message("请选择要删除的资料。", "삭제할 자료를 선택하십시오.");
                return;
            }
            if(common.show_message_yesno("确定您要删除。", "정말 삭제하겠습니까?"))
            {
                try
                {
                    PlacementInfo info = selectedItem as PlacementInfo;
                    int id = info.id;
                    string query = $"delete from placement_info where id='{id}'";
                    var cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    dataGrid2.Items.Remove(selectedItem);
                    common.show_message("操作成功", "조작성공");
                }
                catch
                {
                    common.show_message("操作失败", "조작실패");
                }
            }
        }
        private void move_place(object sender, RoutedEventArgs e)
        {
            var selectedItem = dataGrid2.SelectedItem;
            if (selectedItem == null)
            {
                common.show_message("请选择要移动的资料。", "변경할 자료를 선택하십시오.");
                return;
            }
            PlacementInfo info = selectedItem as PlacementInfo;
            int id = info.id;
            if (place2.Text == "")
            {
                common.show_message("填写变更/移动工程名。", "변경/이동할 공정명을 입력하십시오.");
                return;
            }
            DateTime? selectedDate = date2.SelectedDate;
            string date = "";
            int cnt;
            if (selectedDate.HasValue)
            {
                date = selectedDate.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                common.show_message("选择移动日期。", "이동날자를 입력하십시오.");
                return;
            }
            if (count2.Text == "")
            {
                common.show_message("填写变更/移动数量。", "변경/이동할 수량을 입력하십시오.");
                return;
            }
            else
            {
                cnt = Convert.ToInt32(count2.Text);
                int remain_cnt = get_remain_asset_count(info.asset_id);
                if (cnt > info.count)
                {
                    common.show_message("审查填写数量。超过摆放数量。", "입력수량을 검토하십시오. 배치수량을 초과했습니다.");
                    return;
                }
            }
            if (common.show_message_yesno("确定您要移动。", "정말 이동하겠습니까?"))
            {
                try
                {
                    string remark = remarkCombo2.Text;

                    string query;
                    int new_id = DatabaseConnection.get_last_id_from_table("placement_info") + 1;
                    query = "insert into placement_info (id, asset_id, place, count, date, remark) values(@id, @asset_id, @place, @count, @date, @remark)";
                    var cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id", new_id);
                    cmd.Parameters.AddWithValue("@asset_id", info.asset_id);
                    cmd.Parameters.AddWithValue("@place", place2.Text);
                    cmd.Parameters.AddWithValue("@count", count2.Text);
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.Parameters.AddWithValue("@remark", remark);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();

                    int movement_id = DatabaseConnection.get_last_id_from_table("movement_info") + 1;
                    string sql = "insert into movement_info (id, asset_id, old_place, new_place, count, date) values(@id, @asset_id, @old_place, @new_place, @count, @date)";
                    cmd = new MySqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@id", movement_id);
                    cmd.Parameters.AddWithValue("@asset_id", info.asset_id);
                    cmd.Parameters.AddWithValue("@old_place", info.id);
                    cmd.Parameters.AddWithValue("@new_place", new_id);
                    cmd.Parameters.AddWithValue("@count", cnt);
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                    query = $"update placement_info set count='{info.count - cnt}' where id='{id}'";
                    cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    info.count = info.count - Convert.ToInt32(count2.Text);

                    PlacementInfo newInfo = new PlacementInfo(info.asset_id, new_id, info.chinese_name, info.korean_name, place2.Text, info.in_date, date, info.model, Convert.ToInt32(count2.Text), info.price, remark);
                    dataGrid2.Items.Add(newInfo);
                    dataGrid2.Items.Refresh();
                    common.show_message("操作成功", "조작성공");
                }
                catch
                {
                    common.show_message("操作失败", "조작실패");
                }
            }
        }
        private void back_asset(object sender, RoutedEventArgs e)
        {
            var selectedItem = dataGrid2.SelectedItem;
            if (selectedItem == null)
            {
                common.show_message("请选择该返送的资料。", "반환할 자료를 선택하십시오.");
                return;
            }
            PlacementInfo info = selectedItem as PlacementInfo;
            int id = info.id;
            DateTime? selectedDate = back_date.SelectedDate;
            string date = "";
            int cnt;
            if (selectedDate.HasValue)
            {
                date = selectedDate.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                common.show_message("选择返送日期。", "반환날자를 입력하십시오.");
                return;
            }
            if (back_count.Text == "")
            {
                common.show_message("填写返送数量。", "반환할 수량을 입력하십시오.");
                return;
            }
            else
            {
                cnt = Convert.ToInt32(back_count.Text);
                if (cnt > info.count)
                {
                    common.show_message("审查填写数量。超过摆放数量。", "입력수량을 검토하십시오. 배치수량을 초과했습니다.");
                    return;
                }
            }
            if (common.show_message_yesno("确定您要返送。", "정말 반환하겠습니까?"))
            {
                try
                {
                    int back_id = DatabaseConnection.get_last_id_from_table("back_info") + 1;
                    string sql = "insert into back_info (id, asset_id, place, count, date) values(@id, @asset_id, @place, @count, @date)";
                    var cmd = new MySqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@id", back_id);
                    cmd.Parameters.AddWithValue("@asset_id", info.asset_id);
                    cmd.Parameters.AddWithValue("@place", info.id);
                    cmd.Parameters.AddWithValue("@count", cnt);
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                    string query;
                    query = $"update placement_info set count='{info.count - cnt}' where id='{id}'";
                    cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    Load_dataGrid_back(info.asset_id, info.id);
                    if (info.count == cnt)
                        dataGrid2.Items.Remove(selectedItem);
                    else
                        info.count = info.count - cnt;
                    dataGrid2.Items.Refresh();
                    common.show_message("操作成功", "조작성공");
                }
                catch
                {
                    common.show_message("操作失败", "조작실패");
                }
            }
        }

        private void exhaust_asset(object sender, RoutedEventArgs e)
        {
            var selectedItem = dataGrid2.SelectedItem;
            if (selectedItem == null)
            {
                common.show_message("请选择该处置的资料。", "페기할 자료를 선택하십시오.");
                return;
            }
            try
            {
                PlacementInfo info = selectedItem as PlacementInfo;
                int id = info.id;
                DateTime? selectedDate = exhaust_date.SelectedDate;
                string date;
                int cnt;
                if (selectedDate.HasValue)
                {
                    date = selectedDate.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    common.show_message("选择处置日期。", "페기날자를 입력하십시오.");
                    return;
                }
                if (exhaust_count.Text == "")
                {
                    common.show_message("填写处置数量。", "페기할 수량을 입력하십시오.");
                    return;
                }
                else
                {
                    cnt = Convert.ToInt32(exhaust_count.Text);
                    if (cnt > info.count)
                    {
                        common.show_message("审查填写数量。超过摆放数量。", "입력수량을 검토하십시오. 배치수량을 초과했습니다.");
                        return;
                    }
                }
                if (common.show_message_yesno("确定您要处置。", "정말 페기하겠습니까?"))
                {
                    int exhaust_id = DatabaseConnection.get_last_id_from_table("exhaust_info") + 1;
                    string sql = "insert into exhaust_info (id, asset_id, place, count, date) values(@id, @asset_id, @place, @count, @date)";
                    var cmd = new MySqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@id", exhaust_id);
                    cmd.Parameters.AddWithValue("@asset_id", info.asset_id);
                    cmd.Parameters.AddWithValue("@place", info.id);
                    cmd.Parameters.AddWithValue("@count", cnt);
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                    string query;
                    query = $"update placement_info set count='{info.count - cnt}' where id='{id}'";
                    cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    Load_dataGrid_exhaust(info.asset_id, info.id);
                    if (info.count == cnt)
                        dataGrid2.Items.Remove(selectedItem);
                    else
                       info.count = info.count - cnt;
                    dataGrid2.Items.Refresh();
                    common.show_message("操作成功", "조작성공");
                }
            }
            catch
            {
                common.show_message("操作失败", "조작실패");
            }
        }

        private void dataGrid_back_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void Load_dataGrid_back(int asset_id, int place_id)
        {
            dataGrid_back.Items.Clear();
            string sql = $"select b.id, a.place, b.date, b.count from placement_info as a, back_info as b where a.id={place_id} and a.id=b.place order by b.id";
            var cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int back_id = reader.GetInt32(0);
                string place = reader.GetString(1);
                DateTime back_date = reader.GetDateTime(2);
                string str_back_date = $"{back_date.Year.ToString("D4")}-{back_date.Month.ToString("D2")}-{back_date.Day.ToString("D2")}";
                int count = reader.GetInt32(3);
                dataGrid_back.Items.Add(new BackInfo(asset_id, place_id, back_id, place, str_back_date, count));
            }
            dataGrid_back.Items.Refresh();
            reader.Close();
        }
        private void dataGrid_back_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dataGrid_exhaust_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void Load_dataGrid_exhaust(int asset_id, int place_id)
        {
            dataGrid_exhaust.Items.Clear();
            string sql = $"select b.id, a.place, b.date, b.count from placement_info as a, exhaust_info as b where a.id={place_id} and a.id=b.place order by b.id";
            var cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int exhaust_id = reader.GetInt32(0);
                string place = reader.GetString(1);
                DateTime back_date = reader.GetDateTime(2);
                string str_back_date = $"{back_date.Year.ToString("D4")}-{back_date.Month.ToString("D2")}-{back_date.Day.ToString("D2")}";
                int count = reader.GetInt32(3);
                dataGrid_exhaust.Items.Add(new BackInfo(asset_id, place_id, exhaust_id, place, str_back_date, count));
            }
            dataGrid_exhaust.Items.Refresh();
            reader.Close();
        }
        private int findIdInPlacementGrid(int id)
        {
            int i = 0;
            foreach(PlacementInfo info in dataGrid2.Items)
            {
                if (info.id == id)
                    return i;
                i++;
            }
            return -1;
        }
        private void dataGrid_exhaust_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void back_cancel(object sender, RoutedEventArgs e)
        {
            var selectedItem = dataGrid_back.SelectedItem;
            if (selectedItem == null)
            {
                common.show_message("请选择要取消的资料。", "취소할 자료를 선택하십시오.");
                return;
            }
            try
            {
                if(common.show_message_yesno("确定您要取消", "정말 취소하겠습니까?"))
                {
                    BackInfo info = selectedItem as BackInfo;
                    int back_id = info.back_id;
                    int place_id = info.place_id;
                    int count = info.count;
                    string query = $"update placement_info a, back_info b set a.count=a.count+{count} where b.id={back_id} and a.id=b.place";
                    var cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    query = $"delete from back_info where id={back_id}";
                    cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    dataGrid_back.Items.Remove(selectedItem);
                    dataGrid_back.Items.Refresh();
                    int placeGridIdx = findIdInPlacementGrid(place_id);
                    if (placeGridIdx == -1) 
                    {
                        query = $"select b.id, a.id, b.name, b.kor_name, a.place, b.in_date, a.date, b.model, a.count, b.price, a.remark from placement_info as a, assets_info as b where a.asset_id=b.id and a.id={place_id}";
                        cmd = new MySqlCommand(query, con);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
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
                            count = reader.GetInt32(8);
                            double price = reader.GetDouble(9);
                            string remark = reader.GetString(10);
                            dataGrid2.Items.Add(new PlacementInfo(asset_id, id, chinese_name, korean_name, place, str_in_date, str_date, model, count, price, remark));
                        }
                        reader.Close();

                    }
                    else
                    {
                        PlacementInfo placementInfo = dataGrid2.Items[placeGridIdx] as PlacementInfo;
                        placementInfo.count += count;
                    }
                    dataGrid2.Items.Refresh();
                }
            }
            catch
            {

            }
        }
        private void exhaust_cancel(object sender, RoutedEventArgs e)
        {
            var selectedItem = dataGrid_exhaust.SelectedItem;
            if (selectedItem == null)
            {
                common.show_message("请选择要取消的资料。", "취소할 자료를 선택하십시오.");
                return;
            }
            try
            {
                if (common.show_message_yesno("确定您要取消", "정말 취소하겠습니까?"))
                {
                    BackInfo info = selectedItem as BackInfo;
                    int back_id = info.back_id;
                    int place_id = info.place_id;
                    int count = info.count;
                    string query = $"update placement_info a, exhaust_info b set a.count=a.count+{count} where b.id={back_id} and a.id=b.place";
                    var cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    query = $"delete from exhaust_info where id={back_id}";
                    cmd = new MySqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    dataGrid_exhaust.Items.Remove(selectedItem);
                    dataGrid_exhaust.Items.Refresh();
                    int placeGridIdx = findIdInPlacementGrid(place_id);
                    if (placeGridIdx == -1)
                    {
                        query = $"select b.id, a.id, b.name, b.kor_name, a.place, b.in_date, a.date, b.model, a.count, b.price, a.remark from placement_info as a, assets_info as b where a.asset_id=b.id and a.id={place_id}";
                        cmd = new MySqlCommand(query, con);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
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
                            count = reader.GetInt32(8);
                            double price = reader.GetDouble(9);
                            string remark = reader.GetString(10);
                            dataGrid2.Items.Add(new PlacementInfo(asset_id, id, chinese_name, korean_name, place, str_in_date, str_date, model, count, price, remark));
                        }
                        reader.Close();
                    }
                    else
                    {
                        PlacementInfo placementInfo = dataGrid2.Items[placeGridIdx] as PlacementInfo;
                        placementInfo.count += count;
                    }
                    dataGrid2.Items.Refresh();
                }
            }
            catch
            {

            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl tab = sender as TabControl;
            if (tab.SelectedIndex == -1)
                return;
            string tabItem = ((sender as TabControl).SelectedItem as TabItem).Name as string;
            switch (tabItem)
            {
                case "tab1":
                    if(bGridLoad1==false)
                        bGridLoad2 = false;
                    break;
                case "tab2":
                    bGridLoad1 = false; break;
                default:
                    return;
            }
        }
        private void change_language()
        {
            if(common.lang == 0)
            {
                this.Title = "工厂管理器";
                tab1.Header = "资产入库摆放";
                tab2.Header = "摆放资产管理";
                tab3.Header = "资产材料查看";
                tab4.Header = "设定";
                label_lang_setting.Text = "设定语言";
                label_chinese.Content = "汉语";
                label_korean.Content = "朝鲜语";
                label_input_data.Text = "输入资料";
                label_chinese_name.Text = "品名";
                label_korean_name.Text = "品名(朝鲜语)";
                label_model.Text = "规格/型号";
                label_owner.Text = "归属厂";
                label_in_date.Text = "入库日期";
                label_count.Text = "数量";
                label_price.Text = "单价";
                pay_block.Text = "付款情况";
                pay_yes.Content = "是";
                pay_no.Content = "不";
                label_add.Text = "添加";
                label_place.Text = "摆放";
                label_asset_modify.Text = "变更";
                label_asset_delete.Text = "删除";
                label_add_excel_data.Text = "Excel资料添加";
                grid1_label_no.Header = "No";
                grid1_label_chinese_name.Header = "品名";
                grid1_label_korean_name.Header = "品名(朝鲜语)";
                grid1_label_in_date.Header = "入库日期";
                grid1_label_count.Header = "数量";
                grid1_label_model.Header = "规格/型号";
                grid1_label_price.Header = "单价";
                grid1_label_owner.Header = "归属厂";
                grid1_label_pay_state.Header = "付款情况";
                grid2_label_no.Header = "No";
                grid2_label_chinese_name.Header = "品名";
                grid2_label_korean_name.Header = "品名(朝鲜语)";
                grid2_label_place.Header = "摆放工程";
                grid2_label_in_date.Header = "入库日期";
                grid2_label_place_date.Header = "摆放日期";
                grid2_label_model.Header = "规格/型号";
                grid2_label_count.Header = "数量";
                grid2_label_price.Header = "单价";
                grid2_label_remark.Header = "备注";
                grid_back_label_no.Header = "No";
                grid_back_label_place.Header = "返送工程";
                grid_back_label_date.Header = "返送日期";
                grid_back_label_count.Header = "数量";
                grid_exhaust_label_no.Header = "No";
                grid_exhaust_label_place.Header = "处置工程";
                grid_exhaust_label_date.Header = "处置日期";
                grid_exhaust_label_count.Header = "数量";
                label_simple_search.Text = "简单查询";
                label_button_search.Content = "查看";
                label_place_modify_move_delete.Text = "摆放工程变更/移动/删除";
                label_modify_move_place_name.Text = "变更/移动工程名";
                label_modify_move_date.Text = "变更/移动日期";
                label_modify_move_count.Text = "变更/移动数量";
                label_combo_remark.Text = "备注";
                label_button_modify.Content = "变更";
                label_button_delete.Content = "删除";
                label_button_move.Content = "移动";
                label_group_back.Text = "返送";
                label_group_back_count.Text = "数量";
                label_group_back_date.Text = "返送日期";
                label_group_back_button_back.Content = "返送";
                label_group_back_button_cancel.Content = "取消";
                label_group_exhaust.Text = "处置";
                label_group_exhaust_count.Text = "数量";
                label_group_exhaust_date.Text = "处置日期";
                label_group_exhaust_button_exhaust.Content = "处置";
                label_group_exhaust_button_cancel.Content = "取消";
                label_label_back_data.Content = "返送资料";
                label_label_exhaust_data.Content = "处置资料";
                label_label_place_data.Content = "摆放资料";

                btn_set_default_lang.Content = "设定为标准";
                label_user_setting.Text = "用户管理";
                label_username_setting.Content = "用户名";
                username_add_button.Content = "添加";
                username_modify_button.Content = "变更";
                username_delete_button.Content = "删除";
                group_pass_manage.Header = "密码管理";
                label_old_pass.Content = "旧密码";
                label_new_pass.Content = "新密码";
                label_retype_new_pass.Content = "新密码";
                btn_pass_change.Content = "变更";

                label_group_search_items.Header = "搜索项目";
                label_search_area.Content = "搜索范围";
                search_label_chinese_name.Content = "品名";
                search_label_korean_name.Content = "品名(朝鲜语)";
                search_label_date.Content = "日期";
                search_label_place.Content = "工程";
                search_label_model.Content = "规格/型号";
                btn_search.Content = "查看";
                grid_search_label_place_no.Header = "摆放No";
                grid_search_label_chinese_name.Header = "品名";
                grid_search_label_korean_name.Header = "品名(朝鲜语)";
                grid_search_label_in_date.Header = "入库日期";
                grid_search_label_place_date.Header = "摆放日期";
                grid_search_label_place.Header = "摆放工程";
                grid_search_label_place_old.Header = "前工程";
                grid_search_label_place_new.Header = "后工程";
                grid_search_label_model.Header = "规格/型号";
                grid_search_label_count.Header = "数量";
                grid_search_label_price.Header = "单价";
                grid_search_label_owner.Header = "归属厂";
                grid_search_label_pay_state.Header = "付款情况";
                grid_search_label_remark.Header = "备注";
                grid_search_label_back_count.Header = "返送数量";
                grid_search_label_back_date.Header = "返送日期";
                grid_search_label_back_place.Header = "返送工程";
                grid_search_label_exhaust_count.Header = "处置数量";
                grid_search_label_exhaust_date.Header = "处置日期";
                grid_search_label_exhaust_place.Header = "处置工程";
                grid_search_label_delete_history_date.Header = "删除日期";
                grid_search_label_delete_history_user.Header = "用户";



                item1.Content = "入库资料";
                item2.Content = "摆放资料";
                item3.Content = "移动资料";
                item4.Content = "返送资料";
                item5.Content = "处置资料";
                item6.Content = "删除记录资料";

            }
            else
            {
                this.Title = "공장관리프로그람";
                tab1.Header = "재산입고 및 배치";
                tab2.Header = "배치재산관리";
                tab3.Header = "재산자료검색";
                tab4.Header = "설정";
                label_lang_setting.Text = "언어설정";
                label_chinese.Content = "중국어";
                label_korean.Content = "조선어";
                label_input_data.Text = "자료입력";
                label_chinese_name.Text = "품명(중국어)";
                label_korean_name.Text = "품명(조선어)";
                label_model.Text = "규격/형번호";
                label_owner.Text = "소유공장";
                label_in_date.Text = "입고날자";
                label_count.Text = "수량";
                label_price.Text = "단가";
                pay_block.Text = "지불상황";
                pay_yes.Content = "예";
                pay_no.Content = "아니";
                label_add.Text = "추가";
                label_place.Text = "배치";
                label_asset_modify.Text = "변경";
                label_asset_delete.Text = "삭제";
                label_add_excel_data.Text = "Excel자료추가";

                grid1_label_no.Header = "번호";
                grid1_label_chinese_name.Header = "품명(중국어)";
                grid1_label_korean_name.Header = "품명(조선어)";
                grid1_label_in_date.Header = "입고날자";
                grid1_label_count.Header = "수량";
                grid1_label_model.Header = "규격/형번호";
                grid1_label_price.Header = "단가";
                grid1_label_owner.Header = "소유공장";
                grid1_label_pay_state.Header = "지불상황";
                grid2_label_no.Header = "번호";
                grid2_label_chinese_name.Header = "품명(중국어)";
                grid2_label_korean_name.Header = "품명(조선어)";
                grid2_label_place.Header = "배치공정";
                grid2_label_in_date.Header = "입고날자";
                grid2_label_place_date.Header = "배치날자";
                grid2_label_model.Header = "규격/형번호";
                grid2_label_count.Header = "수량";
                grid2_label_price.Header = "단가";
                grid2_label_remark.Header = "비고";
                grid_back_label_no.Header = "번호";
                grid_back_label_place.Header = "퇴송공정";
                grid_back_label_date.Header = "퇴송날자";
                grid_back_label_count.Header = "수량";
                grid_exhaust_label_no.Header = "번호";
                grid_exhaust_label_place.Header = "페기공정";
                grid_exhaust_label_date.Header = "페기날자";
                grid_exhaust_label_count.Header = "수량";
                label_simple_search.Text = "간단검색";
                label_button_search.Content = "검색";
                label_place_modify_move_delete.Text = "배치공정변경/이동/삭제";
                label_modify_move_place_name.Text = "변경/이동할 공정명";
                label_modify_move_date.Text = "변경/이동날자";
                label_modify_move_count.Text = "변경/이동수량";
                label_combo_remark.Text = "비고";
                label_button_modify.Content = "변경";
                label_button_delete.Content = "삭제";
                label_button_move.Content = "이동";
                label_group_back.Text = "퇴송";
                label_group_back_count.Text = "수량";
                label_group_back_date.Text = "퇴송날자";
                label_group_back_button_back.Content = "퇴송";
                label_group_back_button_cancel.Content = "취소";
                label_group_exhaust.Text = "페기";
                label_group_exhaust_count.Text = "수량";
                label_group_exhaust_date.Text = "페기날자";
                label_group_exhaust_button_exhaust.Content = "페기";
                label_group_exhaust_button_cancel.Content = "취소";
                label_label_back_data.Content = "퇴송자료";
                label_label_exhaust_data.Content = "페기자료";
                label_label_place_data.Content = "배치자료";

                btn_set_default_lang.Content = "표준으로 설정";
                label_user_setting.Text = "사용자관리";
                label_username_setting.Content = "사용자이름";
                username_add_button.Content = "추가";
                username_modify_button.Content = "변경";
                username_delete_button.Content = "삭제";
                group_pass_manage.Header = "암호관리";
                label_old_pass.Content = "전암호";
                label_new_pass.Content = "새암호";
                label_retype_new_pass.Content = "새암호";
                btn_pass_change.Content = "변경";

                label_group_search_items.Header = "검색항목";
                label_search_area.Content = "검색범위";
                search_label_chinese_name.Content = "품명(중국어)";
                search_label_korean_name.Content = "품명(조선어)";
                search_label_date.Content = "날자";
                search_label_place.Content = "공정";
                search_label_model.Content = "규격/형번호";
                btn_search.Content = "검색";
                grid_search_label_place_no.Header = "배치번호";
                grid_search_label_chinese_name.Header = "품명(중국어)";
                grid_search_label_korean_name.Header = "품명(조선어)";
                grid_search_label_in_date.Header = "입고날자";
                grid_search_label_place_date.Header = "배치날자";
                grid_search_label_place.Header = "배치공정";
                grid_search_label_place_old.Header = "전공정";
                grid_search_label_place_new.Header = "후공정";
                grid_search_label_model.Header = "규격/형번호";
                grid_search_label_count.Header = "수량";
                grid_search_label_price.Header = "단가";
                grid_search_label_owner.Header = "소유공장";
                grid_search_label_pay_state.Header = "지불상황";
                grid_search_label_remark.Header = "비고";
                grid_search_label_back_count.Header = "퇴송수량";
                grid_search_label_back_date.Header = "퇴송날자";
                grid_search_label_back_place.Header = "퇴송공정";
                grid_search_label_exhaust_count.Header = "페기수량";
                grid_search_label_exhaust_date.Header = "페기날자";
                grid_search_label_exhaust_place.Header = "페기공정";
                grid_search_label_delete_history_date.Header = "삭제날자";
                grid_search_label_delete_history_user.Header = "사용자";

                item1.Content = "입고자료";
                item2.Content = "배치자료";
                item3.Content = "이동자료";
                item4.Content = "퇴송자료";
                item5.Content = "페기자료";
                item6.Content = "삭제기록자료";

            }
        }
        private void korean_Checked(object sender, RoutedEventArgs e)
        {
            common.lang = 1;
            change_language();
        }

        private void chinese_Checked(object sender, RoutedEventArgs e)
        {
            common.lang = 0;
            change_language();
        }
        private void change_check_default_lang_png_visibility()
        {
            if (common.lang == 0)
            {
                check_chinese_png.Visibility = Visibility.Visible;
                check_korean_png.Visibility = Visibility.Hidden;
            }
            else
            {
                check_chinese_png.Visibility = Visibility.Hidden;
                check_korean_png.Visibility = Visibility.Visible;
            }

        }
        private void read_server_setting_from_file()
        {
            if(File.Exists(".\\server.txt"))
            {
                StreamReader sr = new StreamReader(".\\server.txt");
                string ip = sr.ReadLine().Trim();
                string pass = sr.ReadLine().Trim();
                common.connectionString = $"server = {ip}; userid=root;password={pass};database=factory";
            }
        }
        private void read_default_lang_from_file()
        {
            if (File.Exists(".\\lang"))
            {
                StreamReader sr = new StreamReader(".\\lang");
                string str = ((char)sr.Read()).ToString();
                common.lang = Convert.ToInt32(str);
                sr.Close();
            }
            else
                common.lang = 0;
        }
        private void write_default_lang_to_file()
        {
            StreamWriter sw = new StreamWriter(".\\lang");
            string str = common.lang.ToString();
            sw.Write(str);
            sw.Close();
        }
        private void btn_set_default_lang_Click(object sender, RoutedEventArgs e)
        {
            change_check_default_lang_png_visibility();
            write_default_lang_to_file();
        }
        private string make_query_for_asset_search(string chinese_name, string korean_name, string date_from, string date_to, string model)
        {
            string query = "select * from assets_info order by id";
            if (chinese_name == "" && korean_name == "" && date_from == "" && date_to == "" && model == "")
                return query;
            string q1="", q2="", q3="", q4="", q5="";
            bool bPrev = false;
            if(chinese_name != "")
            {
                q1 = $"name like '%{chinese_name}%'";
                bPrev = true;
            }
            if(korean_name != "")
            {
                if(!bPrev)
                    q2 = $"kor_name like '%{korean_name}%'";
                else
                    q2 = $" and kor_name like '%{korean_name}%'";
                bPrev = true;
            }
            if (date_from != "")
            {
                if (!bPrev)
                    q3 = $"in_date >= '{date_from}'";
                else
                    q3 = $" and in_date >= '{date_from}'";
                bPrev = true;
            }
            if (date_to != "")
            {
                if(!bPrev)
                    q4 = $"in_date <= '{date_to}'";
                else
                    q4 = $" and in_date <= '{date_to}'";
                bPrev = true;
            }
            if (model != "")
            {
                if(!bPrev)
                    q5 = $"model like '%{model}%'";
                else
                    q5 = $" and model like '%{model}%'";
            }
            query = "select * from assets_info where " + q1 + q2 + q3 + q4 + q5;
            return query;
        }
        private string make_query_for_place_search(string chinese_name, string korean_name, string date_from, string date_to, string place, string model)
        {
            string query = "select b.id, a.id, b.name, b.kor_name, a.place, b.in_date, a.date, b.model, a.count, b.price, a.remark from placement_info as a, assets_info as b where a.asset_id=b.id and a.count > 0 order by a.asset_id";
            if (chinese_name == "" && korean_name == "" && date_from == "" && date_to == "" && model == "" && place=="")
                return query;
            string q1 = "", q2 = "", q3 = "", q4 = "", q5 = "", q6 = "";
            bool bPrev = true;
            if (chinese_name != "")
            {
                q1 = $" and b.name like '%{chinese_name}%'";
                bPrev = true;
            }
            if (korean_name != "")
            {
                if (!bPrev)
                    q2 = $"b.kor_name like '%{korean_name}%'";
                else
                    q2 = $" and b.kor_name like '%{korean_name}%'";
                bPrev = true;
            }
            if (date_from != "")
            {
                if (!bPrev)
                    q3 = $"a.date >= '{date_from}'";
                else
                    q3 = $" and a.date >= '{date_from}'";
                bPrev = true;
            }
            if (date_to != "")
            {
                if (!bPrev)
                    q4 = $"a.date <= '{date_to}'";
                else
                    q4 = $" and a.date <= '{date_to}'";
                bPrev = true;
            }
            if (model != "")
            {
                if (!bPrev)
                    q5 = $"b.model like '%{model}%'";
                else
                    q5 = $" and b.model like '%{model}%'";
            }
            if(place != "")
            {
                if (!bPrev)
                    q6 = $"a.place like '%{place}%'";
                else
                    q6 = $" and a.place like '%{place}%'";
            }
            query = "select b.id, a.id, b.name, b.kor_name, a.place, b.in_date, a.date, b.model, a.count, b.price, a.remark from placement_info as a, assets_info as b  where  a.asset_id = b.id and a.count > 0" + q1 + q2 + q3 + q4 + q5 + q6 + "order by a.asset_id";
            return query;
        }
        private string make_query_for_movement_search(string chinese_name, string korean_name, string date_from, string date_to, string place, string model)
        {
            string query = "select b.id, c.id, b.name, b.kor_name, d.place AS old_place, e.place AS new_place, b.in_date, c.date, b.model, c.count, b.price from assets_info as b, movement_info AS c, " + 
                        "(SELECT a.id, a.place FROM placement_info AS a, movement_info AS b WHERE a.id = b.old_place) AS d, (SELECT a.id, a.place FROM placement_info AS a, movement_info AS b WHERE a.id = b.new_place) AS e "+
                        "WHERE c.asset_id = b.id AND c.old_place = d.id AND c.new_place = e.id";
            if (chinese_name == "" && korean_name == "" && date_from == "" && date_to == "" && model == "" && place == "")
                return query;
            string q1 = "", q2 = "", q3 = "", q4 = "", q5 = "", q6 = "";
            bool bPrev = true;
            if (chinese_name != "")
            {
                q1 = $" and b.name like '%{chinese_name}%'";
                bPrev = true;
            }
            if (korean_name != "")
            {
                if (!bPrev)
                    q2 = $"b.kor_name like '%{korean_name}%'";
                else
                    q2 = $" and b.kor_name like '%{korean_name}%'";
                bPrev = true;
            }
            if (date_from != "")
            {
                if (!bPrev)
                    q3 = $"c.date >= '{date_from}'";
                else
                    q3 = $" and c.date >= '{date_from}'";
                bPrev = true;
            }
            if (date_to != "")
            {
                if (!bPrev)
                    q4 = $"c.date <= '{date_to}'";
                else
                    q4 = $" and c.date <= '{date_to}'";
                bPrev = true;
            }
            if (model != "")
            {
                if (!bPrev)
                    q5 = $"b.model like '%{model}%'";
                else
                    q5 = $" and b.model like '%{model}%'";
            }
            if (place != "")
            {
                if (!bPrev)
                    q6 = $"(d.place like '%{place}%' or e.place like '%{place}%'";
                else
                    q6 = $" and (d.place like '%{place}%' or e.place like '%{place}%')";
            }
            query = "select b.id, c.id, b.name, b.kor_name, d.place AS old_place, e.place AS new_place, b.in_date, c.date, b.model, c.count, b.price from assets_info as b, movement_info AS c, " +
                        "(SELECT a.id, a.place FROM placement_info AS a, movement_info AS b WHERE a.id = b.old_place) AS d," +
                        "(SELECT a.id, a.place FROM placement_info AS a, movement_info AS b WHERE a.id = b.new_place) AS e " +
                        "WHERE c.asset_id = b.id AND c.old_place = d.id AND c.new_place = e.id" + q1 + q2 + q3 + q4 + q5 + q6;
            return query;
        }
        private string make_query_for_back_search(string chinese_name, string korean_name, string date_from, string date_to, string place, string model)
        {
            string query = "select b.id, c.id, b.name, b.kor_name, b.in_date, b.model, b.price, d.remark, c.count, c.date, d.place from assets_info as b, back_info AS c, " +
                        "(SELECT a.id, a.place, a.remark FROM placement_info AS a, back_info AS b WHERE a.id = b.place) AS d" +
                        " WHERE c.asset_id = b.id AND c.place = d.id";
            if (chinese_name == "" && korean_name == "" && date_from == "" && date_to == "" && model == "" && place == "")
                return query;
            string q1 = "", q2 = "", q3 = "", q4 = "", q5 = "", q6 = "";
            bool bPrev = true;
            if (chinese_name != "")
            {
                q1 = $" and b.name like '%{chinese_name}%'";
                bPrev = true;
            }
            if (korean_name != "")
            {
                if (!bPrev)
                    q2 = $"b.kor_name like '%{korean_name}%'";
                else
                    q2 = $" and b.kor_name like '%{korean_name}%'";
                bPrev = true;
            }
            if (date_from != "")
            {
                if (!bPrev)
                    q3 = $"c.date >= '{date_from}'";
                else
                    q3 = $" and c.date >= '{date_from}'";
                bPrev = true;
            }
            if (date_to != "")
            {
                if (!bPrev)
                    q4 = $"c.date <= '{date_to}'";
                else
                    q4 = $" and c.date <= '{date_to}'";
                bPrev = true;
            }
            if (model != "")
            {
                if (!bPrev)
                    q5 = $"b.model like '%{model}%'";
                else
                    q5 = $" and b.model like '%{model}%'";
            }
            if (place != "")
            {
                if (!bPrev)
                    q6 = $"(d.place like '%{place}%'";
                else
                    q6 = $" and d.place like '%{place}%'";
            }
            query = "select b.id, c.id, b.name, b.kor_name, b.in_date, b.model, b.price, d.remark, c.count, c.date, d.place from assets_info as b, back_info AS c, " +
                        "(SELECT a.id, a.place, a.remark FROM placement_info AS a, back_info AS b WHERE a.id = b.place) AS d" +
                        " WHERE c.asset_id = b.id AND c.place = d.id" + q1 + q2 + q3 + q4 + q5 + q6;
            return query;
        }
        private string make_query_for_exhaust_search(string chinese_name, string korean_name, string date_from, string date_to, string place, string model)
        {
            string query = "select b.id, c.id, b.name, b.kor_name, b.in_date, b.model, b.price, d.remark, c.count, c.date, d.place from assets_info as b, exhaust_info AS c, " +
                        "(SELECT a.id, a.place, a.remark FROM placement_info AS a, exhaust_info AS b WHERE a.id = b.place) AS d" +
                        " WHERE c.asset_id = b.id AND c.place = d.id";
            if (chinese_name == "" && korean_name == "" && date_from == "" && date_to == "" && model == "" && place == "")
                return query;
            string q1 = "", q2 = "", q3 = "", q4 = "", q5 = "", q6 = "";
            bool bPrev = true;
            if (chinese_name != "")
            {
                q1 = $" and b.name like '%{chinese_name}%'";
                bPrev = true;
            }
            if (korean_name != "")
            {
                if (!bPrev)
                    q2 = $"b.kor_name like '%{korean_name}%'";
                else
                    q2 = $" and b.kor_name like '%{korean_name}%'";
                bPrev = true;
            }
            if (date_from != "")
            {
                if (!bPrev)
                    q3 = $"c.date >= '{date_from}'";
                else
                    q3 = $" and c.date >= '{date_from}'";
                bPrev = true;
            }
            if (date_to != "")
            {
                if (!bPrev)
                    q4 = $"c.date <= '{date_to}'";
                else
                    q4 = $" and c.date <= '{date_to}'";
                bPrev = true;
            }
            if (model != "")
            {
                if (!bPrev)
                    q5 = $"b.model like '%{model}%'";
                else
                    q5 = $" and b.model like '%{model}%'";
            }
            if (place != "")
            {
                if (!bPrev)
                    q6 = $"(d.place like '%{place}%'";
                else
                    q6 = $" and d.place like '%{place}%'";
            }
            query = "select b.id, c.id, b.name, b.kor_name, b.in_date, b.model, b.price, d.remark, c.count, c.date, d.place from assets_info as b, exhaust_info AS c, " +
                        "(SELECT a.id, a.place, a.remark FROM placement_info AS a, exhaust_info AS b WHERE a.id = b.place) AS d" +
                        " WHERE c.asset_id = b.id AND c.place = d.id" + q1 + q2 + q3 + q4 + q5 + q6;
            return query;
        }
        private void search_assets_info()
        {
            dataGrid_search.Items.Clear();
            string chinese_name = search_text_chinese_name.Text.Trim();
            string korean_name = search_text_korean_name.Text.Trim();
            DateTime? selectedDateFrom = search_date_from.SelectedDate;
            string date_from = "";
            if (selectedDateFrom.HasValue)
            {
                date_from = selectedDateFrom.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            DateTime? selectedDateTo = search_date_to.SelectedDate;
            string date_to = "";
            if (selectedDateTo.HasValue)
            {
                date_to = selectedDateTo.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            string model = search_text_model.Text.Trim();
            string sql = make_query_for_asset_search(chinese_name, korean_name, date_from, date_to, model);
            var cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string chi_name = reader.GetString(1);
                string kor_name = reader.GetString(2);
                DateTime in_date = reader.GetDateTime(3);
                string str_in_date = $"{in_date.Year.ToString("D4")}-{in_date.Month.ToString("D2")}-{in_date.Day.ToString("D2")}";
                int count = reader.GetInt32(4);
                string mod = reader.GetString(5);
                double price = reader.GetDouble(6);
                string owner = reader.GetString(7);
                bool pay_state = reader.GetBoolean(8);
                dataGrid_search.Items.Add(new AssetInfo(id, chi_name, kor_name, str_in_date, count, price, owner, mod, pay_state));
            }
            reader.Close();
            dataGrid_search.Items.Refresh();
        }
        private void search_place_info()
        {
            dataGrid_search.Items.Clear();
            string chinese_name = search_text_chinese_name.Text.Trim();
            string korean_name = search_text_korean_name.Text.Trim();
            string place = search_text_place.Text.Trim();
            DateTime? selectedDateFrom = search_date_from.SelectedDate;
            string date_from = "";
            if (selectedDateFrom.HasValue)
            {
                date_from = selectedDateFrom.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            DateTime? selectedDateTo = search_date_to.SelectedDate;
            string date_to = "";
            if (selectedDateTo.HasValue)
            {
                date_to = selectedDateTo.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            string model = search_text_model.Text.Trim();
            string sql = make_query_for_place_search(chinese_name, korean_name, date_from, date_to, place, model);
            var cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int asset_id = reader.GetInt32(0);
                int id = reader.GetInt32(1);
                int place_id = id;
                string chi_name = reader.GetString(2);
                string kor_name = reader.GetString(3);
                string plac = reader.GetString(4);
                DateTime in_date = reader.GetDateTime(5);
                string str_in_date = $"{in_date.Year.ToString("D4")}-{in_date.Month.ToString("D2")}-{in_date.Day.ToString("D2")}";
                DateTime date = reader.GetDateTime(6);
                string str_date = $"{date.Year.ToString("D4")}-{date.Month.ToString("D2")}-{date.Day.ToString("D2")}";
                string mod = reader.GetString(7);
                int count = reader.GetInt32(8);
                double price = reader.GetDouble(9);
                string remark = reader.GetString(10);
                dataGrid_search.Items.Add(new SearchPlacementInfo(asset_id, place_id, chi_name, kor_name, plac, str_in_date, str_date, mod, count, price, remark));
            }
            reader.Close();
            dataGrid_search.Items.Refresh();
        }
        private void search_movement_info()
        {

            dataGrid_search.Items.Clear();
            string chinese_name = search_text_chinese_name.Text.Trim();
            string korean_name = search_text_korean_name.Text.Trim();
            string place = search_text_place.Text.Trim();
            DateTime? selectedDateFrom = search_date_from.SelectedDate;
            string date_from = "";
            if (selectedDateFrom.HasValue)
            {
                date_from = selectedDateFrom.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            DateTime? selectedDateTo = search_date_to.SelectedDate;
            string date_to = "";
            if (selectedDateTo.HasValue)
            {
                date_to = selectedDateTo.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            string model = search_text_model.Text.Trim();
            string sql = make_query_for_movement_search(chinese_name, korean_name, date_from, date_to, place, model);
            var cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int asset_id = reader.GetInt32(0);
                int id = reader.GetInt32(1);
                int place_id = id;
                string chi_name = reader.GetString(2);
                string kor_name = reader.GetString(3);
                string place_old = reader.GetString(4);
                string place_new = reader.GetString(5);
                DateTime in_date = reader.GetDateTime(6);
                string str_in_date = $"{in_date.Year.ToString("D4")}-{in_date.Month.ToString("D2")}-{in_date.Day.ToString("D2")}";
                DateTime date = reader.GetDateTime(7);
                string str_date = $"{date.Year.ToString("D4")}-{date.Month.ToString("D2")}-{date.Day.ToString("D2")}";
                string mod = reader.GetString(8);
                int count = reader.GetInt32(9);
                double price = reader.GetDouble(10);
                dataGrid_search.Items.Add(new SearchMovementInfo(asset_id, place_id, chi_name, kor_name, place_old, place_new, str_in_date, str_date, mod, count, price));
            }
            reader.Close();
            dataGrid_search.Items.Refresh();
        }
        private void search_back_info()
        {
            dataGrid_search.Items.Clear();
            string chinese_name = search_text_chinese_name.Text.Trim();
            string korean_name = search_text_korean_name.Text.Trim();
            string place = search_text_place.Text.Trim();
            DateTime? selectedDateFrom = search_date_from.SelectedDate;
            string date_from = "";
            if (selectedDateFrom.HasValue)
            {
                date_from = selectedDateFrom.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            DateTime? selectedDateTo = search_date_to.SelectedDate;
            string date_to = "";
            if (selectedDateTo.HasValue)
            {
                date_to = selectedDateTo.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            string model = search_text_model.Text.Trim();
            string sql = make_query_for_back_search(chinese_name, korean_name, date_from, date_to, place, model);
            var cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int asset_id = reader.GetInt32(0);
                int id = reader.GetInt32(1);
                int place_id = id;
                string chi_name = reader.GetString(2);
                string kor_name = reader.GetString(3);
                DateTime in_date = reader.GetDateTime(4);
                string str_in_date = $"{in_date.Year.ToString("D4")}-{in_date.Month.ToString("D2")}-{in_date.Day.ToString("D2")}";
                string mod = reader.GetString(5);
                double price = reader.GetDouble(6);
                string remark = reader.GetString(7);
                int count = reader.GetInt32(8);
                DateTime date = reader.GetDateTime(9);
                string str_back_date = $"{date.Year.ToString("D4")}-{date.Month.ToString("D2")}-{date.Day.ToString("D2")}";
                string back_place = reader.GetString(10);
                dataGrid_search.Items.Add(new SearchBackInfo(asset_id, place_id, chi_name, kor_name, str_in_date, mod, price, remark, count, str_back_date, back_place));
            }
            reader.Close();
            dataGrid_search.Items.Refresh();
        }
        private void search_exhaust_info()
        {
            dataGrid_search.Items.Clear();
            string chinese_name = search_text_chinese_name.Text.Trim();
            string korean_name = search_text_korean_name.Text.Trim();
            string place = search_text_place.Text.Trim();
            DateTime? selectedDateFrom = search_date_from.SelectedDate;
            string date_from = "";
            if (selectedDateFrom.HasValue)
            {
                date_from = selectedDateFrom.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            DateTime? selectedDateTo = search_date_to.SelectedDate;
            string date_to = "";
            if (selectedDateTo.HasValue)
            {
                date_to = selectedDateTo.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            string model = search_text_model.Text.Trim();
            string sql = make_query_for_exhaust_search(chinese_name, korean_name, date_from, date_to, place, model);
            var cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int asset_id = reader.GetInt32(0);
                int id = reader.GetInt32(1);
                int place_id = id;
                string chi_name = reader.GetString(2);
                string kor_name = reader.GetString(3);
                DateTime in_date = reader.GetDateTime(4);
                string str_in_date = $"{in_date.Year.ToString("D4")}-{in_date.Month.ToString("D2")}-{in_date.Day.ToString("D2")}";
                string mod = reader.GetString(5);
                double price = reader.GetDouble(6);
                string remark = reader.GetString(7);
                int count = reader.GetInt32(8);
                DateTime date = reader.GetDateTime(9);
                string str_back_date = $"{date.Year.ToString("D4")}-{date.Month.ToString("D2")}-{date.Day.ToString("D2")}";
                string back_place = reader.GetString(10);
                dataGrid_search.Items.Add(new SearchExhaustInfo(asset_id, place_id, chi_name, kor_name, str_in_date, mod, price, remark, count, str_back_date, back_place));
            }
            reader.Close();
            dataGrid_search.Items.Refresh();
        }
        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            int idx = search_area_combo.SelectedIndex;
            if (idx == 0)
                search_assets_info();
            else if (idx == 1)
                search_place_info();
            else if (idx == 2)
                search_movement_info();
            else if (idx == 3)
                search_back_info();
            else if (idx == 4)
                search_exhaust_info();
            else if (idx == 5)
                search_deleted_history_info();
        }

        private void search_area_combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (search_area_combo.SelectedIndex == 0)
            {
                grid_search_label_no.Visibility = Visibility.Visible;
                grid_search_label_place_no.Visibility = Visibility.Hidden;
                grid_search_label_place_date.Visibility = Visibility.Hidden;
                grid_search_label_place.Visibility = Visibility.Hidden;
                grid_search_label_place_old.Visibility = Visibility.Hidden;
                grid_search_label_count.Visibility = Visibility.Visible;
                grid_search_label_place_new.Visibility = Visibility.Hidden;
                grid_search_label_owner.Visibility = Visibility.Visible;
                grid_search_label_pay_state.Visibility = Visibility.Visible;
                grid_search_label_remark.Visibility = Visibility.Hidden;
                grid_search_label_back_place.Visibility = Visibility.Hidden;
                grid_search_label_back_date.Visibility = Visibility.Hidden;
                grid_search_label_back_count.Visibility = Visibility.Hidden;
                grid_search_label_exhaust_place.Visibility = Visibility.Hidden;
                grid_search_label_exhaust_date.Visibility = Visibility.Hidden;
                grid_search_label_exhaust_count.Visibility = Visibility.Hidden;
                search_label_place.Visibility = Visibility.Hidden;
                search_text_place.Visibility = Visibility.Hidden;
                grid_search_label_delete_history_date.Visibility = Visibility.Hidden;
                grid_search_label_delete_history_user.Visibility = Visibility.Hidden;
                search_assets_info();
                return;
            }
            if (search_area_combo.SelectedIndex == 5)
            {
                grid_search_label_no.Visibility = Visibility.Visible;
                grid_search_label_delete_history_date.Visibility = Visibility.Visible;
                grid_search_label_delete_history_user.Visibility = Visibility.Visible;
                grid_search_label_place_no.Visibility = Visibility.Hidden;
                grid_search_label_place_date.Visibility = Visibility.Hidden;
                grid_search_label_place.Visibility = Visibility.Hidden;
                grid_search_label_place_old.Visibility = Visibility.Hidden;
                grid_search_label_count.Visibility = Visibility.Visible;
                grid_search_label_place_new.Visibility = Visibility.Hidden;
                grid_search_label_owner.Visibility = Visibility.Visible;
                grid_search_label_pay_state.Visibility = Visibility.Hidden;
                grid_search_label_remark.Visibility = Visibility.Hidden;
                grid_search_label_back_place.Visibility = Visibility.Hidden;
                grid_search_label_back_date.Visibility = Visibility.Hidden;
                grid_search_label_back_count.Visibility = Visibility.Hidden;
                grid_search_label_exhaust_place.Visibility = Visibility.Hidden;
                grid_search_label_exhaust_date.Visibility = Visibility.Hidden;
                grid_search_label_exhaust_count.Visibility = Visibility.Hidden;
                grid_search_label_delete_history_date.Visibility = Visibility.Visible;
                grid_search_label_delete_history_user.Visibility = Visibility.Visible;
                search_label_place.Visibility = Visibility.Visible;
                search_text_place.Visibility = Visibility.Visible;
                if (common.lang == 0)
                    search_label_place.Content = "用户名";
                else
                    search_label_place.Content = "사용자명";
                search_deleted_history_info();
                return;
            }
            else if (search_area_combo.SelectedIndex == 1)
            {
                grid_search_label_no.Visibility = Visibility.Visible;
                grid_search_label_place_no.Visibility = Visibility.Visible;
                grid_search_label_place_date.Visibility = Visibility.Visible;
                grid_search_label_place.Visibility = Visibility.Visible;
                grid_search_label_place_old.Visibility = Visibility.Hidden;
                grid_search_label_place_new.Visibility = Visibility.Hidden;
                grid_search_label_count.Visibility = Visibility.Visible;
                grid_search_label_owner.Visibility = Visibility.Hidden;
                grid_search_label_pay_state.Visibility = Visibility.Hidden;
                grid_search_label_remark.Visibility = Visibility.Visible;
                grid_search_label_back_place.Visibility = Visibility.Hidden;
                grid_search_label_back_date.Visibility = Visibility.Hidden;
                grid_search_label_back_count.Visibility = Visibility.Hidden;
                grid_search_label_exhaust_place.Visibility = Visibility.Hidden;
                grid_search_label_exhaust_date.Visibility = Visibility.Hidden;
                grid_search_label_exhaust_count.Visibility = Visibility.Hidden;
                grid_search_label_delete_history_date.Visibility = Visibility.Hidden;
                grid_search_label_delete_history_user.Visibility = Visibility.Hidden;
                search_place_info();
            }
            else if (search_area_combo.SelectedIndex == 2)
            {
                if(common.lang==0)
                    grid_search_label_place_date.Header = "移动日期";
                if (common.lang == 1)
                    grid_search_label_place_date.Header = "이동날자";
                grid_search_label_no.Visibility = Visibility.Visible;
                grid_search_label_place_no.Visibility = Visibility.Hidden;
                grid_search_label_place_date.Visibility = Visibility.Visible;
                grid_search_label_place.Visibility = Visibility.Hidden;
                grid_search_label_place_old.Visibility = Visibility.Visible;
                grid_search_label_place_new.Visibility = Visibility.Visible;
                grid_search_label_count.Visibility = Visibility.Visible;
                grid_search_label_owner.Visibility = Visibility.Hidden;
                grid_search_label_pay_state.Visibility = Visibility.Hidden;
                grid_search_label_remark.Visibility = Visibility.Hidden;
                grid_search_label_back_place.Visibility = Visibility.Hidden;
                grid_search_label_back_date.Visibility = Visibility.Hidden;
                grid_search_label_back_count.Visibility = Visibility.Hidden;
                grid_search_label_exhaust_place.Visibility = Visibility.Hidden;
                grid_search_label_exhaust_date.Visibility = Visibility.Hidden;
                grid_search_label_exhaust_count.Visibility = Visibility.Hidden;
                grid_search_label_delete_history_date.Visibility = Visibility.Hidden;
                grid_search_label_delete_history_user.Visibility = Visibility.Hidden;
                search_movement_info();
            }
            else if (search_area_combo.SelectedIndex == 3)
            {
                grid_search_label_no.Visibility = Visibility.Visible;
                grid_search_label_place_no.Visibility = Visibility.Hidden;
                grid_search_label_place_date.Visibility = Visibility.Hidden;
                grid_search_label_place.Visibility = Visibility.Hidden;
                grid_search_label_place_old.Visibility = Visibility.Hidden;
                grid_search_label_place_new.Visibility = Visibility.Hidden;
                grid_search_label_count.Visibility = Visibility.Hidden;
                grid_search_label_owner.Visibility = Visibility.Hidden;
                grid_search_label_pay_state.Visibility = Visibility.Hidden;
                grid_search_label_remark.Visibility = Visibility.Visible;
                grid_search_label_back_place.Visibility = Visibility.Visible;
                grid_search_label_back_date.Visibility = Visibility.Visible;
                grid_search_label_back_count.Visibility = Visibility.Visible;
                grid_search_label_exhaust_place.Visibility = Visibility.Hidden;
                grid_search_label_exhaust_date.Visibility = Visibility.Hidden;
                grid_search_label_exhaust_count.Visibility = Visibility.Hidden;
                grid_search_label_delete_history_date.Visibility = Visibility.Hidden;
                grid_search_label_delete_history_user.Visibility = Visibility.Hidden;
                search_back_info();
            }
            else if (search_area_combo.SelectedIndex == 4)
            {
                grid_search_label_no.Visibility = Visibility.Visible;
                grid_search_label_place_no.Visibility = Visibility.Hidden;
                grid_search_label_place_date.Visibility = Visibility.Hidden;
                grid_search_label_place.Visibility = Visibility.Hidden;
                grid_search_label_place_old.Visibility = Visibility.Hidden;
                grid_search_label_count.Visibility = Visibility.Hidden;
                grid_search_label_place_new.Visibility = Visibility.Hidden;
                grid_search_label_owner.Visibility = Visibility.Hidden;
                grid_search_label_pay_state.Visibility = Visibility.Hidden;
                grid_search_label_remark.Visibility = Visibility.Visible;
                grid_search_label_back_place.Visibility = Visibility.Hidden;
                grid_search_label_back_date.Visibility = Visibility.Hidden;
                grid_search_label_back_count.Visibility = Visibility.Hidden;
                grid_search_label_exhaust_place.Visibility = Visibility.Visible;
                grid_search_label_exhaust_date.Visibility = Visibility.Visible;
                grid_search_label_exhaust_count.Visibility = Visibility.Visible;
                grid_search_label_delete_history_date.Visibility = Visibility.Hidden;
                grid_search_label_delete_history_user.Visibility = Visibility.Hidden;
                search_exhaust_info();
            }
            search_label_place.Visibility = Visibility.Visible;
            search_text_place.Visibility = Visibility.Visible;
        }

        private void search_deleted_history_info()
        {
            dataGrid_search.Items.Clear();
            string chinese_name = search_text_chinese_name.Text.Trim();
            string korean_name = search_text_korean_name.Text.Trim();
            DateTime? selectedDateFrom = search_date_from.SelectedDate;
            string date_from = "";
            if (selectedDateFrom.HasValue)
            {
                date_from = selectedDateFrom.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            DateTime? selectedDateTo = search_date_to.SelectedDate;
            string date_to = "";
            if (selectedDateTo.HasValue)
            {
                date_to = selectedDateTo.Value.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            string model = search_text_model.Text.Trim();
            string user = search_text_place.Text.Trim();
            string sql = make_query_for_delete_history_search(chinese_name, korean_name, date_from, date_to, model, user);
            var cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string chi_name = reader.GetString(1);
                string kor_name = reader.GetString(2);
                DateTime in_date = reader.GetDateTime(3);
                string str_in_date = $"{in_date.Year.ToString("D4")}-{in_date.Month.ToString("D2")}-{in_date.Day.ToString("D2")}";
                int count = reader.GetInt32(4);
                string mod = reader.GetString(5);
                double price = reader.GetDouble(6);
                string owner = reader.GetString(7);
                DateTime delete_date = reader.GetDateTime(8);
                string str_delete_date = $"{delete_date.Year.ToString("D4")}-{delete_date.Month.ToString("D2")}-{delete_date.Day.ToString("D2")}";
                string deleted_user = reader.GetString(9);
                dataGrid_search.Items.Add(new DeletedAssetInfo(id, chi_name, kor_name, str_in_date, count, price, owner, mod, str_delete_date, deleted_user));
            }
            reader.Close();
            dataGrid_search.Items.Refresh();
        }

        private string make_query_for_delete_history_search(string chinese_name, string korean_name, string date_from, string date_to, string model, string user)
        {
            string query = "select * from delete_history_info order by id";
            if (chinese_name == "" && korean_name == "" && date_from == "" && date_to == "" && model == "")
                return query;
            string q1 = "", q2 = "", q3 = "", q4 = "", q5 = "";
            bool bPrev = false;
            if (chinese_name != "")
            {
                q1 = $"name like '%{chinese_name}%'";
                bPrev = true;
            }
            if (korean_name != "")
            {
                if (!bPrev)
                    q2 = $"kor_name like '%{korean_name}%'";
                else
                    q2 = $" and kor_name like '%{korean_name}%'";
                bPrev = true;
            }
            if (date_from != "")
            {
                if (!bPrev)
                    q3 = $"delete_date >= '{date_from}'";
                else
                    q3 = $" and delete_date >= '{date_from}'";
                bPrev = true;
            }
            if (date_to != "")
            {
                if (!bPrev)
                    q4 = $"delete_date <= '{date_to}'";
                else
                    q4 = $" and delete_date <= '{date_to}'";
                bPrev = true;
            }
            if (model != "")
            {
                if (!bPrev)
                    q5 = $"model like '%{model}%'";
                else
                    q5 = $" and model like '%{model}%'";
            }
            if (user != "")
            {
                if (!bPrev)
                    q5 = $"user like '%{user}%'";
                else
                    q5 = $" and user like '%{user}%'";
            }
            query = "select * from delete_history_info where " + q1 + q2 + q3 + q4 + q5;
            return query;
        }

        private void username_add_button_Click(object sender, RoutedEventArgs e)
        {
            string name = username_text_setting.Text;
            if(name=="")
            {
                common.show_message("请填写用户名。", "사용자명을 입력하십시오.");
                return;
            }
            else
            {
                if (DatabaseConnection.check_user(name))
                {
                    common.show_message("用户已存在。", "같은 이름의 사용자가 이미 존재합니다.");
                    return;
                }
            }
            int new_id = DatabaseConnection.get_last_id_from_table("user_info") + 1;
            string query = "insert into user_info (id, username, password) values(@id, @username, @password)";
            var cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", new_id);
            cmd.Parameters.AddWithValue("@username", name);
            cmd.Parameters.AddWithValue("@password", "");
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            username_list.Items.Add(name);
        }

        private void username_modify_button_Click(object sender, RoutedEventArgs e)
        {
            string name = username_text_setting.Text;
            if (name == "")
            {
                common.show_message("请填写用户名。", "사용자명을 입력하십시오.");
                return;
            }
            if(username_list.SelectedIndex == -1)
            {
                common.show_message("请选择用户名。", "변경하려는 사용자이름을 선택하십시오.");
                return;
            }
            string selected_name = username_list.Items[username_list.SelectedIndex] as string;
            DatabaseConnection.modify_user(selected_name, name);
            username_list.Items[username_list.SelectedIndex] = name;
            username_list.Items.Refresh();
        }

        private void username_delete_button_Click(object sender, RoutedEventArgs e)
        {
            if (username_list.SelectedIndex == -1)
            {
                common.show_message("请选择用户名。", "변경하려는 사용자이름을 선택하십시오.");
                return;
            }
            string selected_name = username_list.Items[username_list.SelectedIndex] as string;
            if(DatabaseConnection.check_user(selected_name))
            {
                DatabaseConnection.delete_user(selected_name);
                username_list.Items.RemoveAt(username_list.SelectedIndex);
                username_list.Items.Refresh();
            }
        }

        private void btn_pass_change_Click(object sender, RoutedEventArgs e)
        {
            string old_pass = this.old_pass.Password;
            string new_pass = this.new_pass.Password;
            string re_new_pass = this.retype_new_pass.Password;
            if(DatabaseConnection.check_user_pass(common.username, old_pass)==false)
            {
                common.show_message("旧密码不正确。", "이전 암호가 맞지 않습니다.");
                return;
            }
            if(new_pass != re_new_pass)
            {
                common.show_message("新密码不匹配。", "새 암호가 일치하지 않습니다.");
                return;
            }
            DatabaseConnection.set_pass(common.username, new_pass);
            common.show_message("变更成功。", "성공적으로 변경되였습니다.");
        }

        private void add_excel_Click(object sender, RoutedEventArgs e)
        {
            String filename = excelDataReader.openExcelFile();
            if(File.Exists(filename))
                excelDataReader.readExcelData(dataGrid1, filename);
        }

        private void pay_yes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void pay_no_Click(object sender, RoutedEventArgs e)
        {

        }

        private void username_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(common.username=="admin" && username_list.SelectedIndex>=0)
            {
                string selected_name = username_list.Items[username_list.SelectedIndex] as string;
                string sql = $"select password from user_info where username='{selected_name}'";
                var cmd = new MySqlCommand(sql, con);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string pass = reader.GetString(0);
                    username_text_setting.Text = "password:" + pass;
                }
                reader.Close();
            }
        }
    }
}
