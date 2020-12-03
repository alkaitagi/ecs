// ----------------------------------------------------------------------------
// The MIT License
// Simple Entity Component System framework https://github.com/Leopotam/ecs
// Copyright (c) 2017-2020 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// ReSharper disable InconsistentNaming
// ReSharper disable ClassNeverInstantiated.Global

namespace Leopotam.Ecs
{
    /// <summary>
    /// Container for filtered entities based on specified constraints.
    /// </summary>
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
#if UNITY_2019_1_OR_NEWER
    [UnityEngine.Scripting.Preserve]
#endif
    public abstract partial class EcsFilter
    {
        protected EcsEntity[] Entities;
        protected readonly Dictionary<int, int> EntitiesMap;
        protected int EntitiesCount;
        protected int LockCount;
        protected readonly int EntitiesCacheSize;

        DelayedOp[] _delayedOps;
        int _delayedOpsCount;
#if LEOECS_FILTER_EVENTS
        protected IEcsFilterListener[] Listeners = new IEcsFilterListener[4];
        protected int ListenersCount;
#endif
        protected internal int[] IncludedTypeIndices;
        protected internal int[] ExcludedTypeIndices;

        public Type[] IncludedTypes;
        public Type[] ExcludedTypes;
#if UNITY_2019_1_OR_NEWER
        [UnityEngine.Scripting.Preserve]
#endif
        protected EcsFilter(EcsWorld world)
        {
            EntitiesCacheSize = world.Config.FilterEntitiesCacheSize;
            Entities = new EcsEntity[EntitiesCacheSize];
            EntitiesMap = new Dictionary<int, int>(EntitiesCacheSize);
            _delayedOps = new DelayedOp[EntitiesCacheSize];
        }
#if DEBUG
        public Dictionary<int, int> GetInternalEntitiesMap() =>
            EntitiesMap;
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator() =>
            new Enumerator(this);

        /// <summary>
        /// Gets entity by index.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref EcsEntity GetEntity(in int idx) =>
            ref Entities[idx];

        /// <summary>
        /// Gets entities count.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetEntitiesCount() =>
            EntitiesCount;

        /// <summary>
        /// Is filter not contains entities.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsEmpty() =>
            EntitiesCount == 0;

#if LEOECS_FILTER_EVENTS
        /// <summary>
        /// Subscribes listener to filter events.
        /// </summary>
        /// <param name="listener">Listener.</param>
        public void AddListener (IEcsFilterListener listener) {
#if DEBUG
            for (int i = 0, iMax = ListenersCount; i < iMax; i++) 
                if (Listeners[i] == listener) 
                    throw new Exception ("Listener already subscribed.");
            
#endif
            if (Listeners.Length == ListenersCount) 
                Array.Resize (ref Listeners, ListenersCount << 1);
            
            Listeners[ListenersCount++] = listener;
        }

