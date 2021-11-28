Imports System.Text
Imports Discord
Imports Discord.Commands
Imports JikanDotNet

NotInheritable Class jikanService

    Private Shared ReadOnly _jikan As New Jikan(True)
    Private Shared ReadOnly _utils As New MasterUtils
    Private Shared ReadOnly removeLength As Integer = 11

    Private Shared oldID As Long
    Private Shared trackedID As Boolean = False
    Private Shared retryLimit As Integer = 1

    Public Shared Async Function GetAnimeAsync(id As Integer, ctx As SocketCommandContext) As Task(Of Embed)
Line1:
        Try

            Dim a As Anime = Await _jikan.GetAnime(id)
            Dim s As AnimeStats = Await _jikan.GetAnimeStatistics(id)
#Region "String Builders"
            Dim licensorBuilder = New StringBuilder
            Dim genreBuilder = New StringBuilder
            Dim studioBuilder = New StringBuilder
            Dim synopsisBuilder = New StringBuilder
            Dim endingBuilder = New StringBuilder
            Dim producerBuilder = New StringBuilder
            synopsisBuilder.Append(a.Synopsis)

            If a.Licensors.Count = 1 Then
                licensorBuilder.Append($"{a.Licensors.FirstOrDefault}")
            Else
                For Each li In a.Licensors
                    licensorBuilder.Append($"{li.Name},{Environment.NewLine}")
                Next
                licensorBuilder.Remove(licensorBuilder.Length - 3, 3)
            End If

            If a.Producers.Count = 1 Then
                producerBuilder.Append($"{a.Producers.FirstOrDefault}")
            Else
                For Each pro In a.Producers
                    producerBuilder.Append($"{pro.Name},{Environment.NewLine}")
                Next
                producerBuilder.Remove(producerBuilder.Length - 3, 3)
            End If

            If a.Studios.Count = 1 Then
                studioBuilder.Append($"{a.Studios.FirstOrDefault}")
            Else
                For Each studio In a.Studios
                    studioBuilder.Append($"{studio.Name},{Environment.NewLine}")
                Next
                studioBuilder.Remove(studioBuilder.Length - 3, 3)
            End If

            If a.Genres.Count = 1 Then
                genreBuilder.Append($"{a.Genres.FirstOrDefault}")
            Else
                For Each gen In a.Genres
                    genreBuilder.Append($"{gen.Name},{Environment.NewLine}")
                Next
                genreBuilder.Remove(genreBuilder.Length - 3, 3)
            End If

            While synopsisBuilder.Length > 1021
                synopsisBuilder.Remove(synopsisBuilder.Length - removeLength, removeLength)
            End While
            synopsisBuilder.Append("...")
#End Region

#Region "Embed"

            Dim embed = New EmbedBuilder With {
            .Title = Defaults.defaultValue(a.Title),
            .Color = New Color(_utils.randomEmbedColor),
            .ImageUrl = If(a.ImageURL, "https://i.imgur.com/Kl2Qrd2.png"),
            .ThumbnailUrl = If(a.ImageURL, "https://i.imgur.com/Kl2Qrd2.png"),
            .Footer = New EmbedFooterBuilder With {
                 .Text = $"ID:{Defaults.defaultValue(a.MalId)} - {Defaults.defaultValue(a.LinkCanonical)}",
                 .IconUrl = If(a.ImageURL, "https://i.imgur.com/Kl2Qrd2.png")
            }
            }
            embed.AddField("Japanese Title", Defaults.defaultValue(a.TitleJapanese), True)
            embed.AddField("Title Synonyms", If(a.TitleSynonyms.FirstOrDefault, "N/A"), True)
            embed.AddField("Type", Defaults.defaultValue(a.Type), True)
            embed.AddField("Source", Defaults.defaultValue(a.Source), True)
            embed.AddField("Premiered", Defaults.defaultValue(a.Premiered), True)
            embed.AddField("Status", Defaults.defaultValue(a.Status), True)
            embed.AddField("Episodes", Defaults.defaultValue(a.Episodes), True)
            embed.AddField("Popularity Rank", Defaults.defaultValue(a.Popularity), True)
            embed.AddField("Rank", Defaults.defaultValue(a.Rank), True)
            embed.AddField("Score", Defaults.defaultValue(a.Score), True)
            embed.AddField("Rating", Defaults.defaultValue(a.Rating), True)
            embed.AddField("Genres", Defaults.defaultValue(genreBuilder.ToString), True)
            embed.AddField("Producers", Defaults.defaultValue(producerBuilder.ToString), True)
            embed.AddField("Studios", Defaults.defaultValue(studioBuilder.ToString), True)
            embed.AddField("Licensors", Defaults.defaultValue(licensorBuilder.ToString), True)
            embed.AddField("Completed Users", If(s.Completed.ToString.Format("{0:N0}", s.Completed), "N/A"), True)
            embed.AddField("On-Hold Users", If(s.OnHold.ToString.Format("{0:N0}", s.OnHold), "N/A"), True)
            embed.AddField("Dropped Users", If(s.Dropped.ToString.Format("{0:N0}", s.Dropped), "N/A"), True)
            embed.AddField("Synopsis", Defaults.defaultValue(synopsisBuilder.ToString))
