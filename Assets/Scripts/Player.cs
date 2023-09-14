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
      private float jumpHeight;
      [SerializeField]
      private float dashDistance;

      [Header("Sprint")]
      [SerializeField]
      [Tooltip("Sprinting acceleration rate (multiplied with speed")]
      private float sprint_Factor;
      [Tooltip("The necessary stamina value to start sprinting (not implement yet")]
      private float sprint_Threshold = 2f;

      [Header("Stamina")]
      [SerializeField]
      private float stamina;
      private float stamina_Max = 5f;
      [Tooltip("The change rate of stamina (per second")]
      private float stamina_Change = 1f;

      [Header("Status Check")]
      [SerializeField]
      [Tooltip("whether invert Y axis of mouse")]
      private bool isYInvert;
      [SerializeField]
      [Tooltip("whether Player is on ground")]
      private bool isOnGround;
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
         isOnGround = checkOnGround();

         if(Input.GetButtonDown("Jump")) {
            jump();
         }

         if(Input.GetKeyDown(KeyCode.LeftControl)) {
            dash();
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

            stamina -= stamina_Change * Time.deltaTime;
         }
         else if(!Input.GetKey(KeyCode.LeftShift) && stamina < stamina_Max) {
            stamina += stamina_Change * Time.deltaTime;
         }

         if(stamina > stamina_Max) {
            stamina = stamina_Max;
         }

         cc.Move(moveDirection * moveSpeed);
      }

      /// <summary>
      /// make Player with gravity and fall
      /// </summary>
      private void underGravity() {
         if(hasGravity && !isOnGround) {
            velocity.y += gravity * Time.deltaTime;
            cc.Move(0.5f * velocity * Time.deltaTime);
         }
         else if(velocity.y < 0 && isOnGround) {
            velocity.y = -1f;
         }
      }

      /// <summary>
      /// jump, implement by CharacterController
      /// use calculated simulated force instead of the Physical system in Unity
      /// </summary>
      private void jump() {
         if(isOnGround) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            cc.Move(0.5f * velocity * Time.deltaTime);
         }
      }

      /// <summary>
      /// dash, implement by CharacterController
      /// </summary>
      private void dash() {
         cc.Move(moveDirection * dashDistance);
      }

      /// <summary>
      /// check whether Player is in contact with ground Mask by Physics.CheckSphere
      /// </summary>
      /// <returns></returns>
      private bool checkOnGround() {
         return Physics.CheckSphere(feet.transform.position, groundCheckDistance, GroundMask);
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