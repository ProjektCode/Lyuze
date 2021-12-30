Imports System.Globalization
Imports System.Net.Http
Imports System.Text
Imports Discord
Imports Microsoft.Extensions.DependencyInjection

NotInheritable Class GenshinService

    'Grabbing required services
    Private Shared ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)
    Private Shared ReadOnly _httpClientFactory As IHttpClientFactory = serviceHandler.provider.GetRequiredService(Of IHttpClientFactory)
    Private Shared ReadOnly _img As Images = serviceHandler.provider.GetRequiredService(Of Images)

    Public Shared Async Function GenshinTypes() As Task(Of Embed)

        Try

            Dim httpClient = _httpClientFactory.CreateClient
            Dim response = Await httpClient.GetStringAsync("https://api.genshin.dev")
            Dim types = GenshinImpactBase.FromJson(response)
            Dim description As String

            If types Is Nothing Then
                Return embedHandler.errorEmbed("Genshin - Types", "an error as occurred, please try again later.").Result
            End If

            For Each type In types.Types
                description += $"{type}{Environment.NewLine}"
            Next

            Dim embed = New EmbedBuilder With {
                .Title = "Available Types",
                .Description = description,
                .Color = New Color(_utils.randomEmbedColor)
            }

            Return embed.Build

        Catch ex As Exception
            Return embedHandler.errorEmbed("Genshin - Types", ex.Message).Result
        End Try

    End Function

    Public Shared Async Function GetCharacter(name As String) As Task(Of Embed)

        Try

            Dim httpClient = _httpClientFactory.CreateClient
            Dim response = Await httpClient.GetStringAsync("https://api.genshin.dev/characters")
            Dim genshin = GenshinList.FromJson(response)

            Dim txtInfo As TextInfo = New CultureInfo("en-us", False).TextInfo
            Dim skillText As String = String.Empty
            Dim charList As String = String.Empty
            Dim newName As String = String.Empty

            If genshin Is Nothing Then
                Return embedHandler.errorEmbed("Genshin - Character", "An error has occurred, please try again later.").Result
            End If

            If name = "default" Then
                For Each c In genshin
                    charList += $"*{c.Replace("-", " ")}* **|** "
                Next

                Dim embed = New EmbedBuilder With {
                    .Title = "Available Characters",
                    .Description = charList,
                    .Color = New Color(_utils.randomEmbedColor)
                }

                Return embed.Build
            End If

            If name.Contains(" ") Then
                newName = name.Replace(" ", "-")
            Else
                newName = name
            End If

            For Each c In genshin
                If newName = c Then
                    Try
                        response = Await httpClient.GetStringAsync($"https://api.genshin.dev/characters/{ newName}")
                        Dim character = GenshinCharacter.FromJson(response)
#Region "String Builders"

                        Dim constBuilder As New StringBuilder
                        For Each con In character.Constellations
                            constBuilder.Append($"*{con.Unlock}:* `{con.Name}`{Environment.NewLine}")
                        Next
                        constBuilder.Remove(constBuilder.Length - 2, 2)

                        Dim pTalentBuilder As New StringBuilder
                        For Each pass In character.PassiveTalents
                            pTalentBuilder.Append($"*{pass.Unlock}:* `{pass.Name}`{Environment.NewLine}")
                        Next
                        pTalentBuilder.Remove(pTalentBuilder.Length - 2, 2)

                        Dim sTalentBuilder As New StringBuilder
                        For Each skill In character.SkillTalents
                            skillText = skill.Type.Replace("_", " ").ToLower
                            sTalentBuilder.Append($"*{skill.Name}:* `{txtInfo.ToTitleCase(skillText)}`{Environment.NewLine}")
                        Next

#End Region

