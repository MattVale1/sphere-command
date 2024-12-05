/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;

namespace RPSCore {

    public class CursorService : MonoBehaviour {

        #region VARIABLES - USE SUB-REGIONS 
        #region CURSORS
        public enum CursorType {
            MAIN, UI_HOVER, UI_CLICK, ATTACK, DEFEND
        }
        public Texture2D main;
        public Texture2D ui_hover;
        public Texture2D ui_click;
        public Texture2D attack;
        public Texture2D defend;
        #endregion
        #region CURSOR DATA
        public CursorMode cursorMode = CursorMode.Auto;
        public Vector2 hotSpot = Vector2.zero;
        #endregion
        #endregion

        #region INIT + INSTANCING
        public static CursorService Instance;

        protected void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            } else {
                Instance = this;
            }
            SetCursorType(CursorType.MAIN);
            DontDestroyOnLoad(gameObject);
        }
        #endregion

        #region PUBLIC METHODS
        public void SetCursorType(CursorType targetCursor) {
            switch (targetCursor) {
                case CursorType.MAIN:
                    Cursor.SetCursor(main, hotSpot, cursorMode);
                    break;
                case CursorType.UI_HOVER:
                    Cursor.SetCursor(ui_hover, hotSpot, cursorMode);
                    break;
                case CursorType.UI_CLICK:
                    Cursor.SetCursor(ui_click, hotSpot, cursorMode);
                    break;
                case CursorType.ATTACK:
                    Cursor.SetCursor(attack, hotSpot, cursorMode);
                    break;
                case CursorType.DEFEND:
                    Cursor.SetCursor(defend, hotSpot, cursorMode);
                    break;

                default:
                    Cursor.SetCursor(main, hotSpot, cursorMode);
                    break;
            }
        }
        #endregion

    }

}