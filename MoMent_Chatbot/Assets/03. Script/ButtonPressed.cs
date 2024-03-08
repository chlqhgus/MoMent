using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.
using OpenAI;

// required interface when using the OnPointerDown method.
public class ButtonPressed : MonoBehaviour, IPointerDownHandler
{
    private Recording recordingScript;

    void Start()
    {
        recordingScript = new Recording();
        recordingScript.startRecording();
    }
    
        //Do this when the mouse is clicked over the selectable object this script is attached to.
	public void OnPointerDown (PointerEventData eventData) 
	{
		Debug.Log (this.gameObject.name + " Was Clicked.");

	}

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Pointer Up", gameObject);
        recordingScript.stopRecording();
    }
}