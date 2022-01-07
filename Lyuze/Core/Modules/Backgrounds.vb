Imports Discord.Commands
Imports Microsoft.Extensions.DependencyInjection

<Name("Wallpapers")>
<Group("wall")>
<Summary("To get all your wallpaper needs. [Getting reworked]")>
Public Class Backgrounds
    Inherits ModuleBase(Of SocketCommandContext)
    Dim _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)

    <Command("search")>
    <Summary("Returns a link of wallpapers with your chosen keyword [Will be reworked since some websites don't have any results on a given keyword]")>
    <Remarks("\wall search league of legends")>
    Public Async Function wallpaperCmd(<Remainder> text As String) As Task
        Await ReplyAsync(embed:=Await BackgroundService.SearchWallpaper(text, Context))
    End Function

    <Command("rwall")>
    <Summary("Returns a random link with a random keyword")>
    Public Async Function randomWallpaperCmd() As Task
        Await ReplyAsync(embed:=Await BackgroundService.RandomWallpaper(Context))
    End Function

    <Command("list")>
    <Summary("Gives a list of all of our keywords for our wallpaper command")>
    Public Async Function cmdList() As Task 'Rework with pagination
        ReplyAsync(embed:=Await BackgroundService.ListKeywords(Context.Guild, Context))
    End Function

    <Command("unsplash")>
    <Summary("Get a random high-quality wallpaper with a given keyword from unsplash.com")>
    Public Async Function UnsplashRandomImage(<Remainder> tag As String) As Task
        Dim settings = Lyuze.Settings.Data

        If _utils.CheckAPI(settings.ApIs.UnsplashAccess) = False Or _utils.CheckAPI(settings.ApIs.UnsplashSecret) = False Then
            If settings.IDs.ErrorId = Nothing Then
                loggingHandler.LogCriticalAsync("backgrounds", "No API Key was given.")
            Else
                Dim chnl = Context.Guild.GetTextChannel(settings.IDs.ErrorId)
                Await chnl.SendMessageAsync(embed:=Await embedHandler.errorEmbed("Backgrounds - Unsplash", "No API key given. If you wish to use this command please provide an API key into the settings config."))
            End If
        Else
            Await ReplyAsync(embed:=Await BackgroundService.UnsplashRandomImage(Context, tag))
        End If

    End Function

End Class
