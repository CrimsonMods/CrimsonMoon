using BepInEx.Unity.IL2CPP.Utils.Collections;
using CrimsonMoon.Utils;
using ProjectM.Physics;
using System.Collections;
using System.Linq;
using Unity.Entities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CrimsonMoon;

internal static class Core
{
    public static World Server { get; } = GetServerWorld() ?? throw new System.Exception("There is no Server world...");
    public static DropClearSystem DropItem { get; internal set; }
    public static bool _hasInitialized = false;

    static MonoBehaviour mono;

    internal static void InitializeAfterLoaded()
    { 
        if(_hasInitialized) return;

        DropItem = new DropClearSystem();
        _hasInitialized = true;
    }

    public static Coroutine StartCoroutine(IEnumerator routine)
    {
        if (mono == null)
        {
            var go = new GameObject("CrimsonMoon");
            mono = go.AddComponent<IgnorePhysicsDebugSystem>();
            Object.DontDestroyOnLoad(go);
        }

        return mono.StartCoroutine(routine.WrapToIl2Cpp());
    }

    static World GetServerWorld()
    {
        return World.s_AllWorlds.ToArray().FirstOrDefault(world => world.Name == "Server");
    }
}