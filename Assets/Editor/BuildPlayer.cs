using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;
using UnityEngine.Rendering;
public class BuildPlayer : MonoBehaviour
{
    // Add a menu item named "Do Something" to MyMenu in the menu bar.
    [MenuItem("Build/Android/Mono_OpenGLES")]
    static void Android_Mono_OpenGlES()
    {
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
        PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new[] { GraphicsDeviceType.OpenGLES3 });
        Build(BuildTarget.Android, "_Mono_OpenGLES");
    }
    [MenuItem("Build/Android/Mono_Vulcan")]
    static void Android_Mono_Vulcan()
    {
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
        PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new[] { GraphicsDeviceType.Vulkan });
        Build(BuildTarget.Android, "_Mono_Vulcan");
    }
    [MenuItem("Build/Android/IL2CPP_OpenGLES")]
    static void Android_IL2CPP_OpenGLES()
    {
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new[] { GraphicsDeviceType.OpenGLES3 });
        Build(BuildTarget.Android, "_IL2CPP_OpenGLES");
    }
    [MenuItem("Build/Android/IL2CPP_Vulcan")]
    static void Android_IL2CPP_Vulcan()
    {
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new[] { GraphicsDeviceType.Vulkan });
        Build(BuildTarget.Android, "_IL2CPP_Vulkan");
    }
    [MenuItem("Build/iOS_StrippingLevel/Low")]
    static void IOS_Stripping_Low()
    {
        PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.iOS, ManagedStrippingLevel.Low);
        Build(BuildTarget.iOS, "_Low");
    }
    [MenuItem("Build/iOS_StrippingLevel/Medium")]
    static void IOS_Stripping_Medium()
    {
        PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.iOS, ManagedStrippingLevel.Medium);
        Build(BuildTarget.iOS, "_Medium");
    }
    [MenuItem("Build/iOS_StrippingLevel/High")]
    static void IOS_Stripping_High()
    {
        PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.iOS, ManagedStrippingLevel.High);
        Build(BuildTarget.iOS, "_High");
    }

    static void Build(BuildTarget buildTarget, string title)
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/start.unity" };
        buildPlayerOptions.locationPathName = Application.productName + title;
        buildPlayerOptions.target = buildTarget;
        buildPlayerOptions.options = BuildOptions.None;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }
}
