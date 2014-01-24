Public Class TwitterUser

    Private _created_at As String
    Public Property Id() As String
    Public Property Description() As String
    Public Property Created_At() As String
        Get
            Return Me._created_at
        End Get
        Set(value As String)
            Dim nt As DateTime = Date.ParseExact(value, "ddd MMM dd HH:mm:ss zzz yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Me._created_at = nt.ToShortDateString
        End Set
    End Property
    Public ReadOnly Property CreatedAtLocalTime() As DateTime
        Get
            Dim createdAt As DateTime
            DateTime.TryParse(Me.Created_At, createdAt)
            Return createdAt.ToLocalTime()
        End Get
    End Property
    Public Property Favourites_Count() As Integer
    Public Property Friends_Count() As Integer
    Public Property Followers_Count() As Integer
    Public Property Following() As String
    Public Property IsProtected() As Boolean
    Public Property ListedCount() As Integer
    Public Property Location() As String
    Public Property Name() As String
    Public Property Notifications() As Boolean
    Public Property Profile_Background_Color() As String
    Public Property Profile_Background_Image_Url() As String
    Public Property Profile_Image_Url_Https() As String
    Public Property Profile_Sidebar_Border_Color() As String
    Public Property Profile_Sidebar_Fill_Color() As String
    Public Property Profile_Text_Color() As String
    Public Property Screen_Name() As String
    Public Property Status() As TwitterStatus
    Public Property StatusCount() As Integer
    Public Property Time_Zone() As String
    Public Property Url() As String
    Public Property Verified() As Boolean

End Class