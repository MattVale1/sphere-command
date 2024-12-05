/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;
using UnityEngine.UI;

namespace RPSCore {

    public class GameSpeedController : MonoBehaviour {

        #region VARIABLES - USE SUB-REGIONS 
        #region INSTANCING
        public static GameSpeedController Instance;
        #endregion

        #region UI
        public Button buttonSpeed_Pause;
        private ColorBlock originalBtnColor_Pause;
        public Button buttonSpeed_1;
        private ColorBlock originalBtnColor_1;
        public Button buttonSpeed_2;
        private ColorBlock originalBtnColor_2;
        public Button buttonSpeed_3;
        private ColorBlock originalBtnColor_3;
        #endregion
        #endregion


        #region SETUP
        protected void Awake() {
            Instance = this;
            Init();
        }

        private void Init() {
            originalBtnColor_Pause = buttonSpeed_Pause.colors;
            originalBtnColor_1 = buttonSpeed_1.colors;
            originalBtnColor_2 = buttonSpeed_2.colors;
            originalBtnColor_3 = buttonSpeed_3.colors;
            SetGameSpeed(1f);
        }

        private void ResetButtonColors() {
            buttonSpeed_Pause.colors = originalBtnColor_Pause;
            buttonSpeed_1.colors = originalBtnColor_1;
            buttonSpeed_2.colors = originalBtnColor_2;
            buttonSpeed_3.colors = originalBtnColor_3;
        }
        #endregion

        #region INPUTS
        protected void Update() {
            GetInputs();
        }

        private void GetInputs() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (Time.timeScale != 0) {
                    SetGameSpeed(0f);
                } else {
                    SetGameSpeed(1f);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                SetGameSpeed(1f);
                return;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                SetGameSpeed(3f);
                return;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                SetGameSpeed(5f);
                return;
            }
        }
        #endregion

        #region TIME MANAGEMENT
        private void SetTimeScale(float value, bool lerp, float lerpSpeed) {
            if (lerp) {
                RPSLib.Utils.TimeUtil.LerpTimeScale(Time.timeScale, value, lerpSpeed);
            } else {
                RPSLib.Utils.TimeUtil.SetTimeScale(value);
            }
        }
        #endregion

        #region PUBLIC METHODS
        public void SetGameSpeed(float speed, bool lerp = false, float lerpSpeed = 1f) {
            ResetButtonColors();

            ColorBlock colors;
            switch (speed) {
                case 0f:
                    colors = buttonSpeed_Pause.colors;
                    colors.normalColor = colors.highlightedColor;
                    buttonSpeed_Pause.colors = colors;
                    break;
                case 1f:
                    colors = buttonSpeed_1.colors;
                    colors.normalColor = colors.highlightedColor;
                    buttonSpeed_1.colors = colors;
                    break;
                case 3f:
                    colors = buttonSpeed_2.colors;
                    colors.normalColor = colors.highlightedColor;
                    buttonSpeed_2.colors = colors;
                    break;
                case 5f:
                    colors = buttonSpeed_3.colors;
                    colors.normalColor = colors.highlightedColor;
                    buttonSpeed_3.colors = colors;
                    break;
            }

            SetTimeScale(speed, lerp, lerpSpeed);
        }
        #endregion

    }

}