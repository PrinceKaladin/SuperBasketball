using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System;
using System.IO;

public class BuildScript
{
    public static void PerformBuild()
    {
        // ========================
        // ������ ����
        // ========================
        string[] scenes = {
            "Assets/Scenes/menu.unity",
            "Assets/Scenes/gameplay.unity",
            "Assets/Scenes/Gameover.unity",
            "Assets/Scenes/Settings.unity",
            "Assets/Scenes/howtoplay.unity",
        };

        // ========================
        // ���� � ������ ������
        // ========================
        string aabPath = "ChickenRun.aab";
        string apkPath = "ChickenRun.apk";

        // ========================
        // ��������� Android Signing ����� ���������� ���������
        // ========================
        string keystoreBase64 = Environment.GetEnvironmentVariable("CM_KEYSTORE_BASE64");
        string keystorePass = Environment.GetEnvironmentVariable("CM_KEYSTORE_PASSWORD");
        string keyAlias = Environment.GetEnvironmentVariable("CM_KEY_ALIAS");
        string keyPass = Environment.GetEnvironmentVariable("CM_KEY_PASSWORD");

        string tempKeystorePath = null;

        if (!string.IsNullOrEmpty(keystoreBase64))
        {
            // ������� ��������� ���� keystore
            tempKeystorePath = Path.Combine(Path.GetTempPath(), "TempKeystore.jks");
            File.WriteAllBytes(tempKeystorePath, Convert.FromBase64String(keystoreBase64));

            PlayerSettings.Android.useCustomKeystore = true;
            PlayerSettings.Android.keystoreName = tempKeystorePath;
            PlayerSettings.Android.keystorePass = keystorePass;
            PlayerSettings.Android.keyaliasName = keyAlias;
            PlayerSettings.Android.keyaliasPass = keyPass;

            Debug.Log("Android signing configured from Base64 keystore.");
        }
        else
        {
            Debug.LogWarning("Keystore Base64 not set. APK/AAB will be unsigned.");
        }

        // ========================
        // ����� ��������� ������
        // ========================
        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = scenes,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        // ========================
        // 1. ������ AAB
        // ========================
        EditorUserBuildSettings.buildAppBundle = true;
        options.locationPathName = aabPath;

        Debug.Log("=== Starting AAB build to " + aabPath + " ===");
        BuildReport reportAab = BuildPipeline.BuildPlayer(options);
        if (reportAab.summary.result == BuildResult.Succeeded)
            Debug.Log("AAB build succeeded! File: " + aabPath);
        else
            Debug.LogError("AAB build failed!");

        // ========================
        // 2. ������ APK
        // ========================
        EditorUserBuildSettings.buildAppBundle = false;
        options.locationPathName = apkPath;

        Debug.Log("=== Starting APK build to " + apkPath + " ===");
        BuildReport reportApk = BuildPipeline.BuildPlayer(options);
        if (reportApk.summary.result == BuildResult.Succeeded)
            Debug.Log("APK build succeeded! File: " + apkPath);
        else
            Debug.LogError("APK build failed!");

        Debug.Log("=== Build script finished ===");

        // ========================
        // �������� ���������� keystore
        // ========================
        if (!string.IsNullOrEmpty(tempKeystorePath) && File.Exists(tempKeystorePath))
        {
            File.Delete(tempKeystorePath);
            Debug.Log("Temporary keystore deleted.");
        }
    }
}