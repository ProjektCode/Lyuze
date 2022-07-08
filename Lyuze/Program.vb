Imports System.IO
Imports System.Net
Imports System.Threading.Thread

#Region "-To-Do List / +Found Bugs"
'-Change profile command to images instead of embeds.
'-Add database for leveling system with store for profile backgrounds.
'-Find more commands/apis to use.(try for 100 commands.)
'+Crop command does not work when it's not on the host machine.(works on the pc that it's being developed on but when being hosted on a different pc it does not work.)
#End Region

Module Program
    ReadOnly basePath = AppDomain.CurrentDomain.BaseDirectory
    ReadOnly Resources = $"{basePath}Resources\"
    ReadOnly Settings = $"{Resources}Settings\"

    ReadOnly _Utils As New MasterUtils

    Sub Main()
        Console.Title = "Discord Bot - Multi-Purpose Discord Bot"
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
        Sleep(500)


        Call New bot().mainAsync().GetAwaiter().GetResult()
    End Function

    Private Sub BotSetup()
        Try
            Dim settingsPath = $"{Settings}settings.json"
            Dim settingsURL As String = $"http://lyuze-api.projektcode.com/data/settings"

            loggingHandler.LogInformationAsync("setup", "Downloading and setting up file structure for the first time please wait...")
            'Create the file structure.
            Directory.CreateDirectory(Resources)
            Directory.CreateDirectory(Settings)

            Using client As New WebClient
                loggingHandler.LogInformationAsync("setup", "Downloading settings.json")
                client.DownloadFile(settingsURL, settingsPath)
            End Using

        Catch ex As Exception
            loggingHandler.LogCriticalAsync("setup", ex.Message)
        End Try
    End Sub

End Module
