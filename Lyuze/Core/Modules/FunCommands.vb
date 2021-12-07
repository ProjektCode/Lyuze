Imports System.Net.Http
Imports Discord
Imports Discord.Commands
Imports Newtonsoft.Json.Linq
Imports Microsoft.Extensions.DependencyInjection
Imports HtmlAgilityPack
Imports System.Net

<Name("Gifs")>
<Summary("Get a random anime gif.")>
Public Class Gifs
    Inherits ModuleBase(Of SocketCommandContext)

    <Command("hi")>
    <Summary("Get a random gif from Tenor.com")>
    Public Async Function hiGif() As Task
        Await ReplyAsync(GifService.TenorGif("hi").Result)
    End Function

    <Command("bye")>
    <Summary("Get a random gif from Tenor.com")>
    Public Async Function byeGif() As Task
        Await ReplyAsync(GifService.TenorGif("bye").Result)
    End Function

    <Command("sad")>
    <Summary("Get a random gif from Tenor.com")>
    Public Async Function sadGif() As Task
        Await ReplyAsync(GifService.TenorGif("sad").Result)
    End Function

    <Command("angry")>
    <Summary("Get a random gif from Tenor.com")>
    Public Async Function angryGif() As Task
        Await ReplyAsync(GifService.TenorGif("angry").Result)
    End Function

    <Command("laugh")>
    <[Alias]("lol")>
    <Summary("Get a random gif from Tenor.com")>
    Public Async Function laughGif() As Task
        Await ReplyAsync(GifService.TenorGif("laugh").Result)
    End Function

    <Command("funny")>
    <Summary("Get a random gif from Tenor.com")>
    Public Async Function funnyGif() As Task
        Await ReplyAsync(GifService.TenorGif("funny").Result)
    End Function

    <Command("rage")>
    <Summary("Get a random gif from Tenor.com")>
    Public Async Function rageGif() As Task
        Await ReplyAsync(GifService.TenorGif("rage").Result)
    End Function

    <Command("horny")>
    <Summary("Get a random gif from Tenor.com")>
    Public Async Function hornyGif() As Task
        Await ReplyAsync(GifService.TenorGif("horny").Result)
    End Function

    '<Command("blush")>
    '<Summary("Get a random gif from Tenor.com")>
    'Public Async Function blushGif() As Task
    '    Await ReplyAsync(GifService.TenorGif("blush").Result)
    'End Function

    <Command("search")>
    <Summary("Get a random anime gif with the provided keyword.")>
    <Remarks("\search <keyword> or \search <code geass>")>
    Public Async Function searchGif(<Remainder> tag As String) As Task
        Await ReplyAsync(GifService.TenorGif(tag).Result)
    End Function

    <Command("cry")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function cryGif() As Task
        Await ReplyAsync(GifService.WaifuPicsSFW("cry").Result)
    End Function

    <Command("hug")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function hugGif() As Task
        Await ReplyAsync(GifService.WaifuPicsSFW("hug").Result)
    End Function

    <Command("smug")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function smugGif() As Task
        Await ReplyAsync(GifService.WaifuPicsSFW("smug").Result)
    End Function

    <Command("bonk")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function bonkGif() As Task
        Await ReplyAsync(GifService.WaifuPicsSFW("bonk").Result)
    End Function

    <Command("yeet")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function yeetGif() As Task
        Await ReplyAsync(GifService.WaifuPicsSFW("yeet").Result)
    End Function

    <Command("blush")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function blushGif() As Task
        Await ReplyAsync(GifService.WaifuPicsSFW("blush").Result)
    End Function

    <Command("smile")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function smileGif() As Task
        Await ReplyAsync(GifService.WaifuPicsSFW("smile").Result)
    End Function

    <Command("wave")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function waveGif() As Task
        Await ReplyAsync(GifService.WaifuPicsSFW("wave").Result)
    End Function

    <Command("highfive")>
    <[Alias]("h5")>
    <Summary("h5 | Get a random gif from waifu.pics")>
    Public Async Function highfiveGif() As Task
        Await ReplyAsync(GifService.WaifuPicsSFW("highfive").Result)
    End Function

    <Command("slap")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function slapGif() As Task
        Await ReplyAsync(GifService.WaifuPicsSFW("slap").Result)
    End Function

    <Command("kill")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function killGif() As Task
        Await ReplyAsync(GifService.WaifuPicsSFW("kill").Result)
    End Function

    <Command("kick")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function kickGif() As Task
        Await ReplyAsync(GifService.WaifuPicsSFW("kick").Result)
    End Function

    <Command("happy")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function happyGif() As Task
        Await ReplyAsync(GifService.WaifuPicsSFW("happy").Result)
    End Function

    <Command("wink")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function winkGif() As Task
        Await ReplyAsync(GifService.WaifuPicsSFW("wink").Result)
    End Function

    <Command("poke")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function pokwGif() As Task
        Await ReplyAsync(GifService.WaifuPicsSFW("poke").Result)
    End Function

    <Command("dance")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function danceGif() As Task
        Await ReplyAsync(GifService.WaifuPicsSFW("dance").Result)
    End Function

    <Command("cringe")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function cringeGif() As Task
        Await ReplyAsync(GifService.WaifuPicsSFW("cringe").Result)
    End Function

End Class

<Name("Fun")>
<Summary("Some fun!")>
Public Class Fun

    Inherits ModuleBase(Of SocketCommandContext)
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
        Context.Channel.SendMessageAsync(embed:=embed.Build)
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
        Context.Channel.SendMessageAsync(embed:=embed.Build)
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

    <Command("test")>
    Public Async Function testCMD() As Task


    End Function

End Class