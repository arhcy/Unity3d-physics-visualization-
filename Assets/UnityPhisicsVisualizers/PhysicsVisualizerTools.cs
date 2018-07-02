// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using artics.UnityPhisicsVisualizers;
using System;

namespace artics.UnityPhisicsVisualizers
{
    /// <summary>
    /// Static tools
    /// </summary>
    public class PhysicsVisualizerTools : MonoBehaviour
    {
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

            var relations = GetColliderVisualizerRelations();

            for (int i = 0; i < visualziers.Length; i++)
            {
                Type type = null;
                relations.TryGetValue(visualziers[i].GetType(), out type);

                if (type != null)
                    visualziers[i].gameObject.AddComponent(type);

            }

            Debug.Log("Visualizers added successfuly");
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
    }
}