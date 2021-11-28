Imports System.IO
Imports Discord
Imports Discord.Commands
Imports Discord.Addons.Interactive
Imports System.Net.Http
Imports System.Text
Imports Microsoft.Extensions.DependencyInjection

<Name("Anime")>
<Summary("Anything anime related.")>
Public Class Weeb
    Inherits InteractiveBase(Of SocketCommandContext)

    'Grabbing required services
    Private Shared ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)
    Private Shared ReadOnly _httpClientFactory As IHttpClientFactory = serviceHandler.provider.GetRequiredService(Of IHttpClientFactory)
    Private Shared ReadOnly _img As Images = serviceHandler.provider.GetRequiredService(Of Images)

#Region "Jikan"
#Region "Get Info"

    <Command("getanime")>
    <[Alias]("ganime")>
    <Summary("ganime | Get information on an Anime based on the provided ID.")>
    <Remarks("\ganime 1")>
    Public Async Function GetAnime(id As Integer) As Task

        Await ReplyAndDeleteAsync($"{Context.Message.Author.Mention} Please wait while I attempt to look your anime!", timeout:=New TimeSpan(0, 0, 5))
        Try
            Await Context.Channel.SendMessageAsync(embed:=jikanService.GetAnimeAsync(id, Context).Result)
        Catch ex As Exception
            ReplyAsync(embed:=embedHandler.errorEmbed("Anime", ex.Message).Result)
        End Try
        Await Context.Message.DeleteAsync()
    End Function

    <Command("getmanga")>
    <[Alias]("gmanga")>
    <Summary("gmanga | Get information on a Manga based on the provided ID.")>
    <Remarks("\gmanga 1")>
    Public Async Function GetManga(id As Integer) As Task

        Await ReplyAndDeleteAsync($"{Context.Message.Author.Mention} Please wait while I attempt to look your manga!", timeout:=New TimeSpan(0, 0, 5))
        Try
            Await Context.Channel.SendMessageAsync(embed:=jikanService.GetMangaAsync(id, Context).Result)
        Catch ex As Exception
            ReplyAsync(embed:=embedHandler.errorEmbed($"Manga - {id}", ex.Message).Result)
        End Try
        Await Context.Message.DeleteAsync()
    End Function

    <Command("getcharacter")>
    <[Alias]("gchar")>
    <Summary("gchar | Get information on a Character based on the provided ID.")>
    <Remarks("gchar 1")>
    Public Async Function GetAnimeCharacter(id As Integer) As Task

        Await ReplyAndDeleteAsync($"{Context.Message.Author.Mention} Please wait while I attempt to look your character!", timeout:=New TimeSpan(0, 0, 5))
        Try
            Await Context.Channel.SendMessageAsync(embed:=jikanService.GetCharacterAsync(id, Context).Result)
        Catch ex As Exception
            ReplyAsync(embed:=embedHandler.errorEmbed($"Staff - {id}", ex.Message).Result)
        End Try
        Await Context.Message.DeleteAsync()
    End Function

#End Region

