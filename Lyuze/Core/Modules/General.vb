﻿Imports System.IO
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
    Private Shared ReadOnly _lvl As LevelingSystem = serviceHandler.provider.GetRequiredService(Of LevelingSystem)
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
        Await chnl.SendMessageAsync(embed:=Await AdminService.Report(id, Context))
    End Function

    <Command("url")>
    <Summary("Gets the long url from some url shorteners.")>
    <Remarks("\url https://bit.ly/3uqikrh | returns a mediafire link,")>
    Public Async Function GetUrl(url As String) As Task
        Await ReplyAsync(GeneralService.ReverseShortUrl(url))
    End Function

    <Command("rgb")>
    <Summary("Returns the give Hex color in RGB format")>
    <Remarks("\rgb #dc143c")>
    Public Async Function GetRGB(hex As String) As Task
        Dim color = System.Drawing.ColorTranslator.FromHtml(hex)
        Dim RGB = $"({Convert.ToInt16(color.R)},{Convert.ToInt16(color.G)},{Convert.ToInt16(color.B)})"

        Dim embed As New EmbedBuilder With {
            .Title = " RGB Color",
            .Description = $"Hex Code: {hex.ToUpper} {Environment.NewLine} RGB: {RGB}",
            .Color = New Color(_utils.ConvertToDiscordColor(hex))
            }

        Await ReplyAsync(embed:=embed.Build)
    End Function

    <Command("test")>
    Public Async Function test() As Task

        'Try
        '    Dim client = New HttpClient()
        '    Dim request = New HttpRequestMessage With {
        '        .Method = HttpMethod.Get,
        '        .RequestUri = New Uri("https://anime-recommender.p.rapidapi.com/?anime_title=Plastic%20Memories&number_of_anime=5")
        '    }
        '    request.Headers.Add("X-RapidAPI-Key", "6c28fe7c95msh10d4e16c2bb2955p1a8678jsnc9f172ba1fb0")
        '    request.Headers.Add("X-RapidAPI-Host", "anime-recommender.p.rapidapi.com")
        '    Using response = Await client.SendAsync(request)
        '        'response.EnsureSuccessStatusCode()
        '        Dim body = Await response.Content.ReadAsStringAsync()
        '        Await ReplyAsync(body)
        '    End Using
        'Catch ex As Exception
        '    ReplyAsync(ex.Message)
        'End Try




        'Try
        '    Dim settings = Lyuze.Settings.Data
        '    'Dim channel = Context.Guild.GetTextChannel(settings.IDs.WelcomeId)
        '    Dim msg = $"{Context.User.Username}#{Context.User.Discriminator} has joined the server"
        '    Dim submsg = Context.Guild.MemberCount
        '    Dim path = Await _img.CreateBannerImageAsync(Context.User, msg, submsg)
        '    Await Context.Channel.SendFileAsync(path)
        '    'Await ReplyAsync(path, String.Empty)

        '    File.Delete(path)
        'Catch ex As Exception
        '    ReplyAsync(ex.Message)
        'End Try

        'Dim player As New Player
        'Dim p As PlayerModel = Await Player.GetUser(Context.User)
        'Await ReplyAsync(_lvl.LevelEquation(p.Level))

    End Function
End Class
