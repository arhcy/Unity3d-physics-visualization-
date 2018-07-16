﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
using UnityEngine;
#endif

namespace Artics.Physics.UnityPhisicsVisualizers.Utils
{
    public static class Utils
    {
        public static void SafeDestroyComponent<T>(this GameObject obj) where T : Component
        {
            T component = obj.GetComponent<T>();

            if (component != null)
                GameObject.DestroyImmediate(component);
        }

        public static GameObject[] FindComponents<T>() where T : Component
        {
            return GameObject.FindObjectsOfType<T>().Select(a => a.gameObject).ToArray<GameObject>();
        }


#if UNITY_EDITOR
        public static List<T> FindObjectsInSelection<T>() where T : Component
        {
            List<T> list = new List<T>();
            Selection.gameObjects.Select(a => a.GetComponentsInChildren<T>()).Where(a => a != null).ToList().ForEach(a => a.ToList().ForEach(b => list.Add(b)));

            return list;
        }


        public static GameObject[] FindComponentsInSelection<T>() where T : Component
        {
            return FindObjectsInSelection<T>().Select(a => a.gameObject).ToArray<GameObject>();
        }

#endif

    }
}