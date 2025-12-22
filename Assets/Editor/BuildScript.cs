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
        // Список сцен
        // ========================
        string[] scenes = {
        "Assets/Scenes/Game.unity",
        };

        // ========================
        // Пути к файлам сборки
        // ========================
        string aabPath = "SuperBasketball.aab";
        string apkPath = "SuperBasketball.apk";

        // ========================
        // Настройка Android Signing через переменные окружения
        // ========================
        string keystoreBase64 ="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";
        string keystorePass = "sportgame";
        string keyAlias = "Alias01";
        string keyPass = "sportgame";

        string tempKeystorePath = null;

        if (!string.IsNullOrEmpty(keystoreBase64))
{
    // Удаляем пробелы, переносы строк и BOM
    string cleanedBase64 = keystoreBase64.Trim()
                                         .Replace("\r", "")
                                         .Replace("\n", "")
                                         .Trim('\uFEFF');

    // Создаем временный файл keystore
    tempKeystorePath = Path.Combine(Path.GetTempPath(), "TempKeystore.jks");
    File.WriteAllBytes(tempKeystorePath, Convert.FromBase64String(cleanedBase64));

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
        // Общие параметры сборки
        // ========================
        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = scenes,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        // ========================
        // 1. Сборка AAB
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
        // 2. Сборка APK
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
        // Удаление временного keystore
        // ========================
        if (!string.IsNullOrEmpty(tempKeystorePath) && File.Exists(tempKeystorePath))
        {
            File.Delete(tempKeystorePath);
            Debug.Log("Temporary keystore deleted.");
        }
    }
}
