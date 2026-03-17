using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_Ststem : MonoBehaviour
{
    public OptionController panelOptions;
    // Start is called before the first frame update
    void Start()
    {
        panelOptions = GameObject.FindGameObjectWithTag("options").GetComponent<OptionController>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        { ShowOption(); }
    }

    public void ShowOption()
    { panelOptions.optionScreen.SetActive(true); }

    public void HideOption()
    {
        panelOptions.optionScreen.SetActive(false); 
    }
}
