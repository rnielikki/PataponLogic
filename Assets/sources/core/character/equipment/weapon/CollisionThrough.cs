using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ignores collision and make attack go thourgh.
/// <note>This is invented for Megapon fever attack. In some cases, you can simply define "when to destroy gameobject (attack)" by yourself.</note>
/// </summary>
public class CollisionThrough : MonoBehaviour
{
    Collider2D _collider;
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Ground")
        {
            //You may want to add attack (damaging) logic here - if this is called first the attack won't damage anything.
            Physics2D.IgnoreCollision(collision.collider, _collider);
        }
    }
}
