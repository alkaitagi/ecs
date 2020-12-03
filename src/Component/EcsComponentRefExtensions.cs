// ----------------------------------------------------------------------------
// The MIT License
// Simple Entity Component System framework https://github.com/Leopotam/ecs
// Copyright (c) 2017-2020 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace Leopotam.Ecs
{

#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public static class EcsComponentRefExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Unref<T>(in this EcsComponentRef<T> wrapper) where T : struct =>
            ref wrapper.Pool.Items[wrapper.Idx];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNull<T>(in this EcsComponentRef<T> wrapper) where T : struct =>
            wrapper.Pool == null;
    }
}
