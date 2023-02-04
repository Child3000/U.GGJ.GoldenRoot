using System;
using UnityEngine;

namespace GoldenRoot
{
    public class WorldCanvasFaceCamera : MonoBehaviour
    {
        protected virtual void Update()
        {
            var lookDir = (transform.position - Camera.main.transform.position).normalized;
            lookDir.x = lookDir.z = 0f;
            transform.rotation = Quaternion.LookRotation(-lookDir, Vector3.forward);
        }
    }
}