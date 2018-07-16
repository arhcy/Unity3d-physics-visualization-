// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license

using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Artics.Physics.UnityPhisicsVisualizers
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public class Collider2dRenderer : MonoBehaviour
    {
        [Header("Base")]
        [Tooltip("Set to recalculate meshevery frame")]
        public bool AlwaysUpdate;

        public Color32 MeshColor = Color.white;

        [Header("Circular render:")]
        [Tooltip("Set to use custom segmentation level for meshes of Circle and Capsule colliders")]
        public bool UseCircleProximity;

        [Tooltip("Set the segmentation level for meshes of Circle and Capsule colliders")]
        public uint CustomCircleProximity = 40;

        [Tooltip("Thikness of mesh in points, or in pixels if UsePixelSize is true")]
        [Header("Thikness:")]
        public float Thickness = 1f;

        [Tooltip("For ortographic camera only.")]
        public bool UsePixelSize = true;

        protected MeshFilter MeshFilterComponent;
        protected Vector2[] Points;
        protected Collider2D Collider;
        protected GetCoordinatesMethod OnRecalculate;
        protected bool Closed;
        protected Mesh MeshInstance;

        private void Awake()
        {
            Init();
            Build();
        }

        private void Reset()
        {
            Init();
            UseCircleProximity = !(Collider is CircleCollider2D) || !(Collider is CapsuleCollider2D);
            Build();
        }

        protected void Init()
        {
            if (MeshFilterComponent == null)
                MeshFilterComponent = gameObject.GetComponent<MeshFilter>();

            if (MeshFilterComponent == null)
                MeshFilterComponent = gameObject.AddComponent<MeshFilter>();

            if (Collider == null)
                Collider = gameObject.GetComponent<Collider2D>();

            if (Collider == null && Application.isPlaying)
                throw new NullReferenceException("No Collider2D found");

            var dict = Collider2dPointsGetter.GetTypeMethodDictionary();

            if (dict.ContainsKey(Collider.GetType()))
                OnRecalculate = dict[Collider.GetType()];
            else
                throw new Exception(Collider.GetType().ToString() + " is not supported");

            Closed = !(Collider is EdgeCollider2D);
            //UseCircleProximity = !(Collider is CircleCollider2D) || !(Collider is CapsuleCollider2D);

            if (MeshInstance == null)
                MeshInstance = new Mesh();

            if (MeshFilterComponent != null)
                MeshFilterComponent.sharedMesh = MeshInstance;
        }

        public void Build()
        {
            if (OnRecalculate == null)
                Init();

            if (UseCircleProximity && CustomCircleProximity > 0)
            {
                uint prevProximity = Collider2dPointsGetter.CircleProximity;
                Collider2dPointsGetter.CircleProximity = CustomCircleProximity;

                OnRecalculate(Collider, ref Points);

                Collider2dPointsGetter.CircleProximity = prevProximity;
            }
            else
                OnRecalculate(Collider, ref Points);

            float ppu = UsePixelSize ? (Camera.main.orthographicSize * 2) / Screen.height : 1;

            PolygonTriangulator.TriangulateAsLine(Points, MeshInstance, ppu * Thickness, Closed);

            Color32[] colors = new Color32[MeshInstance.vertices.Length];
            for (int i = 0; i < colors.Length; i++)
                colors[i] = MeshColor;

            MeshInstance.colors32 = colors;
        }

        [ContextMenu("Rebuild")]
        public void ReInitAndBuild()
        {
            Init();
            Build();
        }

        protected void Update()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
#endif
                DoUpdate();
        }

        protected virtual void DoUpdate()
        {
            if (AlwaysUpdate)
                Build();
        }

        public void SetThickness(float thickness, bool usePixelSize)
        {
            Thickness = thickness;
            UsePixelSize = usePixelSize;
            Build();
        }

        public void SetColor(Color32 color)
        {
            MeshColor = color;
            Build();
        }

        public void SetCustomCircleProximity(uint proximity, bool useProximity = true)
        {
            CustomCircleProximity = proximity;
            UseCircleProximity = true;
            Build();
        }


#if UNITY_EDITOR
        [Header("Editor:")]
        public float GizmosGap = 0.2f;
        public bool ShowInfo;

        private void OnDrawGizmos()
        {
            if (!ShowInfo || Points == null)
                return;

            for (int i = 0; i < Points.Length; i++)
                Handles.Label(transform.localToWorldMatrix.MultiplyPoint((Vector3)Points[i] + new Vector3(i % 2 * GizmosGap, 0, 0)), "P" + i.ToString());

            var vertexes = MeshFilterComponent.sharedMesh.vertices;

            for (int i = 0; i < vertexes.Length; i++)
                Handles.Label(transform.localToWorldMatrix.MultiplyPoint(vertexes[i]), "V" + i.ToString());
        }
#endif
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(Collider2dRenderer))]
    [CanEditMultipleObjects]
    public class Collider2dRendererEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.DrawDefaultInspector();

            Update();

            if (GUILayout.Button("Rebuild"))
            {
                foreach (Collider2dRenderer instance in targets)
                    instance.ReInitAndBuild();
            }
        }

        /*
        private void OnEnable()
        {
            EditorApplication.update += Update;
        }

        private void OnDisable()
        {
            EditorApplication.update -= Update;
        }*/

        protected void Update()
        {
            foreach (Collider2dRenderer instance in targets)
                if (!instance.AlwaysUpdate)
                    instance.Build();
        }

    }
#endif


}
