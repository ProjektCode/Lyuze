Imports System.Net.Http
Imports System.Text
Imports Discord
Imports Discord.Commands
Imports JikanDotNet
Imports Microsoft.Extensions.DependencyInjection

NotInheritable Class AnimeService

    Private Shared ReadOnly _jikan As New Jikan(New Config.JikanClientConfiguration With {.SuppressException = True})
    Private Shared ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)
    Private Shared ReadOnly _httpClientFactory As IHttpClientFactory = serviceHandler.provider.GetRequiredService(Of IHttpClientFactory)
    Private Shared ReadOnly _img As Images = serviceHandler.provider.GetRequiredService(Of Images)

    Private Shared ReadOnly removeLength As Integer = 11
    Private Shared ReadOnly searchLimit As Integer = 10
    Private Shared ReadOnly fieldLimit As Integer = 3
    Private Shared ReadOnly defaultImage As String = "https://i.imgur.com/Kl2Qrd2.png"

    Private Shared oldID As Long
    Private Shared trackedID As Boolean = False
    Private Shared retryLimit As Integer = 1
    Private Shared searchLimitStart As Integer = 0
    Private Shared ReadOnly fieldLimitStart As Integer = 0
    Private Shared numLimiter As Integer = 0
    Private Shared searchList As String = String.Empty

    Public Shared Async Function GetAnimeAsync(id As Integer, ctx As SocketCommandContext) As Task(Of Embed)
Line1:
        Try

            Dim a = Await _jikan.GetAnimeAsync(id)
            Dim s = Await _jikan.GetAnimeStatisticsAsync(id)
