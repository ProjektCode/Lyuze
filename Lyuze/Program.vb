Imports System.IO
Imports System.Net
Imports System.Threading.Thread
Imports Microsoft.Extensions.DependencyInjection

#Region "-To-Do List / +Found Bugs"
'-[Backgrounds]Transition code into its service
'-[Settings]Disable API commands by determining if the value is "Disable"
'-[Images]Come up with a better name for the bannerimageasync function.
'-[Roles] For role creation have a param for hex color code for role color.
'-[FunCommands] Move commands into its own service
'-[All] Move all commands into its own service
'-[All] change .result to await in most functions. DO SO MANUALLY and slowly any new commands should follow this getup.

'+[Music Queue] Returns null. Convert it into a embed. Add song URL back into it. Add max of 7 fields.(Should this be configurable?
#End Region

Module Program
    ReadOnly basePath = AppDomain.CurrentDomain.BaseDirectory
    ReadOnly Resources = $"{basePath}Resources\"
    ReadOnly vic = $"{Resources}Victoria\"
    ReadOnly lavalink = $"{vic}Lavalink.jar"
    ReadOnly app = $"{vic}application.yaml"
    ReadOnly Victoria = $"{Resources}Victoria\"
    ReadOnly Settings = $"{Resources}Settings\"

    ReadOnly _Utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)
    ReadOnly process As New Process

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
        'All sleep timers are there only for aesthetic no real reason for them to be there.
        If settings.Discord.Token = "Token Here" Then
            loggingHandler.LogCriticalAsync("setup", "Please go into the settings file and configure the bot.")
            Sleep(5000)
            Environment.Exit(0)
        End If
        Console.Clear()
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
            process.StartInfo.WorkingDirectory() = vic
            process.StartInfo.FileName = "javaw"
            process.StartInfo.Arguments = $"-jar {lavalink}"
            process.Start()
            Sleep(3000)
            Await loggingHandler.LogSetupAsync("setup", "Sever is setup now starting bot...")
            Sleep(500)
            Call New bot().mainAsync().GetAwaiter().GetResult()

        End If
    End Function

    Private Sub BotSetup()
        Try
            Dim settingsPath = $"{Settings}settings.json"
            Dim lavalinkPath = $"{Victoria}Lavalink.jar"
            Dim applicationPath = $"{Victoria}application.yaml"
            Dim settingsURL As String = "https://raw.githubusercontent.com/Projekt-Dev/Lyuze/master/Lyuze/Assets/settings.json?token=AFU5U6WEZ2ADGJRTKWBVRWLB2FJ6W"
            Dim lavalinkURL As String = "https://www.dropbox.com/s/ofe51ep1ow94u9c/Lavalink.jar?dl=1"
            Dim applicationURL As String = "https://www.dropbox.com/s/ny0zvsxc3w7unv9/application.yaml?dl=1"

            'Checks if the Resources directory exists if not it will create the file structure.
            If Not Directory.Exists(Resources) Then
                Directory.CreateDirectory(Resources)
                Directory.CreateDirectory(Victoria)
                Directory.CreateDirectory(Settings)
            End If
            loggingHandler.LogInformationAsync("setup", "Downloading and setting up file structure for the first time please wait...")
            Using client As New WebClient
                client.DownloadFile(settingsURL, settingsPath)
                client.DownloadFile(applicationURL, applicationPath)
                client.DownloadFile(lavalinkURL, lavalinkPath)
            End Using

        Catch ex As Exception
            loggingHandler.LogCriticalAsync("setup", ex.Message)
        End Try
    End Sub

End Module
