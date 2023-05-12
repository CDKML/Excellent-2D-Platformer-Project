using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Debug
{
    public class PlayerLineTrail : MonoBehaviour
    {
        public float eraseTime = 2.0f;
        private LineRenderer line;
        private List<Vector3> points;

        void Start()
        {
            line = gameObject.AddComponent<LineRenderer>();
            line.renderingLayerMask = 1;
            line.startWidth = 0.1f; // You can adjust this value
            line.endWidth = 0.1f; // You can adjust this value
            line.positionCount = 0;
            line.material = new Material(Shader.Find("Particles/Standard Unlit"));
            line.startColor = Color.white; // You can adjust this value
            line.endColor = Color.white; // You can adjust this value
            points = new List<Vector3>();
        }

        void Update()
        {
            Vector3 position = transform.position;
            position.z = -1; // Set Z to a value closer to the camera
            points.Add(position);
            
            line.positionCount = points.Count;
            line.SetPositions(points.ToArray());
            StartCoroutine(EraseTrail());
        }

        IEnumerator EraseTrail()
        {
            yield return new WaitForSeconds(eraseTime);
            if (points.Count > 0)
            {
                points.RemoveAt(0);
                line.positionCount = points.Count;
                line.SetPositions(points.ToArray());
            }
        }
    }

}