        // ReSharper disable once CommentTypo
        /// <summary>
        /// Unsubscribes listener from filter events.
        /// </summary>
        /// <param name="listener">Listener.</param>
        public void RemoveListener (IEcsFilterListener listener) {
            for (int i = 0, iMax = ListenersCount; i < iMax; i++)
                if (Listeners[i] == listener) {
                    ListenersCount--;
                    // cant fill gap with last element due listeners order is important.
                    Array.Copy (Listeners, i + 1, Listeners, i, ListenersCount - i);
                    break;
                }
        }
#endif
        /// <summary>
        /// Is filter compatible with components on entity with optional added / removed component.
        /// </summary>
        /// <param name="entityData">Entity data.</param>
        /// <param name="addedRemovedTypeIndex">Optional added (greater 0) or removed (less 0) component. Will be ignored if zero.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool IsCompatible(in EcsWorld.EcsEntityData entityData, int addedRemovedTypeIndex)
        {
            var incIdx = IncludedTypeIndices.Length - 1;
            for (; incIdx >= 0; incIdx--)
            {
                var typeIdx = IncludedTypeIndices[incIdx];
                var idx = entityData.ComponentsCountX2 - 2;
                for (; idx >= 0; idx -= 2)
                {
                    var typeIdx2 = entityData.Components[idx];
                    if (typeIdx2 == -addedRemovedTypeIndex)
                        continue;
                    if (typeIdx2 == addedRemovedTypeIndex || typeIdx2 == typeIdx)
                        break;
                }
                // not found.
                if (idx == -2)
                    break;
            }
            // one of required component not found.
            if (incIdx != -1)
                return false;
            // check for excluded components.
            if (ExcludedTypeIndices != null)
                for (var excIdx = 0; excIdx < ExcludedTypeIndices.Length; excIdx++)
                {
                    var typeIdx = ExcludedTypeIndices[excIdx];
                    for (var idx = entityData.ComponentsCountX2 - 2; idx >= 0; idx -= 2)
                    {
                        var typeIdx2 = entityData.Components[idx];
                        if (typeIdx2 == -addedRemovedTypeIndex)
                            continue;
                        if (typeIdx2 == addedRemovedTypeIndex || typeIdx2 == typeIdx)
                            return false;
                    }
                }
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool AddDelayedOp(bool isAdd, in EcsEntity entity)
        {
            if (LockCount <= 0)
                return false;
            if (_delayedOps.Length == _delayedOpsCount)
                Array.Resize(ref _delayedOps, _delayedOpsCount << 1);
            ref var op = ref _delayedOps[_delayedOpsCount++];
            op.IsAdd = isAdd;
            op.Entity = entity;
            return true;
        }
#if LEOECS_FILTER_EVENTS
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        protected void ProcessListeners (bool isAdd, in EcsEntity entity) {
            if (isAdd) 
                for (int i = 0, iMax = ListenersCount; i < iMax; i++) 
                    Listeners[i].OnEntityAdded (entity);
             else 
                for (int i = 0, iMax = ListenersCount; i < iMax; i++) 
                    Listeners[i].OnEntityRemoved (entity);
        }
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Lock() =>
            LockCount++;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Unlock()
        {
#if DEBUG
            if (LockCount <= 0)
                throw new Exception($"Invalid lock-unlock balance for \"{GetType().Name}\".");
#endif
            LockCount--;
            if (LockCount == 0 && _delayedOpsCount > 0)
            {
                // process delayed operations.
                for (int i = 0, iMax = _delayedOpsCount; i < iMax; i++)
                {
                    ref var op = ref _delayedOps[i];
                    if (op.IsAdd)
                        OnAddEntity(op.Entity);
                    else
                        OnRemoveEntity(op.Entity);
                }
                _delayedOpsCount = 0;
            }
        }

#if DEBUG
        /// <summary>
        /// For debug purposes. Check filters equality by included / excluded components.
        /// </summary>
        /// <param name="filter">Filter to compare.</param>
        internal bool AreComponentsSame(EcsFilter filter)
        {
            if (IncludedTypeIndices.Length != filter.IncludedTypeIndices.Length)
                return false;
            for (var i = 0; i < IncludedTypeIndices.Length; i++)
                if (Array.IndexOf(filter.IncludedTypeIndices, IncludedTypeIndices[i]) == -1)
                    return false;
            if ((ExcludedTypeIndices == null && filter.ExcludedTypeIndices != null) ||
                (ExcludedTypeIndices != null && filter.ExcludedTypeIndices == null))
                return false;
            if (ExcludedTypeIndices != null)
            {
                if (filter.ExcludedTypeIndices == null || ExcludedTypeIndices.Length != filter.ExcludedTypeIndices.Length)
                    return false;
                for (var i = 0; i < ExcludedTypeIndices.Length; i++)
                    if (Array.IndexOf(filter.ExcludedTypeIndices, ExcludedTypeIndices[i]) == -1)
                        return false;
            }
            return true;
        }
#endif

        /// <summary>
        /// Event for adding compatible entity to filter.
        /// Warning: Don't call manually!
        /// </summary>
        /// <param name="entity">Entity.</param>
        public abstract void OnAddEntity(in EcsEntity entity);

        /// <summary>
        /// Event for removing non-compatible entity to filter.
        /// Warning: Don't call manually!
        /// </summary>
        /// <param name="entity">Entity.</param>
        public abstract void OnRemoveEntity(in EcsEntity entity);
    }
}
