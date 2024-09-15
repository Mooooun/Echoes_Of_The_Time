using UnityEngine;

public class MoveBehaviour : GenericBehaviour
{
    public float walkSpeed = 0.15f;                 // Vitesse de marche par défaut.
    public float runSpeed = 1.0f;                   // Vitesse de course par défaut.
    public float sprintSpeed = 2.0f;                // Vitesse de sprint par défaut.
    public float speedDampTime = 0.1f;              // Temps de transition pour changer d'animation en fonction de la vitesse.
    public string jumpButton = "Jump";              // Bouton de saut par défaut.
    public float jumpHeight = 1.5f;                 // Hauteur de saut par défaut.
    public float jumpIntertialForce = 10f;          // Force inertielle horizontale pendant le saut.

    public GameObject previewObject;                // Référence à l'objet de prévisualisation.
    public Animator previewAnimator;               // Animator pour la prévisualisation.

    public float speed, speedSeeker;               // Vitesse de déplacement.
    private int jumpBool;                           // Variable d'animation pour le saut.
    private int groundedBool;                       // Variable d'animation pour indiquer si le joueur est au sol.
    private bool jump;                              // Détermine si le joueur a commencé à sauter.
    private bool isColliding;                       // Détermine si le joueur entre en collision avec un obstacle.

    public bool canMove = true;                     // Permet ou non au joueur de se déplacer.
    public float m_GravityMultiplier = 1f;          // Multiplicateur de gravité.

    void Start()
    {
        // Initialiser les références des variables d'animation.
        jumpBool = Animator.StringToHash("Jump");
        groundedBool = Animator.StringToHash("Grounded");
        behaviourManager.GetAnim.SetBool(groundedBool, true);

        previewObject = GameObject.FindGameObjectWithTag("PlayerPrewiew");
        // Initialiser l'Animator de la prévisualisation si elle existe.
        if (previewObject != null)
        {
            previewAnimator = previewObject.GetComponent<Animator>();
            Debug.Log("Objet initialiser");
        }

        // S'abonner et enregistrer ce comportement comme comportement par défaut.
        behaviourManager.SubscribeBehaviour(this);
        behaviourManager.RegisterDefaultBehaviour(this.behaviourCode);
        speedSeeker = runSpeed;
    }

    // Update est utilisé pour mettre à jour les caractéristiques indépendamment du comportement actif.
    void Update()
    {
        // Obtenir l'entrée pour le saut.
        if (!jump && Input.GetButtonDown(jumpButton) && behaviourManager.IsCurrentBehaviour(this.behaviourCode) && !behaviourManager.IsOverriding())
        {
            jump = true;
        }

        // Mettre à jour l'état de marche dans l'animation.
        bool isWalking = Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f;
        behaviourManager.GetAnim.SetBool("IsWalking", isWalking); // Mettre à jour le paramètre d'animation pour la marche.

        // Répliquer l'état de marche dans l'Animator de la prévisualisation.
        if (previewAnimator != null)
        {
            previewAnimator.SetBool("IsWalking", isWalking);
        }
    }

    // LocalFixedUpdate surcharge la fonction virtuelle de la classe de base.
    public override void LocalFixedUpdate()
    {
        // Gérer le mouvement du joueur.
        MovementManagement(behaviourManager.GetH, behaviourManager.GetV);

        // Gérer le saut.
        JumpManagement();
    }