#Region "Top Info"

    <Command("gettopanime")>
    <[Alias]("gtopa")>
    <Summary("gtopa | See the top 10 anime.")>
    <Remarks("\gtopa <option> | Options = tv, movies, special, upcoming, airing, ova, popularity, favorite")>
    Public Async Function GetTopAnime(Optional type As String = Nothing) As Task

        Try
            If type Is Nothing Then
                type = "default"
            End If

            Await ReplyAndDeleteAsync($"{Context.Message.Author.Mention} Please wait while I attempt to look your top 10!", timeout:=New TimeSpan(0, 0, 5))
            Await Context.Channel.SendMessageAsync(embed:=jikanService.GetTopAnimeAsync(type, Context).Result)
        Catch ex As Exception
            ReplyAsync(embed:=embedHandler.errorEmbed("Top Anime", ex.Message).Result)
        End Try
    End Function

    <Command("gettopmanga")>
    <[Alias]("gtopm")>
    <Summary("gtopm | See the top 10 Manga.")>
    <Remarks("\gtopm <option> | Options = doujin, favorite, manga, manhua, manhwa, novel, oneshot, popularity")>
    Public Async Function GetTopManga(Optional type As String = Nothing) As Task

        Try
            If type Is Nothing Then
                type = "default"
            End If

            Await ReplyAndDeleteAsync($"{Context.Message.Author.Mention} Please wait while I attempt to look your top 10!", timeout:=New TimeSpan(0, 0, 5))
            Await Context.Channel.SendMessageAsync(embed:=jikanService.GetTopMangaAsync(type, Context).Result)
        Catch ex As Exception
            ReplyAsync(embed:=embedHandler.errorEmbed("Top Manga", ex.Message).Result)
        End Try
    End Function

    <Command("gettopcharacter")>
    <[Alias]("gtopc")>
    <Summary("gtopc | See the top 10 Anime Characters.")>
    Public Async Function GetTopAnimeChatacter() As Task

        Try

            Await ReplyAndDeleteAsync($"{Context.Message.Author.Mention} Please wait while I attempt to look your top 10!", timeout:=New TimeSpan(0, 0, 5))
            Await Context.Channel.SendMessageAsync(embed:=jikanService.GetTopCharacterAsync(Context).Result)

        Catch ex As Exception
            ReplyAsync(embed:=embedHandler.errorEmbed("Top Manga", ex.Message).Result)
        End Try
    End Function

    <Command("gettoppeople")>
    <[Alias]("gtopp")>
    <Summary("gtopp | See the top 10 People.")>
    Public Async Function GetTopPeople() As Task

        Await ReplyAndDeleteAsync($"{Context.Message.Author.Mention} Please wait while I attempt to look your top 10!", timeout:=New TimeSpan(0, 0, 5))
        Await Context.Channel.SendMessageAsync(embed:=jikanService.GetTopPeopleAsync(Context).Result)

    End Function

    <Command("getseason")>
    <[Alias]("gseason")>
    <Summary("gseason | See 10 seasonal anime listings.")>
    Public Async Function GetSeasonalAnime() As Task

        Await ReplyAndDeleteAsync($"{Context.Message.Author.Mention} Please wait while I attempt to look your seasonal anime!", timeout:=New TimeSpan(0, 0, 5))
        Await Context.Channel.SendMessageAsync(embed:=jikanService.GetSeasonAnimeAsync(Context).Result)

    End Function

#End Region
#End Region

