using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    public class DebugOptionsWindow : EditorWindow
    {
        private const string windowName = "Debug Options";
        private bool showFps = true;
        private float deltaTime = 0.0f;
        private int frameCounter = 0;
        private int updateRate = 10; // Update FPS every 10 frames
        private int fps = 0;

        private bool showPlayerTrail = true;
        private readonly string playerTrailName = "PlayerTrail"; // Replace with your actual GameObject's name

        [MenuItem("Window/" + windowName)]
        public static void ShowWindow()
        {
            GetWindow<DebugOptionsWindow>(windowName);
        }

        void OnGUI()
        {
            GUILayout.Label(windowName, EditorStyles.boldLabel);
            showFps = EditorGUILayout.Toggle("Show FPS", showFps);
            showPlayerTrail = EditorGUILayout.Toggle("Show Player Trail", showPlayerTrail);

            if (EditorApplication.isPlaying)
            {
                GameObject playerTrail = GameObject.Find(playerTrailName);
                if (playerTrail != null)
                {
                    LineRenderer lineRenderer = playerTrail.GetComponent<LineRenderer>();
                    if (lineRenderer != null)
                    {
                        lineRenderer.enabled = showPlayerTrail;
                    }
                }
            }
        }

        void Update()
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

            if (showFps && EditorApplication.isPlaying)
            {
                if (++frameCounter >= updateRate)
                {
                    fps = (int)(1.0f / deltaTime);
                    frameCounter = 0;
                }
                titleContent.text = $"{windowName} (FPS: {fps})";
            }
            else
            {
                titleContent.text = windowName;
            }
        }
    }
}