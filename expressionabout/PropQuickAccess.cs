using expressionabout.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace expressionabout
{
    public class PropQuickAccess
    {
        private const int _num = 10000 * 100;

        private static readonly List<PropertyInfo> _readableinfos = new List<PropertyInfo>();
        private static readonly List<PropertyInfo> _writableinfos = new List<PropertyInfo>();

        private static readonly Dictionary<string, Func<TestModel, object>> _readableRef = new Dictionary<string, Func<TestModel, object>>();

        private static readonly Dictionary<string, (Action<TestModel, object>, object)> _assignableRef = new Dictionary<string, (Action<TestModel, object>, object)>();

        private static TestModel _model;

        private static IEnumerable<PropertyInfo> GetReadableProp<T>()
        {
            var props = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var item in props)
            {
                if (item.CanRead)
                {
                    yield return item;
                }
            }
        }

        private static IEnumerable<PropertyInfo> GetWritableProp<T>()
        {
            var props = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var item in props)
            {
                if (item.CanWrite)
                {
                    yield return item;
                }
            }
        }

        public static void Init()
        {
            _readableinfos.AddRange(GetReadableProp<TestModel>());
            _writableinfos.AddRange(GetWritableProp<TestModel>());
            BuildQuickAccess();
            BuildQuickAssign();
            _model = TestModel.NewRandom();
        }

        private static void BuildQuickAccess()
        {
            _readableRef.Clear();
            var input = Expression.Parameter(typeof(TestModel));

            foreach (var item in _readableinfos)
            {
                var getprop = Expression.Call(input, item.GetMethod);
                var ret = Expression.Convert(getprop, typeof(object));
                var func = Expression.Lambda<Func<TestModel, object>>(ret, input).Compile();
                _readableRef[item.Name] = func;
            }
        }

        private static void BuildQuickAssign()
        {
            _assignableRef.Clear();
            var input = Expression.Parameter(typeof(TestModel));
            var input1 = Expression.Parameter(typeof(object));

            foreach (var item in _writableinfos)
            {
                var conv = Expression.Convert(input1, item.PropertyType);
                var getprop = Expression.Call(input, item.SetMethod, conv);
                var func = Expression.Lambda<Action<TestModel, object>>(getprop, input, input1).Compile();
                _assignableRef[item.Name] = (func, item.PropertyType.IsValueType ? (object)0 : (object)null);
            }
        }

        public static void GetByReflection()
        {
            for (int i = 0; i < _num; i++)
            {
                foreach (var prop in _readableinfos)
                {
                    var propval = prop.GetValue(_model);
                }
            }
        }

        public static void AssignByReflection()
        {
            for (int i = 0; i < _num; i++)
            {
                foreach (var prop in _readableinfos)
                {
                    if (prop.PropertyType.IsValueType)
                    {
                        prop.SetValue(_model, 0);
                    }
                    else
                    {
                        prop.SetValue(_model, null);
                    }
                }
            }
        }

        public static void GetByExpression()
        {
            for (int i = 0; i < _num; i++)
            {
                foreach (var item in _readableRef)
                {
                    var propval = item.Value(_model);
                }
            }
        }

        public static void AssignByExpression()
        {
            for (int i = 0; i < _num; i++)
            {
                foreach (var prop in _assignableRef)
                {
                    prop.Value.Item1(_model, prop.Value.Item2);
                }
            }
        }

    }
}
