// ----------------------------------------------------------------------------
// The MIT License
// Simple Entity Component System framework https://github.com/Leopotam/ecs
// Copyright (c) 2017-2020 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;

namespace Leopotam.Ecs
{
    public interface IEcsComponentPool
    {
        Type ItemType { get; }
        object GetItem(int idx);
        void Recycle(int idx);
        int New();
        void CopyData(int srcIdx, int dstIdx);
    }
}
