using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabManager : MonoBehaviour
{
    [SerializeField] private string displayNameInput;
    [SerializeField] private string usernameInput;
    [SerializeField] private string emailInput;
    [SerializeField] private string passwordInput;


    private void Start()
    {
        Login();
    }

    private void Register()
    {
        RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest
        {
            //Display name can be anything
            DisplayName = displayNameInput,
            //Username is case sensitive and gets a # ID (like discord) (10 or 100 different combinations)
            //Different id number = different color for their id 
            Username = usernameInput,
            //Have email verification before registering
            Email = emailInput,
            //Have checks for safe passwords
            Password = passwordInput
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, RegisterSuccess, RegisterFail);
    }

    private void RegisterSuccess(RegisterPlayFabUserResult result)
    {

    }
    private void RegisterFail(PlayFabError error)
    {

    }

    private void Login()
    {
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, LoginSuccess, LoginFail);
    }

    private void LoginSuccess(LoginResult result)
    {
        if (result.NewlyCreated)
            Debug.Log("Account Created");
        else
            Debug.Log("Login Successful");
    }

    public void LoginFail(PlayFabError error)
    {
        Debug.Log("Login Error");
        Debug.Log(error.GenerateErrorReport());
    }


}
