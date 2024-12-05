/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using System.Linq;
using UnityEditor;
using UnityEngine;
using static System.IO.Directory;
using static System.IO.Path;
using static UnityEditor.AssetDatabase;
using static UnityEngine.Application;

namespace RPSEditor {

    public static class ProjectSetup {

        // Enter the root folder of the structure.
        public static string rootFolder = ""; // Leave as empty string to use 'Asset' root dir

        // Enter the ideal folder structure here.
        public static string[] folderNames = {
        "Game/_Notes",        
        "Game/Audio", "Game/Audio/OST", "Game/Audio/SFX",
        "Game/Camera",
        "Game/Editor",
        "Game/Entities",
        "Game/Input",
        "Game/Localization",
        "Game/Materials",        
        "Game/Scenes",
        "Game/Scripts",
        "Game/Shaders",
        "Game/Sprites",
        "Game/Textures",
        "Game/UI",
        "Game/VFX",

        "RPSLib",
        "StreamingAssets",
        "ThirdParty"
    };


        [MenuItem("RPSTools/Project Setup/Create Folder Structure - PascalCase")]
        public static void CreateFolderStructure_PascalCase() {
            CreateDir(rootFolder, folderNames);
        }

        [MenuItem("RPSTools/Project Setup/Create Folder Structure - UPPERCASE")]
        public static void CreateFolderStructure_UPPERCASE() {
            // Convert original array into an upper case string array
            var folderNameArrayUpper = folderNames.Select(s => s.ToUpper()).ToArray();
            CreateDir(rootFolder, folderNameArrayUpper);
        }

        // Logic to create the folders in the Unity project.
        public static void CreateDir(string root, params string[] dir) {
            var fullpath = Combine(dataPath, root);
            foreach (var newDirectory in dir) {
                if (Exists(Combine(fullpath, newDirectory))) {
                    Debug.Log("<color=yellow>Folder already exists, skipping: </color>" + newDirectory);
                    continue;
                }
                CreateDirectory(Combine(fullpath, newDirectory));
                Debug.Log("<color=green>Created folder: </color>" + newDirectory);
            }
            Debug.Log("<color=green>Finished creating folder structure.</color>");
            Refresh();
        }

    }

}