#Region "String Builders"
            Dim licensorBuilder = New StringBuilder
            Dim genreBuilder = New StringBuilder
            Dim studioBuilder = New StringBuilder
            Dim synopsisBuilder = New StringBuilder
            Dim endingBuilder = New StringBuilder
            Dim producerBuilder = New StringBuilder
            synopsisBuilder.Append(a.Data.Synopsis)

            If a.Data.Licensors.Count = 1 Then
                licensorBuilder.Append($"{a.Data.Licensors.FirstOrDefault}")
            Else
                For Each li In a.Data.Licensors
                    licensorBuilder.Append($"{li.Name},{Environment.NewLine}")
                Next
                licensorBuilder.Remove(licensorBuilder.Length - 3, 3)
            End If

            If a.Data.Producers.Count = 1 Then
                producerBuilder.Append($"{a.Data.Producers.FirstOrDefault}")
            Else
                For Each pro In a.Data.Producers
                    producerBuilder.Append($"{pro.Name},{Environment.NewLine}")
                Next
                producerBuilder.Remove(producerBuilder.Length - 3, 3)
            End If

            If a.Data.Studios.Count = 1 Then
                studioBuilder.Append($"{a.Data.Studios.FirstOrDefault}")
            Else
                For Each studio In a.Data.Studios
                    studioBuilder.Append($"{studio.Name},{Environment.NewLine}")
                Next
                studioBuilder.Remove(studioBuilder.Length - 3, 3)
            End If

            If a.Data.Genres.Count = 1 Then
                genreBuilder.Append($"{a.Data.Genres.FirstOrDefault}")
            Else
                For Each gen In a.Data.Genres
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
            .Title = Defaults.defaultValue(a.Data.Title),
            .Color = New Color(_utils.RandomEmbedColor),
            .ImageUrl = If(a.Data.Images.JPG.ImageUrl, defaultImage),
            .ThumbnailUrl = If(a.Data.Images.JPG.ImageUrl, defaultImage),
            .Footer = New EmbedFooterBuilder With {
                 .Text = $"ID:{Defaults.defaultValue(a.Data.MalId)} - {Defaults.defaultValue(a.Data.Url)}",
                 .IconUrl = If(a.Data.Images.JPG.ImageUrl, defaultImage)
            }
            }
            embed.AddField("Japanese Title", Defaults.defaultValue(a.Data.TitleJapanese), True)
            embed.AddField("Title Synonyms", If(a.Data.TitleSynonyms.FirstOrDefault, "N/A"), True)
            embed.AddField("Type", Defaults.defaultValue(a.Data.Type), True)
            embed.AddField("Source", Defaults.defaultValue(a.Data.Source), True)
            embed.AddField("Aired", Defaults.defaultValue($"{a.Data.Aired.From.Value.ToShortDateString} - {a.Data.Aired.To.Value.ToShortDateString}"), True)
            embed.AddField("Status", Defaults.defaultValue(a.Data.Status), True)
            embed.AddField("Episodes", Defaults.defaultValue(a.Data.Episodes), True)
            embed.AddField("Popularity Rank", Defaults.defaultValue(a.Data.Popularity), True)
            embed.AddField("Rank", Defaults.defaultValue(a.Data.Rank), True)
            embed.AddField("Score", Defaults.defaultValue(a.Data.Score), True)
            embed.AddField("Rating", Defaults.defaultValue(a.Data.Rating), True)
            embed.AddField("Genres", Defaults.defaultValue(genreBuilder.ToString), True)
            embed.AddField("Producers", Defaults.defaultValue(producerBuilder.ToString), True)
            embed.AddField("Studios", Defaults.defaultValue(studioBuilder.ToString), True)
            embed.AddField("Licensors", Defaults.defaultValue(licensorBuilder.ToString), True)
            embed.AddField("Completed Users", Defaults.defaultValue(s.Data.Completed.ToString.Format("{0:N0}", s.Data.Completed)), True)
            embed.AddField("On-Hold Users", If(s.Data.OnHold.ToString.Format("{0:N0}", s.Data.OnHold), "N/A"), True)
            embed.AddField("Dropped Users", If(s.Data.Dropped.ToString.Format("{0:N0}", s.Data.Dropped), "N/A"), True)
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
                loggingHandler.LogCriticalAsync("jikan", ex.Message)
                id += 1
                retryLimit += 1
                GoTo Line1
            Else
                Dim _settings = Lyuze.Settings.Data

                If _settings.IDs.ErrorId = 0 Then
                    loggingHandler.LogCriticalAsync("jikan", ex.Message)
                Else
                    Dim chnl = ctx.Guild.GetTextChannel(_settings.IDs.ErrorId)
                    chnl.SendMessageAsync(embed:=embedHandler.errorEmbed("jikan", ex.Message).Result)
                End If
            End If

        End Try

    End Function

    Public Shared Async Function GetMangaAsync(id As Integer, ctx As SocketCommandContext) As Task(Of Embed)
Line1:
        Try

            Dim m = Await _jikan.GetMangaAsync(id)
            Dim s = Await _jikan.GetMangaStatisticsAsync(id)

