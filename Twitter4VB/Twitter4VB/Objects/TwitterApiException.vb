Public Class TwitterApiException

    Private _errors As New List(Of ApiError)

    Public Property Errors() As List(Of ApiError)
        Get
            Return Me._errors
        End Get
        Set(value As List(Of ApiError))
            Me._errors = value
        End Set
    End Property

    Partial Class ApiError

        Public Property Message() As String

        Public Property Code() As Integer

        Public ReadOnly Property ErrorCode() As String
            Get
                Return String.Format("Error Code: {0}", Me.Code)
            End Get
        End Property

    End Class

End Class