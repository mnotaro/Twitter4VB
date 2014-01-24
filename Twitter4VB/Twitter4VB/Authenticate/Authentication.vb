Imports System.Net
Imports System.Text.RegularExpressions
Imports System.Windows.Forms

Namespace Authenticate

    Public Class Authentication
    Inherits Core

        '''
        ''' <summary> Requests Tokens for Authentication </summary>
        ''' 
        Private Shared Function TokenRequest(ByRef twAuth As Credentials, Optional callBackUrl As String = "oob") As String

            Signature = BuildSignature(TWITTER_OAUTH_REQUEST_TOKEN_URL, WebRequestMethods.Http.Post, twAuth,
                                       New Dictionary(Of String, String) From {{"&oauth_callback=", callBackUrl}}, Nothing, Nothing, False, True)

            Header = BuildHeader(HeaderType.Blank, Signature, twAuth)

            Try

                Return GenerateWebRequest(String.Format("{0}?oauth_callback={1}", TWITTER_OAUTH_REQUEST_TOKEN_URL, callBackUrl), Header, WebRequestMethods.Http.Post, Nothing, twAuth)

            Catch ex As Exception

                Console.WriteLine(ex.Message)

                Return Nothing

            End Try

        End Function

        '''
        ''' <summary> Verifies Pin Code and Retrieve Access Codes </summary>
        ''' 
        Private Shared Function VerifyPinCode(ByVal wb As System.Windows.Forms.WebBrowser, ByRef twAuth As Credentials, Optional callBackUrl As String = "oob") As Boolean

            Try

                Signature = BuildSignature(TWITTER_OAUTH_ACCESS_TOKEN_REQUEST_URL, WebRequestMethods.Http.Post, twAuth, New Dictionary(Of String, String) From {{"&oauth_callback=", callBackUrl}},
                                           New Dictionary(Of String, String) From {{"code", wb.Document.GetElementsByTagName("code").Item(0).OuterText}}, Nothing, True, False)

                twAuth.TwitterOauthVerifier = wb.Document.GetElementsByTagName("code").Item(0).OuterText

                Header = BuildHeader(HeaderType.VerifyHeader, Signature, twAuth)

                Dim mc As MatchCollection = New Regex("[\-a-zA-Z0-9]{15,100}").Matches(GenerateWebRequest(String.Concat(TWITTER_OAUTH_ACCESS_TOKEN_REQUEST_URL, "?oauth_callback=", callBackUrl),
                                                                                                          Header, WebRequestMethods.Http.Post))

                twAuth.TwitterOauthToken = mc(0).Value

                twAuth.TwitterOauthTokenSecret = mc(1).Value

                Return True

            Catch ex As Exception

                Console.WriteLine(ex.Message)

                Return False

            End Try

        End Function

        '''
        ''' <summary> Signs out of Twitter </summary>
        ''' 
        Private Shared Sub SignOutOfTwitter(ByVal wb As WebBrowser)

            For Each ele As HtmlElement In wb.Document.GetElementsByTagName("input")

                If (Not ele.GetAttribute("value") = Nothing AndAlso ele.GetAttribute("value").Equals("Sign out")) Then

                    ele.InvokeMember("click")

                    WaitForBrowser(wb, 3)

                    Exit For

                End If

            Next

        End Sub

        ''' <summary> Authenticates User </summary>
        ''' <param name="twAuth"> Twitter Api Credentials </param>
        ''' <param name="consumer"> Application Consumer Key </param>
        ''' <param name="consumerSecret"> Application Consumer Secret </param>
        ''' <param name="userName"> User Name to Authenticate </param>
        ''' <param name="userPassword"> Password of User to Authenticate </param>
        ''' <param name="showWindow"> Show Authentication in Popup window </param>
        ''' <returns> Successful </returns>
        Public Shared Function AuthenticateUser(ByRef twAuth As Credentials, ByVal consumer As String, ByVal consumerSecret As String, ByVal userName As String, ByVal userPassword As String, Optional showWindow As Boolean = False) As Boolean

            twAuth.SetTwitterCredentials(consumer, consumerSecret, "", "", "", "", userName, userPassword)

            If showWindow Then

                Using authWindow As New AuthenticationWindow

                    authWindow.Show()

                    authWindow.WebBrowser1.Navigate(String.Format("https://api.twitter.com/oauth/authorize?{0}", TokenRequest(twAuth)))

                    WaitForBrowser(authWindow.WebBrowser1, 3)

                    SignOutOfTwitter(authWindow.WebBrowser1)

                    If authWindow.WebBrowser1.Document.GetElementById("username_or_email") IsNot Nothing Then

                        If authWindow.WebBrowser1.Document.GetElementById("username_or_email") IsNot Nothing Then authWindow.WebBrowser1.Document.GetElementById("username_or_email").SetAttribute("value", userName)

                        If authWindow.WebBrowser1.Document.GetElementById("password") IsNot Nothing Then authWindow.WebBrowser1.Document.GetElementById("password").SetAttribute("value", userPassword)

                        authWindow.WebBrowser1.Document.GetElementById("allow").InvokeMember("click")

                        WaitForBrowser(authWindow.WebBrowser1, 2)

                        Dim verified As Boolean = VerifyPinCode(authWindow.WebBrowser1, twAuth)

                        Return verified

                    Else

                        Return False

                    End If

                End Using

            Else

                AuthorizationBrowser.Navigate(String.Format("https://api.twitter.com/oauth/authorize?{0}", TokenRequest(twAuth)))

                WaitForBrowser(AuthorizationBrowser, 3)

                SignOutOfTwitter(AuthorizationBrowser)

                If AuthorizationBrowser.Document.GetElementById("username_or_email") IsNot Nothing Then

                    If AuthorizationBrowser.Document.GetElementById("username_or_email") IsNot Nothing Then AuthorizationBrowser.Document.GetElementById("username_or_email").SetAttribute("value", userName)

                    If AuthorizationBrowser.Document.GetElementById("password") IsNot Nothing Then AuthorizationBrowser.Document.GetElementById("password").SetAttribute("value", userPassword)

                    AuthorizationBrowser.Document.GetElementById("allow").InvokeMember("click")

                    WaitForBrowser(AuthorizationBrowser, 2)

                End If

            End If

            Return VerifyPinCode(AuthorizationBrowser, twAuth)

        End Function

    End Class

End Namespace