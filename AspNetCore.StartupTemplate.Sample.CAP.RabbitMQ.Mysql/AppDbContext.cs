using Microsoft.EntityFrameworkCore;

namespace Sample.RabbitMQ.MySql
{
    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return $"Name:{Name}, Id:{Id}";
        }
    }


    public class AppDbContext : DbContext
    {
        public const string ConnectionString = "server=127.0.0.1;port=49153;uid=root;pwd=mysqlpw;database=aspnetcoretemplate;charset='utf8';pooling=true";

        public DbSet<Person> Persons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
        }
    }
}