#End Region

            retryLimit = 1
            trackedID = False
            Return embed.Build

        Catch ex As Exception
            'StartIndex cannot be less than zero. (Parameter 'startIndex') - synopsis is too big
            'Field value length must be less than Or equal To 1024. (Parameter 'Value') - same as above
            'Field value must not be null or empty. (Parameter 'Value') - Something is null.(this has been solved)
            'GET request failed. Status code: NotFound Inner message: System.Net.Http.HttpConnectionResponseContent - ID does not exist
            'GET request failed. Status code: ServiceUnavailable Inner message: System.Net.Http.HttpConnectionResponseContent - request failed

            If trackedID = False Then
                oldID = id
                trackedID = True
            End If

            'Will stop after 10 logged attempts.
            If retryLimit = 11 Then
                trackedID = False
                Return embedHandler.errorEmbed("Jikan", $"Retry limit has been reached. Could not find an existing id within {oldID}-{id}.").Result
            End If
            'if the current ID does not exist it will increase the number to find the closest existing ID.
            If ex.Message.Contains("System.Net.Http.HttpConnectionResponseContent") Then
                'loggingHandler.ErrorLog("Anime", ex.Message, id)
                id += 1
                retryLimit += 1
                GoTo Line1
            Else
                'chnl.SendMessageAsync(embedHandler.errorEmbed($"{id}", $"{ex.Message}").ToString)
                'loggingHandler.ErrorLog("Anime", ex.Message, id)
                Return embedHandler.errorEmbed("Anime", $"{ex.Message}").Result
            End If

        End Try

    End Function

    Public Shared Async Function GetMangaAsync(id As Integer, ctx As SocketCommandContext) As Task(Of Embed)
Line1:
        Try

            Dim m As Manga = Await _jikan.GetManga(id)
            Dim s As MangaStats = Await _jikan.GetMangaStatistics(id)

#Region "String Builders"
            Dim genreBuilder = New StringBuilder
            Dim synopsisBuilder = New StringBuilder
            Dim authorBuilder = New StringBuilder
            synopsisBuilder.Append(m.Synopsis)

            If m.Genres.Count = 1 Then
                genreBuilder.Append($"{m.Genres.First}")
            Else
                For Each gen In m.Genres
                    genreBuilder.Append($"{gen.Name},{Environment.NewLine}")
                Next
                genreBuilder.Remove(genreBuilder.Length - 3, 3)
            End If
            If m.Authors.Count = 1 Then
                authorBuilder.Append($"{m.Authors.First}")
            Else
                For Each authors In m.Authors
                    authorBuilder.Append($"{authors.Name},{Environment.NewLine}")
                Next
                authorBuilder.Remove(authorBuilder.Length - 3, 3)
            End If

            While synopsisBuilder.Length > 1021
                synopsisBuilder.Remove(synopsisBuilder.Length - removeLength, removeLength)
            End While
            synopsisBuilder.Append("...")
