// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license
using UnityEngine;
using Artics.Physics.UnityPhisicsVisualizers.Base;

namespace Artics.Physics.UnityPhisicsVisualizers
{
    /// <summary>
    /// Draws gizmos of <seealso cref="CircleCollider2D"/> which attached for current GameObject.
    /// You can enable and disable visualization by <see cref = "IsVisible" /> parameter
    /// If collider don't change his Offset, Size, and Direction - you can disable <see cref="DynamicBounds"/> to increase performance
    /// </summary>
    /// 
    [RequireComponent(typeof(CircleCollider2D))]
    public class Circle2dVisualizer : ShapeVisualizer
    {
        public uint CustomProximity;

        protected CircleCollider2D Collider;
        protected Vector2 Center;
        protected Vector2 MultipliedCenter;
        protected float Radius;
        protected float MultipliedRadius;


        [ContextMenu("Init")]
        public override void Init()
        {
            Collider = GetComponent<CircleCollider2D>();
            IsClosed = true;
            
            base.Init();
        }

        public override void UpdateBounds()
        {
            Center = Collider.offset;
            Radius = Collider.radius;
        }

        protected override void MultiplyMatrix()
        {
            var matrix = GetMatrix();
            var lossyScale = transform.lossyScale;

            MultipliedCenter = matrix.MultiplyPoint(Center);
            MultipliedRadius = Radius * Mathf.Max(Mathf.Abs(lossyScale.x), Mathf.Abs(lossyScale.y));

            Collider2dPointsGetter.GetCircleCoordinates(MultipliedCenter, MultipliedRadius, ref MultipliedPoints, CustomProximity);
            PointsLenght = MultipliedPoints.Length;
        }
    }
}
