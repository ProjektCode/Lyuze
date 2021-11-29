Imports System.IO
Imports System.Threading.Thread

#Region "-To-Do List / +Found Bugs"
'-Finish the genshin fan-made api integration
'-Find some other cool apis to use
'-Transition the mod commands into it's service class
'-Change the blacklisted roles into a list from a text file and just cycle the mentioned role through the list to check if they mentioned said role.
'+In roles list position is reversed
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
        Console.Title = "Lyuze - Multi-Purpose Discord Bot"
        _Utils.setBanner("/ Lyuze Bot \", "#FFC0CB", ConsoleColor.Green)
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
