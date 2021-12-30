Imports System.Net.Http
Imports System.Text
Imports Discord
Imports Discord.Commands
Imports Microsoft.Extensions.DependencyInjection

NotInheritable Class FunService

    Private Shared ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)
    Private Shared ReadOnly _img As Images = serviceHandler.provider.GetRequiredService(Of Images)
    Private Shared ReadOnly _httpClientFactory As IHttpClientFactory = serviceHandler.provider.GetRequiredService(Of IHttpClientFactory)


    Public Shared Async Function AnimeReddit(ctx As SocketCommandContext) As Task(Of Embed)
        Try
            Dim httpClient = _httpClientFactory.CreateClient
            Dim response = Await httpClient.GetStringAsync("https://reddit.com/r/" + _utils.getAnimeMeme + "/random.json?limit=1")
            Dim meme = Reddit.FromJson(response)
            If meme Is Nothing Then
                Return embedHandler.errorEmbed("Fun - Anime Meme", "An error occurred. Please try again later.").Result
            End If

            Dim memeFormat = meme.First.Data.Children.First
            Dim embed = New EmbedBuilder With {
                .Title = memeFormat.Data.Title,
                .Color = New Color(_utils.randomEmbedColor),
                .ImageUrl = memeFormat.Data.Thumbnail.AbsoluteUri,
                .Url = "https://reddit.com" + memeFormat.Data.Permalink,
                .Footer = New EmbedFooterBuilder With {
                    .Text = $"🗨{memeFormat.Data.NumComments} - ⬆️{memeFormat.Data.Ups}",
                    .IconUrl = ctx.Guild.IconUrl
                }
            }

            Return embed.Build
        Catch ex As Exception
            Return embedHandler.errorEmbed("Reddit", ex.Message).Result
        End Try
    End Function

    Public Shared Async Function RegularReddit(ctx As SocketCommandContext) As Task(Of Embed)
        Try
            Dim httpClient = _httpClientFactory.CreateClient
            Dim response = Await httpClient.GetStringAsync("https://reddit.com/r/" + _utils.getMeme + "/random.json?limit=1")
            Dim meme = Reddit.FromJson(response)
            If meme Is Nothing Then
                Return embedHandler.errorEmbed("Fun - Regular Meme", "An error occurred. Please try again later.").Result
            End If

            Dim memeFormat = meme.First.Data.Children.First
            Dim embed = New EmbedBuilder With {
                .Title = memeFormat.Data.Title,
                .Color = New Color(_utils.randomEmbedColor),
                .ImageUrl = memeFormat.Data.Thumbnail.AbsoluteUri,
                .Url = "https://reddit.com" + memeFormat.Data.Permalink,
                .Footer = New EmbedFooterBuilder With {
                    .Text = $"🗨{memeFormat.Data.NumComments} - ⬆️{memeFormat.Data.Ups}",
                    .IconUrl = ctx.Guild.IconUrl
                }
            }

            Return embed.Build
        Catch ex As Exception
            Return embedHandler.errorEmbed("Reddit", ex.Message).Result
        End Try
    End Function

    Public Shared Async Function GetNeko() As Task(Of Embed)
        Dim rand As New Random
        Dim neko = rand.Next(1, 578)
        Dim image As String

        If neko >= 1 And neko < 10 Then
            image = $"000{neko}"

        ElseIf neko >= 10 And neko < 100 Then
            image = $"00{neko}"
        Else
            image = $"0{neko}"
        End If

        Dim embed = New EmbedBuilder With {
            .ImageUrl = $"https://nekos.best/api/v1/nekos/{ image }.jpg",
            .Color = New Color(_utils.randomEmbedColor)
        }

        Return embed.Build
    End Function

    Public Shared Async Function GetActivity(ctx As SocketCommandContext) As Task(Of Embed)
        Dim httpClient = _httpClientFactory.CreateClient
        Dim response = Await httpClient.GetStringAsync("https://www.boredapi.com/api/activity/")
        Dim activity = ActivityEvent.FromJson(response)

        If activity Is Nothing Then
            Return embedHandler.errorEmbed("Activity", "An error occurred, please try again later").Result
        End If

        Dim embed = New EmbedBuilder With {
            .Title = "New Activity",
            .Color = New Color(_utils.randomEmbedColor),
            .ThumbnailUrl = If(ctx.User.GetAvatarUrl, ctx.User.GetDefaultAvatarUrl)
        }
        embed.AddField("Activity", $"{activity.Activity}", True)
        embed.AddField("Type", $"{activity.Type}", True)
        embed.AddField("Participants", $"{activity.Participants}", True)
        embed.AddField("Accessibility", $"{activity.Accessibility}", True)
        embed.AddField("Price", $"{activity.Price}", True)

        Return embed.Build
    End Function

    Public Shared Async Function GetJoke() As Task(Of Embed)
        Try
            Dim httpClient = _httpClientFactory.CreateClient
            Dim response = Await httpClient.GetStringAsync("https://v2.jokeapi.dev/joke/Any")
            Dim joke = RandomJoke.FromJson(response)

            If joke Is Nothing Then
                Return embedHandler.errorEmbed("Joke", "An error occurred, please try again later").Result
            End If

            If joke.Type = "single" Then

                Dim sEmbed = New EmbedBuilder With {
                    .Description = joke.Joke,
                    .Color = New Color(_utils.randomEmbedColor),
                    .Footer = New EmbedFooterBuilder With {
                        .Text = joke.Category
                    }
                }

                Return sEmbed.Build
            Else

                Dim tEmbed = New EmbedBuilder With {
                    .Description = $"**{joke.Setup}**{Environment.NewLine}*{joke.Delivery}*",
                    .Color = New Color(_utils.randomEmbedColor),
                    .Footer = New EmbedFooterBuilder With {
                        .Text = joke.Category
                    }
                }

                Return tEmbed.Build
            End If


        Catch ex As Exception
            Return embedHandler.errorEmbed("Joke", ex.Message).Result
        End Try
    End Function

    Public Shared Async Function GetDictionary(ctx As SocketCommandContext, word As String) As Task(Of Embed)
        Try
            Dim httpClient = _httpClientFactory.CreateClient
            Dim response = Await httpClient.GetStringAsync($"https://api.dictionaryapi.dev/api/v2/entries/en/{ word}")
            Dim dic = Dictionary.FromJson(response)

            If dic Is Nothing Then
                Return embedHandler.errorEmbed("Fun - Dictionary", "An error occurred please try again later.").Result
            End If

            Dim synonymsBuilder = New StringBuilder
            Dim synCount As Integer = dic.First.Meanings.First.Definitions.First.Synonyms.Count
            Dim num As Integer = 0

            For Each syn In dic.First.Meanings.First.Definitions.First.Synonyms
                If synCount > 10 Then
                    If num = 10 Then
                        Exit For
                    Else
                        synonymsBuilder.Append($"*{syn}*,")
                        num += 1
                    End If
                Else
                    synonymsBuilder.Append($"*{syn}*,")
                End If
            Next
            synonymsBuilder.Remove(synonymsBuilder.Length - 1, 1)

            Dim embed = New EmbedBuilder With {
                .Title = $"First Definition for {word}",
                .Color = New Color(_utils.randomEmbedColor),
                .ThumbnailUrl = If(ctx.Client.CurrentUser.GetAvatarUrl, ctx.Client.CurrentUser.GetDefaultAvatarUrl)
            }
            embed.AddField("Definition", dic.First.Meanings.First.Definitions.First.DefinitionDefinition)
            embed.AddField("Part of Speech", dic.First.Meanings.First.PartOfSpeech, True)
            embed.AddField("Synonyms", synonymsBuilder.ToString, True)

            Return embed.Build
        Catch ex As Exception

        End Try
    End Function

End Class
