using UnityEngine;
using TMPro;
public class TMPInputFilter : MonoBehaviour
{
   public TMP_InputField tmpInputField;
   public TMP_InputField inputField;
   private char[] disabledChars = { '/', '.', ':', ';', '[', '@' };
   void Start()
   {
       tmpInputField.onValidateInput += ValidateInput;

       inputField.ActivateInputField();
   }
   private char ValidateInput(string text, int charIndex, char addedChar)
   {
       foreach (char c in disabledChars)
       {
           if (c == addedChar)
           {
               Debug.Log($"'{c}' は無効化されています");
               return '\0';
           }
       }
       return addedChar;
   }
}