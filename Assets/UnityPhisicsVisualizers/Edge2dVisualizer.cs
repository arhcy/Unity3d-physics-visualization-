// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license
using UnityEngine;
using Artics.Physics.UnityPhisicsVisualizers.Base;

namespace Artics.Physics.UnityPhisicsVisualizers
{
    /// <summary>
    /// Draws gizmos of <seealso cref="EdgeCollider2D"/> which attached for current GameObject.
    /// You can enable and disable visualization by <see cref = "IsVisible" /> parameter
    /// If collider don't change his Offset, Size, and Direction - you can disable <see cref="DynamicBounds"/> to increase performance
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
            float edgeRadius = GetEdgeRadius();

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
            Vector2[] Points = null;

            for (int i = 0; i < multipliedPoints.Length - 1; i++)
            {
                Vector2 HelperVector = multipliedPoints[i + 1] - multipliedPoints[i];
                HelperVector.Normalize();
                HelperVector *= edgeRadius;

                Gizmos.DrawLine(new Vector3(multipliedPoints[i].x - HelperVector.y, multipliedPoints[i].y + HelperVector.x), new Vector3(multipliedPoints[i + 1].x - HelperVector.y, multipliedPoints[i + 1].y + HelperVector.x));
                Gizmos.DrawLine(new Vector3(multipliedPoints[i].x + HelperVector.y, multipliedPoints[i].y - HelperVector.x), new Vector3(multipliedPoints[i + 1].x + HelperVector.y, multipliedPoints[i + 1].y - HelperVector.x));

                DrawCircle((Vector2)multipliedPoints[i], edgeRadius, ref Points, color);
            }

            DrawCircle((Vector2)multipliedPoints[multipliedPoints.Length - 1], edgeRadius, ref Points, color);
        }

        protected static void DrawCircle(Vector2 center, float radius, ref Vector2[] points, Color color)
        {
            Collider2dPointsGetter.GetCircleCoordinates(center, radius, ref points);
            ShapeVisualizer.DrawPoints(points, false, color);
            Gizmos.DrawLine(points[0], points[points.Length - 1]);
        }

        protected float GetEdgeRadius()
        {
            return (Collider as EdgeCollider2D).edgeRadius;
        }

        public override IDrawData CreateDrawData()
        {
            var baseData = (Shape2DDrawData)base.CreateDrawData();
            Edge2DDrawData data = new Edge2DDrawData();
            data.BaseData = baseData;
            data.EdgeRadius = GetEdgeRadius();

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