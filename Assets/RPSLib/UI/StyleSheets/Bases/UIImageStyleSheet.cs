/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;

namespace RPSCore {

    [CreateAssetMenu(fileName = "New Image Style", menuName = "RPS Tools/UI Styling/New Image Style")]
    public class UIImageStyleSheet : StyleSheetBase {

        [Header("Images")]
        public Sprite PrimaryImage;

        [Header("Styles")]
        public Color PrimaryColor;

    }

}