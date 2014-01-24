Public Class LookupType

    Private ReadOnly key As String

    Public Shared ReadOnly ScreenName As LookupType = New LookupType("screen_name=")

    Public Shared ReadOnly UserId As LookupType = New LookupType("user_id=")

    Private Sub New(key As String)
        Me.key = key
    End Sub

    Public Overrides Function ToString() As String
        Return Me.key
    End Function

End Class