#Region "Embed"

                        Dim embed = New EmbedBuilder With {
                            .Title = $"{character.Name} - {Await GenshinUtils.GetRarity(character.Rarity)}",
                            .Color = New Color(Await _img.RandomColorFromURL(Await GenshinUtils.GetImage("char", character.Name, "gacha-splash"))),
                            .ImageUrl = Await GenshinUtils.GetImage("char", character.Name, "gacha-splash"),
                            .ThumbnailUrl = Await GenshinUtils.GetImage("char", character.Name, "icon-big"),
                            .Footer = New EmbedFooterBuilder With {
                                .IconUrl = Await GenshinUtils.GetImage("char", character.Name, "constellation"),
                                .Text = $"{character.Name} - Info Card"
                            }
                        }

                        'Fields
                        embed.AddField("Vision", Defaults.defaultValue(character.Vision), True)
                        embed.AddField("Nation", Defaults.defaultValue(character.Nation), True)
                        embed.AddField("Affiliation", Defaults.defaultValue(character.Affiliation), True)
                        If Not name.Contains("traveler") Then
                            embed.AddField("Birthday", character.Birthday.Replace("0000-", String.Empty).ToString, True)
                        End If
                        embed.AddField("Weapon Type", Defaults.defaultValue(character.Weapon), True)
                        embed.AddField("Constellation", Defaults.defaultValue(character.Constellation), True)
                        embed.AddField("Skill Talents", Defaults.defaultValue(sTalentBuilder.ToString))
                        embed.AddField("Passive Talents", Defaults.defaultValue(pTalentBuilder.ToString))
                        embed.AddField("Constelations", Defaults.defaultValue(constBuilder.ToString))
                        embed.AddField("Description", Defaults.defaultValue(character.Description))
