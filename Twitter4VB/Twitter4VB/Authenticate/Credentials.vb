Imports System.Linq

Namespace Authenticate

    Public Class Credentials

        #Region "Variables"

        Public _TWoAuthConsumer As String
        Public _TWoAuthConsumerSecret As String
        Public _TWoAuthToken As String
        Public _TWoAuthTokenSecret As String
        Public _TWoAuthAccessCode As String
        Public _TWoAuthVerifier As String
        Public _UserName As String
        Public _Password As String

        #End Region

        #Region "Properties"

        Public Property TwitterOauthConsumer As String
            Get
                Return Me._TWoAuthConsumer
            End Get
            Set(value As String)
                Me._TWoAuthConsumer = value
            End Set
        End Property

        Public Property TwitterOauthConsumerSecret As String
            Get
                Return Me._TWoAuthConsumerSecret
            End Get
            Set(value As String)
                Me._TWoAuthConsumerSecret = value
            End Set
        End Property

        Public Property TwitterOauthToken As String
            Get
                Return Me._TWoAuthToken
            End Get
            Set(value As String)
                Me._TWoAuthToken = value
            End Set
        End Property

        Public Property TwitterOauthTokenSecret As String
            Get
                Return Me._TWoAuthTokenSecret
            End Get
            Set(value As String)
                Me._TWoAuthTokenSecret = value
            End Set
        End Property

        Public Property TwitterOauthAccessCode As String
            Get
                Return Me._TWoAuthAccessCode
            End Get
            Set(value As String)
                Me._TWoAuthAccessCode = value
            End Set
        End Property

        Public Property TwitterOauthVerifier As String
            Get
                Return Me._TWoAuthVerifier
            End Get
            Set(value As String)
                Me._TWoAuthVerifier = value
            End Set
        End Property

        Public Property UserName As String
            Get
                Return Me._UserName
            End Get
            Set(value As String)
                Me._UserName = value
            End Set
        End Property

        Public Property Password As String
            Get
                Return Me._Password
            End Get
            Set(value As String)
                Me._Password = value
            End Set
        End Property

        #End Region

        #Region "Functions"

        '''
        ''' <summary> New </summary>
        ''' 
        Public Sub New()
            Me.TwitterOauthConsumer = String.Empty
            Me.TwitterOauthConsumerSecret = String.Empty
            Me.TwitterOauthToken = String.Empty
            Me.TwitterOauthTokenSecret = String.Empty
            Me.TwitterOauthAccessCode = String.Empty
            Me.TwitterOauthVerifier = String.Empty
            Me.UserName = String.Empty
            Me.Password = String.Empty
        End Sub

        '''
        ''' <summary> Set Credentials </summary>
        ''' 
        Public Sub SetTwitterCredentials(ByVal consumerKey As String, ByVal consumerSecret As String,
        ByVal token As String, ByVal tokenSecret As String,
        ByVal accessCode As String, ByVal pinCode As String,
        ByVal userName As String, ByVal password As String)

            Me.TwitterOauthConsumer = consumerKey

            Me.TwitterOauthConsumerSecret = consumerSecret

            Me.TwitterOauthToken = token

            Me.TwitterOauthTokenSecret = tokenSecret

            Me.TwitterOauthAccessCode = accessCode

            Me.TwitterOauthVerifier = pinCode

            Me.UserName = userName

            Me.Password = password

        End Sub

        '''
        ''' <summary> Set Credentials for Testing </summary>
        ''' 
        Public Sub SetTwitterTestCredentials(ByVal consumerKey As String, ByVal consumerSecret As String,
        ByVal token As String, ByVal tokenSecret As String)

            Me.TwitterOauthConsumer = consumerKey

            Me.TwitterOauthConsumerSecret = consumerSecret

            Me.TwitterOauthToken = token

            Me.TwitterOauthTokenSecret = tokenSecret

        End Sub

        #End Region

    End Class

End Namespace