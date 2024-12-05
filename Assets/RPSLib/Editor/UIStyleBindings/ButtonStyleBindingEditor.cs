/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using RPSCore;
using UnityEditor;
using UnityEngine;

namespace RPSEditor {

    [CustomEditor(typeof(ButtonStyleBinding))]
    public class ButtonStyleBindingEditor : Editor {

        public override void OnInspectorGUI() {

            DrawDefaultInspector();

            ButtonStyleBinding bindingBase = (ButtonStyleBinding)target;

            if (GUILayout.Button("Apply Now")) {
                bindingBase.ApplyStyling();
            }
        }

    }

}