#Region "Anime APIs"

    'Can't figure out how to get the cover art. I am not completely happy with the results.
    <Command("manga")>
    <Summary("Get a random manga from MangaDex")>
    Public Async Function getMangaRandom() As Task

        Try
            Dim httpClient = _httpClientFactory.CreateClient
            Dim response = Await httpClient.GetStringAsync("http://api.mangadex.org/manga/random")
            Dim manga = MangaDex.FromJson(response)
            Dim description As New StringBuilder
            Dim remove As Integer = 10

            If manga Is Nothing Then
                Await ReplyAsync(embed:=embedHandler.errorEmbed("Random Manga", "Error occurred, try again later.").Result)
                Return
            End If

            Dim _manga = manga.Data.Attributes
            Dim update As Date = Date.Parse(_manga.UpdatedAt)

            description.Append(_manga.Description.En.ToString)
            While description.Length > 1000
                description.Remove(description.Length - remove, remove)
            End While
            description.Append("...")
            Dim embed = New EmbedBuilder With {
                .Title = $"{_manga.Title.En}",
                .ThumbnailUrl = Context.Client.CurrentUser.GetAvatarUrl,
                .Url = If($"https://mangadex.org/title/ {manga.Data.Id}", "https://mangadex.org"),
                .Footer = New EmbedFooterBuilder With {
                    .Text = manga.Data.Id.ToString
                }
            }

            embed.AddField("Alternate Title", Defaults.defaultValue(_manga.AltTitles.FirstOrDefault.En), True)
            embed.AddField("Type", Defaults.defaultValue(manga.Data.Type), True)
            embed.AddField("Original Language", Defaults.defaultValue(_manga.OriginalLanguage), True)
            embed.AddField("Status", Defaults.defaultValue(_manga.Status), True)
            embed.AddField("Latest chapter", Defaults.defaultValue(_manga.LastChapter), True)
            embed.AddField("Rating", Defaults.defaultValue(_manga.ContentRating), True)
            embed.AddField("Updated at", Defaults.defaultValue(update.Date.ToShortDateString), True)
            embed.AddField("Year", Defaults.defaultValue(_manga.State), True)
            embed.AddField("Description", Defaults.defaultValue(description.ToString))

            Await ReplyAsync(embed:=embed.Build)

        Catch ex As Exception
            ReplyAsync(embed:=embedHandler.errorEmbed("Manga", ex.Message).Result)
        End Try

    End Function

    <Command("aquote")>
    <Summary("Get a random anime quote.")>
    Public Async Function aQuote() As Task

        Try

            Dim httpClient = _httpClientFactory.CreateClient
            Dim response = Await httpClient.GetStringAsync("https://animechan.vercel.app/api/random/")
            Dim quote = AnimeQuote.FromJson(response)

            If quote Is Nothing Then
                Await ReplyAsync(embed:=embedHandler.errorEmbed("Anime - Quote", "An error occurred, please try again later").Result)
                Return
            End If

            Dim embed = New EmbedBuilder With {
                .Title = quote.Anime,
                .Description = $"*{quote.Quote}* - {quote.Character}",
                .Color = New Color(_utils.randomEmbedColor)
            }

            Await ReplyAsync(embed:=embed.Build)

        Catch ex As Exception
            ReplyAsync(embed:=embedHandler.errorEmbed("Anime Quote", ex.Message).Result)
        End Try

    End Function

    <Command("trace")>
    <Summary("Find what anime is from a single screenshot")>
    <Remarks("/trace https://i.imgur.com/QNyCkZh.jpeg | Can also use an attachment.")>
    Public Async Function traceImage(Optional url As String = Nothing) As Task

        Try
            'Check if url is empty
            If url Is Nothing Then
                'Check if an attachment was sent if so get the url
                If Context.Message.Attachments.Count > 0 Then
                    url = Context.Message.Attachments.First.Url

                Else
                    Await ReplyAsync(embed:=embedHandler.errorEmbed("Anime - Trace", "Please supply a url by either using it as an attachment or as a argument.").Result)
                    Return
                End If
            End If

            Dim httpClient = _httpClientFactory.CreateClient
            Dim response = Await httpClient.GetStringAsync($"https://api.trace.moe/search?url={ url }")
            Dim trace = TraceMoe.FromJson(response)
            'If some error occurs return embed
            If trace Is Nothing Then
                Await ReplyAsync(embed:=embedHandler.errorEmbed("Anime - Trace", "An error occurred, please try again later").Result)
                Return
            End If

            Dim embed = New EmbedBuilder With {
                .Title = "Most Relevant Result",
                .ImageUrl = trace.Result.First.Image.AbsoluteUri,
                .ThumbnailUrl = trace.Result.First.Image.AbsoluteUri,
                .Color = New Color(_img.RandomColorFromURL(url).Result),
                .Footer = New EmbedFooterBuilder With {
                    .Text = $"Frame Count: {trace.FrameCount}"
                }
            }
            embed.AddField("Name", trace.Result.First.Filename)
            embed.AddField("Episode", trace.Result.First.Episode, True)
            embed.AddField("Similarity", trace.Result.First.Similarity.ToString("#0.00%"), True)
            embed.AddField("AniList", $"https://anilist.co/anime/{ trace.Result.First.Anilist}")
            embed.AddField("Video", trace.Result.First.Video.AbsoluteUri)

            Await ReplyAsync(embed:=embed.Build)


        Catch ex As Exception
            ReplyAsync(embed:=embedHandler.errorEmbed("Anime - Trace", ex.Message).Result)
        End Try

    End Function

#End Region

End Class