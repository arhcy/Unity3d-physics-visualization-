// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license
#if NET_4_6
using System.Runtime.CompilerServices;
#endif

using UnityEngine;

namespace Artics.Physics.UnityPhisicsVisualizers
{
    public class PolygonTriangulator
    {
        public static void TriangulateAsLine(Vector2[] points, Mesh mesh, float thickness, bool closed = false)
        {
            var last = points.Length - 1;
            var len = points.Length * 4;

            if (!closed)
                len -= 4;

            var linePoints = new Vector3[len];

            if (points.Length > 1)
            {
                for (var i = 0; i < last; i++)
                    LinePointsStep(i, i + 1, i * 4, thickness, linePoints, points);
            }

            if (closed)
                LinePointsStep(last, 0, last * 4, thickness, linePoints, points);

            len = closed ? points.Length * 12 + 6 : last * 12;
            var triangles = new int[len];

            len = closed ? last : last - 1;
            int tr = 0, pt = 0;

            for (var i = 0; i <= last; i++)
            {
                tr = i * 12;
                pt = i * 4;

                //z -1
                triangles[tr] = pt + 0;
                triangles[tr + 1] = pt + 1;
                triangles[tr + 2] = pt + 3;

                triangles[tr + 3] = pt + 3;
                triangles[tr + 4] = pt + 2;
                triangles[tr + 5] = pt + 0;

                if (i == len)
                    break;

                triangles[tr + 6] = pt + 4;
                triangles[tr + 7] = pt + 2;
                triangles[tr + 8] = pt + 3;

                triangles[tr + 9] = pt + 3;
                triangles[tr + 10] = pt + 5;
                triangles[tr + 11] = pt + 4;
            }

            if (closed)
            {
                tr += 6;

                triangles[tr + 6] = 0;
                triangles[tr + 7] = pt + 2;
                triangles[tr + 8] = pt + 3;

                triangles[tr + 9] = pt + 3;
                triangles[tr + 10] = 1;
                triangles[tr + 11] = 0;
            }

            mesh.vertices = linePoints;
            mesh.SetTriangles(triangles, 0);
        }

#if NET_4_6
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static void LinePointsStep(int p1, int p2, int arrIndex, float thickness, Vector3[] linePoints, Vector2[] points)
        {
            var calculatedAngle = CalculateSinCos(points[p1], points[p2]);

            AddCalculateLinePoints(linePoints, points[p1], arrIndex, thickness, calculatedAngle.y, -calculatedAngle.x);
            AddCalculateLinePoints(linePoints, points[p2], arrIndex + 2, thickness, calculatedAngle.y, -calculatedAngle.x);
        }

#if NET_4_6
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static void AddCalculateLinePoints(Vector3[] points, Vector3 start, int id, float thickness, float cos, float sin)
        {
            points[id].x = start.x + cos * thickness;
            points[id].y = start.y + sin * thickness;
            points[id + 1].x = start.x - cos * thickness;
            points[id + 1].y = start.y - sin * thickness;
        }

#if NET_4_6
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static Vector2 CalculateSinCos(Vector2 pointA, Vector2 pointB)
        {
            var result = new Vector2(pointB.x - pointA.x, pointB.y - pointA.y);
            var magnitude = Mathf.Sqrt(result.x * result.x + result.y * result.y);
            result.x /= magnitude;
            result.y /= magnitude;

            return result;
        }
    }
}