#Region "String Builders"
            Dim genreBuilder = New StringBuilder
            Dim synopsisBuilder = New StringBuilder
            Dim authorBuilder = New StringBuilder
            synopsisBuilder.Append(m.Data.Synopsis)

            If m.Data.Genres.Count = 1 Then
                genreBuilder.Append($"{m.Data.Genres.First}")
            Else
                For Each gen In m.Data.Genres
                    genreBuilder.Append($"{gen.Name},{Environment.NewLine}")
                Next
                genreBuilder.Remove(genreBuilder.Length - 3, 3)
            End If
            If m.Data.Authors.Count = 1 Then
                authorBuilder.Append($"{m.Data.Authors.First}")
            Else
                For Each authors In m.Data.Authors
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

            Dim embed = New EmbedBuilder With {
            .Title = Defaults.defaultValue(If(m.Data.TitleEnglish, m.Data.Title)),
            .Color = New Color(_utils.RandomEmbedColor),
            .ImageUrl = If(m.Data.Images.JPG.ImageUrl, defaultImage),
            .ThumbnailUrl = If(m.Data.Images.JPG.ImageUrl, defaultImage),
            .Footer = New EmbedFooterBuilder With {
                 .Text = $"ID: {Defaults.defaultValue(m.Data.MalId)} - {Defaults.defaultValue(m.Data.Url)}",
                 .IconUrl = If(m.Data.Images.JPG.ImageUrl, defaultImage)
            }
            }
            embed.AddField("Japanese Title", Defaults.defaultValue(m.Data.TitleJapanese), True)
            embed.AddField("Title Synonyms", If(m.Data.TitleSynonyms.FirstOrDefault, "N/A"), True)
            embed.AddField("Type", Defaults.defaultValue(m.Data.Type), True)
            embed.AddField("Authors", Defaults.defaultValue(authorBuilder.ToString), True)
            embed.AddField("Status", Defaults.defaultValue(m.Data.Status), True)
            embed.AddField("Chapters", Defaults.defaultValue(m.Data.Chapters), True)
            embed.AddField("Volumes", Defaults.defaultValue(m.Data.Volumes), True)
            embed.AddField("Publishing", Defaults.defaultValue(m.Data.Publishing), True)
            embed.AddField("Popularity Rank", Defaults.defaultValue(m.Data.Popularity), True)
            embed.AddField("Rank", Defaults.defaultValue(m.Data.Rank), True)
            embed.AddField("Score", Defaults.defaultValue(m.Data.Score), True)
            embed.AddField("Genres", Defaults.defaultValue(genreBuilder.ToString), True)
            embed.AddField("Completed Users", If(s.Data.Completed.ToString.Format("{0:N0}", s.Data.Completed), "N/A"), True)
            embed.AddField("On-Hold Users", If(s.Data.OnHold.ToString.Format("{0:N0}", s.Data.OnHold), "N/A"), True)
            embed.AddField("Dropped Users", If(s.Data.Dropped.ToString.Format("{0:N0}", s.Data.Dropped), "N/A"), True)
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
                Dim _settings = Lyuze.Settings.Data

                If _settings.IDs.ErrorId = 0 Then
                    loggingHandler.LogCriticalAsync("jikan - Manga", ex.Message)
                Else
                    Dim chnl = ctx.Guild.GetTextChannel(_settings.IDs.ErrorId)
                    chnl.SendMessageAsync(embed:=embedHandler.errorEmbed("jikan", ex.Message).Result)
                End If
                id += 1
                retryLimit += 1
                GoTo Line1
            Else
                Dim _settings = Lyuze.Settings.Data

                If _settings.IDs.ErrorId = 0 Then
                    loggingHandler.LogCriticalAsync($"jikan", ex.Message)
                Else
                    Dim chnl = ctx.Guild.GetTextChannel(_settings.IDs.ErrorId)
                    chnl.SendMessageAsync(embed:=embedHandler.errorEmbed($"jikan", ex.Message).Result)
                End If
            End If

        End Try

    End Function

    Public Shared Async Function GetCharacterAsync(id As Integer) As Task(Of Embed)
Line1:
        Try

            Dim _char = Await _jikan.GetCharacterAsync(id)

#Region "String Builders"
            Dim aboutBuilder As New StringBuilder
            aboutBuilder.Append(_char.Data.About)

            While aboutBuilder.Length > 1021
                aboutBuilder.Remove(aboutBuilder.Length - removeLength, removeLength)
            End While
            aboutBuilder.Append("...")
#End Region

