// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license
using UnityEngine;
using Artics.Physics.UnityPhisicsVisualizers.Base;

namespace Artics.Physics.UnityPhisicsVisualizers
{
    /// <summary>
    /// Draws gizmos of <seealso cref="PolygonCollider2D"/> which attached for current GameObject.
    /// Multiple polygon Paths not supported
    /// </summary>

    //[RequireComponent(typeof(PolygonCollider2D))]

    public class Polygon2dVisualizer : ShapeVisualizer
    {
        protected Collider2D Collider;

        public override void Init()
        {
            Collider = GetComponent<Collider2D>();
            PointsLenght = GetColliderPoints().Length;
            MultipliedPoints = new Vector2[PointsLenght];
            IsClosed = true;

            base.Init();
        }

        public override void UpdateBounds()
        {
            base.UpdateBounds();

            Points = GetColliderPoints();

            if (PointsLenght != Points.Length)
            {
                PointsLenght = Points.Length;
                MultipliedPoints = new Vector2[PointsLenght];
            }

            Offset = Collider.offset;
        }

        protected Vector2[] GetColliderPoints()
        {
            if (Collider is PolygonCollider2D)
                return (Collider as PolygonCollider2D).points;
            else
                if (Collider is EdgeCollider2D)
                return (Collider as EdgeCollider2D).points;

            return null;
        }
    }
}