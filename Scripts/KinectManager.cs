using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Windows.Kinect;
using System.Text;

public class KinectManager : MonoBehaviour
{
    // material to copy color frame to
    public Material Material;
    private bool fireballReady;
    public float fireballspeed;
    public GameObject fireball;
    GameObject shootFireball;
    public GameObject fireballmini;

    public GameObject rock;
    GameObject shootRock;
    public float rockSpeed;

    // Kinect 
    private KinectSensor kinectSensor;
    
    // color frame and data 
    private ColorFrameReader colorFrameReader;
    private byte[] colorData;
    private Texture2D colorTexture;

    private BodyFrameReader bodyFrameReader;
    private int bodyCount;
    private Body[] bodies;

    // GUI output
    private UnityEngine.Color[] bodyColors;
    private string[] bodyText;

    /// <summary> List of gesture detectors, there will be one detector created for each potential body (max of 6) </summary>
    private List<GestureDetector> gestureDetectorList = null;

    public static KinectManager km = null;
    public Text currentGesture;

    // Use this for initialization
    void Start()
    {
        if (km == null)
        {
            km = this;
        }
        currentGesture.text = "hello";
        Debug.Log("View text hello");
        // GUI color and text objects
        this.bodyColors = new UnityEngine.Color[] {
            UnityEngine.Color.red, 
            UnityEngine.Color.yellow, 
            UnityEngine.Color.green, 
            UnityEngine.Color.white, 
            UnityEngine.Color.cyan, 
            UnityEngine.Color.magenta
        };

        this.bodyText = new string[this.bodyColors.Length];

        // get the sensor object
        this.kinectSensor = KinectSensor.GetDefault();

        if (this.kinectSensor != null)
        {
            this.bodyCount = this.kinectSensor.BodyFrameSource.BodyCount;

            // color reader
            this.colorFrameReader = this.kinectSensor.ColorFrameSource.OpenReader();

            // create buffer from RGBA frame description
            var desc = this.kinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Rgba);
            this.colorData = new byte[desc.BytesPerPixel * desc.LengthInPixels];

            this.colorTexture = new Texture2D(desc.Width, desc.Height, TextureFormat.RGBA32, false);
            Material.mainTexture = this.colorTexture;

            // body data
            this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();

            // body frame to use
            this.bodies = new Body[this.bodyCount];

            // initialize the gesture detection objects for our gestures
            this.gestureDetectorList = new List<GestureDetector>();
            for (int bodyIndex = 0; bodyIndex < this.bodyCount; bodyIndex++)
            {
                this.bodyText[bodyIndex] = "none";
                this.gestureDetectorList.Add(new GestureDetector(this.kinectSensor));
            }

            // start getting data from runtime
            this.kinectSensor.Open();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            GameObject shootRock = Instantiate(rock, GameObject.Find("Spawn").transform.position, Quaternion.identity) as GameObject;
            shootRock.GetComponent<Rigidbody>().AddForce(transform.forward * rockSpeed);
            
        }
        // ensure the readers are valid
        if (this.colorFrameReader != null && this.bodyFrameReader != null)
        {
            // copy the color frame to texture
            using (ColorFrame frame = this.colorFrameReader.AcquireLatestFrame())
            {
                if (frame != null)
                {
                    frame.CopyConvertedFrameDataToArray(this.colorData, ColorImageFormat.Rgba);
                    this.colorTexture.LoadRawTextureData(this.colorData);
                    this.colorTexture.Apply();
                }
            }

            // process bodies
            bool newBodyData = false;
            using (BodyFrame bodyFrame = this.bodyFrameReader.AcquireLatestFrame())
            {
                if (bodyFrame != null)
                {
                    bodyFrame.GetAndRefreshBodyData(this.bodies);
                    newBodyData = true;
                }
            }

            if (newBodyData)
            {
                // update gesture detectors with the correct tracking id
                for (int bodyIndex = 0; bodyIndex < this.bodyCount; bodyIndex++)
                {
                    var body = this.bodies[bodyIndex];
                    if (body != null)
                    {
                        var trackingId = body.TrackingId;

                        // if the current body TrackingId changed, update the corresponding gesture detector with the new value
                        if (trackingId != this.gestureDetectorList[bodyIndex].TrackingId)
                        {
                            this.bodyText[bodyIndex] = "none";
                            this.gestureDetectorList[bodyIndex].TrackingId = trackingId;

                            // if the current body is tracked, unpause its detector to get VisualGestureBuilderFrameArrived events
                            // if the current body is not tracked, pause its detector so we don't waste resources trying to get invalid gesture results
                            this.gestureDetectorList[bodyIndex].IsPaused = (trackingId == 0);
                            this.gestureDetectorList[bodyIndex].OnGestureDetected += CreateOnGestureHandler(bodyIndex);
                        }
                    }
                }
            }
        }
    }

    public void shootfireball()
    {
        GameObject shootFireball = Instantiate(fireball, GameObject.Find("Spawn").transform.position, Quaternion.identity) as GameObject;
        shootFireball.GetComponent<Rigidbody>().AddForce(transform.forward * fireballspeed);

        fireballReady = true;

        Debug.Log("fireball");
    }

    public void shootfireballMini()
    {
        GameObject shootFireball = Instantiate(fireballmini, GameObject.Find("Spawn").transform.position, Quaternion.identity) as GameObject;
        shootFireball.GetComponent<Rigidbody>().AddForce(transform.forward * fireballspeed);

        fireballReady = true;

        Debug.Log("fireball");
    }

    public void FalseGesture1()
    {
        currentGesture.text = "Bend elbow";
        Debug.Log("KM: bend elbow...");
    }

    public void FalseGesture2()
    {
        currentGesture.text = "Arm's are too low, raise elbows";
        Debug.Log("KM: arms too low...");
    }

    public void MP_Level1()
    {
        currentGesture.text = "Arm raise at 30%";
        GameObject shootRock = Instantiate(rock, GameObject.Find("Spawn").transform.position, Quaternion.identity) as GameObject;
        shootRock.GetComponent<Rigidbody>().AddForce(transform.forward * 1000);
        Debug.Log("KM: 30%...");
    }

    public void MP_Level2()
    {
        currentGesture.text = "Arm raise at 60%";
        GameObject shootRock = Instantiate(rock, GameObject.Find("Spawn").transform.position, Quaternion.identity) as GameObject;
        shootRock.GetComponent<Rigidbody>().AddForce(transform.forward * 1500);
        Debug.Log("KM: 60%...");
    }

    public void MP_Level3()
    {
        currentGesture.text = "Arm raise at 100%";
        GameObject shootRock = Instantiate(rock, GameObject.Find("Spawn").transform.position, Quaternion.identity) as GameObject;
        shootRock.GetComponent<Rigidbody>().AddForce(transform.forward * rockSpeed);
        Debug.Log("KM: 100%...");
    }

    private EventHandler<GestureEventArgs> CreateOnGestureHandler(int bodyIndex)
    {
        return (object sender, GestureEventArgs e) => OnGestureDetected(sender, e, bodyIndex);
    }

    private void OnGestureDetected(object sender, GestureEventArgs e, int bodyIndex)
    {
        var isDetected = e.IsBodyTrackingIdValid && e.IsGestureDetected;

        StringBuilder text = new StringBuilder(string.Format("Gesture Detected? {0}\n", isDetected));
        text.Append(string.Format("Confidence: {0}\n", e.DetectionConfidence));
        this.bodyText[bodyIndex] = text.ToString();
    }

    void OnGUI()
    {
        GUI.skin.box.fontSize = 16;
        const int NumColumns = 2;
        for (int i = 0; i < this.bodyCount; i++)
        {
            if (this.bodies[i] != null)
            {
                GUI.contentColor = this.bodyColors[i];
                GUI.Box(new Rect(10 + ((i % NumColumns) * 145), 20 + ((i / NumColumns) * 145), 140, 140), this.bodyText[i]);
            }

        }
    }

    void OnApplicationQuit()
    {
        if (this.colorFrameReader != null)
        {
            this.colorFrameReader.Dispose();
            this.colorFrameReader = null;
        }

        if (this.bodyFrameReader != null)
        {
            this.bodyFrameReader.Dispose();
            this.bodyFrameReader = null;
        }

        if (this.kinectSensor != null)
        {
            if (this.kinectSensor.IsOpen)
            {
                this.kinectSensor.Close();
            }

            this.kinectSensor = null;
        }
    }

}
