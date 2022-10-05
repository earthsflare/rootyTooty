using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabManager : MonoBehaviour
{
    [SerializeField] private string displayNameInput;
    [SerializeField] private string emailInput;
    [SerializeField] private string passwordInput;


    private void Start()
    {
        Login();
    }

    #region Valid Information

    //Returns true if the password is valid
    private bool CheckPassword(string pswd)
    {
        if (string.IsNullOrEmpty(pswd))
            return false;
        if (pswd.Length <= 4)
            return false;

        bool hasDigit = false;
        bool hasChar = false;

        foreach (char c in pswd)
        {
            if (char.IsDigit(c))
                hasDigit = true;
            if (char.IsLetter(c))
                hasChar = true;
        }


        return (hasDigit && hasChar);
    }

    private bool CheckUsername(string displayName)
    {
        return false;
    }

    #endregion

    #region Register Account
    private void Register()
    {
        RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest
        {
            //Username is case sensitive and gets a # ID (like discord) (10 or 100 different combinations)
            //Different id number = different color for their id 
            //Display name can be anything
            DisplayName = displayNameInput,
            //Have email verification before registering
            Email = emailInput,
            //Have checks for safe passwords
            Password = passwordInput
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, RegisterSuccess, PlayFabError);
    }

    private void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Registered and Logged in!");
    }
    #endregion

    #region Log in
    private void Login()
    {
        LoginWithEmailAddressRequest request = new LoginWithEmailAddressRequest
        {
            Email = emailInput,
            Password = passwordInput
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, LoginSuccess, PlayFabError);
    }

    private void LoginSuccess(LoginResult result)
    {
        if (result.NewlyCreated)
            Debug.Log("Account Created");
        else
            Debug.Log("Login Successful");
    }

    #endregion

    #region Reset Password

    private void ResetPassword()
    {
        SendAccountRecoveryEmailRequest request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInput,
            TitleId = "A6F68"
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(request, PasswordResetSuccess, PlayFabError);
    }

    private void PasswordResetSuccess(SendAccountRecoveryEmailResult result)
    {
        Debug.Log("Passward Reset email sent!");
    }

    #endregion

    #region Update E-mail
    private void UpdateEmail()
    {
        AddOrUpdateContactEmailRequest request = new AddOrUpdateContactEmailRequest
        {
            EmailAddress = emailInput
        };

        PlayFabClientAPI.AddOrUpdateContactEmail(request, UpdateEmailSuccess, PlayFabError);
    }

    private void UpdateEmailSuccess(AddOrUpdateContactEmailResult result)
    {
        Debug.Log("Email Reset email sent!");
    }

    #endregion

    private void PlayFabError(PlayFabError error)
    {
        Debug.Log(error.ErrorMessage);
    }
}
