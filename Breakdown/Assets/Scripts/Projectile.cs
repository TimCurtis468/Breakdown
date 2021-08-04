using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.name != "LeftWall") &&
            (collision.gameObject.name != "RightWall"))
        {
            Destroy(this.gameObject);
        }
    }
}
