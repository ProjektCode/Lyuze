Imports System.IO
Imports System.Threading.Thread

#Region "-To-Do List / +Found Bugs"
'-Find some other cool apis to use
'-[Admin]Transition the mod commands into it's service class
'-[Roles]Change the blacklisted roles into a list from a text file(or use the settings) and just cycle the mentioned role through the list to check if they mentioned said role.
'+[Roles]In roles list position is reversed
'-Transition errors into either text log or discord log depending if there's a given error log channel ID.
'-[Info]Transition code into its service
'-[Backgrounds]Transition code into its service
'-[Settings]Disable API commands by determing if the value is "Disable"
#End Region

Module Program
    ReadOnly basePath = AppDomain.CurrentDomain.BaseDirectory
    ReadOnly dir = $"{basePath}Resources\Victoria\"
    ReadOnly lavalink = $"{dir}Lavalink.jar"
    ReadOnly app = $"{dir}application.yaml"
    ReadOnly _Utils As New MasterUtils
    ReadOnly process As New Process
    Sub Main()
        Call setUp().GetAwaiter.GetResult()
    End Sub

    Private Async Function setUp() As Task
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
            Await loggingHandler.LogSetupAsync("setup", "Sever is setup now starting bot")
            Sleep(1500)
            Call New bot().mainAsync().GetAwaiter().GetResult()

        End If
    End Function

End Module
