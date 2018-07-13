// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license
using UnityEngine;

namespace Artics.Physics.UnityPhisicsVisualizers.Base
{
    /// <summary>
    /// Base visualizator class
    /// You can enable and disable visualization by <see cref = "IsVisible" /> parameter
    /// If collider don't change his Offset, Size, and Direction - you can disable <see cref="DynamicBounds"/> to increase performance
    /// </summary>
    [ExecuteInEditMode][DisallowMultipleComponent]
    public class BaseVisualizer : MonoBehaviour
    {
        /// <summary>
        /// Enables or disables rendering of collider
        /// </summary>
        public bool IsVisible = true;

        /// <summary>
        /// Updates bounds of collider every time <see cref="OnDrawGizmos"/> calls. Useful when you changing Offset, Size or Direction of the collider. If you don't just disable to increase performance.
        /// </summary>
        public bool DynamicBounds = true;

        public Color Color = Color.white;

        private void Awake()
        {
            Init();
        }

        [ContextMenu("Init")]
        public virtual void Init()
        {
            UpdateBounds();
        }

        void OnDrawGizmos()
        {
            if (!IsVisible)
                return;

            OnGizmos();
        }

        public virtual void OnGizmos()
        {
            if (DynamicBounds)
                UpdateBounds();

            MultiplyMatrix();
            Draw();
        }

        /// <summary>
        /// Update bounds of collider manually.  Use it if you changed Offset, Size or Direction of the collider.
        /// </summary>
        public virtual void UpdateBounds()
        {

        }

        protected virtual void MultiplyMatrix()
        {

        }

        protected virtual void Draw()
        {

        }

        public virtual IDrawData CreateDrawData()
        {
            return null;
        }

    }
}
