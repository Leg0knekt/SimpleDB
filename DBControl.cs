using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDB
{
    public class DBControl
    {
        private static NpgsqlConnection Connection;
        public static void Connect(string host, string port, string user, string pass, string database)
        {
            string cs = string.Format("Server={0}; Port={1}; User Id={2}; Password={3}; Database={4}", host, port, user, pass, database);

            Connection = new NpgsqlConnection(cs);
            Connection.Open();
        }
        public static ObservableCollection<Contents> contents { get; set; } = new ObservableCollection<Contents>();
        public static NpgsqlCommand GetCommand(string sql)
        {
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = Connection;
            command.CommandText = sql;
            return command;
        }
    }
}
