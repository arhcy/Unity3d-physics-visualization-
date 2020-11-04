// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license

using UnityEngine;

namespace Artics.Math
{
    public static class TransformUtils
    {
        public static Matrix4x4 GetGlobalTransformMatrix(this Transform transform, Vector3 position)
        {
            return Matrix4x4.TRS(position, transform.rotation, transform.lossyScale);
        }

        public static Matrix4x4 GetGlobalTransformMatrix2(this Transform transform)
        {
            return LocalToWorld(transform);
        }

        public static Matrix4x4 LocalToWorld(Transform t)
        {
            if (t == null)
                return Matrix4x4.identity;
            else
                return LocalToWorld(t.parent) * LocalToParent(t);
        }

        public static Matrix4x4 LocalToParent(Transform transform)
        {
            return TRS(transform.localPosition, transform.localEulerAngles, transform.localScale);
        }

        // Matrix4x4.TRS(trans, Quaternion.Euler(euler), scale)
        public static Matrix4x4 TRS(Vector3 trans, Vector3 euler, Vector3 scale)
        {
            return Translate(trans) * Rotate(euler) * Scale(scale);
        }

        // Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(euler), Vector3.one)
        public static Matrix4x4 Rotate(Vector3 euler)
        {
            return RotateY(euler.y) * RotateX(euler.x) * RotateZ(euler.z);
        }

        // Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(deg, 0, 0), Vector3.one)
        public static Matrix4x4 RotateX(float deg)
        {
            var rad = deg * Mathf.Deg2Rad;
            var sin = Mathf.Sin(rad);
            var cos = Mathf.Cos(rad);
            var mat = Matrix4x4.identity;
            mat.m11 = cos;
            mat.m12 = -sin;
            mat.m21 = sin;
            mat.m22 = cos;
            return mat;
        }

        // Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, deg, 0), Vector3.one)
        public static Matrix4x4 RotateY(float deg)
        {
            var rad = deg * Mathf.Deg2Rad;
            var sin = Mathf.Sin(rad);
            var cos = Mathf.Cos(rad);
            var mat = Matrix4x4.identity;

            mat.m22 = cos;
            mat.m20 = -sin;
            mat.m02 = sin;
            mat.m00 = cos;

            return mat;
        }


        // Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, deg), Vector3.one)
        public static Matrix4x4 RotateZ(float deg)
        {
            var rad = deg * Mathf.Deg2Rad;
            var sin = Mathf.Sin(rad);
            var cos = Mathf.Cos(rad);
            var mat = Matrix4x4.identity;

            mat.m00 = cos;
            mat.m01 = -sin;
            mat.m10 = sin;
            mat.m11 = cos;

            return mat;
        }

        // Matrix4x4.Scale(scale)
        public static Matrix4x4 Scale(Vector3 scale)
        {
            var mat = Matrix4x4.identity;

            mat.m00 = scale.x;
            mat.m11 = scale.y;
            mat.m22 = scale.z;

            return mat;
        }

        // Matrix4x4.TRS(vec, Quaternion.identity, Vector3.one)
        public static Matrix4x4 Translate(Vector3 vec)
        {
            var mat = Matrix4x4.identity;

            mat.m03 = vec.x;
            mat.m13 = vec.y;
            mat.m23 = vec.z;

            return mat;
        }
    }
}