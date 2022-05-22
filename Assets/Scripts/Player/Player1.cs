using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//behaviours for the player
[RequireComponent (typeof (Controller2D))]
public class Player1 : MonoBehaviour
{
    public float maxJumpHeight = 4, minJumpHeight = 2, timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f, accelerationTimeGrounded = .1f;
    bool ghostJump = true, ghostJumpActivate = false;

    public Vector2 wallLeap;
    public float wallSlideSpeedMax = 3, wallStickTime = .25f, wallHugTime = 2f;
    int wallHugCount = 0;
    float timeToWallUnstick, timeToWallUnhug;

    public float dashTime, dashWaitTime;
    float moveSpeed = 6, maxJumpVelocity = 8, minJumpVelocity, gravity = -20, velocityXSmoothing;
    public Vector3 velocity;

    bool isJumping = false, jumpCancel = false, refresh = false;
    [HideInInspector] public bool canDash = true;

    Controller2D controller;

    float movementInput;
    int wallDirX;
    bool wallSliding;

    public Material flash;
    public Transform height;
    public ScreenShake screenShakeControler;
    public AudioClip dashSound, jumpSound, groundHitSound, dashRefreshSound;
    AudioSource audioSource;
    Material material;
    public SpriteRenderer renderer, lights;
    public Animator animator, lightsAnimator, heightAnimator;
    Animations1 animations;
    BoxCollider2D box;
    Vector2 boxSize;

    [HideInInspector]
    public bool isDashing = false, invincible = false, isPaused = false;
    [HideInInspector]
    public int gravityMultiplier = 1, directionY = 0;
    int cont = 0;
    public float dashSpeed = 1.5f;
    float faceDir = 1;

    [HideInInspector] public static Player1 instance;

    void Awake(){
        controller = GetComponent<Controller2D>();
        animations = GetComponent<Animations1>();
        audioSource = GetComponent<AudioSource>();
        box = GetComponent<BoxCollider2D>();
        boxSize = box.size;
        instance = this;
    }

    void Start(){
        gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        material = renderer.material;
    }

    public void SetDirectionalInput(float input){
        movementInput = input;
        if(input != 0){
            SwitchSides();
        }
    }
    
    void Update()
    {
        bool grounded = (controller.collisions.below && gravityMultiplier > 0 || controller.collisions.above && gravityMultiplier < 0) ? true : false;
        animator.SetFloat("move", Mathf.Abs(movementInput));
        lightsAnimator.SetFloat("move", Mathf.Abs(movementInput));
        animator.SetBool("isJumping", !grounded);
        lightsAnimator.SetBool("isJumping", !grounded);
        animator.SetBool("wallSliding", wallSliding);
        lightsAnimator.SetBool("wallSliding", wallSliding);

        CalculateVelocity();
        HandleWallSliding();
        CornerCorrection();
        
        controller.Move(velocity * Time.deltaTime);

        if(controller.collisions.below && gravityMultiplier > 0 || 
        controller.collisions.above && gravityMultiplier < 0){
            if(velocity.y != 0)
                //audioSource.PlayOneShot(groundHitSound, audioSource.volume);
            canDash = ghostJump = true;
            wallHugCount = 0;
            lights.color = Color.white;
        }
        else{
            if(!ghostJumpActivate){
                StartCoroutine("GhostJump");
            }
        }

        if(controller.collisions.above || controller.collisions.below){
            velocity.y = 0;
        }
    }

    IEnumerator GhostJump(){
        yield return new WaitForSeconds(0.15f);
        ghostJump = false;
    }

    void SwitchSides(){
        if(!isPaused || wallSliding) return;
        if(faceDir != movementInput){
            renderer.flipX = !renderer.flipX;
            lights.flipX = !lights.flipX;
            faceDir = -faceDir;
        }
    }

    public void OnJumpInputDown(){
        if(wallSliding){
            velocity.x = -wallDirX * wallLeap.x;
            velocity.y = wallLeap.y * gravityMultiplier;
            wallHugCount = 0;
            audioSource.PlayOneShot(jumpSound, audioSource.volume);
        }
        if((controller.collisions.below && gravityMultiplier > 0 || 
        controller.collisions.above && gravityMultiplier < 0) || ghostJump){
            velocity.y = maxJumpVelocity * gravityMultiplier;
            isJumping = jumpCancel = ghostJump = false;
            audioSource.PlayOneShot(jumpSound, audioSource.volume);
        }
    }

    public void OnJumpInputUp(){
        if(velocity.y > minJumpVelocity && gravityMultiplier > 0 || 
        velocity.y < minJumpVelocity && gravityMultiplier < 0){
            velocity.y = minJumpVelocity * gravityMultiplier;
        }
    }

    void CalculateVelocity(){
        float targetVelocityX = movementInput * moveSpeed;
        if(isDashing && canDash){
            if(cont == 0){
                float horizontalDashDirection = (directionY != 0 && movementInput == 0) ? 0 : faceDir;
                velocity.x = moveSpeed * dashSpeed * horizontalDashDirection;
                velocity.y = moveSpeed * dashSpeed * directionY;
                cont += 1;
                StartCoroutine("DashTime");
            }
        }
        else if(!isDashing){
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
            velocity.y += gravity * Time.deltaTime * gravityMultiplier;
        }
    }

