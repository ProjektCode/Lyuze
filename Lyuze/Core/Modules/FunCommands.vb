Imports System.Net.Http
Imports Discord
Imports Discord.Commands
Imports Discord.Addons.Interactive
Imports Newtonsoft.Json.Linq
Imports Microsoft.Extensions.DependencyInjection

<Name("Gifs")>
<Summary("Get a random gif from Tenor.")>
Public Class Gifs
    Inherits InteractiveBase(Of SocketCommandContext)

    <Command("hi")>
    <Summary("Get a random gif with the command as the keyword.")>
    Public Async Function hiGif() As Task

        ReplyAsync(TenorService.animeGif("hi").Result)
    End Function

    <Command("bye")>
    <Summary("Get a random gif with the command as the keyword.")>
    Public Async Function byeGif() As Task

        ReplyAsync(TenorService.animeGif("bye").Result)
    End Function

    <Command("sad")>
    <Summary("Get a random gif with the command as the keyword.")>
    Public Async Function sadGif() As Task

        ReplyAsync(TenorService.animeGif("sad").Result)
    End Function

    <Command("angry")>
    <Summary("Get a random gif with the command as the keyword.")>
    Public Async Function angryGif() As Task

        ReplyAsync(TenorService.animeGif("angry").Result)
    End Function

    <Command("laugh")>
    <[Alias]("lol")>
    <Summary("Get a random gif with the command as the keyword.")>
    Public Async Function laughGif() As Task

        ReplyAsync(TenorService.animeGif("laugh").Result)
    End Function

    <Command("funny")>
    <Summary("Get a random gif with the command as the keyword.")>
    Public Async Function funnyGif() As Task

        ReplyAsync(TenorService.animeGif("funny").Result)
    End Function

    <Command("rage")>
    <Summary("Get a random gif with the command as the keyword.")>
    Public Async Function rageGif() As Task

        ReplyAsync(TenorService.animeGif("rage").Result)
    End Function

    <Command("horny")>
    <Summary("Get a random gif with the command as the keyword.")>
    Public Async Function hornyGif() As Task

        ReplyAsync(TenorService.animeGif("horny").Result)
    End Function

    <Command("blush")>
    <Summary("Get a random gif with the command as the keyword.")>
    Public Async Function blushGif() As Task

        ReplyAsync(TenorService.animeGif("blush").Result)
    End Function

    <Command("search")>
    <Summary("Get a random anime gif with the provided keyword.")>
    <Remarks("\search <keyword> or \search <code geass>")>
    Public Async Function searchGif(<Remainder> tag As String) As Task

        ReplyAsync(TenorService.animeGif(tag).Result)
    End Function

End Class

