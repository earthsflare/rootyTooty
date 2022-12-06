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
    #region Login Screen Object References
    [Header("Login Account")]
    [SerializeField] private TMP_InputField login_EmailInput = null;
    [SerializeField] private TextMeshProUGUI login_EmailError = null;

    [SerializeField] private TMP_InputField login_PswdInput = null;
    [SerializeField] private TextMeshProUGUI login_PswdError = null;
    #endregion
    #region Register Screen Object References
    [Header("Register Account")]
    [SerializeField] private TMP_InputField register_UserInput = null;
    [SerializeField] private TextMeshProUGUI register_UserError = null;

    [SerializeField] private TMP_InputField register_EmailInput = null;
    [SerializeField] private TextMeshProUGUI register_EmailError = null;

    [SerializeField] private TMP_InputField register_PswdInput = null;
    [SerializeField] private TextMeshProUGUI register_PswdError = null;

    [SerializeField] private TMP_InputField register_ConfirmInput = null;
    [SerializeField] private TextMeshProUGUI register_ConfirmError = null;
    #endregion

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

    #region Error Report
    private enum errorNum
    {
        no_error = 0,
        usernameInvalidSize,
        usernameInvalidChar,

        pswdInvalidSize,
        pswdMustHaveChar,
        pswdMustMatch,
        pswdUnknownError,

        emailInvalidSize,
        emailInvalidFormat,

        UnexpectedError,
        PlayfabError,
    }
    private void ManageError(errorNum errorCode, string messge = "")
    {
        Debug.Log("Error: " + errorCode + " " + messge);    

        switch (errorCode)
        {
            case errorNum.no_error:
                return;
            case errorNum.usernameInvalidSize: 
                return;
            case errorNum.PlayfabError:
                Debug.Log(messge);
                return;
            default:
                return;
        }
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
            return (int)errorNum.emailInvalidSize;

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
            return (int)errorNum.pswdUnknownError;
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
    public void AttemptLogin()
    {
        string email = login_EmailInput.text;
        string pswd = login_PswdInput.text;

        Debug.Log(Login(email, pswd));
    }

    private bool Login(string email, string pswd)
    {
        //login account
        try
        {
            int loginError = CheckEmail(email);
            int pswdError = CheckPassword(pswd);

            if (loginError != 0 || pswdError != 0)
            {
                if (loginError != 0)
                    ManageError((errorNum)loginError);
                if (pswdError != 0)
                    ManageError((errorNum)pswdError);

                return false;
            }

            //Deal with error in function and delete textboxes if success (then go to welcome screen)
            LoginWithEmailAddressRequest request = new LoginWithEmailAddressRequest
            {
                Email = email,
                Password = pswd
            };

            bool success = false;

            PlayFabClientAPI.LoginWithEmailAddress(request,
                result => {
                    Debug.Log("Login Successful");
                    //LeaveLogin();
                    success = true;

                    //Get status of email verification and sent a verification email if needed
                    ContactEmailInfoModel contactEmail = result.InfoResultPayload.PlayerProfile.ContactEmailAddresses[0];
                    if (contactEmail == null)
                        SendVerificationEmail(contactEmail.EmailAddress);
                    else if(contactEmail.VerificationStatus != EmailVerificationStatus.Confirmed)
                        SendVerificationEmail(contactEmail.EmailAddress);

                },
                error => {
                    ManageError(errorNum.PlayfabError, error.GenerateErrorReport());
                });
            return success;
        }
        catch { ManageError(errorNum.UnexpectedError); }

        return false;
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

    #region Register Account
    //Function Used for buttons
    public void AttemptRegisterAccount()
    {
        string email = register_EmailInput.text;
        string user = register_UserInput.text;
        string pswd = register_PswdInput.text;
        string confirmPswd = register_ConfirmInput.text;

        RegisterAccount(email, user, pswd, confirmPswd);
    }
    private void RegisterAccount(string email, string user, string pswd, string confirmPswd)
    {
        try
        {
            int emailError = CheckEmail(email);
            int userError = CheckUsername(user);
            int pswdError = CheckPassword(pswd);
            int confirmError = CheckMatchPassword(pswd, confirmPswd);

            if (emailError != 0 || pswdError != 0 || userError != 0 || confirmError != 0)
            {
                if (emailError != 0)
                    ManageError((errorNum)emailError);
                if (userError != 0)
                    ManageError((errorNum)userError);
                if (pswdError != 0)
                    ManageError((errorNum)pswdError);
                else if (confirmError != 0)
                    ManageError((errorNum)confirmError);
                return;
            }

            //Deal with error in function and delete textboxes if success (then go to welcome screen)
            RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest
            {
                Username = user,
                Email = email,
                Password = pswd,
            };

            PlayFabClientAPI.RegisterPlayFabUser(request,
                result => {
                    Debug.Log("Register Successful");
                    Login(email, pswd);
                },
                error => {
                    ManageError(errorNum.PlayfabError, error.GenerateErrorReport());
                });
        }
        catch { ManageError(errorNum.UnexpectedError); }
    }

    //Send a verification email to the user (after logging in)
    private void SendVerificationEmail(string email)
    {
        try
        {
            AddOrUpdateContactEmailRequest verify = new AddOrUpdateContactEmailRequest
            {
                EmailAddress = email,
            };

            PlayFabClientAPI.AddOrUpdateContactEmail(verify,
                result =>
                {
                    Debug.Log("Verification Email Sent");
                },
                error =>
                {
                    ManageError(errorNum.PlayfabError, error.GenerateErrorReport());
                });
        }
        catch (Exception ex) { ManageError(errorNum.UnexpectedError, ex.Message); }
    }

    private void LeaveRegister()
    {
        register_UserInput.text = "";
        register_UserError.gameObject.SetActive(false);

        register_EmailInput.text = "";
        register_EmailError.gameObject.SetActive(false);

        register_PswdInput.text = "";
        register_PswdError.gameObject.SetActive(false);

        register_ConfirmInput.text = "";
        register_ConfirmError.gameObject.SetActive(false);
    }
    #endregion

    #region Reset Info
    private void ResetPassword(string email)
    {
        SendAccountRecoveryEmailRequest request = new SendAccountRecoveryEmailRequest
        {
            Email = email,
            TitleId = "A6F68",
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(request,
            result =>
            {
                Debug.Log("Password Reset Email Sent");
            },
            error => { ManageError(errorNum.PlayfabError, error.GenerateErrorReport()); });

        ManageError(errorNum.UnexpectedError);
    }
    #endregion

}
