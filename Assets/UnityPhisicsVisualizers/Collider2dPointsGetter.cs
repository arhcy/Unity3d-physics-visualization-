// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license

using UnityEngine;
using artics.Math;

namespace artics.UnityPhisicsVisualizers
{
    /// <summary>
    /// set of static funtions solution to get raw poins of Unity physics 2d colliders. 
    /// </summary>
    public class Collider2dPointsGetter
    {
        /// <summary>
        /// number of iterations to get points of circle
        /// </summary>
        public static uint CircleProximity = 20;

        /// <summary>
        /// bakes sin and cos values for performance
        /// </summary>
        public static TrigonometryBaker TrigonometryManager = new TrigonometryBaker();


        #region SimpleShapes
        /// <summary>
        /// Get coordinates of Box2DCollider
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="points"></param>
        public static void GetBoxCoordinates(BoxCollider2D collider, ref Vector2[] points)
        {
            PointsArraySizevalidation(ref points, 4);

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
            CalculatePolygonCoodinates(collider.points, collider.offset, ref points);
        }

        /// <summary>
        /// get coordinates of <seealso cref="EdgeCollider2D"/>
        /// </summary>
        /// <param name="collider">collider</param>
        /// <param name="points">input points</param>
        public static void GetEdgeCoordinates(EdgeCollider2D collider, ref Vector2[] points)
        {
            CalculatePolygonCoodinates(collider.points, collider.offset, ref points);
        }

        protected static void PointsArraySizevalidation(ref Vector2[] points, int size)
        {
            if (points == null || points.Length != size)
                points = new Vector2[size];
        }
        
        protected static void CalculatePolygonCoodinates(Vector2[] colliderPoints, Vector2 offset,  ref Vector2[] points)
        {
            PointsArraySizevalidation(ref points, colliderPoints.Length);

            for (int i = 0; i < colliderPoints.Length; i++)
                points[i].Set(colliderPoints[i].x + offset.x, colliderPoints[i].y + offset.y);
        }

        #endregion

        #region Circle

        protected static void ProximityCheck(ref uint proximity)
        {
            if (proximity == 0)
                proximity = CircleProximity;
        }


        protected static void FillCirclePoint(Vector2[] points, Vector2 center, double radius, double[][] values, int IdOffset, int ValuesOffset, int times)
        {
            for (int i = 0; i < times; i++)
                points[IdOffset + i].Set(center.x + (float)(radius * values[1][ValuesOffset + i]), center.y + (float)(radius * values[0][ValuesOffset + i]));
        }

        /// <summary>
        /// get coordinates of <seealso cref="CircleCollider2D"/>. You can use
        /// </summary>
        /// <param name="collider">collider</param>
        /// <param name="points">points array</param>
        /// <param name="CustomProximity">if set to 0 it uses <see cref="CircleProximity"/>. Otherwise it uses listed value to calculate circular coordinates</param>
        public static void GetCircleCoordinates(CircleCollider2D collider, ref Vector2[] points, uint CustomProximity = 0)
        {
            ProximityCheck(ref CustomProximity);
            PointsArraySizevalidation(ref points, (int)CustomProximity);

            double[][] values = TrigonometryManager.GetProximityBake(CustomProximity);

            FillCirclePoint(points, collider.offset, collider.radius, values, 0, 0, (int)CustomProximity);
        }

        /// <summary>
        /// get coordinates by listing center and radius
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="points"></param>
        /// <param name="CustomProximity">if set to 0 it uses <see cref="CircleProximity"/>. Otherwise it uses listed value to calculate circular coordinates</param>
        public static void GetCircleCoordinates(Vector2 center, float radius, ref Vector2[] points, uint CustomProximity = 0)
        {
            ProximityCheck(ref CustomProximity);
            PointsArraySizevalidation(ref points, (int)CustomProximity);

            double[][] values = TrigonometryManager.GetProximityBake(CustomProximity);

            FillCirclePoint(points, center, radius, values, 0, 0, (int)CustomProximity);
        }
        #endregion