    IEnumerator DashTime(){
        screenShakeControler.Shake(1.0f);
        animator.SetBool("isDashing", true);
        invincible = true;
        audioSource.PlayOneShot(dashSound, audioSource.volume);
        renderer.material = flash;
        lights.material = flash;
        StartCoroutine(animations.Stretch(faceDir == 1 ? true : false));

        yield return new WaitForSeconds(dashTime/2);

        StopCoroutine(animations.Stretch(true));
        StartCoroutine(animations.UnStretch(faceDir == 1 ? true : false));

        yield return new WaitForSeconds(dashTime/2);

        velocity = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(dashWaitTime);

        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        box.size = boxSize;
        canDash = refresh ? true : false;
        renderer.material = lights.material = material;
        lights.color = refresh ? Color.white : Color.red;
        isDashing = invincible = refresh = false;
        animator.SetBool("isDashing", false);
        StartCoroutine(ChangeGravity(0.4f, 0.6f));
        cont = 0;
    }

    public IEnumerator ChangeGravity(float multiplier, float time){
        velocity.y = 0;
        gravity = (-(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2))*multiplier;
        yield return new WaitForSeconds(time);
        gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
    }

    public void CancelDash(){
        StopCoroutine("DashTime");
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        box.size = boxSize;
        canDash = refresh ? true : false;
        renderer.material = lights.material = material;
        lights.color = refresh ? Color.white : Color.red;
        isDashing = invincible = refresh = false;
        animator.SetBool("isDashing", false);
        cont = 0;
    }

    public void Refresh(){
        audioSource.PlayOneShot(dashRefreshSound, audioSource.volume);
        refresh = canDash = true;
        lights.color = Color.white;
    }

    void HandleWallSliding(){
        wallDirX = (controller.collisions.left)?-1:1;
        wallSliding = false;
        if((controller.collisions.wall)
         && !controller.collisions.below
          && (velocity.y <= 0 && gravityMultiplier > 0
          || velocity.y >= 0 && gravityMultiplier < 0)){
            wallSliding = true;
            if(wallHugCount == 0){
                timeToWallUnhug = wallHugTime;
                wallHugCount += 1;
            }
            if(timeToWallUnhug > 0){
                velocity.y = 0;
                timeToWallUnhug -= Time.deltaTime;
            }
            else if(velocity.y < -wallSlideSpeedMax && timeToWallUnhug <= 0){
                velocity.y = -wallSlideSpeedMax;
            }
            if(timeToWallUnstick > 0) {
                velocity.x = 0;
                velocityXSmoothing = 0;

                if(movementInput != wallDirX && movementInput != 0){
                    timeToWallUnstick -= Time.deltaTime;
                }
                else{
                    timeToWallUnstick = wallStickTime;
                }
            }
            else{
                timeToWallUnstick = wallStickTime;
            }
        }
    }

    void CornerCorrection(){
        int mask = 1 << LayerMask.NameToLayer("Ground");
        Bounds bounds = box.bounds;
        Vector2 outerRightPosition = new Vector2(bounds.max.x-0.03f, bounds.max.y*gravityMultiplier);
        Vector2 innerRightPosition = new Vector2(bounds.max.x-0.1f, bounds.max.y*gravityMultiplier);
        Vector2 outerLeftPosition = new Vector2(bounds.min.x+0.03f, bounds.max.y*gravityMultiplier);
        Vector2 innerLeftPosition = new Vector2(bounds.min.x+0.1f, bounds.max.y*gravityMultiplier);

        RaycastHit2D outerRightHit = Physics2D.Raycast(outerRightPosition, Vector2.up*gravityMultiplier, 0.2f, mask);
        RaycastHit2D innerRightHit = Physics2D.Raycast(innerRightPosition, Vector2.up*gravityMultiplier, 0.2f, mask);
        RaycastHit2D outerLeftHit = Physics2D.Raycast(outerLeftPosition, Vector2.up*gravityMultiplier, 0.2f, mask);
        RaycastHit2D innerLeftHit = Physics2D.Raycast(innerLeftPosition, Vector2.up*gravityMultiplier, 0.2f, mask);

        if(outerRightHit.collider && !innerRightHit.collider && velocity.x <= 0){
            transform.Translate(-0.09f, 0, 0);
        }
        else if(outerLeftHit.collider && !innerLeftHit && velocity.x >= 0){
            transform.Translate(0.09f, 0, 0);
        }

        Debug.DrawRay(outerRightPosition, new Vector2(0, 0.2f*gravityMultiplier), Color.red);
        Debug.DrawRay(innerRightPosition, new Vector2(0, 0.2f*gravityMultiplier), Color.red);
        Debug.DrawRay(outerLeftPosition, new Vector2(0, 0.2f*gravityMultiplier), Color.red);
        Debug.DrawRay(innerLeftPosition, new Vector2(0, 0.2f*gravityMultiplier), Color.red);
    }
}
