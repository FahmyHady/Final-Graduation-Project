using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MBInputHelper : MonoBehaviour {
	
	string lastText = "";
	public Text txtReshape;
	Text currentText;

	// Use this for initialization
	void Start () {
		currentText = GetComponent<Text> ();
	}

	// Update is called once per frame
	void Update () {
		if(currentText != null&& txtReshape!= null && !lastText.Equals(currentText.text)){
			lastText = currentText.text;
			if (ArabicUtilities.hasArabicLetters (lastText)) {
				txtReshape.text = ArabicSupport.ArabicFixer.Fix (lastText);
			} else {
				txtReshape.text = currentText.text;
			}
		}
	}
}