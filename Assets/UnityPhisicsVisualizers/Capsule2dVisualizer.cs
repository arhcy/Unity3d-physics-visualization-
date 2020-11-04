// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license

using Artics.Physics.UnityPhisicsVisualizers.Base;
using UnityEngine;

namespace Artics.Physics.UnityPhisicsVisualizers
{
    /// <summary>
    /// Draws gizmos of <seealso cref="CapsuleCollider2D"/> which attached for current GameObject.
    /// You can enable and disable visualization by <see cref = "IsVisible" /> parameter
    /// If collider don't change it's Offset, Size, and Direction - you can disable <see cref="DynamicBounds"/> to increase performance
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class Capsule2dVisualizer : ShapeVisualizer
    {
        public uint CustomProximity;

        protected CapsuleCollider2D Collider;

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
            base.UpdateBounds();

            Collider2dPointsGetter.GetCapsuleCoordinates(Collider, ref Points, CustomProximity, true);

            if (Points.Length != MultipliedPoints.Length)
            {
                PointsLenght = Points.Length;
                MultipliedPoints = new Vector2[PointsLenght];
            }
        }
    }
}