using System;
using System.Linq.Expressions;

namespace OverStory
{
    /// <summary>
    /// 동적 코드생성 AOT (Ahead Of Time) 플랫폼(IOS)에선 사용 불가 BitConvert 사용 권장
    /// 프로젝트 통일성 권장
    /// </summary>
    public class ValueCastTo
    {
        protected static class Cache<TFrom, TTo>
        {
            public static readonly Func<TFrom, TTo> Caster = Get();

            static Func<TFrom, TTo> Get()
            {
                var p = Expression.Parameter(typeof(TFrom), "from");
                var c = Expression.ConvertChecked(p, typeof(TTo));
                return Expression.Lambda<Func<TFrom, TTo>>(c, p).Compile();
            }
        }
    }

    public class ValueCastTo<TTo> : ValueCastTo
    {
        public static TTo From<TFrom>(TFrom from)
        {
            return Cache<TFrom, TTo>.Caster(from);
        }
    }
}