using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DefaultNamespace;
using Firebase;
using Firebase.Auth;
using Google;
using UnityEngine;

public class AuthController : MonoBehaviour
{
    private string CLIENT_ID = "977322275897-pf3hg8n1ahngga1tk7f4tbd5j3geeaot.apps.googleusercontent.com"; //"1025458145801-v2t7ua29jchfjp01lffpoe793hp8at7b.apps.googleusercontent.com";
    
    private GoogleSignInConfiguration configuration;
    
    private FirebaseAuth auth;
    private FirebaseUser user;
    void Start()
    {   
        DebugLogObject.log("Task CheckAndFixDependenciesAsync ContinueWith"); 
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            DebugLogObject.log("CheckAndFixDependenciesAsync ContinueWith Process");
            DebugLogObject.log("IsSuccess: " + task.IsCompletedSuccessfully);    
            // Настройка конфигурации с Client ID
            configuration = new GoogleSignInConfiguration
            {
                WebClientId = CLIENT_ID,  // Client ID
                RequestIdToken = true
            };
            DebugLogObject.log("GoogleSignInConfiguration");
            try
            {
                auth = FirebaseAuth.DefaultInstance;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                DebugLogObject.log(e.ToString());
                DebugLogObject.log(e.Message);
                throw;
            }
            Debug.Log("Firebase инициализировано");
            DebugLogObject.log("Firebase initialized first time");
            if (FirebaseAuth.DefaultInstance.CurrentUser != null)
            {
                Debug.Log("Пользователь уже залогинен");
                DebugLogObject.log("User logged in");
                user = FirebaseAuth.DefaultInstance.CurrentUser;
            }
            else
            {
                Debug.Log("Пользователь не залогинен.");
                DebugLogObject.log("User NOT logged in");
                SignInAnonymously();
            }
        });
        
        
    }

    public void SignInWithGoogle()
    {
        if (auth == null) throw new RuntimeWrappedException("Firebase не инициализирован");
        DebugLogObject.log("Firebase already initialized");
        GoogleSignIn.Configuration = configuration;
        try
        {
            GoogleSignIn.DefaultInstance.SignIn().ContinueWith(task => {
                DebugLogObject.log("GoogleSignIn.DefaultInstance.SignIn ContinueWith Process");
                DebugLogObject.log("task IsCanceled: " + task.IsCanceled + " IsFaulted: " + task.IsFaulted );
                if (task.IsFaulted)
                {
                    using (var enumerator = task.Exception.InnerExceptions.GetEnumerator())
                    {
                        if (enumerator.MoveNext())
                        {
                            GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                            Debug.LogError("Error: " + error.Status + " " + error.Message);
                            DebugLogObject.log("Google sign in error: " + error.Status + " " + error.Message );
                        }
                    }
                }
                else if (task.IsCanceled)
                {
                    Debug.LogWarning("Canceled");
                    DebugLogObject.log("Google sign in canceled");
                }
                else
                {
                    // Успешная аутентификация
                    GoogleSignInUser gUser = task.Result;
                    string googleIdToken = gUser.IdToken;  // Это `idToken` для сессии
                    Debug.Log("ID Token: " + googleIdToken);
                    DebugLogObject.log("Token got: " + googleIdToken);
                    Credential credential = GoogleAuthProvider.GetCredential(googleIdToken, null);
                    auth.SignInWithCredentialAsync(credential).ContinueWith(task1 => {
                        if (task1.IsCanceled || task1.IsFaulted)
                        {
                            Debug.LogError("Вход через Google не удался.");
                            DebugLogObject.log("Firebase sign in problem");
                            return;
                        }

                        user = task1.Result;
                        Debug.Log($"Вход через Google выполнен: {user.DisplayName}");
                        DebugLogObject.log("Google sign in process completed");
                    });
                }
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            DebugLogObject.log(e.ToString());
            DebugLogObject.log(e.Message);
            throw;
        }
        
        
    }

    private void SignInAnonymously()
    {
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }

            AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
            user = result.User;
        });
    }
    

    public string getUserName()
    {
        return (user != null) ? getFireBaseUserName() : "Unknown Person";
    }

    public string getFireBaseUserName()
    {
        return (user.IsAnonymous) ? "Anon " + user.UserId.Substring(0,10) : user.DisplayName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
