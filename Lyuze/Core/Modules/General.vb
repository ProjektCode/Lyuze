Imports System.Net.Http
Imports Discord
Imports Discord.Commands
Imports Microsoft.Extensions.DependencyInjection
<Name("General")>
<Summary("Commands that don't deserve their own place.")>
Public Class General
    Inherits ModuleBase(Of SocketCommandContext)

    'Grabbing required services
    Private Shared ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)
    Private Shared ReadOnly _httpClientFactory As IHttpClientFactory = serviceHandler.provider.GetRequiredService(Of IHttpClientFactory)
    Private Shared ReadOnly _img As Images = serviceHandler.provider.GetRequiredService(Of Images)
    Private Shared ReadOnly _lvl As LevelingSystem = serviceHandler.provider.GetRequiredService(Of LevelingSystem)

    <Command("emotes")>
    <Summary("Returns all available guild emotes.")>
    Public Async Function guildEmotesCmd() As Task
        _lvl.CmdAntiSpam(Context, 2)
        Await ReplyAsync(embed:=Await GeneralService.GuildEmotes(Context))
    End Function

    <Command("report")>
    <Summary("Report messages.")>
    <Remarks("\report @messageid | you get the id by right-clicking the message and then you'll see an option to copy the message id.")>
    Public Async Function Report(id As ULong) As Task
        Dim _settings = Lyuze.Settings.Data
        Dim chnl = Context.Guild.GetTextChannel(_settings.IDs.ReportId)
        _lvl.CmdAntiSpam(Context, 2)
        Await chnl.SendMessageAsync(embed:=Await ModService.Report(id, Context))
    End Function

    <Command("url")>
    <Summary("Gets the long url from url shorteners.")>
    <Remarks("\url https://bit.ly/3uqikrh | returns a mediafire link,")>
    Public Async Function GetUrl(url As String) As Task
        _lvl.CmdAntiSpam(Context, 2)
        Await ReplyAsync(GeneralService.ReverseShortUrl(url))
    End Function

    <Command("test")>
    Public Async Function test() As Task
        _lvl.CmdAntiSpam(Context, 2)
        Dim _player As PlayerModel = Await Player.GetUser(Context)
        Await ReplyAsync($"{Context.User} XP is: {_player.XP}")
        Await ReplyAsync($"{_lvl.LevelEquation(_player.Level)} XP needed.")
    End Function
End Class
