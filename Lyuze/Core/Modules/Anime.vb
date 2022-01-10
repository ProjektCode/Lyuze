Imports Discord.Commands
Imports Discord.Addons.Interactive

<Name("Anime")>
<Summary("Anything anime related.")>
Public Class Weeb
    Inherits InteractiveBase(Of SocketCommandContext)

#Region "Jikan"

#Region "Get Info"

    <Command("getanime")>
    <[Alias]("ganime")>
    <Summary("ganime | Get information on an Anime based on the provided ID.")>
    <Remarks("\ganime 1")>
    Public Async Function GetAnime(id As Integer) As Task

        Await ReplyAndDeleteAsync($"{Context.Message.Author.Mention} Please wait while I attempt to look your anime!", timeout:=New TimeSpan(0, 0, 5))
        Try
            Await Context.Channel.SendMessageAsync(embed:=Await AnimeService.GetAnimeAsync(id, Context))
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
            Await Context.Channel.SendMessageAsync(embed:=Await AnimeService.GetMangaAsync(id, Context))
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
            Await Context.Channel.SendMessageAsync(embed:=Await AnimeService.GetCharacterAsync(id, Context))
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
    Public Async Function GetTopAnime(Optional type As String = "default") As Task

        Try
            Await ReplyAndDeleteAsync($"{Context.Message.Author.Mention} Please wait while I attempt to look your top 10!", timeout:=New TimeSpan(0, 0, 5))
            Await Context.Channel.SendMessageAsync(embed:=Await AnimeService.GetTopAnimeAsync(type, Context))
        Catch ex As Exception
            ReplyAsync(embed:=embedHandler.errorEmbed("Top Anime", ex.Message).Result)
        End Try
    End Function

    <Command("gettopmanga")>
    <[Alias]("gtopm")>
    <Summary("gtopm | See the top 10 Manga.")>
    <Remarks("\gtopm <option> | Options = doujin, favorite, manga, manhua, manhwa, novel, oneshot, popularity")>
    Public Async Function GetTopManga(Optional type As String = "default") As Task

        Try

            Await ReplyAndDeleteAsync($"{Context.Message.Author.Mention} Please wait while I attempt to look your top 10!", timeout:=New TimeSpan(0, 0, 5))
            Await Context.Channel.SendMessageAsync(embed:=Await AnimeService.GetTopMangaAsync(type, Context))
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
            Await Context.Channel.SendMessageAsync(embed:=Await AnimeService.GetTopCharacterAsync(Context))

        Catch ex As Exception
            ReplyAsync(embed:=embedHandler.errorEmbed("Top Manga", ex.Message).Result)
        End Try
    End Function

    <Command("gettoppeople")>
    <[Alias]("gtopp")>
    <Summary("gtopp | See the top 10 People.")>
    Public Async Function GetTopPeople() As Task

        Await ReplyAndDeleteAsync($"{Context.Message.Author.Mention} Please wait while I attempt to look your top 10!", timeout:=New TimeSpan(0, 0, 5))
        Await Context.Channel.SendMessageAsync(embed:=Await AnimeService.GetTopPeopleAsync(Context))

    End Function

    <Command("getseason")>
    <[Alias]("gseason")>
    <Summary("gseason | See 10 seasonal anime listings.")>
    Public Async Function GetSeasonalAnime() As Task

        Await ReplyAndDeleteAsync($"{Context.Message.Author.Mention} Please wait while I attempt to look your seasonal anime!", timeout:=New TimeSpan(0, 0, 5))
        'Await Context.Channel.SendMessageAsync(embed:=Await jikanService.GetSeasonAnimeAsync(Context))

    End Function

#End Region

#Region "Search"
    <Command("asearch")>
    <[Alias]("as")>
    <Summary("Gets the first 10 results from the given name.")>
    <Remarks("\as code geass | gets the first 10 search results.")>
    Public Async Function SearchAnime(<Remainder> query As String) As Task
        Await ReplyAsync(embed:=Await AnimeService.AnimeSearchAsync(Context, query))
    End Function
#End Region

#End Region

#Region "Anime APIs"

    <Command("aquote")>
    <Summary("Get a random anime quote.")>
    Public Async Function aQuote() As Task
        Await ReplyAsync(embed:=Await AnimeService.GetAnimeQuote())
    End Function

    <Command("trace")>
    <Summary("Find what anime is from a single screen-shot")>
    <Remarks("/trace https://i.imgur.com/QNyCkZh.jpeg | Can also use an attachment.")>
    Public Async Function traceImage(Optional url As String = Nothing) As Task
        Await ReplyAsync(embed:=Await AnimeService.GetTraceAnime(Context, url))
    End Function

#End Region

End Class