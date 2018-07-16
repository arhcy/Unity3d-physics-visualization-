// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license
using Artics.Physics.UnityPhisicsVisualizers.Base;
using UnityEngine;

namespace Artics.Physics.UnityPhisicsVisualizers
{
    /// <summary>
    /// Draws gizmos of <seealso cref="CapsuleCollider2D"/> which attached for current GameObject.
    /// You can enable and disable visualization by <see cref = "IsVisible" /> parameter
    /// If collider don't change his Offset, Size, and Direction - you can disable <see cref="DynamicBounds"/> to increase performance
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class Capsule2dVisualizer : ShapeVisualizer
    {
        public uint CustomProximity;

        protected CapsuleCollider2D Collider;
        protected Vector2 StartPosition;
        protected Vector2 EndPosition;
        protected Vector2 MultipliedStartPosition;
        protected Vector2 MultipliedEndPosition;
        public float Radius;

        [ContextMenu("Init")]
        public override void Init()
        {
            Collider = GetComponent<CapsuleCollider2D>();
            Points = new Vector2[0];
            MultipliedPoints = new Vector2[0];
            IsClosed = true;
            base.Init();
        }

        /// <summary>
        /// Update bounds of collider manually.  Use it if you changed Offset, Size or Direction of the collider.
        /// </summary>
        public override void UpdateBounds()
        {
            Collider2dPointsGetter.GetCapsuleCoordinates(Collider, ref Points, CustomProximity);

            if (Points.Length != MultipliedPoints.Length)
                MultipliedPoints = new Vector2[Points.Length];
        }

        /* old way of calculations
        protected void CalculateCapsuleBasePoints()
        {
            if (Collider.direction == CapsuleDirection2D.Vertical)
                UpdateBoundsVertical(Collider.size.y, Collider.size.x);
            else
                UpdateBoundsHorizontal(Collider.size.x, Collider.size.y);

            StartPosition += Collider.offset;
            EndPosition += Collider.offset;
        }

        protected void UpdateBoundsVertical(float height, float radius)
        {
            StartPosition.x = 0;
            EndPosition.x = 0;

            StartPosition.y = height * 0.5f;
            EndPosition.y = StartPosition.y * -1;

            Radius = Mathf.Abs(radius * 0.5f * transform.localScale.x);
        }

        protected void UpdateBoundsHorizontal(float height, float radius)
        {
            StartPosition.y = 0;
            EndPosition.y = 0;

            StartPosition.x = height * 0.5f;
            EndPosition.x = StartPosition.x * -1;

            Radius = Mathf.Abs(radius * 0.5f * transform.localScale.y);
        }
        */
    }
}
