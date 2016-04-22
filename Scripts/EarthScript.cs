using UnityEngine;
using System.Collections;

public class EarthScript : MonoBehaviour
{
    public GameObject explosionPrefab;

    void Update()
    {
        Destroy(gameObject, 2);
    }

    void OnCollisionEnter(Collision col)
    {
        //Debug.Log (col.transform.name);
        ContactPoint contact = col.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = gameObject.transform.position;
        Instantiate(explosionPrefab, pos, rot);
        Destroy(gameObject);
    }
}
