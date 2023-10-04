Imports System.Net.Http
Imports Discord
Imports Discord.Commands
Imports Microsoft.Extensions.DependencyInjection

NotInheritable Class BackgroundService
    Private Shared ReadOnly _httpClientFactory As IHttpClientFactory = serviceHandler.provider.GetRequiredService(Of IHttpClientFactory)

    Public Shared Async Function UnsplashRandomImage(ctx As SocketCommandContext, <Remainder> tag As String) As Task(Of Embed)
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

            Dim dlink = unsplash.Links.DownloadLocation.AbsoluteUri
            Dim downloadResponse = Await httpClient.GetStringAsync($"{dlink}?client_id={settings.ApIs.UnsplashAccess}")
            Dim download = Lyuze.UnsplashDownloadUrl.FromJson(downloadResponse)

            If unsplash Is Nothing Then
                Return Await embedHandler.errorEmbed("Background - Unsplash", "An error occurred please try again later.")
            End If


            Dim embed = New EmbedBuilder With {
                .Title = $"{unsplash.User.Name} - {unsplash.Width}x{unsplash.Height}",
                .ImageUrl = download.Url.AbsoluteUri,
                .Url = unsplash.User.Links.Portfolio.AbsoluteUri,
                .Footer = New EmbedFooterBuilder With {
                    .Text = $"ID: {unsplash.Id} - ⬆️{unsplash.Likes} - ⏬{unsplash.Downloads} - Powered by Unsplash.com"
                }
            }
            Return embed.Build
        Catch ex As Exception
            If settings.IDs.ErrorId = Nothing Then
                loggingHandler.LogCriticalAsync("backgrounds", ex.Message)
            Else
                Dim chnl = ctx.Guild.GetTextChannel(settings.IDs.ErrorId)
                chnl.SendMessageAsync(embed:=embedHandler.errorEmbed("Backgrounds - Unsplash", "No API key given. If you wish to use this command please provide an API key into the settings config.").Result)
            End If
            Return embedHandler.errorEmbed("Backgrounds", "An error has occurred and has been logged.").Result
        End Try

    End Function

End Class