    // Gérer les mouvements de saut.
    void JumpManagement()
    {
        // Démarrer un nouveau saut.
        if (jump && !behaviourManager.GetAnim.GetBool(jumpBool) && behaviourManager.IsGrounded())
        {
            behaviourManager.LockTempBehaviour(this.behaviourCode);
            behaviourManager.GetAnim.SetBool(jumpBool, true);

            // Répliquer le saut dans l'Animator de la prévisualisation.
            if (previewAnimator != null)
            {
                previewAnimator.SetBool(jumpBool, true);
            }

            // Est-ce un saut en locomotion ?
            if (behaviourManager.GetAnim.GetFloat(speedFloat) > 0.1f)
            {
                // Changer temporairement la friction pour passer à travers les obstacles.
                GetComponent<CapsuleCollider>().material.dynamicFriction = 0f;
                GetComponent<CapsuleCollider>().material.staticFriction = 0f;

                // Enlever la vélocité verticale pour éviter les "super sauts".
                RemoveVerticalVelocity();

                // Appliquer une impulsion verticale pour le saut.
                float velocity = 2f * Mathf.Abs(Physics.gravity.y) * jumpHeight;
                velocity = Mathf.Sqrt(velocity);
                behaviourManager.GetRigidBody.AddForce(Vector3.up * velocity, ForceMode.VelocityChange);
            }
        }
        // Est-ce que le joueur est déjà en train de sauter ?
        else if (behaviourManager.GetAnim.GetBool(jumpBool))
        {
            // Maintenir le mouvement vers l'avant pendant le saut.
            if (!behaviourManager.IsGrounded() && !isColliding && behaviourManager.GetTempLockStatus())
            {
                behaviourManager.GetRigidBody.AddForce(transform.forward * jumpIntertialForce * Physics.gravity.magnitude * sprintSpeed, ForceMode.Acceleration);
            }

            // Le joueur a atterri ?
            if ((behaviourManager.GetRigidBody.velocity.y < 0) && behaviourManager.IsGrounded())
            {
                behaviourManager.GetAnim.SetBool(groundedBool, true);

                // Répliquer l'atterrissage dans l'Animator de la prévisualisation.
                if (previewAnimator != null)
                {
                    previewAnimator.SetBool(groundedBool, true);
                }

                // Réinitialiser la friction.
                GetComponent<CapsuleCollider>().material.dynamicFriction = 0.6f;
                GetComponent<CapsuleCollider>().material.staticFriction = 0.6f;

                // Réinitialiser les paramètres liés au saut.
                jump = false;
                behaviourManager.GetAnim.SetBool(jumpBool, false);
                behaviourManager.UnlockTempBehaviour(this.behaviourCode);

                // Répliquer la fin du saut dans la prévisualisation.
                if (previewAnimator != null)
                {
                    previewAnimator.SetBool(jumpBool, false);
                }
            }
        }
    }

    // Gérer le mouvement de base du joueur.
    void MovementManagement(float horizontal, float vertical)
    {
        if (!canMove) return;

        // Si le joueur est au sol, appliquer la gravité.
        if (behaviourManager.IsGrounded())
        {
            behaviourManager.GetRigidBody.useGravity = true;
        }
        else if (!behaviourManager.GetAnim.GetBool(jumpBool) && behaviourManager.GetRigidBody.velocity.y > 0)
        {
            RemoveVerticalVelocity();
        }

        // Appeler la fonction pour gérer l'orientation du joueur.
        Rotating(horizontal, vertical);

        // Calculer la vitesse appropriée.
        Vector2 dir = new Vector2(horizontal, vertical);
        speed = Vector2.ClampMagnitude(dir, 1f).magnitude;

        // Ajuster la vitesse selon la molette de la souris.
        speedSeeker += Input.GetAxis("Mouse ScrollWheel");
        speedSeeker = Mathf.Clamp(speedSeeker, walkSpeed, runSpeed);
        speed *= speedSeeker;

        if (behaviourManager.IsSprinting())
        {
            speed = sprintSpeed;
        }

        // Mettre à jour la vitesse dans l'Animator.
        behaviourManager.GetAnim.SetFloat(speedFloat, speed, speedDampTime, Time.deltaTime);

        // Répliquer la vitesse dans l'Animator de la prévisualisation.
        if (previewAnimator != null)
        {
            previewAnimator.SetFloat(speedFloat, speed, speedDampTime, Time.deltaTime);
        }
    }

    // Supprimer la vélocité verticale.
    private void RemoveVerticalVelocity()
    {
        Vector3 horizontalVelocity = behaviourManager.GetRigidBody.velocity;
        horizontalVelocity.y = 0;
        behaviourManager.GetRigidBody.velocity = horizontalVelocity;
    }

