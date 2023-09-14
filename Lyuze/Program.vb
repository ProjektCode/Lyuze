Imports System.IO
Imports System.Threading.Thread
Imports Newtonsoft.Json


#Region "-To-Do List / +Found Bugs"
'-Find more commands/apis to use.(try for 50 commands.)
'-Possibly make your own "what am I?" API.
'-Possibly revert JikanAPI to before the Async Changes(this is to have all the anime commands working again)
'-For certain music commands check if user has the DJ role
'-For anime radio see if you could implement LISTEN.MOE api.
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
            loggingHandler.LogInformationAsync("setup", "File structure has been created... Now go into Resources/Settings and open settings.json and fill in the required fields, then go to Resources/Victoria and place your Lavalink.jar and application.yaml in the folder.")
            Sleep(5000)
            Environment.Exit(0)
        End If
        Call SetUp().GetAwaiter.GetResult()
    End Sub

    Private Async Function SetUp() As Task
        Dim settings = Lyuze.Settings.Data
        'All sleep timers are there only for aesthetic no real reason for them to be there.
        If settings.Discord.Token = "Token Here" Or settings.Discord.Token.Length < 25 Then
            loggingHandler.LogCriticalAsync("setup", "Please go into the settings file and configure the bot.")
            Sleep(5000)
            Environment.Exit(0)
        End If

        Console.Clear()
        Console.Title = $"{settings.Discord.Name} - Discord Bot"
        _utils.setBanner($"/ {settings.Discord.Name} Bot \", "#DC143C", "#00A36C")
        Await loggingHandler.LogSetupAsync("setup", "Looking for Lavalink server...")

        If Not File.Exists(lavalink) Or Not File.Exists(app) Then
            Await loggingHandler.LogCriticalAsync("setup", "Please add your Lavalink.jar and Application.yaml into the Victoria folder.")
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

            Dim ProfileImages As New List(Of Uri) From {
                New Uri("https://i.imgur.com/0LdZD8l.png"),
                New Uri("https://i.imgur.com/2leJPec.png"),
                New Uri("https://i.imgur.com/QfA4Rrh.png"),
                New Uri("https://i.imgur.com/GxyjOLz.png")
            }

            Dim LinkImages As New List(Of Uri) From {
                New Uri("https://i.imgur.com/14Rf5rb.jpeg"),
                New Uri("https://i.imgur.com/b0odfTt.png"),
                New Uri("https://i.imgur.com/brFnwAn.jpeg"),
                New Uri("https://i.imgur.com/wMvbGO7.jpeg"),
                New Uri("https://i.imgur.com/ui9HY4F.jpeg"),
                New Uri("https://i.imgur.com/GscDHaz.jpeg"),
                New Uri("https://i.imgur.com/c4AfCSZ.jpeg"),
                New Uri("https://i.imgur.com/3Oen3mc.jpeg"),
                New Uri("https://i.imgur.com/qMLsFcS.jpeg"),
                New Uri("https://images.unsplash.com/photo-1445331552301-94139f242587?ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&ixlib=rb-1.2.1&auto=format&fit=crop&w=1470&q=80"),
                New Uri("https://images.unsplash.com/photo-1480796927426-f609979314bd?ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&ixlib=rb-1.2.1&auto=format&fit=crop&w=2070&q=80"),
                New Uri("https://images.unsplash.com/photo-1528360983277-13d401cdc186?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=2070&q=80"),
                New Uri("https://images.unsplash.com/photo-1534214526114-0ea4d47b04f2?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=7952&q=80"),
                New Uri("https://images.unsplash.com/photo-1493514789931-586cb221d7a7?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=6512&q=80")
            }

            Dim newSettings As New Lyuze.Settings With {
                .Discord = New _Discord With {
                    .Name = "Name Here",
                    .Prefix = "!",
                    .Token = "Enter Token Here"
                },
                .ApIs = New ApIs With {
                    .Tenor = "Tenor API Key Here",
                    .UnsplashAccess = "Unsplash Access Key Here",
                    .UnsplashSecret = "Unsplash Secret Key Here"
                },
                .Database = New Database With {
                    .MongoDb = "MongoDB Key Here"
                },
                .IDs = New IDs With {
                    .ErrorId = 1234567890,
                    .KickId = 1234567890,
                    .LeaveId = 1234567890,
                    .OwnerId = 1234567890,
                    .ReportId = 1234567890,
                    .WelcomeId = 1234567890
                },
                .GoodbyeMessage = New List(Of String),
                .WelcomeMessage = New List(Of String),
                .ImageLinks = LinkImages,
                .ProfileBanners = ProfileImages
            }
            File.WriteAllText(Settings + "settings.json", Newtonsoft.Json.JsonConvert.SerializeObject(newSettings, Formatting.Indented))
        Catch ex As Exception
            loggingHandler.LogCriticalAsync("setup", ex.Message)
            Sleep(2000)
            Environment.Exit(0)
        End Try
    End Sub

End Module