<Name("Fun")>
<Summary("Some fun!")>
Public Class Fun

    Inherits InteractiveBase(Of SocketCommandContext)
    Private ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)
    Private ReadOnly _httpClientFactory As IHttpClientFactory = serviceHandler.provider.GetRequiredService(Of IHttpClientFactory)

    <Command("animememe")>
    <[Alias]("ameme")>
    <Summary("Get a meme from a random Anime subreddit")>
    Public Async Function animeRedditMeme() As Task
        Dim client = New HttpClient()
        Dim result = Await client.GetStringAsync("https://reddit.com/r/" + _utils.getAnimeMeme + "/random.json?limit=1")
        Dim arr As JArray = JArray.Parse(result)
        Dim post As JObject = JObject.Parse(arr(0)("data")("children")(0)("data").ToString())

        Dim embed = New EmbedBuilder With {
            .Title = post("title").ToString,
        .ImageUrl = post("url").ToString,
        .Color = New Color(_utils.randomEmbedColor),
        .Url = "https://reddit.com" + post("permalink").ToString,
        .Footer = New EmbedFooterBuilder With {
                .Text = $"🗨{post("num_comments")} ⬆️{post("ups")}",
                .IconUrl = Context.Guild.IconUrl
            }
        }
        Context.Channel.SendMessageAsync("", False, embed.Build)
    End Function

    <Command("regularmememe")>
    <[Alias]("rmeme")>
    <Summary("Get a meme from a random Meme subreddit")>
    Public Async Function regularRedditMeme() As Task
        Dim client = New HttpClient()
        Dim result = Await client.GetStringAsync("https://reddit.com/r/" + _utils.getMeme + "/random.json?limit=1")
        Dim arr As JArray = JArray.Parse(result)
        Dim post As JObject = JObject.Parse(arr(0)("data")("children")(0)("data").ToString())

        Dim embed = New EmbedBuilder With {
            .Title = post("title").ToString,
        .ImageUrl = post("url").ToString,
        .Color = New Color(_utils.randomEmbedColor),
        .Url = "https://reddit.com" + post("permalink").ToString,
        .Footer = New EmbedFooterBuilder With {
                .Text = $"🗨{post("num_comments")} ⬆️{post("ups")}",
                .IconUrl = Context.Guild.IconUrl
            }
        }
        Context.Channel.SendMessageAsync("", False, embed.Build)
    End Function

    <Command("neko")>
    <Summary("Get a random neko image")>
    Public Async Function getNeko() As Task
        Dim rand As New Random
        Dim neko = rand.Next(1, 578)
        Dim image As String

        If neko >= 1 And neko < 10 Then
            image = $"000{neko}"

        ElseIf neko > 10 And neko < 100 Then
            image = $"00{neko}"
        Else
            image = $"0{neko}"
        End If

        Await ReplyAsync($"https://nekos.best/api/v1/nekos/{ image }.jpg")

    End Function

    <Command("activity")>
    <[Alias]("act")>
    <Summary("Get a random activity")>
    Public Async Function getActivity() As Task
        Dim httpClient = _httpClientFactory.CreateClient
        Dim response = Await httpClient.GetStringAsync("https://www.boredapi.com/api/activity/")
        Dim activity = ActivityEvent.FromJson(response)

        If activity Is Nothing Then
            Await ReplyAsync(embed:=embedHandler.errorEmbed("Activity", "An error occurred, please try again later").Result)
            Return
        End If

        Dim embed = New EmbedBuilder With {
            .Title = "New Activity",
            .Color = New Color(_utils.randomEmbedColor),
            .ThumbnailUrl = If(Context.User.GetAvatarUrl, Context.User.GetDefaultAvatarUrl)
        }
        embed.AddField("Activity", $"{activity.Activity}", True)
        embed.AddField("Type", $"{activity.Type}", True)
        embed.AddField("Participants", $"{activity.Participants}", True)
        embed.AddField("Accessibility", $"{activity.Accessibility}", True)
        embed.AddField("Price", $"{activity.Price}", True)

        Await ReplyAsync(embed:=embed.Build)

    End Function

    <Command("joke")>
    <Summary("Get a random joke. WARNING: Some jokes may be offensive to some people. You have been warned.")>
    Public Async Function getRandomJoke() As Task

        Try
            Dim httpClient = _httpClientFactory.CreateClient
            Dim response = Await httpClient.GetStringAsync("https://v2.jokeapi.dev/joke/Any")
            Dim joke = RandomJoke.FromJson(response)

            If joke Is Nothing Then
                Await ReplyAsync(embed:=embedHandler.errorEmbed("Joke", "An error occurred, please try again later").Result)
                Return
            End If

            If joke.Type = "single" Then

                Dim sEmbed = New EmbedBuilder With {
                    .Description = joke.Joke,
                    .Color = New Color(_utils.randomEmbedColor),
                    .Footer = New EmbedFooterBuilder With {
                        .Text = joke.Category
                    }
                }

                Await ReplyAsync(embed:=sEmbed.Build)
            Else

                Dim tEmbed = New EmbedBuilder With {
                    .Description = $"**{joke.Setup}**{Environment.NewLine}*{joke.Delivery}*",
                    .Color = New Color(_utils.randomEmbedColor),
                    .Footer = New EmbedFooterBuilder With {
                        .Text = joke.Category
                    }
                }

                Await ReplyAsync(embed:=tEmbed.Build)
            End If


        Catch ex As Exception
            ReplyAsync(embed:=embedHandler.errorEmbed("Joke", ex.Message).Result)
        End Try

    End Function

End Class