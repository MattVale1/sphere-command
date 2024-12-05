/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;
using UnityEngine.UI;

namespace RPSCore {

    public class UIAnimator : MonoBehaviour {

        #region VARIABLES - USE SUB-REGIONS 
        #region CACHE
        [Header("UI Elements For Animation")]
        [Tooltip("Should be placed in order of animations being run.")]
        public RectTransform[] UIElementsToAnimate;
        #endregion
        #region ANIM SPEEDS
        private const float animSpeed = 0.6f;
        private float randomAnimSpeed;
        private const float animSpeedVariance = 0.4f;
        [Header("Animation Speed Override")]
        public float animSpeedOverride = 0f;
        #endregion
        #region ANIM ON START
        [Header("When To Animate?")]
        public bool animateOnEnable = true;
        #endregion
        #region ANIM TYPES
        [Header("Animation Type")]
        public AnimType animType;
        public enum AnimType {
            SCALE_PUNCH,
            MOVE_RELATIVE,
            SMOOTH_ALPHA_IN_OUT
        }
        public bool looping = false;
        public Vector3 relativePositionMove;
        #endregion
        #endregion


        #region SETUP
        protected void OnEnable() {
            if (UIElementsToAnimate == null || UIElementsToAnimate.Length == 0) {
                return;
            }

            if (animateOnEnable) {
                Play();
            }
        }

        protected void OnDisable() {
            if (UIElementsToAnimate.Length == 0) {
                return;
            }

            for (int i = 0; i < UIElementsToAnimate.Length; i++) {
                LeanTween.cancel(UIElementsToAnimate[i]);
            }
        }
        #endregion

        #region PUBLIC METHODS
        public void Play() {
            switch (animType) {
                case AnimType.SCALE_PUNCH:
                    for (int i = 0; i < UIElementsToAnimate.Length; i++) {
                        LeanTween.cancel(UIElementsToAnimate[i]);
                        UIElementsToAnimate[i].localScale = Vector3.one;
                        randomAnimSpeed = animSpeed + Random.Range(-animSpeedVariance, animSpeedVariance);
                        LeanTween.scale(UIElementsToAnimate[i], Vector3.one * 1.05f, randomAnimSpeed).setEasePunch().setIgnoreTimeScale(true);
                    }
                    break;
                case AnimType.MOVE_RELATIVE:
                    for (int i = 0; i < UIElementsToAnimate.Length; i++) {
                        LeanTween.cancel(UIElementsToAnimate[i]);
                        LeanTween.move(UIElementsToAnimate[i], relativePositionMove, animSpeed).setIgnoreTimeScale(true);
                    }
                    break;
                case AnimType.SMOOTH_ALPHA_IN_OUT:
                    for (int i = 0; i < UIElementsToAnimate.Length; i++) {
                        LeanTween.cancel(UIElementsToAnimate[i]);
                        Color col = UIElementsToAnimate[i].GetComponent<Image>().color;
                        col.a = 0.5f;
                        LeanTween.color(UIElementsToAnimate[i], col, animSpeed).setLoopPingPong();
                    }
                    break;
            }
        }
        #endregion

    }

}