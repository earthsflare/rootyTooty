using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabManager : MonoBehaviour
{
    private void Start()
    {
        Login();
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
