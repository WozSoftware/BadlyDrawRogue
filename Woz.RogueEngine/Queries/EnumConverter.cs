using System;
using System.Linq.Expressions;

namespace Woz.RogueEngine.Queries
{
    public static class EnumConverter<TEnum> 
        where TEnum : struct, IConvertible
    {
        public static readonly Func<int, TEnum> Convert = GenerateConverter();

        static Func<int, TEnum> GenerateConverter()
        {
            var parameter = Expression.Parameter(typeof(int));

            var dynamicMethod = Expression
                .Lambda<Func<int, TEnum>>(
                    Expression.Convert(parameter, typeof(TEnum)),
                    parameter);

            return dynamicMethod.Compile();
        }
    }
}