Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Text.RegularExpressions
Imports Twitter4VB.Authenticate

Public Class Core
    Inherits Credentials

    Public Shared Property ErrorBox As System.Windows.Forms.TextBox

#Region "Enumerations"

    Enum Visibility
        [public]
        [private]
    End Enum

    Enum HeaderType
        Blank
        GeneralHeader
        VerifyHeader
    End Enum

    Structure oAuthVersions
        Const Version1 = "1.0"
        Const Version2 = "2.0"
    End Structure

#End Region

#Region "Functions"

    '''
    ''' <summary> Generates WebRequest </summary>
    '''
    Protected Shared Function GenerateWebRequest(ByVal requestUrl As String, ByVal header As String, ByVal method As String,
    Optional content() As Byte = Nothing, Optional twAuth As Credentials = Nothing) As String

        Dim wr As HttpWebRequest = HttpWebRequest.Create(requestUrl)

        Try

            With wr
                .Headers.Add("Authorization", header)
                .ContentType = "application/x-www-form-urlencoded"
                .ServicePoint.Expect100Continue = False
                .MaximumAutomaticRedirections = 4
                .MaximumResponseHeadersLength = 4
                .Method = method
            End With

            If method = WebRequestMethods.Http.Post AndAlso content IsNot Nothing Then

                Using stream As IO.Stream = wr.GetRequestStream()

                    stream.Write(content, 0, content.Length)

                End Using

            End If

            Response = wr.GetResponse()

            If twAuth IsNot Nothing Then

                Dim mc As MatchCollection = New Regex("oauth_token=[a-zA-Z0-9]{10,100}").Matches(New StreamReader(Response.GetResponseStream()).ReadToEnd().ToString)

                twAuth.TwitterOauthAccessCode = mc(0).Value.Replace("oauth_token=", "")

                ResponseString = mc(0).Value

                Return ResponseString

            End If

            Return New StreamReader(Response.GetResponseStream).ReadToEnd

        Catch ex As WebException

            TwitterApiEx = Newtonsoft.Json.JsonConvert.DeserializeObject(Of TwitterApiException)(New StreamReader(ex.Response.GetResponseStream).ReadToEnd)

            If TwitterApiEx IsNot Nothing AndAlso TwitterApiEx.errors.Count > 0 Then

                If ErrorBox IsNot Nothing Then ErrorBox.Text = TwitterApiEx.errors(0).Message & vbCrLf & TwitterApiEx.errors(0).ErrorCode

                Console.WriteLine(TwitterApiEx.errors(0).Message & vbCrLf & TwitterApiEx.errors(0).ErrorCode)

            End If


            Return Nothing

        Finally

            wr = Nothing

            If Response IsNot Nothing Then Response.Close()

        End Try

    End Function

    '''
    ''' <summary> Builds Headers </summary>
    ''' 
    Protected Shared Function BuildHeader(ByVal typeOfHeader As HeaderType, ByVal oAuthSignature As String, ByVal twAuth As Credentials) As String

        Return String.Concat("OAuth ",
                             String.Format("oauth_consumer_key=""{0}"", ", EncodeString(twAuth.TwitterOauthConsumer)),
                             String.Format("oauth_nonce=""{0}"", ", EncodeString(Generate_Nonce())),
                             String.Format("oauth_signature=""{0}"", ", EncodeString(oAuthSignature)),
                             String.Format("oauth_signature_method=""{0}"", ", EncodeString(HMACSHA1)),
                             String.Format("oauth_timestamp=""{0}"", ", EncodeString(Generate_TimeStamp())),
                             If(typeOfHeader = HeaderType.GeneralHeader, String.Format("oauth_token=""{0}"", ", EncodeString(twAuth.TwitterOauthToken)), Nothing),
                             If(typeOfHeader = HeaderType.VerifyHeader, String.Format("oauth_token=""{0}"", ", EncodeString(twAuth.TwitterOauthAccessCode)), Nothing),
                             If(typeOfHeader = HeaderType.VerifyHeader, String.Format("oauth_verifier=""{0}"", ", EncodeString(twAuth.TwitterOauthVerifier)), Nothing),
                             String.Format("oauth_version=""{0}""", EncodeString(oAuthVersions.Version1)))

    End Function

    '''
    ''' <summary> Builds Signature </summary>
    ''' 
    Protected Shared Function BuildSignature(ByVal requestUrl As String,
    ByVal method As String,
    ByRef twAuth As Credentials,
    Optional beginingParams As Dictionary(Of String, String) = Nothing,
    Optional endingParams As Dictionary(Of String, String) = Nothing,
    Optional count As Integer = 0,
    Optional isTokenVerify As Boolean = False,
    Optional isTokenRequest As Boolean = False) As String

        Dim signature As String = String.Empty

        If count > 0 Then signature += String.Format("&count={0}", count)

        If beginingParams IsNot Nothing Then
            For Each p As KeyValuePair(Of String, String) In beginingParams
                signature += String.Format("{0}{1}", p.Key, p.Value)
            Next
        End If

        signature += String.Format("&oauth_consumer_key={0}", twAuth.TwitterOauthConsumer)
        If isTokenVerify Then signature += String.Format("&oauth_verifier={0}", twAuth.TwitterOauthVerifier)
        signature += String.Format("&oauth_nonce={0}", Generate_Nonce())
        signature += String.Format("&oauth_signature_method={0}", HMACSHA1)
        signature += String.Format("&oauth_timestamp={0}", Generate_TimeStamp())
        If isTokenRequest = False Then signature += String.Format("&oauth_token={0}", twAuth.TwitterOauthToken)
        signature += String.Format("&oauth_version={0}", EncodeString(oAuthVersions.Version1))


        If endingParams IsNot Nothing Then
            For Each p As KeyValuePair(Of String, String) In endingParams
                signature += String.Format("{0}{1}", p.Key, p.Value)
            Next
        End If

        signature = String.Format("{0}&{1}&{2}", method, Uri.EscapeDataString(requestUrl), Uri.EscapeDataString(signature.TrimStart("&")))

        Dim compString As String = String.Format("{0}&{1}", EncodeString(twAuth.TwitterOauthConsumerSecret), EncodeString(twAuth.TwitterOauthTokenSecret))

        signature = Convert.ToBase64String((New System.Security.Cryptography.HMACSHA1(Encoding.ASCII.GetBytes(compString)).ComputeHash(Encoding.ASCII.GetBytes(signature))))

        Return signature

    End Function

#End Region

End Class