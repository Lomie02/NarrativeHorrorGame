using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("General")]
    Rigidbody m_PlayerMovement = null;

    [SerializeField] float m_WalkSpeed = 2f;
    [SerializeField] float m_SprintSpeed = 5f;

    [Space]

    [SerializeField] float m_JumpMultiplier = 5;

    float m_MovementSpeed;
    PlayerCamera m_PlayerCamera = null;
    bool m_CrouchState = false;
    bool m_IsGrounded = false;

    void Start()
    {
        m_PlayerCamera = FindObjectOfType<PlayerCamera>();
        m_PlayerMovement = GetComponent<Rigidbody>();

        m_PlayerCamera.CursorState(false);
        m_PlayerMovement.freezeRotation = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && m_IsGrounded)
        {
            CycleCrouch();
            m_PlayerCamera.SetCrouchState(m_CrouchState);
        }

        if (Input.GetKeyDown(KeyCode.Space) && m_IsGrounded && !m_CrouchState)
        {
            m_PlayerMovement.AddForce(transform.up * m_JumpMultiplier, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        if (m_PlayerCamera.GetPlayerState())
        {
            float x = Input.GetAxisRaw("Horizontal") * Time.deltaTime;
            float z = Input.GetAxisRaw("Vertical") * Time.deltaTime;

            Vector3 MoveV = transform.right * x + transform.forward * z;

            if (Physics.Raycast(transform.position, -transform.up, 1.5f))
            {
                m_IsGrounded = true;
            }
            else
            {
                m_IsGrounded = false;
            }


            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && !m_CrouchState && m_IsGrounded)
            {
                m_MovementSpeed = m_SprintSpeed;
            }
            else
            {
                m_MovementSpeed = m_WalkSpeed;
            }





            m_PlayerMovement.MovePosition(transform.position + MoveV.normalized * m_MovementSpeed * Time.fixedDeltaTime);
        }
    }

    public void CycleCrouch()
    {
        if (m_CrouchState)
        {
            m_CrouchState = false;
        }
        else
        {
            m_CrouchState = true;
        }
    }
}
