/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using RPSCore;
using UnityEditor;
using UnityEngine;

namespace RPSEditor {

    [CustomEditor(typeof(DropdownStyleBinding))]
    public class DropdownStyleBindingEditor : Editor {

        public override void OnInspectorGUI() {

            DrawDefaultInspector();

            DropdownStyleBinding bindingBase = (DropdownStyleBinding)target;

            if (GUILayout.Button("Apply Now")) {
                bindingBase.ApplyStyling();
            }
        }

    }

}