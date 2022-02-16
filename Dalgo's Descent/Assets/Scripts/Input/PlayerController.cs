using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Modifer")]
    [Range(0, 1)][SerializeField] private float runSpeed;
    [Range(0, 1)][SerializeField] private float slowSpeed;
    [Range(0, 1)][SerializeField] private float walkSpeed;
    [Range(0, 1)][SerializeField] private float turnSpeed;
    [SerializeField] private float jumpForce;

    [Header("Variables")]
    [SerializeField] private LayerMask GroundLayer;
    public Rigidbody PlayerRigidBody;
    public Animator PlayerAnimator;
    public PlayerInput InputScript;

    [Header("Skill 1")]
    public Image skill_image1;
    public float cooldown1 = 5.0f;
    public bool isCooldown = false;
    public bool isSkill1Pressed = false;

    [Header("Skill 2")]
    public Image skill_image2;
    public float cooldown2 = 5.0f;
    public bool isCooldown2 = false;
    public bool isSkill2Pressed = false;

    [Header("Skill 3")]
    public Image skill_image3;
    public float cooldown3 = 5.0f;
    public bool isCooldown3 = false;
    public bool isSkill3Pressed = false;

    [Header("Skill 4")]
    public Image skill_image4;
    public float cooldown4 = 5.0f;
    public bool isCooldown4 = false;
    public bool isSkill4Pressed = false;

    public Vector3 MoveDirection { get; private set; }
    public Quaternion Rotation { get; private set; }
    public float Velocity { get; private set; }
    public bool IsMoving { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsJump { get; private set; }
    public bool IsLanding { get; private set; }
    public bool IsGrounded { get; private set; }

    private int IsWalkingHash;
    private int JumpTriggerHash;
    private int IsLandingHash;
    private int IsGroundedHash;
    private int VelocityHash;
    void Awake()
    {
        InputScript = GetComponent<PlayerInput>();
        PlayerRigidBody = GetComponent<Rigidbody>();
        PlayerAnimator = GetComponent<Animator>();
        GameStateManager.Get_Instance.OnGameStateChanged += OnGameStateChanged;
    }

    void Start()
    {
        IsWalkingHash = Animator.StringToHash("IsWalking");
        JumpTriggerHash = Animator.StringToHash("JumpTrigger");
        IsLandingHash = Animator.StringToHash("IsLanding");
        IsGroundedHash = Animator.StringToHash("IsGrounded");
        VelocityHash = Animator.StringToHash("Velocity");
        skill_image1.fillAmount = 0.0f;
        skill_image2.fillAmount = 0.0f;
        skill_image3.fillAmount = 0.0f;
        skill_image4.fillAmount = 0.0f;
    }

    void Update()
    {
        Render_Skill1_Cooldown();
        Render_Skill2_Cooldown();
        Render_Skill3_Cooldown();
        Render_Skill4_Cooldown();

        IsGrounded = Physics.CheckSphere(gameObject.transform.position, 0.2f, GroundLayer) && !IsJump;
        //Set to Landing - Going Down + Not Grounded
        if (PlayerRigidBody.velocity.y < -0.0001f && !IsGrounded) 
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
            Velocity = Mathf.Lerp(Velocity, 0, slowSpeed);
        }

        PlayerAnimator.SetBool(IsWalkingHash, IsMoving);
        PlayerAnimator.SetFloat(VelocityHash, Velocity);
        PlayerAnimator.SetBool(IsLandingHash, IsLanding);
        PlayerAnimator.SetBool(IsGroundedHash, IsGrounded);
    }

    /*    void OnDestroy()
        {
            GameStateManager.Get_Instance.OnGameStateChanged -= OnGameStateChanged;


        }*/

    void OnAnimatorMove()
    {
        float animationDelta = PlayerAnimator.deltaPosition.magnitude;

        if(IsGrounded)
            PlayerRigidBody.MovePosition(PlayerRigidBody.position + Rotation * Vector3.forward * animationDelta);
        else
            PlayerRigidBody.MovePosition(PlayerRigidBody.position + Rotation * Vector3.forward * Velocity * Time.fixedDeltaTime);

        PlayerRigidBody.MoveRotation(Quaternion.Slerp(PlayerRigidBody.rotation, Rotation, turnSpeed));
    }

    #region InputAction
    public void OnMovement(InputAction.CallbackContext context)
    {
        var contextDirection = context.ReadValue<Vector2>();
        IsMoving = !context.canceled;
        MoveDirection = IsMoving ? walkSpeed * new Vector3(contextDirection.x, 0, contextDirection.y) : Vector3.zero;

        if (IsMoving && !IsRunning)
        {
            Velocity = walkSpeed;
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
            Velocity = !context.canceled ? runSpeed : walkSpeed;
        }
    }

    public void Fire(InputAction.CallbackContext context)
    {
        //Debug.Log(context.control + " " + context.phase);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && !IsJump && IsGrounded)
        {
            PlayerAnimator.SetTrigger(JumpTriggerHash);
            PlayerRigidBody.AddForce(Rotation * Vector3.forward * jumpForce * Velocity, ForceMode.Impulse);
            PlayerRigidBody.AddRelativeForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);

            IsJump = true;
            IsGrounded = false;
        }
    }

    public void OnSkill1Used(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            Debug.Log("Q press");
            isSkill1Pressed = true;
        }
       
    }

    public void OnSkill2Used(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("E press");
            isSkill2Pressed = true;
        }
    }

    public void OnSkill3Used(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("R press");
            isSkill3Pressed = true;
        }
    }

    public void OnSkill4Used(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("T press");
            isSkill4Pressed = true;
}
    }
    #endregion

    void Render_Skill1_Cooldown()
    {
        if(isSkill1Pressed && !isCooldown) //if skill 1 key is pressed and cooldown is 0
        {
            isCooldown = true; //reset these var
            isSkill1Pressed = false;
            skill_image1.fillAmount = 1.0f; //display the image that has cooldown
        }

        if(isCooldown) //while the skill is on cooldown
        {
            skill_image1.fillAmount -= 1 / cooldown1 * Time.deltaTime; //reduce the fill amount by 1 with respective to delta time

            if(skill_image1.fillAmount <= 0) //if fill amount is lesser or equal to 0, make it to 0 and reset the cooldown to false
            {
                skill_image1.fillAmount = 0;
                isCooldown = false;
            }
        }
    }

    void Render_Skill2_Cooldown()
    {
        if (isSkill2Pressed && !isCooldown2)
        {
            isCooldown2 = true;
            isSkill2Pressed = false;
            skill_image2.fillAmount = 1.0f;
        }

        if (isCooldown2)
        {
            skill_image2.fillAmount -= 1 / cooldown2 * Time.deltaTime;

            if (skill_image2.fillAmount <= 0)
            {
                skill_image2.fillAmount = 0;
                isCooldown2 = false;
            }
            
        }
    }

    void Render_Skill3_Cooldown()
    {
        if (isSkill3Pressed && !isCooldown3)
        {
            isCooldown3 = true;
            isSkill3Pressed = false;
            skill_image3.fillAmount = 1.0f;
        }

        if (isCooldown3)
        {
            skill_image3.fillAmount -= 1 / cooldown3 * Time.deltaTime;

            if (skill_image3.fillAmount <= 0)
            {
                skill_image3.fillAmount = 0;
                isCooldown3 = false;
            }
            
        }
    }

    void Render_Skill4_Cooldown()
    {
        if (isSkill4Pressed && !isCooldown4)
        {
            isCooldown4 = true;
            isSkill4Pressed = false;
            skill_image4.fillAmount = 1.0f;
        }

        if (isCooldown4)
        {
            skill_image4.fillAmount -= 1 / cooldown4 * Time.deltaTime;

            if (skill_image4.fillAmount <= 0)
            {
                skill_image4.fillAmount = 0;
                isCooldown4 = false;
            }
        }
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
}
