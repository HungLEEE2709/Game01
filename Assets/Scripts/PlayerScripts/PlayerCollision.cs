using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private PlayerAttack playerAttack;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerHealth.IsDead) return;

        GameObject other = collision.collider.gameObject;

        if (other.CompareTag("Enemy"))
        {
            HandleEnemyCollision(other);
        }
        //else if (other.CompareTag("Coin"))
        //{
            //HandleCoinPickup(other);
        //}
    }

    private void HandleEnemyCollision(GameObject enemyObject)
    {
        if (!playerAttack.IsAttacking) return;

        PatrolEnemy enemy = enemyObject.GetComponent<PatrolEnemy>();
        if (enemy != null)
        {
            int damage = 10 * playerAttack.CurrentAttack;
            enemy.TakeDamage(damage);

            Debug.Log($"Gây {damage} damage cho Enemy bằng Attack{playerAttack.CurrentAttack}");
        }
    }


    //private void HandleCoinPickup(GameObject coinObject)
    //{
    //Coin coin = coinObject.GetComponent<Coin>();
    //if (coin != null)
    //{
    //coin.Collect(); // Gọi xử lý thu thập coin
    //}
    // }
}