#End Region

#Region "Embed"

            Dim embed = Await Task.Run(Function() New EmbedBuilder() _
            .WithTitle(Defaults.defaultValue(If(m.TitleEnglish, m.Title))) _
            .WithColor(New Color(_utils.randomEmbedColor)) _
            .WithImageUrl(If(m.ImageURL, "https://i.imgur.com/Kl2Qrd2.png")) _
            .WithThumbnailUrl(If(m.ImageURL, "https://i.imgur.com/Kl2Qrd2.png")) _
            .WithFooter(New EmbedFooterBuilder With {
                 .Text = $"ID: {Defaults.defaultValue(m.MalId)} - {Defaults.defaultValue(m.LinkCanonical)}",
                 .IconUrl = If(m.ImageURL, "https://i.imgur.com/Kl2Qrd2.png")
            }) _
            .AddField("Japanese Title", Defaults.defaultValue(m.TitleJapanese), True) _
            .AddField("Title Synonyms", If(m.TitleSynonyms.FirstOrDefault, "N/A"), True) _
            .AddField("Type", Defaults.defaultValue(m.Type), True) _
            .AddField("Authors", Defaults.defaultValue(authorBuilder.ToString), True) _
            .AddField("Status", Defaults.defaultValue(m.Status), True) _
            .AddField("Chapters", Defaults.defaultValue(m.Chapters), True) _
            .AddField("Volumes", Defaults.defaultValue(m.Volumes), True) _
            .AddField("Publishing", Defaults.defaultValue(m.Publishing), True) _
            .AddField("Popularity Rank", Defaults.defaultValue(m.Popularity), True) _
            .AddField("Rank", Defaults.defaultValue(m.Rank), True) _
            .AddField("Score", Defaults.defaultValue(m.Score), True) _
            .AddField("Genres", Defaults.defaultValue(genreBuilder.ToString), True) _
            .AddField("Completed Users", If(s.Completed.ToString.Format("{0:N0}", s.Completed), "N/A"), True) _
            .AddField("On-Hold Users", If(s.OnHold.ToString.Format("{0:N0}", s.OnHold), "N/A"), True) _
            .AddField("Dropped Users", If(s.Dropped.ToString.Format("{0:N0}", s.Dropped), "N/A"), True) _
            .AddField("Synopsis", Defaults.defaultValue(synopsisBuilder.ToString)) _
            .Build())

            Return embed

#End Region

            retryLimit = 1
            trackedID = False
            Return embed

        Catch ex As Exception
            'StartIndex cannot be less than zero. (Parameter 'startIndex') - synopsis is too big
            'Field value length must be less than Or equal To 1024. (Parameter 'Value') - same as above
            'Field value must not be null or empty. (Parameter 'Value') - Something is null.(this has been solved)
            'GET request failed. Status code: NotFound Inner message: System.Net.Http.HttpConnectionResponseContent - ID does not exist
            'GET request failed. Status code: ServiceUnavailable Inner message: System.Net.Http.HttpConnectionResponseContent - request failed

            If trackedID = False Then
                oldID = id
                trackedID = True
            End If

            'Will stop after 10 logged attempts.
            If retryLimit = 11 Then
                trackedID = False
                Return embedHandler.errorEmbed("Jikan", $"Retry limit has been reached. Could not find an existing id within {oldID}-{id}.").Result
            End If
            'if the current ID does not exist it will increase the number to find the closest existing ID.
            If ex.Message.Contains("System.Net.Http.HttpConnectionResponseContent") Then
                loggingHandler.ErrorLog("Manga", ex.Message, id)
                id += 1
                retryLimit += 1
                GoTo Line1
            Else
                'chnl.SendMessageAsync(embedHandler.errorEmbed($"{id}", $"{ex.Message}").ToString)
                loggingHandler.ErrorLog("Manga", ex.Message, id)
                Return embedHandler.errorEmbed($"Manga - {id}", $"{ex.Message}").Result
            End If

        End Try

    End Function

    Public Shared Async Function GetCharacterAsync(id As Integer, ctx As SocketCommandContext) As Task(Of Embed)