#Region "Embed"

            Dim embed = New EmbedBuilder With {
                .Title = _char.Data.Name,
                .Color = New Color(_utils.RandomEmbedColor),
                .ImageUrl = If(_char.Data.Images.JPG.ImageUrl, defaultImage),
                .ThumbnailUrl = If(_char.Data.Images.JPG.ImageUrl, defaultImage),
                .Footer = New EmbedFooterBuilder With {
                    .Text = $"ID: {_char.Data.MalId} - {_char.Data.Url}",
                    .IconUrl = If(_char.Data.Images.JPG.ImageUrl, defaultImage)
                }
            }
            embed.AddField("Japanese Name", _char.Data.NameKanji, True)
            embed.AddField("Nicknames", If(_char.Data.Nicknames.FirstOrDefault, "N/A"), True)
            embed.AddField("Member Favorites", _char.Data.Favorites, True)
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
                Return embedHandler.errorEmbed("Anime - Character Search", ex.Message).Result
            End If

        End Try

    End Function

    Public Shared Async Function GetSeasonalAsync(season As String, year As Integer) As Task(Of Embed)
        Try
            Dim _season As String
            Dim sType As JikanDotNet.Season
            Select Case season.ToLower
                Case "spring"
                    _season = sType.Spring
                Case "summer"
                    _season = sType.Summer
                Case "fall"
                    _season = sType.Fall
                Case "winter"
                    _season = sType.Winter
                Case Else
                    _season = sType.Spring
            End Select

            If year = 0 Then
                year = Date.Now.Year
            End If
            If year + 2 > Date.Now.Year Then
                year = Date.Now.Year
            End If

            Dim seasonal = Await _jikan.GetSeasonAsync(year, _season)
            Dim sData = seasonal.Data
            Dim desc As String = String.Empty

            For Each sea In seasonal.Data
                If Not numLimiter = 10 Then
                    desc += $"*[{sea.MalId}]* - **{sea.Title}**{Environment.NewLine}{sea.Url}{Environment.NewLine}"
                Else
                    Exit For
                End If
                numLimiter += 1
            Next
            numLimiter = 0
            Dim embed = New EmbedBuilder With {
                .Title = $"This Season's Anime",
                .Description = desc,
                .ImageUrl = If(sData.FirstOrDefault.Images.JPG.ImageUrl, defaultImage),
                .Color = New Color(Await _img.RandomColorFromURL(sData.FirstOrDefault.Images.JPG.ImageUrl)),
                .ThumbnailUrl = If(sData.FirstOrDefault.Images.JPG.ImageUrl, defaultImage),
                .Footer = New EmbedFooterBuilder With {
                    .IconUrl = If(sData.FirstOrDefault.Images.JPG.ImageUrl, defaultImage),
                    .Text = "10 Seasonal anime"
                }
            }

            Return embed.Build
        Catch ex As Exception
            Return embedHandler.errorEmbed("Anime - Season", ex.Message).Result
        End Try
    End Function

