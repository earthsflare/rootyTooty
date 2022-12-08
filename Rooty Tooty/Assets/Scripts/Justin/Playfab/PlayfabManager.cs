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
    private static PlayfabManager instance;
    public static PlayfabManager Instance { get => instance; }

    private string userEmail = "";
    private string userPswd = "";
    private string userName = "";
    private bool? accountVerified = null;
    public bool passwordResetted = false; //serves no other purpose than as a trigger for Resetting Password
    public string UserEmail { get => userEmail; }
    public bool? AccountVerified { get => accountVerified; }

    private const string otherLetters = "ßàÁâãóôþüúðæåïçèõöÿýòäœêëìíøùîûñé";
    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    #region Error Report
    private enum errorNum
    {
        no_error = 0,

        usernameInvalidSize,
        usernameInvalidChar,

        pswdInvalidSize,
        pswdMustHaveChar,
        pswdMustMatch,

        emailInvalidFormat,

        UnexpectedError,
        PlayfabError,
    }
    private void ManageError(errorNum errorCode, TextMeshProUGUI textBox, string messge = "")
    {
        Debug.Log("Error: " + errorCode);    

        switch (errorCode)
        {
            case errorNum.no_error:
                textBox.text = "";
                textBox.gameObject.SetActive(false);
                return;
            
            //Username related errors
            case errorNum.usernameInvalidSize:
                Debug.Log("Username must be between 3 to 20 characters long.");
                textBox.text = "Username must be between 3 to 20 characters long.";
                break;
            case errorNum.usernameInvalidChar:
                Debug.Log("Username must contain only letters or numbers.");
                textBox.text = "Username must contain only letters or numbers.";
                break;

            //Password related errors
            case errorNum.pswdInvalidSize:
                Debug.Log("Password must be at least 6 characters long.");
                textBox.text = "Password must be at least 6 characters long.";
                break;
            case errorNum.pswdMustHaveChar:
                Debug.Log("Password must contain a letter, a number, and a symbol.");
                textBox.text = "Password must contain a letter, a number, and a symbol.";
                break;
            case errorNum.pswdMustMatch:
                Debug.Log("Passwords do not match.");
                textBox.text = "Passwords do not match.";
                break;

            //Email related errors
            case errorNum.emailInvalidFormat:
                Debug.Log("This is not a valid email for this game.");
                textBox.text = "This is not a valid email for this game.";
                break;

            case errorNum.PlayfabError:
                Debug.Log("Playfab Error: " + messge);
                textBox.text = "Playfab Error: " + messge;
                break;
            case errorNum.UnexpectedError:
                Debug.Log("An unexpected error has occured: " + messge);
                textBox.text = "An unexpected error has occured.";
                break;
            default:
                break;
        }

        if(textBox != null)
            if(textBox.text != "")
                textBox.gameObject.SetActive(true);
    }
    #endregion

    #region Validate Information
    //Returns 0 if username is valid (Username must be 3-20, and cannot contain any special character)
    private int CheckUsername(string user)
    {
        if (string.IsNullOrEmpty(user))
            return (int)errorNum.usernameInvalidSize;
        if (user.Length < 3 || user.Length > 20)
            return (int)errorNum.usernameInvalidSize;

        foreach(char c in user)
        {
            if (Char.IsLetterOrDigit(c))
                continue;

            return (int)errorNum.usernameInvalidChar;
        }

        return (int)errorNum.no_error;
    }

    //Returns 0 if email is valid
    private int CheckEmail(string email)
    {
        if (email.Length < 5)
            return (int)errorNum.emailInvalidFormat;

        string[] info = email.Split('@');
        if (info.Length != 2)
            return (int)errorNum.emailInvalidFormat;

        if(info[1].Split('.').Length == 1)
            return (int)errorNum.emailInvalidFormat;

        char last = ' ';
        string acceptableName = "-_.";
        string acceptableDomain = "-.";

        if (acceptableName.Contains(info[0][0]) || acceptableName.Contains(info[0][info[0].Length - 1]))
            return (int)errorNum.emailInvalidFormat;

        if (acceptableDomain.Contains(info[1][0]) || acceptableDomain.Contains(info[0][info[0].Length - 1]))
            return (int)errorNum.emailInvalidFormat;

        foreach (char c in info[0])
        {
            if (acceptableName.Contains(c))
            {
                if (c == last)
                    return (int)errorNum.emailInvalidFormat;
            }
            else if(!char.IsLetterOrDigit(c) && !otherLetters.Contains(c))
                    return (int)errorNum.emailInvalidFormat;

            last = c;
        }

        last = ' ';
        foreach (char c in info[1])
        {
            if (acceptableDomain.Contains(c))
            {
                if (c == last)
                    return (int)errorNum.emailInvalidFormat;
            }
            else if (!char.IsLetterOrDigit(c) && !otherLetters.Contains(c))
                    return (int)errorNum.emailInvalidFormat;
            last = c;
        }

        return (int)errorNum.no_error;
    }

    //Returns 0 if the password is valid (Password must be at least 6 characters long. it must contain a letter, a number, and a special character)
    private int CheckPassword(string pswd)
    {
        try
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
        catch(Exception ex)
        {
            return (int)errorNum.UnexpectedError;
        }
    }

    //Returns 0 if the passwords matches
    private int CheckMatchPassword(string pswd1, string pswd2)
    {
        try
        {
            if (pswd1.Equals(pswd2))
                return (int)errorNum.no_error;
        }
        catch (Exception ex) { }

        return (int)errorNum.pswdMustMatch;
    }
    #endregion

    #region Log in
    public bool Login(string email, string pswd, TextMeshProUGUI eError, TextMeshProUGUI pError, TextMeshProUGUI uError)
    {
        //login account
        try
        {
            ManageError(errorNum.no_error, uError);
            int loginError = CheckEmail(email);
            int pswdError = CheckPassword(pswd);

            ManageError((errorNum)loginError, eError);
            ManageError((errorNum)pswdError, pError);

            if (loginError != 0 || pswdError != 0)
                return false;

            //Deal with error in function and delete textboxes if success (then go to welcome screen)
            LoginWithEmailAddressRequest request = new LoginWithEmailAddressRequest
            {
                Email = email,
                Password = pswd,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true,
                    ProfileConstraints = new PlayerProfileViewConstraints()
                    {
                        ShowContactEmailAddresses = true,
                    },

                },
                TitleId = "A6F68",
            };

            bool success = false;

            PlayFabClientAPI.LoginWithEmailAddress(request,
                result => {
                    //LeaveLogin();
                    success = true;

                    //Setup login invormation for future saving
                    userEmail = email;
                    userPswd = pswd;

                    //Get status of email verification and sent a verification email if needed
                    List<ContactEmailInfoModel> contactEmail = result.InfoResultPayload.PlayerProfile.ContactEmailAddresses;
                    if(contactEmail.Count < 0)
                        SendVerificationEmail(userEmail, uError);
                    else if (contactEmail[0].VerificationStatus != EmailVerificationStatus.Confirmed)
                        SendVerificationEmail(userEmail, uError);
                    else
                        accountVerified = true;

                    Debug.Log("Login Successful");
                },
                error => {
                    ManageError(errorNum.PlayfabError, uError,error.GenerateErrorReport());
                });
            return success;
        }
        catch (Exception ex) { ManageError(errorNum.UnexpectedError, uError, ex.Message); }

        return false;
    }
    #endregion

    #region Register Account
    public bool RegisterAccount(string email, string user, string pswd, string confirmPswd, 
        TextMeshProUGUI eError, TextMeshProUGUI nError, TextMeshProUGUI pError, TextMeshProUGUI cError, TextMeshProUGUI uError)
    {
        try
        {
            ManageError(errorNum.no_error, uError);

            int emailError = CheckEmail(email);
            int userError = CheckUsername(user);
            int pswdError = CheckPassword(pswd);
            int confirmError = CheckMatchPassword(pswd, confirmPswd);

            ManageError((errorNum)emailError, eError);
            ManageError((errorNum)userError, nError);
            ManageError((errorNum)pswdError, pError);
            ManageError((errorNum)confirmError, cError);

            if (emailError != 0 || pswdError != 0 || userError != 0 || confirmError != 0)
                return false;

            //Deal with error in function and delete textboxes if success (then go to welcome screen)
            RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest
            {
                Username = user,
                Email = email,
                Password = pswd,
            };

            bool success = false;
            PlayFabClientAPI.RegisterPlayFabUser(request,
                result => {
                    Debug.Log("Register Successful");
                    Login(email, pswd, null, null, null);
                    success = true;
                },
                error => {
                    ManageError(errorNum.PlayfabError, uError, error.GenerateErrorReport());
                });
            return success;
        }
        catch (Exception ex) { ManageError(errorNum.UnexpectedError, uError, ex.Message); }
        return false;
    }

    //Send a verification email to the user (after logging in)
    public bool SendVerificationEmail(string email, TextMeshProUGUI uError)
    {
        try
        {
            ManageError(errorNum.no_error, uError);

            AddOrUpdateContactEmailRequest verify = new AddOrUpdateContactEmailRequest
            {
                EmailAddress = email,
            };

            bool succeeded = false;

            PlayFabClientAPI.AddOrUpdateContactEmail(verify,
                result =>
                {
                    succeeded = true;
                    Debug.Log("Verification Email Sent");
                },
                error =>
                {
                    ManageError(errorNum.PlayfabError, uError, error.GenerateErrorReport());
                });
            return succeeded;
        }
        catch (Exception ex) { ManageError(errorNum.UnexpectedError, uError, ex.Message); }

        return false;
    }
    #endregion

    #region Reset Info
    public bool ResetPassword(string email, TextMeshProUGUI eError, TextMeshProUGUI uError)
    {
        try
        {
            SendAccountRecoveryEmailRequest request = new SendAccountRecoveryEmailRequest
            {
                Email = email,
                TitleId = "A6F68",
            };

            bool success = false;
            PlayFabClientAPI.SendAccountRecoveryEmail(request,
                result =>
                {
                    success = true;
                    Debug.Log("Password Reset Email Sent");
                },
                error => { ManageError(errorNum.PlayfabError, uError, error.GenerateErrorReport()); });

            return success;
        }
        catch(Exception ex) { ManageError(errorNum.UnexpectedError, uError, ex.Message); }

        return false;
    }
    public void LogOut()
    {
        userEmail = "";
        userPswd = "";
        userName = "";
        accountVerified = null;
    }
    #endregion

}
