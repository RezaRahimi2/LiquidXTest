using MonoBehaviours.Player;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Animator m_animator;
    [SerializeField] private float lerpTime = 2f;
    private float currentLerpTime = 0f;
    
    [SerializeField] private float m_currentSpeed;
    [SerializeField] private float m_normalSpeed = 5.0f; // The normalSpeed at which the player moves
    [SerializeField] private float m_sprintSpeed = 15.0f; // The normalSpeed at which the player moves
    [SerializeField] private float m_rotationSpeed;
    [SerializeField] private Transform m_cameraTransform;
    [SerializeField] private SoundEmitter m_soundEmitter;
    [SerializeField] private Quaternion lookRotation;
   
    private Rigidbody m_rigidBody;
    public Vector3 Velocity { get; private set; }
    void Start()
    {
        m_cameraTransform = Camera.main.transform;
        m_rigidBody = GetComponent<Rigidbody>();
        m_soundEmitter.Initialize(this);
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
        Velocity = inputDirection * m_currentSpeed;
        
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0; // keep only the horizontal direction
        cameraForward = cameraForward.normalized; // normalize the vector

        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0; // keep only the horizontal direction
        cameraRight = cameraRight.normalized; // normalize the vector

        Vector3 moveDirection = cameraForward * inputDirection.z + cameraRight * inputDirection.x;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime) {
                currentLerpTime = 0; // Reset the lerp time
            }

            // calculate the percentage of the lerp time
            float perc = currentLerpTime / lerpTime;

            m_currentSpeed = Mathf.Lerp(m_currentSpeed, m_sprintSpeed, perc);
        }
        else
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime) {
                currentLerpTime = 0; // Reset the lerp time
            }

            // calculate the percentage of the lerp time
            float perc = currentLerpTime / lerpTime;

            m_currentSpeed = Mathf.Lerp(m_currentSpeed, m_normalSpeed, perc);
        }
        
        m_rigidBody.velocity = moveDirection * m_currentSpeed;


        if (moveDirection != Vector3.zero) // if the input is not zero
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            m_rigidBody.rotation = Quaternion.RotateTowards(m_rigidBody.rotation, toRotation, m_rotationSpeed * Time.deltaTime);
        }
        
        m_animator.SetFloat("Speed",Velocity.magnitude);
        OnMovementEvent.Invoke(Velocity.magnitude,m_currentSpeed);
    }

}