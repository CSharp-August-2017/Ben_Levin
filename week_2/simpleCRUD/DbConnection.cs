using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

using System;
//run this in terminal: dotnet add package MySql.Data -v 7.0.7-*


namespace simpleCRUD
{
    public class DbConnector
    {
        static string server = "localhost";
        static string db = "consoleDB"; //Change to your schema name
        static string port = "8889"; //Potentially 8889
        static string user = "root";
        static string pass = "root";
        internal static IDbConnection Connection {
            get {
                return new MySqlConnection($"Server={server};Port={port};Database={db};UserID={user};Password={pass};SslMode=None");
            }
        }
        
        //This method runs a query and stores the response in a list of dictionary records
        public static List<Dictionary<string, object>> Query(string queryString)
        {
            using(IDbConnection dbConnection = Connection)
            {
                using(IDbCommand command = dbConnection.CreateCommand())
                {
                   command.CommandText = queryString;
                   dbConnection.Open();
                   var result = new List<Dictionary<string, object>>();
                   using(IDataReader rdr = command.ExecuteReader())
                   {
                      while(rdr.Read())
                      {
                          var dict = new Dictionary<string, object>();
                          for( int i = 0; i < rdr.FieldCount; i++ ) {
                              dict.Add(rdr.GetName(i), rdr.GetValue(i));
                          }
                          result.Add(dict);
                      }
                      return result;
                                      }
                }
            }
        }

        //This method run a query and returns no values
        public static void Execute(string queryString)
        {
            using (IDbConnection dbConnection = Connection)
            {
                using(IDbCommand command = dbConnection.CreateCommand())
                {
                    command.CommandText = queryString;
                    dbConnection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void Read(){
            var read = DbConnector.Query("SELECT * FROM Users;");
            foreach(var thing in read){
                Console.WriteLine(thing["id"] + ". " + thing["FirstName"] + " " + thing["LastName"] + " " + thing["FavoriteNumber"]);
            }
        }

        public static void Create(string first, string last, int favNum){
            DbConnector.Execute("INSERT INTO USERS (FirstName, LastName, FavoriteNumber) VALUES ('" + first + "','" + last + "','" + favNum + "')");
        }

        public static void Update(int id, string first, string last, int favNum){
            DbConnector.Execute("UPDATE Users SET FirstName = '" + first + "', LastName = '" + last + "', FavoriteNumber = '" + favNum + "' WHERE id = " + id);
        }

        public static void Delete(int id){
            DbConnector.Execute("DELETE FROM Users WHERE id = " + id);
        }
    }
}