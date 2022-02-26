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
    

    [Header("Idle Modifier")]
    [SerializeField] [Range(0, 10)] private float IdleInterval;

    [Header("Jump Modifier")]
    [SerializeField] [Range(5, 20)] private float JumpForce;
    [SerializeField] [Range(0, 3)] private float JumpDelay;

    [Header("Walk/Run Modifier")]
    [SerializeField] [Range(0, 10)] private float RunSpeed;
    [SerializeField] [Range(0, 10)] private float WalkSpeed;
    [SerializeField] [Range(0, 10)] private float DashImpact;
    [SerializeField] [Range(0, 1)] private float SlideFriction;
    [SerializeField] [Range(0, 1)] private float IFrameDuration;
  
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
    public bool IsLanding { get; private set; }
    public bool IsGrounded { get; private set; }
    public bool IsIFrame { get; private set; }

    private Vector3 Impact;
    private Vector3 SlopeNormal;
    private float TargetSpeed;
    private float SlopeAngle;
    private double IdleTimer;
    private double IFrameTimer;
    private int AnimationLayerIndex;
    private bool HasJumpInput;
    //Animator Parameter Hashes
    public int VelocityHash { get; private set; }
    public int IdleTriggerHash { get; private set; }
    public int JumpTriggerHash { get; private set; }
    public int SprintTriggerHash { get; private set; }
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
        AnimationLayerIndex = PlayerAnimator.GetLayerIndex("Base Layer");

    }

    private void OnDestroy()
    {
        GameStateManager.Get_Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    void Update()
    {
        IsGrounded = Controller.isGrounded && (SlopeAngle < Controller.slopeLimit) && (Impact.y <= -Gravity.y);

        if (SlopeAngle > Controller.slopeLimit)
        {
            IsMoving = false;
            MoveDirection = Vector3.zero;
        }
        if (Controller.velocity.y < -0.0001f && !IsGrounded)  //Set to Landing - Going Down + Not Grounded
            IsLanding = true;
        else
            IsLanding = false;

        ProcessTimers();

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
        PlayerAnimator.SetFloat(VelocityHash, Velocity * 0.14f);
        PlayerAnimator.SetBool(IsLandingHash, IsLanding);
        PlayerAnimator.SetBool(IsGroundedHash, IsGrounded);
    }
    private void FixedUpdate()
    {
        if (IsMoving && !(GetComponent<PlayerSkillsManager>().ActiveSkillIndex >= 0 || GetComponent<PlayerAttackManager>().IsAttacking))
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

        var v3Velocity = Rotation * Vector3.forward * Velocity;
        v3Velocity += Gravity;

        Impact += Gravity * Mass * Time.fixedDeltaTime;
        Impact.x -= Mathf.Lerp(Impact.x, 0, SlowSpeed * Time.fixedDeltaTime);
        Impact.z -= Mathf.Lerp(Impact.z, 0, SlowSpeed * Time.fixedDeltaTime);

        if (!IsGrounded)
        {
            v3Velocity.x += ((1f - SlopeNormal.y) * SlopeNormal.x * (1f - SlideFriction));
            v3Velocity.z += ((1f - SlopeNormal.y) * SlopeNormal.z * (1f - SlideFriction));
        }
        else
        {
            Impact.y = 0;
        }
        
        Controller.Move((v3Velocity + Impact) * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Rotation, TurnSpeed * Time.fixedDeltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        SlopeNormal = hit.normal;
        SlopeAngle = Vector3.Angle(hit.normal, Vector3.up);
    }

    void ProcessTimers()
    {
        if (IdleTimer >= IdleInterval)
        {
            if (!IsMoving && IsGrounded && !GetComponent<PlayerAttackManager>().IsAttacking)
                PlayerAnimator.SetTrigger(IdleTriggerHash);
            IdleTimer = 0;
        }
        else IdleTimer += Time.deltaTime;

        if (IFrameTimer >= IFrameDuration) IsIFrame = true;
        else IFrameTimer += Time.deltaTime;
    }

    public void ForceSetOnGround()
    {
        IsLanding = false;
        IsGrounded = true;
    }

    public Vector3 GetGravity()
    {
        return Gravity;
    }

    public void SetGravity(Vector3 Grav)
    {
        Gravity = Grav;
    }

    #region InputAction
    public void OnMovement(InputAction.CallbackContext context)
    {
        
        if (GetComponent<PlayerStats>().m_Health.currentHealth <= 0)
        {
            IsMoving = false;
            MoveDirection = Vector3.zero;
            return;
        }
        else
        {
            IsMoving = !context.canceled;
            var contextDirection = context.ReadValue<Vector2>();
            MoveDirection = (IsMoving || (SlopeAngle > Controller.slopeLimit)) ? WalkSpeed * new Vector3(contextDirection.x, 0, contextDirection.y) : Vector3.zero;
        }


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
            else
            {
                print("NO CHEST LA");
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
            {
                GetComponent<PlayerAttackManager>().ResetState(false);
                PlayerAnimator.Play("Movement Tree", AnimationLayerIndex);
                AddImpact(transform.rotation * Vector3.forward, DashImpact);
            }
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && Impact.y <= 0 && IsGrounded && GetComponent<PlayerStats>().m_Health.currentHealth > 0 && !HasJumpInput)
        {
            HasJumpInput = true;
            PlayerAnimator.SetTrigger(JumpTriggerHash);
            Jump();
        }
    }
    #endregion
    private async void Jump()
    {
        await Task.Delay((int)(JumpDelay * 1000));

        if (MoveDirection.sqrMagnitude > 0)
        {
            Vector3 cameraForward = InputScript.camera.transform.forward;
            cameraForward = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;

            Rotation = Quaternion.LookRotation(cameraForward);
            Rotation = Rotation * Quaternion.LookRotation(MoveDirection);
            transform.rotation = Rotation;

            AddImpact(Rotation * Vector3.forward, JumpForce * 0.2f);
        }
        AddImpact(Vector3.up, JumpForce);
        IsGrounded = false;
        HasJumpInput = false;
    }

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

    public void ResetImpactForJump(bool resetAllAxes = true) //Resetting for additional jump (3RD SKILL)
    {
        PlayerAnimator.SetTrigger(JumpTriggerHash);
        IsGrounded = false;
        Impact.y = 0;

        Impact = resetAllAxes ? new Vector3(0, 0, 0) : Impact;
    }

}
