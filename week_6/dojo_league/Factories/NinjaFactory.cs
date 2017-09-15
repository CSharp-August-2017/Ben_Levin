using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using dojo_league.Models;


using System; //for dev, Console.WriteLine
 
namespace dojo_league.Factory
{
    public class NinFactory : IFactory<Ninja>
    {
        private string connectionString;
        public NinFactory()
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

        public void Register(Ninja ninja)
        {
            using (IDbConnection dbConnection = Connection) 
            {
                string query =  "INSERT INTO ninjas (name, level, description, created_at, updated_at, dojo_id) VALUES (@Name, @Level, @Description, NOW(), NOW(), @DojoID)";
                dbConnection.Open();
                dbConnection.Execute(query, ninja);
            }
        }

        public Ninja GetNinjaByID(int ID)
        {
            using (IDbConnection dbConnection = Connection)
            {
                var query = $"SELECT * FROM ninjas JOIN dojos ON ninjas.dojo_id WHERE dojos.id = ninjas.dojo_id AND ninjas.id={ID}";
                dbConnection.Open();
                var Ninja = dbConnection.Query<Ninja, Dojo, Ninja>(query, (ninja, dojo) => { ninja.dojo = dojo; return ninja; }).Single();
                return Ninja;
            }
        }

        public IEnumerable<Ninja> FindAll() //returns ninjas per dojo id //DojoID not working
        {
            using (IDbConnection dbConnection = Connection)
            {
                var query = $"SELECT * FROM ninjas JOIN dojos ON ninjas.dojo_id WHERE dojos.id = ninjas.dojo_id";
                dbConnection.Open();
        
                var myNinjas = dbConnection.Query<Ninja, Dojo, Ninja>(query, (ninja, dojo) => { ninja.dojo = dojo; return ninja; });
                return myNinjas;
            }
        }

        public void Banish(int ID)
        {
            using (IDbConnection dbConnection = Connection) 
            {
                string query = $"UPDATE ninjas SET dojo_id=1 WHERE id={ID}";
                dbConnection.Open();
                dbConnection.Execute(query);
            }
        }

        public void Recruit(int ninjaID, int dojoID)
        {
            using (IDbConnection dbConnection = Connection) 
            {
                string query = $"UPDATE ninjas SET dojo_id={dojoID} WHERE id={ninjaID}";
                dbConnection.Open();
                dbConnection.Execute(query);
            }
        }

        public IEnumerable<Ninja> NinjasForDojoByID(int ID) //returns ninjas per dojo id //not in use, copied from portal
        {
            using (IDbConnection dbConnection = Connection)
            {
                var query = $"SELECT * FROM ninjas JOIN dojos ON ninjas.dojo_id WHERE dojos.id = ninjas.dojo_id AND dojos.id = {ID}";
                dbConnection.Open();
                var myNinjas = dbConnection.Query<Ninja, Dojo, Ninja>(query, (ninja, dojo) => { ninja.dojo = dojo; return ninja; });
                return myNinjas;
            }
        }
    }
}
