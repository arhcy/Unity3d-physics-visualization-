// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license
using System;
using UnityEngine;

namespace artics.UnityPhisicsVisualizers
{
    /// <summary>
    /// Draws gizmos of <seealso cref="BoxCollider2D"/> which attached for current GameObject.
    /// EdgeRadius is not supported
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class Box2dVisualizer : BaseVisualizer
    {
        protected BoxCollider2D Collider;
        protected Vector2[] Points;
        protected Vector2[] MultipliedPoints;

        public override void Init()
        {
            Collider = GetComponent<BoxCollider2D>();
            Points = new Vector2[4];
            MultipliedPoints = new Vector2[4];

            base.Init();
        }

        public override void UpdateBounds()
        {
            Vector2 offset = Collider.offset;
            Vector2 size = Collider.size * 0.5f;

            Points[0].Set(offset.x - size.x, offset.y - size.y);
            Points[1].Set(offset.x - size.x, offset.y + size.y);
            Points[2].Set(offset.x + size.x, offset.y - size.y);
            Points[3].Set(offset.x + size.x, offset.y + size.y);
        }

        protected override void MultiplyMatrix()
        {
            for (int i = 0; i < 4; i++)
                MultipliedPoints[i] = transform.localToWorldMatrix.MultiplyPoint(Points[i]);
        }

        protected override void Draw()
        {
            Gizmos.color = Color;

            Gizmos.DrawLine(MultipliedPoints[0], MultipliedPoints[1]);
            Gizmos.DrawLine(MultipliedPoints[2], MultipliedPoints[3]);
            Gizmos.DrawLine(MultipliedPoints[1], MultipliedPoints[3]);
            Gizmos.DrawLine(MultipliedPoints[2], MultipliedPoints[0]);
        }

        public override IDrawData CreateDrawData()
        {
            Box2DDrawData drawData = new Box2DDrawData();
            drawData.Color = Color;
            drawData.MultipliedPoints = new Vector2[4];
            Array.Copy(MultipliedPoints, drawData.MultipliedPoints, 4);

            return drawData;
        }
    }

    /// <summary>
    /// struct to store calculated data and draw it outside of class
    /// </summary>
    [System.Serializable]
    public struct Box2DDrawData : IDrawData
    {
        public Vector2[] MultipliedPoints;
        public Color Color;

        public void Draw()
        {
            Draw(Color);
        }

        public void Draw(Color color)
        {
            Gizmos.color = Color;

            Gizmos.DrawLine(MultipliedPoints[0], MultipliedPoints[1]);
            Gizmos.DrawLine(MultipliedPoints[2], MultipliedPoints[3]);
            Gizmos.DrawLine(MultipliedPoints[1], MultipliedPoints[3]);
            Gizmos.DrawLine(MultipliedPoints[2], MultipliedPoints[0]);
        }
    }
}
