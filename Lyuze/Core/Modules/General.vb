Imports System.Net.Http
Imports Discord
Imports Discord.Commands
Imports Discord.Addons.Interactive
Imports Microsoft.Extensions.DependencyInjection
Imports System.Text
Imports SauceNET

<Name("General")>
<Summary("Commands that don't deserve their own place.")>
Public Class General
    Inherits InteractiveBase(Of SocketCommandContext)

    'Grabbing required services
    Private Shared ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)
    Private Shared ReadOnly _httpClientFactory As IHttpClientFactory = serviceHandler.provider.GetRequiredService(Of IHttpClientFactory)
    Private Shared ReadOnly _img As Images = serviceHandler.provider.GetRequiredService(Of Images)

    <Command("emotes")>
    <Summary("Returns all available guild emotes.")>
    Public Async Function guildEmotesCmd() As Task 'Rework for pagination
        Await ReplyAsync(embed:=Await GeneralService.GuildEmotes(Context))
    End Function

    <Command("report")>
    <Summary("Report messages.")>
    <Remarks("\report @messageid | you get the id by right-clicking the message and then you'll see an option to copy the message id.")>
    Public Async Function Report(id As ULong) As Task
        Await ReplyAsync(embed:=Await ModService.Report(id, Context))
    End Function

    <Command("url")>
    <Summary("Gets the long url from url shorteners.")>
    <Remarks("\url https://bit.ly/3uqikrh | returns a mediafire link,")>
    Public Async Function GetUrl(url As String) As Task
        Await ReplyAsync(GeneralService.ReverseShortUrl(url))
    End Function

    <Command("test")>
    Public Async Function test(url As String) As Task
        Await ReplyAsync(embed:=Await AnimeService.GetSauce(Context, url))
    End Function
End Class
