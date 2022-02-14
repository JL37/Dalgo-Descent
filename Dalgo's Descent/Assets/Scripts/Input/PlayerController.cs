using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [Range(0, 1)][SerializeField] private float runSpeed;
    [Range(0, 1)] [SerializeField] private float walkSpeed;
    [Range(0, 1)][SerializeField] private float turnSpeed;

    public Rigidbody PlayerRigidBody;
    public Animator PlayerAnimator;
    public PlayerInput InputScript;
    public Vector3 MoveDirection { get; private set; }
    public Quaternion Rotation { get; private set; }
    public bool IsMoving { get; private set; }

    private int IsWalkingHash;
    void Awake()
    {
        InputScript = GetComponent<PlayerInput>();
        PlayerRigidBody = GetComponent<Rigidbody>();
        PlayerAnimator = GetComponent<Animator>();
    }

    void Start()
    {
        IsWalkingHash = Animator.StringToHash("IsWalking");
    }

    void Update()
    {
        PlayerAnimator.SetBool(IsWalkingHash, IsMoving);

        if (IsMoving)
        {
            Vector3 cameraForward = InputScript.camera.transform.forward;
            cameraForward = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;

            //Apply Camera Direction
            Rotation = Quaternion.LookRotation(cameraForward);

            //Apply Movement Direction based on Camera Direction
            Rotation = Rotation * Quaternion.LookRotation(MoveDirection);
        }
    }

    void OnAnimatorMove()
    {

        PlayerRigidBody.MovePosition(PlayerRigidBody.position + Rotation * Vector3.forward * PlayerAnimator.deltaPosition.magnitude);
        PlayerRigidBody.MoveRotation(Quaternion.Slerp(PlayerRigidBody.rotation, Rotation, turnSpeed));
    }

    #region InputAction
    public void OnMovement(InputAction.CallbackContext context)
    {
        Debug.Log(context.action);
        var contextDirection = context.ReadValue<Vector2>();
        IsMoving = !context.canceled;
        MoveDirection = IsMoving ? walkSpeed * new Vector3(contextDirection.x, 0, contextDirection.y) : Vector3.zero;
    }

    public void Fire(InputAction.CallbackContext context)
    {
        //Debug.Log(context.control + " " + context.phase);
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
}
