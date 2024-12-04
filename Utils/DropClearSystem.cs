using Il2CppInterop.Runtime;
using ProjectM;
using ProjectM.Shared;
using Stunlock.Core;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace CrimsonMoon.Utils;

internal class DropClearSystem
{
    EntityQuery dropItemQuery;

    public DropClearSystem()
    {
        EntityQueryDesc dropItemQueryDesc = new()
        {
            All = new ComponentType[] {
                new(Il2CppType.Of<ItemPickup>(), ComponentType.AccessMode.ReadWrite),
                new(Il2CppType.Of<PrefabGUID>(), ComponentType.AccessMode.ReadWrite),
                new(Il2CppType.Of<Translation>(), ComponentType.AccessMode.ReadWrite),
                new(Il2CppType.Of<DestroyWhenNoCharacterNearbyAfterDuration>(), ComponentType.AccessMode.ReadWrite),

            },
            None = new ComponentType[]
                {
                new(Il2CppType.Of<AttachedBuffer>(), ComponentType.AccessMode.ReadOnly),
                },
            Options = EntityQueryOptions.IncludeDisabled
        };

        dropItemQuery = Core.Server.EntityManager.CreateEntityQuery(dropItemQueryDesc.All);
    }

    IEnumerable<Entity> GetDropItems()
    {
        var entities = dropItemQuery.ToEntityArray(Allocator.Temp);
        foreach (var entity in entities)
        {
            var prefabGuid = entity.Read<PrefabGUID>();
            yield return entity;
        }
        entities.Dispose();
    }

    int ClearDropItems(Func<Entity, bool> shouldClear = null)
    {
        var count = 0;
        foreach (var entity in GetDropItems())
        {
            if (shouldClear != null && !shouldClear(entity)) continue;
            if (Core.Server.EntityManager.TryGetBuffer<InventoryBuffer>(entity, out var inventory))
            {
                foreach (var item in inventory)
                {
                    if (item.ItemEntity.GetEntityOnServer() == Entity.Null) continue;
                    DestroyUtility.Destroy(Core.Server.EntityManager, item.ItemEntity.GetEntityOnServer());
                }
            }

            DestroyUtility.Destroy(Core.Server.EntityManager, entity);
            count++;
        }
        return count;
    }

    public int ClearDropItemsInRadius(float3 pos, float radius)
    {
        var posXZ = pos.xz;
        return ClearDropItems(entity =>
        {
            var translation = entity.Read<Translation>();
            return math.distance(posXZ, translation.Value.xz) <= radius;
        });
    }
}