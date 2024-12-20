using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public class SceneScreenshotTaker : EditorWindow
{
    private string[] scenePaths; // List of scenes to capture
    private string saveFolder = "Screenshots"; // Folder to save screenshots
    private int resolutionWidth = 1920;
    private int resolutionHeight = 1080;
    private bool loadScenesFromBuildSettings = false;
    private bool includeUI = false; // Option to include UI elements in screenshots

    [MenuItem("Tools/Scene Screenshot Taker")]
    public static void ShowWindow()
    {
        GetWindow<SceneScreenshotTaker>("Scene Screenshot Taker");
    }

    private void OnGUI()
    {
        GUILayout.Label("Scene Screenshot Taker", EditorStyles.boldLabel);

        // Toggle: Load scenes from Build Settings
        loadScenesFromBuildSettings = EditorGUILayout.Toggle("Use Build Settings Scenes", loadScenesFromBuildSettings);

        if (!loadScenesFromBuildSettings)
        {
            // Input field for scene paths
            EditorGUILayout.LabelField("Scene Paths (comma-separated):", EditorStyles.label);
            string scenesInput = EditorGUILayout.TextField(scenePaths == null ? "" : string.Join(", ", scenePaths));
            if (GUILayout.Button("Load Scenes"))
            {
                scenePaths = scenesInput.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < scenePaths.Length; i++)
                {
                    scenePaths[i] = scenePaths[i].Trim();
                }
            }
        }

        // Folder to save screenshots
        EditorGUILayout.LabelField("Save Folder (relative to Assets):", EditorStyles.label);
        saveFolder = EditorGUILayout.TextField(saveFolder);

        // Screenshot resolution
        EditorGUILayout.LabelField("Resolution:", EditorStyles.label);
        resolutionWidth = EditorGUILayout.IntField("Width", resolutionWidth);
        resolutionHeight = EditorGUILayout.IntField("Height", resolutionHeight);

        // Option to include UI
        includeUI = EditorGUILayout.Toggle("Include UI in Screenshot", includeUI);

        // Button to take screenshots
        if (GUILayout.Button("Take Screenshots"))
        {
            TakeScreenshots();
        }
    }

    private void TakeScreenshots()
    {
        // Get the folder's full path
        string fullSavePath = Path.Combine(Application.dataPath, saveFolder);
        if (!Directory.Exists(fullSavePath))
        {
            Directory.CreateDirectory(fullSavePath);
        }

        // Get scenes to process
        if (loadScenesFromBuildSettings)
        {
            scenePaths = GetScenesFromBuildSettings();
        }

        if (scenePaths == null || scenePaths.Length == 0)
        {
            EditorUtility.DisplayDialog("Error", "No scenes specified.", "OK");
            return;
        }

        foreach (string scenePath in scenePaths)
        {
            if (!File.Exists(scenePath))
            {
                Debug.LogError($"Scene not found: {scenePath}");
                continue;
            }

            // Load the scene
            EditorSceneManager.OpenScene(scenePath);

            // Take a screenshot
            TakeScreenshotForCurrentScene(fullSavePath, scenePath);
        }

        EditorUtility.DisplayDialog("Screenshots Complete", $"Screenshots have been saved to:\n{fullSavePath}", "OK");
    }

    private void TakeScreenshotForCurrentScene(string saveFolder, string scenePath)
    {
        // Find the main camera in the scene
        Camera camera = Camera.main;
        if (camera == null)
        {
            Debug.LogError($"No main camera found in scene: {scenePath}");
            return;
        }

        // Optionally include UI
        Canvas[] canvases = null;
        if (includeUI)
        {
            canvases = EnableUICapture(camera);
        }

        // Create a RenderTexture
        RenderTexture renderTexture = new RenderTexture(resolutionWidth, resolutionHeight, 24);
        camera.targetTexture = renderTexture;

        // Render the camera view
        camera.Render();

        // Create a Texture2D to store the image
        RenderTexture.active = renderTexture;
        Texture2D screenshot = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, resolutionWidth, resolutionHeight), 0, 0);
        screenshot.Apply();

        // Save the screenshot as a PNG
        string sceneName = Path.GetFileNameWithoutExtension(scenePath);
        string screenshotPath = Path.Combine(saveFolder, $"{sceneName}_{resolutionWidth}x{resolutionHeight}.png");
        File.WriteAllBytes(screenshotPath, screenshot.EncodeToPNG());

        Debug.Log($"Screenshot saved: {screenshotPath}");

        // Clean up
        camera.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(renderTexture);
        DestroyImmediate(screenshot);

        // Restore UI settings if modified
        if (includeUI && canvases != null)
        {
            RestoreUISettings(canvases);
        }
    }

    private Canvas[] EnableUICapture(Camera camera)
    {
        // Find all canvases in the scene
        Canvas[] canvases = GameObject.FindObjectsOfType<Canvas>();

        foreach (Canvas canvas in canvases)
        {
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                // Switch ScreenSpace-Overlay to ScreenSpace-Camera
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = camera;
            }
        }

        return canvases;
    }

    private void RestoreUISettings(Canvas[] canvases)
    {
        foreach (Canvas canvas in canvases)
        {
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                // Restore to ScreenSpace-Overlay
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.worldCamera = null;
            }
        }
    }

    private string[] GetScenesFromBuildSettings()
    {
        EditorBuildSettingsScene[] buildScenes = EditorBuildSettings.scenes;
        string[] scenePaths = new string[buildScenes.Length];

        for (int i = 0; i < buildScenes.Length; i++)
        {
            scenePaths[i] = buildScenes[i].path;
        }

        return scenePaths;
    }
}
