/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace RPSCore {

    public class CameraControllerV2 : MonoBehaviour {

        #region VARIABLES - USE SUB-REGIONS
        #region Singleton
        public static CameraControllerV2 Instance;
        #endregion

        #region Cached Properties
        private Camera _camera;
        [Header("Camera Pivot")]
        public Transform cameraPivot;
        private Transform _cameraTransform;
        #endregion

        #region Input System Actions
        // Positional input actions
        private InputAction _moveAction;
        private InputAction _moveBoostAction;
        // Mouse input actions
        private InputAction _mousePositionAction;
        private InputAction _mouseScrollWheelAction;
        private InputAction _middleMouseClickedAction;
        #endregion

        #region Movement (H + V)
        [Header("Normal Movement")]
        public float moveSpeed = 10f;
        public float movementSmoothness = 2f;

        private float _currentMaxMoveSpeed;
        public float maxMoveSpeed = 100f;

        public bool canBoost = false;
        public float boostMoveMultiplier = 3f;
        public float maxBoostSpeed = 200f;

        private Vector2 _moveDirection;
        private Vector3 _directionalVector = Vector3.zero;
        #endregion

        #region Camera Rotation
        [Header("Rotations")]
        public Vector3 startingRotation = new(45f, 0f, 0f);
        public float rotationSensitivity = 4f;
        public float orbitDampening = 10f;
        private Vector3 _localRot;
        #endregion

        #region Camera Zoom
        private float _currentZoomDistance = 0f;
        [Header("Zoom")]
        public float maxZoom = 1.5f;
        public float minZoom = 20f;
        public float zoomDistanceMultiplier = 1.5f;
        public float zoomDampening = 6f;
        #endregion

        #region Camera Shake
        //private bool _cameraShaking = false;
        private float _targetShakeIntensity = 0f;
        private float _currentShakeIntensity = 0f;
        private readonly float _shakeChangeSpeed = 1.5f;
        #endregion

        #region Mouse Position + Delta + Drifting
        private Vector3 _mousePos;
        private Vector2 _mouseDelta;
        [Header("Camera Drifting")]
        public float cameraDriftAmount = 2f;
        #endregion

        #region GoTo / Following Object
        private Transform _objectToFollow;
        private bool _isFollowing = false;
        [Header("Following Objects")]
        public float followSpeed = 2f;
        private Vector2 _mouseOffsetPosition;

        private Vector3 _targetPosition;
        private bool _movingToTargetPosition = false;
        [Header("Go To Position")]
        public float goToPositionSpeed = 10f;
        public float reachedTargetDistance = 0.01f;
        #endregion

        #region Clamping
        [Header("Clamping")]
        public Vector2 minMaxYRotation;
        public bool shouldClampPosition = true;
        public Vector3 minPositionConstraints;
        public Vector3 maxPositionConstraints;
        #endregion
        #endregion


        #region Setup
        protected void Awake() {
            Instance = this;
            Init();
        }

        private void Init() {
            CacheVars();
            CacheInput();
            InitDefaultCameraState();
        }

        private void CacheVars() {
            _camera = GetComponent<Camera>();
            _cameraTransform = transform;

            _currentMaxMoveSpeed = maxMoveSpeed;
        }

        private void CacheInput() {
            _moveAction = InputSystem.actions.FindAction(InputActionConstants.DirectionalMove);
            _moveBoostAction = InputSystem.actions.FindAction(InputActionConstants.Sprint);
            _moveBoostAction.started += EnableBoost;
            _moveBoostAction.canceled += DisableBoost;

            _mousePositionAction = InputSystem.actions.FindAction(InputActionConstants.MousePosition);
            _mouseScrollWheelAction = InputSystem.actions.FindAction(InputActionConstants.MouseScrollWheel);
            _middleMouseClickedAction = InputSystem.actions.FindAction(InputActionConstants.MouseScrollButton);
        }

        private void InitDefaultCameraState() {
            _localRot.y = startingRotation.x;
        }

        protected void OnDestroy() {
            _moveBoostAction.started -= EnableBoost;
            _moveBoostAction.canceled -= DisableBoost;
        }
        #endregion

        #region Input Handling
        protected void Update() {
            GetPositionInput();
            GetRotationAndZoomInput();
        }

        private void GetPositionInput() {
            _moveDirection = _moveAction.ReadValue<Vector2>();
        }

        private void GetRotationAndZoomInput() {
            // Mouse position
            Vector2 mousePosRaw = Mouse.current.position.ReadValue();
            _mousePos = _camera.ScreenToWorldPoint(mousePosRaw);
            _mouseDelta = Mouse.current.delta.ReadValue() * Time.unscaledDeltaTime;

            // Mouse middle button
            if (_middleMouseClickedAction.IsPressed()) {
                _localRot.x += _mouseDelta.x * rotationSensitivity;
                _localRot.y -= _mouseDelta.y * rotationSensitivity;
            }

            // Mouse scroll wheel
            float scrollAmount = _mouseScrollWheelAction.ReadValue<Vector2>().y;
            scrollAmount *= zoomDistanceMultiplier;
            CalculateZoom(scrollAmount);
        }
        #endregion

        #region Positioning
        private void UpdateCameraPosition() {
            // DirectionalMove on the horizontal axis (left and right).
            Vector3 smoothMoveHorizontal = cameraPivot.position + _moveDirection.x * moveSpeed * Time.deltaTime * cameraPivot.right;
            cameraPivot.position = Vector3.SmoothDamp(cameraPivot.position, smoothMoveHorizontal, ref _directionalVector, movementSmoothness, _currentMaxMoveSpeed, Time.unscaledDeltaTime);

            // Get the rotation of the camera pivot so we can move in the facing direction, but set y to 0, so our height doesn't change.
            // This mean we only move on the X, Z axis, ignoring Y. 
            Vector3 pivotDirection = cameraPivot.forward;
            pivotDirection.y = 0f;
            pivotDirection.Normalize();

            // DirectionalMove on the vertical axis (forward and back), but ignore the X rotation of the pivot.
            Vector3 smoothMoveVertical = cameraPivot.position + _moveDirection.y * moveSpeed * Time.deltaTime * pivotDirection;
            cameraPivot.position = Vector3.SmoothDamp(cameraPivot.position, smoothMoveVertical, ref _directionalVector, movementSmoothness, _currentMaxMoveSpeed, Time.unscaledDeltaTime);
        }
        #endregion

        #region Rotation and Zooming
        private void CalculateZoom(float scrollAmount) {
            _currentZoomDistance += scrollAmount * -1f;
            _currentZoomDistance = Mathf.Clamp(_currentZoomDistance, maxZoom, minZoom);

            // Change speed of camera based on zoom
            //moveSpeed += (scrollAmount * moveSpeedZoomMultiplier) * -1f;
            //moveSpeed = Mathf.Clamp(moveSpeed, _baseMoveSpeed, maxMoveSpeed);
        }

        private void UpdateCameraRotationsAndZoom() {
            // Clamp camera rotation
            _localRot.y = Mathf.Clamp(_localRot.y, minMaxYRotation.x, minMaxYRotation.y);

            // Rotate
            Quaternion qt = Quaternion.Euler(_localRot.y, _localRot.x, 0);
            cameraPivot.rotation = Quaternion.Lerp(cameraPivot.rotation, qt, orbitDampening * Time.unscaledDeltaTime);

            // Zoom
            if (_cameraTransform.localPosition.z != _currentZoomDistance * -1f) {
                _cameraTransform.localPosition = new Vector3(
                    0f,
                    0f,
                    Mathf.Lerp(_cameraTransform.localPosition.z, _currentZoomDistance * -1f, zoomDampening * Time.unscaledDeltaTime)
                );
            }
        }
        #endregion

        #region Boosting
        private void EnableBoost(CallbackContext ctx) {
            if (!canBoost)
                return;

            _currentMaxMoveSpeed = maxBoostSpeed;
        }

        private void DisableBoost(CallbackContext ctx) {
            if (!canBoost)
                return;

            _currentMaxMoveSpeed = maxMoveSpeed;
        }
        #endregion

        #region Camera Update Handling (LateUpdate)
        protected void LateUpdate() {
            // Always update camera rotation and zoom
            UpdateCameraRotationsAndZoom();

            // Camera shake
            if (_targetShakeIntensity >= 0f) {
                _currentShakeIntensity = Mathf.Lerp(_currentShakeIntensity, _targetShakeIntensity, _shakeChangeSpeed * Time.unscaledDeltaTime);
                _cameraTransform.localPosition += Random.insideUnitSphere * _currentShakeIntensity;

                if (_currentShakeIntensity == 0f) {
                    _targetShakeIntensity = 0f;
                }
            }

            if (_isFollowing) {
                if (_objectToFollow == null) {
                    RPSLib.Debug.Log(name + ":: Marked as following, but no target object to follow!", RPSLib.Debug.Style.Warning);
                    _isFollowing = false;
                    return;
                }

                Vector3 _mouseDirection = _objectToFollow.position - _mousePos;
                if (_mouseDirection.magnitude > cameraDriftAmount) {
                    _mouseDirection = _mouseDirection.normalized * cameraDriftAmount;
                }

                //cameraPivot.position = Vector3.Slerp(cameraPivot.position, _objectToFollow.position + _mouseDirection, followSpeed * Time.unscaledDeltaTime);
                cameraPivot.position = Vector3.SmoothDamp(cameraPivot.position, _objectToFollow.position + _mouseDirection, ref _directionalVector, followSpeed * Time.unscaledDeltaTime, _currentMaxMoveSpeed, Time.unscaledDeltaTime);
                return;
            }

            if (_movingToTargetPosition) {
                if (_targetPosition == null) {
                    RPSLib.Debug.Log(name + " :: Marked as moving to target position, but no target position to go to!", RPSLib.Debug.Style.Warning);
                    _movingToTargetPosition = false;
                    return;
                }

                if (Vector3.Distance(cameraPivot.position, _targetPosition) < reachedTargetDistance) {
                    _movingToTargetPosition = false;
                }

                cameraPivot.position = Vector3.Slerp(cameraPivot.position, _targetPosition, goToPositionSpeed * Time.unscaledDeltaTime);
                return;
            }

            UpdateCameraPosition();
            ClampCameraPosition();
        }
        #endregion

        #region Clamping
        private void ClampCameraPosition() {
            if (shouldClampPosition)
                cameraPivot.position = new Vector3(
                    Mathf.Clamp(cameraPivot.position.x, minPositionConstraints.x, maxPositionConstraints.x),
                    Mathf.Clamp(cameraPivot.position.y, minPositionConstraints.y, maxPositionConstraints.y),
                    Mathf.Clamp(cameraPivot.position.z, minPositionConstraints.z, maxPositionConstraints.z)
                );
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Set the position of the camera immediately, with no care for animations or smoothing.
        /// </summary>
        public void SetPositionImmediate(Vector3 newPos) {
            cameraPivot.position = newPos;
        }

        /// <summary>
        /// Set the target position of the camera and smoothly move to it.
        /// </summary>
        public void SetPositionSmooth(Vector3 targetPos) {
            _movingToTargetPosition = true;
            _targetPosition = targetPos;
        }

        /// <summary>
        /// Pause normal camera controls mand follow a target.
        /// </summary>
        public void StartFollowingObject(Transform targetTransform) {
            _objectToFollow = targetTransform;
            _isFollowing = true;
        }

        /// <summary>
        /// Stop following a target and resume normal camera controls.
        /// </summary>
        public void StopFollowingObject() {
            _isFollowing = false;
            _objectToFollow = null;
        }

        public Vector3 GetCameraRotation() {
            return _localRot;
        }

        public void SetCameraRotation_Instant(Vector3 newRot) {
            _localRot = newRot;

            Quaternion qt = Quaternion.Euler(_localRot.y, _localRot.x, 0);
            cameraPivot.rotation = qt;
        }

        public void ToggleCameraShake(float targetValue) {
            _targetShakeIntensity = targetValue;
            //_currentShakeIntensity = _targetShakeIntensity;

            //if (targetValue == 0f) {
            //    _cameraShaking = false;
            //    return;
            //}

            //_cameraShaking = true;        
        }
        #endregion

    }

}