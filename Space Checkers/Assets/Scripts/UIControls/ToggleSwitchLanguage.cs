using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSwitchLanguage : MonoBehaviour
{
    Toggle toggleLanguage;

	public Text firstLabel;
	public Color activeColor;
	public GameObject secondState;
	public Text secondLabel;
	public Color disactiveColor;

	// Use this for initialization
	void Start () {
		toggleLanguage = GetComponent<Toggle>();
        //Add listener for when the state of the Toggle changes, to take action
        toggleLanguage.onValueChanged.AddListener(delegate {
                ToggleValueChanged(toggleLanguage);
            });
		if (!toggleLanguage.isOn) 
		{
			firstLabel.color = disactiveColor;
			secondLabel.color = activeColor;
			secondState.SetActive(true);
		}
        Debug.Log(toggleLanguage.isOn);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Output the new state of the Toggle into Text
    void ToggleValueChanged(Toggle change)
    {
        Debug.Log(toggleLanguage.isOn);
		if (toggleLanguage.isOn)
		{
			firstLabel.color = activeColor;
			secondLabel.color = disactiveColor;
			secondState.SetActive(false);
		}
		else
		{
			firstLabel.color = disactiveColor;
			secondLabel.color = activeColor;
			secondState.SetActive(true);
		}
    }
}

