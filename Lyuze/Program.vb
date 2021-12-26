Imports System.IO
Imports System.Net
Imports System.Threading.Thread

#Region "-To-Do List / +Found Bugs"
'-Transition errors into either text log or discord log depending if there's a given error log channel ID.
'-[Backgrounds]Transition code into its service
'-[Settings]Disable API commands by determing if the value is "Disable=0"
'-[Roles]For adding the role find a better way to check if the role is not an admin role for it to be added to non-mod users
'-[Images]Come up with a better name for the bannerimageasync function.
'-[Roles] For role creation have a param for hex color code for role color.
'-[FunCommands] Move commands into its own service
#End Region

Module Program
    ReadOnly basePath = AppDomain.CurrentDomain.BaseDirectory
    ReadOnly dir = $"{basePath}Resources\Victoria\"
    ReadOnly lavalink = $"{dir}Lavalink.jar"
    ReadOnly app = $"{dir}application.yaml"
    ReadOnly _Utils As New MasterUtils
    ReadOnly process As New Process
    Dim createDir As New List(Of String)

    Sub Main()
        Call setUp().GetAwaiter.GetResult()
    End Sub

    Private Async Function setUp() As Task
        'All sleep timers are there only for aesthetic no real reason for them to be there.
        Dim settings = Lyuze.Settings.Data
        Console.Title = $"{settings.Discord.Name} - Multi-Purpose Discord Bot"
        _Utils.setBanner($"/ {settings.Discord.Name} Bot \", "#FFC0CB", ConsoleColor.Green)
        Await loggingHandler.LogSetupAsync("setup", "Looking for Lavalink server...")
        If Not File.Exists(lavalink) Or Not File.Exists(app) Then
            Await loggingHandler.LogCriticalAsync("setup", "After the program closes please add your Lavalink.jar and application.yml file into the correct folder.")
            Console.WriteLine()
            Sleep(3000)
            Environment.Exit(0)
        Else
            Await loggingHandler.LogSetupAsync("setup", "Lavalink server has been found. Now starting...")
            process.EnableRaisingEvents = False
            process.StartInfo.UseShellExecute = False
            process.StartInfo.WorkingDirectory() = dir
            process.StartInfo.FileName = "javaw"
            process.StartInfo.Arguments = $"-jar {lavalink}"
            process.Start()
            Sleep(3000)
            Await loggingHandler.LogSetupAsync("setup", "Sever is setup now setting up file structure...")
            Sleep(500)
            Await loggingHandler.LogSetupAsync("setup", "Downloading settings file....")
            Call New bot().mainAsync().GetAwaiter().GetResult()

        End If
    End Function

    Private Sub FileStructure()
        Try
            Dim path = AppDomain.CurrentDomain.BaseDirectory
            Dim Resources = path + "Resources\"
            Dim Logging = Resources + "Logging\"
            Dim Victoria = Resources + "Victoria\"
            Dim Settings = Resources + "Settings\"
            Dim settingsPath = $"{Settings}settings.json"
            Dim jsonString As String = String.Empty
            Dim settingsURL As String = "https://raw.githubusercontent.com/Projekt-Dev/Lyuze/master/Lyuze/Core/Models/WaifuPicsModel.vb?token=AFU5U6QDITHG7QETOXDKSMTBZ7WGK"
            Using client As New WebClient
                jsonString = client.DownloadString(settingsURL)
            End Using

            If Not Directory.Exists(Resources) Then
                Directory.CreateDirectory(Resources)
                Directory.CreateDirectory(Logging)
                Directory.CreateDirectory(Victoria)
                Directory.CreateDirectory(Settings)
            End If

            Using writer As New StreamWriter(settingsPath, True)
                writer.Write(jsonString)
            End Using

        Catch ex As Exception
            loggingHandler.LogCriticalAsync("setup", ex.Message)
        End Try
    End Sub

End Module
