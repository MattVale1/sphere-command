/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;
using UnityEngine.UI;

namespace RPSCore {

    [CreateAssetMenu(fileName = "New Button Style", menuName = "RPS Tools/UI Styling/New Button Style")]
    public class UIButtonStyleSheet : StyleSheetBase {

        [Header("Images")]
        public Sprite primaryImage;

        [Header("Styles")]
        public ColorBlock style;

    }

}