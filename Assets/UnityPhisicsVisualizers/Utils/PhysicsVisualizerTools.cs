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
            BaseVisualizer[] visualziers = GameObject.FindObjectsOfType<BaseVisualizer>();

            for (int i = 0; i < visualziers.Length; i++)
                GameObject.DestroyImmediate(visualziers[i]);

            Debug.Log("Visualizers removed successfuly");
        }

        /// <summary>
        /// Seraches for all 2d colliders in scene and adds visualizers to them.
        /// </summary>
#if UNITY_EDITOR
        [MenuItem("Tools/Physics2dVisualizer/Add visualizers for all Colliders2d")]
#endif
        public static void AddVisualizersForAllColliders2d()
        {
            Collider2D[] visualziers = GameObject.FindObjectsOfType<Collider2D>();

            if (visualziers.Length == 0)
            {
                Debug.Log("No visualizers found");
                return;
            }

            AddVisualizersForListedColliders(visualziers);

            Debug.Log("Visualizers added successfuly");
        }

        public static void AddVisualizersForListedColliders(Collider2D[] visualziers)
        {
            var relations = GetColliderVisualizerRelations();

            for (int i = 0; i < visualziers.Length; i++)
            {
                Type type = null;
                relations.TryGetValue(visualziers[i].GetType(), out type);

                if (type != null)
                    visualziers[i].gameObject.AddComponent(type);
            }
        }

        /// <summary>
        /// Returns dictionary which relates colldier and visualizer types.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<Type, Type> GetColliderVisualizerRelations()
        {
            Dictionary<Type, Type> dict = new Dictionary<Type, Type>();

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
        public static void RemoveAllrenderers()
        {
            Collider2dRenderer[] visualziers = GameObject.FindObjectsOfType<Collider2dRenderer>();

            for (int i = 0; i < visualziers.Length; i++)
                RemoveCollider2DRenderer(visualziers[i].gameObject);

            Debug.Log("Rendereers removed successfuly");
        }

        /// <summary>
        /// Seraches for all 2d colliders in scene and adds <see cref="Collider2dRenderer"/> to them.
        /// </summary>
#if UNITY_EDITOR
        [MenuItem("Tools/Physics2dVisualizer/Add the Collider2dRenderer for all Colliders2d")]
#endif
        public static void AddRendereresForAllColliders2d()
        {
            Collider2D[] visualziers = GameObject.FindObjectsOfType<Collider2D>();

            if (visualziers.Length == 0)
            {
                Debug.Log("No visualizers found");
                return;
            }

            for (int i = 0; i < visualziers.Length; i++)
                visualziers[i].gameObject.AddComponent<Collider2dRenderer>();

            Debug.Log("Renderers added successfuly");
        }

        /// <summary>
        /// removes <see cref="Collider2dRenderer"/> and depended <see cref="MeshRenderer"/>, <see cref="MeshFilter"/> from gameobject
        /// </summary>
        /// <param name="instance"></param>
        public static void RemoveCollider2DRenderer(GameObject instance)
        {
            instance.SafeDestroyComponent<Collider2dRenderer>();
            instance.SafeDestroyComponent<MeshFilter>();
            instance.SafeDestroyComponent<MeshRenderer>();
        }

        /// <summary>
        /// Addds "Sprites/Default" material to all colliders renderers
        /// </summary>
#if UNITY_EDITOR
        [MenuItem("Tools/Physics2dVisualizer/Add default sprites material for all Colliders2d")]
#endif
        public static void AddDefaultMaterialToAllRenderers()
        {
            AddMaterialToAllRenderers(new Material(Shader.Find("Sprites/Default")));
            Debug.Log("Rendereers removed successfuly");
        }

        /// <summary>
        /// Adds material to all renderers
        /// </summary>
        /// <param name="material"></param>
        public static void AddMaterialToAllRenderers(Material material)
        {
            Collider2dRenderer[] visualziers = GameObject.FindObjectsOfType<Collider2dRenderer>();

            for (int i = 0; i < visualziers.Length; i++)
                visualziers[i].GetComponent<MeshRenderer>().sharedMaterial = material;
        }

        #endregion
    }
}