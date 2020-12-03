// ----------------------------------------------------------------------------
// The MIT License
// Simple Entity Component System framework https://github.com/Leopotam/ecs
// Copyright (c) 2017-2020 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace Leopotam.Ecs
{
    public static partial class EcsEntityExtensions
    {
        /// <summary>
        /// Is entity null-ed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNull(in this EcsEntity entity) =>
            entity.Id == 0 && entity.Gen == 0;

        /// <summary>
        /// Is entity alive. If world was destroyed - false will be returned.
        /// </summary>
#if ENABLE_IL2CPP
        [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAlive(in this EcsEntity entity)
        {
            if (!IsWorldAlive(entity)) return false;
            ref var entityData = ref entity.Owner.GetEntityData(entity);
            return entityData.Gen == entity.Gen && entityData.ComponentsCountX2 >= 0;
        }

        /// <summary>
        /// Is world alive.
        /// </summary>
#if ENABLE_IL2CPP
        [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsWorldAlive(in this EcsEntity entity) =>
            entity.Owner != null && entity.Owner.IsAlive();

        /// <summary>
        /// Compares entities. 
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AreEquals(in this EcsEntity lhs, in EcsEntity rhs) =>
            lhs.Id == rhs.Id && lhs.Gen == rhs.Gen;

        /// <summary>
        /// Compares internal Ids without Gens check. Use carefully! 
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AreIdEquals(in this EcsEntity lhs, in EcsEntity rhs) =>
            lhs.Id == rhs.Id;
    }
}
