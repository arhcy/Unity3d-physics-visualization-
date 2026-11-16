// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license

using UnityEngine;
using Artics.Physics.UnityPhisicsVisualizers.Base;

namespace Artics.Physics.UnityPhisicsVisualizers
{
    /// <summary>
    /// Draws gizmos of <seealso cref="BoxCollider2D"/> which attached for current GameObject.
    /// EdgeRadius is not supported
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class Box2dVisualizer : ShapeVisualizer
    {
        protected BoxCollider2D Collider;

        public override void Init()
        {
            Collider = GetComponent<BoxCollider2D>();
            Points = new Vector2[4];
            MultipliedPoints = new Vector2[4];
            IsClosed = true;

            base.Init();
        }

        public override void UpdateBounds()
        {
            base.UpdateBounds();

            var size = Collider.size * 0.5f;

            if (RigidBodyAttached)
                size.Scale(transform.lossyScale);

            Points[0].Set(-size.x, -size.y);
            Points[1].Set(-size.x, +size.y);
            Points[2].Set(+size.x, +size.y);
            Points[3].Set(+size.x, -size.y);

            Offset = Collider.offset;
            RigidBodyAttached = Collider.attachedRigidbody != null;
        }

        protected override void MultiplyMatrix()
        {
            Offset = Collider.offset;
            base.MultiplyMatrix();
        }
    }
}