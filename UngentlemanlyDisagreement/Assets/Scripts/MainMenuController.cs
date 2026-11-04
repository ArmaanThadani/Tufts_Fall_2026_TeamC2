
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
	public AudioSource soundFX1;
	public AudioSource soundFX2;
	public AudioSource soundFX3;
	public AudioSource soundFX4;


	void Start(){
		//soundFX1 = GetCOmponent<AudioSource>();
	}
		

    public void StartGame()
    {
        StartCoroutine(PlaySound(soundFX1, "startgame"));
    }

	public void Settings()
    {
        StartCoroutine(PlaySound(soundFX2, "settings"));
    }

	public void Credits()
    {
        StartCoroutine(PlaySound(soundFX3, "credits"));
    }


    public void QuitGame()
    {
       StartCoroutine(PlaySound(soundFX4, "quit"));
    }



	IEnumerator PlaySound(AudioSource soundFX, string buttonName){
		
		soundFX.Play();
		yield return new WaitForSeconds(0.3f);

		if (buttonName == "startgame"){
			Debug.Log("MAIN MENU START BUTTON CLICKED!");
        	UnityEngine.SceneManagement.SceneManager.LoadScene("ControllerLobby");
		}
		else if (buttonName == "settings"){
			 	Debug.Log("hit settinbs button");
		}
		else if (buttonName == "credits"){
			SceneManager.LoadScene("Credits");
		}
		else if (buttonName == "quit"){
			 Application.Quit();	
		}

	}




}
