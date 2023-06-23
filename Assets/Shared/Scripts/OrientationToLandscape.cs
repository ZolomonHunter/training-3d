using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationToLandscape : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        if (Screen.orientation != ScreenOrientation.LandscapeLeft)
            Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
}
