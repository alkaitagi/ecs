// ----------------------------------------------------------------------------
// The MIT License
// Simple Entity Component System framework https://github.com/Leopotam/ecs
// Copyright (c) 2017-2020 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Threading;

namespace Leopotam.Ecs
{
    /// <summary>
    /// Global descriptor of used component type.
    /// </summary>
    /// <typeparam name="T">Component type.</typeparam>
    public static class EcsComponentType<T> where T : struct
    {
        // ReSharper disable StaticMemberInGenericType
        public static readonly int TypeIndex;
        public static readonly Type Type;
        public static readonly bool IsIgnoreInFilter;
        public static readonly bool IsAutoReset;
        // ReSharper restore StaticMemberInGenericType

        static EcsComponentType()
        {
            TypeIndex = Interlocked.Increment(ref EcsComponentPool.ComponentTypesCount);
            Type = typeof(T);
            IsIgnoreInFilter = typeof(IEcsIgnoreInFilter).IsAssignableFrom(Type);
            IsAutoReset = typeof(IEcsAutoReset<T>).IsAssignableFrom(Type);
#if DEBUG
            if (!IsAutoReset && Type.GetInterface("IEcsAutoReset`1") != null)
                throw new Exception($"IEcsAutoReset should have <{typeof(T).Name}> constraint for component \"{typeof(T).Name}\".");
#endif
        }
    }
}
