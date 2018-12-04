using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour
{
    Toggle toggleView;

	public Text firstLabel;
	public Color activeColor;
	public GameObject secondState;
	public Text secondLabel;
	public Color disactiveColor;

	// Use this for initialization
	void Start ()
	{
		toggleView = GetComponent<Toggle>();
        //Add listener for when the state of the Toggle changes, to take action
        toggleView.onValueChanged.AddListener(delegate {
                ToggleValueChanged(toggleView);
            });
		if (!toggleView.isOn) 
		{
			firstLabel.color = disactiveColor;
			secondLabel.color = activeColor;
			secondState.SetActive(true);
		}
        Debug.Log(toggleView.isOn);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	//Output the new state of the Toggle into Text
    void ToggleValueChanged(Toggle change)
    {
        Debug.Log(toggleView.isOn);
		if (toggleView.isOn)
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

