Imports System.Windows.Forms

Module ControlItems

    Public StopWatch As New Stopwatch

    Public AuthorizationBrowser As New WebBrowser With {.ScriptErrorsSuppressed = True}

    '''
    ''' <summary> Wait for Browser Completion </summary>
    ''' 
    Public Sub WaitForBrowser(ByVal wb As WebBrowser, Optional delay As Integer = 1)

        StopWatch.Start()

        Do Until (wb.ReadyState = WebBrowserReadyState.Complete AndAlso StopWatch.Elapsed.Seconds >= delay)

            Application.DoEvents()

        Loop

        StopWatch.Reset()

    End Sub

End Module