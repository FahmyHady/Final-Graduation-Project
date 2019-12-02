using UnityEngine;
using System.Collections;
using ArabicSupport;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class FixTextInput : MonoBehaviour {

	public bool tashkeel = true;
	public bool hinduNumbers = true;
	//public Canvas parentCanvas;
	private bool doOnce = true,skipFirstTime = true;
	private int capturedFont;
	private Text uitext;

	// Use this for initialization
	void Start () {
		uitext = GetComponent<Text>();
        reshapeText();

    }


	public void reshapeText(){
		string reshapedString = "";
		string[] words = uitext.text.Split(' ');
		string englishWords = "";
		for(int i = 0; i < words.Length; i++){
			if(IsEnglish(words[i])){
				words[i] = ArabicFixer.Fix(words[i], tashkeel, hinduNumbers);
				reshapedString = " "+words[i]+reshapedString;
			}else{
				englishWords += " "+words[i];
			}
			if(englishWords.Equals("") == false && ((i+1 < words.Length && IsEnglish(words[i+1])) || i == words.Length-1)){
				reshapedString = " "+englishWords+reshapedString;
				englishWords = "";
			}
		}
		uitext.text = reshapedString;
		
		uitext.text = FormatGuiTextArea(uitext);
		uitext.resizeTextForBestFit = false;
		uitext.fontSize = capturedFont;
		uitext.horizontalOverflow = HorizontalWrapMode.Overflow;
	}

	public static bool IsEnglish(string text) {
		char[] glyphs = text.ToCharArray();
		foreach (char glyph in glyphs) {
			if (glyph >= 0x600 && glyph <= 0x6ff) return true;
			if (glyph >= 0x750 && glyph <= 0x77f) return true;
			if (glyph >= 0xfb50 && glyph <= 0xfc3f) return true;
			if (glyph >= 0xfe70 && glyph <= 0xfefc) return true;
		}
		return false;
	}
	
	public String FormatGuiTextArea(Text guiText)
	{
		string[] words = guiText.text.Split(' ');
		string result = "";
		string resch = "";
		float textArea;
		//Array.Reverse (words);
		
		
		if (words [0] == "") {
			List<string> tmp = new List<string>(words);
			tmp.RemoveAt(0);
			words = tmp.ToArray();
		}
		
		// measure it
		textArea = guiText.rectTransform.rect.width;
		/*if(parentCanvas.GetComponent<CanvasScaler>().matchWidthOrHeight> 0){
			textArea = textArea*(Screen.height/parentCanvas.GetComponent<CanvasScaler>().referenceResolution.y);
		}else{
			textArea = textArea*(Screen.width/parentCanvas.GetComponent<CanvasScaler>().referenceResolution.x);
		}*/
		
		// if it didn't fit, put word onto next line, otherwise keep it
		GUIStyle myStyle = new GUIStyle();
		myStyle.font = guiText.font;
		if(guiText.resizeTextForBestFit){
			capturedFont = (int)(guiText.cachedTextGenerator.fontSizeUsedForBestFit);
			myStyle.fontSize = (int)(guiText.cachedTextGenerator.fontSizeUsedForBestFit*1.1f);
		}else{
			capturedFont = (int)(guiText.fontSize);
			myStyle.fontSize = (int)(guiText.fontSize*1.1f);
		}
		
		for(int i = words.Length-1; i >= 0 ; i--)
		{
			resch = " " + words[i] +resch;
			
			Vector2 size1 = myStyle.CalcSize( new GUIContent( resch ) );
			
			if(size1.x >= textArea)
			{
				if(result.Equals("")){
					result = resch ;
				}else{
					result = result + "\n" + resch ;
				}
				resch = "";
			}
		}
		if (result.Equals ("")) {
			result = resch ;
		} else {
			result = result + "\n" + resch ;
		}
		
		return result;
	}

}
