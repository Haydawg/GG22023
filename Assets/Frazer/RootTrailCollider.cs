using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootTrailCollider : MonoBehaviour
{

    public RootTrailTest rootTrail;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Trigger2D Hit, " + collision.gameObject.name);

        if (collision.CompareTag("Enemy"))
        {
            //RootController.Instance.TrailCollidedWithEnemy(this.gameObject, collision);

            Vector3 hitLocation = collision.gameObject.transform.position;
            rootTrail.GrowBackToPosition(hitLocation, collision.gameObject);

            EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
            EnemyType enemyType = enemyController.GetEnemyType();

            if (enemyType == EnemyType.LawnMower || enemyType == EnemyType.WeedWacker)
            {
                EventsManager.Instance.TailCollidedWithEnemy?.Invoke();
            }
        }
    }
}
