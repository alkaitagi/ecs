
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
#if UNITY_2019_1_OR_NEWER
    [UnityEngine.Scripting.Preserve]
#endif
    public class EcsFilter<Inc1, Inc2> : EcsFilter
        where Inc1 : struct
        where Inc2 : struct
    {
        int[] _get1;
        int[] _get2;

        readonly bool _allow1;
        readonly bool _allow2;

        readonly EcsComponentPool<Inc1> _pool1;
        readonly EcsComponentPool<Inc2> _pool2;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref Inc1 Get1(in int idx) =>
            ref _pool1.Items[_get1[idx]];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref Inc2 Get2(in int idx) =>
            ref _pool2.Items[_get2[idx]];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsComponentRef<Inc1> Get1Ref(in int idx) =>
            _pool1.Ref(_get1[idx]);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsComponentRef<Inc2> Get2Ref(in int idx) =>
            _pool2.Ref(_get2[idx]);

#if UNITY_2019_1_OR_NEWER
        [UnityEngine.Scripting.Preserve]
#endif
        protected EcsFilter(EcsWorld world) : base(world)
        {
            _allow1 = !EcsComponentType<Inc1>.IsIgnoreInFilter;
            _allow2 = !EcsComponentType<Inc2>.IsIgnoreInFilter;
            _pool1 = world.GetPool<Inc1>();
            _pool2 = world.GetPool<Inc2>();
            _get1 = _allow1 ? new int[EntitiesCacheSize] : null;
            _get2 = _allow2 ? new int[EntitiesCacheSize] : null;
            IncludedTypeIndices = new[] {
                EcsComponentType<Inc1>.TypeIndex,
                EcsComponentType<Inc2>.TypeIndex
            };
            IncludedTypes = new[] {
                EcsComponentType<Inc1>.Type,
                EcsComponentType<Inc2>.Type
            };
        }

        /// <summary>
        /// Event for adding compatible entity to filter.
        /// Warning: Don't call manually!
        /// </summary>
        /// <param name="entity">Entity.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void OnAddEntity(in EcsEntity entity)
        {
            if (AddDelayedOp(true, entity)) { return; }
            if (Entities.Length == EntitiesCount)
            {
                var newSize = EntitiesCount << 1;
                Array.Resize(ref Entities, newSize);
                if (_allow1) { Array.Resize(ref _get1, newSize); }
                if (_allow2) { Array.Resize(ref _get2, newSize); }
            }
            // inlined and optimized EcsEntity.Get() call.
            ref var entityData = ref entity.Owner.GetEntityData(entity);
            var allow1 = _allow1;
            var allow2 = _allow2;
            for (int i = 0, iMax = entityData.ComponentsCountX2, left = 2; left > 0 && i < iMax; i += 2)
            {
                var typeIdx = entityData.Components[i];
                var itemIdx = entityData.Components[i + 1];
                if (allow1 && typeIdx == EcsComponentType<Inc1>.TypeIndex)
                {
                    _get1[EntitiesCount] = itemIdx;
                    allow1 = false;
                    left--;
                    continue;
                }
                if (allow2 && typeIdx == EcsComponentType<Inc2>.TypeIndex)
                {
                    _get2[EntitiesCount] = itemIdx;
                    allow2 = false;
                    left--;
                }
            }
            EntitiesMap[entity.GetInternalId()] = EntitiesCount;
            Entities[EntitiesCount++] = entity;
#if LEOECS_FILTER_EVENTS
            ProcessListeners (true, entity);
#endif
        }

        /// <summary>
        /// Event for removing non-compatible entity to filter.
        /// Warning: Don't call manually!
        /// </summary>
        /// <param name="entity">Entity.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void OnRemoveEntity(in EcsEntity entity)
        {
            if (AddDelayedOp(false, entity)) { return; }
            var entityId = entity.GetInternalId();
            var idx = EntitiesMap[entityId];
            EntitiesMap.Remove(entityId);
            EntitiesCount--;
            if (idx < EntitiesCount)
            {
                Entities[idx] = Entities[EntitiesCount];
                EntitiesMap[Entities[idx].GetInternalId()] = idx;
                if (_allow1) { _get1[idx] = _get1[EntitiesCount]; }
                if (_allow2) { _get2[idx] = _get2[EntitiesCount]; }
            }
#if LEOECS_FILTER_EVENTS
            ProcessListeners (false, entity);
#endif
        }

        public class Exclude<Exc1> : EcsFilter<Inc1, Inc2>
            where Exc1 : struct
        {
#if UNITY_2019_1_OR_NEWER
            [UnityEngine.Scripting.Preserve]
#endif
            protected Exclude(EcsWorld world) : base(world)
            {
                ExcludedTypeIndices = new[] {
                    EcsComponentType<Exc1>.TypeIndex
                };
                ExcludedTypes = new[] {
                    EcsComponentType<Exc1>.Type
                };
            }
        }

        public class Exclude<Exc1, Exc2> : EcsFilter<Inc1, Inc2>
            where Exc1 : struct
            where Exc2 : struct
        {
#if UNITY_2019_1_OR_NEWER
            [UnityEngine.Scripting.Preserve]
#endif
            protected Exclude(EcsWorld world) : base(world)
            {
                ExcludedTypeIndices = new[] {
                    EcsComponentType<Exc1>.TypeIndex,
                    EcsComponentType<Exc2>.TypeIndex
                };
                ExcludedTypes = new[] {
                    EcsComponentType<Exc1>.Type,
                    EcsComponentType<Exc2>.Type
                };
            }
        }
    }
}
