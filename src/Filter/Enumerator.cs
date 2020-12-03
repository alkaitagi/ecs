// ----------------------------------------------------------------------------
// The MIT License
// Simple Entity Component System framework https://github.com/Leopotam/ecs
// Copyright (c) 2017-2020 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;

namespace Leopotam.Ecs
{
    public abstract partial class EcsFilter
    {
        public struct Enumerator : IDisposable
        {
            readonly EcsFilter _filter;
            readonly int _count;
            int _idx;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator(EcsFilter filter)
            {
                _filter = filter;
                _count = _filter.GetEntitiesCount();
                _idx = -1;
                _filter.Lock();
            }

            public int Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _idx;
            }

#if ENABLE_IL2CPP
            [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
            [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Dispose() =>
                _filter.Unlock();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() =>
                ++_idx < _count;
        }
    }
}
