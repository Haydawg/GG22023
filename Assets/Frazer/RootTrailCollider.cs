using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootTrailCollider : MonoBehaviour
{

    public RootTrailTest rootTrail;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
            EnemyType enemyType = enemyController.GetEnemyType();

            if (enemyType == EnemyType.LawnMower || enemyType == EnemyType.WeedWacker)
            {
                Vector3 hitLocation = collision.gameObject.transform.position;
                rootTrail.GrowBackToPosition(hitLocation);
                EventsManager.Instance.TailCollidedWithEnemy?.Invoke();
            }
            else if (enemyType == EnemyType.Herbicides)
            {
                Debug.Log("Trail collided with herbicides enemy, taking no damage");
            }
        }
        else if (collision.CompareTag("Herbicides"))
        {
            Vector3 hitLocation = collision.gameObject.transform.position;
            rootTrail.GrowBackToPosition(hitLocation);
            EventsManager.Instance.TailCollidedWithHerbicides?.Invoke();
        }
    }
}
