// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license

#if NET_4_6
using System.Runtime.CompilerServices;
#endif

using System;
using System.Collections.Generic;
using Artics.Math;
using UnityEngine;

namespace Artics.Physics.UnityPhisicsVisualizers
{
    public delegate void GetCoordinatesMethod(Collider2D collider, ref Vector2[] points);

    /// <summary>
    /// Set of static funtions to get raw poins of physics colliders2d. 
    /// </summary>
    public static class Collider2dPointsGetter
    {
        /// <summary>
        /// number of iterations to get points of circle
        /// </summary>
        public static uint CircleProximity = 20;

        /// <summary>
        /// bakes sin and cos values for performance
        /// </summary>
        private static TrigonometryBaker TrigonometryManager = new TrigonometryBaker();
        
        private static Dictionary<Type, GetCoordinatesMethod> TypeMethodDict;

        public static Dictionary<Type, GetCoordinatesMethod> GetTypeMethodDictionary()
        {
            if (TypeMethodDict == null)
                TypeMethodDict = new Dictionary<Type, GetCoordinatesMethod>()
                {
                    {typeof(BoxCollider2D), GetBoxCoordinates},
                    {typeof(PolygonCollider2D), GetPolygonCoordinates},
                    {typeof(EdgeCollider2D), GetEdgeCoordinates},
                    {typeof(CircleCollider2D), GetCircleCoordinates},
                    {typeof(CapsuleCollider2D), GetCapsuleCoordinates}
                };

            return TypeMethodDict;
        }

        public static void GetColliderPoints(Collider2D collider, ref Vector2[] points)
        {
            GetTypeMethodDictionary()[collider.GetType()](collider, ref points);
        }

        #region UncastedMethods

        public static void GetBoxCoordinates(Collider2D collider, ref Vector2[] points)
        {
            GetBoxCoordinates(collider as BoxCollider2D, ref points);
        }

        public static void GetPolygonCoordinates(Collider2D collider, ref Vector2[] points)
        {
            GetPolygonCoordinates(collider as PolygonCollider2D, ref points);
        }

        public static void GetEdgeCoordinates(Collider2D collider, ref Vector2[] points)
        {
            GetEdgeCoordinates(collider as EdgeCollider2D, ref points);
        }

        public static void GetCircleCoordinates(Collider2D collider, ref Vector2[] points)
        {
            GetCircleCoordinates(collider as CircleCollider2D, ref points);
        }

        public static void GetCapsuleCoordinates(Collider2D collider, ref Vector2[] points)
        {
            GetCapsuleCoordinates(collider as CapsuleCollider2D, ref points);
        }

        #endregion

        #region SimpleShapes

        /// <summary>
        /// Get coordinates of Box2DCollider
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="points"></param>
        public static void GetBoxCoordinates(BoxCollider2D collider, ref Vector2[] points)
        {
            PointsArraySizeValidation(ref points, 4);

            Vector2 offset = collider.offset;
            Vector2 size = collider.size * 0.5f;

            points[0].Set(offset.x - size.x, offset.y - size.y);
            points[1].Set(offset.x - size.x, offset.y + size.y);
            points[2].Set(offset.x + size.x, offset.y + size.y);
            points[3].Set(offset.x + size.x, offset.y - size.y);
        }

        /// <summary>
        /// get coordinates of <seealso cref="PolygonCollider2D"/>
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="points"></param>
        public static void GetPolygonCoordinates(PolygonCollider2D collider, ref Vector2[] points)
        {
            CalculatePolygonCoordinates(collider.points, collider.offset, ref points);
        }

        /// <summary>
        /// get coordinates of <seealso cref="EdgeCollider2D"/>
        /// </summary>
        /// <param name="collider">collider</param>
        /// <param name="points">input points</param>
        public static void GetEdgeCoordinates(EdgeCollider2D collider, ref Vector2[] points)
        {
            CalculatePolygonCoordinates(collider.points, collider.offset, ref points);
        }

        public static void PointsArraySizeValidation(ref Vector2[] points, int size)
        {
            if (points == null || points.Length != size)
                points = new Vector2[size];
        }

        private static void CalculatePolygonCoordinates(Vector2[] colliderPoints, Vector2 offset, ref Vector2[] points)
        {
            PointsArraySizeValidation(ref points, colliderPoints.Length);

            for (int i = 0; i < colliderPoints.Length; i++)
                points[i].Set(colliderPoints[i].x + offset.x, colliderPoints[i].y + offset.y);
        }

        #endregion

        #region Circle

        private static void ProximityCheck(ref uint proximity)
        {
            if (proximity == 0)
                proximity = CircleProximity;
        }

        private static void FillCirclePoint(Vector2[] points, Vector2 center, double radius, double[,] values, int idOffset, int valuesOffset, int times)
        {
            for (int i = 0; i < times; i++)
                points[idOffset + i].Set(center.x + (float) (radius * values[1, valuesOffset + i]), center.y + (float) (radius * values[0, valuesOffset + i]));
        }

        /// <summary>
        /// get coordinates of <seealso cref="CircleCollider2D"/>. You can use
        /// </summary>
        /// <param name="collider">collider</param>
        /// <param name="points">points array</param>
        /// <param name="customProximity">if set to 0 it uses <see cref="CircleProximity"/>. Otherwise it uses listed value to calculate circular coordinates</param>
        public static void GetCircleCoordinates(CircleCollider2D collider, ref Vector2[] points, uint customProximity = 0)
        {
            ProximityCheck(ref customProximity);
            PointsArraySizeValidation(ref points, (int) customProximity);

            var values = TrigonometryManager.GetProximityBake(customProximity);

            FillCirclePoint(points, collider.offset, collider.radius, values, 0, 0, (int) customProximity);
        }

