using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using TMPro;
using System;
using UnityEditor.Animations;

public class Movement : MonoBehaviour
{
    #region Varibles

    //Movement Varibles
    [SerializeField]
    private float NormalMovementSpeed;

    [SerializeField]
    private float SprintMovementSpeed;

    [SerializeField]
    private float CrouchMovementSpeed;

    [Space]

    [SerializeField]
    private float JumpStrenght;

    //Rotation
    private float XRotation;
    private float YRotation;

    //Movement
    private float ForwardMove;
    private float SideMove;
    private float Jump;

    [Space]

    //Staminas
    private float Stamina;

    [SerializeField]
    private float MaxStamina;

    [SerializeField]
    private float StaminaUsage;

    [SerializeField]
    private float StaminaDelay;

    [Space]

    //Other Varibles
    public Camera PlayerCamera;
    public CharacterController PlayerCC;
    public Collider ParkourTrigger;
    public GameObject Body;

    [Space]

    public Slider StaminaSlider;
    public TMP_Text StaminaText;

    [Space]

    //Crouching
    public float CrouchHeight;
    private float NormalHeight;

    [Space]

    public Animator FasolekAnimator;

    #endregion


    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Stamina = MaxStamina;

        StaminaSlider.maxValue = MaxStamina;

        NormalHeight = PlayerCC.height;
    }

    void Update()
    {
        //Vertical Rotation
        XRotation -= Input.GetAxis("Mouse Y");
        XRotation = Mathf.Clamp(XRotation, -75, 75);

        //Horizontal Rotation
        YRotation = Input.GetAxis("Mouse X");

        //Rotate
        this.transform.Rotate(0, YRotation, 0);
        PlayerCamera.transform.localRotation = Quaternion.Euler(XRotation, 0, 0);

        ForwardMove = Input.GetAxis("Vertical") * NormalMovementSpeed;
        SideMove = Input.GetAxis("Horizontal") * NormalMovementSpeed;

        //Sprint
        if (Input.GetKey(KeyCode.LeftShift) && Stamina > 0 && ForwardMove > 0)
        {
            ForwardMove = Input.GetAxis("Vertical") * SprintMovementSpeed;
            SideMove = Input.GetAxis("Horizontal") * SprintMovementSpeed;

            Stamina -= StaminaUsage * Time.deltaTime;

            StaminaDelay = 2;
            FasolekAnimator.SetBool("Run", true);
            Debug.Log("Sprint true");
        }
        else
        { 
        FasolekAnimator.SetBool("Run", false);
            Debug.Log("Sprint false");
        }
        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && PlayerCC.isGrounded && Stamina > MaxStamina * 0.25)
        {
            Jump = JumpStrenght;
            Stamina -= 35;
            StaminaDelay = 2;
        }

        //Crouch
        if (Input.GetKey(KeyCode.LeftControl))
        {
            ForwardMove = Input.GetAxis("Vertical") * CrouchMovementSpeed;
            SideMove = Input.GetAxis("Horizontal") * CrouchMovementSpeed;

            FasolekAnimator.SetBool("Crouch", true);
        }
        else
        {
            FasolekAnimator.SetBool("Crouch", false);
        }

        Jump += Physics.gravity.y * Time.deltaTime;

        Vector3 MoveVector = new Vector3(SideMove, Jump, ForwardMove);

        PlayerCC.Move(this.transform.rotation * MoveVector * Time.deltaTime);

        if (Input.GetKey(KeyCode.W))
        {
            FasolekAnimator.SetBool("Walk", true);
            //Console.WriteLine("true");
        }
        else
        {
            FasolekAnimator.SetBool("Walk", false);
        }

        if (Stamina < MaxStamina && !Input.GetKey(KeyCode.LeftShift) && StaminaDelay <= 0)
            Stamina += StaminaUsage * 0.6f * Time.deltaTime;

        //Asign UI Values
        StaminaSlider.value = Stamina;
        StaminaText.text = Convert.ToInt16((Stamina / MaxStamina) * 100) + " %";
        StaminaDelay -= Time.deltaTime;

    }
}
