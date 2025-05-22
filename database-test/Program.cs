using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using System;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace database_test
{
    public class Program
    {

        internal class ContextTest : DbContext
        {
            public DbSet<DbObj1> Obj1s { get; set; }

            public ContextTest()
            {
                
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                SqliteConnectionStringBuilder builder = new SqliteConnectionStringBuilder();
                builder.DataSource = @"context.db";
                Debug.WriteLine(builder.ConnectionString);
                optionsBuilder.UseSqlite(new SqliteConnection(builder.ConnectionString));
            }
        }

        static Program()
        {
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
        }

        public static void TestCollection()
        {
            using var db = new ContextTest();
            db.Database.EnsureCreated();

            var obj1s = db.Obj1s;
            if (obj1s.CountAsync().Result > 0)
            {
                var obj1last = db.Find<DbObj1>((long)2);
                db.Entry(obj1last).Collection(b => b.Ojb2s).Load();
                _ = obj1last.Ojb2s;
                return;
            }

            var obj1 = new DbObj1 { Id = 2, Str1 = "dbitem1" };
            obj1.Ojb2s.Add(new DbObj2 { Id = 21, Str1 = "dbitem2" });
            obj1.Ojb2s.Add(new DbObj2 { Id = 22, Str1 = "dbitem2" });
            db.Obj1s.Add(obj1);
            db.SaveChanges();

        }
    }
}