#Region "Waiting for updated docs"
    'Public Shared Async Function GetAnimeTopAsync(tag As String, ctx As SocketCommandContext) As Task(Of Embed)

    '    Try

    '        Dim a As Object
    '        Dim type As String = "TV Anime"

    '        Select Case tag.ToLower
    '            Case "default"
    '                a = Await _jikan.GetTopAnimeAsync()
    '            Case "ova"
    '                a = Await _jikan.GetTopAnimeAsync(TopAnimeExtension.TopOva)
    '                type = "Ovas"
    '            Case "movies"
    '                a = Await _jikan.GetTopAnimeAsync(TopAnimeExtension.TopMovies)
    '                type = "Movies"
    '            Case "special"
    '                a = Await _jikan.GetTopAnimeAsync(TopAnimeExtension.TopSpecial)
    '                type = "Specials"
    '            Case "tv"
    '                a = Await _jikan.GetTopAnimeAsync(TopAnimeExtension.TopTV)
    '                type = "TV Anime"
    '            Case "upcoming"
    '                a = Await _jikan.GetTopAnimeAsync(TopAnimeExtension.TopUpcoming)
    '                type = "Upcoming"
    '            Case "popularity"
    '                a = Await _jikan.GetTopAnimeAsync(TopAnimeExtension.TopPopularity)
    '                type = "Popularity"
    '            Case "airing"
    '                a = Await _jikan.GetTopAnimeAsync(TopAnimeExtension.TopAiring)
    '                type = "Airing"
    '            Case "favorite"
    '                a = Await _jikan.GetTopAnimeAsync(TopAnimeExtension.TopFavorite)
    '                type = "Favorited"
    '            Case Else
    '                a = Await _jikan.GetTopAnimeAsync()
    '        End Select

    '        Dim trackNum As Integer = 0
    '        Dim embed = New EmbedBuilder With {
    '            .Title = $"Top 10 {type}",
    '            .Color = New Color(_utils.RandomEmbedColor),
    '            .ImageUrl = If(a.Top.FirstOrDefault.ImageURL, defaultImage)
    '        }

    '        For Each aEntry In a.Top
    '            If trackNum = 10 Then
    '                Return embed.Build()
    '            End If
    '            embed.AddField($"[{aEntry.Rank}]", $"{aEntry.Score}: *{aEntry.Title}*{Environment.NewLine}{aEntry.Url}")
    '            trackNum += 1
    '        Next

    '    Catch ex As Exception
    '        Dim _settings = Lyuze.Settings.Data

    '        If _settings.IDs.ErrorId = 0 Then
    '            loggingHandler.LogCriticalAsync($"jikan", ex.Message)
    '        Else
    '            Dim chnl = ctx.Guild.GetTextChannel(_settings.IDs.ErrorId)
    '            chnl.SendMessageAsync(embed:=embedHandler.errorEmbed($"jikan", ex.Message).Result)
    '        End If
    '    End Try
    'End Function

    'Public Shared Async Function GetTopMangaAsync(tag As String, ctx As SocketCommandContext) As Task(Of Embed)

    '    Try

    '        Dim m As MangaTop
    '        Dim type As String = "Manga"

    '        Select Case tag.ToLower
    '            Case "default"
    '                m = Await _jikan.GetMangaTop()
    '            Case "doujin"
    '                m = Await _jikan.GetMangaTop(TopMangaExtension.TopDoujinshi)
    '                type = "Doujins"
    '            Case "favorite"
    '                m = Await _jikan.GetMangaTop(TopMangaExtension.TopFavorite)
    '                type = "Favorited"
    '            Case "manga"
    '                m = Await _jikan.GetMangaTop(TopMangaExtension.TopManga)
    '                type = "Manga"
    '            Case "manhua"
    '                m = Await _jikan.GetMangaTop(TopMangaExtension.TopManhua)
    '                type = "Manhua"
    '            Case "manhwa"
    '                m = Await _jikan.GetMangaTop(TopMangaExtension.TopManhwa)
    '                type = "Manhwa"
    '            Case "novel"
    '                m = Await _jikan.GetMangaTop(TopMangaExtension.TopNovel)
    '                type = "Novels"
    '            Case "oneshot"
    '                m = Await _jikan.GetMangaTop(TopMangaExtension.TopOneShot)
    '                type = "Oneshots"
    '            Case "popularity"
    '                m = Await _jikan.GetMangaTop(TopMangaExtension.TopPopularity)
    '                type = "Popularity"
    '            Case Else
    '                m = Await _jikan.GetMangaTop()
    '        End Select

    '        Dim trackNum As Integer = 0

    '        Dim embed = New EmbedBuilder With {
    '            .Title = $"Top 10 {type}",
    '            .Color = New Color(_utils.RandomEmbedColor),
    '            .ImageUrl = If(m.Top.FirstOrDefault.ImageURL, defaultImage)
    '        }

    '        For Each mEntry In m.Top
    '            If trackNum = 10 Then
    '                Return embed.Build()
    '            End If
    '            embed.AddField($"[{mEntry.Rank}]", $"{mEntry.Score}: *{mEntry.Title}*{Environment.NewLine}{mEntry.Url}")
    '            trackNum += 1
    '        Next

    '    Catch ex As Exception
    '        Dim _settings = Lyuze.Settings.Data

    '        If _settings.IDs.ErrorId = 0 Then
    '            loggingHandler.LogCriticalAsync($"jikan", ex.Message)
    '        Else
    '            Dim chnl = ctx.Guild.GetTextChannel(_settings.IDs.ErrorId)
    '            chnl.SendMessageAsync(embed:=embedHandler.errorEmbed($"jikan", ex.Message).Result)
    '        End If
    '    End Try
    'End Function

    'Public Shared Async Function GetTopCharacterAsync(ctx As SocketCommandContext) As Task(Of Embed)

    '    Try

    '        Dim c As CharactersTop = Await _jikan.GetCharactersTop()
    '        Dim trackNum As Integer = 0

    '        Dim embed = New EmbedBuilder With {
    '            .Title = $"Top 10 Characters",
    '            .Color = New Color(_utils.RandomEmbedColor),
    '            .ImageUrl = If(c.Top.FirstOrDefault.ImageURL, defaultImage)
    '        }

    '        For Each cEntry In c.Top
    '            If trackNum = 10 Then
    '                Return embed.Build()
    '            End If
    '            embed.AddField($"[{cEntry.Rank}]", $"{cEntry.Favorites}: *{cEntry.Name}*{Environment.NewLine}{cEntry.Url}")
    '            trackNum += 1
    '        Next

    '    Catch ex As Exception
    '        Dim _settings = Lyuze.Settings.Data

    '        If _settings.IDs.ErrorId = 0 Then
    '            loggingHandler.LogCriticalAsync($"jikan", ex.Message)
    '        Else
    '            Dim chnl = ctx.Guild.GetTextChannel(_settings.IDs.ErrorId)
    '            chnl.SendMessageAsync(embed:=embedHandler.errorEmbed($"Jikan", ex.Message).Result)
    '        End If
    '    End Try
    'End Function

    'Public Shared Async Function GetTopPeopleAsync(ctx As SocketCommandContext) As Task(Of Embed)

    '    Try

    '        Dim p As PeopleTop = Await _jikan.GetPeopleTop()
    '        Dim trackNum As Integer = 0

    '        Dim embed = New EmbedBuilder With {
    '            .Title = $"Top 10 People",
    '            .Color = New Color(_utils.RandomEmbedColor),
    '            .ImageUrl = If(p.Top.FirstOrDefault.ImageURL, defaultImage)
    '        }

    '        For Each pEntry In p.Top
    '            If trackNum = 10 Then
    '                Return embed.Build()
    '            End If
    '            embed.AddField($"[{pEntry.Rank}]", $"{pEntry.Favorites}: *{pEntry.Name}*{Environment.NewLine}{pEntry.Url}")
    '            trackNum += 1
    '        Next

    '    Catch ex As Exception
    '        Dim _settings = Lyuze.Settings.Data

    '        If _settings.IDs.ErrorId = 0 Then
    '            loggingHandler.LogCriticalAsync($"jikan", ex.Message)
    '        Else
    '            Dim chnl = ctx.Guild.GetTextChannel(_settings.IDs.ErrorId)
    '            chnl.SendMessageAsync(embed:=embedHandler.errorEmbed($"Jikan", ex.Message).Result)
    '        End If
    '    End Try
    'End Function
