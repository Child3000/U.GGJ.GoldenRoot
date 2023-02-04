using System;
using UnityEngine;

namespace GoldenRoot
{
    public class WorldCanvasFaceCamera : MonoBehaviour
    {
        private Camera _MainCam;

        public Camera MainCam
        {
            get
            {
                if (_MainCam == null) _MainCam = Camera.main;
                return _MainCam;
            }
        }
        protected virtual void Update()
        {
            var lookDir = (transform.position - MainCam.transform.position).normalized;
            lookDir.x = lookDir.z = 0f;
            transform.rotation = Quaternion.LookRotation(-lookDir, Vector3.forward);
        }
    }
}