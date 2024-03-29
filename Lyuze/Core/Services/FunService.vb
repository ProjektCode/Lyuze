﻿Imports System.Net.Http
Imports System.Text
Imports Discord
Imports Discord.Commands
Imports Microsoft.Extensions.DependencyInjection

NotInheritable Class FunService

    Private Shared ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)
    Private Shared ReadOnly _img As Images = serviceHandler.provider.GetRequiredService(Of Images)
    Private Shared ReadOnly _httpClientFactory As IHttpClientFactory = serviceHandler.provider.GetRequiredService(Of IHttpClientFactory)

    Public Shared Async Function GetNeko() As Task(Of Embed)

        Dim httpClient = _httpClientFactory.CreateClient
        Dim response = Await httpClient.GetStringAsync("https://nekos.best/api/v2/neko")
        Dim neko = Nekos.FromJson(response)

        If neko Is Nothing Then
            Return embedHandler.errorEmbed("Neko", "An error occurred, please try again later.").Result
        End If

        Dim embed = New EmbedBuilder With {
            .Title = neko.Results.FirstOrDefault.ArtistName,
            .ImageUrl = neko.Results.FirstOrDefault.Url.AbsoluteUri,
            .Color = New Color(Await _img.RandomColorFromURL(neko.Results.FirstOrDefault.Url.AbsoluteUri)),
            .Url = neko.Results.FirstOrDefault.SourceUrl.AbsoluteUri,
            .Timestamp = DateTime.Now,
            .Footer = New EmbedFooterBuilder With {
                .IconUrl = neko.Results.FirstOrDefault.Url.AbsoluteUri,
                .Text = neko.Results.FirstOrDefault.ArtistName
            }
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
            .Color = New Color(_utils.RandomEmbedColor),
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
                    .Color = New Color(_utils.RandomEmbedColor),
                    .Footer = New EmbedFooterBuilder With {
                        .Text = joke.Category
                    }
                }

                Return sEmbed.Build
            Else

                Dim tEmbed = New EmbedBuilder With {
                    .Description = $"**{joke.Setup}**{Environment.NewLine}*{joke.Delivery}*",
                    .Color = New Color(_utils.RandomEmbedColor),
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
                .Color = New Color(_utils.RandomEmbedColor),
                .ThumbnailUrl = If(ctx.Client.CurrentUser.GetAvatarUrl, ctx.Client.CurrentUser.GetDefaultAvatarUrl)
            }
            embed.AddField("Definition", dic.First.Meanings.First.Definitions.First.DefinitionDefinition)
            embed.AddField("Part of Speech", dic.First.Meanings.First.PartOfSpeech, True)
            embed.AddField("Synonyms", synonymsBuilder.ToString, True)

            Return embed.Build
        Catch ex As Exception

        End Try
    End Function

    Public Shared Async Function UselessFact(ctx As SocketCommandContext) As Task(Of Embed)
        Try
            Dim httpClient = _httpClientFactory.CreateClient
            Dim response = Await httpClient.GetStringAsync("https://uselessfacts.jsph.pl/random.json?language=en")
            Dim fact = UselessFactsModel.FromJson(response)

            If fact Is Nothing Then
                Return embedHandler.errorEmbed("Fun - Useless Fact", "An error occurred please try again later.").Result
            End If

            Dim embed = New EmbedBuilder With {
                .Title = fact.Source,
                .Description = fact.Text,
                .Color = New Color(_utils.RandomEmbedColor),
                .Url = fact.SourceUrl.AbsoluteUri,
                .Footer = New EmbedFooterBuilder With {
                    .Text = fact.Id.ToString,
                    .IconUrl = If(ctx.Guild.IconUrl, ctx.User.GetAvatarUrl)
                }
            }

            Return embed.Build
        Catch ex As Exception
            Return embedHandler.errorEmbed("Fun - Usless Fact", ex.Message).Result
        End Try
    End Function

    Public Shared Async Function Quote(ctx As SocketCommandContext, id As ULong) As Task(Of Embed)
        Try
            Dim msg = ctx.Channel.GetMessageAsync(id)
            Dim embed = New EmbedBuilder With {
                .Title = msg.Result.Author.ToString,
                .Description = $"*""{msg.Result.Content} - {Date.Now.Year}*""",
                .ThumbnailUrl = If(msg.Result.Author.GetAvatarUrl, msg.Result.Author.GetDefaultAvatarUrl),
                .Color = New Color(_utils.RandomEmbedColor)
            }
            Return embed.Build
        Catch ex As Exception

        End Try
    End Function

    Public Shared Async Function WouldYouRatherAsync(ctx As SocketCommandContext) As Task(Of Embed)
        Try
            Dim httpClient = _httpClientFactory.CreateClient
            Dim response = Await httpClient.GetStringAsync("https://would-you-rather-api.abaanshanid.repl.co")
            Dim wyr = WouldYouRather.FromJson(response)

            If wyr Is Nothing Then
                Return embedHandler.errorEmbed("Fun - Useless Fact", "An error occurred please try again later.").Result
            End If

            Dim embed = New EmbedBuilder With {
                .Title = "Would You Rather...",
                .Description = wyr.Data,
                .Color = New Color(_utils.RandomEmbedColor),
                .ThumbnailUrl = If(ctx.User.GetAvatarUrl, ctx.User.GetDefaultAvatarUrl),
                .Footer = New EmbedFooterBuilder With {
                    .Text = wyr.Id.ToString,
                    .IconUrl = If(ctx.Guild.IconUrl, ctx.User.GetAvatarUrl)
                }
            }

            Return embed.Build
        Catch ex As Exception
            Return embedHandler.errorEmbed("Fun - Usless Fact", ex.Message).Result
        End Try
    End Function

End Class
