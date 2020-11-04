// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license
using System;
using UnityEngine;

namespace Artics.Physics.UnityPhisicsVisualizers.Base
{
    /// <summary>
    /// Base class for Physics2dVisualizers
    /// </summary>
    public class ShapeVisualizer : BaseVisualizer
    {
        public bool IsClosed;
        public bool RigidBodyAttached;
        
        protected Vector2[] Points;
        protected Vector2 Offset;
        protected Collider2D BaseCollider;

        public override void Init()
        {
            BaseCollider = GetComponent<Collider2D>();
            base.Init();
        }

        public override void UpdateBounds()
        {
            if (BaseCollider != null)
                RigidBodyAttached = BaseCollider.attachedRigidbody != null;
        }

        protected override void MultiplyMatrix()
        {
            Matrix4x4 matrix = GetMatrix();

            for (var i = 0; i < Points.Length; i++)
                MultipliedPoints[i] = matrix.MultiplyPoint(Points[i] + Offset);
        }

        protected override void Draw()
        {
            DrawPoints(MultipliedPoints, IsClosed, Color);
            DrawEdgesIdHandles();
        }

        public override IDrawData CreateDrawData()
        {
            Shape2DDrawData data = new Shape2DDrawData();
            data.Color = Color;
            data.MultipliedPoints = new Vector2[PointsLenght];
            data.IsClosed = IsClosed;

            if (MultipliedPoints != null)
                Array.Copy(MultipliedPoints, data.MultipliedPoints, PointsLenght);

            return data;
        }

        public static void DrawPoints(Vector2[] MultipliedPoints, bool IsClosed, Color color)
        {
            Gizmos.color = color;
            var PointsLenght = MultipliedPoints.Length - 1;

            for (var i = 0; i < PointsLenght; i++)
                Gizmos.DrawLine(MultipliedPoints[i], MultipliedPoints[i + 1]);

            if (IsClosed && PointsLenght > 0)
                Gizmos.DrawLine(MultipliedPoints[0], MultipliedPoints[PointsLenght]);
        }

        protected Matrix4x4 GetMatrix()
        {
            if (RigidBodyAttached)
                return Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            else
                return transform.localToWorldMatrix;
        }
    }

    /// <summary>
    /// struct to store calculated data and draw it outside of class
    /// </summary>
    [System.Serializable]
    public struct Shape2DDrawData : IDrawData
    {
        public Vector2[] MultipliedPoints;
        public Color Color;
        public bool IsClosed;

        public void Draw()
        {
            Draw(Color);
        }

        public void Draw(Color color)
        {
            ShapeVisualizer.DrawPoints(MultipliedPoints, IsClosed, Color);
        }
    }
}
