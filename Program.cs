using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using Cassandra;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Cassandra.Mapping;
using Išskirstytosios_duomenų_bazės.Model;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Išskirstytosios_duomenų_bazės
{

    class Program
    {

        private const string UserName = "books";
        private const string Password = "DHRhmg5fJxpEdkozd3FBx4LdLrJKhEdpJu3oOE57aP3yDJQj8OeBSOWrj1ryJa9IukeiuyfJhp6dlp4neLo7qA==";
        private const string CassandraContactPoint = "books.cassandra.cosmos.azure.com";  // DnsName  
        private static int CassandraPort = 10350;

        public static async Task Main(string[] args)
        {
            Console.WriteLine("||------------------Cassandra database------------------\\\\\n");

            // Connect to cassandra cluster  (Cassandra API on Azure Cosmos DB supports only TLSv1.2)
            var options = new Cassandra.SSLOptions(SslProtocols.Tls12, true, ValidateServerCertificate);
            options.SetHostNameResolver((ipAddress) => CassandraContactPoint);
            Cluster cluster = Cluster.Builder().WithCredentials(UserName, Password).WithPort(CassandraPort).AddContactPoint(CassandraContactPoint).WithSSL(options).Build();
            ISession session = cluster.Connect();


            session = cluster.Connect("booksdb");
            IMapper mapper = new Mapper(session);

            Console.WriteLine("||------------------Select records from Book table------------------\\\\");
            PrintCassandraTable(mapper);

            Console.WriteLine("\n||------------------Insert record to Book table------------------\\\\");
            Book bookCs = new Book("JK2354470", "https://www.sena.lt/religija--filosofija/kantas_imanuelis-prolegomenai67629", "1993", false, "Vilniaus universiteto leidykla", 1, 4, 6, 4);
            await mapper.InsertAsync<Book>(bookCs);
            PrintCassandraTable(mapper);


            Console.WriteLine("\n||------------------Update record in Book table------------------\\\\");
            bookCs.Fk_Publishing_house = "Žalias kalnas";
            await mapper.UpdateAsync<Book>(bookCs.CQLString());
            PrintCassandraTable(mapper);


            Console.WriteLine("\n||------------------Delete record in Book table------------------\\\\");
            await mapper.DeleteAsync<Book>("WHERE isbn = 'JK2354470'");
            PrintCassandraTable(mapper);

            Console.WriteLine("||------------------Count records ------------------\\\\");

            var rez = session.Execute("SELECT Count(*) AS count FROM Book").GetEnumerator();
            rez.MoveNext();
            Console.WriteLine(rez.Current.GetValue<Int64>(0));


            Console.WriteLine("\n||------------------SQL database------------------\\\\\n");
            
            SQLConnector _SQLConnector = new SQLConnector();

            Console.WriteLine("||------------------Select record(s) from Book table------------------\\\\");
            PrintSQLTable(_SQLConnector);

            //Pasiimti vieną įrašą iš norimos lentelės
            //Book bookk = (Book)_SQLConnector.ReadRecords<Book>("SELECT * FROM Book where isbn = 'JK2354454'", false);
            //Console.WriteLine(bookk.ToString());


            BookSQL book1 = new BookSQL{ 
                    Isbn = "JK2354470",
                    Title = "Prolegomenai",
                    Price = 15,
                    Cover = 2,
                    Mass = 25f            
            };

            Console.WriteLine("||------------------Insert record to Book table------------------\\\\");
            bool insert = _SQLConnector.InsertRecord("INSERT INTO Book(isbn, title, price, cover, mass)VALUES(@isbn, @title, @price, @cover, @mass)", book1);
            PrintSQLTable(_SQLConnector);
            Console.WriteLine("\nOperation worked: " + insert + "\n");

            Console.WriteLine("||------------------Update record in Book table------------------\\\\");
            book1.Isbn = "JK2354490";
            bool update = _SQLConnector.UpdateRecord("UPDATE Book SET isbn=@isbn, title=@title, price=@price, cover=@cover, mass=@mass WHERE isbn='JK2354470'", book1);
            PrintSQLTable(_SQLConnector);
            Console.WriteLine("\nOperation worked: " + update + "\n");

            Console.WriteLine("||------------------Delete record from Book table------------------\\\\");
            bool delete = _SQLConnector.RemoveRecord("DELETE FROM Book where isbn='JK2354490'");
            PrintSQLTable(_SQLConnector);
            Console.WriteLine("\nOperation worked: " + delete + "\n");

        }

        public static void PrintSQLTable(SQLConnector _SQLConnector)
        {
            var book = (List<BookSQL>)_SQLConnector.ReadRecords<BookSQL>("SELECT * FROM Book", true);
            foreach (var item in book)
            {
                Console.WriteLine(item);
            }
        }

        public static void PrintCassandraTable(IMapper mapper)
        {
            foreach (Book book in mapper.Fetch<Book>("Select * from book"))
            {
                Console.WriteLine(book);
            }
        }

        public static bool ValidateServerCertificate(
        object sender,
        X509Certificate certificate,
        X509Chain chain,
        SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);
            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }
    }
}



