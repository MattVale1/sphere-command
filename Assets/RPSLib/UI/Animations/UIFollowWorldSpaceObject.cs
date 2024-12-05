/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;

namespace RPSCore {

    public class UIFollowWorldSpaceObject : MonoBehaviour {

        #region VARIABLES - USE SUB-REGIONS 
        #region CACHE
        private Camera mainCam;
        private Transform mTransform;
        public Transform objectToFollow;
        public float yOffset = 100f;
        public float moveToSpeed = 15f;
        #endregion
        #endregion


        #region SETUP
        protected void Awake() {
            mainCam = Camera.main;
            mTransform = transform;
        }
        #endregion


        #region FOLLOWING
        protected void Update() {
            if (objectToFollow != null && gameObject.activeInHierarchy) {
                Vector2 sp = mainCam.WorldToScreenPoint(objectToFollow.position);
                sp.y += yOffset;
                mTransform.position = Vector3.Lerp(mTransform.position, sp, moveToSpeed * Time.unscaledDeltaTime);
            }
        }
        #endregion

        #region PUBLIC METHODS
        public void SetObjectToFollow(Transform objectToFollow) {
            this.objectToFollow = objectToFollow;
        }
        #endregion

    }

}