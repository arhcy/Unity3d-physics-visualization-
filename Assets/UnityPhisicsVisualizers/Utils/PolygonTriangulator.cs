// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license
#if NET_4_6
using System.Runtime.CompilerServices;
#endif

using UnityEngine;

namespace Artics.Physics.UnityPhisicsVisualizers
{
    public class PolygonTriangulator
    {
        public static void TriangulateAsLine(Vector2[] points, Mesh mesh, float thikness, bool closed = false)
        {
            int last = points.Length - 1;
            int len = points.Length * 4;

            if (!closed)
                len -= 4;

            Vector3[] LinePoints = new Vector3[len];

            if (points.Length > 1)
            {
                for (int i = 0; i < last; i++)
                    LinePointsStep(i, i + 1, i * 4, thikness, LinePoints, points);
            }

            if (closed)
                LinePointsStep(last, 0, last * 4, thikness, LinePoints, points);

            len = closed ? points.Length * 12 + 6 : last * 12;
            int[] triangles = new int[len];

            len = closed ? last : last - 1;
            int tr = 0, pt = 0;

            for (int i = 0; i <= last; i++)
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

                //z +1
                /*triangles[tr] = pt + 3;
                triangles[tr + 1] = pt + 1;
                triangles[tr + 2] = pt + 0;

                triangles[tr + 3] = pt + 2;
                triangles[tr + 4] = pt + 3;
                triangles[tr + 5] = pt + 0;*/

                if (i == len)
                    break;

                /*triangles[tr + 6] = pt + 3;
                triangles[tr + 7] = pt + 2;
                triangles[tr + 8] = pt + 4;

                triangles[tr + 9] = pt + 5;
                triangles[tr + 10] = pt + 4;
                triangles[tr + 11] = pt + 3;*/

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

            mesh.vertices = LinePoints;
            mesh.SetTriangles(triangles, 0);
        }

#if NET_4_6
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected static void LinePointsStep(int p1, int p2, int arrIndex, float thikness, Vector3[] linePoints, Vector2[] points)
        {
            Vector2 CalculatedAngle = CalculateSinCos(points[p1], points[p2]);

            AddCalculateLinePoints(linePoints, points[p1], arrIndex, thikness, CalculatedAngle.y, -CalculatedAngle.x);
            AddCalculateLinePoints(linePoints, points[p2], arrIndex + 2, thikness, CalculatedAngle.y, -CalculatedAngle.x);
        }

#if NET_4_6
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected static void AddCalculateLinePoints(Vector3[] points, Vector3 start, int id, float thikness, float cos, float sin)
        {
            points[id].x = start.x + cos * thikness;
            points[id].y = start.y + sin * thikness;
            points[id + 1].x = start.x - cos * thikness;
            points[id + 1].y = start.y - sin * thikness;
        }

#if NET_4_6
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        protected static Vector2 CalculateSinCos(Vector2 pointA, Vector2 pointB)
        {
            Vector2 rezult = new Vector2(pointB.x - pointA.x, pointB.y - pointA.y);
            float magnitude = Mathf.Sqrt(rezult.x * rezult.x + rezult.y * rezult.y);
            rezult.x /= magnitude;
            rezult.y /= magnitude;

            return rezult;
        }
    }
}
