using System;

namespace ManagedDoomCodegen
{
    class Program
    {
        static void Main(string[] args)
        {
            GenState.Run();
            GenDoomInfoStates.Run();
            GenSfx.Run();
            GenMobjType.Run();
            GenDoomInfoMobjInfos.Run();
            GenDoomInfoPlayerActions.Run();
            GenDoomInfoMobjActions.Run();
            GenDoomInfoSpriteNames.Run();
            GenDoomInfoSfxSfxNames.Run();
            GenDoomInfoSwitchNames.Run();
            GenDoomKeys.Run();
            GenDoomKeysEx.Run();
        }
    }
}
