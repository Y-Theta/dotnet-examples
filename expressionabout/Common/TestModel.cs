using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace expressionabout.Common
{
    [Flags]
    public enum TestEnum
    {
        None,

        Item1  =  1 ,
        Item2  =  1 << 1,
        Item3  =  1 << 2,
    }

    public class TestModel
    {
        public readonly string RoStr;

        private string _privateStr;

        private static string _privateConstStr;

        public string Str { get; set; }

        public int Number { get; set; }

        public int? NullableNumber { get; set; }
        
        public double Dou { get; set; }

        public float Fl { get; set; }

        public TestEnum En { get; set; }


        public static TestModel NewEmpty() => new TestModel();

        public static TestModel NewRandom() => new TestModel
        {
            Str = nameof(Str),
            _privateStr = nameof(Str),
            Dou = new Random().NextDouble(),
            Fl = (float)(new Random().NextDouble()),
            Number = new Random().Next(),
            NullableNumber = new Random().Next(),
            En = TestEnum.Item1
        };

    }
}
