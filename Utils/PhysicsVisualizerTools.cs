// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license

#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using UnityEngine;
using Artics.Physics.UnityPhisicsVisualizers.Utils;
using Artics.Physics.UnityPhisicsVisualizers.Base;
using System;

namespace Artics.Physics.UnityPhisicsVisualizers
{
    /// <summary>
    /// Static tools
    /// </summary>
    public class PhysicsVisualizerTools : MonoBehaviour
    {
        #region GizmosVisualizer

        /// <summary>
        /// Removes all visualizers components in scene.
        /// </summary>
#if UNITY_EDITOR
        [MenuItem("Tools/Physics2dVisualizer/Remove all visualizers")]
#endif
        public static void RemoveAllVisualizers()
        {
            var visualizers = FindObjectsOfType<BaseVisualizer>();

            foreach (var item in visualizers)
                GameObject.DestroyImmediate(item);

            Debug.Log("Visualizers removed successfully");
        }

        /// <summary>
        /// Searches for all 2d colliders in scene and adds visualizers to them.
        /// </summary>
#if UNITY_EDITOR
        [MenuItem("Tools/Physics2dVisualizer/Add visualizers for all Colliders2d and Joints")]
#endif
        public static void AddVisualizersForAllColliders2d()
        {
            var visualizers = FindObjectsOfType<Collider2D>();

            if (visualizers.Length == 0)
            {
                Debug.Log("No Collider visualizers found");
                return;
            }

            AddVisualizersForListedColliders(visualizers);

            var joints = FindObjectsOfType<AnchoredJoint2D>();

            if (joints.Length == 0)
            {
                Debug.Log("No joints found");
                return;
            }

            for (int i = 0; i < joints.Length; i++)
                joints[i].gameObject.AddComponent<AnchoredJoint2DVisualizer>();

            Debug.Log("Visualizers added successfully");
        }

        public static void AddVisualizersForListedColliders(Collider2D[] visualizers)
        {
            var relations = GetColliderVisualizerRelations();

            foreach (var item in visualizers)
            {
                relations.TryGetValue(item.GetType(), out var type);

                if (type != null)
                    item.gameObject.AddComponent(type);
            }
        }

        /// <summary>
        /// Returns dictionary which relates collier and visualizer types.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<Type, Type> GetColliderVisualizerRelations()
        {
            var dict = new Dictionary<Type, Type>();

            dict.Add(typeof(BoxCollider2D), typeof(Box2dVisualizer));
            dict.Add(typeof(CircleCollider2D), typeof(Circle2dVisualizer));
            dict.Add(typeof(CapsuleCollider2D), typeof(Capsule2dVisualizer));
            dict.Add(typeof(PolygonCollider2D), typeof(Polygon2dVisualizer));
            dict.Add(typeof(EdgeCollider2D), typeof(Edge2dVisualizer));

            return dict;
        }

        #endregion

        #region RendererVisualizer

        /// <summary>
        /// Removes all <see cref="Collider2dRenderer"/> components in scene.
        /// </summary>
#if UNITY_EDITOR
        [MenuItem("Tools/Physics2dVisualizer/Remove all Collider2dRenderer")]
#endif
        public static void RemoveAllRenderers()
        {
            var visualizers = FindObjectsOfType<Collider2dRenderer>();

            foreach (var item in visualizers)
                RemoveCollider2DRenderer(item.gameObject);

            Debug.Log("Renderers removed successfully");
        }

        /// <summary>
        /// Searches for all 2d colliders in scene and adds <see cref="Collider2dRenderer"/> to them.
        /// </summary>
#if UNITY_EDITOR
        [MenuItem("Tools/Physics2dVisualizer/Add the Collider2dRenderer for all Colliders2d")]
#endif
        public static void AddRenderersForAllColliders2d()
        {
            var visualizers = FindObjectsOfType<Collider2D>();

            if (visualizers.Length == 0)
            {
                Debug.Log("No visualizers found");
                return;
            }

            for (int i = 0; i < visualizers.Length; i++)
                visualizers[i].gameObject.AddComponent<Collider2dRenderer>();

            Debug.Log("Renderers added successfully");
        }

        /// <summary>
        /// Removes <see cref="Collider2dRenderer"/> and depended <see cref="MeshRenderer"/>, <see cref="MeshFilter"/> from gameobject
        /// </summary>
        /// <param name="instance"></param>
        public static void RemoveCollider2DRenderer(GameObject instance)
        {
            instance.SafeDestroyComponent<Collider2dRenderer>();
            instance.SafeDestroyComponent<MeshFilter>();
            instance.SafeDestroyComponent<MeshRenderer>();
        }

        /// <summary>
        /// Adds "Sprites/Default" material to all colliders renderers
        /// </summary>
#if UNITY_EDITOR
        [MenuItem("Tools/Physics2dVisualizer/Add default sprites material for all Colliders2d")]
#endif
        public static void AddDefaultMaterialToAllRenderers()
        {
            AddMaterialToAllRenderers(new Material(Shader.Find("Sprites/Default")));
            Debug.Log("Renderers removed successfully");
        }

        /// <summary>
        /// Adds material to all renderers
        /// </summary>
        /// <param name="material"></param>
        public static void AddMaterialToAllRenderers(Material material)
        {
            var visualizers = GameObject.FindObjectsOfType<Collider2dRenderer>();

            foreach (var item in visualizers)
                item.GetComponent<MeshRenderer>().sharedMaterial = material;
        }

        #endregion
    }
}