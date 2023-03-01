using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Cdm.Authentication.Browser;
using Cdm.Authentication.Clients;
using Cdm.Authentication.OAuth2;
using UnityEngine;
using static Cdm.Authentication.OAuth2.AuthorizationCodeFlow;

public class Authenticate : MonoBehaviour
{

  async void Start()
  {
    await TestAsync();
  }

  async Task TestAsync()
  {
    //using var authenticationSession = new AuthenticationSession(auth, new StandaloneBrowser());
    // Also you can use your own client configuration.
    Configuration c = new Configuration() { clientId = "", redirectUri = "http://localhost", scope = "openid email profile" };
    var auth = new GoogleAuth(c);

    var crossPlatformBrowser = new CrossPlatformBrowser();
    crossPlatformBrowser.platformBrowsers.Add(RuntimePlatform.WindowsEditor, new StandaloneBrowser());
    crossPlatformBrowser.platformBrowsers.Add(RuntimePlatform.OSXEditor, new StandaloneBrowser());

    using var authenticationSession = new AuthenticationSession(auth, crossPlatformBrowser);

    // Opens a browser to log user in
    AccessTokenResponse accessTokenResponse = await authenticationSession.AuthenticateAsync();

  }


  async Task StartAsync()
  {

    // // Also you can use your own client configuration.
    // var auth = new GoogleAuth()
    // {
    //   clientId = "...",
    //   redirectUrl = "...",
    //   scope = "openid email profile"
    // };



    // var crossPlatformBrowser = new CrossPlatformBrowser();
    // var crossPlatformBrowser.platformBrowsers.Add(RuntimePlatform.WindowsEditor, new StandaloneBrowser());
    // var crossPlatformBrowser.platformBrowsers.Add(RuntimePlatform.WindowsPlayer, new StandaloneBrowser());
    // var crossPlatformBrowser.platformBrowsers.Add(RuntimePlatform.OSXEditor, new StandaloneBrowser());
    // var crossPlatformBrowser.platformBrowsers.Add(RuntimePlatform.OSXPlayer, new StandaloneBrowser());
    // var crossPlatformBrowser.platformBrowsers.Add(RuntimePlatform.IPhonePlayer, new ASWebAuthenticationSessionBrowser());

    // using var authenticationSession = new AuthenticationSession(auth, crossPlatformBrowser);

    // // Opens a browser to log user in
    // AccessTokenResponse accessTokenResponse = await authenticationSession.AuthenticateAsync();

    // // Authentication header can be used to make authorized http calls.
    // AuthenticationHeaderValue authenticationHeader = accessTokenResponse.GetAuthenticationHeader();

    // // Gets the current acccess token, or refreshes if it is expired.
    // accessTokenResponse = await authenticationSession.GetOrRefreshTokenAsync();

    // // Gets new access token by using the refresh token.
    // AccessTokenResponse newAccessTokenResponse = await authenticationSession.RefreshTokenAsync();

    // // Or you can get new access token with specified refresh token (i.e. stored on the local disk to prevent multiple sign-in for each app launch)
    // newAccessTokenResponse = await authenticationSession.RefreshTokenAsync("my_refresh_token");


  }

}
