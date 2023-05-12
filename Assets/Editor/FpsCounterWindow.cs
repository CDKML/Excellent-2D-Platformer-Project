using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    public class FpsCounterWindow : EditorWindow
    {
        private bool showFps = true;
        private float deltaTime = 0.0f;
        private int frameCounter = 0;
        private int updateRate = 10; // Update FPS every 10 frames
        private int fps = 0;

        [MenuItem("Window/FPS Counter")]
        public static void ShowWindow()
        {
            GetWindow<FpsCounterWindow>("FPS Counter");
        }

        void OnGUI()
        {
            GUILayout.Label("FPS Counter Settings", EditorStyles.boldLabel);
            showFps = EditorGUILayout.Toggle("Show FPS", showFps);
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
                titleContent.text = $"FPS Counter ({fps})";
            }
            else
            {
                titleContent.text = "FPS Counter";
            }
        }
    }
}