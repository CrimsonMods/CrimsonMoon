using HarmonyLib;
using ProjectM;
using Stunlock.Core;
using System.Collections;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace CrimsonMoon.Hooks;

[HarmonyPatch]
internal class Sacrafice
{
    [HarmonyPatch(typeof(DropItemThrowSystem), nameof(DropItemThrowSystem.OnUpdate))]
    public class DropItemThrowSystem_Prefix
    {
        EntityQuery dropItemQuery;

        public static void Prefix(DropItemThrowSystem __instance)
        {
            var entities = __instance.__query_2070481711_0.ToEntityArray(Allocator.TempJob);
            
            foreach(var entity in entities)
            {
                if (Core.Server.EntityManager.TryGetComponentData<DropItemAroundPosition>(entity, out DropItemAroundPosition item))
                {
                    if (item.ItemHash == new PrefabGUID(Plugin.Settings.SACRAFICE_GUID.Value) && item.Amount >= Plugin.Settings.MIN_AMOUNT.Value)
                    {
                        Vector3 pointA = new Vector3(item.Position.x, item.Position.y, item.Position.z);
                        Vector3 pointB = new Vector3((float)Plugin.Settings.ALTER_LOCATION_X.Value, (float)Plugin.Settings.ALTER_LOCATION_Y.Value, (float)Plugin.Settings.ALTER_LOCATION_Z.Value);
                        var distance = Vector3.Distance(pointA, pointB);

                        if (distance < 10)
                        {
                            ServerChatUtils.SendSystemMessageToAllClients(Core.Server.EntityManager, "The Blood Alter runs red!");

                            var eventsSystem = Core.Server.GetExistingSystemManaged<DebugEventsSystem>();
                            eventsSystem.JumpToNextBloodMoon();

                            Core.StartCoroutine(Clear(pointB, 20));
                        }
                    }
                }
            }
        }

        static IEnumerator Clear(Vector3 vector, int radius)
        {
            yield return new WaitForSeconds(1f);

            var cleared = Core.DropItem.ClearDropItemsInRadius(vector, radius);
            ServerChatUtils.SendSystemMessageToAllClients(Core.Server.EntityManager, $"Cleared {cleared}");
        }
    }

}
