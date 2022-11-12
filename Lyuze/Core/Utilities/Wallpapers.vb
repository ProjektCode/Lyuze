Imports System.ComponentModel
Imports System.IO

NotInheritable Class Wallpapers
    Private Shared ReadOnly rand As New Random

    'Later on do error checking to see if a website contains any wallpapers if not change website.
#Region "Wallpaper Command"

#Region "Keywords"

    Public Shared keywords() As String =
    {
    "anime",
    "league of legends",
    "valorant",
    "payday",
    "demon slayer",
    "game",
    "halo",
    "code geass",
    "japan",
    "minecraft",
    "basketball",
    "2b",
    "tohru",
    "lenn",
    "purge",
    "gun",
    "mecha",
    "dishonored",
    "rainbow six siege",
    "computers",
    "flowers",
    "tiger",
    "women",
    "men",
    "crossover",
    "kill la kill",
    "sinoalice",
    "arknights",
    "girls frontline",
    "memes",
    "honkai impact",
    "Logitech",
    "Corsair",
    "video games",
    "Zelda",
    "Princess Link",
    "Tracer",
    "Widowmaker",
    "Neeko",
    "NZXT",
    "minimalist",
    "minimalistic anime",
    "logo",
    "anime crossover",
    "robots",
    "rimuru",
    "c.c.",
    "ryuko",
    "ryuuko",
    "waifu",
    "rave",
    "cyberpunk",
    "cyber punk",
    "amd",
    "intel",
    "rtx",
    "gtx",
    "weapons",
    "genshin impact",
    "klee",
    "genshin impact jean",
    "genshin impact mona",
    "genshin impact lisa",
    "genshin impact Keqing",
    "genshin impact klee",
    "glorious",
    "keyboard",
    "keyboard switches",
    "escape front tarkov"
    }

    Shared Async Function getWord() As Task(Of String)
        Dim word As String = keywords(rand.Next(0, keywords.Length))

        Return word
    End Function

#End Region

#Region "Websites"

    Shared sites As String() = {
        "https://wall.alphacoders.com/search.php?search=",
        "https://wallhaven.cc/search?q=",
        "https://wallpaperscraft.com/search/?query=",
        "https://www.pixiv.net/en/tags/",
        "https://www.artstation.com/search?q=",
        "https://wallpaperaccess.com/search?q=",
        "https://wallpaperplay.com/search?term=",
        "https://wallpapercave.com/search?q=",
        "https://www.wallpaperflare.com/search?wallpaper=",
        "https://www.wallpapermaiden.com/search?term="
    }

    Shared Async Function getSite() As Task(Of String)
        Dim website As String = sites(rand.Next(0, sites.Length))
        Return website
    End Function
#End Region

#End Region

#Region "Images"

    Private Shared ReadOnly basePath = AppDomain.CurrentDomain.BaseDirectory

    Public Shared images As New List(Of Uri)

    Public Shared Async Function returnImage() As Task(Of String)
        GrabImages()
        Dim i As String = images(rand.Next(images.Count)).ToString
        Return i
    End Function

    Private Shared Sub GrabImages()
        Dim settings = Lyuze.Settings.Data

        If Not images.Count = 0 Then
            Return
        End If
        For Each img In settings.ImageLinks
            images.Add(img)
        Next
    End Sub

#End Region

End Class