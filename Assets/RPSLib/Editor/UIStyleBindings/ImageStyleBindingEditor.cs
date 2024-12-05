/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using RPSCore;
using UnityEditor;
using UnityEngine;

namespace RPSEditor {

    [CustomEditor(typeof(ImageStyleBinding))]
    public class ImageStyleBindingEditor : Editor {

        public override void OnInspectorGUI() {

            DrawDefaultInspector();

            ImageStyleBinding bindingBase = (ImageStyleBinding)target;

            if (GUILayout.Button("Apply Now")) {
                bindingBase.ApplyStyling();
            }
        }

    }

}