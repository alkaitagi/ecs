// ----------------------------------------------------------------------------
// The MIT License
// Simple Entity Component System framework https://github.com/Leopotam/ecs
// Copyright (c) 2017-2020 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;

namespace Leopotam.Ecs
{
    public sealed class EcsComponentPool
    {
        /// <summary>
        /// Global component type counter.
        /// First component will be "1" for correct filters updating (add component on positive and remove on negative).
        /// </summary>
        internal static int ComponentTypesCount;
    }

#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public sealed class EcsComponentPool<T> : IEcsComponentPool where T : struct
    {
        delegate void AutoResetHandler(ref T component);

        public Type ItemType { get; }
        public T[] Items = new T[128];
        int[] _reservedItems = new int[128];
        int _itemsCount;
        int _reservedItemsCount;
        readonly AutoResetHandler _autoReset;
#if ENABLE_IL2CPP && !UNITY_EDITOR
        T _autoresetFakeInstance;
#endif

        internal EcsComponentPool()
        {
            ItemType = typeof(T);
            if (EcsComponentType<T>.IsAutoReset)
            {
                var autoResetMethod = typeof(T).GetMethod(nameof(IEcsAutoReset<T>.AutoReset));
#if DEBUG

                if (autoResetMethod == null)
                    throw new Exception(
                        $"IEcsAutoReset<{typeof(T).Name}> explicit implementation not supported, use implicit instead.");

#endif
                _autoReset = (AutoResetHandler)Delegate.CreateDelegate(
                    typeof(AutoResetHandler),
#if ENABLE_IL2CPP && !UNITY_EDITOR
                    _autoresetFakeInstance,
#else
                    null,
#endif
                    autoResetMethod);
            }
        }

        /// <summary>
        /// Sets new capacity (if more than current amount).
        /// </summary>
        /// <param name="capacity">New value.</param>
        public void SetCapacity(int capacity)
        {
            if (capacity > Items.Length)
                Array.Resize(ref Items, capacity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int New()
        {
            int id;
            if (_reservedItemsCount > 0)
                id = _reservedItems[--_reservedItemsCount];
            else
            {
                id = _itemsCount;
                if (_itemsCount == Items.Length)
                    Array.Resize(ref Items, _itemsCount << 1);
                // reset brand new instance if custom AutoReset was registered.
                _autoReset?.Invoke(ref Items[_itemsCount]);
                _itemsCount++;
            }
            return id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetItem(int idx) =>
            ref Items[idx];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Recycle(int idx)
        {
            if (_autoReset != null)
                _autoReset(ref Items[idx]);
            else
                Items[idx] = default;
            if (_reservedItemsCount == _reservedItems.Length)
                Array.Resize(ref _reservedItems, _reservedItemsCount << 1);
            _reservedItems[_reservedItemsCount++] = idx;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyData(int srcIdx, int dstIdx) =>
            Items[dstIdx] = Items[srcIdx];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsComponentRef<T> Ref(int idx)
        {
            EcsComponentRef<T> componentRef;
            componentRef.Pool = this;
            componentRef.Idx = idx;
            return componentRef;
        }

        object IEcsComponentPool.GetItem(int idx) =>
            Items[idx];
    }
}
