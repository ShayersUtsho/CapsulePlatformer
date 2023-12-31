using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private LayerMask playerMask;
    private bool jumpKeyWasPressed = false;
    private float horizontalInput;
    private Rigidbody rigidbodyCompoent;
    private bool isGrounded;
    private bool superJumpAvailable = false;
    private bool airJumpAvailable = false;
    private float jumpSpeed = 3.0f;
    [SerializeField] private float standardJumpSpeed = 3.0f;
    [SerializeField] private float superJumpSpeed = 8.0f;
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private Transform cameraPosition;

    // Awake is called before Start
    void Awake()
    {
        rigidbodyCompoent = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpKeyWasPressed = true;
        }

        horizontalInput = Input.GetAxis("Horizontal");
    }

    // FixedUpdate is called everytime a Physics Update occurs
    private void FixedUpdate()
    {
        rigidbodyCompoent.velocity = new Vector3(horizontalInput * moveSpeed, rigidbodyCompoent.velocity.y, 0);
        cameraPosition.position = transform.position + new Vector3(0f, 0f, -10f);

        if (Physics.OverlapSphere(groundCheckTransform.position, 0.1f, playerMask).Length == 0 && !airJumpAvailable)
        {
            isGrounded = true;
            return;
        }
        else
        {
            isGrounded = false;
        }

        if (jumpKeyWasPressed)
        {
            if (superJumpAvailable) {
                jumpSpeed = superJumpSpeed;
                superJumpAvailable = false;
            }
            else
            {
                jumpSpeed = standardJumpSpeed;
            }
            if(airJumpAvailable && !isGrounded)
            {
                airJumpAvailable = false;
            }
            rigidbodyCompoent.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            jumpKeyWasPressed = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            superJumpAvailable = true;
            Destroy(other.gameObject);
        }
        if (other.gameObject.layer == 8)
        {
            airJumpAvailable = true;
            Destroy(other.gameObject);
        }
    }
}
