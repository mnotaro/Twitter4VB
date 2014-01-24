Public Class TrendList

    Private _trends As New List(Of TwitterTrend)
    Public _locations As New List(Of TwitterTrendLocation)

    Public Property Trends As List(Of TwitterTrend)
        Get
            Return Me._trends
        End Get
        Set(value As List(Of TwitterTrend))
            Me._trends = value
        End Set
    End Property

    Public Property as_of() As String

    Public Property created_at() As String

    Public Property locations() As List(Of TwitterTrendLocation)
        Get
            Return Me._locations
        End Get
        Set(value As List(Of TwitterTrendLocation))
            Me._locations = value
        End Set
    End Property

End Class