#End Region

    Public Shared Async Function AnimeSearchAsync(ctx As SocketCommandContext, query As String) As Task(Of Embed)
        Try
            searchList = String.Empty
            Dim searchResult = Await _jikan.SearchAnimeAsync(query)
            For Each anime In searchResult.Data
                If searchLimitStart = searchLimit Then
                    Exit For
                End If
                searchList += $"[{anime.MalId}] - {anime.Title} - {anime.Url}{Environment.NewLine}"
                searchLimitStart += 1
            Next

            Dim embed As New EmbedBuilder With {
                .Title = "Top 10 most relevant search results",
                .Description = searchList,
                .Color = New Color(_utils.RandomEmbedColor),
                .ThumbnailUrl = If(ctx.Client.CurrentUser.GetAvatarUrl, ctx.Client.CurrentUser.GetDefaultAvatarUrl),
                .Footer = New EmbedFooterBuilder With {
                    .Text = "Anime Search Results"
                }
            }

            Return embed.Build
        Catch ex As Exception
            Return embedHandler.errorEmbed("Anime - Search", ex.Message).Result
        End Try
    End Function

    Public Shared Async Function CharacterSearchAsync(ctx As SocketCommandContext, query As String) As Task(Of Embed)
        Try
            searchList = String.Empty
            Dim searchResult = Await _jikan.SearchCharacterAsync(query)
            For Each [char] In searchResult.Data
                If searchLimitStart = searchLimit Then
                    Exit For
                End If
                searchList += $"[{[char].MalId}] - {[char].Name} - {[char].Url}{Environment.NewLine}"
                searchLimitStart += 1
            Next

            Dim embed As New EmbedBuilder With {
                .Title = "Top 10 most relevant search results",
                .Description = searchList,
                .Color = New Color(_utils.RandomEmbedColor),
                .ThumbnailUrl = If(ctx.Client.CurrentUser.GetAvatarUrl, ctx.Client.CurrentUser.GetDefaultAvatarUrl),
                .Footer = New EmbedFooterBuilder With {
                    .Text = "Character Search Results"
                }
            }

            Return embed.Build
        Catch ex As Exception
            Return embedHandler.errorEmbed("Anime - Search - Character", ex.Message).Result
        End Try
    End Function

    Public Shared Async Function PersonSearchAsync(ctx As SocketCommandContext, query As String) As Task(Of Embed)
        Try
            searchList = String.Empty
            Dim searchResult = Await _jikan.SearchPersonAsync(query)
            For Each pers In searchResult.Data
                If searchLimitStart = searchLimit Then
                    Exit For
                End If
                searchList += $"[{pers.MalId}] - {pers.Name} - {pers.Url}{Environment.NewLine}"
                searchLimitStart += 1
            Next

            Dim embed As New EmbedBuilder With {
                .Title = "Top 10 most relevant search results",
                .Description = searchList,
                .Color = New Color(_utils.RandomEmbedColor),
                .ThumbnailUrl = If(ctx.Client.CurrentUser.GetAvatarUrl, ctx.Client.CurrentUser.GetDefaultAvatarUrl),
                .Footer = New EmbedFooterBuilder With {
                    .Text = "Person Search Results"
                }
            }

            Return embed.Build
        Catch ex As Exception
            Return embedHandler.errorEmbed("Anime - Search - Character", ex.Message).Result
        End Try
    End Function

