﻿using Il2CppInterop.Runtime;
using ProjectM;
using Stunlock.Core;
using System;
using System.Runtime.InteropServices;
using Unity.Entities;

namespace CrimsonMoon.Utils;

#pragma warning disable CS8500
public static class ECSExtensionService
{
    public unsafe static void Write<T>(this Entity entity, T componentData) where T : struct
    {
        // Get the ComponentType for T
        var ct = new ComponentType(Il2CppType.Of<T>());

        // Marshal the component data to a byte array
        byte[] byteArray = StructureToByteArray(componentData);

        // Get the size of T
        int size = Marshal.SizeOf<T>();

        // Create a pointer to the byte array
        fixed (byte* p = byteArray)
        {
            // Set the component data
            Core.Server.EntityManager.SetComponentDataRaw(entity, ct.TypeIndex, p, size);
        }
    }

    // Helper function to marshal a struct to a byte array
    public static byte[] StructureToByteArray<T>(T structure) where T : struct
    {
        int size = Marshal.SizeOf(structure);
        byte[] byteArray = new byte[size];
        IntPtr ptr = Marshal.AllocHGlobal(size);

        Marshal.StructureToPtr(structure, ptr, true);
        Marshal.Copy(ptr, byteArray, 0, size);
        Marshal.FreeHGlobal(ptr);

        return byteArray;
    }

    public unsafe static T Read<T>(this Entity entity) where T : struct
    {
        // Get the ComponentType for T
        var ct = new ComponentType(Il2CppType.Of<T>());

        // Get a pointer to the raw component data
        void* rawPointer = Core.Server.EntityManager.GetComponentDataRawRO(entity, ct.TypeIndex);

        // Marshal the raw data to a T struct
        T componentData = Marshal.PtrToStructure<T>(new IntPtr(rawPointer));

        return componentData;
    }

    public static DynamicBuffer<T> ReadBuffer<T>(this Entity entity) where T : struct
    {
        return Core.Server.EntityManager.GetBuffer<T>(entity);
    }

    public static void Add<T>(this Entity entity)
    {
        var ct = new ComponentType(Il2CppType.Of<T>());
        Core.Server.EntityManager.AddComponent(entity, ct);
    }

    public static void Remove<T>(this Entity entity)
    {
        var ct = new ComponentType(Il2CppType.Of<T>());
        Core.Server.EntityManager.RemoveComponent(entity, ct);
    }

    public static bool Has<T>(this Entity entity)
    {
        var ct = new ComponentType(Il2CppType.Of<T>());
        return Core.Server.EntityManager.HasComponent(entity, ct);
    }

    public static void LogComponentTypes(this Entity entity)
    {
        var comps = Core.Server.EntityManager.GetComponentTypes(entity);
        foreach (var comp in comps)
        {
            Plugin.LogInstance.LogInfo($"{comp}");
        }
        Plugin.LogInstance.LogInfo("===");
    }

    public static void LogComponentTypes(this EntityQuery entityQuery)
    {
        var types = entityQuery.GetQueryTypes();
        foreach (var t in types)
        {
            Plugin.LogInstance.LogInfo($"Query Component Type: {t}");
        }
        Plugin.LogInstance.LogInfo($"===");
    }

    public static string LookupName(this PrefabGUID prefabGuid)
    {
        var prefabCollectionSystem = Core.Server.GetExistingSystemManaged<PrefabCollectionSystem>();
        return (prefabCollectionSystem.PrefabGuidToNameDictionary.ContainsKey(prefabGuid)
            ? prefabCollectionSystem.PrefabGuidToNameDictionary[prefabGuid] + " " + prefabGuid : "GUID Not Found").ToString();
    }
}
#pragma warning restore CS8500

