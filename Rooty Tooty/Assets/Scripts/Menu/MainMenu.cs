using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // used whenever we want to change scenes in Unity
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private static MainMenu instance = null;
    public static MainMenu Instance { get => instance; }

    [Header("Object References")]
    [SerializeField] private TMP_Text newGameTxt;
    #region Menu Object References
    [Header("Menu Object References")]
    [SerializeField] private RectTransform mainMenuScreen = null;
    [SerializeField] private RectTransform optionsMenuScreen = null;

    [SerializeField] private RectTransform loginScreen = null;
    [SerializeField] private RectTransform registerScreen = null;
    [SerializeField] private RectTransform forgotPswdScreen = null;

    [SerializeField] private RectTransform verifyScreen = null;
    [SerializeField] private RectTransform resetSaveScreen = null;
    #endregion
    #region Login Screen Object References
    [Header("Login Account")]
    [SerializeField] private TextMeshProUGUI loginError = null;

    [SerializeField] private TMP_InputField login_EmailInput = null;
    [SerializeField] private TextMeshProUGUI login_EmailError = null;

    [SerializeField] private TMP_InputField login_PswdInput = null;
    [SerializeField] private TextMeshProUGUI login_PswdError = null;
    #endregion
    #region Register Screen Object References
    [Header("Register Account")]
    [SerializeField] private TextMeshProUGUI registerError = null;

    [SerializeField] private TMP_InputField register_UserInput = null;
    [SerializeField] private TextMeshProUGUI register_UserError = null;

    [SerializeField] private TMP_InputField register_EmailInput = null;
    [SerializeField] private TextMeshProUGUI register_EmailError = null;

    [SerializeField] private TMP_InputField register_PswdInput = null;
    [SerializeField] private TextMeshProUGUI register_PswdError = null;

    [SerializeField] private TMP_InputField register_ConfirmInput = null;
    [SerializeField] private TextMeshProUGUI register_ConfirmError = null;
    #endregion
    #region Forgot Pswd Screen Object References
    [Header("Forgot Password Account")]
    [SerializeField] private TextMeshProUGUI forgotDescription = null;

    [SerializeField] private TMP_InputField forgot_EmailInput = null;
    [SerializeField] private TextMeshProUGUI forgot_EmailError = null;
    #endregion
    #region Options Menu / Verification Menu/ Confirmation Menu
    [Header("Others")]
    [SerializeField] private TextMeshProUGUI options_resetGameText = null;

    [SerializeField] private TextMeshProUGUI verifyError = null;

    //[SerializeField] private TextMeshProUGUI confirmDescription = null;
    [SerializeField] private TextMeshProUGUI confirmError = null;
    //[SerializeField] private Button confirmResetSaveButton = null;
    #endregion
    private void CloseAllMenus(RectTransform exception)
    {
        RectTransform[] menus = new RectTransform[] { 
            mainMenuScreen,
            optionsMenuScreen,
            loginScreen,
            registerScreen,
            forgotPswdScreen,
            verifyScreen,
            resetSaveScreen,
        };

        foreach (RectTransform menu in menus)
        {
            if(exception != menu)
                menu.gameObject.SetActive(false);
        }
    }
    private void Awake()
    {
        StopAllCoroutines();

        if(instance == null)
            instance = this;

        //Change startGame to reflect if save is a newgame or not (check to see if file exists)
        if (newGameTxt != null)
        {
            if (gameManagerScript.instance.UsingPlayFab)
            {
                StartCoroutine(SetNewGameTxtOnLogin());
            }
            else if (SaveManager.LoadGameFromFile() == null)
            {
                gameManagerScript.instance.ToggleNewGame(false);
                newGameTxt.text = "New Game";
            }
            else
            {
                gameManagerScript.instance.ToggleNewGame(true);
                newGameTxt.text = "Continue";
            }
        }

        #region Local File Save
        if (!gameManagerScript.instance.UsingPlayFab)
        {
            mainMenuScreen.gameObject.SetActive(true);
            CloseAllMenus(mainMenuScreen);
        }
        #endregion
        #region Playfab Account Save
        else
        {
            #region Update Input Fields
            //Playfab: username should be 3-20 characters
            register_UserInput.characterLimit = 20;

            //Playfab: pswd should be 6-100 characters
            login_PswdInput.characterLimit = 100;
            register_PswdInput.characterLimit = 100;
            register_ConfirmInput.characterLimit = 100;

            //Make sure pswd display is hidden
            login_PswdInput.contentType = TMP_InputField.ContentType.Password;
            register_PswdInput.contentType = TMP_InputField.ContentType.Password;
            register_ConfirmInput.contentType = TMP_InputField.ContentType.Password;

            //Most emails shouldn't be more than 150 characters...
            login_EmailInput.characterLimit = 150;
            register_EmailInput.characterLimit = 150;
            forgot_EmailInput.characterLimit = 150;

            //Reset all menus
            LeaveLogin();
            LeaveRegister();
            LeaveResetPassword();
            LeaveSendVerification();
            #endregion

            if (!PlayfabManager.Instance.AccountVerified.HasValue)
            {
                loginScreen.gameObject.SetActive(true);
                CloseAllMenus(loginScreen);
            }
            else if (PlayfabManager.Instance.AccountVerified.Value)
            {
                mainMenuScreen.gameObject.SetActive(true);
                CloseAllMenus(mainMenuScreen);
            }
            //Case player is logged in but not verified (unreachable use case)
            else
            {
                verifyScreen.gameObject.SetActive(true);
                CloseAllMenus(verifyScreen);
            }
            
        }
        #endregion
    }

    private IEnumerator SetNewGameTxtOnLogin()
    {

        while (!PlayfabManager.Instance.AccountVerified.HasValue)
            yield return null;

        StartCoroutine(SaveManager.LoadGameFromPlayfab(PlayfabManager.Instance.UserPlayfabID));

        while (!PlayfabManager.Instance.ReLoginAttempt.HasValue)
            yield return null;

        while (!PlayfabManager.Instance.FinishedLoadingGame.Value)
            yield return null;

        //Sections runs if relogin attempt failed (attempts to login again)
        if (!PlayfabManager.Instance.ReLoginAttempt.Value)
        {
            yield return new WaitForSeconds(1);
            StartCoroutine(SetNewGameTxtOnLogin());
        }
        //case save found
        else if (PlayfabManager.Instance.LoadedSave == null)
        {
            gameManagerScript.instance.ToggleNewGame(false);
            newGameTxt.text = "New Game";
        }
        //case user has no save
        else
        {
            gameManagerScript.instance.ToggleNewGame(true);
            newGameTxt.text = "Continue";
        }
    }

    #region Navigation Functions / Buttons
    private void ChangeMenu(RectTransform menu)
    {
        menu.gameObject.SetActive(true);
        CloseAllMenus(menu);
    }
    public void ChangeToRegisterAccount()
    {
        ChangeMenu(registerScreen);
        LeaveLogin();
    }
    public void ChangeToForgotPassword()
    {
        ChangeMenu(forgotPswdScreen);
        LeaveLogin();
    }
    public void ChangeToLogin()
    {
        ChangeMenu(loginScreen);
        LeaveRegister();
        LeaveResetPassword();
    }
    public void ChangeToMainMenu()
    {
        ChangeMenu(mainMenuScreen);
        LeaveSendVerification();
    }
    public void ChangeToOptions()
    {
        ChangeMenu(optionsMenuScreen);
    }
    public void ChangeToResetSave()
    {
        ChangeMenu(resetSaveScreen);
    }
    #endregion
    //Buttons that are used in multiple Scenes
    #region Universal Buttons
    public void QuitGame()
    {
        Debug.Log("Game has been quit");
        Application.Quit();
    }
    #endregion
    #region Main Menu Buttons
    public void PlayGame ()
    {
        //Reset local data if gameManager exists (should exist)
        if (gameManagerScript.instance != null)
        {
            // Have GameManager load the game
            gameManagerScript.instance.StartGame();

        }
    }

    #endregion
    #region Login Buttons
    public void AttemptLogin()
    {
        string email = login_EmailInput.text;
        string pswd = login_PswdInput.text;

        PlayfabManager.Instance.Login(email, pswd, login_EmailError, login_PswdError, loginError);

        Debug.Log("Logging In: " + PlayfabManager.Instance.AccountVerified + " Verify: " + PlayfabManager.Instance.AccountVerified);
        StartCoroutine(AttemptVerifyLogin());
    }
    private IEnumerator AttemptVerifyLogin()
    {
        while(!PlayfabManager.Instance.AccountVerified.HasValue)
            yield return null;

        if ((bool)PlayfabManager.Instance.AccountVerified)
            ChangeMenu(mainMenuScreen);
        else
        {
            ChangeMenu(verifyScreen);
        }

        LeaveLogin();
    }
    private void LeaveLogin()
    {
        login_EmailInput.text = "";
        login_PswdInput.text = "";
        loginError.text = "";
        login_EmailError.text = "";
        login_PswdError.text = "";
        loginError.gameObject.SetActive(false);
        login_EmailError.gameObject.SetActive(false);
        login_PswdError.gameObject.SetActive(false);

    }
    #endregion
    #region Account Verification Buttons
    public void AttemptResendVerificaton()
    {
        PlayfabManager.Instance.SendVerificationEmail(PlayfabManager.Instance.UserEmail, verifyError);
    }
    private void LeaveSendVerification()
    {
        verifyError.text = "";
        verifyError.gameObject.SetActive(false);
    }
    #endregion
    #region Create Account Buttons
    public void AttemptRegisterAccount()
    {
        string email = register_EmailInput.text;
        string user = register_UserInput.text;
        string pswd = register_PswdInput.text;
        string confirmPswd = register_ConfirmInput.text;

        PlayfabManager.Instance.RegisterAccount(email, user, pswd, confirmPswd,
            register_EmailError, register_UserError, register_PswdError, register_ConfirmError, registerError);

        StartCoroutine(AttemptVerifyLogin());
        LeaveRegister();
    }
    private void LeaveRegister()
    {
        registerError.text = "";
        registerError.gameObject.SetActive(false);

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
    #region Reset Password Buttons
    public void AttemptResetPassword()
    {
        StartCoroutine(WaitResetPassword(forgot_EmailInput.text));
    }
    private IEnumerator WaitResetPassword(string email)
    {
        while(!PlayfabManager.Instance.ResetPassword(email, forgot_EmailError, forgot_EmailError))
            yield return null;
        forgotDescription.gameObject.SetActive(true);
    }
    private void LeaveResetPassword()
    {
        forgot_EmailInput.text = "";
        forgot_EmailError.gameObject.SetActive(false);
        forgotDescription.gameObject.SetActive(false);
    }
    #endregion
    #region Options / ResetSave Buttons
    public void LogOutAccount()
    {
        PlayfabManager.Instance.LogOut();
        ChangeMenu(loginScreen);
        options_resetGameText.gameObject.SetActive(false);
    }
    public void DeleteGameSave()
    {
        options_resetGameText.gameObject.SetActive(true);
        gameManagerScript.instance.ResetSave();
    }

    #endregion
}