        /// <summary>
        /// get coordinates by listing center and radius
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="points"></param>
        /// <param name="customProximity">if set to 0 it uses <see cref="CircleProximity"/>. Otherwise it uses listed value to calculate circular coordinates</param>
        public static void GetCircleCoordinates(Vector2 center, float radius, ref Vector2[] points, uint customProximity = 0)
        {
            ProximityCheck(ref customProximity);
            PointsArraySizeValidation(ref points, (int) customProximity);

            var values = TrigonometryManager.GetProximityBake(customProximity);

            FillCirclePoint(points, center, radius, values, 0, 0, (int) customProximity);
        }

        #endregion

        #region Capsule

        /// <summary>
        /// get coordinates of CapsuleCollider2D
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="points"></param>
        /// <param name="CustomProximity">if set to 0 it uses <see cref="CircleProximity"/>. Otherwise it uses listed value to calculate circular coordinates</param>
        public static void GetCapsuleCoordinates(CapsuleCollider2D collider, ref Vector2[] points, uint CustomProximity = 0, bool UseLossyScale = false)
        {
            ProximityCheck(ref CustomProximity);
            PointsArraySizeValidation(ref points, (int) CustomProximity + 4);

            var rest = CustomProximity % 4;

            if (rest > 0)
                CustomProximity += rest;

            float radius = 0;
            var startPosition = new Vector2();
            var endPosition = new Vector2();
            var size = collider.size * 0.5f;
            var scale = collider.transform.lossyScale;

            if (UseLossyScale)
            {
                size.x *= scale.x;
                size.y *= scale.y;
            }

            size.x = Mathf.Abs(size.x);
            size.y = Mathf.Abs(size.y);

            if (collider.direction == CapsuleDirection2D.Vertical)
            {
                CalculateCapsuleBoundsVertical(ref startPosition, ref endPosition, ref radius, size);
                AddCapsuleOffset(collider, ref startPosition, ref endPosition, scale, UseLossyScale);
                GetCapsulePointsVertical(startPosition, endPosition, radius, CustomProximity, points);
            }
            else
            {
                CalculateCapsuleBoundsHorizontal(ref startPosition, ref endPosition, ref radius, size);
                AddCapsuleOffset(collider, ref startPosition, ref endPosition, scale, UseLossyScale);
                GetCapsulePointsHorizontal(startPosition, endPosition, radius, CustomProximity, points);
            }
        }

#if NET_4_6
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static void CalculateCapsuleBoundsVertical(ref Vector2 startPosition, ref Vector2 endPosition, ref float radius, Vector2 size)
        {
            radius = size.x;

            if (size.y > size.x)
            {
                startPosition.y = size.y - radius;
                endPosition.y = startPosition.y * -1;
            }
        }

#if NET_4_6
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static void CalculateCapsuleBoundsHorizontal(ref Vector2 startPosition, ref Vector2 endPosition, ref float radius, Vector2 size)
        {
            radius = size.y;

            if (size.x > size.y)
            {
                startPosition.x = size.x - radius;
                endPosition.x = startPosition.x * -1;
            }
        }

#if NET_4_6
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static void AddCapsuleOffset(CapsuleCollider2D collider, ref Vector2 startPosition, ref Vector2 endPosition, Vector2 scale, bool useLossyScale)
        {
            if (useLossyScale)
                scale.Scale(collider.offset);
            else
                scale = collider.offset;

            startPosition += scale;
            endPosition += scale;
        }

#if NET_4_6
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static void GetCapsulePointsVertical(Vector2 startPosition, Vector2 endPosition, float radius, uint customProximity, Vector2[] points)
        {
            var values = TrigonometryManager.GetProximityBake(customProximity);
            var half = Mathf.RoundToInt(customProximity / 2f);

            points[0].Set(startPosition.x + radius, endPosition.y);
            points[1].Set(startPosition.x + radius, startPosition.y);

            FillCirclePoint(points, startPosition, radius, values, 2, 0, half);

            points[half + 2].Set(startPosition.x - radius, startPosition.y);
            points[half + 3].Set(startPosition.x - radius, endPosition.y);

            FillCirclePoint(points, endPosition, radius, values, half + 4, half, half);
        }

#if NET_4_6
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static void GetCapsulePointsHorizontal(Vector2 startPosition, Vector2 endPosition, float radius, uint customProximity, Vector2[] points)
        {
            var values = TrigonometryManager.GetProximityBake(customProximity);
            var quart = Mathf.RoundToInt(customProximity / 4f);

            points[0].Set(endPosition.x, endPosition.y - radius);
            points[1].Set(startPosition.x, startPosition.y - radius);

            var idOffset = 2;

            FillCirclePoint(points, startPosition, radius, values, idOffset, quart * 3, quart);
            idOffset += quart;
            FillCirclePoint(points, startPosition, radius, values, idOffset, 0, quart);
            idOffset += quart;

            points[idOffset].Set(startPosition.x, startPosition.y + radius);
            points[idOffset + 1].Set(endPosition.x, endPosition.y + radius);

            idOffset += 2;

            FillCirclePoint(points, endPosition, radius, values, idOffset, quart * 1, quart);
            idOffset += quart;
            FillCirclePoint(points, endPosition, radius, values, idOffset, quart * 2, quart);
        }

        #endregion
    }
}