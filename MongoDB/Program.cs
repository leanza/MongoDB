using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;

namespace MongoDB
{
    class Program
    {
       
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine("Eerst in cmd het volgende command uitvoeren nadat docker is geinstalleerd: ");
            Console.WriteLine("docker run -d -p 27017-27019:27017-27019 --name mongodb mongo:latest");
            
            DoMongoDBStuff();
        }

        private static void DoMongoDBStuff()
        {
            var dbClient = new MongoClient("mongodb://127.0.0.1:27017");
            var dbList = dbClient.ListDatabases().ToList();

            Console.WriteLine("The list of databases are:");

            foreach (var item in dbList)
            {
                Console.WriteLine(item);
            }

            IMongoDatabase mongoDB = dbClient.GetDatabase("Person");
            IMongoCollection<Person> collection = mongoDB.GetCollection<Person>("employees");
            Person p = new Person
            {
                FirstName = "Bob",
                LastName = "Baker",
                Age = new Random(DateTime.Now.Millisecond).Next(0, 150)
            };
            collection.InsertOne(p);
            var document = collection.Find(x => x.FirstName.Equals("Bob")).ToList();

            foreach (var item in document)
            {
                Console.WriteLine($"{item.LastName}, {item.FirstName} with age: {item.Age}");
            }

            //No type
            var col2 = mongoDB.GetCollection<BsonDocument>("cars");
            var doc = new BsonDocument
            {
                {"name", "BMW"},
                {"price", DateTime.Now.Millisecond + "34621" + DateTime.Now.Millisecond}
            };

            col2.InsertOne(doc);
            var nieuw = col2.Find(new BsonDocument()).ToList();
        }
    }

    public class Person
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}
