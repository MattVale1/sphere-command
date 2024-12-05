/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using RPSCore;
using UnityEditor;
using UnityEngine;

namespace RPSEditor {

    [CustomEditor(typeof(ToggleListStyleBinding))]
    public class ToggleListStyleBindingEditor : Editor {

        public override void OnInspectorGUI() {

            DrawDefaultInspector();

            ToggleListStyleBinding bindingBase = (ToggleListStyleBinding)target;

            if (GUILayout.Button("Apply Now")) {
                bindingBase.ApplyStyling();
            }
        }

    }

}