Line1:
        Try

            Dim _char As Character = Await _jikan.GetCharacter(id)

#Region "String Builders"
            Dim animeBuilder As New StringBuilder
            Dim aboutBuilder As New StringBuilder
            Dim mangaBuilder As New StringBuilder
            Dim voice As String = $"{_char.VoiceActors.FirstOrDefault.Name} - {_char.VoiceActors.FirstOrDefault.Language}"
            aboutBuilder.Append(_char.About)

            If _char.Animeography.Count = 1 Then
                animeBuilder.Append($"{_char.Animeography.First}")
            Else
                For Each ani In _char.Animeography
                    animeBuilder.Append($"{ani.Name},{Environment.NewLine}")
                Next
                animeBuilder.Remove(animeBuilder.Length - 3, 3)
            End If

            While aboutBuilder.Length > 1021
                aboutBuilder.Remove(aboutBuilder.Length - removeLength, removeLength)
            End While
            aboutBuilder.Append("...")

            If _char.Mangaography.Count = 1 Then
                mangaBuilder.Append($"{_char.Mangaography.First}")
            Else
                For Each manga In _char.Mangaography
                    mangaBuilder.Append($"{manga.Name},{Environment.NewLine}")
                Next
                mangaBuilder.Remove(mangaBuilder.Length - 3, 3)
            End If

#End Region

#Region "Embed"

            Dim embed = New EmbedBuilder With {
                .Title = _char.Name,
                .Color = New Color(_utils.randomEmbedColor),
                .ImageUrl = If(_char.ImageURL, "https://i.imgur.com/Kl2Qrd2.png"),
                .ThumbnailUrl = If(_char.ImageURL, "https://i.imgur.com/Kl2Qrd2.png"),
                .Footer = New EmbedFooterBuilder With {
                    .Text = $"ID: {_char.MalId} - {_char.LinkCanonical}",
                    .IconUrl = If(_char.ImageURL, "https://i.imgur.com/Kl2Qrd2.png")
                }
            }
            embed.AddField("Japanese Name", _char.NameKanji, True)
            embed.AddField("Nicknames", If(_char.Nicknames.FirstOrDefault, "N/A"), True)
            embed.AddField("Member Favorites", _char.MemberFavorites, True)
            embed.AddField("Animeography", animeBuilder.ToString, True)
            embed.AddField("Mangaography", mangaBuilder.ToString, True)
            embed.AddField("Voice Actors", If(voice, "N/A"), True)
            embed.AddField("About", aboutBuilder.ToString)

