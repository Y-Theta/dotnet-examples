using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace database_test
{
    [Table("db_obj_1")]
    public class DbObj1
    {
        public int Number1 { get; set; }
        public double Number2 { get; set; }
        public decimal Number3 { get; set; }

        public int? Number4 { get; set; }

        public string Str1 { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [ForeignKey(nameof(DbObj2.Obj1Id))]
        public ICollection<DbObj2> Ojb2s { get; } = new List<DbObj2>();
    }

    [Table("db_obj_2")]
    public class DbObj2
    {
        public int Number1 { get; set; }
        public double Number2 { get; set; }
        public decimal Number3 { get; set; }

        public int? Number4 { get; set; }

        public string Str1 { get; set; }

        public long Obj1Id { get; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
    }
}
