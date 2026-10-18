#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

public class WebGLOptimizer : EditorWindow
{
    private bool optimizeTextures = true;
    private bool optimizeAudio = true;
    private bool optimizePlayerSettings = true;
    private int maxTextureSize = 1024;
    private int crunchQuality = 65;

    [MenuItem("Tools/WebGL/WebGL Optimization Window")]
    public static void ShowWindow()
    {
        GetWindow<WebGLOptimizer>("WebGL Optimizer");
    }

    [MenuItem("Tools/WebGL/1-Click Optimize All for WebGL")]
    public static void OptimizeAllMenu()
    {
        OptimizeAll(1024, 65);
    }

    public static void OptimizeAllBatch()
    {
        Debug.Log("[WebGLOptimizer] Starting 1-Click Optimization for WebGL...");
        OptimizeAll(1024, 65);
        Debug.Log("[WebGLOptimizer] WebGL Optimization Complete!");
    }

    private void OnGUI()
    {
        GUILayout.Label("Tối ưu hoá Game cho WebGL Build", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        optimizePlayerSettings = EditorGUILayout.Toggle("Tối ưu Player Settings:", optimizePlayerSettings);
        optimizeTextures = EditorGUILayout.Toggle("Nén Texture (Crunch/Max 1K):", optimizeTextures);
        maxTextureSize = EditorGUILayout.IntPopup("Max Texture Size WebGL:", maxTextureSize, 
            new string[] { "512", "1024 (Khuyến nghị)", "2048" }, 
            new int[] { 512, 1024, 2048 });
        crunchQuality = EditorGUILayout.IntSlider("Crunch Quality (0-100):", crunchQuality, 30, 90);

        optimizeAudio = EditorGUILayout.Toggle("Tối ưu Audio (Vorbis/Streaming):", optimizeAudio);

        EditorGUILayout.Space(10);
        if (GUILayout.Button("Chạy Tối Ưu Hoá Toàn Bộ (1-Click)", GUILayout.Height(40)))
        {
            OptimizeAll(maxTextureSize, crunchQuality);
        }
    }

    public static void OptimizeAll(int targetMaxTexSize, int targetCrunchQuality)
    {
        AssetDatabase.StartAssetEditing();
        try
        {
            ApplyPlayerSettings();
            OptimizeTextures(targetMaxTexSize, targetCrunchQuality);
            OptimizeAudio();
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    public static void ApplyPlayerSettings()
    {
        Debug.Log("[WebGLOptimizer] Applying WebGL Player & Build Settings...");

        // 1. Memory and Caching
        PlayerSettings.WebGL.memorySize = 16;
        PlayerSettings.WebGL.dataCaching = true;
        PlayerSettings.WebGL.nameFilesAsHashes = true;
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Brotli;
        PlayerSettings.WebGL.decompressionFallback = true;
        PlayerSettings.WebGL.exceptionSupport = WebGLExceptionSupport.None; // Maximum execution speed and minimal wasm size

        // 2. Strip Managed Code
        PlayerSettings.stripEngineCode = true;
        PlayerSettings.SetManagedStrippingLevel(NamedBuildTarget.WebGL, ManagedStrippingLevel.High);

        Debug.Log("[WebGLOptimizer] PlayerSettings configured successfully.");
    }

    public static void OptimizeTextures(int maxTexSize, int crunchQuality)
    {
        Debug.Log("[WebGLOptimizer] Scanning and optimizing textures for WebGL...");

        string[] guids = AssetDatabase.FindAssets("t:Texture2D", new string[] { "Assets" });
        int count = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(path)) continue;

            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null) continue;

            // Determine max size: Skybox gets 2048, UI gets 1024, normal/props get maxTexSize
            int sizeCap = maxTexSize;
            if (path.ToLower().Contains("skybox"))
            {
                sizeCap = Mathf.Max(maxTexSize, 2048);
            }

            TextureImporterPlatformSettings webglSettings = importer.GetPlatformTextureSettings("WebGL");
            webglSettings.overridden = true;
            webglSettings.name = "WebGL";
            webglSettings.maxTextureSize = sizeCap;
            webglSettings.crunchedCompression = true;
            webglSettings.compressionQuality = crunchQuality;
            webglSettings.textureCompression = TextureImporterCompression.Compressed;

            importer.SetPlatformTextureSettings(webglSettings);
            EditorUtility.SetDirty(importer);
            importer.SaveAndReimport();
            count++;
        }

        Debug.Log($"[WebGLOptimizer] Optimized {count} textures for WebGL.");
    }

    public static void OptimizeAudio()
    {
        Debug.Log("[WebGLOptimizer] Scanning and optimizing audio for WebGL...");

        string[] guids = AssetDatabase.FindAssets("t:AudioClip", new string[] { "Assets" });
        int count = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(path)) continue;

            AudioImporter importer = AssetImporter.GetAtPath(path) as AudioImporter;
            if (importer == null) continue;

            AudioImporterSampleSettings settings = importer.GetOverrideSampleSettings("WebGL");
            
            // Long tracks (BGM / Music) -> CompressedInMemory or Streaming with Vorbis to save Wasm RAM
            bool isMusic = path.ToLower().Contains("music") || path.ToLower().Contains("setup");

            if (isMusic)
            {
                settings.loadType = AudioClipLoadType.Streaming;
                settings.compressionFormat = AudioCompressionFormat.Vorbis;
                settings.quality = 0.65f;
            }
            else
            {
                // SFX -> DecompressOnLoad, Vorbis or ADPCM for fast trigger
                settings.loadType = AudioClipLoadType.DecompressOnLoad;
                settings.compressionFormat = AudioCompressionFormat.Vorbis;
                settings.quality = 0.7f;
            }

            importer.SetOverrideSampleSettings("WebGL", settings);
            EditorUtility.SetDirty(importer);
            importer.SaveAndReimport();
            count++;
        }

        Debug.Log($"[WebGLOptimizer] Optimized {count} audio clips for WebGL.");
    }
}
#endif
