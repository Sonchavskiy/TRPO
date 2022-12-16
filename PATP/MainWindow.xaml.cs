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
using Microsoft.Data.Sqlite;

namespace PATP
{
    public class Person
    {
        public Int64 ID { get; set; }
        public string Code { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronym { get; set; }
        public string BirthDate { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Button_Click(null, null);
        }
        public static T ConvertFromDBVal<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default(T); // returns the default value for the type
            }
            else
            {
                return (T)obj;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var connection = new SqliteConnection("Data Source=DB_TRPO.db"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand("select p.ID, Short_name as code, Last_name as Surname, First_name as Name, Patronym, Birth_date as BirthDate, Phone_number as Phone, d.name\r\nfrom Person p\r\nleft join Department d \r\non p.department_id = d.ID", connection);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    List<Person> personList = new List<Person>();

                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read())   // построчно считываем данныe
                        {

                            var person = new Person
                            {
                                ID = ConvertFromDBVal<Int64>(reader.GetValue(0)),
                                Code = ConvertFromDBVal<string>(reader.GetValue(1)),
                                Surname = ConvertFromDBVal<string>(reader.GetValue(2)),
                                Name = ConvertFromDBVal<string>(reader.GetValue(3)),
                                Patronym = ConvertFromDBVal<string>(reader.GetValue(4)),
                                BirthDate = ConvertFromDBVal<string>(reader.GetValue(5)),
                                Phone = ConvertFromDBVal<string>(reader.GetValue(6)),
                                Department = ConvertFromDBVal<string>(reader.GetValue(7)),
                            };
                            personList.Add(person);
                        }
                    }
                    grid.ItemsSource = personList;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var dialog = new AddWindow();
            if (dialog.ShowDialog() == true)
            {
                Button_Click(null, null);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (grid.SelectedItem != null)
            {
                var del_ID = ((Person)grid.SelectedItem).ID;
                using (var connection = new SqliteConnection("Data Source=DB_TRPO.db"))
                {
                    connection.Open();

                    SqliteCommand command = new SqliteCommand($"delete from Person where id = {del_ID};", connection);

                    int number = command.ExecuteNonQuery();

                    MessageBox.Show($"Удалено объектов: {number}");
                }
                Button_Click(null, null);
            }
        }

        private void grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grid.SelectedIndex == -1)
            {
                btn2.IsEnabled = false;
            }
            else
            {
                btn2.IsEnabled = true;
            }
        }
    }
}
