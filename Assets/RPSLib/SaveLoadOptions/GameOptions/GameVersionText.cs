/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using TMPro;
using UnityEngine;

namespace RPSCore {

    public class GameVersionText : MonoBehaviour {

        public bool shorthand = false;

        protected void Awake() {
            if (shorthand) {
                GetComponent<TextMeshProUGUI>().text = GameVersion.GameVersionShorthand;
            } else {
                GetComponent<TextMeshProUGUI>().text = GameVersion.GameVersionLonghand;
            }
        }

    }

}