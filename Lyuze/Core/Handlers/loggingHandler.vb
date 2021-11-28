Imports Discord
Imports System.IO
Imports Pastel

NotInheritable Class loggingHandler
    Public Shared ReadOnly path = AppDomain.CurrentDomain.BaseDirectory
    Public Shared ReadOnly _path = $"{path}\Resources\Logging\"
    Public Shared ReadOnly t_file = $"{Date.Now.ToShortDateString} - logs.txt"
    Public Shared ReadOnly n_file = t_file.Replace("/", "-")
    Public Shared ReadOnly _file = n_file
    Public Shared ReadOnly save = _path + _file


#Region "Discord Logging"

    Public Shared Async Function LogAsync(ByVal src As String, ByVal severity As LogSeverity, ByVal message As String, Optional exception As Exception = Nothing) As Task
        If severity.Equals(Nothing) Then
            severity = LogSeverity.Warning
        End If
        Await Append($"{vbTab + GetSeverityString(severity)}", GetConsoleColor(severity))
        Await Append($" [{SourceToString(src)}] ".Pastel("#90EE72"), ConsoleColor.DarkGray)

        If Not String.IsNullOrWhiteSpace(message) Then
            Await Append($"{message}" & vbLf, ConsoleColor.White)
        ElseIf exception Is Nothing Then
            Await Append("Unknown Exception. Exception Returned Null." & vbLf, ConsoleColor.DarkRed)
        ElseIf exception.Message Is Nothing Then
            Await Append($"Unknown {exception.StackTrace}" & vbLf, GetConsoleColor(severity))
        End If

    End Function


    ' The Way To Log Critical Errors
    Public Shared Function LogCriticalAsync(ByVal source As String, ByVal message As String, Optional ByVal exc As Exception = Nothing) As Task
        Return LogAsync(source, LogSeverity.Critical, message, exc)
    End Function

    ' The Way To Log Basic Infomation 
    Public Shared Function LogInformationAsync(ByVal source As String, ByVal message As String) As Task
        Return LogAsync(source, LogSeverity.Info, message)
    End Function

    Public Shared Function LogSetupAsync(ByVal source As String, ByVal message As String)
        Return LogAsync(source, LogSeverity.Verbose, message)
    End Function

    ' Format The Output 
    Private Shared Async Function Append(ByVal message As String, ByVal color As ConsoleColor) As Task
        Await Task.Run(Sub()
                           Console.ForegroundColor = color
                           Console.Write(message)
                       End Sub)
    End Function

    ' Swap The Normal Source Input To Something Neater 
    Private Shared Function SourceToString(ByVal src As String) As String
        Select Case src.ToLower()
            Case "discord"
                Return "DISCD"
            Case "victoria"
                Return "VICTR"
            Case "audio"
                Return "AUDIO"
            Case "admin"
                Return "ADMIN"
            Case "gateway"
                Return "GTWAY"
            Case "blacklist"
                Return "BLAKL"
            Case "lavanode_0_socket"
                Return "LAVAS"
            Case "lavanode_0"
                Return "LAVA#"
            Case "bot"
                Return "BOTWN"
            Case "setup"
                Return "SETUP"
            Case "command"
                Return "CMMND"
            Case "database"
                Return "DBASE"
            Case Else
                Return src
        End Select
    End Function

    ' Swap The Severity To a String So We Can Output It To The Console 
    Private Shared Function GetSeverityString(ByVal severity As LogSeverity) As String
        Select Case severity
            Case LogSeverity.Critical
                Return "CRIT"
            Case LogSeverity.Debug
                Return "DBUG"
            Case LogSeverity.Error
                Return "EROR"
            Case LogSeverity.Info
                Return "INFO"
            Case LogSeverity.Verbose
                Return "SETP"
            Case LogSeverity.Warning
                Return "WARN"
            Case Else
                Return "UNKN"
        End Select
    End Function

    ' Return The Console Color Based On Severity Selected 
    Private Shared Function GetConsoleColor(ByVal severity As LogSeverity) As ConsoleColor
        Select Case severity
            Case LogSeverity.Critical
                Return ConsoleColor.Red
            Case LogSeverity.Debug
                Return ConsoleColor.Magenta
            Case LogSeverity.Error
                Return ConsoleColor.DarkRed
            Case LogSeverity.Info
                Return ConsoleColor.Green
            Case LogSeverity.Verbose
                Return ConsoleColor.DarkMagenta
            Case LogSeverity.Warning
                Return ConsoleColor.Yellow
            Case Else
                Return ConsoleColor.White
        End Select
    End Function

#End Region

#Region "Custom Logging" 'Add function to replace error codes with shorten error codes.

    Public Shared Sub ErrorLog(source As String, msg As String, Optional id As Integer = Nothing)

        If Not File.Exists(_path + _file) Then
            File.CreateText(_path + _file)
        End If

        Using writer As New StreamWriter(save, True)
            If id = Nothing Then
                writer.WriteLine($"Source: [{source}] - {msg}")
            Else
                writer.WriteLine($"Source: [{source}] - ID: [{id}] - {msg}")
            End If

        End Using
    End Sub


#End Region

End Class