    // Gérer l'orientation du joueur en fonction de la caméra et des touches appuyées.
    Vector3 Rotating(float horizontal, float vertical)
    {
        // Obtenir la direction avant de la caméra sans la composante verticale.
        Vector3 forward = behaviourManager.playerCamera.TransformDirection(Vector3.forward);
        forward.y = 0.0f;
        forward = forward.normalized;

        // Calculer la direction cible basée sur la caméra et la touche pressée.
        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        Vector3 targetDirection = forward * vertical + right * horizontal;

        // Interpoler la rotation actuelle vers la direction cible.
        if (behaviourManager.IsMoving() && targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion newRotation = Quaternion.Slerp(behaviourManager.GetRigidBody.rotation, targetRotation, behaviourManager.turnSmoothing);
            behaviourManager.GetRigidBody.MoveRotation(newRotation);
            behaviourManager.SetLastDirection(targetDirection);
        }

        // Si le joueur est immobile, réinitialiser l'orientation selon la dernière direction.
        if (!(Mathf.Abs(horizontal) > 0.9f || Mathf.Abs(vertical) > 0.9f))
        {
            behaviourManager.Repositioning();
        }

        return targetDirection;
    }

    private void OnCollisionStay(Collision collision)
    {
        isColliding = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isColliding = false;
    }

    // Méthode pour arrêter tous les mouvements du joueur.
    public void StopMovement()
    {
        canMove = false; // Désactiver la capacité à se déplacer.

        // Réinitialiser la vitesse du joueur.
        speed = 0;
        behaviourManager.GetRigidBody.velocity = Vector3.zero;

        // Arrêter les animations de mouvement.
        behaviourManager.GetAnim.SetFloat(speedFloat, speed);
        if (previewAnimator != null)
        {
            previewAnimator.SetFloat(speedFloat, speed);
        }

        // Réinitialiser les états de saut et de collision.
        jump = false;
        isColliding = false;

        // Mettre à jour l'état de l'Animator pour refléter que le joueur est immobile.
        behaviourManager.GetAnim.SetBool("IsWalking", false);
        behaviourManager.GetAnim.SetBool(jumpBool, false);
        behaviourManager.GetAnim.SetBool(groundedBool, true);

        if (previewAnimator != null)
        {
            previewAnimator.SetBool("IsWalking", false);
            previewAnimator.SetBool(jumpBool, false);
            previewAnimator.SetBool(groundedBool, true);
        }

        // Réinitialiser les paramètres de friction du Collider.
        GetComponent<CapsuleCollider>().material.dynamicFriction = 0.6f;
        GetComponent<CapsuleCollider>().material.staticFriction = 0.6f;

        // Réinitialiser la gravité si le joueur est en l'air.
        if (!behaviourManager.IsGrounded())
        {
            behaviourManager.GetRigidBody.useGravity = true;
        }
    }


    // Méthode pour réactiver les mouvements du joueur.
    public void ResumeMovement()
    {
        canMove = true; // Réactiver la capacité à se déplacer.

        // Réinitialiser la vitesse du joueur.
        speed = walkSpeed; // Par exemple, reprendre la vitesse de marche par défaut.
        behaviourManager.GetRigidBody.velocity = Vector3.zero;

        // Mettre à jour les animations pour refléter la capacité de mouvement.
        behaviourManager.GetAnim.SetFloat(speedFloat, speed);
        if (previewAnimator != null)
        {
            previewAnimator.SetFloat(speedFloat, speed);
        }

        // Réinitialiser les états de saut et de collision.
        jump = false;
        isColliding = false;

        // Mettre à jour l'état de l'Animator pour refléter que le joueur est prêt à marcher.
        behaviourManager.GetAnim.SetBool("IsWalking", true);
        behaviourManager.GetAnim.SetBool(jumpBool, false);
        behaviourManager.GetAnim.SetBool(groundedBool, true);

        if (previewAnimator != null)
        {
            previewAnimator.SetBool("IsWalking", true);
            previewAnimator.SetBool(jumpBool, false);
            previewAnimator.SetBool(groundedBool, true);
        }

        // Réinitialiser les paramètres de friction du Collider.
        GetComponent<CapsuleCollider>().material.dynamicFriction = 0.6f;
        GetComponent<CapsuleCollider>().material.staticFriction = 0.6f;

        // Réinitialiser la gravité si nécessaire.
        if (!behaviourManager.IsGrounded())
        {
            behaviourManager.GetRigidBody.useGravity = true;
        }
    }


}
