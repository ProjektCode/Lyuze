Imports System.Globalization
Imports System.Net.Http
Imports Discord
Imports Microsoft.Extensions.DependencyInjection
Imports Tenor
Imports Tenor.Schema

NotInheritable Class GifService
    Private Shared ReadOnly _httpClientFactory As IHttpClientFactory = serviceHandler.provider.GetRequiredService(Of IHttpClientFactory)

    Public Shared Async Function TenorGif(tag As String) As Task(Of String)
        Dim settings = Lyuze.Settings.Data
        Dim horny As String() = {
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
        Dim rand As New Random

        If tag = "horny" Then
            tag = horny(rand.Next(0, horny.Length))
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
            Dim searchResults = Await client.GetRandomPostsAsync(_tag, limit:=1)

            Return searchResults.Results.First.ShortUrl.AbsoluteUri

        Catch ex As Exception
            Return $"An error ocurred - {ex.Message}"
        End Try
    End Function

    Public Shared Async Function WaifuPicsSFW(tag As String) As Task(Of String)
        Try
            Dim httpClient = _httpClientFactory.CreateClient
            Dim response = Await httpClient.GetStringAsync($"https://api.waifu.pics/sfw/{ tag}")
            Dim pic = WaifuPics.FromJson(response)

            If pic Is Nothing Then
                Return "An error occured, please try again later."
            End If

            Return pic.Url.AbsoluteUri
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

End Class
