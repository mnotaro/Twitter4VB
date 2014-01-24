Imports System.ComponentModel
Imports System.Globalization
Imports System.Text

Module Global_Functions

    'Generate Nonce
    <Description("Generates a Nonce.  Works for any oAuthV1 API Requiring a Nonce")>
    Public Function Generate_Nonce() As String

        Return New Random().Next(123400, Integer.MaxValue).ToString("X", CultureInfo.InvariantCulture)

    End Function

    'Get Time Stamp
    <Description("Generates a proper time stamp for any oAuth requiring one")>
    Public Function Generate_TimeStamp() As String

        Return Convert.ToInt64((DateTime.UtcNow - New DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds, CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture)

    End Function

    'Encode Text
    <Description("Encodes text for use in oAuth V1 API")>
    Public Function EncodeString(value As String) As String

        If value = Nothing Then Return Nothing

        EncodedStringBuilder.Clear()

        Dim str As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~"

        Dim str1 As String = value

        Dim num As Integer = 0

        Dim length As Integer = str1.Length

        While num < length

            Dim chr As Char = str1(num)

            If str.IndexOf(chr) = -1 Then

                Dim bytes As Byte() = Encoding.UTF8.GetBytes(chr.ToString())

                Dim numArray As Byte() = bytes

                For i As Integer = 0 To CInt(numArray.Length) - 1

                    Dim num1 As Byte = numArray(i)

                    EncodedStringBuilder.AppendFormat("%{0:X2}", num1)

                Next

            Else

                EncodedStringBuilder.Append(chr)

            End If

            num += 1

        End While

        Return EncodedStringBuilder.ToString()

    End Function

End Module