// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license
using UnityEngine;

namespace artics.UnityPhisicsVisualizers
{
    /// <summary>
    /// Draws gizmos of <seealso cref="PolygonCollider2D"/> which attached for current GameObject.
    /// You can enable and disable visualization by <see cref = "IsVisible" /> parameter
    /// If collider don't change his Offset, Size, and Direction - you can disable <see cref="DynamicBounds"/> to increase performance
    /// </summary>

    [RequireComponent(typeof(EdgeCollider2D))]

    public class Edge2dVisualizer : BaseVisualizer
    {
        protected EdgeCollider2D Collider;
        protected Vector2[] MultipliedPoints;
        protected int PointsLenght;

        public override void Init()
        {
            Collider = GetComponent<EdgeCollider2D>();
            PointsLenght = Collider.points.Length;
            MultipliedPoints = new Vector2[PointsLenght];

            base.Init();
        }

        protected override void MultiplyMatrix()
        {
            if (MultipliedPoints.Length != PointsLenght)
            {
                PointsLenght = Collider.points.Length;
                MultipliedPoints = new Vector2[PointsLenght];
            }

            for (int i = 0; i < PointsLenght; i++)
                MultipliedPoints[i] = transform.localToWorldMatrix.MultiplyPoint(Collider.points[i] + Collider.offset);
        }

        protected override void Draw()
        {
            for (int i = 0; i < PointsLenght - 1; i++)
                Gizmos.DrawLine(MultipliedPoints[i], MultipliedPoints[i + 1]);
        }

    }
}