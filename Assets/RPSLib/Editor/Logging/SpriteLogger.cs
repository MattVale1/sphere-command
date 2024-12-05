/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace RPSEditor {

    public static class SpriteLogger {

        [MenuItem("RPSTools/Logging/Sprite Logger/List ALL Sprites in project - Editor Log")]
        public static void ListAllSpritesInProject_EditorLog() {
            ListAllSpritesInProject();
        }

        [MenuItem("RPSTools/Logging/Sprite Logger/List ALL Sprites in project - CSV Export")]
        public static void ListAllSpritesInProject_CSV() {
            ListAllSpritesInProject(true);
        }


        private static void ListAllSpritesInProject(bool exportAsCSV = false) {
            string log = "";
            string[] spriteGUIDs = AssetDatabase.FindAssets("t:Sprite");

            List<string> spriteAssetPaths = new();

            foreach (string guid in spriteGUIDs) {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                spriteAssetPaths.Add(path);
                log += string.Format("{0}, {1}, {2}, {3}, {4} KB\n", path, sprite.name, sprite.texture.width, sprite.texture.height, new FileInfo(path).Length / 1024);
            }

            if (exportAsCSV) {
                StringBuilder stringBuilder = new();
                stringBuilder.AppendLine(string.Join(",", log));
                File.AppendAllText("Assets/SpriteLog_" + Guid.NewGuid().ToString("n") + ".csv", stringBuilder.ToString());
                Debug.Log("<color=green>SpriteLogger :: CSV Export completed...</color>");
            } else {
                Debug.Log(log);
            }
        }

    }

}