Imports System.Text

Public Class Form1

    Public TwitterApi As New Twitter4VB.Authenticate.Credentials

    Private Sub Login(ByVal showWindow As Boolean)

        Me.Enabled = False

        If Twitter4VB.Authenticate.Authentication.AuthenticateUser(TwitterApi,
                                                   Me.consumerTxtBox.Text,
                                                   Me.consumerSecretTxtBox.Text,
                                                   Me.userNameTxtBox.Text,
                                                   Me.passwordTxtBox.Text,
                                                   showWindow) Then

            Me.authTokenTxtBox.Text = TwitterApi.TwitterOauthToken
            Me.authTokenSecretTxtBox.Text = TwitterApi.TwitterOauthTokenSecret

            MsgBox("Success!")

        Else

            MsgBox("Failed!")
            
        End If

        Me.Enabled = True

    End Sub

    Private Sub AuthWwindowBtn_Click(sender As System.Object, e As System.EventArgs) Handles authWwindowBtn.Click

        Me.Login(True)

    End Sub

    Private Sub AuthWoWindowBtn_Click(sender As System.Object, e As System.EventArgs) Handles authWoWindowBtn.Click

        Me.Login(False)

    End Sub

    Private Sub LookupUserBtn_Click(sender As System.Object, e As System.EventArgs) Handles lookupUserBtn.Click

        If Me.screenNameTxtBox.Text.Equals(String.Empty) Or Me.screenNameTxtBox.Text.Length < 4 Then MsgBox("Enter Identifier") : Exit Sub

        Dim tUsers As New List(Of Twitter4VB.TwitterUser)

        Select Case typeComboBox.SelectedIndex

            Case 1

                tUsers = Twitter4VB.Requests.ShowUserBy(TwitterApi, Twitter4VB.LookupType.UserId, Me.screenNameTxtBox.Text)

            Case 2

                tUsers = Twitter4VB.Requests.ShowUserBy(TwitterApi, Twitter4VB.LookupType.ScreenName, Me.screenNameTxtBox.Text)

        End Select

        If tUsers.Count > 0 Then

            Dim sb As New StringBuilder
            sb.AppendLine(tUsers(0).Created_At)
            sb.AppendLine(tUsers(0).CreatedAtLocalTime)
            sb.AppendLine(tUsers(0).Description)
            sb.AppendLine(tUsers(0).Favourites_Count)
            sb.AppendLine(tUsers(0).Followers_Count)
            sb.AppendLine(tUsers(0).Friends_Count)
            sb.AppendLine(tUsers(0).Id)
            sb.AppendLine(tUsers(0).Following)
            sb.AppendLine(tUsers(0).IsProtected)
            sb.AppendLine(tUsers(0).Location)
            sb.AppendLine(tUsers(0).Name)
            sb.AppendLine(tUsers(0).Notifications)
            sb.AppendLine(tUsers(0).Profile_Background_Color)
            sb.AppendLine(tUsers(0).Profile_Image_Url_Https)
            sb.AppendLine(tUsers(0).Profile_Sidebar_Border_Color)
            sb.AppendLine(tUsers(0).Profile_Sidebar_Fill_Color)
            sb.AppendLine(tUsers(0).Profile_Text_Color)
            sb.AppendLine(tUsers(0).Screen_Name)
            sb.AppendLine(tUsers(0).Status.Text)
            sb.AppendLine(tUsers(0).Status.Id)
            sb.AppendLine(tUsers(0).Time_Zone)
            sb.AppendLine(tUsers(0).Url)
            sb.AppendLine(tUsers(0).Verified)

            Me.resultsTxtBox.Text = sb.ToString

        End If

    End Sub

    Private Sub GetListsBtn_Click(sender As System.Object, e As System.EventArgs) Handles getListsBtn.Click

        If Me.listUserIdTxtBox.Text.Equals(String.Empty) Or Me.listUserIdTxtBox.Text.Length < 4 Then MsgBox("Enter Identifier") : Exit Sub

        Dim tListList As New Twitter4VB.Requests.ListResults

        Select Case TypeComboBox2.SelectedIndex

            Case 1

                tListList = Twitter4VB.Requests.GetOwnedListsOfUser(TwitterApi, Twitter4VB.LookupType.UserId, Me.listUserIdTxtBox.Text)

            Case 2

                tListList = Twitter4VB.Requests.GetOwnedListsOfUser(TwitterApi, Twitter4VB.LookupType.ScreenName, Me.listUserIdTxtBox.Text)

        End Select

        If tListList.Lists.Count > 0 Then

            Dim sb As New StringBuilder
            sb.AppendLine(tListList.Lists(0).Description)
            sb.AppendLine(tListList.Lists(0).Following)
            sb.AppendLine(tListList.Lists(0).Full_Name)
            sb.AppendLine(tListList.Lists(0).Id)
            sb.AppendLine(tListList.Lists(0).Member_Count)
            sb.AppendLine(tListList.Lists(0).Mode)
            sb.AppendLine(tListList.Lists(0).Name)
            sb.AppendLine(tListList.Lists(0).Slug)
            sb.AppendLine(tListList.Lists(0).Subscriber_Count)
            sb.AppendLine(tListList.Lists(0).Uri)
            sb.AppendLine(tListList.Lists(0).User.Screen_Name)

            Me.resultsTxtBox.Text = sb.ToString

        End If

    End Sub

    Private Sub GetSubListsBtn_Click(sender As System.Object, e As System.EventArgs) Handles GetSubListsBtn.Click

        If Me.listUserIdTxtBox.Text.Equals(String.Empty) Or Me.listUserIdTxtBox.Text.Length < 4 Then MsgBox("Enter Identifier") : Exit Sub

        Dim tListList As New List(Of Twitter4VB.TwitterList)

        Select Case TypeComboBox2.SelectedIndex

            Case 1

                tListList = Twitter4VB.Requests.GetSubscribedListsOfUser(TwitterApi, Twitter4VB.LookupType.UserId, Me.listUserIdTxtBox.Text)

            Case 2

                tListList = Twitter4VB.Requests.GetSubscribedListsOfUser(TwitterApi, Twitter4VB.LookupType.ScreenName, Me.listUserIdTxtBox.Text)

        End Select

        If tListList.Count > 0 Then

            Dim sb As New StringBuilder
            sb.AppendLine(tListList(1).Description)
            sb.AppendLine(tListList(1).Following)
            sb.AppendLine(tListList(1).Full_Name)
            sb.AppendLine(tListList(1).Id)
            sb.AppendLine(tListList(1).Member_Count)
            sb.AppendLine(tListList(1).Mode)
            sb.AppendLine(tListList(1).Name)
            sb.AppendLine(tListList(1).Slug)
            sb.AppendLine(tListList(1).Subscriber_Count)
            sb.AppendLine(tListList(1).Uri)
            sb.AppendLine(tListList(1).User.Screen_Name)

            Me.resultsTxtBox.Text = sb.ToString

        End If

    End Sub

    Private Sub FollowUserBtn_Click(sender As System.Object, e As System.EventArgs) Handles FollowUserBtn.Click

        If Me.screenNameTxtBox.Text.Equals(String.Empty) Or Me.screenNameTxtBox.Text.Length < 4 Then MsgBox("Enter Identifier") : Exit Sub

        Select Case typeComboBox.SelectedIndex

            Case 1

                Me.resultsTxtBox.Text = Twitter4VB.Requests.FollowUser(TwitterApi, Twitter4VB.LookupType.UserId, Me.screenNameTxtBox.Text)

            Case 2

                Me.resultsTxtBox.Text = Twitter4VB.Requests.FollowUser(TwitterApi, Twitter4VB.LookupType.ScreenName, Me.screenNameTxtBox.Text)

        End Select

    End Sub

    Private Sub UnFollowUserBtn_Click(sender As System.Object, e As System.EventArgs) Handles unFollowUserBtn.Click

        If Me.screenNameTxtBox.Text.Equals(String.Empty) Or Me.screenNameTxtBox.Text.Length < 4 Then MsgBox("Enter Identifier") : Exit Sub

        Select Case typeComboBox.SelectedIndex

            Case 1

                Me.resultsTxtBox.Text = Twitter4VB.Requests.UnFollowUser(TwitterApi, Twitter4VB.LookupType.UserId, Me.screenNameTxtBox.Text)

            Case 2

                Me.resultsTxtBox.Text = Twitter4VB.Requests.UnFollowUser(TwitterApi, Twitter4VB.LookupType.ScreenName, Me.screenNameTxtBox.Text)

        End Select

    End Sub

    Private Sub BlockUserBtn_Click(sender As System.Object, e As System.EventArgs) Handles BlockUserBtn.Click

        Select Case typeComboBox.SelectedIndex

            Case 1

                resultsTxtBox.Text = Twitter4VB.Requests.BlockUser(TwitterApi, Twitter4VB.LookupType.UserId, Me.screenNameTxtBox.Text)

            Case 2

                resultsTxtBox.Text = Twitter4VB.Requests.BlockUser(TwitterApi, Twitter4VB.LookupType.ScreenName, Me.screenNameTxtBox.Text)

        End Select

    End Sub

    Private Sub UnBlockUserBtn_Click(sender As System.Object, e As System.EventArgs) Handles UnBlockUserBtn.Click

        Select Case typeComboBox.SelectedIndex

            Case 1

                resultsTxtBox.Text = Twitter4VB.Requests.UnBlockUser(TwitterApi, Twitter4VB.LookupType.UserId, Me.screenNameTxtBox.Text)

            Case 2

                resultsTxtBox.Text = Twitter4VB.Requests.UnBlockUser(TwitterApi, Twitter4VB.LookupType.ScreenName, Me.screenNameTxtBox.Text)

        End Select

    End Sub

    Private Sub SendDmBtn_Click(sender As System.Object, e As System.EventArgs) Handles SendDmBtn.Click

        Select Case TypeComboBox3.SelectedIndex

            Case 1

                resultsTxtBox.Text = Twitter4VB.Requests.CreateDirectMessage(TwitterApi, Twitter4VB.LookupType.UserId, Me.UserDmTxtBox.Text, Me.StatusTextBox.Text)

            Case 2

                resultsTxtBox.Text = Twitter4VB.Requests.CreateDirectMessage(TwitterApi, Twitter4VB.LookupType.ScreenName, Me.UserDmTxtBox.Text, Me.StatusTextBox.Text)

        End Select

    End Sub

    Private Sub SendTweetBtn_Click(sender As System.Object, e As System.EventArgs) Handles SendTweetBtn.Click

        Me.resultsTxtBox.Text = Twitter4VB.Requests.UpdateStatus(TwitterApi, StatusTextBox.Text)

    End Sub

    Private Sub CreateListBtn_Click(sender As System.Object, e As System.EventArgs) Handles CreateListBtn.Click

        Me.resultsTxtBox.Text = Twitter4VB.Requests.CreateList(TwitterApi, Me.ListNameTxtBox.Text, Me.ListDescTxtBox.Text, Twitter4VB.Core.Visibility.public)

    End Sub

    Private Sub DeleteListBtn_Click(sender As System.Object, e As System.EventArgs) Handles DeleteListBtn.Click

        Me.resultsTxtBox.Text = Twitter4VB.Requests.DeleteListBySlug(TwitterApi, Me.DeleteListTxtBox.Text)

    End Sub

    Private Sub DeleteListByIdBtn_Click(sender As System.Object, e As System.EventArgs) Handles DeleteListByIdBtn.Click

        Me.resultsTxtBox.Text = Twitter4VB.Requests.DeleteListById(TwitterApi, Me.DeleteListTxtBox.Text)

    End Sub

    Private Sub AddUserToListByIdBtn_Click(sender As System.Object, e As System.EventArgs) Handles AddUserToListByIdBtn.Click

        Select Case Me.TypeComboBox2.SelectedIndex

            Case 1

                Me.resultsTxtBox.Text = Twitter4VB.Requests.AddUserToListById(TwitterApi, Twitter4VB.LookupType.UserId, Me.addUserToListUserIdBox.Text, Me.addUserToListListIdTxtBox.Text)

            Case 2

                Me.resultsTxtBox.Text = Twitter4VB.Requests.AddUserToListById(TwitterApi, Twitter4VB.LookupType.ScreenName, Me.addUserToListUserIdBox.Text, Me.addUserToListListIdTxtBox.Text)

        End Select

    End Sub

    Private Sub RemoveUserFromListByIdBtn_Click(sender As System.Object, e As System.EventArgs) Handles RemoveUserFromListByIdBtn.Click

        Select Case Me.TypeComboBox2.SelectedIndex

            Case 1

                Me.resultsTxtBox.Text = Twitter4VB.Requests.RemoveUserFromListById(TwitterApi, Twitter4VB.LookupType.UserId, Me.addUserToListUserIdBox.Text, Me.addUserToListListIdTxtBox.Text)

            Case 2

                Me.resultsTxtBox.Text = Twitter4VB.Requests.RemoveUserFromListById(TwitterApi, Twitter4VB.LookupType.ScreenName, Me.addUserToListUserIdBox.Text, Me.addUserToListListIdTxtBox.Text)

        End Select

    End Sub

    Private Sub LookUpTweetBtn_Click(sender As System.Object, e As System.EventArgs) Handles LookUpTweetBtn.Click

        Dim tStatus As New Twitter4VB.TwitterStatus

        tStatus = Twitter4VB.Requests.LookupStatusById(TwitterApi, Me.TweetIdTxtBox.Text)

        If tStatus IsNot Nothing Then

            Dim sb As New StringBuilder

            sb.AppendLine(tStatus.Created_At)
            sb.AppendLine(tStatus.Favorited)
            sb.AppendLine(tStatus.Id)
            sb.AppendLine(tStatus.In_Reply_To_Screen_Name)
            sb.AppendLine(tStatus.In_Reply_To_Status_Id_Str)
            sb.AppendLine(tStatus.In_Reply_To_User_Id)
            sb.AppendLine(tStatus.IsDirectMessage)
            sb.AppendLine(tStatus.Recipient_Screen_Name)
            sb.AppendLine(tStatus.Retweet_Count)
            sb.AppendLine(tStatus.Source)
            sb.AppendLine(tStatus.Text)
            sb.AppendLine(tStatus.Truncated)
            sb.AppendLine(tStatus.User.Screen_Name)

            Me.resultsTxtBox.Text = sb.ToString

        End If

    End Sub

    Private Sub GetTrendingLocationsBtn_Click(sender As System.Object, e As System.EventArgs) Handles GetTrendingLocationsBtn.Click

        Dim trendList As New List(Of Twitter4VB.TwitterTrendLocation)

        trendList = Twitter4VB.Requests.GetTrendingLocations(TwitterApi)

        If trendList.Count > 0 Then

            Dim sb As New StringBuilder

            For i As Integer = 0 To trendList.Count - 1

                sb.AppendLine(trendList(i).Country)
                sb.AppendLine(trendList(i).CountryCode)
                sb.AppendLine(trendList(i).Name)
                sb.AppendLine(trendList(i).Url)
                sb.AppendLine(trendList(i).WoeId)

            Next

            Me.resultsTxtBox.Text = sb.ToString

        End If

    End Sub

    Private Sub GetTrendsBtn_Click(sender As System.Object, e As System.EventArgs) Handles GetTrendsBtn.Click

        Dim trendList As New Twitter4VB.TrendList

        trendList = Twitter4VB.Requests.GetTrendByWoeId(TwitterApi, Me.WoeIdTxtBox.Text)

        If trendList.Trends.Count > 0 Then

            Dim sb As New StringBuilder

            For i As Integer = 0 To trendList.Trends.Count - 1

                sb.AppendLine(trendList.Trends(i).Events)
                sb.AppendLine(trendList.Trends(i).Name)
                sb.AppendLine(trendList.Trends(i).Query)
                sb.AppendLine(trendList.Trends(i).Url)
                sb.AppendLine(trendList.as_of)
                sb.AppendLine(trendList.created_at)

                For q As Integer = 0 To trendList.locations.Count - 1

                    sb.AppendLine(trendList.locations(q).Name)

                Next

            Next

            Me.resultsTxtBox.Text = sb.ToString

        End If

    End Sub

    Private Sub GetTimeLineButton_Click(sender As System.Object, e As System.EventArgs) Handles GetTimeLineButton.Click

        Dim tStatus As New List(Of Twitter4VB.TwitterStatus)

        tStatus = Twitter4VB.Requests.RetrieveMyStream(TwitterApi)

        If tStatus.Count > 0 Then

            Dim sb As New StringBuilder

            For i As Integer = 0 To tStatus.Count - 1

                sb.AppendLine(tStatus(i).Text)

            Next

            Me.resultsTxtBox.Text = sb.ToString

        End If

    End Sub

    Private Sub GetMentionsBtn_Click(sender As System.Object, e As System.EventArgs) Handles GetMentionsBtn.Click

        Dim tStatus As New List(Of Twitter4VB.TwitterStatus)

        tStatus = Twitter4VB.Requests.RetrieveMentions(TwitterApi)

        If tStatus.Count > 0 Then

            Dim sb As New StringBuilder

            For i As Integer = 0 To tStatus.Count - 1

                sb.AppendLine(tStatus(i).Text)

            Next

            Me.resultsTxtBox.Text = sb.ToString

        End If

    End Sub

    Private Sub GetRetweetersBtn_Click(sender As System.Object, e As System.EventArgs) Handles GetRetweetersBtn.Click

        Dim tStatus As New List(Of Twitter4VB.TwitterStatus)

        tStatus = Twitter4VB.Requests.RetrieveRetweetsOfMe(TwitterApi)

        If tStatus.Count > 0 Then

            Dim sb As New StringBuilder

            For i As Integer = 0 To tStatus.Count - 1

                sb.AppendLine(tStatus(i).Text)

            Next

            Me.resultsTxtBox.Text = sb.ToString

        End If


    End Sub

    Private Sub RetweetBtn_Click(sender As System.Object, e As System.EventArgs) Handles RetweetBtn.Click

        Me.resultsTxtBox.Text = Twitter4VB.Requests.RetweetTweet(TwitterApi, Me.RetweetIdTxtBox.Text)

    End Sub

    Private Sub DeleteTweetBtn_Click(sender As System.Object, e As System.EventArgs) Handles DeleteTweetBtn.Click

        Me.resultsTxtBox.Text = Twitter4VB.Requests.DeleteStatus(TwitterApi, Me.TweetIdTxtBox.Text)

    End Sub

    Private Sub SearchUsersBtn_Click(sender As System.Object, e As System.EventArgs) Handles SearchUsersBtn.Click

        Dim tUsers As New List(Of Twitter4VB.TwitterUser)

        tUsers = Twitter4VB.Requests.SearchTwitterUsers(TwitterApi, Me.KeywordsTxtBox.Text)

        If tUsers.Count > 0 Then

            Dim sb As New StringBuilder

            For i As Integer = 0 To tUsers.Count - 1

                sb.AppendLine(tUsers(i).Screen_Name)

            Next

            Me.resultsTxtBox.Text = sb.ToString

        End If

    End Sub

    Private Sub SearchTweetsBtn_Click(sender As System.Object, e As System.EventArgs) Handles SearchTweetsBtn.Click

        Dim tStatus As New Twitter4VB.Requests.SearchStatuses

        tStatus = Twitter4VB.Requests.SearchTwitterKeywords(TwitterApi, Me.KeywordsTxtBox.Text)

        If tStatus.Statuses.Count > 0 Then

            Dim sb As New StringBuilder

            For i As Integer = 0 To tStatus.Statuses.Count - 1

                sb.AppendLine(tStatus.Statuses(i).Text)

            Next

            Me.resultsTxtBox.Text = sb.ToString

        End If

    End Sub

#Region "Background Methods"

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        Me.typeComboBox.SelectedIndex = 0

        Me.TypeComboBox2.SelectedIndex = 0

        Me.TypeComboBox3.SelectedIndex = 0

        Twitter4VB.Core.ErrorBox = Me.errorTxtBox

    End Sub

#End Region

End Class