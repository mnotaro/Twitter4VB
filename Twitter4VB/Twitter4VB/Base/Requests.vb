Imports System.Net
Imports System.Text
Imports Newtonsoft.Json
Imports Twitter4VB.Authenticate

Public Class Requests
Inherits Core

    Partial Class IdListing

        Private _ids As New List(Of String)

        Public Property Ids As List(Of String)
            Get
                Return Me._ids
            End Get
            Set(value As List(Of String))
                Me._ids = value
            End Set
        End Property

    End Class

    Partial Class ListResults

        Private _lists As New List(Of TwitterList)

        Public Property Lists As List(Of TwitterList)
            Get
                Return Me._lists
            End Get
            Set(value As List(Of TwitterList))
                Me._lists = value
            End Set
        End Property

    End Class

    Partial Class SearchStatuses

        Private _statuses As New List(Of TwitterStatus)

        Public Property Statuses As List(Of TwitterStatus)
            Get
                Return Me._statuses
            End Get
            Set(value As List(Of TwitterStatus))
                Me._statuses = value
            End Set
        End Property

    End Class

    #Region "User"

    ''' <summary> Checks Authenticated User for Protected Requests </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <returns> List of Protected Request Id's </returns>
    Public Shared Function CheckProtectedRequests(ByVal twAuth As Credentials) As IdListing

        Dim protectedIdList As New IdListing

        Signature = BuildSignature(TWITTER_PROTECTED_REQUEST_URL, WebRequestMethods.Http.Get, twAuth)

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Dim ids As String = GenerateWebRequest(TWITTER_PROTECTED_REQUEST_URL, Header, WebRequestMethods.Http.Get)

        If Not ids = Nothing Then

            Try

                protectedIdList = JsonConvert.DeserializeObject(Of IdListing)(ids)

            Catch ex As Exception

                Console.WriteLine(ex.Message)

                Return Nothing

            End Try

        End If

        Return protectedIdList

    End Function

    ''' <summary> Unfollows User </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="unFollowBy"> Unfollow by Screen Name or Id </param>
    ''' <param name="identifier"> Screen Name or Id to be Unfollowed </param>
    ''' <returns> Successful </returns>
    Public Shared Function UnFollowUser(ByVal twAuth As Credentials, ByVal unFollowBy As LookupType, ByVal identifier As String) As Boolean

        Dim content As Byte() = Nothing

        Signature = BuildSignature(TWITTER_UNFOLLOW_USER_URL,
                                   WebRequestMethods.Http.Post,
                                   twAuth, Nothing,
                                   New Dictionary(Of String, String) From {{String.Format("&{0}", unFollowBy), identifier}})

        content = ASCIIEncoding.ASCII.GetBytes(String.Format("{0}{1}", unFollowBy, identifier))

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Return GenerateWebRequest(TWITTER_UNFOLLOW_USER_URL,
                                  Header,
                                  WebRequestMethods.Http.Post,
                                  content) IsNot Nothing

    End Function

    ''' <summary> Follows User </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="followBy"> Follow by Screen Name or Id </param>
    ''' <param name="identifier"> Screen Name or Id to be Followed </param>
    ''' <returns> Successful </returns>
    Public Shared Function FollowUser(ByVal twAuth As Credentials, ByVal followBy As LookupType, ByVal identifier As String) As Boolean

        Dim content As Byte() = Nothing

        Signature = BuildSignature(TWITTER_FOLLOW_USER_URL,
                                   WebRequestMethods.Http.Post,
                                   twAuth,
                                   New Dictionary(Of String, String) From {{"&follow=", "true"}},
                                   New Dictionary(Of String, String) From {{String.Format("&{0}", followBy), identifier}})

        content = ASCIIEncoding.ASCII.GetBytes(String.Format("{0}{1}&follow=true", followBy, identifier))

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Return GenerateWebRequest(TWITTER_FOLLOW_USER_URL,
                                  Header,
                                  WebRequestMethods.Http.Post,
                                  content) IsNot Nothing

    End Function

    ''' <summary> Looks up Retweeters of a specified Tweet </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="retweetId"> Id of Tweet to Lookup </param>
    ''' <param name="count"> Number of Retweeters to Return </param>
    ''' <returns> List of Users who Retweeted the Tweet </returns>
    Public Shared Function LookupRetweeters(ByVal twAuth As Credentials, ByVal retweetId As String, Optional count As Integer = 10) As IdListing

        Dim retweeterList As New IdListing

        Signature = BuildSignature(TWITTER_RETWEETER_IDS_URL,
                                   WebRequestMethods.Http.Get,
                                   twAuth,
                                   New Dictionary(Of String, String) From {{"&id=", retweetId}},
                                   New Dictionary(Of String, String) From {{"&stringify_ids=", "true"}},
                                   count)

        Header = BuildHeader(HeaderType.GeneralHeader,
                             Signature,
                             twAuth)

        Dim ids As String = GenerateWebRequest(String.Format("{0}?id={1}&count={2}&stringify_ids=true", TWITTER_RETWEETER_IDS_URL, retweetId, count),
                                               Header,
                                               WebRequestMethods.Http.Get)

        If Not ids = Nothing Then

            Try

                retweeterList = JsonConvert.DeserializeObject(Of IdListing)(ids)

            Catch ex As Exception

                Console.WriteLine(ex.Message)

                Return Nothing

            End Try

        End If

        Return retweeterList

    End Function

    ''' <summary> Lookup User </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="searchBy"> Lookup by Screen Name or Id </param>
    ''' <param name="identifier"> Screen Name or Id to Lookup </param>
    ''' <returns> List of Twitter Users </returns>
    ''' <remarks> If using multiple users: separate with a comma ie: user1,user2  or  12345,56789  </remarks>
    Public Shared Function ShowUserBy(ByVal twAuth As Credentials, ByVal searchBy As LookupType, ByVal identifier As String) As List(Of TwitterUser)

        Dim tweeter As New List(Of TwitterUser)

        Signature = BuildSignature(TWITTER_SHOWUSER_URL, WebRequestMethods.Http.Get, twAuth, Nothing, New Dictionary(Of String, String) From {{String.Format("&{0}", searchBy), EncodeString(identifier)}})

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        ResponseString = GenerateWebRequest(String.Format("{0}?{1}{2}", TWITTER_SHOWUSER_URL, searchBy, identifier), Header, WebRequestMethods.Http.Get)

        If Not ResponseString = Nothing Then

            Try

                tweeter = JsonConvert.DeserializeObject(Of List(Of TwitterUser))(ResponseString)

            Catch ex As Exception

                Console.WriteLine(ex.Message)

                Return Nothing

            End Try

        End If

        Return tweeter

    End Function

    ''' <summary> Blocks User </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="blockBy"> Screen Name or Id to be Blocked </param>
    ''' <param name="identifier"> Screen Name or Id to Block </param>
    ''' <returns> Successful </returns>
    Public Shared Function BlockUser(ByVal twAuth As Credentials, ByVal blockBy As LookupType, ByVal identifier As String) As Boolean

        Dim content As Byte() = Nothing

        Select Case blockBy.ToString

            Case Is = LookupType.UserId.ToString

                Signature = BuildSignature(TWITTER_BLOCK_USER_URL,
                                           WebRequestMethods.Http.Post,
                                           twAuth, Nothing,
                                           New Dictionary(Of String, String) From {{"&skip_status=", "1"}, {String.Format("&{0}", blockBy), identifier}})

            Case Is = LookupType.ScreenName.ToString

                Signature = BuildSignature(TWITTER_BLOCK_USER_URL,
                                           WebRequestMethods.Http.Post,
                                           twAuth, Nothing,
                                           New Dictionary(Of String, String) From {{String.Format("&{0}", blockBy), identifier}, {"&skip_status=", "1"}})

        End Select

        content = ASCIIEncoding.ASCII.GetBytes(String.Format("{0}{1}&skip_status=1", blockBy, identifier))

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Return GenerateWebRequest(TWITTER_BLOCK_USER_URL,
                                  Header,
                                  WebRequestMethods.Http.Post,
                                  content) IsNot Nothing

    End Function

    ''' <summary> Unblocks User </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="unBlockBy"> Screen Name or Id to be UnBlocked </param>
    ''' <param name="identifier"> Screen Name or Id to UnBlock </param>
    ''' <returns> Successful </returns>
    Public Shared Function UnBlockUser(ByVal twAuth As Credentials, ByVal unBlockBy As LookupType, ByVal identifier As String) As Boolean

        Dim content As Byte() = Nothing

        Signature = BuildSignature(TWITTER_UNBLOCK_USER_URL, WebRequestMethods.Http.Post, twAuth, Nothing, New Dictionary(Of String, String) From {{String.Format("&{0}", unBlockBy), identifier}, {"&skip_status=", "1"}})

        Select Case unBlockBy.ToString

            Case LookupType.UserId.ToString

                Signature = BuildSignature(TWITTER_UNBLOCK_USER_URL,
                                           WebRequestMethods.Http.Post,
                                           twAuth, Nothing,
                                           New Dictionary(Of String, String) From {{"&skip_status=", "1"}, {String.Format("&{0}", unBlockBy), identifier}})

            Case LookupType.ScreenName.ToString

                Signature = BuildSignature(TWITTER_UNBLOCK_USER_URL,
                                           WebRequestMethods.Http.Post,
                                           twAuth, Nothing,
                                           New Dictionary(Of String, String) From {{String.Format("&{0}", unBlockBy), identifier}, {"&skip_status=", "1"}})

        End Select


        content = ASCIIEncoding.ASCII.GetBytes(String.Format("{0}{1}&skip_status=1", unBlockBy, identifier))

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Return GenerateWebRequest(TWITTER_UNBLOCK_USER_URL, Header, WebRequestMethods.Http.Post, content) IsNot Nothing



        'Dim content As Byte() = Nothing

        'Select Case unBlockBy

        '    Case LookupType.ScreenName

        '        Signature = BuildSignature(TWITTER_UNBLOCK_USER_URL, WebRequestMethods.Http.Post, twAuth, Nothing, New Dictionary(Of String, String) From {{"&screen_name=", identifier}, {"&skip_status=", "1"}})

        '        content = ASCIIEncoding.ASCII.GetBytes(String.Format("screen_name={0}&skip_status=1", identifier))

        '    Case LookupType.UserId

        '        Signature = BuildSignature(TWITTER_UNBLOCK_USER_URL, WebRequestMethods.Http.Post, twAuth, Nothing, New Dictionary(Of String, String) From {{"&user_id=", identifier}, {"&skip_status=", "1"}})

        '        content = ASCIIEncoding.ASCII.GetBytes(String.Format("user_id={0}&skip_status=1", identifier))

        'End Select

        'Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        'Return GenerateWebRequest(TWITTER_UNBLOCK_USER_URL, Header, WebRequestMethods.Http.Post, content) IsNot Nothing

    End Function

    #End Region

    #Region "Favorites"

    ''' <summary> Creates Favorite </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="identifier"> Id of Tweet to be Favorited </param>
    ''' <returns> Successful </returns>
    Public Shared Function CreateFavorite(ByVal twAuth As Credentials, ByVal identifier As String) As Boolean

        Dim content As Byte() = Nothing

        Signature = BuildSignature(TWITTER_CREATE_FAVORITE_URL, WebRequestMethods.Http.Post, twAuth, New Dictionary(Of String, String) From {{"&id=", identifier}})

        content = ASCIIEncoding.ASCII.GetBytes(String.Format("id={0}", identifier))

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Return GenerateWebRequest(TWITTER_CREATE_FAVORITE_URL, Header, WebRequestMethods.Http.Post, content) IsNot Nothing

    End Function

    ''' <summary> Deletes Favorite </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="identifier"> Id of Tweet to be Unfavorited </param>
    ''' <returns> Successful </returns>
    Public Shared Function DeleteFavorite(ByVal twAuth As Credentials, ByVal identifier As String) As Boolean

        Dim content As Byte() = Nothing

        Signature = BuildSignature(TWITTER_DESTROY_FAVORITE_URL, WebRequestMethods.Http.Post, twAuth, New Dictionary(Of String, String) From {{"&id=", identifier}})

        content = ASCIIEncoding.ASCII.GetBytes(String.Format("id={0}", identifier))

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Return GenerateWebRequest(TWITTER_DESTROY_FAVORITE_URL, Header, WebRequestMethods.Http.Post, content) IsNot Nothing

    End Function

    #End Region

    #Region "Direct Messages"

    ''' <summary> Sends Direct Message </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="msgBy"> Screen Name or Id for Sending </param>
    ''' <param name="identifier"> Screen Name or Id to Send to </param>
    ''' <param name="text"> Message to send </param>
    ''' <returns> Successful </returns>
    Public Shared Function CreateDirectMessage(ByVal twAuth As Credentials, ByVal msgBy As LookupType, ByVal identifier As String, ByVal text As String) As Boolean

        Dim content As Byte() = Nothing

        Select Case msgBy.ToString

            Case Is = LookupType.UserId.ToString

                Signature = BuildSignature(TWITTER_DIRECT_MESSAGE_SEND_UPDATE_URL,
                                           WebRequestMethods.Http.Post,
                                           twAuth, Nothing,
                                           New Dictionary(Of String, String) From {{"&text=", EncodeString(text)}, {String.Format("&{0}", msgBy), identifier}})

            Case Is = LookupType.ScreenName.ToString

                Signature = BuildSignature(TWITTER_DIRECT_MESSAGE_SEND_UPDATE_URL,
                                           WebRequestMethods.Http.Post,
                                           twAuth, Nothing,
                                           New Dictionary(Of String, String) From {{String.Format("&{0}", msgBy), identifier}, {"&text=", EncodeString(text)}})

        End Select

        content = ASCIIEncoding.ASCII.GetBytes(String.Format("text={0}&{1}{2}", EncodeString(text), msgBy, identifier))

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Return GenerateWebRequest(TWITTER_DIRECT_MESSAGE_SEND_UPDATE_URL, Header, WebRequestMethods.Http.Post, content) IsNot Nothing

    End Function

    ''' <summary> Deletes Direct Message </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="identifier"> Direct Message Id </param>
    ''' <returns> Successful </returns>
    Public Shared Function DeleteDirectMessage(ByVal twAuth As Credentials, ByVal identifier As String) As Boolean

        Dim content As Byte() = Nothing

        Signature = BuildSignature(TWITTER_DELETE_DMS_URL, WebRequestMethods.Http.Post, twAuth, New Dictionary(Of String, String) From {{"id=", identifier}})

        content = ASCIIEncoding.ASCII.GetBytes(String.Format("id={0}", identifier))

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Return GenerateWebRequest(TWITTER_DELETE_DMS_URL, Header, WebRequestMethods.Http.Post, content) IsNot Nothing

    End Function

    ''' <summary> Gets Authenticated Users Incoming Direct Messages </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="count"> Number of Direct Messages to Return </param>
    ''' <returns> List of Authenticated Users Recieved Direct Messages </returns>
    Public Shared Function RetrieveDirectMessages(ByVal twAuth As Credentials, Optional count As Integer = 10) As List(Of TwitterStatus)

        Dim directMessages As New List(Of TwitterStatus)

        Signature = BuildSignature(TWITTER_RETRIEVE_DIRECT_MESSAGES_URL, WebRequestMethods.Http.Get, twAuth, Nothing, Nothing, count)

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Dim statuses As String = GenerateWebRequest(String.Format("{0}?count={1}", TWITTER_RETRIEVE_DIRECT_MESSAGES_URL, count), Header, WebRequestMethods.Http.Get)

        If Not statuses = Nothing Then

            Try

                directMessages = JsonConvert.DeserializeObject(Of List(Of TwitterStatus))(statuses)

            Catch ex As Exception

                Console.WriteLine(ex.Message)

                Return Nothing

            End Try

        End If

        Return directMessages

    End Function

    ''' <summary> Gets Authenticated Users Sent Direct Messages </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="count"> Number of Direct Messages to Return </param>
    ''' <returns> List of Authenticated Users Sent Direct Messages </returns>
    Public Shared Function RetrieveSentDirectMessages(ByVal twAuth As Credentials, Optional count As Integer = 10) As List(Of TwitterStatus)

        Dim directMessages As New List(Of TwitterStatus)

        Signature = BuildSignature(TWITTER_DIRECT_MESSAGES_SENT_URL, WebRequestMethods.Http.Get, twAuth, Nothing, Nothing, count)

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Dim statuses As String = GenerateWebRequest(String.Format("{0}?count={1}", TWITTER_DIRECT_MESSAGES_SENT_URL, count), Header, WebRequestMethods.Http.Get)

        If Not statuses = Nothing Then

            Try

                directMessages = JsonConvert.DeserializeObject(Of List(Of TwitterStatus))(statuses)

            Catch ex As Exception

                Console.WriteLine(ex.Message)

                Return Nothing

            End Try

        End If

        Return directMessages

    End Function

    #End Region

    #Region "User Streams"

    ''' <summary> Send Tweet </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="text"> Text of Tweet to Send </param>
    ''' <returns> Successful </returns>
    Public Shared Function UpdateStatus(ByVal twAuth As Credentials, ByVal text As String) As Boolean

        Dim content As Byte() = Nothing

        Signature = BuildSignature(TWITTER_UPDATE_STATUS_URL, WebRequestMethods.Http.Post, twAuth, Nothing, New Dictionary(Of String, String) From {{"&status=", text}})

        content = ASCIIEncoding.ASCII.GetBytes(String.Format("&status={0}", text))

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Return GenerateWebRequest(TWITTER_UPDATE_STATUS_URL, Header, WebRequestMethods.Http.Post, content) IsNot Nothing

    End Function

    ''' <summary> Delete Tweet </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="identifier"> Tweet Id to Delete </param>
    ''' <returns> Successful </returns>
    Public Shared Function DeleteStatus(ByVal twAuth As Credentials, ByVal identifier As String) As Boolean

        Signature = BuildSignature(String.Format(TWITTER_DELETE_URL, identifier), WebRequestMethods.Http.Post, twAuth)

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Return GenerateWebRequest(String.Format(TWITTER_DELETE_URL, identifier), Header, WebRequestMethods.Http.Post) IsNot Nothing

    End Function

    ''' <summary> Reply to Tweet </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="text"> Text to Reply with </param>
    ''' <param name="replyId"> Id of Tweet to Reply to </param>
    ''' <param name="user"> Screen Name to Reply to </param>
    ''' <returns> Successful </returns>
    Public Shared Function ReplyToTweet(ByVal twAuth As Credentials, ByVal text As String, ByVal replyId As String, ByVal user As String) As Boolean

        Dim content As Byte() = Nothing

        Dim replyTweet As String = String.Format("@{0} {1}", user, text)

        Signature = BuildSignature(TWITTER_UPDATE_STATUS_URL, WebRequestMethods.Http.Post, twAuth, New Dictionary(Of String, String) From {{"&in_reply_to_status_id=", replyId}},
                                   New Dictionary(Of String, String) From {{"&status=", EncodeString(replyTweet)}})

        content = ASCIIEncoding.ASCII.GetBytes(String.Format("status={0}&in_reply_to_status_id={1}", EncodeString(replyTweet), replyId))

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Return GenerateWebRequest(TWITTER_UPDATE_STATUS_URL, Header, WebRequestMethods.Http.Post, content) IsNot Nothing

    End Function

    ''' <summary> Retweets Tweet </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="identifier"> Tweet ID to be Retweeted </param>
    ''' <returns> Successful </returns>
    Public Shared Function RetweetTweet(ByVal twAuth As Credentials, ByVal identifier As String) As Boolean

        Signature = BuildSignature(String.Format(TWITTER_RETWEET_URL, identifier), WebRequestMethods.Http.Post, twAuth)

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Return GenerateWebRequest(String.Format(TWITTER_RETWEET_URL, identifier), Header, WebRequestMethods.Http.Post) IsNot Nothing

    End Function

    ''' <summary> Retrieves Authenticated Users Stream </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="count"> Number of Tweets to return </param>
    ''' <returns> List of Tweetws by Authenticated User </returns>
    Public Shared Function RetrieveMyStream(ByVal twAuth As Credentials, Optional count As Integer = 10) As List(Of TwitterStatus)

        Dim statuses As New List(Of TwitterStatus)

        Signature = BuildSignature(TWITTER_HOME_TIMELINE_URL, WebRequestMethods.Http.Get, twAuth, Nothing, Nothing, count)

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Dim statusString As String = GenerateWebRequest(String.Format("{0}?count={1}", TWITTER_HOME_TIMELINE_URL, count), Header, WebRequestMethods.Http.Get)

        If Not statusString = Nothing Then

            Try

                statuses = JsonConvert.DeserializeObject(Of List(Of TwitterStatus))(statusString)

            Catch ex As Exception

                Console.WriteLine(ex.Message)

                Return Nothing

            End Try

        End If

        Return statuses

    End Function

    ''' <summary> Gets a list of Tweets Sent by Authenticated user which have been ReTweeted </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="count"> Number of Retweets to Return </param>
    ''' <returns> List of Retweets </returns>
    Public Shared Function RetrieveRetweetsOfMe(ByVal twAuth As Credentials, Optional count As Integer = 10) As List(Of TwitterStatus)

        Dim tweets As New List(Of TwitterStatus)

        Signature = BuildSignature(TWITTER_RETWEETS_OF_ME_URL, WebRequestMethods.Http.Get, twAuth, New Dictionary(Of String, String) From {{"&include_user_entities=", "true"}}, Nothing, count)

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Dim statuses As String = GenerateWebRequest(String.Format("{0}?count={1}&include_user_entities=true", TWITTER_RETWEETS_OF_ME_URL, count), Header, WebRequestMethods.Http.Get)

        If Not statuses = Nothing Then

            Try

                tweets = JsonConvert.DeserializeObject(Of List(Of TwitterStatus))(statuses)

            Catch ex As Exception

                Console.WriteLine(ex.Message)

                Return Nothing

            End Try

        End If

        Return tweets

    End Function

    #End Region

    #Region "Lists"

    ''' <summary> Adds User to List </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="listId"> The ID of the List </param>
    ''' <param name="user"> The Screen Name of the User </param>
    ''' <param name="owner"> The Screen Name of the Owner of the List </param>
    ''' <returns> Successful </returns>
    Public Shared Function AddUserToListBySlug(ByVal twAuth As Credentials, ByVal listId As String, ByVal user As String, ByVal owner As String) As Boolean

        Dim content As Byte() = Nothing

        Signature = BuildSignature(TWITTER_ADD_USER_TO_LIST_URL, WebRequestMethods.Http.Post, twAuth, Nothing, New Dictionary(Of String, String) From {{"&owner_screen_name=", owner}, {"&screen_name=", user}, {"&slug=", listId}})

        content = ASCIIEncoding.ASCII.GetBytes(String.Format("slug={0}&owner_screen_name={1}&screen_name={2}", listId, owner, user))

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Return GenerateWebRequest(TWITTER_ADD_USER_TO_LIST_URL, Header, WebRequestMethods.Http.Post, content) IsNot Nothing

    End Function

    ''' <summary> Adds User to List </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="listId"> The ID of the List </param>
    ''' <param name="addBy"> Screen Name or Id </param>
    ''' <param name="identifier"> The Screen Name/Id of the User to be added to the List </param>
    ''' <returns> Successful </returns>
    Public Shared Function AddUserToListById(ByVal twAuth As Credentials, ByVal addBy As LookupType, ByVal identifier As String, ByVal listId As String) As Boolean

        Dim content As Byte() = Nothing

        Signature = BuildSignature(TWITTER_ADD_USER_TO_LIST_URL,
                                   WebRequestMethods.Http.Post,
                                   twAuth,
                                   New Dictionary(Of String, String) From {{"&list_id=", listId}},
                                   New Dictionary(Of String, String) From {{String.Format("&{0}", addBy), identifier}})

        content = ASCIIEncoding.ASCII.GetBytes(String.Format("list_id={0}&{1}{2}", listId, addBy, identifier))

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Return GenerateWebRequest(TWITTER_ADD_USER_TO_LIST_URL, Header, WebRequestMethods.Http.Post, content) IsNot Nothing

    End Function

    ''' <summary> Removes User from List </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="listId"> The ID of the List </param>
    ''' <param name="user"> The Screen Name of the User </param>
    ''' <param name="owner"> The Screen Name of the Owner of the List </param>
    ''' <returns> Successful </returns>
    Public Shared Function RemoveUserFromListBySlug(ByVal twAuth As Credentials, ByVal listId As String, ByVal user As String, ByVal owner As String) As Boolean

        Dim content As Byte() = Nothing

        Signature = BuildSignature(TWITTER_REMOVE_USER_FROM_LIST_URL, WebRequestMethods.Http.Post, twAuth, Nothing, New Dictionary(Of String, String) From {{"&owner_screen_name=", owner}, {"&screen_name=", user}, {"&slug=", listId}})

        content = ASCIIEncoding.ASCII.GetBytes(String.Format("slug={0}&owner_screen_name={1}&screen_name={2}", listId, owner, user))

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Return GenerateWebRequest(TWITTER_REMOVE_USER_FROM_LIST_URL, Header, WebRequestMethods.Http.Post, content) IsNot Nothing

    End Function

    ''' <summary> Removes User from List </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="listId"> The ID of the List </param>
    ''' <param name="identifier"> The Screen Name or ID of the User </param>
    ''' <returns> Successful </returns>
    Public Shared Function RemoveUserFromListById(ByVal twAuth As Credentials, ByVal removeBy As LookupType, ByVal identifier As String, ByVal listId As String) As Boolean

        Dim content As Byte() = Nothing

        Signature = BuildSignature(TWITTER_REMOVE_USER_FROM_LIST_URL,
                                   WebRequestMethods.Http.Post,
                                   twAuth,
                                   New Dictionary(Of String, String) From {{"&list_id=", listId}},
                                   New Dictionary(Of String, String) From {{String.Format("&{0}", removeBy), identifier}})

        content = ASCIIEncoding.ASCII.GetBytes(String.Format("list_id={0}&{1}{2}", listId, removeBy, identifier))

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Return GenerateWebRequest(TWITTER_REMOVE_USER_FROM_LIST_URL, Header, WebRequestMethods.Http.Post, content) IsNot Nothing

    End Function

    ''' <summary> </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="user"> Screen Name of User </param>
    ''' <param name="count"> Number of Lists to Return </param>
    ''' <returns> Returns list of Lists that the User Subscribes to </returns>
    Public Shared Function GetSubscribedListsOfUser(ByVal twAuth As Credentials, ByVal lookupBy As LookupType, ByVal user As String, Optional count As Integer = 10) As List(Of TwitterList)

        Dim lists As New List(Of TwitterList)

        Signature = BuildSignature(TWITTER_LISTS_URL, WebRequestMethods.Http.Get, twAuth, Nothing, New Dictionary(Of String, String) From {{String.Format("&{0}", lookupBy), user}}, count)

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Dim twitterLists As String = GenerateWebRequest(String.Format("{0}?{1}{2}&count={3}", TWITTER_LISTS_URL, lookupBy, user, count),
                                                        Header,
                                                        WebRequestMethods.Http.Get)

        If Not twitterLists = Nothing Then

            Try

                lists = JsonConvert.DeserializeObject(Of List(Of TwitterList))(twitterLists)

            Catch ex As Exception

                Console.WriteLine(ex.Message)

                Return Nothing

            End Try

        End If

        Return lists

    End Function

    ''' <summary> </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="user"> Screen Name of the User </param>
    ''' <param name="count"> Number of Lists to Return </param>
    ''' <returns> Returns the list of Lists that the User Owns </returns>
    Public Shared Function GetOwnedListsOfUser(ByVal twAuth As Credentials, ByVal lookupBy As LookupType, ByVal user As String, Optional count As Integer = 10) As ListResults

        Dim lists As New ListResults

        Signature = BuildSignature(TWITTER_OWNED_LISTS_BY_USER_URL, WebRequestMethods.Http.Get, twAuth, Nothing, New Dictionary(Of String, String) From {{String.Format("&{0}", lookupBy), user}}, count)

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Dim twitterLists As String = GenerateWebRequest(String.Format("{0}?{1}{2}&count={3}", TWITTER_OWNED_LISTS_BY_USER_URL, lookupBy, user, count), Header, WebRequestMethods.Http.Get)

        If Not twitterLists = Nothing Then

            Try

                lists = JsonConvert.DeserializeObject(Of ListResults)(twitterLists)

            Catch ex As Exception

                Console.WriteLine(ex.Message)

                Return Nothing

            End Try

        End If

        Return lists

    End Function

    ''' <summary> Returns Tweets of Specified List </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="slug"> List Slug </param>
    ''' <param name="owner"> Screen Name of the Owner of the List </param>
    ''' <param name="count"> Number of Tweets to Return </param>
    ''' <returns></returns>
    Public Shared Function GetStatusesFromList(ByVal twAuth As Credentials, ByVal slug As String, ByVal owner As String, Optional count As Integer = 10) As List(Of TwitterStatus)

        Dim listStatuses As New List(Of TwitterStatus)

        Signature = BuildSignature(TWITTER_LISTS_STATUSES_URL, WebRequestMethods.Http.Get, twAuth, Nothing, New Dictionary(Of String, String) From {{"&owner_screen_name=", owner}, {"&slug=", slug}}, count)

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Dim statuses As String = GenerateWebRequest(String.Format("{0}?slug={1}&owner_screen_name={2}&count={3}", TWITTER_LISTS_STATUSES_URL, slug, owner, count), Header, WebRequestMethods.Http.Get)

        If Not statuses = Nothing Then

            Try

                listStatuses = JsonConvert.DeserializeObject(Of List(Of TwitterStatus))(statuses)

            Catch ex As Exception

                Console.WriteLine(ex.Message)

                Return Nothing

            End Try

        End If

        Return listStatuses

    End Function

    ''' <summary> Creates List </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="listName"> Name of List </param>
    ''' <param name="description"> Description or List</param>
    ''' <param name="visible"> Public or Private List as Boolean </param>
    ''' <returns> Successful </returns>
    Public Shared Function CreateList(ByVal twAuth As Credentials, ByVal listName As String, ByVal description As String, ByVal visible As Visibility) As Boolean

        Dim content As Byte() = Nothing

        Dim signature As String = BuildSignature(TWITTER_CREATE_LIST_URL,
                                                 WebRequestMethods.Http.Post,
                                                 twAuth, New Dictionary(Of String, String) From {{"&description=", EncodeString(description)}, {"&mode=", visible.ToString}, {"&name=", listName}})

        content = ASCIIEncoding.ASCII.GetBytes(String.Format("name={0}&mode={1}&description={2}", listName, visible.ToString.ToLower, EncodeString(description)))

        Header = BuildHeader(HeaderType.GeneralHeader, signature, twAuth)

        Return GenerateWebRequest(TWITTER_CREATE_LIST_URL, Header, WebRequestMethods.Http.Post, content) IsNot Nothing

    End Function

    ''' <summary> Deletes List By Slug </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="slug"> Slug of List to be Deleted </param>
    ''' <returns> Successful </returns>
    Public Shared Function DeleteListBySlug(ByVal twAuth As Credentials, ByVal slug As String) As Boolean

        Dim content As Byte() = Nothing

        Signature = BuildSignature(TWITTER_DELETE_LIST_URL, WebRequestMethods.Http.Post, twAuth, Nothing,
                                   New Dictionary(Of String, String) From {{"&owner_screen_name=", twAuth.UserName}, {"&slug=", slug}})

        content = ASCIIEncoding.ASCII.GetBytes(String.Format("owner_screen_name={0}&slug={1}", twAuth.UserName, slug))

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Return GenerateWebRequest(TWITTER_DELETE_LIST_URL, Header, WebRequestMethods.Http.Post, content) IsNot Nothing

    End Function

    ''' <summary> Deletes List By Id </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="identifier"> Id of List to be Deleted </param>
    ''' <returns> Successful </returns>
    Public Shared Function DeleteListById(ByVal twAuth As Credentials, ByVal identifier As String) As Boolean

        Dim content As Byte() = Nothing

        Signature = BuildSignature(TWITTER_DELETE_LIST_URL,
                                   WebRequestMethods.Http.Post,
                                   twAuth, New Dictionary(Of String, String) From {{"&list_id=", identifier}})

        content = ASCIIEncoding.ASCII.GetBytes(String.Format("list_id={0}", identifier))

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Return GenerateWebRequest(TWITTER_DELETE_LIST_URL, Header, WebRequestMethods.Http.Post, content) IsNot Nothing

    End Function

    #End Region

    #Region "Searching"

    ''' <summary> Search for Tweets by Keyword </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="keywords"> Keywords to Search for </param>
    ''' <param name="count"> Number of Search Results to Return </param>
    ''' <returns> List of Tweets </returns>
    Public Shared Function SearchTwitterKeywords(ByVal twAuth As Credentials, ByVal keywords As String, Optional count As Integer = 10) As SearchStatuses

        Signature = BuildSignature(TWITTER_SEARCH_URL, WebRequestMethods.Http.Get, twAuth, New Dictionary(Of String, String) From {{"&include_entities=", "false"}}, New Dictionary(Of String, String) From {{"&q=", EncodeString(keywords)}}, count)

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Dim statusString As String = GenerateWebRequest(String.Format("{0}?q={1}&include_entities=false&count={2}", TWITTER_SEARCH_URL, keywords.Replace(" ", "%2520"), count), Header, WebRequestMethods.Http.Get)

        Dim statuses As New SearchStatuses

        If Not statusString = Nothing Then

            Try
                statuses = JsonConvert.DeserializeObject(Of SearchStatuses)(statusString)

            Catch ex As Exception

                Console.WriteLine(ex.Message)

                Return Nothing

            End Try

        End If

        Return statuses

    End Function

    ''' <summary> Lookup Status by ID </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="statusId"> ID of Tweet to Lookup </param>
    ''' <returns> Tweet </returns>
    Public Shared Function LookupStatusById(ByVal twAuth As Credentials, ByVal statusId As String) As TwitterStatus

        Dim tweet As New TwitterStatus

        Signature = BuildSignature(TWITTER_STATUS_LOOKUP_URL, WebRequestMethods.Http.Get, twAuth, New Dictionary(Of String, String) From {{"&id=", statusId}})

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Dim statuses As String = GenerateWebRequest(String.Format("{0}?id={1}", TWITTER_STATUS_LOOKUP_URL, statusId), Header, WebRequestMethods.Http.Get)

        If Not statuses = Nothing Then

            Try

                tweet = JsonConvert.DeserializeObject(Of TwitterStatus)(statuses)

            Catch ex As Exception

                Console.WriteLine(ex.Message)

                Return Nothing

            End Try

        End If

        Return tweet

    End Function

    ''' <summary> Gets Mentions of Authenticated User </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="count"> Number of Mentions to Return </param>
    ''' <returns> Mentions sent to User </returns>
    Public Shared Function RetrieveMentions(ByVal twAuth As Credentials, Optional count As Integer = 10) As List(Of TwitterStatus)

        Dim tweets As New List(Of TwitterStatus)

        Signature = BuildSignature(TWITTER_MENTIONS_URL, WebRequestMethods.Http.Get, twAuth, Nothing, Nothing, count)

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Dim statuses As String = GenerateWebRequest(String.Format("{0}?count={1}", TWITTER_MENTIONS_URL, count), Header, WebRequestMethods.Http.Get)

        If Not statuses = Nothing Then

            Try

                tweets = JsonConvert.DeserializeObject(Of List(Of TwitterStatus))(statuses)

            Catch ex As Exception

                Return Nothing

                Console.WriteLine(ex.Message)

            End Try

        End If

        Return tweets

    End Function

    ''' <summary> Search for Twitter Users </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <param name="keywords"> Keywords to Search for </param>
    ''' <param name="count"> Number of Users to Return </param>
    ''' <returns> List of Twitter Users </returns>
    Public Shared Function SearchTwitterUsers(ByVal twAuth As Credentials, ByVal keywords As String, Optional count As Integer = 10) As List(Of TwitterUser)

        Dim users As New List(Of TwitterUser)

        Signature = BuildSignature(TWITTER_USER_SEARCH_URL, WebRequestMethods.Http.Get, twAuth, Nothing, New Dictionary(Of String, String) From {{"&q=", keywords}}, count)

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Dim statusString As String = GenerateWebRequest(String.Format("{0}?q={1}&count={2}", TWITTER_USER_SEARCH_URL, keywords.Replace(" ", "%2520"), count), Header, WebRequestMethods.Http.Get)

        If Not statusString = Nothing Then

            Try

                users = JsonConvert.DeserializeObject(Of List(Of TwitterUser))(statusString)

            Catch ex As Exception

                Console.WriteLine(ex.Message)

                Return Nothing

            End Try

        End If


        Return users

    End Function

    #End Region

    #Region "Trends"

    ''' <summary> Checks for Trends </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <returns> List of Trend Locations </returns>
    Public Shared Function GetTrendingLocations(ByVal twAuth As Credentials) As List(Of TwitterTrendLocation)

        Dim locationsList As New List(Of TwitterTrendLocation)

        Signature = BuildSignature(TWITTER_GET_TREND_LOCATIONS_URL, WebRequestMethods.Http.Get, twAuth)

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Dim locations As String = GenerateWebRequest(TWITTER_GET_TREND_LOCATIONS_URL, Header, WebRequestMethods.Http.Get)

        If Not locations = Nothing Then

            Try

                locationsList = JsonConvert.DeserializeObject(Of List(Of TwitterTrendLocation))(locations)

            Catch ex As Exception

                Console.WriteLine(ex.Message)

                Return Nothing

            End Try

        End If

        Return locationsList

    End Function

    ''' <summary> Checks for Trends </summary>
    ''' <param name="twAuth"> Twitter Api Credentials </param>
    ''' <returns> List of Trend Locations </returns>
    Public Shared Function GetTrendByWoeId(ByVal twAuth As Credentials, ByVal woeid As String) As TrendList

        Dim trendList As New TrendList

        Signature = BuildSignature(TWITTER_GET_TRENDS_BY_LOCATION_URL, WebRequestMethods.Http.Get, twAuth, New Dictionary(Of String, String) From {{"&id=", woeid}})

        Header = BuildHeader(HeaderType.GeneralHeader, Signature, twAuth)

        Dim trends As String = GenerateWebRequest(String.Format("{0}?id={1}", TWITTER_GET_TRENDS_BY_LOCATION_URL, woeid), Header, WebRequestMethods.Http.Get)

        If Not trends = Nothing Then

            trends = trends.Trim("[").Trim("]")

            Try

                trendList = JsonConvert.DeserializeObject(Of TrendList)(trends)

            Catch ex As Exception

                Console.WriteLine(ex.Message)

                Return Nothing

            End Try

        End If

        Return trendList

    End Function

    #End Region

End Class