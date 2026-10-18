#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelGenerator : EditorWindow
{
    private int startLevel = 16;
    private int endLevel = 50;

    [MenuItem("Tools/Ball Game/Level Generator Window")]
    public static void ShowWindow()
    {
        GetWindow<LevelGenerator>("Level Generator");
    }

    [MenuItem("Tools/Ball Game/Generate Levels 16 to 30 (Batch)")]
    public static void GenerateAllLevelsMenu()
    {
        GenerateAllLevels(16, 30);
    }

    [MenuItem("Tools/Ball Game/Generate Levels 31 to 50 (Batch)")]
    public static void GenerateLevels31To50Menu()
    {
        GenerateAllLevels(31, 50);
    }

    public static void GenerateAllLevelsBatch()
    {
        Debug.Log("[LevelGenerator] CLI: Starting batch generation for Levels 16 to 30...");
        GenerateAllLevels(16, 30);
        Debug.Log("[LevelGenerator] CLI: All levels from 16 to 30 generated and enabled!");
    }

    public static void GenerateLevels31To50Batch()
    {
        Debug.Log("[LevelGenerator] CLI: Starting batch generation for Levels 31 to 50...");
        GenerateAllLevels(31, 50);
        Debug.Log("[LevelGenerator] CLI: All levels from 31 to 50 generated and enabled!");
    }

    private void OnGUI()
    {
        GUILayout.Label("Ball Platform 3D - Progressive Level Generator", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        startLevel = EditorGUILayout.IntField("Start Level:", startLevel);
        endLevel = EditorGUILayout.IntField("End Level:", endLevel);

        EditorGUILayout.Space(10);
        if (GUILayout.Button($"Generate Levels {startLevel} to {endLevel} Now", GUILayout.Height(45)))
        {
            GenerateAllLevels(startLevel, endLevel);
        }
    }

    public static void GenerateAllLevels(int fromLevel, int toLevel)
    {
        for (int lvl = fromLevel; lvl <= toLevel; lvl++)
        {
            // Number of segments scales from 10 (lvl 16) up to 18 (lvl 30)
            int segments = 10 + (int)((lvl - 16) * 0.6f);
            GenerateLevel(lvl, segments);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"[LevelGenerator] Successfully generated all levels from {fromLevel} to {toLevel}!");
    }

    public static void GenerateLevel(int lvlNum, int segments)
    {
        string sceneDirectory = "Assets/Scenes/LevelScene";
        if (!AssetDatabase.IsValidFolder(sceneDirectory))
        {
            AssetDatabase.CreateFolder("Assets/Scenes", "LevelScene");
        }

        string scenePath = $"{sceneDirectory}/Level {lvlNum}.unity";
        int difficultyTier = Mathf.Clamp((lvlNum - 1) / 5 + 1, 1, 6);

        // 1. Create a fresh scene
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // 2. Load Core Prefabs
        GameObject startPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/useInEachFolder/Start.prefab");
        GameObject finishPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/useInEachFolder/NextLevel.prefab");
        GameObject gmPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/useInEachFolder/GameManager.prefab");
        GameObject canvasPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/useInEachFolder/Canvas.prefab");
        GameObject saveGamePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/useInEachFolder/SaveGame.prefab");
        GameObject lampPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/useInEachFolder/PTK_Lamp.prefab");
        
        // Modular Track Prefabs
        GameObject bridgeLargePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PlantAndGround/Bridge/Bridge_Large.prefab");
        GameObject bridgeSmallPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PlantAndGround/Bridge/Bridge_Small.prefab");
        GameObject movingPlatformPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PlantAndGround/Bridge/PTK_Cuboid_5.prefab");
        GameObject slabPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PlantAndGround/Rocks/slab_1.prefab");
        GameObject hurdlePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PlantAndGround/Hurdle/Hurdle01.prefab");

        // New Mechanics Prefabs
        GameObject jumpPadPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Mechanics/JumpPad.prefab");
        GameObject speedBoostPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Mechanics/SpeedBoostPad.prefab");
        GameObject crumblingPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Mechanics/CrumblingPlatform.prefab");
        GameObject periodicTrapPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Mechanics/PeriodicTrap.prefab");

        // 3. Environment & Lighting
        GameObject lightObj = new GameObject("Directional Light");
        Light light = lightObj.AddComponent<Light>();
        light.type = LightType.Directional;
        light.color = new Color(1f, 0.95f, 0.88f);
        light.intensity = 1.15f;
        lightObj.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        GameObject eventSystemObj = new GameObject("EventSystem");
        eventSystemObj.AddComponent<UnityEngine.EventSystems.EventSystem>();
        eventSystemObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();

        // 4. Spawn Positions & Camera Setup
        GameObject spawnPoint = new GameObject("SpawnPoint");
        spawnPoint.transform.position = new Vector3(0f, 1.5f, 0f);

        GameObject spawnWorldPos = new GameObject("SpawnWorldPos");
        spawnWorldPos.transform.position = Vector3.zero;

        GameObject camObj = new GameObject("MainCamera");
        Camera cam = camObj.AddComponent<Camera>();
        cam.clearFlags = CameraClearFlags.Skybox;
        camObj.AddComponent<AudioListener>();
        camObj.tag = "MainCamera";
        camObj.transform.position = new Vector3(0f, 5.5f, -8f);
        camObj.transform.rotation = Quaternion.Euler(22f, 0f, 0f);

        CameraFollow camFollow = camObj.AddComponent<CameraFollow>();
        camFollow.target = spawnPoint.transform;

        // 5. Instantiate Canvas & GameManager
        GameObject canvasObj = null;
        if (canvasPrefab != null)
        {
            canvasObj = (GameObject)PrefabUtility.InstantiatePrefab(canvasPrefab);
            canvasObj.name = "Canvas";
        }

        GameObject gmObj = null;
        if (gmPrefab != null)
        {
            gmObj = (GameObject)PrefabUtility.InstantiatePrefab(gmPrefab);
            gmObj.name = "GameManager";

            LoadBalls loadBalls = gmObj.GetComponent<LoadBalls>();
            if (loadBalls != null) loadBalls.spawnPosBalls = spawnPoint.transform;

            LoadWorld loadWorld = gmObj.GetComponent<LoadWorld>();
            if (loadWorld != null) loadWorld.spawnPosWorld = spawnWorldPos.transform;

            if (canvasObj != null)
            {
                GameManager gmComponent = gmObj.GetComponent<GameManager>();
                ItemCollector itemCollector = gmObj.GetComponent<ItemCollector>();

                Transform victoryT = canvasObj.transform.Find("Victory");
                Transform settingT = canvasObj.transform.Find("Setting");
                Transform levelTextT = canvasObj.transform.Find("LevelText");
                Transform coinsTextT = canvasObj.transform.Find("CoinsText");
                Transform hearthTextT = canvasObj.transform.Find("HearthText");

                if (gmComponent != null)
                {
                    if (victoryT != null) gmComponent.VictoryDialog = victoryT.gameObject;
                    if (settingT != null) gmComponent.setting = settingT.gameObject;
                    if (levelTextT != null) gmComponent.levelText = levelTextT.GetComponent<Text>();
                }

                if (itemCollector != null)
                {
                    if (coinsTextT != null) itemCollector.coinsText = coinsTextT.GetComponent<Text>();
                    if (hearthTextT != null) itemCollector.hearthText = hearthTextT.GetComponent<Text>();
                }
            }
        }

        // 6. Starting Platform
        if (startPrefab != null)
        {
            GameObject startPlatform = (GameObject)PrefabUtility.InstantiatePrefab(startPrefab);
            startPlatform.name = "Start";
            startPlatform.transform.position = Vector3.zero;
        }

        // 7. Track Generation
        GameObject levelContainer = new GameObject("Level");
        GameObject trackRoot = new GameObject("Track");
        trackRoot.transform.SetParent(levelContainer.transform);

        GameObject hurdleRoot = new GameObject("Hurdles");
        hurdleRoot.transform.SetParent(levelContainer.transform);

        if (gmObj != null)
        {
            SpawnCoins spawnCoins = gmObj.GetComponent<SpawnCoins>();
            if (spawnCoins != null) spawnCoins.targetparent = trackRoot.transform;
        }

        float currentZ = 6f;
        float currentX = 0f;
        float currentY = 0f;

        // Progressive Generation Loop
        for (int i = 0; i < segments; i++)
        {
            // Place midpoint checkpoint with SaveGame
            if (i == segments / 2 && slabPrefab != null)
            {
                currentX = Mathf.MoveTowards(currentX, 0f, 3f);
                GameObject island = (GameObject)PrefabUtility.InstantiatePrefab(slabPrefab, trackRoot.transform);
                island.name = $"MidpointIsland_{i}";
                island.transform.position = new Vector3(currentX, currentY - 0.2f, currentZ);
                island.transform.localScale = new Vector3(2.5f, 1f, 2.5f);
                island.tag = "Ground";

                if (saveGamePrefab != null)
                {
                    GameObject saveCheckpoint = (GameObject)PrefabUtility.InstantiatePrefab(saveGamePrefab, trackRoot.transform);
                    saveCheckpoint.name = "SaveGame_Checkpoint";
                    saveCheckpoint.transform.position = new Vector3(currentX, currentY + 0.5f, currentZ);
                }

                currentZ += 10f;
                continue;
            }

            // Determine pattern based on index and difficultyTier
            int pattern = (i + lvlNum) % 6;

            switch (pattern)
            {
                case 0: // Speed Boost straight runway
                    if (speedBoostPrefab != null && difficultyTier >= 4)
                    {
                        GameObject boost = (GameObject)PrefabUtility.InstantiatePrefab(speedBoostPrefab, trackRoot.transform);
                        boost.name = $"SpeedBoost_{i}";
                        boost.transform.position = new Vector3(currentX, currentY, currentZ);
                        boost.transform.rotation = Quaternion.Euler(0, 0, 0);

                        currentZ += 5f;
                        if (bridgeLargePrefab != null)
                        {
                            GameObject longRunway = (GameObject)PrefabUtility.InstantiatePrefab(bridgeLargePrefab, trackRoot.transform);
                            longRunway.name = $"BridgeRunway_{i}";
                            longRunway.transform.position = new Vector3(currentX, currentY, currentZ);
                            longRunway.tag = "Bridge";
                        }
                        currentZ += 10f;
                    }
                    else
                    {
                        if (bridgeLargePrefab != null)
                        {
                            GameObject bridge = (GameObject)PrefabUtility.InstantiatePrefab(bridgeLargePrefab, trackRoot.transform);
                            bridge.name = $"BridgeStraight_{i}";
                            bridge.transform.position = new Vector3(currentX, currentY, currentZ);
                            bridge.tag = "Bridge";
                        }
                        currentZ += 10f;
                    }
                    break;

                case 1: // Crumbling Platform danger zone
                    if (crumblingPrefab != null && difficultyTier >= 4)
                    {
                        // 2 consecutive crumbling stepping stones over a void
                        for (int k = 0; k < 2; k++)
                        {
                            GameObject crumb = (GameObject)PrefabUtility.InstantiatePrefab(crumblingPrefab, trackRoot.transform);
                            crumb.name = $"CrumblingPlatform_{i}_{k}";
                            crumb.transform.position = new Vector3(currentX + (k == 1 ? 1.5f : 0f), currentY, currentZ);
                            currentZ += 5f;
                        }
                        // Landing bridge
                        if (bridgeSmallPrefab != null)
                        {
                            GameObject land = (GameObject)PrefabUtility.InstantiatePrefab(bridgeSmallPrefab, trackRoot.transform);
                            land.name = $"BridgeLanding_{i}";
                            land.transform.position = new Vector3(currentX + 1.5f, currentY, currentZ);
                            land.tag = "Bridge";
                        }
                        currentX += 1.5f;
                        currentZ += 8f;
                    }
                    else
                    {
                        float turnOffset = (i % 2 == 0) ? 2.5f : -2.5f;
                        currentX += turnOffset;
                        if (bridgeSmallPrefab != null)
                        {
                            GameObject turnBridge = (GameObject)PrefabUtility.InstantiatePrefab(bridgeSmallPrefab, trackRoot.transform);
                            turnBridge.name = $"BridgeTurn_{i}";
                            turnBridge.transform.position = new Vector3(currentX, currentY, currentZ);
                            turnBridge.tag = "Bridge";
                        }
                        currentZ += 8f;
                    }
                    break;

                case 2: // Jump Pad launch to elevated platform
                    if (jumpPadPrefab != null && difficultyTier >= 3)
                    {
                        GameObject jpad = (GameObject)PrefabUtility.InstantiatePrefab(jumpPadPrefab, trackRoot.transform);
                        jpad.name = $"JumpPad_{i}";
                        jpad.transform.position = new Vector3(currentX, currentY, currentZ);

                        // Elevated landing platform ahead
                        currentY += 2.5f;
                        currentZ += 8f;
                        if (slabPrefab != null)
                        {
                            GameObject highPlatform = (GameObject)PrefabUtility.InstantiatePrefab(slabPrefab, trackRoot.transform);
                            highPlatform.name = $"HighPlatform_{i}";
                            highPlatform.transform.position = new Vector3(currentX, currentY - 0.2f, currentZ);
                            highPlatform.transform.localScale = new Vector3(2.5f, 1f, 2.5f);
                            highPlatform.tag = "Ground";
                        }
                        currentZ += 10f;
                    }
                    else
                    {
                        if (bridgeLargePrefab != null)
                        {
                            GameObject bridgeObs = (GameObject)PrefabUtility.InstantiatePrefab(bridgeLargePrefab, trackRoot.transform);
                            bridgeObs.name = $"BridgeObs_{i}";
                            bridgeObs.transform.position = new Vector3(currentX, currentY, currentZ);
                            bridgeObs.tag = "Bridge";
                        }
                        if (hurdlePrefab != null)
                        {
                            GameObject hurdle = (GameObject)PrefabUtility.InstantiatePrefab(hurdlePrefab, hurdleRoot.transform);
                            hurdle.name = $"Hurdle_{i}";
                            hurdle.transform.position = new Vector3(currentX, currentY + 0.3f, currentZ);
                            hurdle.tag = "Hurdle";
                        }
                        currentZ += 10f;
                    }
                    break;

                case 3: // Periodic Spike Trap section
                    if (periodicTrapPrefab != null && difficultyTier >= 5)
                    {
                        if (slabPrefab != null)
                        {
                            GameObject trapPlatform = (GameObject)PrefabUtility.InstantiatePrefab(slabPrefab, trackRoot.transform);
                            trapPlatform.name = $"TrapPlatform_{i}";
                            trapPlatform.transform.position = new Vector3(currentX, currentY - 0.2f, currentZ);
                            trapPlatform.transform.localScale = new Vector3(2.5f, 1f, 2.5f);
                            trapPlatform.tag = "Ground";
                        }
                        GameObject spikeTrap = (GameObject)PrefabUtility.InstantiatePrefab(periodicTrapPrefab, hurdleRoot.transform);
                        spikeTrap.name = $"PeriodicSpike_{i}";
                        spikeTrap.transform.position = new Vector3(currentX, currentY, currentZ);

                        currentZ += 10f;
                    }
                    else
                    {
                        // Moving platform gap
                        if (movingPlatformPrefab != null)
                        {
                            GameObject movingPlatform = (GameObject)PrefabUtility.InstantiatePrefab(movingPlatformPrefab, trackRoot.transform);
                            movingPlatform.name = $"MovingPlatform_{i}";
                            movingPlatform.transform.position = new Vector3(currentX, currentY, currentZ);
                            movingPlatform.tag = "Ground";

                            if (movingPlatform.GetComponent<StickyPlatform>() == null)
                            {
                                movingPlatform.AddComponent<StickyPlatform>();
                            }

                            WaypointFollow wf = movingPlatform.GetComponent<WaypointFollow>();
                            if (wf == null) wf = movingPlatform.AddComponent<WaypointFollow>();
                            wf.speed = 1.8f + (difficultyTier * 0.4f);

                            float travelDist = 3.5f + (difficultyTier * 0.3f);
                            GameObject wp1 = new GameObject($"Waypoint_{i}_A");
                            wp1.transform.position = new Vector3(currentX - travelDist, currentY, currentZ);
                            wp1.transform.SetParent(levelContainer.transform);

                            GameObject wp2 = new GameObject($"Waypoint_{i}_B");
                            wp2.transform.position = new Vector3(currentX + travelDist, currentY, currentZ);
                            wp2.transform.SetParent(levelContainer.transform);

                            wf.waypoints = new GameObject[] { wp1, wp2 };
                        }
                        currentZ += 12f;
                    }
                    break;

                case 4: // Moving platform with high speed
                    if (movingPlatformPrefab != null)
                    {
                        GameObject movingPlatform = (GameObject)PrefabUtility.InstantiatePrefab(movingPlatformPrefab, trackRoot.transform);
                        movingPlatform.name = $"MovingPlatform_{i}";
                        movingPlatform.transform.position = new Vector3(currentX, currentY, currentZ);
                        movingPlatform.tag = "Ground";

                        if (movingPlatform.GetComponent<StickyPlatform>() == null)
                        {
                            movingPlatform.AddComponent<StickyPlatform>();
                        }

                        WaypointFollow wf = movingPlatform.GetComponent<WaypointFollow>();
                        if (wf == null) wf = movingPlatform.AddComponent<WaypointFollow>();
                        wf.speed = 2.2f + (difficultyTier * 0.4f);

                        GameObject wp1 = new GameObject($"Waypoint_{i}_A");
                        wp1.transform.position = new Vector3(currentX - 3.5f, currentY, currentZ);
                        wp1.transform.SetParent(levelContainer.transform);

                        GameObject wp2 = new GameObject($"Waypoint_{i}_B");
                        wp2.transform.position = new Vector3(currentX + 3.5f, currentY, currentZ);
                        wp2.transform.SetParent(levelContainer.transform);

                        wf.waypoints = new GameObject[] { wp1, wp2 };
                    }
                    currentZ += 12f;
                    break;

                case 5: // Narrow S-curve balance beam
                    float offset = (i % 2 == 0) ? -2.5f : 2.5f;
                    currentX += offset;
                    if (bridgeSmallPrefab != null)
                    {
                        GameObject narrowBridge = (GameObject)PrefabUtility.InstantiatePrefab(bridgeSmallPrefab, trackRoot.transform);
                        narrowBridge.name = $"NarrowBridge_{i}";
                        narrowBridge.transform.position = new Vector3(currentX, currentY, currentZ);
                        narrowBridge.tag = "Bridge";
                    }
                    currentZ += 8f;
                    break;
            }

            // Decorative lamp
            if (i % 3 == 0 && lampPrefab != null)
            {
                GameObject lamp = (GameObject)PrefabUtility.InstantiatePrefab(lampPrefab, trackRoot.transform);
                lamp.transform.position = new Vector3(currentX + 2.2f, currentY, currentZ - 3f);
            }
        }

        // 8. Final stretch & Finish Gate
        currentX = Mathf.MoveTowards(currentX, 0f, 4f);
        if (bridgeLargePrefab != null)
        {
            GameObject finalBridge = (GameObject)PrefabUtility.InstantiatePrefab(bridgeLargePrefab, trackRoot.transform);
            finalBridge.name = "FinalBridge";
            finalBridge.transform.position = new Vector3(currentX, currentY, currentZ);
            finalBridge.tag = "Bridge";
        }
        currentZ += 10f;

        if (finishPrefab != null)
        {
            GameObject finishObj = (GameObject)PrefabUtility.InstantiatePrefab(finishPrefab);
            finishObj.name = "NextLevel";
            finishObj.transform.position = new Vector3(currentX, currentY, currentZ);
        }

        // 9. Void Killplane
        GameObject killplane = GameObject.CreatePrimitive(PrimitiveType.Cube);
        killplane.name = "VoidKillGround";
        killplane.tag = "BackGround";
        killplane.transform.position = new Vector3(0f, -15f, currentZ * 0.5f);
        killplane.transform.localScale = new Vector3(180f, 1f, currentZ + 80f);
        
        MeshRenderer kr = killplane.GetComponent<MeshRenderer>();
        if (kr != null) kr.enabled = false;

        // 10. Save Scene
        EditorSceneManager.SaveScene(scene, scenePath);

        // 11. Register in Build Settings & Enable
        AddSceneToBuildSettings(scenePath);
        Debug.Log($"[LevelGenerator] Generated Level {lvlNum} (Tier {difficultyTier}, {segments} segments) -> {scenePath}");
    }

    private static void AddSceneToBuildSettings(string scenePath)
    {
        EditorBuildSettingsScene[] existingScenes = EditorBuildSettings.scenes;
        bool found = false;

        for (int i = 0; i < existingScenes.Length; i++)
        {
            if (existingScenes[i].path == scenePath)
            {
                existingScenes[i].enabled = true;
                found = true;
                break;
            }
        }

        if (found)
        {
            EditorBuildSettings.scenes = existingScenes;
        }
        else
        {
            var newScenes = new EditorBuildSettingsScene[existingScenes.Length + 1];
            Array.Copy(existingScenes, newScenes, existingScenes.Length);
            newScenes[newScenes.Length - 1] = new EditorBuildSettingsScene(scenePath, true);
            EditorBuildSettings.scenes = newScenes;
        }
    }
}
#endif
