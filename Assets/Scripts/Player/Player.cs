using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//behaviours for the player
public class Player : MonoBehaviour
{
    public float speed, jumpForce, dashTime = 0.5f, dashSpeed = 2f, wallJumpForceX, wallJumpForceY;
    public Material flash;
    public AudioClip dashSound, jumpSound, groundHitSound, dashRefreshSound;
    public ScreenShake screenShakeControler;
    AudioSource audioSource;
    Rigidbody2D rb;
    Material material; 
    SpriteRenderer renderer, lights;
    Animator animator, lightsAnimator, heightAnimator;
    Animations animations;
    
    bool isRight = true, canDash = true, isGrounded = true, ghostJump = true;
    
    bool inAction, isJumping, isWallJumping, wallSliding, wallHugging, refresh = false;
    float fallMultiplier = 3.5f, raycastLength = 0.58f;
    int cantGrab = 0;
    
    [HideInInspector]
    public bool appliedForce, invincible, isPaused = false;
    [HideInInspector]
    public float direction, directionY = 0;
    [HideInInspector]
    public int gravityMultiplier = 1;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        animations = GetComponent<Animations>();

        foreach(Transform child in transform){
            if(child.gameObject.name == "h"){
                heightAnimator = child.gameObject.GetComponent<Animator>();
                animator = child.GetChild(0).GetComponent<Animator>();
                renderer = child.GetChild(0).GetComponent<SpriteRenderer>();
                lights = child.GetChild(1).GetComponent<SpriteRenderer>();
                lightsAnimator = child.GetChild(1).GetComponent<Animator>();
                material = renderer.material;
            }
        }
    }

    
    void Update()
    {
        animator.SetFloat("move", Mathf.Abs(direction));
        lightsAnimator.SetFloat("move", Mathf.Abs(direction));
        SwitchSides();
        isGrounded = IsGrounded();
        if(!wallHugging && !wallSliding){
            animator.SetBool("wallSliding", false);
            lightsAnimator.SetBool("wallSliding", false);
        }
        CornerCorrection();
    }

    void FixedUpdate(){
        Move();
        WallHug();
        if(rb.velocity.y < 0 && gravityMultiplier == 1 || rb.velocity.y > 0 && gravityMultiplier == -1){
            rb.velocity += (Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1.7f) * Time.deltaTime)*gravityMultiplier;
        } else if ((rb.velocity.y > 0 && gravityMultiplier == 1 || rb.velocity.y < 0 && gravityMultiplier == -1) &&
        !isJumping){
            rb.velocity += (Vector2.up * Physics2D.gravity.y * (fallMultiplier * 1.4f - 1.7f) * Time.deltaTime)*gravityMultiplier;
        }
        if(wallSliding){
            rb.velocity = new Vector2(0, -2.0f*gravityMultiplier);
        }
        if(isGrounded && (rb.velocity.x > speed || rb.velocity.x < -speed) && !inAction && !appliedForce){
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if(rb.velocity.y < 0 && gravityMultiplier > 0 || rb.velocity.y > 0 && gravityMultiplier < 0){
            raycastLength = 0.8f;
        }
        else{
            raycastLength = 0.58f;
        }
    }

    void Move(){
        if(inAction || !isPaused || appliedForce || (isWallJumping && direction == 0)) return;
        
        rb.velocity = new Vector2(speed*direction, rb.velocity.y);
    }

    void SwitchSides(){
        if(!isPaused) return;
        if(isRight && direction<0 || !isRight && direction>0){
            transform.Rotate(0, 180, 0);
            isRight = !isRight;
        }
    }

    void CornerCorrection(){
        int mask = 1 << LayerMask.NameToLayer("Ground");
        Vector2 outerRightPosition = new Vector2(transform.position.x+0.3f, transform.position.y+0.46f*gravityMultiplier);
        Vector2 innerRightPosition = new Vector2(transform.position.x+0.22f, transform.position.y+0.46f*gravityMultiplier);
        Vector2 outerLeftPosition = new Vector2(transform.position.x-0.3f, transform.position.y+0.46f*gravityMultiplier);
        Vector2 innerLeftPosition = new Vector2(transform.position.x-0.22f, transform.position.y+0.46f*gravityMultiplier);

        RaycastHit2D outerRightHit = Physics2D.Raycast(outerRightPosition, Vector2.up*gravityMultiplier, 0.2f, mask);
        RaycastHit2D innerRightHit = Physics2D.Raycast(innerRightPosition, Vector2.up*gravityMultiplier, 0.2f, mask);
        RaycastHit2D outerLeftHit = Physics2D.Raycast(outerLeftPosition, Vector2.up*gravityMultiplier, 0.2f, mask);
        RaycastHit2D innerLeftHit = Physics2D.Raycast(innerLeftPosition, Vector2.up*gravityMultiplier, 0.2f, mask);

        if(outerRightHit.collider && !innerRightHit.collider && rb.velocity.x <= 0){
            transform.Translate(-0.09f, 0, 0);
        }
        else if(outerLeftHit.collider && !innerLeftHit && rb.velocity.x >= 0){
            transform.Translate(0.09f, 0, 0);
        }

        Debug.DrawRay(outerRightPosition, new Vector2(0, 0.2f*gravityMultiplier), Color.red);
        Debug.DrawRay(innerRightPosition, new Vector2(0, 0.2f*gravityMultiplier), Color.red);
        Debug.DrawRay(outerLeftPosition, new Vector2(0, 0.2f*gravityMultiplier), Color.red);
        Debug.DrawRay(innerLeftPosition, new Vector2(0, 0.2f*gravityMultiplier), Color.red);
    }

    bool IsGrounded(){
        int mask = 1 << LayerMask.NameToLayer("Ground");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up*gravityMultiplier, raycastLength, mask);
        Debug.DrawRay(transform.position, new Vector2(0, -raycastLength*gravityMultiplier), Color.red);

        if(hit.collider){
            animator.SetBool("isJumping", false);
            lightsAnimator.SetBool("isJumping", false);
            StopCoroutine("GhostJump");
            lights.color = Color.white;
            canDash = ghostJump = true;
            appliedForce = isWallJumping = false;
            if(!isGrounded)
                audioSource.PlayOneShot(groundHitSound, audioSource.volume);
            return true;
        }
        lightsAnimator.SetBool("isJumping", true);
        animator.SetBool("isJumping", true);
        StartCoroutine("GhostJump");
        return false;
    }

    IEnumerator GhostJump(){
        yield return new WaitForSeconds(0.1f);
        ghostJump = false;
    }

    public void Jump(){
        if(!isPaused) return;
        if(isGrounded || ghostJump){
            rb.velocity = new Vector2(0,0);
            rb.AddForce(transform.up*jumpForce*gravityMultiplier);
            ghostJump = false;
            isJumping = true;
            audioSource.PlayOneShot(jumpSound, audioSource.volume);
        }
        else if(wallHugging || wallSliding){
            StopCoroutine("CantGrabTimer");
            StartCoroutine("CantGrabTimer");
            isWallJumping = true;
            cantGrab = isRight ? 1 : -1;
            int dir = isRight ? -1 : 1;
            transform.Rotate(0, 180, 0);
            isRight = !isRight;
            rb.AddForce(new Vector2(dir*wallJumpForceX, wallJumpForceY*gravityMultiplier));
            audioSource.PlayOneShot(jumpSound, audioSource.volume);
        }
    }

    public IEnumerator AppliedForce(){
        appliedForce = true;
        yield return new WaitForSeconds(0.5f);
        appliedForce = false;
    }

    public void JumpCancel(){
        isJumping = false;
    }

    public void Dash(){
        if(inAction || !isPaused || !canDash) return;
        float d = isRight ? 1 : -1;
        StartCoroutine(Dashing(d));
    }

    IEnumerator Dashing(float dashDirection){
        screenShakeControler.Shake(1.0f);
        animator.SetBool("isDashing", true);
        inAction = invincible = true;
        canDash = refresh = appliedForce = isWallJumping = false;
        audioSource.PlayOneShot(dashSound, audioSource.volume);
        if(directionY != 0)
            rb.velocity = new Vector2(speed*dashSpeed*direction, speed*dashSpeed*directionY*gravityMultiplier);
        else
            rb.velocity = new Vector2(speed*dashSpeed*dashDirection, 0f);
        renderer.material = flash;
        lights.material = flash;
        //heightAnimator.SetBool("stretch", true);
        StartCoroutine(animations.Stretch(isRight));
        rb.gravityScale = 0;

        yield return new WaitForSeconds(dashTime/2);

        StopCoroutine(animations.Stretch(isRight));
        StartCoroutine(animations.UnStretch(isRight));

        yield return new WaitForSeconds(dashTime/2);

        rb.velocity = new Vector2(0,0);

        yield return new WaitForSeconds(0.1f);
        //heightAnimator.SetBool("stretch", false);
        animator.SetBool("isDashing", false);
        inAction = invincible = false;
        renderer.material = material;
        lights.material = material;
        lights.color = Color.red;
        rb.gravityScale = 1.7f*gravityMultiplier;
    }

    public void Refresh(){
        audioSource.PlayOneShot(dashRefreshSound, audioSource.volume);
        canDash = true;
        refresh = true;
        lights.color = Color.white;
    }

    void WallHug(){
        int mask = 1 << LayerMask.NameToLayer("Ground");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 0.5f, mask);
        Debug.DrawRay(transform.position, transform.right*0.5f, Color.red);

        if(hit.collider && 
        (rb.velocity.y < 0 && gravityMultiplier == 1 || rb.velocity.y > 0 && gravityMultiplier == -1) && 
        !inAction && !wallHugging && !wallSliding && 
        (isRight && cantGrab != 1 || !isRight && cantGrab != -1)){
            rb.gravityScale = 0;
            rb.velocity = new Vector2(0,0);
            animator.SetBool("wallSliding", true);
            lightsAnimator.SetBool("wallSliding", true);
            appliedForce = isWallJumping = false;
            wallHugging = true;
            StartCoroutine("WallSlide");
        }
        else if(!hit.collider){
            wallHugging = false;
            wallSliding = false;
            StopCoroutine("WallSlide");
            rb.gravityScale = 1.7f*gravityMultiplier;
        }
    }

    IEnumerator WallSlide(){
        yield return new WaitForSeconds(1.0f);
        wallSliding = true;
        wallHugging = false;
    }

    IEnumerator CantGrabTimer(){
        yield return new WaitForSeconds(0.8f);
        cantGrab = 0;
    }
}
