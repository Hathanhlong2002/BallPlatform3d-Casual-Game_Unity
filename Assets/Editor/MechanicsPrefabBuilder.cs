#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

public class MechanicsPrefabBuilder
{
    private const string FolderPath = "Assets/Prefabs/Mechanics";

    [MenuItem("Tools/Ball Game/Build Mechanics Prefabs")]
    public static void BuildAllPrefabsMenu()
    {
        BuildAllPrefabs();
    }

    public static void BuildAllPrefabs()
    {
        Debug.Log("[MechanicsPrefabBuilder] Starting prefab generation via Unity CLI...");

        if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }
        if (!AssetDatabase.IsValidFolder(FolderPath))
        {
            AssetDatabase.CreateFolder("Assets/Prefabs", "Mechanics");
        }

        BuildJumpPadPrefab();
        BuildSpeedBoostPadPrefab();
        BuildCrumblingPlatformPrefab();
        BuildPeriodicTrapPrefab();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[MechanicsPrefabBuilder] All 4 mechanics prefabs built successfully!");
    }

    private static Material GetOrCreateColorMaterial(string matName, Color color, float metallic = 0.5f, float smoothness = 0.5f)
    {
        string matPath = $"Assets/Materials/{matName}.mat";
        Material mat = AssetDatabase.LoadAssetAtPath<Material>(matPath);
        if (mat == null)
        {
            Shader standardShader = Shader.Find("Standard");
            if (standardShader == null) standardShader = Shader.Find("Universal Render Pipeline/Lit");
            if (standardShader == null) standardShader = Shader.Find("Mobile/Diffuse");

            mat = new Material(standardShader);
            mat.color = color;
            mat.SetFloat("_Metallic", metallic);
            mat.SetFloat("_Glossiness", smoothness);

            if (!AssetDatabase.IsValidFolder("Assets/Materials"))
            {
                AssetDatabase.CreateFolder("Assets", "Materials");
            }
            AssetDatabase.CreateAsset(mat, matPath);
        }
        return mat;
    }

    private static void BuildJumpPadPrefab()
    {
        string prefabPath = $"{FolderPath}/JumpPad.prefab";

        GameObject root = new GameObject("JumpPad");
        root.tag = "Untagged";

        // Base frame
        GameObject baseFrame = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        baseFrame.name = "BaseFrame";
        baseFrame.transform.SetParent(root.transform);
        baseFrame.transform.localPosition = new Vector3(0, 0.05f, 0);
        baseFrame.transform.localScale = new Vector3(2.4f, 0.1f, 2.4f);
        Material baseMat = GetOrCreateColorMaterial("Mat_JumpPad_Base", new Color(0.25f, 0.25f, 0.28f));
        baseFrame.GetComponent<Renderer>().sharedMaterial = baseMat;

        // Top spring plate (Trigger)
        GameObject topPlate = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        topPlate.name = "SpringPlate";
        topPlate.transform.SetParent(root.transform);
        topPlate.transform.localPosition = new Vector3(0, 0.2f, 0);
        topPlate.transform.localScale = new Vector3(2.0f, 0.12f, 2.0f);
        Material springMat = GetOrCreateColorMaterial("Mat_JumpPad_Top", new Color(1.0f, 0.75f, 0.0f)); // Bright Yellow
        topPlate.GetComponent<Renderer>().sharedMaterial = springMat;

        // Trigger Collider
        Collider topCol = topPlate.GetComponent<Collider>();
        if (topCol != null) topCol.isTrigger = true;

        // Add script
        root.AddComponent<JumpPad>();

        PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
        Object.DestroyImmediate(root);
        Debug.Log($"[MechanicsPrefabBuilder] Saved {prefabPath}");
    }

    private static void BuildSpeedBoostPadPrefab()
    {
        string prefabPath = $"{FolderPath}/SpeedBoostPad.prefab";

        GameObject root = new GameObject("SpeedBoostPad");
        root.tag = "Bridge";

        // Pad mesh
        GameObject padMesh = GameObject.CreatePrimitive(PrimitiveType.Cube);
        padMesh.name = "PadMesh";
        padMesh.transform.SetParent(root.transform);
        padMesh.transform.localPosition = new Vector3(0, 0.02f, 0);
        padMesh.transform.localScale = new Vector3(2.4f, 0.08f, 4.0f);
        Material boostMat = GetOrCreateColorMaterial("Mat_SpeedBoost_Cyan", new Color(0.0f, 0.9f, 1.0f), 0.8f, 0.9f);
        padMesh.GetComponent<Renderer>().sharedMaterial = boostMat;

        // Arrow marker
        GameObject arrowMesh = GameObject.CreatePrimitive(PrimitiveType.Cube);
        arrowMesh.name = "ArrowIndicator";
        arrowMesh.transform.SetParent(root.transform);
        arrowMesh.transform.localPosition = new Vector3(0, 0.08f, 0);
        arrowMesh.transform.localScale = new Vector3(1.2f, 0.05f, 2.0f);
        arrowMesh.transform.localRotation = Quaternion.Euler(0, 45, 0);
        Material arrowMat = GetOrCreateColorMaterial("Mat_SpeedBoost_Arrow", Color.white);
        arrowMesh.GetComponent<Renderer>().sharedMaterial = arrowMat;

        // Add Trigger & Script to root
        BoxCollider triggerCol = root.AddComponent<BoxCollider>();
        triggerCol.size = new Vector3(2.4f, 1.0f, 4.0f);
        triggerCol.center = new Vector3(0, 0.5f, 0);
        triggerCol.isTrigger = true;

        root.AddComponent<SpeedBoostPad>();

        PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
        Object.DestroyImmediate(root);
        Debug.Log($"[MechanicsPrefabBuilder] Saved {prefabPath}");
    }

    private static void BuildCrumblingPlatformPrefab()
    {
        string prefabPath = $"{FolderPath}/CrumblingPlatform.prefab";

        GameObject root = new GameObject("CrumblingPlatform");
        root.tag = "Ground";

        // Main platform slab
        GameObject slabMesh = GameObject.CreatePrimitive(PrimitiveType.Cube);
        slabMesh.name = "SlabMesh";
        slabMesh.transform.SetParent(root.transform);
        slabMesh.transform.localPosition = Vector3.zero;
        slabMesh.transform.localScale = new Vector3(3.0f, 0.4f, 3.0f);
        slabMesh.tag = "Ground";

        Material crumbleMat = GetOrCreateColorMaterial("Mat_Crumbling_Rock", new Color(0.6f, 0.35f, 0.25f)); // Weathered brick/stone
        slabMesh.GetComponent<Renderer>().sharedMaterial = crumbleMat;

        // Add script
        root.AddComponent<CrumblingPlatform>();

        PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
        Object.DestroyImmediate(root);
        Debug.Log($"[MechanicsPrefabBuilder] Saved {prefabPath}");
    }

    private static void BuildPeriodicTrapPrefab()
    {
        string prefabPath = $"{FolderPath}/PeriodicTrap.prefab";

        GameObject root = new GameObject("PeriodicTrap");
        root.tag = "Hurdle";

        // Base Plate
        GameObject basePlate = GameObject.CreatePrimitive(PrimitiveType.Cube);
        basePlate.name = "BasePlate";
        basePlate.transform.SetParent(root.transform);
        basePlate.transform.localPosition = new Vector3(0, 0.05f, 0);
        basePlate.transform.localScale = new Vector3(2.6f, 0.1f, 2.6f);
        Material plateMat = GetOrCreateColorMaterial("Mat_Trap_Base", new Color(0.2f, 0.2f, 0.22f));
        basePlate.GetComponent<Renderer>().sharedMaterial = plateMat;

        // Spikes Root Container
        GameObject spikesRoot = new GameObject("Spikes");
        spikesRoot.transform.SetParent(root.transform);
        spikesRoot.transform.localPosition = new Vector3(0, -0.4f, 0);

        Material spikeMat = GetOrCreateColorMaterial("Mat_Trap_Spikes", new Color(0.85f, 0.15f, 0.15f), 0.9f, 0.7f); // Crimson Metal

        // Grid of 4 spike pillars
        float[] coords = new float[] { -0.6f, 0.6f };
        foreach (float x in coords)
        {
            foreach (float z in coords)
            {
                GameObject spike = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                spike.name = $"Spike_{x}_{z}";
                spike.transform.SetParent(spikesRoot.transform);
                spike.transform.localPosition = new Vector3(x, 0.5f, z);
                spike.transform.localScale = new Vector3(0.25f, 0.5f, 0.25f);
                spike.GetComponent<Renderer>().sharedMaterial = spikeMat;

                // Spike collider is trigger to kill player
                Collider sc = spike.GetComponent<Collider>();
                if (sc != null) sc.isTrigger = true;
            }
        }

        // Add PeriodicTrap script to root
        root.AddComponent<PeriodicTrap>();

        PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
        Object.DestroyImmediate(root);
        Debug.Log($"[MechanicsPrefabBuilder] Saved {prefabPath}");
    }
}
#endif
