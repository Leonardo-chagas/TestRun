using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//behaviours for the colisions of the player with raycasts
public class Controller2D : RaycastController
{
    
    public CollisionInfo collisions;

    float maxClimbAngle = 50;
    float maxDescendAngle = 45;
    public bool descend = false;


    public override void Start(){
        base.Start();
        collisions.faceDir = 1;
    }

    public void Move(Vector3 moveAmount, bool standingOnPlatform = false){
        UpdateRaycastOrigins();
        CalculateRaySpacing();
        collisions.Reset();
        collisions.velocityOld = moveAmount;

        if (moveAmount.x != 0){
            collisions.faceDir = (int)Mathf.Sign(moveAmount.x);
        }

        if (moveAmount.y < 0){
            DescendSlope(ref moveAmount);
        }

        HorizontalCollisions(ref moveAmount);

        if (moveAmount.y != 0){
        VerticalCollisions(ref moveAmount);
        }

        transform.Translate (moveAmount);

        if(standingOnPlatform){
            collisions.below = true;
        }
    }

    void HorizontalCollisions(ref Vector3 moveAmount){
        float directionX = collisions.faceDir;
        float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;
        bool hasHit = false;

        if(Mathf.Abs(moveAmount.x) < skinWidth){
            rayLength = 2*skinWidth;
        }

        for (int i = 0; i < horizontalRayCount; i++){
            Vector2 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);
        
            if (hit){
                hasHit = true;
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                collisions.wall = true;
                
                /* if(hit.collider.CompareTag("Stairs")){
                    if(i != 0){
                        collisions.passThroughStairs = true;
                    }
                    if(collisions.passThroughStairs || collisions.fallingThroughPlatform){
                        continue;
                    }
                    if(descend){
                        collisions.fallingThroughPlatform = true;
                        Invoke("ResetFallingThroughPlatform",.5f);
                        continue;
                    }
                } */
                if(hit.collider.CompareTag("block"))
                    collisions.wall = false;
                if(hit.distance == 0){
                    continue;
                }

                if(i == 0 && slopeAngle <= maxClimbAngle){
                    if(collisions.descendingSlope){
                        collisions.descendingSlope = false;
                        moveAmount = collisions.velocityOld;
                    }
                    float distanceToSlopeStart = 0;
                    if(slopeAngle != collisions.slopeAngleOld){
                        distanceToSlopeStart = hit.distance - skinWidth;
                        moveAmount.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref moveAmount, slopeAngle);
                    moveAmount.x += distanceToSlopeStart * directionX;
                }

                if(!collisions.climbingSlope || slopeAngle > maxClimbAngle){
                    //moveAmount.x = (hit.distance - skinWidth) * directionX;
                    //rayLength = hit.distance;
                    moveAmount.x = Mathf.Min(Mathf.Abs(moveAmount.x), (hit.distance - skinWidth)) * directionX;
                    rayLength = Mathf.Min(Mathf.Abs(moveAmount.x) + skinWidth, hit.distance);

                    if(collisions.climbingSlope){
                        moveAmount.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
                    }

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
            /* else if(i == 0){
                collisions.passThroughStairs = false;
            } */
        }
        if(!hasHit)
            collisions.wall = false;
    }

    void VerticalCollisions(ref Vector3 moveAmount){
        float directionY = Mathf.Sign (moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++){
            Vector2 rayOrigin = (directionY == -1)?raycastOrigins.bottomLeft:raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);
        
            if (hit){
                /* if(hit.collider.CompareTag("Through") || hit.collider.CompareTag("Stairs")){
                    if(directionY == 1 || hit.distance == 0){
                        continue;
                    }
                    if(collisions.fallingThroughPlatform){
                        continue;
                    }
                    if(descend){
                        collisions.fallingThroughPlatform = true;
                        Invoke("ResetFallingThroughPlatform",.5f);
                        continue;
                    }
                } */
                if(hit.distance == 0){
                    continue;
                }
                moveAmount.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                if(collisions.climbingSlope){
                    moveAmount.x = moveAmount.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(moveAmount.x);
                }

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
        if(collisions.climbingSlope){
            float directionX = Mathf.Sign(moveAmount.x);
            rayLength = Mathf.Abs(moveAmount.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight) + Vector2.up * moveAmount.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit){
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != collisions.slopeAngle){
                    moveAmount.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                }
            }
        }
    }

    void ClimbSlope(ref Vector3 moveAmount, float slopeAngle){
        float moveDistance = Mathf.Abs(moveAmount.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if(moveAmount.y <= climbVelocityY){
            moveAmount.y = climbVelocityY;
            moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    void DescendSlope(ref Vector3 moveAmount){
        float directionX = Mathf.Sign(moveAmount.x);
        Vector2 rayOrigin = (directionX == -1)?raycastOrigins.bottomRight:raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        if(hit){
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if(slopeAngle != 0 && slopeAngle <= maxDescendAngle){
                if(Mathf.Sign(hit.normal.x) == directionX){
                    if(hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x)){
                        float moveDistance = Mathf.Abs(moveAmount.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
                        moveAmount.y -= descendVelocityY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }

    void ResetFallingThroughPlatform(){
        collisions.fallingThroughPlatform = false;
        descend = false;
    }

    public struct CollisionInfo {
        public bool above, below;
        public bool left, right;
        public bool wall;

        public bool climbingSlope;
        public bool descendingSlope;
        public float slopeAngle, slopeAngleOld;
        public Vector3 velocityOld;
        public int faceDir;
        public bool fallingThroughPlatform, passThroughStairs;

        public void Reset(){
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;
            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }
}
