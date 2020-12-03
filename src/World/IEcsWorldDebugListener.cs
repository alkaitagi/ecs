// ----------------------------------------------------------------------------
// The MIT License
// Simple Entity Component System framework https://github.com/Leopotam/ecs
// Copyright (c) 2017-2020 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

namespace Leopotam.Ecs
{
#if DEBUG
    /// <summary>
    /// Debug interface for world events processing.
    /// </summary>
    public interface IEcsWorldDebugListener
    {
        void OnEntityCreated(EcsEntity entity);
        void OnEntityDestroyed(EcsEntity entity);
        void OnFilterCreated(EcsFilter filter);
        void OnComponentListChanged(EcsEntity entity);
        void OnWorldDestroyed(EcsWorld world);
    }
#endif
}
