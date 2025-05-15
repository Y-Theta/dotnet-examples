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
        [Column]
        public int Number1 { get; set; }
        public double Number2 { get; set; }
        public decimal Number3 { get; set; }

        public int? Number4 { get; set; }

        public string Str1 { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        public ICollection<DbObj2> Ojb2s { get; } = new List<DbObj2>();

        [NotMapped]
        public string Content => Str1;
    }

    [Table("db_obj_2")]
    public class DbObj2
    {
        public int Number1 { get; set; }
        public double Number2 { get; set; }
        public decimal Number3 { get; set; }

        public int? Number4 { get; set; }

        public string Str1 { get; set; }

        [ForeignKey(nameof(DbObj1))]
        public long Obj1Id { get; }

        [ForeignKey(nameof(Obj1Id))]
        public virtual DbObj1 Obj1 { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
    }
}
