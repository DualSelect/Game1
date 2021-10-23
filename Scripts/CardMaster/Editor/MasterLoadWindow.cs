using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MasterLoader
{
    public class MasterLoadWindow : EditorWindow
    {

        [MenuItem("Window/MasterLoader")]
        static void Open()
        {
            GetWindow<MasterLoadWindow>();
        }

        void OnGUI()
        {
            EditorGUILayout.Space();

            if (GUILayout.Button("Card", GUILayout.Width(80.0f)))
            {
                _ = MasterLoader.LoadMaster("CardMaster");
            }
            if (GUILayout.Button("Shield", GUILayout.Width(80.0f)))
            {
                _ = MasterLoader.LoadMaster("ShieldMaster");
            }
            if (GUILayout.Button("Special", GUILayout.Width(80.0f)))
            {
                _ = MasterLoader.LoadMaster("SpecialMaster");
            }
            EditorGUILayout.Space();
        }

    }
}