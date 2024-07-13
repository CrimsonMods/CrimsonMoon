using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace CrimsonMoon.Structs;

public readonly struct Settings
{
    internal readonly ConfigFile CONFIG;
    internal readonly ConfigEntry<bool> ENABLE_MOD;
    internal readonly ConfigEntry<double> ALTER_LOCATION_X;
    internal readonly ConfigEntry<double> ALTER_LOCATION_Y;
    internal readonly ConfigEntry<double> ALTER_LOCATION_Z;
    internal readonly ConfigEntry<int> SACRAFICE_GUID;
    internal readonly ConfigEntry<int> MIN_AMOUNT;

    public Settings(ConfigFile config)
    { 
        CONFIG = config;
        ENABLE_MOD = config.Bind("Config", "EnableMod", true, "Enable or disable the mod");
        
        SACRAFICE_GUID = config.Bind("Sacrafice", "SacraficeGUID", 1566989408, "ItemHash for the sacraficed material");
        MIN_AMOUNT = config.Bind("Sacrafice", "MinimumRequired", 3, "This is how many must be dropped to trigger blood moon");

        ALTER_LOCATION_X = config.Bind("Alter", "AlterLocation_X", -1585.3368, "The float3 position of the alter, accepts sacrafices within 10 distance");
        ALTER_LOCATION_Y = config.Bind("Alter", "AlterLocation_Y", -3.8146);
        ALTER_LOCATION_Z = config.Bind("Alter", "AlterLocation_Z", -937.0102);
    }
}
