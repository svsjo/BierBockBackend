using Microsoft.EntityFrameworkCore;

namespace DataStorage
{
    public class AppDatabaseContext: DbContext
    { 
        //Add-Migration Name
        //Update-Database

        private DbSet<DemoEntry> _demos;
        
        public void AddDemo(DemoEntry entry)
        {
            this._demos.Add(entry);
            this.SaveChanges();
        }

        public IQueryable<DemoEntry> GetDemos()
        {
            return _demos.AsQueryable();
        }


        
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source=ConnectionStrings:DefaultConnection");
        
    }

    public class DemoEntry
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
