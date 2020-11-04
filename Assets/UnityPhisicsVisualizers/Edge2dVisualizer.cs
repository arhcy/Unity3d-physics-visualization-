// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license
using UnityEngine;
using Artics.Physics.UnityPhisicsVisualizers.Base;

namespace Artics.Physics.UnityPhisicsVisualizers
{
    /// <summary>
    /// Draws gizmos of <seealso cref="EdgeCollider2D"/> which attached for current GameObject.
    /// You can enable and disable visualization by <see cref = "IsVisible" /> parameter
    /// If collider don't change it's Offset, Size, and Direction - you can disable <see cref="DynamicBounds"/> to increase performance
    /// </summary>

    [RequireComponent(typeof(EdgeCollider2D))]

    public class Edge2dVisualizer : Polygon2dVisualizer
    {
        public override void Init()
        {
            base.Init();

            IsClosed = false;
        }

        protected override void Draw()
        {
            var edgeRadius = GetEdgeRadius();

            if (edgeRadius == 0)
            {
                base.Draw();
            }
            else
            {
                DrawCustomShape(MultipliedPoints, edgeRadius, Color);
            }
        }

        public static void DrawCustomShape(Vector2[] multipliedPoints, float edgeRadius, Color color)
        {
            Gizmos.color = color;
            Vector2[] points = null;

            for (int i = 0; i < multipliedPoints.Length - 1; i++)
            {
                Vector2 helperVector = multipliedPoints[i + 1] - multipliedPoints[i];
                helperVector.Normalize();
                helperVector *= edgeRadius;

                Gizmos.DrawLine(new Vector3(multipliedPoints[i].x - helperVector.y, multipliedPoints[i].y + helperVector.x), new Vector3(multipliedPoints[i + 1].x - helperVector.y, multipliedPoints[i + 1].y + helperVector.x));
                Gizmos.DrawLine(new Vector3(multipliedPoints[i].x + helperVector.y, multipliedPoints[i].y - helperVector.x), new Vector3(multipliedPoints[i + 1].x + helperVector.y, multipliedPoints[i + 1].y - helperVector.x));

                DrawCircle(multipliedPoints[i], edgeRadius, ref points, color);
            }

            DrawCircle(multipliedPoints[multipliedPoints.Length - 1], edgeRadius, ref points, color);
        }

        protected static void DrawCircle(Vector2 center, float radius, ref Vector2[] points, Color color)
        {
            Collider2dPointsGetter.GetCircleCoordinates(center, radius, ref points);
            DrawPoints(points, false, color);
            Gizmos.DrawLine(points[0], points[points.Length - 1]);
        }

        protected float GetEdgeRadius()
        {
            return ((EdgeCollider2D) Collider).edgeRadius;
        }

        public override IDrawData CreateDrawData()
        {
            var baseData = (Shape2DDrawData)base.CreateDrawData();
            var data = new Edge2DDrawData()
            {
                BaseData = baseData,
                EdgeRadius = GetEdgeRadius()
            };

            return data;
        }
    }

    /// <summary>
    /// struct to store calculated data and draw it outside of class
    /// </summary>
    [System.Serializable]
    public struct Edge2DDrawData : IDrawData
    {
        public Shape2DDrawData BaseData;
        public float EdgeRadius;

        public void Draw()
        {
            Draw(BaseData.Color);
        }

        public void Draw(Color color)
        {
            if (EdgeRadius == 0)
                BaseData.Draw();
            else
                Edge2dVisualizer.DrawCustomShape(BaseData.MultipliedPoints, EdgeRadius, color);
        }
    }
}