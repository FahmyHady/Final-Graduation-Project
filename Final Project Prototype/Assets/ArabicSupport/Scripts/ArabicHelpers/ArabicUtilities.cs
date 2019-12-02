using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;

public class ArabicUtilities {
	
	/**
	 * the path of teh fonts file must be under assets folder
	 */
	static Font face ;
	
	
	
	/**
	 * Helper function is to check if the character passed, is Arabic
	 * @param target The Character to check Against
	 * @return true if the Character is Arabic letter, otherwise returns false
	 */
	private static bool isArabicCharacter(char target){
		
		//Iterate over the 36 Characters in ARABIC_GLPHIES Matrix
		for(int i = 0; i < ArabicReshaper.ARABIC_GLPHIES.Length;i++){
			//Check if the target Character exist in ARABIC_GLPHIES Matrix
			if(ArabicReshaper.ARABIC_GLPHIES[i][0]==target)
				return true;
		}
		
		for(int i = 0; i < ArabicReshaper.HARAKATE.Length;i++){
			//Check if the target Character exist in ARABIC_GLPHIES Matrix
			if(ArabicReshaper.HARAKATE[i]==target)
				return true;
		}
		
		return false;
	}
    private static bool isArabicCharacterAfterReshape(char target)
    {

        //Iterate over the 36 Characters in ARABIC_GLPHIES Matrix
        for (int i = 0; i < ArabicReshaper.ARABIC_GLPHIES.Length; i++)
        {
            //Check if the target Character exist in ARABIC_GLPHIES Matrix
            if (ArabicReshaper.ARABIC_GLPHIES[i][0] == target)
                return true;
        }

        for (int i = 0; i < ArabicReshaper.HARAKATE.Length; i++)
        {
            //Check if the target Character exist in ARABIC_GLPHIES Matrix
            if (ArabicReshaper.HARAKATE[i] == target)
                return true;
        }

        return false;
    }
    /**
	 * Helper function to split Sentence By Space
	 * @param sentence the Sentence to Split into Array of Words
	 * @return Array Of words
	 */
    static string[] getWords(string sentence){
		if (sentence != null) {
			return sentence.Split(new char[]{' '});
		} else {
			return new string[0];
		}
	}
	
	/**
	 * Helper function to check if the word has Arabic Letters
	 * @param word The to check Against
	 * @return true if the word has Arabic letters, false otherwise
	 */
	public static bool hasArabicLetters(string word){
		
		//Iterate over the word to check all the word's letters
		for(int i=0;i<word.Length;i++){
			
			if(isArabicCharacter(word.ToCharArray()[i]))
				return true;
		}
		return false;
	}

	public static bool hasEnglishLetters(string word){

		bool foundEnglish = false;
		//Iterate over the word to check all the word's letters
		for(int i=0;i<word.Length;i++){
			if(isArabicCharacter(word.ToCharArray()[i]) == false)
				foundEnglish = true;
		}
		return foundEnglish;
	}
	
	/**
	 * Helper function to check if the word is all Arabic Word
	 * @param word The word to check against
	 * @return true if the word is Arabic Word, false otherwise
	 */
	public static bool isArabicWord(string word){
		//Iterate over the Word
		for(int i=0;i<word.Length;i++){
			if(!isArabicCharacter(word.ToCharArray()[i]))
				return false;
		}
		return true;
	}
	
