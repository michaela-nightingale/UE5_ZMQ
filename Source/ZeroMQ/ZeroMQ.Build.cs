using UnrealBuildTool;
using System;
using System.IO;
using System.Diagnostics;

public class ZeroMQ : ModuleRules
{
    private string ThirdPartyPath
    {
        get { return Path.GetFullPath(Path.Combine(ModuleDirectory, "..", "..", "ThirdParty")); }
    }

    private string ZeroMQRootPath
    {
        get { return Path.GetFullPath(Path.Combine(ThirdPartyPath, "libzmq_4.3.1")); }
    }

    public void AddZeroMQ(ReadOnlyTargetRules Target)
    {
        // Add include paths
        PublicIncludePaths.Add(Path.Combine(ZeroMQRootPath, "include"));

        // Define that we're linking the static library
        PublicDefinitions.Add("ZMQ_STATIC");

        string staticLibrary = "";
        switch (Target.Platform)
        {
            case UnrealTargetPlatform.Win64:  // Use Win64 instead of Win32
                staticLibrary = Path.Combine(ZeroMQRootPath, "Windows", "x64", "libzmq-v141-mt-s-4_3_2.lib");
                break;
            case UnrealTargetPlatform.Linux:
                staticLibrary = Path.Combine(ZeroMQRootPath, "Linux", "libzmq.so");
                PublicAdditionalLibraries.Add("stdc++");
                break;
            case UnrealTargetPlatform.Mac:
                staticLibrary = Path.Combine(ZeroMQRootPath, "MacOS", "libzmq.a");
                break;
            default:
                Console.WriteLine("Unsupported target platform: {0}", Target.Platform);
                Debug.Assert(false);
                break;
        }

        // Ensure that we're using a constant value (string) in this line
        if (!string.IsNullOrEmpty(staticLibrary))
        {
            Console.WriteLine("Using ZeroMQ static library: {0}", staticLibrary);
            PublicAdditionalLibraries.Add(staticLibrary);
        }
        else
        {
            Debug.Assert(false, "Static library path is empty or invalid");
        }

        // Enable exceptions
        bEnableExceptions = true;
    }

    public ZeroMQ(ReadOnlyTargetRules Target) : base(Target)
    {
        PCHUsage = ModuleRules.PCHUsageMode.UseExplicitOrSharedPCHs;
        PublicDependencyModuleNames.AddRange(new string[] { "Core" });
        AddZeroMQ(Target);
    }
}
