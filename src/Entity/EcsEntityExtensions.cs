// ----------------------------------------------------------------------------
// The MIT License
// Simple Entity Component System framework https://github.com/Leopotam/ecs
// Copyright (c) 2017-2020 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;

namespace Leopotam.Ecs
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public static partial class EcsEntityExtensions
    {
        /// <summary>
        /// Replaces or adds new one component to entity.
        /// Slower than Get() direct call.
        /// </summary>
        /// <typeparam name="T">Type of component.</typeparam>
        /// <param name="entity">Entity.</param>
        /// <param name="item">New value of component.</param>
#if ENABLE_IL2CPP
        [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EcsEntity Replace<T>(in this EcsEntity entity, in T item) where T : struct
        {
            Get<T>(entity) = item;
            return entity;
        }

        /// <summary>
        /// Returns exist component on entity or adds new one otherwise.
        /// </summary>
        /// <typeparam name="T">Type of component.</typeparam>
#if ENABLE_IL2CPP
        [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Get<T>(in this EcsEntity entity) where T : struct
        {
            ref var entityData = ref entity.Owner.GetEntityData(entity);
#if DEBUG
            if (entityData.Gen != entity.Gen) throw new Exception("Cant add component to destroyed entity.");
#endif
            var typeIdx = EcsComponentType<T>.TypeIndex;
            // check already attached components.
            for (int i = 0, iiMax = entityData.ComponentsCountX2; i < iiMax; i += 2)
                if (entityData.Components[i] == typeIdx)
                {
                    return ref ((EcsComponentPool<T>)entity.Owner.ComponentPools[typeIdx]).Items[entityData.Components[i + 1]];
                }
            // attach new component.
            if (entityData.Components.Length == entityData.ComponentsCountX2)
                Array.Resize(ref entityData.Components, entityData.ComponentsCountX2 << 1);
            entityData.Components[entityData.ComponentsCountX2++] = typeIdx;

            var pool = entity.Owner.GetPool<T>();

            var idx = pool.New();
            entityData.Components[entityData.ComponentsCountX2++] = idx;
#if DEBUG
            for (var ii = 0; ii < entity.Owner.DebugListeners.Count; ii++)
                entity.Owner.DebugListeners[ii].OnComponentListChanged(entity);
#endif
            entity.Owner.UpdateFilters(typeIdx, entity, entityData);
            return ref pool.Items[idx];
        }

        /// <summary>
        /// Checks that component is attached to entity.
        /// </summary>
        /// <typeparam name="T">Type of component.</typeparam>
#if ENABLE_IL2CPP
        [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Has<T>(in this EcsEntity entity) where T : struct
        {
            ref var entityData = ref entity.Owner.GetEntityData(entity);
#if DEBUG
            if (entityData.Gen != entity.Gen) throw new Exception("Cant check component on destroyed entity.");
#endif
            var typeIdx = EcsComponentType<T>.TypeIndex;
            for (int i = 0, iMax = entityData.ComponentsCountX2; i < iMax; i += 2)
                if (entityData.Components[i] == typeIdx)
                    return true;
            return false;
        }

        /// <summary>
        /// Removes component from entity.
        /// </summary>
        /// <typeparam name="T">Type of component.</typeparam>
#if ENABLE_IL2CPP
        [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Del<T>(in this EcsEntity entity) where T : struct
        {
            var typeIndex = EcsComponentType<T>.TypeIndex;
            ref var entityData = ref entity.Owner.GetEntityData(entity);
            // save copy to local var for protect from cleanup fields outside.
            var owner = entity.Owner;
#if DEBUG
            if (entityData.Gen != entity.Gen) throw new Exception("Cant touch destroyed entity.");
#endif
            for (int i = 0, iMax = entityData.ComponentsCountX2; i < iMax; i += 2)
                if (entityData.Components[i] == typeIndex)
                {
                    owner.UpdateFilters(-typeIndex, entity, entityData);
#if DEBUG
                    // var removedComponent = owner.ComponentPools[typeIndex].GetItem (entityData.Components[i + 1]);
#endif
                    owner.ComponentPools[typeIndex].Recycle(entityData.Components[i + 1]);
                    // remove current item and move last component to this gap.
                    entityData.ComponentsCountX2 -= 2;
                    if (i < entityData.ComponentsCountX2)
                    {
                        entityData.Components[i] = entityData.Components[entityData.ComponentsCountX2];
                        entityData.Components[i + 1] = entityData.Components[entityData.ComponentsCountX2 + 1];
                    }
#if DEBUG
                    for (var ii = 0; ii < entity.Owner.DebugListeners.Count; ii++)
                        entity.Owner.DebugListeners[ii].OnComponentListChanged(entity);
#endif
                    break;
                }
            // unrolled and inlined Destroy() call.
            if (entityData.ComponentsCountX2 == 0)
            {
                owner.RecycleEntityData(entity.Id, ref entityData);
#if DEBUG
                for (var ii = 0; ii < entity.Owner.DebugListeners.Count; ii++)
                    owner.DebugListeners[ii].OnEntityDestroyed(entity);
#endif
            }
        }

        /// <summary>
        /// Creates copy of entity with all components.
        /// </summary>
#if ENABLE_IL2CPP
        [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EcsEntity Copy(in this EcsEntity entity)
        {
            var owner = entity.Owner;
#if DEBUG
            if (owner == null) throw new Exception("Cant copy invalid entity.");
#endif
            ref var srcData = ref owner.GetEntityData(entity);
#if DEBUG
            if (srcData.Gen != entity.Gen) throw new Exception("Cant copy destroyed entity.");
#endif
            var dstEntity = owner.NewEntity();
            ref var dstData = ref owner.GetEntityData(dstEntity);
            if (dstData.Components.Length < srcData.ComponentsCountX2)
                dstData.Components = new int[srcData.Components.Length];
            dstData.ComponentsCountX2 = 0;
            for (int i = 0, iiMax = srcData.ComponentsCountX2; i < iiMax; i += 2)
            {
                var typeIdx = srcData.Components[i];
                var pool = owner.ComponentPools[typeIdx];
                var dstItemIdx = pool.New();
                dstData.Components[i] = typeIdx;
                dstData.Components[i + 1] = dstItemIdx;
                pool.CopyData(srcData.Components[i + 1], dstItemIdx);
                dstData.ComponentsCountX2 += 2;
                owner.UpdateFilters(typeIdx, dstEntity, dstData);
            }
#if DEBUG
            for (var ii = 0; ii < owner.DebugListeners.Count; ii++)
                owner.DebugListeners[ii].OnComponentListChanged(entity);
#endif
            return dstEntity;
        }

        /// <summary>
        /// Adds copies of source entity components
        /// on target entity (overwrite exists) and
        /// removes source entity.
        /// </summary>
#if ENABLE_IL2CPP
        [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MoveTo(in this EcsEntity source, in EcsEntity target)
        {
#if DEBUG
            if (!source.IsAlive()) throw new Exception("Cant move from invalid entity.");
            if (!target.IsAlive()) throw new Exception("Cant move to invalid entity.");
            if (source.Owner != target.Owner) throw new Exception("Cant move data between worlds.");
            if (source.AreEquals(target)) throw new Exception("Source and target entities are same.");
            var componentsListChanged = false;
#endif
            var owner = source.Owner;
            ref var srcData = ref owner.GetEntityData(source);
            ref var dstData = ref owner.GetEntityData(target);
            if (dstData.Components.Length < srcData.ComponentsCountX2)
                dstData.Components = new int[srcData.Components.Length];
            for (int i = 0, iiMax = srcData.ComponentsCountX2; i < iiMax; i += 2)
            {
                var typeIdx = srcData.Components[i];
                var pool = owner.ComponentPools[typeIdx];
                var j = dstData.ComponentsCountX2 - 2;
                // search exist component on target.
                for (; j >= 0; j -= 2)
                    if (dstData.Components[j] == typeIdx) { break; }
                if (j >= 0)
                    // found, copy data.
                    pool.CopyData(srcData.Components[i + 1], dstData.Components[j + 1]);
                else
                {
                    // add new one.
                    if (dstData.Components.Length == dstData.ComponentsCountX2)
                        Array.Resize(ref dstData.Components, dstData.ComponentsCountX2 << 1);
                    dstData.Components[dstData.ComponentsCountX2] = typeIdx;
                    var idx = pool.New();
                    dstData.Components[dstData.ComponentsCountX2 + 1] = idx;
                    dstData.ComponentsCountX2 += 2;
                    pool.CopyData(srcData.Components[i + 1], idx);
                    owner.UpdateFilters(typeIdx, target, dstData);
#if DEBUG
                    componentsListChanged = true;
#endif
                }
            }
#if DEBUG
            if (componentsListChanged)
                for (var ii = 0; ii < owner.DebugListeners.Count; ii++)
                    owner.DebugListeners[ii].OnComponentListChanged(target);
#endif
            source.Destroy();
        }

        /// <summary>
        /// Gets ComponentRef wrapper to keep direct reference to component.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <typeparam name="T">Component type.</typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EcsComponentRef<T> Ref<T>(in this EcsEntity entity) where T : struct
        {
            ref var entityData = ref entity.Owner.GetEntityData(entity);
#if DEBUG
            if (entityData.Gen != entity.Gen) throw new Exception("Cant wrap component on destroyed entity.");
#endif
            var typeIdx = EcsComponentType<T>.TypeIndex;
            for (int i = 0, iMax = entityData.ComponentsCountX2; i < iMax; i += 2)
                if (entityData.Components[i] == typeIdx)
                    return ((EcsComponentPool<T>)entity.Owner.ComponentPools[entityData.Components[i]]).Ref(entityData.Components[i + 1]);
#if DEBUG
            throw new Exception($"\"{typeof(T).Name}\" component not exists on entity for wrapping.");
#else
            return default;
#endif
        }

        /// <summary>
        /// Removes components from entity and destroys it.
        /// </summary>
#if ENABLE_IL2CPP
        [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Destroy(in this EcsEntity entity)
        {
            ref var entityData = ref entity.Owner.GetEntityData(entity);
            // save copy to local var for protect from cleanup fields outside.
            EcsEntity savedEntity;
            savedEntity.Id = entity.Id;
            savedEntity.Gen = entity.Gen;
            savedEntity.Owner = entity.Owner;
#if DEBUG
            if (entityData.Gen != entity.Gen) throw new Exception("Cant touch destroyed entity.");
#endif
            // remove components first.
            for (var i = entityData.ComponentsCountX2 - 2; i >= 0; i -= 2)
            {
                savedEntity.Owner.UpdateFilters(-entityData.Components[i], savedEntity, entityData);
                savedEntity.Owner.ComponentPools[entityData.Components[i]].Recycle(entityData.Components[i + 1]);
                entityData.ComponentsCountX2 -= 2;
#if DEBUG
                for (var ii = 0; ii < savedEntity.Owner.DebugListeners.Count; ii++)
                    savedEntity.Owner.DebugListeners[ii].OnComponentListChanged(savedEntity);
#endif
            }
            entityData.ComponentsCountX2 = 0;
            savedEntity.Owner.RecycleEntityData(savedEntity.Id, ref entityData);
#if DEBUG
            for (var ii = 0; ii < savedEntity.Owner.DebugListeners.Count; ii++)
                savedEntity.Owner.DebugListeners[ii].OnEntityDestroyed(savedEntity);
#endif
        }
    }
}
