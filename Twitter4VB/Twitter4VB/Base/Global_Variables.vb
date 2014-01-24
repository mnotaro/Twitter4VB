Imports System.Net
Imports System.Text

'#######################################
'GLOBAL VARIABLES
Module GlobalVariables

    Public Response As HttpWebResponse

    Public ResponseString As String

    Public Const HMACSHA1 As String = "HMAC-SHA1"

    Public EncodedStringBuilder As New StringBuilder

    Public Signature As String = String.Empty

    Public Header As String = String.Empty

    Public TwitterApiEx As New TwitterApiException

End Module