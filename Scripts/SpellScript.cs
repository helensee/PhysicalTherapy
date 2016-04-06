using UnityEngine;
using System.Collections;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

// Change the material when certain poses are made with the Myo armband.
// Vibrate the Myo armband when a fist pose is made.
public class SpellScript : MonoBehaviour
{
	// Myo game object to connect with.
	// This object must have a ThalmicMyo script attached.
	public GameObject myo = null;

	// The pose from the last update. This is used to determine if the pose has changed
	// so that actions are only performed upon making them rather than every frame during
	// which they are active.
	private Pose _lastPose;
	private bool fireballReady;
	public float fireballspeed;
	public GameObject fireball;
	GameObject shootFireball;

	void Start() {
		_lastPose = Pose.Unknown;
		fireballReady = false;
	}

	// Update is called once per frame.
	void Update ()
	{
		if (Input.GetMouseButtonDown (0) || Input.GetKeyDown("space")) {
			GameObject shootFireball = Instantiate (fireball, GameObject.Find ("Spawn").transform.position, Quaternion.identity) as GameObject;
			shootFireball.GetComponent<Rigidbody> ().AddForce (transform.forward * fireballspeed);

			fireballReady = true;

			Debug.Log ("fireball");
		}

		// Access the ThalmicMyo component attached to the Myo game object.
		ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo> ();

		// Check if the pose has changed since last update.
		// The ThalmicMyo component of a Myo game object has a pose property that is set to the
		// currently detected pose (e.g. Pose.Fist for the user making a fist). If no pose is currently
		// detected, pose will be set to Pose.Rest. If pose detection is unavailable, e.g. because Myo
		// is not on a user's arm, pose will be set to Pose.Unknown.
		if (thalmicMyo.pose != _lastPose) {
			_lastPose = thalmicMyo.pose;

            // Vibrate the Myo armband when a fist is made.
            if (fireballReady==false && (thalmicMyo.pose == Pose.Fist)) {
                thalmicMyo.Vibrate (VibrationType.Medium);

				shootFireball = Instantiate (fireball, GameObject.Find ("Spawn").transform.position, Quaternion.identity) as GameObject;
				//shootFireball.GetComponent<Rigidbody> ().AddForce (Vector3.forward * fireballspeed);

				ExtendUnlockAndNotifyUserAction (thalmicMyo);

				fireballReady = true;

				Debug.Log ("fireball");

				// Change material when wave in, wave out or double tap poses are made.
			} if (fireballReady==true && thalmicMyo.pose == Pose.FingersSpread) {
				thalmicMyo.Vibrate (VibrationType.Short);

				shootFireball.GetComponent<Rigidbody> ().AddForce (transform.forward * fireballspeed);
				fireballReady = false;

				Debug.Log ("shoot forward");
			} if(fireballReady==true && thalmicMyo.pose == Pose.WaveIn) {
				thalmicMyo.Vibrate (VibrationType.Short);

                shootFireball.GetComponent<Rigidbody>().AddForce(-1 * transform.right * fireballspeed);
                fireballReady = false;

				Debug.Log ("shoot left");
			} if(fireballReady==true && thalmicMyo.pose == Pose.WaveOut) {
				thalmicMyo.Vibrate (VibrationType.Short);

				shootFireball.GetComponent<Rigidbody> ().AddForce (transform.right * fireballspeed);
				fireballReady = false;

				Debug.Log ("shoot right");
			}
			/*else if (thalmicMyo.pose == Pose.FingersSpread && fireballReady) {

				//Instantiate(fireball, GameObject.Find ("Spawn").transform.position, Quaternion.identity);
				//fireball.transform.Translate(transform.forward * fireballspeed);

			} else if (thalmicMyo.pose == Pose.WaveIn) {
				//GetComponent<Renderer>().material = waveInMaterial;

				ExtendUnlockAndNotifyUserAction (thalmicMyo);
			} else if (thalmicMyo.pose == Pose.WaveOut) {

				ExtendUnlockAndNotifyUserAction (thalmicMyo);
			} else if (thalmicMyo.pose == Pose.DoubleTap) {

				ExtendUnlockAndNotifyUserAction (thalmicMyo);
			}*/
		}
	}

	// Extend the unlock if ThalmcHub's locking policy is standard, and notifies the given myo that a user action was
	// recognized.
	void ExtendUnlockAndNotifyUserAction (ThalmicMyo myo)
	{
		ThalmicHub hub = ThalmicHub.instance;

		if (hub.lockingPolicy == LockingPolicy.Standard) {
			myo.Unlock (UnlockType.Timed);
		}

		myo.NotifyUserAction ();
	}
}
