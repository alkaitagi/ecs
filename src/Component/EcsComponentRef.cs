// ----------------------------------------------------------------------------
// The MIT License
// Simple Entity Component System framework https://github.com/Leopotam/ecs
// Copyright (c) 2017-2020 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace Leopotam.Ecs
{
    /// <summary>
    /// Helper for save reference to component. 
    /// </summary>
    /// <typeparam name="T">Type of component.</typeparam>
    public struct EcsComponentRef<T> where T : struct
    {
        internal EcsComponentPool<T> Pool;
        internal int Idx;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(in EcsComponentRef<T> lhs, in EcsComponentRef<T> rhs) =>
            lhs.Idx == rhs.Idx && lhs.Pool == rhs.Pool;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(in EcsComponentRef<T> lhs, in EcsComponentRef<T> rhs) =>
            lhs.Idx != rhs.Idx || lhs.Pool != rhs.Pool;

        public override bool Equals(object obj) =>
            obj is EcsComponentRef<T> other && Equals(other);

        // ReSharper disable NonReadonlyMemberInGetHashCode
        // ReSharper restore NonReadonlyMemberInGetHashCode
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() =>
            Idx;
    }
}
