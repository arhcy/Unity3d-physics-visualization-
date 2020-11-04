// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license

using System;
using Artics.Physics.UnityPhisicsVisualizers;
using Artics.Physics.UnityPhisicsVisualizers.Base;
using Artics.Physics.UnityPhisicsVisualizers.Utils;
using UnityEditor;
using UnityEngine;

public class Collider2dVisualizersWindow : EditorWindow
{
    protected Material DefaultMaterial;
    protected Color32 DefaultColor;
    protected float DefaultThickness;
    protected bool DefaultPixelSize;
    protected int DefaultCircleProximity;
    protected bool DefaultUseCircleProximity;
    protected bool DefaultUpdate;


    // Add menu named "My Window" to the Window menu
    [MenuItem("Tools/Physics2dVisualizer/Open Manager", false, 0)]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        var window = GetWindow(typeof(Collider2dVisualizersWindow));
        window.Show();
    }

    private void Awake()
    {
        DefaultMaterial = new Material(Shader.Find("Sprites/Default"));
        DefaultColor = Color.white;
        DefaultCircleProximity = 20;
        DefaultPixelSize = true;
        DefaultThickness = 1;
    }

    void OnGUI()
    {
        #region Colliderd2d
        GUILayout.Label("Collider2D:", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        DrawSelectionButtons("Select All", "Select all in children",
            () => Selection.objects = Utils.FindComponents<Collider2D>(),
            () => Selection.objects = Utils.FindComponentsInSelection<Collider2D>()
        );
        GUILayout.EndHorizontal();
        #endregion
        //---------------------------------------------------------------------------------
        #region Gizmos

        //GUILayout.Space(15);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Gizmos visualizers:", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        DrawSelectionButtons("Select All", "Select all in children",
            () => Selection.objects = Utils.FindComponents<BaseVisualizer>(),
            () => Selection.objects = Utils.FindComponentsInSelection<BaseVisualizer>()
        );
        GUILayout.EndHorizontal();

        //adding
        GUILayout.Label("Add/Remove:");
        GUILayout.BeginHorizontal();

        DrawSelectionButtons("Add visualizers", "Remove visualizers",
            () => PhysicsVisualizerTools.AddVisualizersForListedColliders(Utils.FindObjectsInSelection<Collider2D>().ToArray()),
            () => Utils.FindObjectsInSelection<BaseVisualizer>().ForEach(DestroyImmediate)
        );

        GUILayout.EndHorizontal();

        #endregion

        #region Rendereres
        //GUILayout.Space(10);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Mesh visualizers:", EditorStyles.boldLabel);

        //select
        GUILayout.BeginHorizontal();
        DrawSelectionButtons("Select All", "Select all in children",
            () => Selection.objects = Utils.FindComponents<Collider2dRenderer>(),
            () => Selection.objects = Utils.FindComponentsInSelection<Collider2dRenderer>()
        );
        GUILayout.EndHorizontal();
        GUILayout.Space(4);

        //add/remove
        GUILayout.Label("Add/Remove:");
        GUILayout.BeginHorizontal();
        DrawSelectionButtons("Add renderers", "Remove rendrers",
            () => Utils.FindObjectsInSelection<Collider2D>().ForEach(a => a.gameObject.AddComponent<Collider2dRenderer>()),
            () => Utils.FindObjectsInSelection<Collider2dRenderer>().ForEach(a => PhysicsVisualizerTools.RemoveCollider2DRenderer(a.gameObject))
        );
        GUILayout.EndHorizontal();
        GUILayout.Space(4);

        //material
        GUILayout.Label("Material:");
        GUILayout.BeginHorizontal();
        DefaultMaterial = (Material)EditorGUILayout.ObjectField(DefaultMaterial, typeof(Material), GUILayout.ExpandWidth(false));
        if (GUILayout.Button("Set"))
            Utils.FindObjectsInSelection<Collider2dRenderer>().ForEach(a => a.GetComponent<MeshRenderer>().sharedMaterial = DefaultMaterial);        
        GUILayout.EndHorizontal();
        GUILayout.Space(4);

        //color
        GUILayout.Label("Color:");
        GUILayout.BeginHorizontal();
        DefaultColor = EditorGUILayout.ColorField(DefaultColor, GUILayout.ExpandWidth(false));
        if (GUILayout.Button("Set"))
            Utils.FindObjectsInSelection<Collider2dRenderer>().ForEach(a => a.SetColor(DefaultColor));        
        GUILayout.EndHorizontal();
        GUILayout.Space(4);

        //thickness
        GUILayout.Label("Thickness:");
        GUILayout.BeginHorizontal();
        DefaultThickness = EditorGUILayout.FloatField(DefaultThickness, GUILayout.ExpandWidth(false), GUILayout.Width(50));
        GUILayout.Label("Use pixel size:");
        DefaultPixelSize = EditorGUILayout.Toggle(DefaultPixelSize, GUILayout.ExpandWidth(false));
        if (GUILayout.Button("Set"))
            Utils.FindObjectsInSelection<Collider2dRenderer>().ForEach(a => a.SetThickness(DefaultThickness, DefaultPixelSize));        
        GUILayout.EndHorizontal();
        GUILayout.Space(4);

        //Circle proximity
        GUILayout.Label("Circle proximity:");
        GUILayout.BeginHorizontal();
        DefaultCircleProximity = EditorGUILayout.IntField(DefaultCircleProximity, GUILayout.ExpandWidth(false), GUILayout.Width(50));
        GUILayout.Label("Use proximity:");
        DefaultUseCircleProximity = EditorGUILayout.Toggle(DefaultUseCircleProximity, GUILayout.ExpandWidth(false));
        if (GUILayout.Button("Set"))
            Utils.FindObjectsInSelection<Collider2dRenderer>().ForEach(a => a.SetCustomCircleProximity((uint)DefaultCircleProximity, DefaultUseCircleProximity));
        GUILayout.EndHorizontal();
        GUILayout.Space(4);

        //Always update
        GUILayout.Label("Always update:", GUILayout.ExpandWidth(false));
        GUILayout.BeginHorizontal();
        DefaultUpdate = EditorGUILayout.Toggle(DefaultUpdate, GUILayout.ExpandWidth(false), GUILayout.Width(50));
        if (GUILayout.Button("Set"))
            Utils.FindObjectsInSelection<Collider2dRenderer>().ForEach(a => a.AlwaysUpdate = DefaultUpdate);
        GUILayout.EndHorizontal();


        #endregion
    }

    protected void DrawSelectionButtons(string label1, string label2, Action onAll, Action onSelected)
    {
        if (GUILayout.Button(label1, GUILayout.ExpandWidth(false)))
            onAll();

        if (GUILayout.Button(label2, GUILayout.ExpandWidth(false)))
            onSelected();
    }

    protected void DrawSelectionButtons(Action onAll, Action onSelected)
    {
        if (GUILayout.Button("All colliders", GUILayout.ExpandWidth(false)))
            onAll();

        if (GUILayout.Button("All selected", GUILayout.ExpandWidth(false)))
            onSelected();
    }
}
