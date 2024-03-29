﻿Imports Discord.Commands
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
        Await ReplyAsync(embed:=Await AnimeService.GetAnimeAsync(id, Context))
    End Function

    <Command("getmanga")>
    <[Alias]("gmanga")>
    <Summary("gmanga | Get information on a Manga based on the provided ID.")>
    <Remarks("\gmanga 1")>
    Public Async Function GetManga(id As Integer) As Task
        Await ReplyAndDeleteAsync($"{Context.Message.Author.Mention} Please wait while I attempt to look your manga!", timeout:=New TimeSpan(0, 0, 5))
        Await ReplyAsync(embed:=Await AnimeService.GetMangaAsync(id, Context))
    End Function

    <Command("getcharacter")>
    <[Alias]("gchar")>
    <Summary("gchar | Get information on a Character based on the provided ID.")>
    <Remarks("gchar 1")>
    Public Async Function GetAnimeCharacter(id As Integer) As Task
        Await ReplyAndDeleteAsync($"{Context.Message.Author.Mention} Please wait while I attempt to look your character!", timeout:=New TimeSpan(0, 0, 5))
        Await ReplyAsync(embed:=Await AnimeService.GetCharacterAsync(id))
    End Function

#End Region

#Region "Top Info"

#Region "waiting for updated docs"
    '<Command("gettopanime")>
    '<[Alias]("gtopa")>
    '<Summary("gtopa | See the top 10 anime.")>
    '<Remarks("\gtopa <option> | Options = tv, movies, special, upcoming, airing, ova, popularity, favorite")>
    'Public Async Function GetTopAnime(Optional type As String = "default") As Task

    '    Try
    '        Await ReplyAndDeleteAsync($"{Context.Message.Author.Mention} Please wait while I attempt to look your top 10!", timeout:=New TimeSpan(0, 0, 5))
    '        Await ReplyAsync(embed:=Await AnimeService.GetTopAnimeAsync(type, Context))
    '    Catch ex As Exception
    '        ReplyAsync(embed:=embedHandler.errorEmbed("Top Anime", ex.Message).Result)
    '    End Try
    'End Function

    '<Command("gettopmanga")>
    '<[Alias]("gtopm")>
    '<Summary("gtopm | See the top 10 Manga.")>
    '<Remarks("\gtopm <option> | Options = doujin, favorite, manga, manhua, manhwa, novel, oneshot, popularity")>
    'Public Async Function GetTopManga(Optional type As String = "default") As Task

    '    Try

    '        Await ReplyAndDeleteAsync($"{Context.Message.Author.Mention} Please wait while I attempt to look your top 10!", timeout:=New TimeSpan(0, 0, 5))
    '        Await ReplyAsync(embed:=Await AnimeService.GetTopMangaAsync(type, Context))
    '    Catch ex As Exception
    '        ReplyAsync(embed:=embedHandler.errorEmbed("Top Manga", ex.Message).Result)
    '    End Try
    'End Function

    '<Command("gettopcharacter")>
    '<[Alias]("gtopc")>
    '<Summary("gtopc | See the top 10 Anime Characters.")>
    'Public Async Function GetTopAnimeChatacter() As Task

    '    Try

    '        Await ReplyAndDeleteAsync($"{Context.Message.Author.Mention} Please wait while I attempt to look your top 10!", timeout:=New TimeSpan(0, 0, 5))
    '        Await ReplyAsync(embed:=Await AnimeService.GetTopCharacterAsync(Context))

    '    Catch ex As Exception
    '        ReplyAsync(embed:=embedHandler.errorEmbed("Top Manga", ex.Message).Result)
    '    End Try
    'End Function

    '<Command("gettoppeople")>
    '<[Alias]("gtopp")>
    '<Summary("gtopp | See the top 10 People.")>
    'Public Async Function GetTopPeople() As Task

    '    Await ReplyAndDeleteAsync($"{Context.Message.Author.Mention} Please wait while I attempt to look your top 10!", timeout:=New TimeSpan(0, 0, 5))
    '    Await ReplyAsync(embed:=Await AnimeService.GetTopPeopleAsync(Context))

    'End Function
#End Region

    <Command("getseason")>
    <[Alias]("gseason")>
    <Summary("gseason | See 10 seasonal anime listings.")>
    Public Async Function GetSeasonalAnime(season As String, Optional _date As Integer = 0) As Task
        Await ReplyAsync(embed:=Await AnimeService.GetSeasonalAsync(season, _date))
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

    <Command("csearch")>
    <[Alias]("cs")>
    <Summary("Gets the first 10 results with the given character name.")>
    <Remarks("\cs Edward Elric")>
    Public Async Function SearchCharacter(<Remainder> query As String) As Task
        Await ReplyAsync(embed:=Await AnimeService.CharacterSearchAsync(Context, query))
    End Function

    <Command("psearch")>
    <[Alias]("ps")>
    <Summary("Get the first 10 results from the given name")>
    <Remarks("\ps john")>
    Public Async Function SearchPerson(query As String) As Task
        Await ReplyAsync(embed:=Await AnimeService.PersonSearchAsync(Context, query))
    End Function
#End Region
#End Region

#Region "Anime APIs"
    <Command("animequote")>
    <[Alias]("aquote")>
    <Summary("Get a random anime quote.")>
    Public Async Function aQuote() As Task
        Await ReplyAsync(embed:=Await AnimeService.GetAnimeQuote())
    End Function

    <Command("trace")>
    <Summary("Find what anime is from a single screen-shot")>
    <Remarks("\trace https://i.imgur.com/QNyCkZh.jpeg | Can also use a message attachment.")>
    Public Async Function traceImage(Optional url As String = Nothing) As Task
        Await ReplyAsync(embed:=Await AnimeService.TraceAnimeAsync(Context, url))
    End Function

    <Command("sauce")>
    <Summary("Get the sauce of an image with decent results")>
    <Remarks("\sauce https://i.imgur.com/WRCuQAG.jpg or \sauce [discord image attachment]")>
    Public Async Function Sauce(Optional url As String = Nothing) As Task
        If Context.Message.Attachments.Count = 1 Then
            url = Context.Message.Attachments.First.Url
        End If
        Await ReplyAsync(embed:=Await AnimeService.GetSauce(url))
    End Function
#End Region

End Class