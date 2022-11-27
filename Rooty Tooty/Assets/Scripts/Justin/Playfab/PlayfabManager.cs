using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayfabManager : MonoBehaviour
{
    [Header("Login Account")]
    [SerializeField] private TMP_InputField login_EmailInput = null;
    [SerializeField] private TMP_InputField login_PswdInput = null;
    [SerializeField] private TextMeshProUGUI login_EmailError = null;
    [SerializeField] private TextMeshProUGUI login_PswdError = null;

    private const string otherLetters = "ßàÁâãóôþüúðæåïçèõöÿýòäœêëìíøùîûñé";

    private void Awake()
    {
        //Playfab: username should be 3-20 characters
        

        //Playfab: pswd should be 6-100 characters
        login_PswdInput.characterLimit = 100;

        //Make sure pswd display is hidden
        login_PswdInput.contentType = TMP_InputField.ContentType.Password;

        //Most emails shouldn't be more than 150 characters...
        login_EmailInput.characterLimit = 150;

    }


    #region Valid Information

    private enum errorNum
    {
        no_error = 0,
        usernameInvalidSize,
        usernameInvalidFirstLastChar,
        usernameInvalidChar,
        usernameNoDoubleSpacePeriod,

        pswdInvalidSize,
        pswdMustHaveChar,
        pswdMustMatch,

        emailInvalidSize,
        emailInvalidFormat,

        loginInvalidEmailUsername,

        playfabError,
    }
    private void ManageError(errorNum errorCode, string messge = "")
    {
        switch (errorCode)
        {
            case errorNum.no_error:
                return;
            case errorNum.usernameInvalidSize: 
                return;
            default:
                return;
        }
    }

    //Returns 0 if username is valid
    private int CheckUsername(string user)
    {
        if (string.IsNullOrEmpty(user))
            return (int)errorNum.usernameInvalidSize;
        if (user.Length < 3 || user.Length > 20)
            return (int)errorNum.usernameInvalidSize;

        if (!Char.IsLetter(user[0]))
            return (int)errorNum.usernameInvalidFirstLastChar;
        if (!Char.IsLetterOrDigit(user[user.Length - 1]))
            return (int)errorNum.usernameInvalidFirstLastChar;

        bool prevSpace = false;
        bool prevPeriod = false;

        string allowedChar = "$_" + otherLetters;

        foreach(char c in user)
        {
            if (c == ' ')
            {
                if (prevSpace)
                    return (int)errorNum.usernameNoDoubleSpacePeriod;
                prevSpace = true;
                prevPeriod = false;
                continue;
            }
            else if(c == '.')
            {
                if(prevPeriod)
                    return (int)errorNum.usernameNoDoubleSpacePeriod;
                prevPeriod = true;
                prevSpace = false;
            }
            else
                prevSpace = prevPeriod = false;

            if (Char.IsLetterOrDigit(c))
                continue;
            if (allowedChar.Contains(c))
                continue;

            return (int)errorNum.usernameInvalidChar;
        }

        return (int)errorNum.no_error;
    }

    //Returns 0 if email is valid
    private int CheckEmail(string email)
    {
        if (email.Length < 5)
            return (int)errorNum.emailInvalidSize;

        string[] info = email.Split('@');
        if (info.Length != 2)
            return (int)errorNum.emailInvalidFormat;

        if(info[1].Split('.').Length == 1)
            return (int)errorNum.emailInvalidFormat;

        char last = ' ';
        string acceptableName = "-_." + otherLetters;
        string acceptableDomain = "-." + otherLetters;

        if (acceptableName.Contains(info[0][0]) || acceptableName.Contains(info[0][info[0].Length - 1]))
            return (int)errorNum.emailInvalidFormat;

        if (acceptableDomain.Contains(info[1][0]) || acceptableDomain.Contains(info[0][info[0].Length - 1]))
            return (int)errorNum.emailInvalidFormat;

        foreach (char c in info[0])
        {
            if (acceptableName.Contains(c))
                if(c == last)
                    return (int)errorNum.emailInvalidFormat;
            else
                if (!char.IsLetterOrDigit(c))
                    return (int)errorNum.emailInvalidFormat;

            last = c;
        }

        last = ' ';
        foreach (char c in info[1])
        {
            if(acceptableDomain.Contains(c))
                if(c == last)
                    return (int)errorNum.emailInvalidFormat;
            else
                if (!char.IsLetterOrDigit(c))
                    return (int)errorNum.emailInvalidFormat;
            last = c;
        }

        return (int)errorNum.no_error;
    }

    //Returns 0 if the password is valid
    private int CheckPassword(string pswd)
    {
        if (string.IsNullOrEmpty(pswd))
            return (int)errorNum.pswdInvalidSize;
        if (pswd.Length < 6 || pswd.Length > 100)
            return (int)errorNum.pswdInvalidSize;

        bool hasDigit, hasChar, hasSpec;
        hasDigit = hasChar = hasSpec = false;

        foreach (char c in pswd)
        {
            if (char.IsDigit(c))
                hasDigit = true;
            else if (char.IsLetter(c))
                hasChar = true;
            else
                hasSpec = true;
        }

        if (hasDigit && hasChar && hasSpec)
            return (int)errorNum.no_error;
        else
            return (int)errorNum.pswdMustHaveChar;
    }

    //Returns 0 if the passwords matches
    private int PasswordMatch(string pswd1, string pswd2)
    {
        if(pswd1.Equals(pswd2))
            return (int)errorNum.no_error;

        return (int)errorNum.pswdMustMatch;
    }

    #endregion

    #region Log in
    private void AttemptLogin()
    {
        string email = login_EmailInput.text;
        string pswd = login_PswdInput.text;



        int loginError = CheckEmail(email);
        int pswdError = CheckPassword(pswd);

        if(loginError != 0 || pswdError != 0)
        {
            if (loginError != 0)
                ManageError((errorNum)loginError);
            if (pswdError != 0)
                ManageError((errorNum)pswdError);

            return;
        }

        //Deal with error in function and delete textboxes if success (then go to welcome screen)
        LoginWithEmailAddressRequest request = new LoginWithEmailAddressRequest
        {
            Email = login_EmailInput.text,
            Password = login_PswdInput.text
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, 
            result => {
                Debug.Log("Login Successful");
                LeaveLogin();
            },
            error => {
                ManageError(errorNum.playfabError, error.GenerateErrorReport());
            });


    }

    private void LeaveLogin()
    {
        login_EmailInput.text = "";
        login_PswdInput.text = "";
        login_EmailError.text = "";
        login_PswdError.text = "";
        login_EmailError.gameObject.SetActive(false);
        login_PswdError.gameObject.SetActive(false);
        
    }

    #endregion

    /*

    #region Register Account
    private void Register()
    {
        RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest
        {
            //Have email verification before registering
            Email = emailInput.text,
            //Have checks for safe passwords
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, RegisterSuccess, PlayFabError);
    }

    private void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Registered and Logged in!");
    }
    #endregion

    #region Reset Password

    private void ResetPassword()
    {
        SendAccountRecoveryEmailRequest request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInput.text,
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
            EmailAddress = emailInput.text
        };

        PlayFabClientAPI.AddOrUpdateContactEmail(request, UpdateEmailSuccess, PlayFabError);
    }

    private void UpdateEmailSuccess(AddOrUpdateContactEmailResult result)
    {
        Debug.Log("Email Reset email sent!");
    }

    #endregion

    */
    private void PlayFabError(PlayFabError error)
    {
        Debug.Log(error.ErrorMessage);
    }
}
