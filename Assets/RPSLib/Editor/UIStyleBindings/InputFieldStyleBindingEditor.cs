/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using RPSCore;
using UnityEditor;
using UnityEngine;

namespace RPSEditor {

    [CustomEditor(typeof(InputFieldStyleBinding))]
    public class InputFieldStyleBindingEditor : Editor {

        public override void OnInspectorGUI() {

            DrawDefaultInspector();

            InputFieldStyleBinding bindingBase = (InputFieldStyleBinding)target;

            if (GUILayout.Button("Apply Now")) {
                bindingBase.ApplyStyling();
            }
        }

    }

}