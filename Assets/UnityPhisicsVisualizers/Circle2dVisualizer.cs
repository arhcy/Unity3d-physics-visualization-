// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license
using UnityEngine;

namespace artics.UnityPhisicsVisualizers
{
    /// <summary>
    /// Draws gizmos of <seealso cref="CircleCollider2D"/> which attached for current GameObject.
    /// You can enable and disable visualization by <see cref = "IsVisible" /> parameter
    /// If collider don't change his Offset, Size, and Direction - you can disable <see cref="DynamicBounds"/> to increase performance
    /// </summary>
    public class Circle2dVisualizer : BaseVisualizer
    {
        protected CircleCollider2D Collider;
        protected Vector2 Center;
        protected Vector2 MultipliedCenter;
        protected float Radius;
        protected float MultipliedRadius;

        [ContextMenu("Init")]
        public override void Init()
        {
            Collider = GetComponent<CircleCollider2D>();
            base.Init();
        }

        public override void UpdateBounds()
        {
            Center = Collider.offset;
            Radius = Collider.radius;
        }

        protected override void MultiplyMatrix()
        {
            MultipliedCenter = transform.localToWorldMatrix.MultiplyPoint(Center);
            MultipliedRadius = Radius * Mathf.Max(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y));
        }

        protected override void Draw()
        {
            DebugExtension.DrawCircle(MultipliedCenter, Vector3.forward, Color.white, MultipliedRadius);
        }



    }
}
