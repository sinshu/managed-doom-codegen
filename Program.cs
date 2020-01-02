using System;

namespace ManagedDoomCodegen
{
    class Program
    {
        static void Main(string[] args)
        {
            GenState.Run();
            GenStateDefList.Run();
            GenSfx.Run();
            GenMobjType.Run();
            GenMobjInfoList.Run();
        }
    }
}
