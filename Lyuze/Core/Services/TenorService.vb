Imports System.Globalization
Imports Discord
Imports Microsoft.Extensions.DependencyInjection
Imports Tenor
Imports Tenor.Schema

NotInheritable Class TenorService
    Private Shared _settings As settingsHandler
    Public Shared Async Function animeGif(tag As String) As Task(Of String)
        _settings = settingsHandler.Load
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
            .ApiKey = _settings.tenorAPIKey,
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

End Class
