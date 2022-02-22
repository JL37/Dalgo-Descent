using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using UnityEngine.InputSystem;
using System.Threading.Tasks;
public class PlayerController : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private Vector3 Gravity = new Vector3(0, -10.0f, 0);
    [SerializeField] [Min(1)] private float Mass;
    [SerializeField] [Range(0, 5)] private float SlowSpeed;
    [SerializeField] [Range(0, 10)] private float TurnSpeed;

    [Header("IdleModifier")]
    [SerializeField] [Range(0, 10)] private float IdleInterval;

    [Header("Jump Modifier")]
    [SerializeField] [Range(5, 20)] private float JumpForce;
    [SerializeField] [Range(0, 3)] private float JumpDelay;

    [Header("Walk/Run Modifier")]
    [SerializeField] [Range(0, 10)] private float RunSpeed;
    [SerializeField] [Range(0, 10)] private float WalkSpeed;

    [Header("Variables")]
    [SerializeField] private Weapon OnHandWeapon;
    [SerializeField] private Weapon BackWeapon;
    [SerializeField] private LayerMask GroundLayer;
    [SerializeField] private CharacterController Controller;
    [SerializeField] private Animator PlayerAnimator;
    [SerializeField] private PlayerInput InputScript;
    public Vector3 MoveDirection { get; private set; }
    public Quaternion Rotation { get; private set; }
    public float Velocity { get; private set; }
    public bool IsMoving { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsJump { get; private set; }
    public bool IsLanding { get; private set; }
    public bool IsGrounded { get; private set; }

    private float TargetSpeed;
    private Vector3 Impact;
    private double IdleTimer;

    //Animator Parameter Hashes
    public int VelocityHash { get; private set; }
    public int IdleTriggerHash { get; private set; }
    public int JumpTriggerHash { get; private set; }
    public int IsWalkingHash { get; private set; }
    public int IsLandingHash { get; private set; }
    public int IsGroundedHash { get; private set; }

    void Awake()
    {
        InputScript = GetComponent<PlayerInput>();
        if(InputScript.camera == null) InputScript.camera = Camera.main;
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
        IdleTriggerHash = Animator.StringToHash("IdleTrigger");
    }

    void Update()
    {
        IdleTimer += Time.deltaTime;

        if (IdleTimer >= IdleInterval)
        {
            if(!IsMoving && IsGrounded && !GetComponent<PlayerAttackManager>().IsAttacking)
                PlayerAnimator.SetTrigger(IdleTriggerHash);
            IdleTimer = 0;
        }

        IsGrounded = Physics.CheckSphere(gameObject.transform.position, 0.2f, GroundLayer) && !IsJump;
        if (Controller.velocity.y < -0.0001f && !IsGrounded)  //Set to Landing - Going Down + Not Grounded
        {
            IsJump = false;
            IsLanding = true;
        }
        else if (IsGrounded) //Set to Grounded
        {
            IsLanding = false;
            IsGrounded = true;
        }

        if (GetComponent<PlayerSkillsManager>().ActiveSkillIndex >= 0 || GetComponent<PlayerAttackManager>().IsAttacking)
        {
            OnHandWeapon.GetComponent<WeaponVisibility>().SetVisible(true);
            BackWeapon.GetComponent<WeaponVisibility>().SetVisible(false);
        }
        else
        {
            OnHandWeapon.GetComponent<WeaponVisibility>().SetVisible(false);
            BackWeapon.GetComponent<WeaponVisibility>().SetVisible(true);
        }
        
        PlayerAnimator.SetBool(IsWalkingHash, IsMoving);
        PlayerAnimator.SetFloat(VelocityHash, Velocity * 0.1f);
        PlayerAnimator.SetBool(IsLandingHash, IsLanding);
        PlayerAnimator.SetBool(IsGroundedHash, IsGrounded);
        PlayerAnimator.SetBool(IsAttackHash, IsAttacking);
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

            Velocity = Mathf.MoveTowards(Velocity, TargetSpeed, 30 * Time.fixedDeltaTime);
        }
        else
        {
            Velocity = Mathf.Lerp(Velocity, 0, SlowSpeed * Time.fixedDeltaTime);
        }


        if (!IsAttacking)
        {

            Impact += Gravity * Mass * Time.fixedDeltaTime;
            Impact.x = Mathf.Lerp(Impact.x, 0, SlowSpeed * Time.fixedDeltaTime);
            Impact.z = Mathf.Lerp(Impact.z, 0, SlowSpeed * Time.fixedDeltaTime);
            Impact = ClampValue(Impact, new Vector3(0, 0, 0), new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity));

            var ForwardVelocity = Rotation * Vector3.forward * Velocity;
            Controller.Move((ForwardVelocity + Gravity + Impact) * Time.fixedDeltaTime);

            transform.rotation = Quaternion.Slerp(transform.rotation, Rotation, TurnSpeed * Time.fixedDeltaTime);
        }
    }

    public void Slash() // Called by Animation
    {
        Instantiate(SlashVFXPrefabs[CurrentSlash], transform);
        CurrentSlash++;
    }

    #region InputAction
    public void OnMovement(InputAction.CallbackContext context)
    {
        var contextDirection = context.ReadValue<Vector2>();
        IsMoving = !context.canceled;
        MoveDirection = IsMoving ? WalkSpeed * new Vector3(contextDirection.x, 0, contextDirection.y) : Vector3.zero;

        if (IsMoving && !IsRunning)
        {
            TargetSpeed = WalkSpeed;
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
        TargetSpeed = !context.canceled ? RunSpeed : WalkSpeed;
        if (IsMoving)
        {
            IsRunning = !context.canceled;

            if (context.started)
                AddImpact(transform.rotation * Vector3.forward, 10);
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started && AttackCDTimer <= 0)
            HasAttackInput = true;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && !IsJump && IsGrounded)
        {
            PlayerAnimator.SetTrigger(JumpTriggerHash);
            Jump();

            IsJump = true;
            IsGrounded = false;
        }
    }
    private async void Jump()
    {
        await Task.Delay((int)(JumpDelay * 1000));
        AddImpact(new Vector3(0, 1, 0), JumpForce);

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

    public void AddImpact(Vector3 dir, float force)
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
