using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;
using factory_manager.Properties;
using System.Windows;

namespace factory_manager
{
    public static class common
    {
        public static int lang = 0;
        public static bool user_mode = true;
        public static string username = "";
        public static string connectionString = "server = localhost; userid=root;password=;database=factory";
        public static bool check_double_number(string str)
        {
            if (Regex.IsMatch(str, @"^[0-9]+[.]?[0-9]*$"))
                return true;
            else
                return false;

        }
        public static bool check_int_number(string str)
        {
            if (Regex.IsMatch(str, @"^[0-9]+$"))
                return true;
            else
                return false;
        }
        public static void show_message(string chinese, string korean)
        {
            if (lang == 0)
            {
                MessageBox.Show(chinese);
            }
            else if (lang == 1)
                MessageBox.Show(korean);
        }
        public static bool show_message_yesno(string chinese, string korean)
        {
            if (lang == 0)
            {
                if (MessageBox.Show(chinese, "警告", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    return true;
            }
            else if (lang == 1)
                if (MessageBox.Show(korean, "경고", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    return true;
            return false;
        }
    }
}
