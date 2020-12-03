// ----------------------------------------------------------------------------
// The MIT License
// Simple Entity Component System framework https://github.com/Leopotam/ecs
// Copyright (c) 2017-2020 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;

namespace Leopotam.Ecs
{
    public static partial class EcsEntityExtensions
    {
        /// <summary>
        /// Gets component index at component pool.
        /// If component doesn't exists "-1" will be returned.
        /// </summary>
        /// <typeparam name="T">Type of component.</typeparam>
#if ENABLE_IL2CPP
        [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetComponentIndexInPool<T>(in this EcsEntity entity) where T : struct
        {
            ref var entityData = ref entity.Owner.GetEntityData(entity);
#if DEBUG
            if (entityData.Gen != entity.Gen) throw new Exception("Cant check component on destroyed entity.");
#endif
            var typeIdx = EcsComponentType<T>.TypeIndex;
            for (int i = 0, iMax = entityData.ComponentsCountX2; i < iMax; i += 2)
                if (entityData.Components[i] == typeIdx)
                    return entityData.Components[i + 1];
            return -1;
        }

        /// <summary>
        /// Gets components count on entity.
        /// </summary>
#if ENABLE_IL2CPP
        [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetComponentsCount(in this EcsEntity entity)
        {
            ref var entityData = ref entity.Owner.GetEntityData(entity);
#if DEBUG
            if (entityData.Gen != entity.Gen) throw new Exception("Cant touch destroyed entity.");
#endif
            return entityData.ComponentsCountX2 <= 0 ? 0 : (entityData.ComponentsCountX2 >> 1);
        }

        /// <summary>
        /// Gets types of all attached components.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <param name="list">List to put results in it. if null - will be created. If not enough space - will be resized.</param>
        /// <returns>Amount of components in list.</returns>
        public static int GetComponentTypes(in this EcsEntity entity, ref Type[] list)
        {
            ref var entityData = ref entity.Owner.GetEntityData(entity);
#if DEBUG
            if (entityData.Gen != entity.Gen) throw new Exception("Cant touch destroyed entity.");
#endif
            var itemsCount = entityData.ComponentsCountX2 >> 1;
            if (list == null || list.Length < itemsCount)
                list = new Type[itemsCount];
            for (int i = 0, j = 0, iMax = entityData.ComponentsCountX2; i < iMax; i += 2, j++)
                list[j] = entity.Owner.ComponentPools[entityData.Components[i]].ItemType;
            return itemsCount;
        }

        /// <summary>
        /// Gets types of all attached components. Important: force boxing / unboxing!
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <param name="list">List to put results in it. if null - will be created. If not enough space - will be resized.</param>
        /// <returns>Amount of components in list.</returns>
        public static int GetComponentValues(in this EcsEntity entity, ref object[] list)
        {
            ref var entityData = ref entity.Owner.GetEntityData(entity);
#if DEBUG
            if (entityData.Gen != entity.Gen) throw new Exception("Cant touch destroyed entity.");
#endif
            var itemsCount = entityData.ComponentsCountX2 >> 1;
            if (list == null || list.Length < itemsCount)
                list = new object[itemsCount];
            for (int i = 0, j = 0, iMax = entityData.ComponentsCountX2; i < iMax; i += 2, j++)
            {
                list[j] = entity.Owner.ComponentPools[entityData.Components[i]].GetItem(entityData.Components[i + 1]);
            }
            return itemsCount;
        }
    }
}
