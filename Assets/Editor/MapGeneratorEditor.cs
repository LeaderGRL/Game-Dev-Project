using Script;
using UnityEditor;
using UnityEngine;

namespace Editor
{
   [CustomEditor(typeof(MapGenerator))]
   public class MapGeneratorEditor : UnityEditor.Editor
   {
      public override void OnInspectorGUI()
      {
         MapGenerator mapGen = (MapGenerator)target;
         if (DrawDefaultInspector())
         {
            if (mapGen.autoUpdate)
            {
               mapGen.GenerateMap();
            }
         };
         if (GUILayout.Button("Generate"))
         {
            mapGen.GenerateMap();
         }
      }
   }
}
