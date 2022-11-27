using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestingPlayfab : MonoBehaviour
{
    [SerializeField] private string email = "jusboy123123@gmail.com";


    [SerializeField] private string titleID = "A6F68";

    [SerializeField] private TMP_InputField exampleText;

    private void Start()
    {
        /*
        RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest
        {
            //Have email verification before registering
            Email = "jusboy123123@gmail.com",
            //Have checks for safe passwords
            Password = "abc123",
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, RegisterSuccess, PlayFabError);
        */
    }
    void Update()
    {
        if(exampleText != null)
            email = exampleText.text;


        if (Input.GetKeyDown(KeyCode.Space))
        {
            SendAccountRecoveryEmailRequest request = new SendAccountRecoveryEmailRequest
            {
                Email = email,
                TitleId = titleID,
            };

            PlayFabClientAPI.SendAccountRecoveryEmail(request, PasswordResetSuccess, PlayFabError);
        }
    }

    private void PasswordResetSuccess(SendAccountRecoveryEmailResult result)
    {
        Debug.Log("Passward Reset email sent!");
    }
    private void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Registered and Logged in!");
    }
    private void PlayFabError(PlayFabError error)
    {
        Debug.Log(error.ErrorMessage);
    }
}
