Imports System.Net.Http
Imports Discord
Imports Discord.Commands
Imports Microsoft.Extensions.DependencyInjection

NotInheritable Class BackgroundService
    Private Shared ReadOnly _httpClientFactory As IHttpClientFactory = serviceHandler.provider.GetRequiredService(Of IHttpClientFactory)
    Private Shared ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)

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

    Public Shared Async Function SearchWallpaper(text As String, ctx As SocketCommandContext) As Task(Of Embed)
        Dim _text As String = Replace(text, " ", "+")
        Dim embed = New EmbedBuilder With {
            .Title = _text,
            .Description = Wallpapers.getSite.Result + _text,
            .Color = _utils.RandomEmbedColor,
            .Footer = New EmbedFooterBuilder With {
                .Text = ctx.Message.Timestamp.Date.ToShortDateString,
                .IconUrl = ctx.Guild.CurrentUser.GetAvatarUrl
            }
        }

        Return embed.Build
    End Function

    Public Shared Async Function RandomWallpaper(ctx As SocketCommandContext) As Task(Of Embed)
        Dim text As String = Replace(Await Wallpapers.getWord, " ", "+")
        Dim embed = New EmbedBuilder With {
            .Title = text,
            .Description = Await Wallpapers.getSite + text,
            .Color = _utils.RandomEmbedColor,
            .Footer = New EmbedFooterBuilder With {
                .Text = ctx.Message.Timestamp.Date.ToShortDateString,
                .IconUrl = ctx.Guild.CurrentUser.GetAvatarUrl
            }
        }

        Return embed.Build
    End Function

    Public Shared Async Function ListKeywords(g As IGuild, ctx As SocketCommandContext) As Task(Of Embed)
        Dim embed As New EmbedBuilder With {
            .Title = $"Wallpaper keyword list",
            .ImageUrl = "https://i.imgur.com/vc241Ku.jpeg",
            .Description = "The full list of keywords in our random wallpaper list",
            .Color = New Color(_utils.RandomEmbedColor),
            .ThumbnailUrl = g.IconUrl,
            .Timestamp = ctx.Message.Timestamp,
            .Footer = New EmbedFooterBuilder With {
                    .Text = "Keyword Data",
                    .IconUrl = g.IconUrl
                }
            }

        Dim row As Integer = 0
        Dim words As String = String.Empty

        For Each keyword As String In Wallpapers.keywords
            'If appending the keyword to the list of words exceeds 256
            'don't append, but instead add the existing words to a field.
            If words.Length + keyword.Length + 7 > 256 Then
                row += 1
                embed.AddField($"List #{row}", words, True) 'Add words to field

                'reset words
                words = String.Empty
            End If

            words = words + keyword + " **|** "
        Next

        'The add condition within the for loop is only entered when we are
        'about to exceed to field length. Any string under the max 
        'length would exit the loop without being added. Add it here
        embed.AddField($"List #{row + 1}", words, True)

        Return embed.Build()
    End Function

End Class