	/**
	 * Helper function to split the Mixed Word into words with only Arabic, and English Words
	 * @param word The Mixed Word
	 * @return The Array of the Words of each Word may exist inside that word
	 */
	private static string[] getWordsFromMixedWord(string word){
		
		//The return result of words
		List<string> finalWords = new List<string>();
		
		//Temp word to hold the current word
		string tempWord="";
		
		//Iterate over the Word Length
		for(int i=0;i<word.Length;i++){
			
			//Check if the Character is Arabic Character
			if(isArabicCharacter(word.ToCharArray()[i])){
				
				//Check if the tempWord is not empty, and what left in tempWord is not Arabic Word
				if(!tempWord.Equals("") && !isArabicWord(tempWord)) {
					
					//add the Word into the Array
					finalWords.Add(tempWord);
					
					//initiate the tempWord again
					tempWord=""+word.ToCharArray()[i];
					
				}else{
					
					//Not to add the tempWord, but to add the character to the rest of the characters
					tempWord+=word.ToCharArray()[i];
				}
				
			}else{
				
				//Check if the tempWord is not empty, and what left in tempWord is Arabic Word
				if(!tempWord.Equals("") && isArabicWord(tempWord)){
					
					//add the Word into the Array
					finalWords.Add(tempWord);
					
					//initiate the tempWord again
					tempWord=""+word.ToCharArray()[i];
					
				}else{
					
					//Not to add the tempWord, but to add the character to the rest of the characters
					tempWord+=word.ToCharArray()[i];
				}
			} 
		}
		
		// add remaining tempWord to finalWords
		if (!tempWord.Equals("")) {
			finalWords.Add(tempWord);
		}
		
		string[] theWords=new string[finalWords.Count];
		theWords = finalWords.ToArray();
		
		return theWords;
	}
	
	public static string reshape(string allText) {
		if (allText != null) {
			StringBuilder result = new StringBuilder();
			string[] sentences = allText.Split(new char[]{'\n'});
			for (int i = 0; i < sentences.Length; i++) {
				result.Append(reshapeSentence(sentences[i]));
				// don't append the line feed to the final sentence
				if (i < sentences.Length - 1) {
					result.Append("\n");
				}
			}
			return result.ToString();
		} else {
			return null;
		}
		
	}
	/**
	 * The Main Reshaping Function to be Used in Android Program
	 * @param allText The text to be Reshaped
	 * @return the Reshaped Text
	 */
	public static string reshapeSentence(string sentence){
		//get the Words from the Text
		string[] words=getWords(sentence);
		
		//prepare the Reshaped Text
		StringBuilder reshapedText=new StringBuilder("");
		
		//Iterate over the Words
		for(int i=0;i<words.Length;i++){
			
			//Check if the Word has Arabic Letters
			if(hasArabicLetters(words[i])){
				
				//Check if the Whole word is Arabic
				if(isArabicWord(words[i])){
					
					//Initiate the ArabicReshaper functionality
					ArabicReshaper arabicReshaper = new ArabicReshaper(words[i]);
					
					
					//Append the Reshaped Arabic Word to the Reshaped Whole Text
					reshapedText.Append(arabicReshaper.getReshapedWord());
				}else{ //The word has Arabic Letters, but its not an Arabic Word, its a mixed word
					
					//Extract words from the words (split Arabic, and English)
					string [] mixedWords=getWordsFromMixedWord(words[i]);
					
					//iterate over mixed Words
					for(int j=0;j<mixedWords.Length;j++){
						
						//Initiate the ArabicReshaper functionality
						ArabicReshaper arabicReshaper=new ArabicReshaper(mixedWords[j]);
						
						//Append the Reshaped Arabic Word to the Reshaped Whole Text
						reshapedText.Append(arabicReshaper.getReshapedWord());
					}
				}	
			}else{//The word doesn't have any Arabic Letters
				
				//Just append the word to the whole reshaped Text
				reshapedText.Append(words[i]);
			}
			
			// don't append the space to the final word
			if (i < words.Length - 1) {
				//Append the space to separate between words
				reshapedText.Append(" ");
			}
		}
		
		//return the final reshaped whole text
		return reshapedText.ToString();
	}
	
	public static Text getArabicEnabledTextView(Text targetTextView) {
		//this is a static for testing!
		targetTextView.font = face;
		targetTextView.alignment = TextAnchor.UpperRight;
		return targetTextView;
	}
}
