// ----------------------------------------------------------------------------
// The MIT License
// Simple Entity Component System framework https://github.com/Leopotam/ecs
// Copyright (c) 2017-2020 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

namespace Leopotam.Ecs
{
    public abstract partial class EcsFilter
    {
        struct DelayedOp
        {
            public bool IsAdd;
            public EcsEntity Entity;
        }
    }
}
