// ----------------------------------------------------------------------------
// The MIT License
// Simple Entity Component System framework https://github.com/Leopotam/ecs
// Copyright (c) 2017-2020 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace Leopotam.Ecs
{
    public static partial class EcsEntityExtensions
    {
        /// <summary>
        /// Gets internal identifier.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetInternalId(in this EcsEntity entity) =>
            entity.Id;

        /// <summary>
        /// Gets internal generation.
        /// </summary>
        public static int GetInternalGen(in this EcsEntity entity) =>
            entity.Gen;
    }
}