        #region Capsule
        /// <summary>
        /// get coordinates of CapsuleCollider2D
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="points"></param>
        /// <param name="CustomProximity">if set to 0 it uses <see cref="CircleProximity"/>. Otherwise it uses listed value to calculate circular coordinates</param>
        public static void GetCapsuleCoordinates(CapsuleCollider2D collider, ref Vector2[] points, uint CustomProximity = 0)
        {
            ProximityCheck(ref CustomProximity);
            PointsArraySizevalidation(ref points, (int)CustomProximity + 4);

            uint rest = CustomProximity % 4;

            if (rest > 0)
                CustomProximity += rest;

            Vector2 StartPosition = new Vector2();
            Vector2 EndPosition = new Vector2();
            float radius = 0;

            if (collider.direction == CapsuleDirection2D.Vertical)
            {
                CalculateCapsuleBoundsVertical(collider, ref StartPosition, ref EndPosition, ref radius);
                GetCapsulePointsVertical(StartPosition, EndPosition, radius, CustomProximity, points);
            }
            else
            {
                CalculateCapsuleBoundsHorizontal(collider, ref StartPosition, ref EndPosition, ref radius);
                GetCapsulePointsHorizontal(StartPosition, EndPosition, radius, CustomProximity, points);
            }
        }

        protected static void CalculateCapsuleBoundsVertical(CapsuleCollider2D collider, ref Vector2 StartPosition, ref Vector2 EndPosition, ref float Radius)
        {
            Vector2 size = collider.size;
            Radius = Mathf.Abs(size.x * 0.5f * collider.transform.localScale.x);

            StartPosition.x = 0;
            EndPosition.x = 0;

            StartPosition.y = Mathf.Max(0, size.y * 0.5f - Radius);
            EndPosition.y = Mathf.Min(0, StartPosition.y * -1);

            StartPosition += collider.offset;
            EndPosition += collider.offset;
        }

        protected static void CalculateCapsuleBoundsHorizontal(CapsuleCollider2D collider, ref Vector2 StartPosition, ref Vector2 EndPosition, ref float Radius)
        {
            Vector2 size = collider.size;
            Radius = Mathf.Abs(size.y * 0.5f * collider.transform.localScale.x);

            StartPosition.y = 0;
            EndPosition.y = 0;

            StartPosition.x = Mathf.Max(0, size.x * 0.5f - Radius);
            EndPosition.x = Mathf.Min(0, StartPosition.x * -1);

            StartPosition += collider.offset;
            EndPosition += collider.offset;
        }

        protected static void GetCapsulePointsVertical(Vector2 StartPosition, Vector2 EndPosition, float radius, uint CustomProximity, Vector2[] points)
        {
            double[][] values = TrigonometryManager.GetProximityBake(CustomProximity);
            int half = Mathf.RoundToInt(CustomProximity / 2);

            points[0].Set(StartPosition.x + radius, EndPosition.y);
            points[1].Set(StartPosition.x + radius, StartPosition.y);
            
            FillCirclePoint(points, StartPosition, radius, values, 2, 0, half);

            points[half + 2].Set(StartPosition.x - radius, StartPosition.y);
            points[half + 3].Set(StartPosition.x - radius, EndPosition.y);

            FillCirclePoint(points, EndPosition, radius, values, half + 4, half, half);
        }

        protected static void GetCapsulePointsHorizontal(Vector2 StartPosition, Vector2 EndPosition, float radius, uint CustomProximity, Vector2[] points)
        {
            double[][] values = TrigonometryManager.GetProximityBake(CustomProximity);
            int quart = Mathf.RoundToInt(CustomProximity / 4);

            points[0].Set(EndPosition.x, EndPosition.y - radius);
            points[1].Set(StartPosition.x, StartPosition.y - radius);

            int idOffset = 2;

            FillCirclePoint(points, StartPosition, radius, values, idOffset, quart * 3, quart);
            idOffset += quart;
            FillCirclePoint(points, StartPosition, radius, values, idOffset, 0, quart);
            idOffset += quart;

            points[idOffset].Set(StartPosition.x, StartPosition.y + radius);
            points[idOffset + 1].Set(EndPosition.x, EndPosition.y + radius);

            idOffset += 2;

            FillCirclePoint(points, EndPosition, radius, values, idOffset, quart * 1, quart);
            idOffset += quart;
            FillCirclePoint(points, EndPosition, radius, values, idOffset, quart * 2, quart);
        }
        #endregion
    }
}
