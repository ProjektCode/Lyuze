Imports System.Net.Http
Imports Discord
Imports Discord.Commands
Imports Microsoft.Extensions.DependencyInjection

NotInheritable Class BackgroundService
    Private Shared ReadOnly _httpClientFactory As IHttpClientFactory = serviceHandler.provider.GetRequiredService(Of IHttpClientFactory)


    Public Shared Async Function UnsplashRandomImage(<Remainder> tag As String) As Task(Of Embed)
        Dim settings = Lyuze.Settings.Data
        Dim newTag As String
        Try
            If tag.Contains(" ") Then
                newTag = tag.Replace(" ", "+")
            Else
                newTag = tag
            End If

            Dim httpClient = _httpClientFactory.CreateClient
            Dim response = Await httpClient.GetStringAsync($"https://api.unsplash.com/photos/random?client_id={ settings.ApIs.UnsplashAccess}&query={newTag}")
            Dim unsplash = Lyuze.Unsplash.FromJson(response)

            If unsplash Is Nothing Then
                Return embedHandler.errorEmbed("Background - Unsplash", "An error occurred please try again later.").Result
            End If


            Dim embed = New EmbedBuilder With {
                .Title = $"{unsplash.User.Name} - {unsplash.Width}x{unsplash.Height}",
                .ImageUrl = unsplash.Links.Download.AbsoluteUri,
                .Url = unsplash.Urls.Small.AbsoluteUri,
                .Footer = New EmbedFooterBuilder With {
                    .Text = $"ID: {unsplash.Id} - ⬆️{unsplash.Likes} - ⏬{unsplash.Downloads}"
                }
            }
            Return embed.Build
        Catch ex As Exception
            Return embedHandler.errorEmbed("Background - Unsplash", ex.Message).Result
        End Try
    End Function

End Class
