// ----------------------------------------------------------------------------
// The MIT License
// Simple Entity Component System framework https://github.com/Leopotam/ecs
// Copyright (c) 2017-2020 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

namespace Leopotam.Ecs
{
    /// <summary>
    /// World config to setup default caches.
    /// </summary>
    public struct EcsWorldConfig
    {
        /// <summary>
        /// World.Entities cache size.
        /// </summary>
        public int WorldEntitiesCacheSize;
        /// <summary>
        /// World.Filters cache size.
        /// </summary>
        public int WorldFiltersCacheSize;
        /// <summary>
        /// World.ComponentPools cache size.
        /// </summary>
        public int WorldComponentPoolsCacheSize;
        /// <summary>
        /// Entity.Components cache size (not doubled).
        /// </summary>
        public int EntityComponentsCacheSize;
        /// <summary>
        /// Filter.Entities cache size.
        /// </summary>
        public int FilterEntitiesCacheSize;
        /// <summary>
        /// World.Entities default cache size.
        /// </summary>
        public const int DefaultWorldEntitiesCacheSize = 1024;
        /// <summary>
        /// World.Filters default cache size.
        /// </summary>
        public const int DefaultWorldFiltersCacheSize = 128;
        /// <summary>
        /// World.ComponentPools default cache size.
        /// </summary>
        public const int DefaultWorldComponentPoolsCacheSize = 512;
        /// <summary>
        /// Entity.Components default cache size (not doubled).
        /// </summary>
        public const int DefaultEntityComponentsCacheSize = 8;
        /// <summary>
        /// Filter.Entities default cache size.
        /// </summary>
        public const int DefaultFilterEntitiesCacheSize = 256;
    }
}
