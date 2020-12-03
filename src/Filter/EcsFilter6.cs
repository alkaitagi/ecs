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
    public class EcsFilter<Inc1, Inc2, Inc3, Inc4, Inc5, Inc6> : EcsFilter
        where Inc1 : struct
        where Inc2 : struct
        where Inc3 : struct
        where Inc4 : struct
        where Inc5 : struct
        where Inc6 : struct
    {
        int[] _get1;
        int[] _get2;
        int[] _get3;
        int[] _get4;
        int[] _get5;
        int[] _get6;

        readonly bool _allow1;
        readonly bool _allow2;
        readonly bool _allow3;
        readonly bool _allow4;
        readonly bool _allow5;
        readonly bool _allow6;

        readonly EcsComponentPool<Inc1> _pool1;
        readonly EcsComponentPool<Inc2> _pool2;
        readonly EcsComponentPool<Inc3> _pool3;
        readonly EcsComponentPool<Inc4> _pool4;
        readonly EcsComponentPool<Inc5> _pool5;
        readonly EcsComponentPool<Inc6> _pool6;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref Inc1 Get1(in int idx) =>
            ref _pool1.Items[_get1[idx]];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref Inc2 Get2(in int idx) =>
            ref _pool2.Items[_get2[idx]];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref Inc3 Get3(in int idx) =>
            ref _pool3.Items[_get3[idx]];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref Inc4 Get4(in int idx) =>
            ref _pool4.Items[_get4[idx]];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref Inc5 Get5(in int idx) =>
            ref _pool5.Items[_get5[idx]];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref Inc6 Get6(in int idx) =>
            ref _pool6.Items[_get6[idx]];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsComponentRef<Inc1> Get1Ref(in int idx) =>
            _pool1.Ref(_get1[idx]);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsComponentRef<Inc2> Get2Ref(in int idx) =>
            _pool2.Ref(_get2[idx]);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsComponentRef<Inc3> Get3Ref(in int idx) =>
            _pool3.Ref(_get3[idx]);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsComponentRef<Inc4> Get4Ref(in int idx) =>
            _pool4.Ref(_get4[idx]);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsComponentRef<Inc5> Get5Ref(in int idx) =>
            _pool5.Ref(_get5[idx]);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsComponentRef<Inc6> Get6Ref(in int idx) =>
            _pool6.Ref(_get6[idx]);

#if UNITY_2019_1_OR_NEWER
        [UnityEngine.Scripting.Preserve]
#endif
        protected EcsFilter(EcsWorld world) : base(world)
        {
            _allow1 = !EcsComponentType<Inc1>.IsIgnoreInFilter;
            _allow2 = !EcsComponentType<Inc2>.IsIgnoreInFilter;
            _allow3 = !EcsComponentType<Inc3>.IsIgnoreInFilter;
            _allow4 = !EcsComponentType<Inc4>.IsIgnoreInFilter;
            _allow5 = !EcsComponentType<Inc5>.IsIgnoreInFilter;
            _allow6 = !EcsComponentType<Inc6>.IsIgnoreInFilter;
            _pool1 = world.GetPool<Inc1>();
            _pool2 = world.GetPool<Inc2>();
            _pool3 = world.GetPool<Inc3>();
            _pool4 = world.GetPool<Inc4>();
            _pool5 = world.GetPool<Inc5>();
            _pool6 = world.GetPool<Inc6>();
            _get1 = _allow1 ? new int[EntitiesCacheSize] : null;
            _get2 = _allow2 ? new int[EntitiesCacheSize] : null;
            _get3 = _allow3 ? new int[EntitiesCacheSize] : null;
            _get4 = _allow4 ? new int[EntitiesCacheSize] : null;
            _get5 = _allow5 ? new int[EntitiesCacheSize] : null;
            _get6 = _allow6 ? new int[EntitiesCacheSize] : null;
            IncludedTypeIndices = new[] {
                EcsComponentType<Inc1>.TypeIndex,
                EcsComponentType<Inc2>.TypeIndex,
                EcsComponentType<Inc3>.TypeIndex,
                EcsComponentType<Inc4>.TypeIndex,
                EcsComponentType<Inc5>.TypeIndex,
                EcsComponentType<Inc6>.TypeIndex
            };
            IncludedTypes = new[] {
                EcsComponentType<Inc1>.Type,
                EcsComponentType<Inc2>.Type,
                EcsComponentType<Inc3>.Type,
                EcsComponentType<Inc4>.Type,
                EcsComponentType<Inc5>.Type,
                EcsComponentType<Inc6>.Type
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
                if (_allow3) { Array.Resize(ref _get3, newSize); }
                if (_allow4) { Array.Resize(ref _get4, newSize); }
                if (_allow5) { Array.Resize(ref _get5, newSize); }
                if (_allow6) { Array.Resize(ref _get6, newSize); }
            }
            // inlined and optimized EcsEntity.Get() call.
            ref var entityData = ref entity.Owner.GetEntityData(entity);
            var allow1 = _allow1;
            var allow2 = _allow2;
            var allow3 = _allow3;
            var allow4 = _allow4;
            var allow5 = _allow5;
            var allow6 = _allow6;
            for (int i = 0, iMax = entityData.ComponentsCountX2, left = 6; left > 0 && i < iMax; i += 2)
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
                    continue;
                }
                if (allow3 && typeIdx == EcsComponentType<Inc3>.TypeIndex)
                {
                    _get3[EntitiesCount] = itemIdx;
                    allow3 = false;
                    left--;
                    continue;
                }
                if (allow4 && typeIdx == EcsComponentType<Inc4>.TypeIndex)
                {
                    _get4[EntitiesCount] = itemIdx;
                    allow4 = false;
                    left--;
                    continue;
                }
                if (allow5 && typeIdx == EcsComponentType<Inc5>.TypeIndex)
                {
                    _get5[EntitiesCount] = itemIdx;
                    allow5 = false;
                    left--;
                    continue;
                }
                if (allow6 && typeIdx == EcsComponentType<Inc6>.TypeIndex)
                {
                    _get6[EntitiesCount] = itemIdx;
                    allow6 = false;
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
                if (_allow3) { _get3[idx] = _get3[EntitiesCount]; }
                if (_allow4) { _get4[idx] = _get4[EntitiesCount]; }
                if (_allow5) { _get5[idx] = _get5[EntitiesCount]; }
                if (_allow6) { _get6[idx] = _get6[EntitiesCount]; }
            }
#if LEOECS_FILTER_EVENTS
            ProcessListeners (false, entity);
#endif
        }

        public class Exclude<Exc1> : EcsFilter<Inc1, Inc2, Inc3, Inc4, Inc5, Inc6>
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

        public class Exclude<Exc1, Exc2> : EcsFilter<Inc1, Inc2, Inc3, Inc4, Inc5, Inc6>
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
