Imports Discord.Commands

<Name("Gifs")>
<Summary("Get a random anime gif.")>
Public Class Gifs
    Inherits ModuleBase(Of SocketCommandContext)

    <Command("hi")>
    <Summary("Get a random gif from Tenor.com")>
    Public Async Function hiGif() As Task
        Await ReplyAsync(Await GifService.TenorGif(Context, "hi"))
    End Function

    <Command("bye")>
    <Summary("Get a random gif from Tenor.com")>
    Public Async Function byeGif() As Task
        Await ReplyAsync(Await GifService.TenorGif(Context, "bye"))
    End Function

    <Command("sad")>
    <Summary("Get a random gif from Tenor.com")>
    Public Async Function sadGif() As Task
        Await ReplyAsync(Await GifService.TenorGif(Context, "sad"))
    End Function

    <Command("angry")>
    <Summary("Get a random gif from Tenor.com")>
    Public Async Function angryGif() As Task
        Await ReplyAsync(Await GifService.TenorGif(Context, "angry"))
    End Function

    <Command("laugh")>
    <[Alias]("lol")>
    <Summary("Get a random gif from Tenor.com")>
    Public Async Function laughGif() As Task
        Await ReplyAsync(Await GifService.TenorGif(Context, "laugh"))
    End Function

    <Command("funny")>
    <Summary("Get a random gif from Tenor.com")>
    Public Async Function funnyGif() As Task
        Await ReplyAsync(Await GifService.TenorGif(Context, "funny"))
    End Function

    <Command("rage")>
    <Summary("Get a random gif from Tenor.com")>
    Public Async Function rageGif() As Task
        Await ReplyAsync(Await GifService.TenorGif(Context, "rage"))
    End Function

    <Command("horny")>
    <Summary("Get a random gif from Tenor.com")>
    Public Async Function hornyGif() As Task
        Await ReplyAsync(Await GifService.TenorGif(Context, "horny"))
    End Function

    <Command("search")>
    <Summary("Get a random anime gif with the provided keyword.")>
    <Remarks("\search <keyword> or \search <code geass>")>
    Public Async Function searchGif(<Remainder> tag As String) As Task
        Await ReplyAsync(Await GifService.TenorGif(Context, tag))
    End Function

    <Command("cry")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function cryGif() As Task
        Await ReplyAsync(Await GifService.WaifuPicsSFW("cry"))
    End Function

    <Command("hug")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function hugGif() As Task
        Await ReplyAsync(Await GifService.WaifuPicsSFW("hug"))
    End Function

    <Command("smug")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function smugGif() As Task
        Await ReplyAsync(Await GifService.WaifuPicsSFW("smug"))
    End Function

    <Command("bonk")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function bonkGif() As Task
        Await ReplyAsync(Await GifService.WaifuPicsSFW("bonk"))
    End Function

    <Command("yeet")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function yeetGif() As Task
        Await ReplyAsync(Await GifService.WaifuPicsSFW("yeet"))
    End Function

    <Command("blush")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function blushGif() As Task
        Await ReplyAsync(Await GifService.WaifuPicsSFW("blush"))
    End Function

    <Command("smile")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function smileGif() As Task
        Await ReplyAsync(Await GifService.WaifuPicsSFW("smile"))
    End Function

    <Command("wave")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function waveGif() As Task
        Await ReplyAsync(Await GifService.WaifuPicsSFW("wave"))
    End Function

    <Command("highfive")>
    <[Alias]("h5")>
    <Summary("h5 | Get a random gif from waifu.pics")>
    Public Async Function highfiveGif() As Task
        Await ReplyAsync(Await GifService.WaifuPicsSFW("highfive"))
    End Function

    <Command("slap")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function slapGif() As Task
        Await ReplyAsync(Await GifService.WaifuPicsSFW("slap"))
    End Function

    <Command("kill")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function killGif() As Task
        Await ReplyAsync(Await GifService.WaifuPicsSFW("kill"))
    End Function

    <Command("kick")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function kickGif() As Task
        Await ReplyAsync(Await GifService.WaifuPicsSFW("kick"))
    End Function

    <Command("happy")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function happyGif() As Task
        Await ReplyAsync(Await GifService.WaifuPicsSFW("happy"))
    End Function

    <Command("wink")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function winkGif() As Task
        Await ReplyAsync(Await GifService.WaifuPicsSFW("wink"))
    End Function

    <Command("poke")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function pokwGif() As Task
        Await ReplyAsync(Await GifService.WaifuPicsSFW("poke"))
    End Function

    <Command("dance")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function danceGif() As Task
        Await ReplyAsync(Await GifService.WaifuPicsSFW("dance"))
    End Function

    <Command("cringe")>
    <Summary("Get a random gif from waifu.pics")>
    Public Async Function cringeGif() As Task
        Await ReplyAsync(Await GifService.WaifuPicsSFW("cringe"))
    End Function

End Class

<Name("Fun")>
<Summary("Some fun!")>
Public Class Fun

    Inherits ModuleBase(Of SocketCommandContext)

    <Command("animememe")>
    <[Alias]("ameme")>
    <Summary("Get a meme from a random Anime subreddit")>
    Public Async Function animeRedditMeme() As Task
        Await ReplyAsync(embed:=Await FunService.AnimeRedit(Context))
    End Function

    <Command("regularmememe")>
    <[Alias]("rmeme")>
    <Summary("Get a meme from a random Meme subreddit")>
    Public Async Function regularRedditMeme() As Task
        Await ReplyAsync(embed:=Await FunService.RegularRedit(Context))
    End Function

    <Command("neko")>
    <Summary("Get a random neko image")>
    Public Async Function getNeko() As Task
        Await ReplyAsync(embed:=Await FunService.GetNeko())
    End Function

    <Command("activity")>
    <[Alias]("act")>
    <Summary("Get a random activity")>
    Public Async Function getActivity() As Task
        Await ReplyAsync(embed:=Await FunService.GetActivity(Context))
    End Function

    <Command("joke")>
    <Summary("Get a random joke. WARNING: Some jokes may be offensive to some people. You have been warned.")>
    Public Async Function getRandomJoke() As Task
        Await ReplyAsync(embed:=Await FunService.GetJoke())
    End Function

    <Command("dictionary")>
    <[Alias]("dic")>
    <Summary("Gives you the first definition of the provided word.")>
    <Remarks("\dic hello")>
    Public Async Function GetDefinition(word As String) As Task
        Await ReplyAsync(embed:=Await FunService.GetDictionary(Context, word))
    End Function

    <Command("uselessfact")>
    <[Alias]("ufact")>
    <Summary("Get a random useless fact.")>
    <Remarks("\ufact")>
    Public Async Function GetFact() As Task
        Await ReplyAsync(embed:=Await FunService.UselessFact(Context))
    End Function

    <Command("quotethem")>
    <[Alias]("qt")>
    <Summary("Somesone said something stupid? Get it quoted. Right click the msg and copy id.")>
    <Remarks("\qt [message id] | \qt 995153475442843819")>
    Public Async Function Quote(id As ULong) As Task
        Await ReplyAsync(embed:=Await FunService.Quote(Context, id))
    End Function
End Class