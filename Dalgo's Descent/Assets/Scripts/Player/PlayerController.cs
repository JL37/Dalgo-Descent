using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using UnityEngine.InputSystem;
using System.Threading.Tasks;
public class PlayerController : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private Vector3 Gravity = new Vector3 (0,-10.0f,0);
    [SerializeField][Min(1)] private float Mass;
    [Range(0, 5)] [SerializeField] private float SlowSpeed;
    [Range(0, 10)] [SerializeField] private float TurnSpeed;

    [Header("Jump Modifier")]
    [Range(5, 20)][SerializeField] private float JumpForce;
    [Range(0, 3)][SerializeField] private float JumpDelay;

    [Header("Walk/Run Modifier")]
    [Range(0, 10)][SerializeField] private float RunSpeed;
    [Range(0, 10)][SerializeField] private float WalkSpeed;
    private float TargetSpeed;
    [Header("Attack Modifier")]
    [Range(0, 1)][SerializeField] private double AttackCoolDown; //Start Recording after this period
    [Range(0, 1)][SerializeField] private double AttackInputDelay; //Start Recording after this period
    [SerializeField] private List<AnimationClip> Combos;

    public int CurrentCombo;
    public int CurrentSlash;
    public double AttackCDTimer = 0;
    public double AttackInputTimer = 0;
    public bool IsAttacking;
    public bool AttackTriggered;
    public bool HasAttackInput;

    private Vector3 Impact;

    [Header("Variables")]
    [SerializeField] private LayerMask GroundLayer;
    public CharacterController Controller;
    public Weapon PlayerWeapon;
    public Animator PlayerAnimator;
    public PlayerInput InputScript;
    public List<SlashScript> SlashVFXPrefabs;
    public Vector3 MoveDirection { get; private set; }
    public Quaternion Rotation { get; private set; }
    public float Velocity { get; private set; }
    public bool IsMoving { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsJump { get; private set; }
    public bool IsLanding { get; private set; }
    public bool IsGrounded { get; private set; }

    //Animator Parameter Hashes
    private int IsWalkingHash;
    private int JumpTriggerHash;
    private int IsLandingHash;
    private int IsGroundedHash;
    private int VelocityHash;
    private int IsAttackHash;
    private int AttackTriggerHash;

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

        if(AttackCDTimer > 0)
            AttackCDTimer -= Time.deltaTime;

        if(IsAttacking)
        {
            PlayerWeapon.gameObject.SetActive(true);
            AttackInputTimer += Time.deltaTime;

            if (CurrentCombo <= Combos.Count)
            {
                if (AttackInputTimer >= AttackInputDelay) // Wait 0.1s after Start of Attack
                {
                    if (AttackInputTimer < Combos[CurrentCombo - 1].length)
                    {
                        if (HasAttackInput && !AttackTriggered)
                        {
                            HasAttackInput = false;
                            if(CurrentCombo != Combos.Count)
                            {
                                AttackTriggered = true;
                                PlayerAnimator.SetTrigger(AttackTriggerHash);
                            }
                        }
                    }
                    else
                    {
                        AttackInputTimer = 0;
                        if(AttackTriggered)
                        {
                            CurrentCombo++;
                            AttackTriggered = false;
                        }
                        else
                        {
                            CurrentCombo = 0;
                            CurrentSlash = 0;
                            IsAttacking = false;
                            AttackCDTimer = AttackCoolDown;
                        }
                    }
                }
            }
            else
            {
                IsAttacking = false;
                AttackTriggered = false;
            }
        }
        else
        {
            if(HasAttackInput && AttackCDTimer <= 0 && !AttackTriggered)
            {
                HasAttackInput = false;
                IsAttacking = true;
                CurrentCombo++;
                //AttackTriggered = true;
                PlayerAnimator.SetTrigger(AttackTriggerHash);
            }
            else
            {
                AttackTriggered = false;
                CurrentCombo = 0;
                CurrentSlash = 0;
            }
            PlayerWeapon.gameObject.SetActive(false);
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
