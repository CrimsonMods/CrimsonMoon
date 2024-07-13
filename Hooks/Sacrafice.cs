using Bloodstone.API;
using HarmonyLib;
using ProjectM;
using Stunlock.Core;
using System;
using System.IO;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using CrimsonMoon.Utils;
using System.Collections;

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
                if (VWorld.Server.EntityManager.TryGetComponentData<DropItemAroundPosition>(entity, out DropItemAroundPosition item))
                {
                    if (item.ItemHash == new PrefabGUID(Plugin.Settings.SACRAFICE_GUID.Value) && item.Amount >= Plugin.Settings.MIN_AMOUNT.Value)
                    {
                        Vector3 pointA = new Vector3(item.Position.x, item.Position.y, item.Position.z);
                        Vector3 pointB = new Vector3((float)Plugin.Settings.ALTER_LOCATION_X.Value, (float)Plugin.Settings.ALTER_LOCATION_Y.Value, (float)Plugin.Settings.ALTER_LOCATION_Z.Value);
                        var distance = Vector3.Distance(pointA, pointB);

                        if (distance < 10)
                        {
                            ServerChatUtils.SendSystemMessageToAllClients(VWorld.Server.EntityManager, "The Blood Alter runs red!");

                            var eventsSystem = VWorld.Server.GetExistingSystemManaged<DebugEventsSystem>();
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
            ServerChatUtils.SendSystemMessageToAllClients(VWorld.Server.EntityManager, $"Cleared {cleared}");
        }

        public static void EntityCompomponentDumper(string filePath, Entity entity)
        {
            File.AppendAllText(filePath, $"--------------------------------------------------" + Environment.NewLine);
            File.AppendAllText(filePath, $"Dumping components of {entity.ToString()}:" + Environment.NewLine);

            foreach (var componentType in VWorld.Server.EntityManager.GetComponentTypes(entity))
            { File.AppendAllText(filePath, $"{componentType.ToString()}" + Environment.NewLine); }

            File.AppendAllText(filePath, $"--------------------------------------------------" + Environment.NewLine);

            File.AppendAllText(filePath, DumpEntity(entity));
        }

        private static string DumpEntity(Entity entity, bool fullDump = true)
        {
            var sb = new Il2CppSystem.Text.StringBuilder();
            ProjectM.EntityDebuggingUtility.DumpEntity(VWorld.Server, entity, fullDump, sb);
            return sb.ToString();
        }
    }

}
