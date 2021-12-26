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
    'Bin File Structure
    ReadOnly Resources = basePath + "Resources\"
    ReadOnly Logging = Resources + "Logging\"
    ReadOnly Victoria = Resources + "Victoria\"
    ReadOnly Settings = Resources + "Settings\"

    ReadOnly _Utils As New MasterUtils
    ReadOnly process As New Process
    Dim createDir As New List(Of String)

    Sub Main()
        Console.Title = "Discord Bot - Multi-Purpose Discord Bot"
        _Utils.setBanner("/ Discord Bot \", "#FFC0CB", ConsoleColor.Green)
        If Not Directory.Exists(Resources) Then
            BotSetup()
            loggingHandler.LogCriticalAsync("setup", "File structure has been created... Now go into Resources/Settings and open settings.json and fill in the required fields.")
            Sleep(5000)
            Environment.Exit(0)
        End If
        Call setUp().GetAwaiter.GetResult()
    End Sub

    Private Async Function setUp() As Task
        Dim settings = Lyuze.Settings.Data
        Console.Clear()
        Console.Title = $"{settings.Discord.Name} - Multi-Purpose Discord Bot"
        _Utils.setBanner($"/ {settings.Discord.Name} Bot \", "#FFC0CB", ConsoleColor.Green)
        'All sleep timers are there only for aesthetic no real reason for them to be there.
        If settings.Discord.Token = "Token Here" Then
            loggingHandler.LogCriticalAsync("setup", "Please go into the settings file and configure the bot.")
            Sleep(5000)
            Environment.Exit(0)
        End If
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
            Await loggingHandler.LogSetupAsync("setup", "Sever is setup now checking settings file...")
            Sleep(500)
            Await loggingHandler.LogSetupAsync("setup", "Downloading settings file....")
            Call New bot().mainAsync().GetAwaiter().GetResult()

        End If
    End Function

    Private Sub BotSetup()
        Try
            Dim settingsPath = $"{Settings}settings.json"
            Dim lavalinkPath = $"{Victoria}Lavalink.jar"
            Dim applicationPath = $"{Victoria}application.yaml"
            Dim jsonString As String = String.Empty
            Dim appString As String = String.Empty
            Dim settingsURL As String = "https://raw.githubusercontent.com/Projekt-Dev/Lyuze/master/Lyuze/Assets/settings.json?token=AFU5U6XFMZXK44JYEM2PIT3B2FDEM"
            Dim lavalinkURL As String = "https://www.dropbox.com/s/ofe51ep1ow94u9c/Lavalink.jar?dl=1"
            Dim applicationURL As String = "https://www.dropbox.com/s/ny0zvsxc3w7unv9/application.yaml?dl=1"

            If Not Directory.Exists(Resources) Then
                Directory.CreateDirectory(Resources)
                Directory.CreateDirectory(Logging)
                Directory.CreateDirectory(Victoria)
                Directory.CreateDirectory(Settings)
            End If
            loggingHandler.LogInformationAsync("setup", "Downloading and setting up file structure for the first time... Please wait.")
            Using client As New WebClient
                jsonString = client.DownloadString(settingsURL)
                appString = client.DownloadString(applicationURL)
                client.DownloadFile(lavalinkURL, lavalinkPath)
            End Using

            If Not jsonString = String.Empty Then
                Using writer As New StreamWriter(settingsPath, True)
                    writer.Write(jsonString)
                End Using
            End If
            If Not appString = String.Empty Then
                Using writer As New StreamWriter(applicationPath, True)
                    writer.Write(appString)
                End Using
            End If

        Catch ex As Exception
            loggingHandler.LogCriticalAsync("setup", ex.Message)
        End Try
    End Sub

End Module
