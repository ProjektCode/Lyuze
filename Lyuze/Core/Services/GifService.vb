Imports System.Globalization
Imports System.Net.Http
Imports Discord
Imports Discord.Commands
Imports Microsoft.Extensions.DependencyInjection
Imports Tenor
Imports Tenor.Schema

NotInheritable Class GifService
    Private Shared ReadOnly _httpClientFactory As IHttpClientFactory = serviceHandler.provider.GetRequiredService(Of IHttpClientFactory)
    Private Shared ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)

    Public Shared Async Function TenorGif(ctx As SocketCommandContext, tag As String) As Task(Of String)
        Dim settings = Lyuze.Settings.Data

        'Check to see if there's an API Key
        If _utils.CheckAPI(settings.ApIs.Tenor) = False Then
            Return "No API Key was given. Please provide an API Key in the settings config."
        End If
        Dim rand As New Random

        If tag = "horny" Then
            Dim hrny As String() = {
                "oppai",
                "boobs",
                "booty",
                "horny",
                "ecchi",
                "lewd",
                "ass",
                "tits",
                "boobies"
            }

            tag = hrny(rand.Next(hrny.Length))
        End If

        Try
            Dim config = New TenorConfiguration With {
            .ApiKey = settings.ApIs.Tenor,
            .AspectRatio = AspectRatio.All,
            .ContentFilter = ContentFilter.Off,
            .Locale = CultureInfo.GetCultureInfo("en"),
            .MediaFilter = MediaFilter.Minimal
        }
            Dim client = New TenorClient(config)

            Dim _tag = $"anime {tag}"
            Dim searchResults = Await client.GetRandomPostsAsync(_tag, limit:=100)
            Dim result As Uri = searchResults.Results(rand.Next(searchResults.Results.Count)).ShortUrl

            Return result.AbsoluteUri

        Catch ex As Exception
            If Not settings.IDs.ErrorId Then
                loggingHandler.LogCriticalAsync("gif", ex.Message)
            Else
                Dim chnl = ctx.Guild.GetTextChannel(settings.IDs.ErrorId)
                chnl.SendMessageAsync(embed:=embedHandler.errorEmbed("Gifs - Tenor", ex.Message).Result)
            End If
            Return "An error occurred and has been logged."
        End Try
    End Function

    Public Shared Async Function WaifuPicsSFW(tag As String) As Task(Of String)
        Try
            Dim httpClient = _httpClientFactory.CreateClient
            Dim response = Await httpClient.GetStringAsync($"https://api.waifu.pics/sfw/{ tag}")
            Dim pic = WaifuPics.FromJson(response)

            If pic Is Nothing Then
                Return "An error occurred, please try again later."
            End If

            Return pic.Url.AbsoluteUri
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

End Class
