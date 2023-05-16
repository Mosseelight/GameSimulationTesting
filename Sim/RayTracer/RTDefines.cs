using System;
using Microsoft.Xna.Framework;

namespace GameTesting
{
    public class Ray
    {

        public VectorD3 origin;
        public VectorD3 direction;

        public Ray(VectorD3 _origin, VectorD3 _direction)
        {
            origin = _origin;
            direction = _direction;
        }

        public VectorD3 at (float t)
        {
            return origin + (direction * t);
        }
    }

    public class CameraRT
    {
        public float aspectRatio = 0;
        public float viewportHeight = 2;
        public float viewportWidth = 0f;
        public float focalLength = 0;

        public VectorD3 origin = VectorD3.Zero();
        public VectorD3 horizontal = VectorD3.Zero();
        public VectorD3 vertical = VectorD3.Zero();
        public VectorD3 lowerLeft = VectorD3.Zero();

        public CameraRT(float _aspectRatio, float _viewportHeight, float _focalLength, VectorD3 _origin)
        {
            aspectRatio = _aspectRatio;
            viewportHeight = _viewportHeight;
            viewportWidth = aspectRatio * viewportHeight;
            focalLength = _focalLength;
            origin = _origin;
            horizontal = new VectorD3(viewportWidth, 0, 0);
            vertical = new VectorD3(0, viewportHeight, 0);
            lowerLeft = origin - horizontal / 2 - vertical / 2 - new VectorD3(0, 0, focalLength);
        }
    }

    public class ColorRT
    {
        public Color WriteColor(Color color)
        {
            return new Color((int)(255.999 * color.R), (int)(255.999 * color.G), (int)(255.999 * color.B));
        }
    }


    //needed for extra percision
    public class VectorD3
    {
        public double x = 0;
        public double y = 0;
        public double z = 0;


        public VectorD3(double _x, double _y, double _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public static VectorD3 Identity()
        {
            return new VectorD3(1, 1, 1);
        }
        public static VectorD3 Zero()
        {
            return new VectorD3(0, 0, 0);
        }

        public static double LengthSqr(VectorD3 input)
        {
            return input.x * input.x + input.y * input.y + input.z * input.z;
        }

        public static double Length(VectorD3 input)
        {
            return Math.Sqrt(LengthSqr(input));
        }

        public static double dot(VectorD3 input1, VectorD3 input2)
        {
            return input1.x * input2.x + input1.y * input2.y + input1.z * input2.z;
        }

        public static VectorD3 Cross(VectorD3 input1, VectorD3 input2)
        {
            return new VectorD3(input1.y * input2.z - input1.z * input2.y, input1.z * input2.x - input1.x * input2.z, input1.x * input2.y - input1.y * input2.x);
        }

        public static VectorD3 Normalize(VectorD3 input)
        {
            return input / VectorD3.Length(input);
        }

        public static VectorD3 operator +(VectorD3 vector1, VectorD3 vector2)
        {
            vector1.x += vector2.x;
            vector1.y += vector2.y;
            vector1.z += vector2.z;
            return vector1;
        }

        public static VectorD3 operator -(VectorD3 vector1, VectorD3 vector2)
        {
            vector1.x -= vector2.x;
            vector1.y -= vector2.y;
            vector1.z -= vector2.z;
            return vector1;
        }

        public static VectorD3 operator -(VectorD3 vector1)
        {
            vector1.x = -vector1.x;
            vector1.y = -vector1.y;
            vector1.z = -vector1.z;
            return vector1;
        }

        public static VectorD3 operator *(VectorD3 vector1, VectorD3 vector2)
        {
            vector1.x *= vector2.x;
            vector1.y *= vector2.y;
            vector1.z *= vector2.z;
            return vector1;
        }
        public static VectorD3 operator /(VectorD3 vector1, VectorD3 vector2)
        {
            vector1.x /= vector2.x;
            vector1.y /= vector2.y;
            vector1.z /= vector2.z;
            return vector1;
        }

        public static bool operator !=(VectorD3 vector1, VectorD3 vector2)
        {
            return vector1.x != vector2.x || vector1.y != vector2.y || vector1.z != vector2.z;
        }

        public static bool operator ==(VectorD3 vector1, VectorD3 vector2)
        {
            return vector1.x == vector2.x && vector1.y == vector2.y && vector1.z == vector2.z;
        }


        //float value
        public static VectorD3 operator +(VectorD3 vector1, float value)
        {
            vector1.x += value;
            vector1.y += value;
            vector1.z += value;
            return vector1;
        }
        public static VectorD3 operator -(VectorD3 vector1, float value)
        {
            vector1.x -= value;
            vector1.y -= value;
            vector1.z -= value;
            return vector1;
        }
        public static VectorD3 operator *(VectorD3 vector1, float value)
        {
            vector1.x *= value;
            vector1.y *= value;
            vector1.z *= value;
            return vector1;
        }
        public static VectorD3 operator /(VectorD3 vector1, float value)
        {
            vector1.x /= value;
            vector1.y /= value;
            vector1.z /= value;
            return vector1;
        }

        //double value
        public static VectorD3 operator +(VectorD3 vector1, double value)
        {
            vector1.x += value;
            vector1.y += value;
            vector1.z += value;
            return vector1;
        }
        public static VectorD3 operator -(VectorD3 vector1, double value)
        {
            vector1.x -= value;
            vector1.y -= value;
            vector1.z -= value;
            return vector1;
        }
        public static VectorD3 operator *(VectorD3 vector1, double value)
        {
            vector1.x *= value;
            vector1.y *= value;
            vector1.z *= value;
            return vector1;
        }
        public static VectorD3 operator /(VectorD3 vector1, double value)
        {
            vector1.x /= value;
            vector1.y /= value;
            vector1.z /= value;
            return vector1;
        }


        //int value
        public static VectorD3 operator +(VectorD3 vector1, int value)
        {
            vector1.x += value;
            vector1.y += value;
            vector1.z += value;
            return vector1;
        }
        public static VectorD3 operator -(VectorD3 vector1, int value)
        {
            vector1.x -= value;
            vector1.y -= value;
            vector1.z -= value;
            return vector1;
        }
        public static VectorD3 operator *(VectorD3 vector1, int value)
        {
            vector1.x *= value;
            vector1.y *= value;
            vector1.z *= value;
            return vector1;
        }
        public static VectorD3 operator /(VectorD3 vector1, int value)
        {
            vector1.x /= value;
            vector1.y /= value;
            vector1.z /= value;
            return vector1;
        }
        
        
    }
}