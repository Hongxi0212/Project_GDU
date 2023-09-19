using UnityEngine;

namespace GDUGame {
   public class Player: MonoBehaviour {
      private const float gravity = -9.81f;

      #region Variables
      private CharacterController cc;

      private GameObject head;
      private GameObject feet;

      [SerializeField]
      [Tooltip("Record Player velocity (only used for vertical speed now")]
      private Vector3 velocity;
      private Vector3 moveDirection;
      public LayerMask GroundMask;
      public LayerMask WallMask;
      private PlayerView pv = new();

      [SerializeField]
      [Tooltip("perspective rotation rate (multiplied with rotate dirction")]
      private float rotateSensititvity;
      [SerializeField]
      private float groundCheckDistance;

      [Header("Basic Movement")]
      [SerializeField]
      private float moveSpeed;
      [SerializeField]
      [Tooltip("influence falling speed (multiplied with gravity")]
      private float mass;
      [SerializeField]
      private float jumpHeight;
      [SerializeField]
      private float dashDistance;

      [Header("Climb")]
      [SerializeField]
      [Tooltip("The distance of detecting whether has wall in front")]
      private float climbDetectionDis;
      [SerializeField]
      [Tooltip("The player can climb only when the Angle between the view and the wall normal is less than this value")]
      private float climbableViewAngle;
      [SerializeField]
      private float climbDuration;
      [SerializeField]
      private float climbDuration_Max;
      [SerializeField]
      private float climbSpeed;


      [Header("Sprint")]
      [SerializeField]
      [Tooltip("Sprinting acceleration rate (multiplied with speed")]
      private float sprint_Factor;
      [Tooltip("The necessary stamina value to start sprinting (not implement yet")]
      private float sprint_Threshold = 2f;

      [Header("Stamina")]
      [SerializeField]
      private float stamina;
      [SerializeField]
      private float stamina_Max;
      [SerializeField]
      [Tooltip("The cost rate of stamina (per second")]
      private float stamina_Cost;
      [SerializeField]
      [Tooltip("The recover rate of stamina (per second")]
      private float stamina_Recover;

      [Header("Status Check")]
      [SerializeField]
      [Tooltip("whether invert Y axis of mouse")]
      private bool isYInvert;
      [SerializeField]
      [Tooltip("whether Player is on ground")]
      private bool isOnGround;
      [SerializeField]
      [Tooltip("whether Player is climbing on the wall")]
      private bool isClimbing;
      [SerializeField]
      [Tooltip("whether Player influenced by gravity")]
      private bool hasGravity;

      #endregion

      private void Awake() {
         Cursor.visible = false;
         Cursor.lockState = CursorLockMode.Locked;

         cc = GetComponent<CharacterController>();

         head = transform.GetChild(0).gameObject;
         feet = transform.GetChild(1).gameObject;

         velocity = Vector3.zero;
         pv.set(head.transform.localEulerAngles);
      }

      private void Update() {
         if(Input.GetButtonDown("Jump")) {
            jump();
         }

         isOnGround = checkOnGround();

         if(Input.GetKeyDown(KeyCode.LeftControl)) {
            dash();
         }

         if(Input.GetKey(KeyCode.W) && checkFrontWallandClimbable()) {
            climb();
         }

         underGravity();

         lookAt(pv.get());
      }

      private void FixedUpdate() {
         move();
      }

      /// <summary>
      /// rotate the Player Camera to input vector
      /// and synchronize the body rotation with perspective
      /// </summary>
      /// <param name="vector"></param>
      private void lookAt(Vector3 vector) {
         var mouseMovement = getInputViewRotation() * rotateSensititvity;

         pv.yaw += mouseMovement.x;
         pv.pitch += mouseMovement.y;

         vector = new Vector3(Mathf.Clamp(vector.x, -30, 60), vector.y, vector.z);

         head.transform.eulerAngles = new Vector3(vector.x, vector.y, vector.z);
         transform.eulerAngles = new Vector3(0f, vector.y, 0f);
      }

      /// <summary>
      /// move the Player
      /// include sprint and dash
      /// </summary>
      private void move() {
         moveDirection = getInputMoveDirection();

         moveDirection = new Vector3(moveDirection.x, 0f, moveDirection.z);

         if(Input.GetKey(KeyCode.LeftShift) && stamina > 0) {
            moveDirection *= sprint_Factor;

            stamina -= stamina_Cost * Time.deltaTime;
         }
         else if(!Input.GetKey(KeyCode.LeftShift) && stamina < stamina_Max) {
            stamina += stamina_Recover * Time.deltaTime;
         }

         if(stamina > stamina_Max) {
            stamina = stamina_Max;
         }

         if(isClimbing) {
            moveDirection.x = 0f;
            moveDirection.z *= 0.5f;
         }

         cc.Move(moveDirection * moveSpeed);
      }

      /// <summary>
      /// make Player with gravity and fall
      /// </summary>
      private void underGravity() {
         if(hasGravity && !isOnGround) {
            velocity.y += mass * 0.5f * gravity * Time.deltaTime;
            cc.Move(velocity * Time.deltaTime);
         }
         else if(velocity.y < 0 && isOnGround) {
            if(!isClimbing) {
               velocity.y = 0f;
            }

            isClimbing = false;
            climbDuration = climbDuration_Max;
         }
      }

      /// <summary>
      /// jump, implement by CharacterController
      /// use calculated simulated force instead of the Physical system in Unity
      /// </summary>
      private void jump() {
         if(isOnGround) {
            velocity.y = jumpHeight;
            cc.Move(velocity * Time.deltaTime);
         }
      }

      /// <summary>
      /// dash, implement by CharacterController
      /// </summary>
      private void dash() {
         cc.Move(moveDirection * dashDistance);
      }

      private void climb() {
         if(climbDuration > 0) {
            isOnGround = false;
            isClimbing = true;
            climbDuration -= Time.deltaTime;

            velocity.y = climbSpeed / mass;
            cc.Move(velocity * Time.deltaTime);
         }
         else {
            isClimbing = false;
         }
      }
      

      /// <summary>
      /// check whether Player is in contact with ground Mask by Physics.CheckSphere
      /// </summary>
      /// <returns></returns>
      private bool checkOnGround() {
         return Physics.CheckSphere(feet.transform.position, groundCheckDistance, GroundMask);
      }

      private bool checkFrontWallandClimbable() {
         RaycastHit hit;

         if(Physics.Raycast(feet.transform.position, transform.forward, out hit, climbDetectionDis, WallMask)) {
            return Vector3.Angle(head.transform.forward, -hit.normal) <= climbableViewAngle/2f;
         }

         return false;
      }

      /// <summary>
      /// get input move direction from player (WASD
      /// </summary>
      /// <returns></returns>
      private Vector3 getInputMoveDirection() {
         Vector3 direction = Vector3.zero;
         direction = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");

         return direction;
      }

      /// <summary>
      /// get input mouse rotation from player
      /// </summary>
      /// <returns></returns>
      private Vector2 getInputViewRotation() {
         if(!isYInvert) {
            return new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
         }
         else {
            return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
         }
      }

      /// <summary>
      /// Used to record rotation of Player Camera and direction of upcoming rotation
      /// </summary>
      struct PlayerView {
         public float pitch;
         public float yaw;
         public float roll;

         public void set(Vector3 v) {
            pitch = v.x;
            yaw = v.y;
            roll = v.z;
         }

         public Vector3 get() {
            return new Vector3(pitch, yaw, roll);
         }
      }
   }
}