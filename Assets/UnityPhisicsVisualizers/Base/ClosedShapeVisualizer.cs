using System;
using System.Collections.Generic;
using UnityEngine;

namespace Artics.Physics.UnityPhisicsVisualizers.Base
{
    public class ClosedShapeVisualizer:BaseVisualizer
    {
        protected Vector2[] Points;
        protected Vector2[] MultipliedPoints;

        public override void Init()
        {
            Points = new Vector2[0];
            MultipliedPoints = new Vector2[0];

            base.Init();
        }

        protected override void MultiplyMatrix()
        {
            for (int i = 0; i < Points.Length; i++)
                MultipliedPoints[i] = transform.localToWorldMatrix.MultiplyPoint(Points[i]);
        }

        protected override void Draw()
        {
            Gizmos.color = Color;
            int PointsLenght = MultipliedPoints.Length;

            for (int i = 0; i < PointsLenght - 1; i++)
                Gizmos.DrawLine(MultipliedPoints[i], MultipliedPoints[i + 1]);

            Gizmos.DrawLine(MultipliedPoints[0], MultipliedPoints[PointsLenght - 1]);
        }
    }
}
