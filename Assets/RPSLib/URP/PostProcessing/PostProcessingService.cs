/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace RPSCore {

    public class PostProcessingService : MonoBehaviour {

        #region Instancing
        public static PostProcessingService Instance { get; private set; }
        #endregion

        #region Private Properties
        private LensDistortion _lensDistortion;
        private float _targetLensDistortionValue = 0f;
        private float _targetLensDistortionSpeed = 0f;
        private float _lensDistortionTime = 0f;
        #endregion

        #region Public Properties
        public Volume ActiveVolume;
        public VolumeProfile ActiveVolumeProfile;
        #endregion


        #region Unity Flow
        private void Awake() {
            Instance = this;

            ActiveVolumeProfile = ActiveVolume.profile;

            if (ActiveVolumeProfile.TryGet<LensDistortion>(out var effect)) {
                _lensDistortion = effect;
            }
        }

        private void LateUpdate() {
            if (_lensDistortion != null) {
                if (_lensDistortion.intensity.value == 0f && _targetLensDistortionValue == 0f) {
                    _lensDistortion.active = false;
                    return;
                }

                _lensDistortionTime += Time.deltaTime / _targetLensDistortionSpeed;
                _lensDistortion.intensity.value = Mathf.Lerp(_lensDistortion.intensity.value, _targetLensDistortionValue, _lensDistortionTime);
            }
        }
        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        public void ToggleFishEye(float targetValue, float speed) {
            if (_lensDistortion == null) {
                RPSLib.Debug.Log("No Lens Distortion Effect found.");
                return;
            }

            _lensDistortionTime = 0f;
            _targetLensDistortionValue = targetValue;
            _targetLensDistortionSpeed = speed;
            _lensDistortion.active = true;
        }
        #endregion

    }

}