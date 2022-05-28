using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalcticCraftLauncher
{
    public class Settings
    {
        public string connectionString { get; set; }
        string connection;

        public void getConnection()
        {
            connection = @"Data Source=Settings.db; Version=3";
            connectionString = connection;
        }
    }
}
