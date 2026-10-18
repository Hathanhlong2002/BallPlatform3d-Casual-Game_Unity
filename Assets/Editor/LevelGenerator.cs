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
    private int levelNumber = 30;
    private int trackLength = 12; // number of segments
    private float segmentSpacing = 10f;

    [MenuItem("Tools/Ball Game/Level Generator Window")]
    public static void ShowWindow()
    {
        GetWindow<LevelGenerator>("Level Generator");
    }

    [MenuItem("Tools/Ball Game/Generate Level 30 Now")]
    public static void GenerateLevel30Menu()
    {
        GenerateLevel(30, 14);
    }

    public static void GenerateLevel30Batch()
    {
        Debug.Log("[LevelGenerator] Starting Batch Generation for Level 30...");
        GenerateLevel(30, 14);
        Debug.Log("[LevelGenerator] Level 30 Batch Generation Complete!");
    }

    private void OnGUI()
    {
        GUILayout.Label("Ball Platform 3D - Level Generator", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        levelNumber = EditorGUILayout.IntField("Level Number:", levelNumber);
        trackLength = EditorGUILayout.IntSlider("Track Segments:", trackLength, 6, 25);
        segmentSpacing = EditorGUILayout.FloatField("Segment Spacing (m):", segmentSpacing);

        EditorGUILayout.Space();
        if (GUILayout.Button("Generate Level", GUILayout.Height(40)))
        {
            GenerateLevel(levelNumber, trackLength);
        }
    }

    public static void GenerateLevel(int lvlNum, int segments)
    {
        string sceneDirectory = "Assets/Scenes/LevelScene";
        if (!AssetDatabase.IsValidFolder(sceneDirectory))
        {
            AssetDatabase.CreateFolder("Assets/Scenes", "LevelScene");
        }

        string scenePath = $"{sceneDirectory}/Level {lvlNum}.unity";

        // 1. Create a fresh scene
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // 2. Load Required Prefabs
        GameObject startPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/useInEachFolder/Start.prefab");
        GameObject finishPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/useInEachFolder/NextLevel.prefab");
        GameObject gmPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/useInEachFolder/GameManager.prefab");
        GameObject canvasPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/useInEachFolder/Canvas.prefab");
        GameObject saveGamePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/useInEachFolder/SaveGame.prefab");
        GameObject lampPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/useInEachFolder/PTK_Lamp.prefab");
        
        GameObject bridgeLargePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PlantAndGround/Bridge/Bridge_Large.prefab");
        GameObject bridgeSmallPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PlantAndGround/Bridge/Bridge_Small.prefab");
        GameObject movingPlatformPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PlantAndGround/Bridge/PTK_Cuboid_5.prefab");
        GameObject slabPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PlantAndGround/Rocks/slab_1.prefab");
        GameObject hurdlePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PlantAndGround/Hurdle/Hurdle01.prefab");
        GameObject bushPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PlantAndGround/Plant/bush_4.prefab");

        // 3. Environment & Lighting
        GameObject lightObj = new GameObject("Directional Light");
        Light light = lightObj.AddComponent<Light>();
        light.type = LightType.Directional;
        light.color = new Color(1f, 0.95f, 0.85f);
        light.intensity = 1.1f;
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

        // Attach CameraFollow
        CameraFollow camFollow = camObj.AddComponent<CameraFollow>();
        camFollow.target = spawnPoint.transform;

        // 5. Instantiate Core Systems (Canvas & GameManager)
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

            // Wire up LoadBalls & LoadWorld
            LoadBalls loadBalls = gmObj.GetComponent<LoadBalls>();
            if (loadBalls != null)
            {
                loadBalls.spawnPosBalls = spawnPoint.transform;
            }

            LoadWorld loadWorld = gmObj.GetComponent<LoadWorld>();
            if (loadWorld != null)
            {
                loadWorld.spawnPosWorld = spawnWorldPos.transform;
            }

            // Wire up UI references from Canvas
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

        // 7. Track Generation (Root container)
        GameObject levelContainer = new GameObject("Level");
        GameObject trackRoot = new GameObject("Track");
        trackRoot.transform.SetParent(levelContainer.transform);

        GameObject hurdleRoot = new GameObject("Hurdles");
        hurdleRoot.transform.SetParent(levelContainer.transform);

        float currentZ = 6f;
        float currentX = 0f;
        float currentY = 0f;

        // Connect SpawnCoins target parent
        if (gmObj != null)
        {
            SpawnCoins spawnCoins = gmObj.GetComponent<SpawnCoins>();
            if (spawnCoins != null)
            {
                spawnCoins.targetparent = trackRoot.transform;
            }
        }

        // Generate diverse modular segments
        for (int i = 0; i < segments; i++)
        {
            int segmentType = i % 4; // 0: Straight, 1: Turn/S-curve, 2: Obstacle/Hurdle, 3: Moving platform

            // Checkpoint island at midpoint
            if (i == segments / 2 && slabPrefab != null)
            {
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

            switch (segmentType)
            {
                case 0: // Straight bridge section
                    if (bridgeLargePrefab != null)
                    {
                        GameObject bridge = (GameObject)PrefabUtility.InstantiatePrefab(bridgeLargePrefab, trackRoot.transform);
                        bridge.name = $"BridgeStraight_{i}";
                        bridge.transform.position = new Vector3(currentX, currentY, currentZ);
                        bridge.tag = "Bridge";
                    }
                    currentZ += 10f;
                    break;

                case 1: // Turn / Zigzag section
                    float turnOffset = (i % 2 == 0) ? 2.5f : -2.5f;
                    currentX += turnOffset;
                    if (bridgeSmallPrefab != null)
                    {
                        GameObject bridgeTurn = (GameObject)PrefabUtility.InstantiatePrefab(bridgeSmallPrefab, trackRoot.transform);
                        bridgeTurn.name = $"BridgeTurn_{i}";
                        bridgeTurn.transform.position = new Vector3(currentX, currentY, currentZ);
                        bridgeTurn.tag = "Bridge";
                    }
                    currentZ += 8f;
                    break;

                case 2: // Hurdle / Obstacle section
                    if (bridgeLargePrefab != null)
                    {
                        GameObject bridgeObs = (GameObject)PrefabUtility.InstantiatePrefab(bridgeLargePrefab, trackRoot.transform);
                        bridgeObs.name = $"BridgeObstacle_{i}";
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
                    break;

                case 3: // Moving Platform section
                    if (movingPlatformPrefab != null)
                    {
                        GameObject movingPlatform = (GameObject)PrefabUtility.InstantiatePrefab(movingPlatformPrefab, trackRoot.transform);
                        movingPlatform.name = $"MovingPlatform_{i}";
                        movingPlatform.transform.position = new Vector3(currentX, currentY, currentZ);
                        movingPlatform.tag = "Ground";

                        // Add StickyPlatform
                        if (movingPlatform.GetComponent<StickyPlatform>() == null)
                        {
                            movingPlatform.AddComponent<StickyPlatform>();
                        }

                        // Waypoint follow setup
                        WaypointFollow wf = movingPlatform.GetComponent<WaypointFollow>();
                        if (wf == null)
                        {
                            wf = movingPlatform.AddComponent<WaypointFollow>();
                        }
                        wf.speed = 2.5f;

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
            }

            // Occasional decorative bush/lamp along borders
            if (i % 3 == 0 && lampPrefab != null)
            {
                GameObject lamp = (GameObject)PrefabUtility.InstantiatePrefab(lampPrefab, trackRoot.transform);
                lamp.transform.position = new Vector3(currentX + 2.2f, currentY, currentZ - 4f);
            }
        }

        // 8. Align to Center and Place Finish Gate
        currentX = Mathf.MoveTowards(currentX, 0f, 5f);
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

        // 9. Void Killplane (BackGround tag triggers BallPlayerMovement.Die())
        GameObject killplane = GameObject.CreatePrimitive(PrimitiveType.Cube);
        killplane.name = "VoidKillGround";
        killplane.tag = "BackGround";
        killplane.transform.position = new Vector3(0f, -12f, currentZ * 0.5f);
        killplane.transform.localScale = new Vector3(150f, 1f, currentZ + 50f);
        
        // Hide mesh renderer so it is an invisible trigger collider
        MeshRenderer kr = killplane.GetComponent<MeshRenderer>();
        if (kr != null) kr.enabled = false;

        // 10. Save Scene
        EditorSceneManager.SaveScene(scene, scenePath);
        Debug.Log($"[LevelGenerator] Saved Scene successfully: {scenePath}");

        // 11. Register scene in EditorBuildSettings
        AddSceneToBuildSettings(scenePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void AddSceneToBuildSettings(string scenePath)
    {
        EditorBuildSettingsScene[] existingScenes = EditorBuildSettings.scenes;
        foreach (var s in existingScenes)
        {
            if (s.path == scenePath)
            {
                s.enabled = true;
                EditorBuildSettings.scenes = existingScenes;
                return;
            }
        }

        var newScenes = new EditorBuildSettingsScene[existingScenes.Length + 1];
        Array.Copy(existingScenes, newScenes, existingScenes.Length);
        newScenes[newScenes.Length - 1] = new EditorBuildSettingsScene(scenePath, true);
        EditorBuildSettings.scenes = newScenes;
        Debug.Log($"[LevelGenerator] Added {scenePath} to EditorBuildSettings!");
    }
}
#endif
