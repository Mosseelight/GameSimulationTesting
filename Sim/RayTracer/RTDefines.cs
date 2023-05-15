using System;
using Microsoft.Xna.Framework;

namespace GameTesting
{
    public class ray
    {

        public Vector3 origin;
        public Vector3 direction;

        public ray()
        {

        }

        public ray(Vector3 _origin, Vector3 _direction)
        {
            origin = _origin;
            direction = _direction;
        }

        public Vector3 at (float t)
        {
            return origin + t * direction;
        }
    }

    public class CameraRT
    {
        public float aspectRatio = 0;
        public float viewportHeight = 2;
        public float viewportWidth = 0f;
        public float focalLength = 0;

        public Vector3 origin = Vector3.Zero;
        public Vector3 horizontal = Vector3.Zero;
        public Vector3 vertical = Vector3.Zero;
        public Vector3 lowerLeft = Vector3.Zero;

        public CameraRT(float _aspectRatio, float _viewportHeight, float _focalLength, Vector3 _origin)
        {
            aspectRatio = _aspectRatio;
            viewportHeight = _viewportHeight;
            viewportWidth = aspectRatio * viewportHeight;
            focalLength = _focalLength;
            origin = _origin;
            horizontal = new Vector3(viewportWidth, 0, 0);
            vertical = new Vector3(0, viewportHeight, 0);
            lowerLeft = origin - horizontal / 2 - vertical / 2 - new Vector3(0, 0, focalLength);
        }
    }
}