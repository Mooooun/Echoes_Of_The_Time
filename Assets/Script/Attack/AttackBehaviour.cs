using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Equipment equipmentSystem;

    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private InteractBehaviour interactBehaviour;

    [Header("Configuration")]
    [SerializeField]
    private Collider weaponCollider; // Collider de l'arme

    private bool isAttacking;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && CanAttack())
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
            // Activer le collider pour détecter les collisions
            if (weaponCollider != null)
            {
                weaponCollider.enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (isAttacking && other.CompareTag("AI"))
        {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDammage(equipmentSystem.equipedWeaponItem.attackPoints);
            }
        }
    }

    bool CanAttack()
    {
       return equipmentSystem.equipedWeaponItem != null && !isAttacking && !uiManager.atLeastOnePanelOpened && !interactBehaviour.isBusy;
    }

    public void AttackFinished()
    {
        isAttacking = false;
        // Désactiver le collider après l'attaque
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }
    }
}