#End Region


                        Return embed.Build
                    Catch ex As Exception
                        Return embedHandler.errorEmbed("Genshin - Character", ex.Message).Result
                    End Try
                End If
            Next

            Return embedHandler.errorEmbed("Genshin - Character", $"character with the name **{name}** not found.").Result

        Catch ex As Exception
            Return embedHandler.errorEmbed("Genshin Character", ex.Message).Result
        End Try

    End Function

    Public Shared Async Function GetWeapon(name As String) As Task(Of Embed)
        Try

            Dim httpClient = _httpClientFactory.CreateClient
            Dim response = Await httpClient.GetStringAsync("https://api.genshin.dev/weapons")
            Dim weapons = GenshinList.FromJson(response)

            Dim txtInfo As TextInfo = New CultureInfo("en-us", False).TextInfo
            Dim newName As String = String.Empty
            Dim weaList As String = String.Empty
            Dim newList As String = String.Empty
            Dim listNum As Integer = 1

            If weapons Is Nothing Then
                Return embedHandler.errorEmbed("Genshin - Weapon", "An error has occurred, please try again later.").Result
            End If

            If name = "default" Then
                Dim embed = New EmbedBuilder With {
                    .Title = "Available Weapons",
                    .Color = New Color(_utils.randomEmbedColor)
                }

                For Each w In weapons
                    If weaList.Length + w.Length + 5 > 256 Then
                        If weaList.EndsWith(" **|** ") Then
                            newList = weaList.Remove(weaList.Length - 7)
                        End If
                        embed.AddField($"List{listNum}", newList, True)
                        listNum += 1
                        weaList = String.Empty
                    End If

                    If w.Contains("-s-") Then
                        weaList += $"*{w.Replace("-s-", "'s ")}* **|** "
                    ElseIf w.Contains("-") Then
                        weaList += $"*{w.Replace("-", " ")}* **|** "
                    End If
                Next

                Return embed.Build
            End If


            If name.Contains(" ") And name.Contains("'s") Then
                newName = name.Replace("'s ", "-s-")
            Else
                newName = name.Replace(" ", "-")
            End If

            For Each w In weapons
                If newName = w Then
                    Try
                        response = Await httpClient.GetStringAsync($"https://api.genshin.dev/weapons/{ newName}")
                        Dim weapon = GenshinWeapon.FromJson(response)

                        Dim embed = New EmbedBuilder With {
                            .Title = $"{weapon.Name} - {Await GenshinUtils.GetRarity(weapon.Rarity)}",
                            .ThumbnailUrl = Await GenshinUtils.GetImage("wea", newName, "icon"),
                            .Color = New Color(Await _img.RandomColorFromURL(Await GenshinUtils.GetImage("wea", newName, "icon"))),
                            .Footer = New EmbedFooterBuilder With {
                                .IconUrl = Await GenshinUtils.GetImage("wea", newName, "icon"),
                                .Text = $"{weapon.Name}'s Info Card"
                            }
                        }

                        embed.AddField("Base Attack", weapon.BaseAttack, True)
                        embed.AddField("Sub Stat", weapon.SubStat, True)
                        embed.AddField("Type", weapon.Type, True)
                        embed.AddField("Location", weapon.Location, True)
                        embed.AddField(weapon.PassiveName, weapon.PassiveDesc)

                        Return embed.Build
                    Catch ex As Exception
                        Return embedHandler.errorEmbed("Genshin - Weapon", ex.Message).Result
                    End Try
                End If
            Next

            Return embedHandler.errorEmbed("Genshin Weapon", $"Could not find weapon with name: {name}").Result
        Catch ex As Exception
            Return embedHandler.errorEmbed("Genshin - Weapon", ex.Message).Result
        End Try
    End Function

    Public Shared Async Function GetArtifact(name As String) As Task(Of Embed)
        Try
            Dim httpClient = _httpClientFactory.CreateClient
            Dim response = Await httpClient.GetStringAsync("https://api.genshin.dev/artifacts")
            Dim artifacts = GenshinList.FromJson(response)

            Dim txtInfo As TextInfo = New CultureInfo("en-us", False).TextInfo
            Dim newName As String = String.Empty
            Dim artList As String = String.Empty
            Dim newList As String = String.Empty
            Dim listNum As Integer = 1

            If artifacts Is Nothing Then
                Return embedHandler.errorEmbed("Genshin - Artifact", "An error has occurred, please try again later.").Result
            End If

            'Returns a list of all available artifacts
            If name = "default" Then
                Dim embed = New EmbedBuilder With {
                    .Title = "Available Weapons",
                    .Color = New Color(_utils.randomEmbedColor)
                }

                For Each a In artifacts
                    If artList.Length + a.Length + 5 > 256 Then
                        If artList.EndsWith(" **|** ") Then
                            newList = artList.Remove(artList.Length - 7)
                        End If
                        embed.AddField($"List{listNum}", newList, True)
                        listNum += 1
                        artList = String.Empty
                    End If

                    If a.Contains("-s-") Then
                        artList += $"*{a.Replace("-s-", "'s ")}* **|** "
                    ElseIf a.Contains("-") Then
                        artList += $"*{a.Replace("-", " ")}* **|** "
                    End If
                Next

                Return embed.Build
            End If


            If name.Contains(" ") And name.Contains("'s") Then
                newName = name.Replace("'s ", "-s-")
            Else
                newName = name.Replace(" ", "-")
            End If
            If newName Is Nothing Then
                newName = name
            End If

            For Each a In artifacts
                If newName = a Then
                    Try
                        response = Await httpClient.GetStringAsync($"https://api.genshin.dev/artifacts/{ newName}")
                        Dim artifact = GenshinArtifact.FromJson(response)

                        Dim embed = New EmbedBuilder With {
                            .Title = $"{artifact.Name} - {Await GenshinUtils.GetRarity(artifact.MaxRarity)}",
                            .ThumbnailUrl = Await GenshinUtils.GetImage("art", newName, "icon"),
                            .Color = New Color(Await _img.RandomColorFromURL(Await GenshinUtils.GetImage("art", newName, "icon"))),
                            .Footer = New EmbedFooterBuilder With {
                                .IconUrl = GenshinUtils.GetImage("art", newName, "icon").Result,
                                .Text = $"{artifact.Name}'s Info Card"
                            }
                        }

                        embed.AddField("2-Piece Bonus", artifact.The2PieceBonus)
                        embed.AddField("4-Piece Bonus", artifact.The4PieceBonus)

                        Return embed.Build
                    Catch ex As Exception
                        Return embedHandler.errorEmbed("Genshin - Weapon", ex.Message).Result
                    End Try
                End If
            Next

            Return embedHandler.errorEmbed("Genshin Artifact", $"Could not find weapon with name: {name}").Result
        Catch ex As Exception
            Return embedHandler.errorEmbed("Genshin Artifact", ex.Message).Result
        End Try
    End Function

    Public Shared Async Function GetEnemy(name As String) As Task(Of Embed)
        Try
            Dim httpClient = _httpClientFactory.CreateClient
            Dim response = Await httpClient.GetStringAsync("https://api.genshin.dev/enemies")
            Dim enemies = GenshinList.FromJson(response)



            Dim txtInfo As TextInfo = New CultureInfo("en-us", False).TextInfo
            Dim newName As String = String.Empty
            Dim eneList As String = String.Empty
            Dim newList As String = String.Empty
            Dim listNum As Integer = 1

            If enemies Is Nothing Then
                Return embedHandler.errorEmbed("Genshin - Enemies", "An error has occurred, please try again later.").Result
            End If

            'If the name is default return a list of available enemies
            If name = "default" Then
                Dim embed = New EmbedBuilder With {
                    .Title = "Available Enemies",
                    .Color = New Color(_utils.randomEmbedColor)
                }

                For Each e In enemies
                    If eneList.Length + e.Length + 5 > 256 Then
                        If eneList.EndsWith(" **|** ") Then
                            newList = eneList.Remove(eneList.Length - 7)
                        End If
                        embed.AddField($"List{listNum}", newList, True)
                        listNum += 1
                        eneList = String.Empty
                    End If

                    If e.Contains("-s-") Then
                        eneList += $"*{e.Replace("-s-", "'s ")}* **|** "
                    ElseIf e.Contains("-") Then
                        eneList += $"*{e.Replace("-", " ")}* **|** "
                    End If
                Next

                Return embed.Build
            End If

            If name.Contains(" ") And name.Contains("'s") Then
                newName = name.Replace("'s ", "-s-")
            Else
                newName = name.Replace(" ", "-")
            End If

            For Each e In enemies
                If newName = e Then
                    Try
                        response = Await httpClient.GetStringAsync($"https://api.genshin.dev/enemies/{ newName}")
                        Dim enemy = GenshinEnemies.FromJson(response)

