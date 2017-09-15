using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using dojo_league.Models;
 
namespace dojo_league.Factory
{
    public class DojFactory : IFactory<Dojo>
    {
        private string connectionString;
        public DojFactory()
        {
            connectionString = "server=localhost;userid=root;password=root;port=8889;database=mydb;SslMode=None";
        }
        
        internal IDbConnection Connection
        {
            get 
            {
                return new MySqlConnection(connectionString);
            }
        }

        public void Register(Dojo dojo)
        {
            using (IDbConnection dbConnection = Connection) 
            {
                string query =  "INSERT INTO dojos (name, location, info, created_at, updated_at) VALUES (@Name, @Location, @Info, NOW(), NOW())";
                dbConnection.Open();
                dbConnection.Execute(query, dojo);
            }
        }

        public IEnumerable<Dojo> FindAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Dojo>("SELECT * FROM dojos");
            }
        }
        
        public Dojo GetDojoByID(int ID)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var query = 
                @"
                SELECT * FROM dojos WHERE id=@Id;
                SELECT * FROM ninjas WHERE dojo_id = @Id;
                ";
                using (var multi = dbConnection.QueryMultiple(query, new {Id = ID}))
                {
                    var dojo = multi.Read<Dojo>().Single();
                    dojo.ninjas = multi.Read<Ninja>().ToList();
                    return dojo;
                }
            }
        }
    }
}