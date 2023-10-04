using Npgsql;
using NpgsqlTypes;
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

namespace SimpleDB
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
            DBControl.Connect("localhost", "5432", "postgres", "1488", "SimpleDB");

            Binding Binding = new Binding();
            Binding.Source = DBControl.contents;
            lvContent.ItemsSource = DBControl.contents;
            LoadContent();

            //if (lvContent.SelectedIndex == -1)
            //{
            //    UpdateButton.IsEnabled = false;
            //    DeleteButton.IsEnabled = false;
            //}
        }

        private void LoadContent()
        {
            DBControl.contents.Clear();
            NpgsqlCommand command = DBControl.GetCommand("SELECT id, name FROM \"tab\" ORDER BY id");
            NpgsqlDataReader result = command.ExecuteReader();
            if (result.HasRows)
            {
                while
                 (result.Read())
                {
                    DBControl.contents.Add(new Contents(result.GetInt32(0), result.GetString(1)));
                }
            }
            result.Close();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            string newName = tbName.Text.Trim();
            if (newName.Length == 0)
                return;

            NpgsqlCommand command = DBControl.GetCommand("INSERT INTO \"tab\"(name) VALUES(@name)");
            try
            {
                command.Parameters.AddWithValue("@name", NpgsqlDbType.Varchar, newName);
                int result = command.ExecuteNonQuery();
                if (result == 1)
                {
                    MessageBox.Show("Successful creation!");
                    LoadContent();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Something is wrong...");
            }

            tbName.Clear();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            int itsId = (lvContent.SelectedItem as Contents).Id;
            string newName = tbName.Text.Trim();
            if (newName.Length == 0 || lvContent.SelectedIndex == -1)
                return;

            NpgsqlCommand command = DBControl.GetCommand("UPDATE \"tab\" SET name=@name WHERE id=@id");
            try
            {
                command.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, itsId);
                command.Parameters.AddWithValue("@name", NpgsqlDbType.Varchar, newName);
                int result = command.ExecuteNonQuery();
                if (result == 1)
                {
                    MessageBox.Show("Successful update!");
                    LoadContent();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Something is wrong...");
            }

            tbName.Clear();
            lvContent.SelectedItem = null;
            tbName.Clear();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Contents mark_for_delete = lvContent.SelectedItem as Contents;
            NpgsqlCommand command = DBControl.GetCommand("DELETE FROM \"tab\" WHERE id = @id");
            try
            {
                command.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, mark_for_delete.Id);
                command.Parameters.AddWithValue("@name", NpgsqlDbType.Varchar, mark_for_delete.Name);
                int result = command.ExecuteNonQuery();
                if (result == 1)
                {
                    DBControl.contents.Remove(lvContent.SelectedItem as Contents);
                    MessageBox.Show("Successful deletion!");
                    LoadContent();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Something is wrong...");
            }

            lvContent.SelectedItem = null;
            tbName.Clear();
        }

        private void lvContent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButton.IsEnabled = lvContent.SelectedIndex != -1;
            DeleteButton.IsEnabled = lvContent.SelectedIndex != -1;
            CancelButton.IsEnabled = lvContent.SelectedIndex != -1;

            if(lvContent.SelectedItem != null)
            {
                tbName.Text = (lvContent.SelectedItem as Contents).Name.ToString();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            lvContent.SelectedItem = null;
            tbName.Clear();
        }
    }
}
