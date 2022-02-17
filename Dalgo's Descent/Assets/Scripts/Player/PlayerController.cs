using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [Header("Modifer")]
    [SerializeField] private Vector3 Gravity = new Vector3 (0,-10.0f,0);
    [SerializeField][Min(1)] private float Mass;
    [Range(5, 20)][SerializeField] private float JumpForce;
    [Range(0, 10)][SerializeField] private float RunSpeed;
    [Range(0, 10)][SerializeField] private float WalkSpeed;
    [Range(0, 5)][SerializeField] private float SlowSpeed;
    [Range(0, 10)][SerializeField] private float TurnSpeed;
    [Range(0, 1)][SerializeField] private double AttackInputGracePeriod;
    [Range(0, 2)][SerializeField] private double AttackDuration;

    private Vector3 Impact;
    private double AttackInputTimer = 0;

    [Header("Variables")]
    [SerializeField] private LayerMask GroundLayer;
    public CharacterController Controller;
    public Animator PlayerAnimator;
    public PlayerInput InputScript;
    public Vector3 MoveDirection { get; private set; }
    public Quaternion Rotation { get; private set; }
    public float Velocity { get; private set; }
    public bool IsMoving { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsJump { get; private set; }
    public bool IsLanding { get; private set; }
    public bool IsGrounded { get; private set; }
    public bool IsAttack { get; private set; }
    
    //Animator Parameter Hashes
    private int IsWalkingHash;
    private int JumpTriggerHash;
    private int IsLandingHash;
    private int IsGroundedHash;
    private int VelocityHash;
    private int IsAttackHash;
    private int AttackTriggerHash;

    private float weight = 0;
    void Awake()
    {
        InputScript = GetComponent<PlayerInput>();
        PlayerAnimator = GetComponent<Animator>();
        Controller = GetComponent<CharacterController>();
        GameStateManager.Get_Instance.OnGameStateChanged += OnGameStateChanged;
    }

    void Start()
    {
        IsWalkingHash = Animator.StringToHash("IsWalking");
        JumpTriggerHash = Animator.StringToHash("JumpTrigger");
        IsLandingHash = Animator.StringToHash("IsLanding");
        IsGroundedHash = Animator.StringToHash("IsGrounded");
        VelocityHash = Animator.StringToHash("Velocity");
        IsAttackHash = Animator.StringToHash("IsAttack");

        AttackTriggerHash = Animator.StringToHash("AttackTrigger");
    }

    void Update()
    {
        IsGrounded = Physics.CheckSphere(gameObject.transform.position, 0.2f, GroundLayer) && !IsJump;
        //Set to Landing - Going Down + Not Grounded
        if (Controller.velocity.y < -0.0001f && !IsGrounded) 
        {
            IsJump = false;
            IsLanding = true;
        }
        //Set to Grounded
        else if (IsGrounded)
        {
            IsLanding = false;
            IsGrounded = true;
        }

        if(AttackInputTimer >= AttackDuration)
            IsAttack = false;
        else
            AttackInputTimer += Time.deltaTime;



        PlayerAnimator.SetBool(IsWalkingHash, IsMoving);
        PlayerAnimator.SetFloat(VelocityHash, Velocity * 0.1f);
        PlayerAnimator.SetBool(IsLandingHash, IsLanding);
        PlayerAnimator.SetBool(IsGroundedHash, IsGrounded);
        PlayerAnimator.SetBool(IsAttackHash, IsAttack);
    }

    private void FixedUpdate()
    {
        if (IsMoving)
        {
            Vector3 cameraForward = InputScript.camera.transform.forward;
            cameraForward = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;

            //Apply Camera Direction
            Rotation = Quaternion.LookRotation(cameraForward);

            //Apply Movement Direction based on Camera Direction
            Rotation = Rotation * Quaternion.LookRotation(MoveDirection);
        }
        else
        {
            Velocity = Mathf.Lerp(Velocity, 0, SlowSpeed * Time.fixedDeltaTime);
        }

        Controller.Move(Impact * Time.deltaTime);
        // consumes the impact energy each cycle:
        Impact += Gravity * Mass * Time.fixedDeltaTime;

        Impact = ClampValue(Impact, new Vector3(0, 0, 0), new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity));

        var ForwardVelocity = Rotation * Vector3.forward * Velocity;
        Controller.Move((ForwardVelocity + Gravity + Impact) * Time.fixedDeltaTime);

        transform.rotation = Quaternion.Slerp(transform.rotation, Rotation, TurnSpeed * Time.fixedDeltaTime);
    }
    #region InputAction
    public void OnMovement(InputAction.CallbackContext context)
    {
        var contextDirection = context.ReadValue<Vector2>();
        IsMoving = !context.canceled;
        MoveDirection = IsMoving ? WalkSpeed * new Vector3(contextDirection.x, 0, contextDirection.y) : Vector3.zero;

        if (IsMoving && !IsRunning)
        {
            Velocity = WalkSpeed;
        }
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        PlayerStats pStats = GetComponent<PlayerStats>();
        if (pStats && context.started)
        {
            if (pStats.GetChest())
            {
                pStats.GetChest().OnInteract();
                return;
            }
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (IsMoving)
        {
            IsRunning = !context.canceled;
            Velocity = !context.canceled ? RunSpeed : WalkSpeed;
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started && AttackInputTimer >= AttackInputGracePeriod) 
        {
            AttackInputTimer = 0;
            PlayerAnimator.SetTrigger(AttackTriggerHash);
            IsAttack = true;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && !IsJump && IsGrounded)
        {
            PlayerAnimator.SetTrigger(JumpTriggerHash);
            AddImpact(new Vector3(0, 1, 0), JumpForce);

            IsJump = true;
            IsGrounded = false;
        }
    }
    #endregion

    void OnEnable()
    {
        InputScript.ActivateInput();
    }

    void OnDisable()
    {
        InputScript.DeactivateInput();
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }

    private void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();
        if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
        Impact += dir.normalized * force / Mass;
    }

    private static Vector3 ClampValue (Vector3 target, Vector3 min, Vector3 max)
    {
        if (target.x <= min.x)
            target.x = min.x;

        if (target.y <= min.y)
            target.y = min.y;

        if (target.z <= min.z)
            target.z = min.z;

        if (target.x > max.x)
            target.x = max.x;
        
        if (target.y > max.y)
            target.y = max.y;
        
        if (target.z > max.z)
            target.z = max.z;

        return target;
    }
}
