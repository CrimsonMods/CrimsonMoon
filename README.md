# CrimsonMoon
`Server side only`

Allows the players the ability to trigger Blood Moon to take place by sacraficing a set number of a specified item within a range of a specific location.

By default, it's 3 Primal Blood Essence at the Sacraficial Alter.

You can find new coordinates using KindredCommands' "whereami" command.

## Installation
* Install [BepInEx](https://v-rising.thunderstore.io/package/BepInEx/BepInExPack_V_Rising/)
* Install [VampireCommandFramework](https://thunderstore.io/c/v-rising/p/deca/VampireCommandFramework/)
* Extract _CrimsonMoon.dll_ into _(VRising server folder)/BepInEx/plugins_

## Configurable Values
```ini
[Config]

## Enable or disable the mod
# Setting type: Boolean
# Default value: true
EnableMod = true

[Sacrafice]

## ItemHash for the sacraficed material
# Setting type: int
# Default value: 1566989408
SacraficeGUID = 1566989408

## This is how many must be dropped to trigger blood moon
# Setting type: int
# Default value: 3
MinimumRequired = 3

[Alter]

## The float3 position of the alter, accepts sacrafices within 10 distance 
# Setting type: double
# Default value: -1585.3368
AlterLocation_X = -1585.3368
AlterLocation_Y = -3.8146
AlterLocation_Z = -937.0102
```
## Support

Want to support my V Rising Mod development? 

Donations Accepted with [Ko-Fi](https://ko-fi.com/skytech6)

Or buy/play my games! 

[Train Your Minibot](https://store.steampowered.com/app/713740/Train_Your_Minibot/) 

[Boring Movies](https://store.steampowered.com/app/1792500/Boring_Movies/) **FREE TO PLAY**

**I do commission mod work. Contact me on Discord (skytech6) for any Unity modding needs!**