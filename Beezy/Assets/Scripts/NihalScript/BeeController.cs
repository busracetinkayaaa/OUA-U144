using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeeController : MonoBehaviour
{
    // vertical = w-s (forward)
    // horizontal = a-d (strafe)
    // hover = space-altctrl (hover)

    public float forwardSpeed = 7f, strafeSpeed = 7f, hoverSpeed = 5f;
    private float activeForwardSpeed, activeStrafeSpeed, activeHoverSpeed;
    private float forwardAcceleration = 0.3f, strateAcceleration = 0.5f, hoverAcceleration = 0.5f;

    private Animator animator;

    public float lookRateSpeed = 40f;
    private Vector2 lookInput, screenCenter, mouseDistance;


    // E�im 
    public float tiltAngle = 20f;
    public float turnSpeed = 3f;
    public float turnSmoothness = 5f;
    private Quaternion targetRotation;
    private Quaternion smoothRotation;

    // buttons
    public Button gButton; // guard
    public Button cButton; // capacity 
    public Button sButton; // speed
    public Button dButton; // damage

    public Button useGuard;
    public Button useSpeed;
    public Button useDamage; 

    public GameObject player;
    public GameObject shield;
    private GameObject shieldE;
    private bool isShieldActive = false;


    void Start()
    {
        player = GameObject.Find("FantasyBee");

        animator = GetComponent<Animator>();
        targetRotation = transform.rotation;
        smoothRotation = transform.rotation;

        screenCenter.x = Screen.width * .5f;
        screenCenter.y = Screen.height * .5f;

        gButton.onClick.AddListener(gOnClick);
        sButton.onClick.AddListener(sOnClick);
        dButton.onClick.AddListener(dOnClick);
        cButton.onClick.AddListener(cOnClick);

        useGuard.onClick.AddListener(guardOnClick);
        useSpeed.onClick.AddListener(speedOnClick);
    }

    private void guardOnClick()
    {
        StartCoroutine(shieldEff());
        useGuard.interactable = false;
    }

    private void speedOnClick()
    {
        StartCoroutine(IncreaseSpeed());
        useSpeed.interactable = false;
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Space)|| Input.GetKey(KeyCode.LeftControl) )
        {
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }


        if (Input.GetKey(KeyCode.Q))
        {
            animator.SetBool("IsAttack", true);
        }
        if (!Input.GetKey(KeyCode.Q))
        {
            animator.SetBool("IsAttack", false);
        }

        /*if (Input.GetKey(KeyCode.A))
        {
            // Sola d�nme
            targetRotation = Quaternion.Euler(0f, 0f, tiltAngle/3);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // Sa�a d�nme
            targetRotation = Quaternion.Euler(0f, 0f, -tiltAngle/3);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
        else
        {
            // D�nme yoksa
            targetRotation = Quaternion.identity;
        }
        smoothRotation = Quaternion.Lerp(smoothRotation, targetRotation, turnSmoothness * Time.deltaTime);
        transform.rotation = smoothRotation;
        */
        //
        /*lookInput.x = Input.mousePosition.x;
        //lookInput.y = Input.mousePosition.y;

        mouseDistance.x = (lookInput.x - screenCenter.x) / screenCenter.x;
        //mouseDistance.y = (lookInput.y - screenCenter.y) / screenCenter.y;

        mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);

        transform.Rotate(0f, mouseDistance.x * lookRateSpeed * Time.deltaTime, 0f, Space.Self);
        */
        Vector2 lookInput = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 mouseDistance = (lookInput - screenCenter) / screenCenter;

        if (Input.GetMouseButton(0))
        {
            mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);
            transform.Rotate(0f, mouseDistance.x * lookRateSpeed * Time.deltaTime, 0f, Space.Self);
        }
        activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, Input.GetAxisRaw("Vertical") * forwardSpeed, forwardAcceleration * Time.deltaTime);  
        activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed, Input.GetAxisRaw("Horizontal") * strafeSpeed, strateAcceleration * Time.deltaTime);
        activeHoverSpeed = Mathf.Lerp(activeHoverSpeed, Input.GetAxisRaw("Hover") * hoverSpeed, hoverAcceleration * Time.deltaTime);

        transform.position += transform.forward * activeForwardSpeed * Time.deltaTime;
        transform.position += (transform.right * activeStrafeSpeed * Time.deltaTime) + (transform.up * activeHoverSpeed * Time.deltaTime);

        }

    private void gOnClick()
    {
        gButton.interactable = false;
    }

    private void sOnClick()
    {
        sButton.interactable = false;

    }

    private void dOnClick()
    {
        dButton.interactable = false;
    }

    private void cOnClick()
    {
        cButton.interactable = false;
    }

    private IEnumerator IncreaseSpeed()
    {
        forwardSpeed *= 2f;
        strafeSpeed *= 2f;
        hoverSpeed *= 2f;

        yield return new WaitForSeconds(15f);
        forwardSpeed /= 2f;
        strafeSpeed /= 2f;
        hoverSpeed /= 2f;

        yield return null;
    }

    private IEnumerator shieldEff()
    {
        Vector3 shieldPosition = player.transform.position;
        float shieldHeight = shield.GetComponent<Renderer>().bounds.size.y;
        float yOffset = (player.transform.lossyScale.y + shieldHeight) / 2f;
        shieldPosition.y += yOffset / 2;

        shieldE = Instantiate(shield, shieldPosition, player.transform.rotation);
        shieldE.transform.parent = player.transform;

        isShieldActive = true;

        yield return new WaitForSeconds(15f);

        Destroy(shieldE);
        isShieldActive = false;

        yield return null;
    }
}
