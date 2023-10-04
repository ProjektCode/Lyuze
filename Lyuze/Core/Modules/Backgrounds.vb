Imports Discord.Commands
Imports Microsoft.Extensions.DependencyInjection

<Name("Wallpapers")>
<Group("wall")>
<Summary("To get all your wallpaper needs. [Getting reworked]")>
Public Class Backgrounds
    Inherits ModuleBase(Of SocketCommandContext)

    ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)

    <Command("unsplash")>
    <Summary("Get a random high-quality wallpaper with a given keyword from unsplash.com")>
    Public Async Function UnsplashRandomImage(<Remainder> tag As String) As Task
        Dim settings = Lyuze.Settings.Data

        If _utils.CheckAPI(settings.ApIs.UnsplashAccess) = False Or _utils.CheckAPI(settings.ApIs.UnsplashSecret) = False Then
            Await ReplyAsync(embed:=Await embedHandler.errorEmbed("Backgrounds - Unsplash", "No API key given. If you wish to use this command please provide an API key into the settings config."))
        Else
            Await ReplyAsync(embed:=Await BackgroundService.UnsplashRandomImage(Context, tag))
        End If

    End Function

End Class
