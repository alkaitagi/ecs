// ----------------------------------------------------------------------------
// The MIT License
// Simple Entity Component System framework https://github.com/Leopotam/ecs
// Copyright (c) 2017-2020 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

namespace Leopotam.Ecs
{
    /// <summary>
    /// System for removing OneFrame component.
    /// </summary>
    /// <typeparam name="T">OneFrame component type.</typeparam>
    sealed class RemoveOneFrame<T> : IEcsRunSystem where T : struct
    {
        readonly EcsFilter<T> _oneFrames = null;

        void IEcsRunSystem.Run()
        {
            for (var idx = _oneFrames.GetEntitiesCount() - 1; idx >= 0; idx--)
                _oneFrames.GetEntity(idx).Del<T>();
        }
    }
}