#End Region

            retryLimit = 1
            trackedID = False
            Return embed.Build

        Catch ex As Exception
            'StartIndex cannot be less than zero. (Parameter 'startIndex') - synopsis is too big
            'Field value length must be less than Or equal To 1024. (Parameter 'Value') - same as above
            'Field value must not be null or empty. (Parameter 'Value') - Something is null.(this has been solved)
            'GET request failed. Status code: NotFound Inner message: System.Net.Http.HttpConnectionResponseContent - ID does not exist
            'GET request failed. Status code: ServiceUnavailable Inner message: System.Net.Http.HttpConnectionResponseContent - request failed

            If trackedID = False Then
                oldID = id
                trackedID = True
            End If

            'Will stop after 10 logged attempts.
            If retryLimit = 11 Then
                trackedID = False
                Return embedHandler.errorEmbed("Jikan", $"Retry limit has been reached. Could not find an existing id within {oldID}-{id}.").Result
            End If
            'if the current ID does not exist it will increase the number to find the closest existing ID.
            If ex.Message.Contains("System.Net.Http.HttpConnectionResponseContent") Then
                'loggingHandler.ErrorLog("Manga", ex.Message, id)
                id += 1
                retryLimit += 1
                GoTo Line1
            Else
                'chnl.SendMessageAsync(embedHandler.errorEmbed($"{id}", $"{ex.Message}").ToString)
                'loggingHandler.ErrorLog("Manga", ex.Message, id)
                Return embedHandler.errorEmbed($"Manga - {id}", $"{ex.Message}").Result
            End If

        End Try

    End Function

    Public Shared Async Function GetTopAnimeAsync(tag As String, ctx As SocketCommandContext) As Task(Of Embed)

        Try

            Dim a As AnimeTop
            Dim type As String = "TV Anime"

            Select Case tag.ToLower
                Case "default"
                    a = Await _jikan.GetAnimeTop()
                Case "ova"
                    a = Await _jikan.GetAnimeTop(TopAnimeExtension.TopOva)
                    type = "Ovas"
                Case "movies"
                    a = Await _jikan.GetAnimeTop(TopAnimeExtension.TopMovies)
                    type = "Movies"
                Case "special"
                    a = Await _jikan.GetAnimeTop(TopAnimeExtension.TopSpecial)
                    type = "Specials"
                Case "tv"
                    a = Await _jikan.GetAnimeTop(TopAnimeExtension.TopTV)
                    type = "TV Anime"
                Case "upcoming"
                    a = Await _jikan.GetAnimeTop(TopAnimeExtension.TopUpcoming)
                    type = "Upcoming"
                Case "popularity"
                    a = Await _jikan.GetAnimeTop(TopAnimeExtension.TopPopularity)
                    type = "Popularity"
                Case "airing"
                    a = Await _jikan.GetAnimeTop(TopAnimeExtension.TopAiring)
                    type = "Airing"
                Case "favorite"
                    a = Await _jikan.GetAnimeTop(TopAnimeExtension.TopFavorite)
                    type = "Favorited"
                Case Else
                    a = Await _jikan.GetAnimeTop()
            End Select

            Dim trackNum As Integer = 0
            Dim embed = New EmbedBuilder With {
                .Title = $"Top 10 {type}",
                .Color = New Color(_utils.randomEmbedColor),
                .ImageUrl = If(a.Top.FirstOrDefault.ImageURL, "https://i.imgur.com/Kl2Qrd2.png")
            }

            For Each aEntry In a.Top
                If trackNum = 10 Then
                    Return embed.Build()
                End If
                embed.AddField($"[{aEntry.Rank}]", $"{aEntry.Score}: *{aEntry.Title}*{Environment.NewLine}{aEntry.Url}")
                trackNum += 1
            Next

        Catch ex As Exception
            Return embedHandler.errorEmbed("Top Anime", ex.Message).Result
        End Try
    End Function

    Public Shared Async Function GetTopMangaAsync(tag As String, ctx As SocketCommandContext) As Task(Of Embed)

        Try

            Dim m As MangaTop
            Dim type As String = "Manga"

            Select Case tag.ToLower
                Case "default"
                    m = Await _jikan.GetMangaTop()
                Case "doujin"
                    m = Await _jikan.GetMangaTop(TopMangaExtension.TopDoujinshi)
                    type = "Doujins"
                Case "favorite"
                    m = Await _jikan.GetMangaTop(TopMangaExtension.TopFavorite)
                    type = "Favorited"
                Case "manga"
                    m = Await _jikan.GetMangaTop(TopMangaExtension.TopManga)
                    type = "Manga"
                Case "manhua"
                    m = Await _jikan.GetMangaTop(TopMangaExtension.TopManhua)
                    type = "Manhua"
                Case "manhwa"
                    m = Await _jikan.GetMangaTop(TopMangaExtension.TopManhwa)
                    type = "Manhwa"
                Case "novel"
                    m = Await _jikan.GetMangaTop(TopMangaExtension.TopNovel)
                    type = "Novels"
                Case "oneshot"
                    m = Await _jikan.GetMangaTop(TopMangaExtension.TopOneShot)
                    type = "Oneshots"
                Case "popularity"
                    m = Await _jikan.GetMangaTop(TopMangaExtension.TopPopularity)
                    type = "Popularity"
                Case Else
                    m = Await _jikan.GetMangaTop()
            End Select

            Dim trackNum As Integer = 0

            Dim embed = New EmbedBuilder With {
                .Title = $"Top 10 {type}",
                .Color = New Color(_utils.randomEmbedColor),
                .ImageUrl = If(m.Top.FirstOrDefault.ImageURL, "https://i.imgur.com/Kl2Qrd2.png")
            }

            For Each mEntry In m.Top
                If trackNum = 10 Then
                    Return embed.Build()
                End If
                embed.AddField($"[{mEntry.Rank}]", $"{mEntry.Score}: *{mEntry.Title}*{Environment.NewLine}{mEntry.Url}")
                trackNum += 1
            Next

        Catch ex As Exception
            Return embedHandler.errorEmbed("Top Manga", ex.Message).Result
        End Try
    End Function

    Public Shared Async Function GetTopCharacterAsync(ctx As SocketCommandContext) As Task(Of Embed)

        Try

            Dim c As CharactersTop = Await _jikan.GetCharactersTop()
            Dim trackNum As Integer = 0

            Dim embed = New EmbedBuilder With {
                .Title = $"Top 10 Characters",
                .Color = New Color(_utils.randomEmbedColor),
                .ImageUrl = If(c.Top.FirstOrDefault.ImageURL, "https://i.imgur.com/Kl2Qrd2.png")
            }

            For Each cEntry In c.Top
                If trackNum = 10 Then
                    Return embed.Build()
                End If
                embed.AddField($"[{cEntry.Rank}]", $"{cEntry.Favorites}: *{cEntry.Name}*{Environment.NewLine}{cEntry.Url}")
                trackNum += 1
            Next

        Catch ex As Exception
            Return embedHandler.errorEmbed("Top Characters", ex.Message).Result
        End Try
    End Function

    Public Shared Async Function GetTopPeopleAsync(ctx As SocketCommandContext) As Task(Of Embed)

        Try

            Dim p As PeopleTop = Await _jikan.GetPeopleTop()
            Dim trackNum As Integer = 0

            Dim embed = New EmbedBuilder With {
                .Title = $"Top 10 People",
                .Color = New Color(_utils.randomEmbedColor),
                .ImageUrl = If(p.Top.FirstOrDefault.ImageURL, "https://i.imgur.com/Kl2Qrd2.png")
            }

            For Each pEntry In p.Top
                If trackNum = 10 Then
                    Return embed.Build()
                End If
                embed.AddField($"[{pEntry.Rank}]", $"{pEntry.Favorites}: *{pEntry.Name}*{Environment.NewLine}{pEntry.Url}")
                trackNum += 1
            Next

        Catch ex As Exception
            Return embedHandler.errorEmbed("Top People", ex.Message).Result
        End Try
    End Function

    Public Shared Async Function GetSeasonAnimeAsync(ctx As SocketCommandContext) As Task(Of Embed)

        Try

            Dim s As Season = Await _jikan.GetSeason()
            Dim trackNum As Integer = 0

            Dim embed = New EmbedBuilder With {
                .Title = $"10 Seasonal Animes: {s.SeasonName} ({s.SeasonYear})",
                .Color = New Color(_utils.randomEmbedColor),
                .ImageUrl = If(s.SeasonEntries.FirstOrDefault.ImageURL, "https://i.imgur.com/Kl2Qrd2.png"),
                .Footer = New EmbedFooterBuilder With {
                    .Text = "Find more here https://myanimelist.net/anime/season",
                    .IconUrl = ctx.Client.CurrentUser.GetAvatarUrl
                }
            }

            For Each sEntry In s.SeasonEntries
                If trackNum = 10 Then
                    Return embed.Build()
                End If
                embed.AddField($"[{sEntry.MalId} - {sEntry.Title}]", $"`{sEntry.AiringStart.GetValueOrDefault.Now.ToShortDateString} - {sEntry.Type} - {sEntry.Genres.FirstOrDefault}`{Environment.NewLine}{sEntry.URL}")
                trackNum += 1
            Next

        Catch ex As Exception
            Return embedHandler.errorEmbed("Season Anime", ex.Message).Result
        End Try

    End Function

End Class