/*

using Cassandra;
using Cassandra.Mapping;
using System;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace CassandraQuickStartSample
{
    public class Program
    {
        // Cassandra Cluster Configs      
        private const string UserName = "books";
        private const string Password = "DHRhmg5fJxpEdkozd3FBx4LdLrJKhEdpJu3oOE57aP3yDJQj8OeBSOWrj1ryJa9IukeiuyfJhp6dlp4neLo7qA==";
        private const string CassandraContactPoint = "books.cassandra.cosmos.azure.com";  // DnsName  
        private static int CassandraPort = 10350;

        public static void Main(string[] args)
        {
            // Connect to cassandra cluster  (Cassandra API on Azure Cosmos DB supports only TLSv1.2)
            var options = new Cassandra.SSLOptions(SslProtocols.Tls12, true, ValidateServerCertificate);
            options.SetHostNameResolver((ipAddress) => CassandraContactPoint);
            Cluster cluster = Cluster.Builder().WithCredentials(UserName, Password).WithPort(CassandraPort).AddContactPoint(CassandraContactPoint).WithSSL(options).Build();
            ISession session = cluster.Connect();

            // Creating KeySpace and table
            session.Execute("DROP KEYSPACE IF EXISTS uprofile");
            session.Execute("CREATE KEYSPACE uprofile WITH REPLICATION = { 'class' : 'NetworkTopologyStrategy', 'datacenter1' : 1 };");
            Console.WriteLine(String.Format("created keyspace uprofile"));
            session.Execute("CREATE TABLE IF NOT EXISTS uprofile.user (user_id int PRIMARY KEY, user_name text, user_bcity text)");
            Console.WriteLine(String.Format("created table user"));

            session = cluster.Connect("uprofile");
            IMapper mapper = new Mapper(session);

            // Inserting Data into user table
            mapper.Insert<User>(new User(1, "LyubovK", "Dubai"));
            mapper.Insert<User>(new User(2, "JiriK", "Toronto"));
            mapper.Insert<User>(new User(3, "IvanH", "Mumbai"));
            mapper.Insert<User>(new User(4, "LiliyaB", "Seattle"));
            mapper.Insert<User>(new User(5, "JindrichH", "Buenos Aires"));
            Console.WriteLine("Inserted data into user table");

            Console.WriteLine("Select ALL");
            Console.WriteLine("-------------------------------");
            foreach (User user in mapper.Fetch<User>("Select * from user"))
            {
                Console.WriteLine(user);
            }

            Console.WriteLine("Getting by id 3");
            Console.WriteLine("-------------------------------");
            User userId3 = mapper.FirstOrDefault<User>("Select * from user where user_id = ?", 3);
            Console.WriteLine(userId3);

            // Clean up of Table and KeySpace
            session.Execute("DROP table user");
            session.Execute("DROP KEYSPACE uprofile");

            // Wait for enter key before exiting  
            Console.ReadLine();
        }

        public static bool ValidateServerCertificate(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);
            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }
    }
}

*/