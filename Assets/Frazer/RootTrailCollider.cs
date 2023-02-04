using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootTrailCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Trigger2D Hit, " + collision.gameObject.name);

        if (collision.CompareTag("Enemy"))
        {
            RootController.Instance.TrailCollidedWithEnemy(this.gameObject, collision);
            EventsManager.Instance.TailCollidedWithEnemy?.Invoke();
        }
    }
}
