using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildScript
{
    // ===== ENTRY POINT FOR CI =====
    public static void PerformBuild()
    {
        Debug.Log("CI PerformBuild started");
        Debug.Log("Active BuildTarget: " + EditorUserBuildSettings.activeBuildTarget);

        switch (EditorUserBuildSettings.activeBuildTarget)
        {
            case BuildTarget.Android:
                BuildAndroid();
                break;

            case BuildTarget.iOS:
                BuildIos();
                break;

            case BuildTarget.StandaloneWindows64:
                BuildWindows();
                break;

            case BuildTarget.StandaloneOSX:
                BuildMac();
                break;

            default:
                Debug.LogError("Unsupported build target: " + EditorUserBuildSettings.activeBuildTarget);
                EditorApplication.Exit(1);
                break;
        }
    }

    // ===== ANDROID =====
    public static void BuildAndroid()
    {
        Debug.Log("Starting Android build (APK + AAB)");

        ApplyAndroidVersionCode();

        string outputDir = "android";
        Directory.CreateDirectory(outputDir);

        var scenes = GetScenes();
        if (scenes.Length == 0)
        {
            Debug.LogError("No enabled scenes found in Build Settings!");
            EditorApplication.Exit(1);
            return;
        }

        // ---------- AAB ----------
        EditorUserBuildSettings.buildAppBundle = true;

        var aabOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            target = BuildTarget.Android,
            options = BuildOptions.None,
            locationPathName = Path.Combine(outputDir, "app.aab")
        };

        BuildAndValidate(aabOptions, "AAB");

        // ---------- APK ----------
        EditorUserBuildSettings.buildAppBundle = false;

        var apkOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            target = BuildTarget.Android,
            options = BuildOptions.None,
            locationPathName = Path.Combine(outputDir, "app.apk")
        };

        BuildAndValidate(apkOptions, "APK");

        Debug.Log("Android build finished successfully (APK + AAB)");
    }

    // ===== iOS =====
    public static void BuildIos()
    {
        Debug.Log("Starting iOS build");

        var options = new BuildPlayerOptions
        {
            scenes = GetScenes(),
            target = BuildTarget.iOS,
            options = BuildOptions.None,
            locationPathName = "ios"
        };

        BuildAndValidate(options, "iOS");
    }

    // ===== WINDOWS =====
    public static void BuildWindows()
    {
        Debug.Log("Starting Windows build");

        var options = new BuildPlayerOptions
        {
            scenes = GetScenes(),
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None,
            locationPathName = "win/" + Application.productName + ".exe"
        };

        BuildAndValidate(options, "Windows");
    }

    // ===== MAC =====
    public static void BuildMac()
    {
        Debug.Log("Starting macOS build");

        var options = new BuildPlayerOptions
        {
            scenes = GetScenes(),
            target = BuildTarget.StandaloneOSX,
            options = BuildOptions.None,
            locationPathName = "mac/" + Application.productName + ".app"
        };

        BuildAndValidate(options, "macOS");
    }

    // ===== COMMON =====
    private static void BuildAndValidate(BuildPlayerOptions options, string label)
    {
        Debug.Log($"Building {label} → {options.locationPathName}");

        BuildReport report = BuildPipeline.BuildPlayer(options);

        if (report.summary.result != BuildResult.Succeeded)
        {
            Debug.LogError($"{label} build failed!");
            EditorApplication.Exit(1);
        }

        Debug.Log($"{label} build succeeded, size: {report.summary.totalSize} bytes");
    }

    private static string[] GetScenes()
    {
        var scenes = EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();

        Debug.Log("Scenes count: " + scenes.Length);
        return scenes;
    }

    private static void ApplyAndroidVersionCode()
    {
        string versionCodeStr = Environment.GetEnvironmentVariable("AC_ANDROID_VERSION_CODE");

        if (int.TryParse(versionCodeStr, out int versionCode))
        {
            PlayerSettings.Android.bundleVersionCode = versionCode;
            Debug.Log("Android bundleVersionCode set to " + versionCode);
        }
        else
        {
            Debug.Log("Android version code not provided, using Player Settings value");
        }
    }
}
