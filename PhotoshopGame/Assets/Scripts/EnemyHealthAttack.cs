using UnityEngine;

public class EnemyHealthAttack : MonoBehaviour
{
    [SerializeField] private float attackDamage = 5f;   // damage amount
    [SerializeField] private float attackSpeed = 1f;    // time in seconds between attack

    private float attackTimer = 0f;
    private bool canAttack = true;

    private void Update() {
        if (!canAttack) {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackSpeed) {
                canAttack = true;
                attackTimer = 0f;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("AttackPoint") && canAttack) {
            Debug.Log("Attacking");
            AttackPoint.Instance.TakeDamage(attackDamage);
            canAttack = false;
        }
    }
}
