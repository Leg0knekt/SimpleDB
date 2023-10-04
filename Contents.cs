using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDB
{
    public class Contents
    {
        public Contents(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public static ObservableCollection<Contents> mark_for_delete { get; set; } = new ObservableCollection<Contents>();
    }
}
