using UnityEngine;
using System.Collections;

public class FireballScript : MonoBehaviour {

	public GameObject explosionPrefab;

	void OnCollisionEnter(Collision col) {
		//Debug.Log (col.transform.name);
		ContactPoint contact = col.contacts[0];
		Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
		Vector3 pos = gameObject.transform.position;
		Instantiate(explosionPrefab, pos, rot);
		Destroy(gameObject);
	}
}
