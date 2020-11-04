// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Artics.Physics.UnityPhisicsVisualizers.Utils
{
    public static class Utils
    {
        public static void SafeDestroyComponent<T>(this GameObject obj) where T : Component
        {
            var component = obj.GetComponent<T>();

            if (component != null)
                Object.DestroyImmediate(component);
        }

        public static GameObject[] FindComponents<T>() where T : Component
        {
            return Object.FindObjectsOfType<T>().Select(a => a.gameObject).ToArray<GameObject>();
        }

        public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
        {
            var instance = obj.GetComponent<T>();

            if (instance == null)
                instance = obj.AddComponent<T>();

            return instance;
        }

#if UNITY_EDITOR
        public static List<T> FindObjectsInSelection<T>() where T : Component
        {
            var list = new List<T>();
            Selection.gameObjects
                .Select(a => a.GetComponentsInChildren<T>())
                .Where(a => a != null)
                .ToList()
                .ForEach(
                    a => a.ToList()
                        .ForEach(b => list.Add(b))
                );

            return list;
        }

        public static GameObject[] FindComponentsInSelection<T>() where T : Component
        {
            return FindObjectsInSelection<T>().Select(a => a.gameObject).ToArray<GameObject>();
        }
#endif
    }
}
