using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Scripting;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace expressionabout
{
    public class ScriptRunner
    {
        public class Input
        {
            public Dictionary<string, object> Param { get; } = new Dictionary<string, object>();
        }

        public static T GenerateExpression<T>(string expression) where T : MulticastDelegate
        {
            var itemType = typeof(T);
            var invoke = itemType.GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public);

            if (invoke is null)
            {
                throw new ArgumentException("generic type error, generic type has no \"Invoke\" method");
            }

            if (invoke.ReturnType == typeof(void))
            {
                throw new ArgumentException("generic type error, generic delegate must has return value");
            }

            Type returntype = invoke.ReturnType;

            var inputparameters = invoke.GetParameters();
            List<Expression> callParameters = new List<Expression>();
            List<ParameterExpression> parameters = new List<ParameterExpression>();
            Type typeofglobal = null;
            if (inputparameters != null)
            {
                int index = 0;
                typeofglobal = typeof(Input);
                var newins = Expression.Constant(new Input());
                var dic = Expression.Property(newins, nameof(Input.Param));
                var addmethod = typeof(Dictionary<string, object>).GetMethod(nameof(Dictionary<string, object>.Add));
                List<Expression> assigns = new List<Expression>();
                foreach (var arg in inputparameters)
                {
                    index++;
                    var paramName = $"x{index}";
                    var paraminput = Expression.Parameter(arg.ParameterType, paramName);
                    var converted = Expression.Convert(paraminput, typeof(object));
                    var signed = Expression.Call(dic, addmethod, new Expression[] { Expression.Constant(paramName), converted });
                    //var prop = Expression.Property(newins, paramName);
                    //var signed = Expression.Assign(prop, paraminput);
                    assigns.Add(paraminput);
                    assigns.Add(signed);
                    parameters.Add(paraminput);
                }
                assigns.Add(newins);
                callParameters.Add(Expression.Block(assigns));
            }
            else
            {
                callParameters.Add(Expression.Constant(null));
            }

            var convertedExp = ConvertParameter(expression, parameters.Count);
            callParameters.Add(Expression.Constant(default(CancellationToken)));
            Script<object> script = CSharpScript.Create(convertedExp,
                options: ScriptOptions.Default.WithImports("System"),
                globalsType: typeofglobal);
            script.Compile();   //<-- load the Compilation from database/file here
            var comp = script.GetCompilation();
            SyntaxTree syntaxTree = comp.SyntaxTrees.Single();
            SyntaxNode syntaxTreeRoot = syntaxTree.GetRoot();
            SemanticModel semanticModel = comp.GetSemanticModel(syntaxTree);
            var symbol = semanticModel.GetDeclaredSymbol(syntaxTreeRoot);
            var scriptExp = Expression.Constant(script);
            var methods = typeof(Script<object>)
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .Where(m => m.Name == nameof(Script<object>.RunAsync));
            var method = methods.FirstOrDefault(m => m.GetParameters().Length == 2);
            var exp = Expression.Call(scriptExp, method, callParameters);
            var result = Expression.Property(exp, typeof(Task<ScriptState<object>>).GetProperty(nameof(Task<ScriptState<object>>.Result)));
            var propinfo = typeof(ScriptState).GetProperty(nameof(ScriptState.ReturnValue));
            var resultval = Expression.Property(result, propinfo);
            var typedResult = Expression.Convert(resultval, returntype);
            var lambda = Expression.Lambda(typedResult, parameters.ToArray());

            return lambda.Compile() as T;
        }

        private static string ConvertParameter(string expression, int paramCount)
        {
            if (string.IsNullOrEmpty(expression))
                return expression;

            List<string> parameters = new List<string>();
            for (int i = 0; i < paramCount; i++)
            {
                parameters.Add($"Param[\"x{i + 1}\"]");
            }

            return string.Format(expression, parameters.ToArray());
        }

        //private static Type GenerateClass(IEnumerable<ParameterInfo> proptypes)
        //{
        //    var builder = GetModuleBuilder();
        //    TypeBuilder typeBuilder = builder.DefineType(Guid.NewGuid().ToString("N"), TypeAttributes.Public);
        //    int index = 0;
        //    foreach (var prop in proptypes)
        //    {
        //        index++;
        //        AddProperty(typeBuilder, $"x{index}", prop.ParameterType);
        //    }

        //    return typeBuilder.CreateType();
        //}

        private static void AddProperty(TypeBuilder typeBuilder, string propertyName, Type propertyType)
        {
            // Define the private field
            FieldBuilder fieldBuilder = typeBuilder.DefineField($"_{propertyName.ToLower()}", propertyType, FieldAttributes.Private);

            // Define the property
            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);

            // Define the getter method
            MethodBuilder getterMethodBuilder = typeBuilder.DefineMethod(
                $"get_{propertyName}",
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                propertyType,
                Type.EmptyTypes);

            ILGenerator getterIL = getterMethodBuilder.GetILGenerator();
            getterIL.Emit(OpCodes.Ldarg_0);
            getterIL.Emit(OpCodes.Ldfld, fieldBuilder);
            getterIL.Emit(OpCodes.Ret);

            // Define the setter method
            MethodBuilder setterMethodBuilder = typeBuilder.DefineMethod(
                $"set_{propertyName}",
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                null,
                new Type[] { propertyType });

            ILGenerator setterIL = setterMethodBuilder.GetILGenerator();
            setterIL.Emit(OpCodes.Ldarg_0);
            setterIL.Emit(OpCodes.Ldarg_1);
            setterIL.Emit(OpCodes.Stfld, fieldBuilder);
            setterIL.Emit(OpCodes.Ret);

            // Map the getter and setter methods to the property
            propertyBuilder.SetGetMethod(getterMethodBuilder);
            propertyBuilder.SetSetMethod(setterMethodBuilder);
        }
    }
}
