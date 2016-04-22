using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InformationGUI : MonoBehaviour {

    bool earthTutorialWindow;

    private float leafOffset;
    private float frameOffset;
    private float skullOffset;

    private float RibbonOffsetX;
    private float FrameOffsetX;
    private float SkullOffsetX;
    private float RibbonOffsetY;
    private float FrameOffsetY;
    private float SkullOffsetY;

    private float WSwaxOffsetX;
    private float WSwaxOffsetY;
    private float WSribbonOffsetX;
    private float WSribbonOffsetY;

    private int spikeCount;
    
    public GUI skin;
    public GUISkin necromancer;

    private Rect windowRectEarth = new Rect(0, 40, 350, 500);

    private Vector2 scrollPosition;
    private float HroizSliderValue = 0.5f;
    private float VertSliderValue = 0.5f;
    private bool ToggleBTN = false;

    public Texture MP1;

    //skin info
    private string NecroText = "This started as a question... How flexible is the built in GUI in unity? The answer... pretty damn flexible! At first I wasn抰 so sure; it seemed no one ever used it to make a non OS style GUI at least not a publicly available one. So I decided I couldn抰 be sure until I tried to develop a full GUI, Long story short Necromancer was the result and is now available to the general public, free for comercial and non-comercial use. I only ask that if you add something Share it.   Credits to Kevin King for the fonts.";

    public void AddSpikes(float winX)
    {
        float spikeCount = Mathf.Floor(winX - 152) / 22;
        GUILayout.BeginHorizontal();
        GUILayout.Label("", "SpikeLeft");//-------------------------------- custom
        for (int i = 0; i < spikeCount; i++)
        {
            GUILayout.Label("", "SpikeMid");//-------------------------------- custom
        }
        GUILayout.Label("", "SpikeRight");//-------------------------------- custom
        GUILayout.EndHorizontal();
    }

    public void FancyTop(float topX)
    {
        leafOffset = (topX / 2) - 64;
        frameOffset = (topX / 2) - 27;
        skullOffset = (topX / 2) - 20;
        GUI.Label(new Rect(leafOffset, 18, 0, 0), "", "GoldLeaf");//-------------------------------- custom	
        GUI.Label(new Rect(frameOffset, 3, 0, 0), "", "IconFrame");//-------------------------------- custom	
        GUI.Label(new Rect(skullOffset, 12, 0, 0), "", "Skull");//-------------------------------- custom	
    }

    public void WaxSeal(float x, float y)
    {
        WSwaxOffsetX = x - 120;
        WSwaxOffsetY = y - 115;
        WSribbonOffsetX = x - 114;
        WSribbonOffsetY = y - 83;

        GUI.Label(new Rect(WSribbonOffsetX, WSribbonOffsetY, 0, 0), "", "RibbonBlue");//-------------------------------- custom	
        GUI.Label(new Rect(WSwaxOffsetX, WSwaxOffsetY, 0, 0), "", "WaxSeal");//-------------------------------- custom	
    }

    public void MP1Build(float x, float y)
    {
        WSwaxOffsetX = x - 120;
        WSwaxOffsetY = y - 115;
        WSribbonOffsetX = x - 114;
        WSribbonOffsetY = y - 83;

        GUI.DrawTexture(new Rect(WSwaxOffsetX, WSwaxOffsetY, 0, 0), MP1, ScaleMode.ScaleToFit, true, 10.0F);
    }

    public void DeathBadge(float x, float y)
    {
        RibbonOffsetX = x;
        FrameOffsetX = x + 3;
        SkullOffsetX = x + 10;
        RibbonOffsetY = y + 22;
        FrameOffsetY = y;
        SkullOffsetY = y + 9;

        GUI.Label(new Rect(RibbonOffsetX, RibbonOffsetY, 0, 0), "", "RibbonRed");//-------------------------------- custom	
        GUI.Label(new Rect(FrameOffsetX, FrameOffsetY, 0, 0), "", "IconFrame");//-------------------------------- custom	
        GUI.Label(new Rect(SkullOffsetX, SkullOffsetY, 0, 0), "", "Skull");//-------------------------------- custom	
    }

    public void MakeEarthWindow(int windowID)
    {
        // use the spike function to add the spikes
        AddSpikes(windowRectEarth.width);

        //add a fancy top using the fancy top function
        FancyTop(windowRectEarth.width);

        GUILayout.Space(8);
        GUILayout.Label("Tutorial - Earth");
        GUILayout.Label("", "Divider");
        GUILayout.Label("To summon earth:", "LightText");
        GUILayout.Space(8);
        GUILayout.Label("Raise your arms so that your arms form a goal post. Raise your arms so that your elbows are close to your ears. Lower arms back to 90 degrees, and then you may put your arms down", "LightText");
        GUILayout.Space(8);
        GUILayout.Label("", "Divider");
        GUILayout.Label("Please read through the source of this script to see", "PlainText");

        // add a wax seal at the bottom of the window

        MP1Build(windowRectEarth.width, windowRectEarth.height);
        //WaxSeal(windowRectEarth.width, windowRectEarth.height);

        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

    void OnGUI()
    {
        GUI.skin = necromancer;
        
        if (earthTutorialWindow)// && Application.loadedLevel == "Scenes/TutorialShootingRock")
            windowRectEarth = GUI.Window(3, windowRectEarth, MakeEarthWindow, "");
        //now adjust to the group. (0,0) is the topleft corner of the group.
        GUI.BeginGroup(new Rect(0, 0, 100, 100));
        // End the group we started above. This is very important to remember!
        GUI.EndGroup();
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name == "Scenes/TutorialShootingRock")
        {
            earthTutorialWindow = true;
        }
        else
        {
            earthTutorialWindow = false;
        }
    }
}
