// ----------------------------------------------------------------------------
// The MIT License
// Simple Entity Component System framework https://github.com/Leopotam/ecs
// Copyright (c) 2017-2020 Leopotam <leopotam@gmail.com>
// -----

using System;
using System.Runtime.CompilerServices;

namespace Leopotam.Ecs
{
    /// <summary>
    /// Entity descriptor.
    /// </summary>
    public struct EcsEntity : IEquatable<EcsEntity>
    {
        internal int Id;
        internal ushort Gen;
        internal EcsWorld Owner;

        public static readonly EcsEntity Null = new EcsEntity();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(in EcsEntity lhs, in EcsEntity rhs) =>
            lhs.Id == rhs.Id && lhs.Gen == rhs.Gen;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(in EcsEntity lhs, in EcsEntity rhs) =>
            lhs.Id != rhs.Id || lhs.Gen != rhs.Gen;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            unchecked
            {
                // ReSharper disable NonReadonlyMemberInGetHashCode
                var hashCode = (Id * 397) ^ Gen.GetHashCode();
                hashCode = (hashCode * 397) ^ (Owner != null ? Owner.GetHashCode() : 0);
                // ReSharper restore NonReadonlyMemberInGetHashCode
                return hashCode;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other) =>
            other is EcsEntity otherEntity && Equals(otherEntity);

#if DEBUG
        public override string ToString() =>
            this.IsNull() ? "Entity-Null" : $"Entity-{Id}:{Gen}";
#endif
        public bool Equals(EcsEntity other) =>
            Id == other.Id && Gen == other.Gen && Owner == other.Owner;
    }
}
