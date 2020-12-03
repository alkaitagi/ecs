// ----------------------------------------------------------------------------
// The MIT License
// Simple Entity Component System framework https://github.com/Leopotam/ecs
// Copyright (c) 2017-2020 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;

namespace Leopotam.Ecs
{
    /// <summary>
    /// Fast List replacement for growing only collections.
    /// </summary>
    /// <typeparam name="T">Type of item.</typeparam>
    public class EcsGrowList<T>
    {
        public T[] Items;
        public int Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsGrowList(int capacity)
        {
            Items = new T[capacity];
            Count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T item)
        {
            if (Items.Length == Count)
                Array.Resize(ref Items, Items.Length << 1);

            Items[Count++] = item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnsureCapacity(int count)
        {
            if (Items.Length < count)
            {
                var len = Items.Length << 1;
                while (len <= count)
                    len <<= 1;
                Array.Resize(ref Items, len);
            }
        }
    }
}