#Region "APIs"

    Public Shared Async Function GetAnimeQuote() As Task(Of Embed)
        Try

            Dim httpClient = _httpClientFactory.CreateClient
            Dim response = Await httpClient.GetStringAsync("https://animechan.vercel.app/api/random/")
            Dim quote = AnimeQuote.FromJson(response)

            If quote Is Nothing Then
                Return Await embedHandler.errorEmbed("Anime - Quote", "An error occurred, please try again later")
            End If

            Dim embed = New EmbedBuilder With {
                .Title = quote.Anime,
                .Description = $"*{quote.Quote}* - {quote.Character}",
                .Color = New Color(_utils.RandomEmbedColor)
            }

            Return embed.Build

        Catch ex As Exception
            Return embedHandler.errorEmbed("Anime Quote", ex.Message).Result
        End Try
    End Function

    Public Shared Async Function TraceAnimeAsync(ctx As SocketCommandContext, Optional url As String = Nothing) As Task(Of Embed)
        Try
            'Check if URL is empty
            If url Is Nothing Then
                'Check if an attachment was sent if so get the URL
                If ctx.Message.Attachments.Count > 0 Then
                    url = ctx.Message.Attachments.First.Url

                Else
                    Return Await embedHandler.errorEmbed("Anime - Trace", "Please supply a URL by either using it as an attachment or as a argument.")
                End If
            End If

            Dim httpClient = _httpClientFactory.CreateClient
            Dim response = Await httpClient.GetStringAsync($"https://api.trace.moe/search?url={url}")
            Dim trace = TraceMoe.FromJson(response)
            'If some error occurs return embed
            If trace Is Nothing Then
                Return Await embedHandler.errorEmbed("Anime - Trace", "An error occurred, please try again later")
            End If

            Dim embed = New EmbedBuilder With {
                .Title = "Most Relevant Result",
                .ImageUrl = trace.Result.First.Image.AbsoluteUri,
                .ThumbnailUrl = trace.Result.First.Image.AbsoluteUri,
                .Color = New Color(Await _img.RandomColorFromURL(url)),
                .Footer = New EmbedFooterBuilder With {
                    .Text = $"Frame Count: {trace.FrameCount}"
                }
            }
            embed.AddField("Name", trace.Result.First.Filename)
            embed.AddField("Episode", trace.Result.First.Episode, True)
            embed.AddField("Similarity", trace.Result.First.Similarity.ToString("#0.00%"), True)
            embed.AddField("AniList", $"https://anilist.co/anime/{ trace.Result.First.Anilist}")
            embed.AddField("Video", trace.Result.First.Video.AbsoluteUri)

            Return embed.Build
        Catch ex As Exception
            Return embedHandler.errorEmbed("Anime - Trace", ex.Message).Result
        End Try
    End Function

    'Turn API Key into option in settings - same with database - then do a for loop to add it into a string
    Public Shared Async Function GetSauce(url As String) As Task(Of Embed)
        Try
            Dim httpClient = _httpClientFactory.CreateClient
            Dim db As String = "dbs[]=5&dbs[]=37&dbs[]=9&dbs[]=12&dbs[]=14&dbs[]=16&dbs[]=25&db=26&dbs[]=27&dbs[]=39&dbs[]=41"
            Dim response = Await httpClient.GetStringAsync($"https://saucenao.com/search.php?api_key=9da56c3c737d7725384df5bb282ab04a70afb1c8&{db}&output_type=2&testmode=1&numres=16&url={url}")
            Dim sauce = SauceNaoModel.FromJson(response)

            If sauce Is Nothing Then
                Return embedHandler.errorEmbed("Test - Sauce", "an error as occurred, please try again later.").Result
            End If

            Dim embed = New EmbedBuilder With {
                .Title = "Most Relevant Result",
                .Description = $"***Top 3 results:***{Environment.NewLine}{sauce.Results.Item(1).Data.ExtUrls.First.AbsoluteUri}{Environment.NewLine}{sauce.Results.Item(2).Data.ExtUrls.First.AbsoluteUri}{Environment.NewLine}{sauce.Results.Item(3).Data.ExtUrls.First.AbsoluteUri}",
                .ImageUrl = sauce.Results.Item(0).Header.Thumbnail.AbsoluteUri,
                .ThumbnailUrl = sauce.Results.Item(0).Header.Thumbnail.AbsoluteUri,
                .Url = sauce.Results.Item(0).Data.ExtUrls.First.AbsoluteUri,
                .Color = New Color(Await _img.RandomColorFromURL(sauce.Results.Item(0).Header.Thumbnail.AbsoluteUri)),
                .Footer = New EmbedFooterBuilder With {
                    .Text = "Time for the sauce - Click title for most relevant result.",
                    .IconUrl = sauce.Results.Item(0).Header.Thumbnail.AbsoluteUri
                }
            }
            Return embed.Build
        Catch ex As Exception
            Return embedHandler.errorEmbed("Test - Sauce", ex.Message).Result
        End Try
    End Function

#End Region

End Class