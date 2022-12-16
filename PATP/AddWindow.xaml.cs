using Microsoft.Data.Sqlite;
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

namespace PATP
{
    /// <summary>
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public AddWindow()
        {
            InitializeComponent();
            using (var connection = new SqliteConnection("Data Source=DB_TRPO.db"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand("select * from Department", connection);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read())   // построчно считываем данныe
                        {
                            department.SelectedValuePath = "Key";
                            department.DisplayMemberPath = "Value";
                            department.Items.Add(new KeyValuePair<Int64, string>((Int64)reader.GetValue(0), (string)reader.GetValue(1)));
                        }
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) //add
        {
            if (last_name.Text == "")
                MessageBox.Show("Фамилия не может быть пустой");
            else if (first_name.Text == "")
                MessageBox.Show("Имя не может быть пустым");
            else if (code.Text == "")
                MessageBox.Show("Код не может быть пустым");
            else
            {
                try
                {
                    using (var connection = new SqliteConnection("Data Source=DB_TRPO.db"))
                    {
                        connection.Open();
                        var dep_id = "NULL";
                        if (department.SelectedValue is not null)
                        {
                            dep_id = department.SelectedValue.ToString();
                        }
                        SqliteCommand command = new SqliteCommand($"insert into Person(Short_name,Last_name,First_name,Patronym,Birth_date,Phone_number,Department_id) values(\"{code.Text}\",\"{last_name.Text}\",\"{first_name.Text}\",\"{patronym.Text}\",\"{birth_date.Text}\",\"{phone.Text}\",{dep_id});", connection);

                        int number = command.ExecuteNonQuery();

                        MessageBox.Show($"Добавлено объектов: {number}");
                    }
                    this.DialogResult = true;
                }
                catch(Microsoft.Data.Sqlite.SqliteException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) //cancel
        {
            //MessageBox.Show(department.SelectedValue.ToString());
            this.DialogResult = false;
        }
    }
}
