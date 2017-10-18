using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor {

    public override void OnInspectorGUI()
    {
        PlayerController playerControllerScript = (PlayerController)target;

        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.Label("Speed Roll");
        playerControllerScript.speed = EditorGUILayout.IntSlider(playerControllerScript.speed, 0, 20);
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }
}
