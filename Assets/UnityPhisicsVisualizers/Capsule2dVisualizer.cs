// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license
using UnityEngine;

namespace artics.UnityPhisicsVisualizers
{
    /// <summary>
    /// Draws gizmos of <seealso cref="CapsuleCollider2D"/> which attached for current GameObject.
    /// You can enable and disable visualization by <see cref = "IsVisible" /> parameter
    /// If collider don't change his Offset, Size, and Direction - you can disable <see cref="DynamicBounds"/> to increase performance
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class Capsule2dVisualizer : BaseVisualizer
    {
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
            base.Init();
        }

        /// <summary>
        /// Update bounds of collider manually.  Use it if you changed Offset, Size or Direction of the collider.
        /// </summary>
        public override void UpdateBounds()
        {
            if (Collider.direction == CapsuleDirection2D.Vertical)
                UpdateBoundsVertical(Collider.size.y, Collider.size.x);
            else
                UpdateBoundsHorizontal(Collider.size.x, Collider.size.y);
        }

        protected override void MultiplyMatrix()
        {
            MultipliedStartPosition = transform.localToWorldMatrix.MultiplyPoint(StartPosition);
            MultipliedEndPosition = transform.localToWorldMatrix.MultiplyPoint(EndPosition);
        }

        protected void UpdateBoundsVertical(float height, float radius)
        {
            StartPosition.x = 0;
            EndPosition.x = 0;

            StartPosition.y = height * 0.5f;
            EndPosition.y = StartPosition.y * -1;

            StartPosition += Collider.offset;
            EndPosition += Collider.offset;

            Radius = Mathf.Abs(radius * 0.5f * transform.localScale.x);
        }

        protected void UpdateBoundsHorizontal(float height, float radius)
        {
            StartPosition.y = 0;
            EndPosition.y = 0;

            StartPosition.x = height * 0.5f;
            EndPosition.x = StartPosition.x * -1;

            StartPosition += Collider.offset;
            EndPosition += Collider.offset;

            Radius = Mathf.Abs(radius * 0.5f * transform.localScale.y);
        }

        /// <summary>
        /// You can override drawing method for yout needs
        /// </summary>
        protected override void Draw()
        {
            DebugExtension.DrawCapsule(MultipliedStartPosition, MultipliedEndPosition, Color, Radius);
        }

        public override IDrawData CreateDrawData()
        {
            Capsule2DDrawData data = new Capsule2DDrawData();
            data.Color = Color;
            data.MultipliedEndPosition = MultipliedEndPosition;
            data.MultipliedStartPosition = MultipliedStartPosition;
            data.Radius = Radius;

            return data;
        }
    }

    /// <summary>
    /// struct to store calculated data and draw it outside of class
    /// </summary>
    [System.Serializable]
    public struct Capsule2DDrawData : IDrawData
    {
        public Vector2 MultipliedStartPosition;
        public Vector2 MultipliedEndPosition;
        public float Radius;
        public Color Color;

        public void Draw()
        {
            DebugExtension.DrawCapsule(MultipliedStartPosition, MultipliedEndPosition, Color, Radius);
        }

        public void Draw(Color color)
        {
            DebugExtension.DrawCapsule(MultipliedStartPosition, MultipliedEndPosition, color, Radius);
        }
    }
}
