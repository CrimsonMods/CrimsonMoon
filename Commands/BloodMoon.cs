using ProjectM;
using VampireCommandFramework;

namespace CrimsonMoon.Commands;

[CommandGroup("crimsonmoon", "cmoon")]
internal class BloodMoon
{
    [Command("trigger", "t", description: "Forces next moon to be blood", adminOnly: true)]
    public static void TriggerBloodMoon(ChatCommandContext ctx)
    {
        var settings = Core.Server.GetExistingSystemManaged<DebugEventsSystem>();
        settings.JumpToNextBloodMoon();

        ctx.Reply($"Jumping to Next Blood Moon");
    }
}
