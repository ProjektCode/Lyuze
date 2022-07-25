Imports System.Net.Http
Imports Discord
Imports Discord.Commands
Imports Microsoft.Extensions.DependencyInjection
Imports MongoDB.Bson

<Name("General")>
<Summary("Commands that don't deserve their own place.")>
Public Class General
    Inherits ModuleBase(Of SocketCommandContext)

    'Grabbing required services
    Private Shared ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)
    Private Shared ReadOnly _httpClientFactory As IHttpClientFactory = serviceHandler.provider.GetRequiredService(Of IHttpClientFactory)
    Private Shared ReadOnly _img As Images = serviceHandler.provider.GetRequiredService(Of Images)
    ' Private Shared ReadOnly _lvl As LevelingSystem = serviceHandler.provider.GetRequiredService(Of LevelingSystem)
    'Private Shared ReadOnly _gold As GoldSystem = serviceHandler.provider.GetRequiredService(Of GoldSystem)

    <Command("emotes")>
    <Summary("Returns all available guild emotes.")>
    Public Async Function guildEmotesCmd() As Task
        Await ReplyAsync(embed:=Await GeneralService.GuildEmotes(Context))
    End Function

    <Command("report")>
    <Summary("Report messages.")>
    <Remarks("\report @messageid | you get the id by right-clicking the message and then you'll see an option to copy the message id.")>
    Public Async Function Report(id As ULong) As Task
        Dim _settings = Lyuze.Settings.Data
        Dim chnl = Context.Guild.GetTextChannel(_settings.IDs.ReportId)
        Await chnl.SendMessageAsync(embed:=Await ModService.Report(id, Context))
    End Function

    <Command("url")>
    <Summary("Gets the long url from url shorteners.")>
    <Remarks("\url https://bit.ly/3uqikrh | returns a mediafire link,")>
    Public Async Function GetUrl(url As String) As Task
        Await ReplyAsync(GeneralService.ReverseShortUrl(url))
    End Function

    <Command("test")>
    Public Async Function test() As Task

        Try
            Dim client = New HttpClient()
            Dim request = New HttpRequestMessage With {
                .Method = HttpMethod.Get,
                .RequestUri = New Uri("https://anime-recommender.p.rapidapi.com/?anime_title=Plastic%20Memories&number_of_anime=5")
            }
            request.Headers.Add("X-RapidAPI-Key", "6c28fe7c95msh10d4e16c2bb2955p1a8678jsnc9f172ba1fb0")
            request.Headers.Add("X-RapidAPI-Host", "anime-recommender.p.rapidapi.com")
            Using response = Await client.SendAsync(request)
                'response.EnsureSuccessStatusCode()
                Dim body = Await response.Content.ReadAsStringAsync()
                Await ReplyAsync(body)
            End Using
        Catch ex As Exception
            ReplyAsync(ex.Message)
        End Try

    End Function
End Class
