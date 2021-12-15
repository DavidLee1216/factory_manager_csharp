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
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class login : Window
    {
        public MySqlConnection con;
        public bool closed = true;
        public login()
        {
            InitializeComponent();
            change_label_strings();
            con = DatabaseConnection.getDBConnection();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            common.user_mode = true;
            closed = false;
            string uname = username.Text;
            string pass = passwd.Password;
            if(uname=="")
            {
                common.show_message("请填写用户名。", "사용자이름을 입력하십시오.");
                return;
            }
            //             if (pass == "")
            //             {
            //                 common.show_message("请填写密码。", "암호를 입력하십시오.");
            //                 return;
            //             }
            if (DatabaseConnection.check_user_pass(uname, pass))
            {
                common.username = uname;
                this.Close();
            }
            else
            {
                common.show_message("用户不存在或密码错误。", "사용자가 존재하지 않거나 암호가 틀립니다.");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            common.user_mode = false;
            common.username = "";
            closed = false;
            this.Close();
        }
        public void change_label_strings()
        {
            if (common.lang == 0)
            {
                this.Title = "用户登录";
                label_username.Content = "用户名";
                label_passrd.Content = "密码";
                Login.Content = "用户登录";
                Cancel.Content = "非用户订阅";
            }
            else
            {
                this.Title = "사용자가입";
                label_username.Content = "사용자이름";
                label_passrd.Content = "암호";
                Login.Content = "사용자등록";
                Cancel.Content = "비사용자열람";
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }
    }
}
