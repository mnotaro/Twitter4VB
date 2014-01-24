Public Class TwitterStatus

    Private _created_at As String
    Public Property Created_At As String
        Get
            Return Me._created_at
        End Get
        Set(value As String)
            Dim nt As DateTime = Date.ParseExact(value, "ddd MMM dd HH:mm:ss zzz yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Me._created_at = nt.ToShortDateString
        End Set
    End Property
    Public Property Favorited() As Boolean
    Public Property Id() As String
    Public Property In_Reply_To_Status_Id_Str() As String
    Public Property In_Reply_To_User_Id() As String
    Public Property In_Reply_To_Screen_Name() As String
    Public Property IsDirectMessage() As Boolean
    Public Property Recipient_Screen_Name() As String
    Public Property Text() As String
    Public Property User() As TwitterUser
    Public Property Source() As String
    Public Property Truncated() As Boolean
    Public Property Retweeted As Boolean
    Public Property Retweet_Count() As Integer

End Class
