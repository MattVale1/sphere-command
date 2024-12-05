/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;

namespace RPSCore {

    public class ObjectAutoScaler : MonoBehaviour {

        #region VARIABLES - USE SUB-REGIONS
        private Transform _transform;
        private Transform _cameraTransform;

        public bool inverse = false;

        public float baseScale = 1.0f;
        public float distanceMultiplier = 1.0f;
        private float _cameraDistance = 0.0f;

        public float minDistance = 0.5f;
        public float maxDistance = 10f;

        public float maxScale = 20f;
        #endregion


        #region SETUP
        protected void Awake() {
            Init();
        }

        private void Init() {
            CacheVars();
        }

        private void CacheVars() {
            _transform = transform;
            _cameraTransform = Camera.main.transform;
        }
        #endregion

        #region SCALING
        protected void Update() {
            if (_cameraTransform != null) {
                _cameraDistance = Vector3.Distance(_cameraTransform.position, _transform.position);

                float scale;
                if (inverse) {
                    scale = Mathf.Lerp(maxScale, baseScale, Mathf.InverseLerp(minDistance, maxDistance, _cameraDistance * distanceMultiplier));
                } else {
                    scale = Mathf.Lerp(baseScale, maxScale, Mathf.InverseLerp(minDistance, maxDistance, _cameraDistance * distanceMultiplier));
                }

                _transform.localScale = new Vector3(scale, scale, scale);
            }
        }
        #endregion

    }

}