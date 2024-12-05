/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RPSCore {

    public class UILookAtCamera : MonoBehaviour {

        #region VARS
        // Cache
        private Transform UICam;
        private Transform thisTransform;
        // Distance until hide
        public bool hideAfterDistance = false;
        public Image[] UIImages;
        public bool shouldScale;
        public float distanceToHide = 450f;
        // Update delay
        private readonly WaitForSeconds updateDelay = new(0.1f); //How often do we want the UI to update looking at the camera? (Performance)
        #endregion

        #region SETUP AND CACHING                                                            
        protected void OnEnable() {
            if (CameraControllerV2.Instance != null) {
                //UICam = CameraControllerV2.Instance.UICamera.transform;
            }
            thisTransform = transform;

            StartCoroutine(UpdateUI());
        }
        #endregion
        #region DISABLING
        protected void OnDisable() {
            StopAllCoroutines();
        }
        #endregion
        #region VISIBLE OR NOT?
        protected void OnBecameVisible() {
            StopAllCoroutines(); // Stop all first to make sure we don't run more than 1 at same time
            StartCoroutine(UpdateUI());
        }
        protected void OnBecameInvisible() {
            StopAllCoroutines();
        }
        #endregion

        #region COROUTINE TO CHECK IF MAIN CAM IS ACTIVE
        IEnumerator UpdateUI() {
            yield return updateDelay;
            if (UICam.gameObject.activeInHierarchy) {
                LookAtCamera();
            }
            StopAllCoroutines();
            StartCoroutine(UpdateUI());
        }
        #endregion

        #region MAKE THIS UI ELEMENT LOOK AT THE MAIN CAMERA
        void LookAtCamera() {
            if (hideAfterDistance) {
                if (RPSLib.Maths.Distances.GetFastDistance(thisTransform.position, UICam.position) > distanceToHide) {
                    for (int i = 0; i < UIImages.Length; i++) {
                        UIImages[i].enabled = false;
                    }
                } else {
                    for (int i = 0; i < UIImages.Length; i++) {
                        UIImages[i].enabled = true;
                    }
                }
            }
            thisTransform.rotation = UICam.transform.rotation;
            if (shouldScale) {

            }
        }
        #endregion

    }

}