#Region "String Builders"
                        Dim elementBuilder As New StringBuilder
                        For Each ele In enemy.Elements
                            elementBuilder.Append($"{ele},{Environment.NewLine}")
                        Next
                        elementBuilder.Remove(elementBuilder.Length - 3, 3)
                        Dim dropsBuilder As New StringBuilder
                        For Each drop In enemy.Drops
                            dropsBuilder.Append($"{drop.Rarity}: {drop.Name}{Environment.NewLine}")
                        Next
                        dropsBuilder.Remove(dropsBuilder.Length - 3, 3)

#End Region

                        Dim embed = New EmbedBuilder With {
                            .Title = enemy.Name,
                            .ThumbnailUrl = Await GenshinUtils.GetImage("ene", newName, "icon"),
                            .Color = New Color(Await _img.RandomColorFromURL(Await GenshinUtils.GetImage("ene", newName, "icon")))
                        }
                        embed.AddField("Region", enemy.Region, True)
                        embed.AddField("Type", enemy.Type, True)
                        embed.AddField("Family", enemy.Family, True)
                        embed.AddField("Elements", elementBuilder.ToString, True)
                        embed.AddField("Drops", dropsBuilder.ToString, True)
                        embed.AddField("Description", enemy.Description)

                        Return embed.Build
                    Catch ex As Exception
                        Return embedHandler.errorEmbed("Genshin Enemy", ex.Message).Result
                    End Try
                End If
            Next

        Catch ex As Exception
            Return embedHandler.errorEmbed("Genshin Enemy", ex.Message).Result
        End Try
    End Function

    Public Shared Async Function GetElement(name As String) As Task(Of Embed)

        Try
            Dim httpClient = _httpClientFactory.CreateClient
            Dim response = Await httpClient.GetStringAsync($"https://api.genshin.dev/elements/{ name}")
            Dim ele = GenshinElements.FromJson(response)



            Dim txtInfo As TextInfo = New CultureInfo("en-us", False).TextInfo
            Dim url = Await GenshinUtils.GetImage("ele", ele.Name, "icon")

            If ele Is Nothing Then
                Return embedHandler.errorEmbed("Genshin - Elements", "An error has occurred, please try again later.").Result
            End If

            Dim embed = New EmbedBuilder With {
                .Title = ele.Name,
                .ThumbnailUrl = url,
                .Color = New Color(Await _img.RandomColorFromURL(url)),
                .Footer = New EmbedFooterBuilder With {
                    .IconUrl = url,
                    .Text = $"{ele.Name} information"
                }
            }

            For Each e In ele.Reactions
                embed.AddField(e.Elements.First, $"**{e.Name} -** *{e.Description}*")
            Next
            Return embed.Build
        Catch ex As Exception
            Dim message As String
            If ex.Message.Contains("Response status code does not indicate success: 404 (Not Found)") Then
                message = $"Element with {name} was not found."
            Else
                message = ex.Message
            End If

            Return embedHandler.errorEmbed("Genshin - Elements", message).Result
        End Try

    End Function

End Class