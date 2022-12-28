Imports System.IO
Imports System.Net
Imports System.Threading.Thread
Imports Discord


#Region "-To-Do List / +Found Bugs"
'-Find more commands/apis to use.(try for 50 commands.)
'-Turn bannerasync into a command to make custom banners(this will require a nodejs server).
'-Decided whether to add node server or not.
'-Possibly make your own "what am I?" API.
'+Found cropped images in main folder. don't know where they're from = possibly because bot was placed into a admin privilage folder when the bot isn't on elevated privilages.
'-Possibly revert JikanAPI to before the Async Changes(this is to have all the anime commands working again)
#End Region

Module Program
    ReadOnly basePath = AppDomain.CurrentDomain.BaseDirectory
    ReadOnly Resources = $"{basePath}Resources\"
    ReadOnly Settings = $"{Resources}Settings\"
    ReadOnly vic = $"{Resources}Victoria\"
    ReadOnly lavalink = $"{vic}Lavalink.jar"
    ReadOnly app = $"{vic}application.yaml"

    ReadOnly _utils As New MasterUtils
    ReadOnly process As New Process

    Sub Main()
        Console.Title = "Discord Bot - Multi-Purpose Discord Bot"
        If Not Directory.Exists(Resources) Then
            BotSetup()
            loggingHandler.LogInformationAsync("setup", "File structure has been created... Now go into Resources/Settings and open settings.json and fill in the required fields.")
            Sleep(5000)
            Environment.Exit(0)
        End If
        Call SetUp().GetAwaiter.GetResult()
    End Sub

    Private Async Function SetUp() As Task
        Dim settings = Lyuze.Settings.Data
        'All sleep timers are there only for aesthetic no real reason for them to be there.
        If settings.Discord.Token = "Token Here" Then
            loggingHandler.LogCriticalAsync("setup", "Please go into the settings file and configure the bot.")
            Sleep(5000)
            Environment.Exit(0)
        End If

        Console.Clear()
        Console.Title = $"{settings.Discord.Name} - Discord Bot"
        _utils.setBanner($"/ {settings.Discord.Name} Bot \", "#DC143C", "#00A36C")
        Await loggingHandler.LogSetupAsync("setup", "Looking for Lavalink server...")

        If Not File.Exists(Lavalink) Or Not File.Exists(app) Then
            Await loggingHandler.LogCriticalAsync("setup", "Please add your Lavalink.jar and Application.yaml into the Victoria folder.")
            Console.WriteLine()
            Sleep(3000)
            Environment.Exit(0)
        Else
            Await loggingHandler.LogSetupAsync("setup", "Lavalink server has been found. Now starting...")
            Process.EnableRaisingEvents = False
            Process.StartInfo.UseShellExecute = False
            process.StartInfo.WorkingDirectory() = vic
            process.StartInfo.FileName = "javaw"
            process.StartInfo.Arguments = $"-jar {lavalink}"
            process.Start()
            Sleep(3000)
            Await loggingHandler.LogSetupAsync("setup", "Lavalink has been launched now starting bot.")
            Sleep(500)


            Call New Bot().MainAsync().GetAwaiter().GetResult()
        End If
    End Function

    Private Sub BotSetup()
        Try

            loggingHandler.LogInformationAsync("setup", "Setting up file structure for the first time please wait...")

            'Create the file structure.
            Directory.CreateDirectory(Resources)
            Directory.CreateDirectory(Settings)
            Directory.CreateDirectory(vic)

        Catch ex As Exception
            loggingHandler.LogCriticalAsync("setup", ex.Message)
            Sleep(2000)
            Environment.Exit(0)
        End Try
    End Sub

End Module
