using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    public TCE tce;

    public void ChangeTheScene(string sceneName)
    {
    tce._escenaSiguiente = sceneName;
        tce._pasarEscena = true;
    }
    
}
