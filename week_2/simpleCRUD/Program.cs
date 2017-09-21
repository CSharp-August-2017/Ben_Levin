using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace simpleCRUD
{
    
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Testing Read Function");
            DbConnector.Read();
            Console.WriteLine("Testing Create Function");
            DbConnector.Create("Steve", "Smith", 44);
            DbConnector.Read();
            Console.WriteLine("Testing Update Function");
            DbConnector.Update(6, "Scotty", "Jones", 444);
            DbConnector.Read();
            Console.WriteLine("Testing Delete Function");
            DbConnector.Delete(6);
            DbConnector.Read();
        }
    }
}
