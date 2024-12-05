/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPSCore {

    [ExecuteInEditMode()]
    public class Tooltip : MonoBehaviour {

        #region VARIABLES - USE SUB-REGIONS 
        #region CACHE
        private RectTransform mTransform;
        public TextMeshProUGUI headerText;
        public TextMeshProUGUI contentText;
        public LayoutElement layoutElement;
        #endregion
        #region CONSTRAINTS
        public int charWrapLimit;
        public float mouseOffset = 10f;
        #endregion
        #endregion

        #region SETUP
        protected void Awake() {
            mTransform = GetComponent<RectTransform>();
        }
        #endregion

        #region PUBLIC METHODS
        public void SetText(string content, string header = "") {
            if (string.IsNullOrEmpty(header)) {
                headerText.gameObject.SetActive(false);
            } else {
                headerText.gameObject.SetActive(true);
                headerText.text = header;
            }
            contentText.text = content;

            // Set size based on contents
            int headerLength = headerText.text.Length;
            int contentLength = contentText.text.Length;
            layoutElement.enabled = headerLength > charWrapLimit || contentLength > charWrapLimit;
        }
        #endregion

        #region TOOLTIP POSITIONING
        protected void LateUpdate() {
            Vector2 mousePos = Input.mousePosition;
            float pivotX = mousePos.x / Screen.width;
            float pivotY = mousePos.y / Screen.height;
            mTransform.pivot = new Vector2(pivotX, pivotY + mouseOffset);
            mTransform.position = mousePos;
            //transform.position = mousePos;
        }
        #endregion

    }

}