Imports System.IO
Imports System.Drawing
Imports System.Globalization
Imports System.Runtime.InteropServices
Imports Discord.WebSocket
Imports Figgle
Imports Pastel

Public Class MasterUtils
    ReadOnly rand As New Random

#Region "Command Window Options"
    <DllImport("Kernel32.dll")>
    Public Shared Function GetConsoleWindow() As IntPtr

    End Function
    <DllImport("User32.dll")>
    Private Shared Function ShowWindow(hwd As IntPtr, cmdShow As Integer) As Boolean

    End Function
    ReadOnly hwd As IntPtr = GetConsoleWindow()

    Public Function winHide()
        Return ShowWindow(hwd, 0)
    End Function
    Public Function winShow()
        Return ShowWindow(hwd, 1)
    End Function

    Public Sub setBanner(text As String, primarycolor As String, accentColor As String)
        Console.Write(FiggleFonts.Standard.Render(text).Pastel(ColorTranslator.FromHtml(primarycolor)))
        'Console.ForegroundColor = accentColor
        Console.WriteLine("===================================================================".Pastel(accentColor))
    End Sub
#End Region

#Region "Color Options"

    Public Function ConvertToDiscordColor(hex As String)
        If hex.Contains("#") Then
            hex = hex.Remove(0, 1)
        End If
        Dim colorInt As Integer = Integer.Parse(hex, NumberStyles.HexNumber)
        Dim color As UInteger = Convert.ToInt32(colorInt)
        Return color
    End Function

    Public Function RandomEmbedColor()
        Dim rand As New Random
        Dim colors() As String = {
            "DC143C", ' Crimson
            "C3E4E8", 'Light Cyan
            "FF5733", 'Light Green
            "E6E6FA", 'Lavender
            "7289DA", 'Discord Purple
            "5865F2", 'Discord Blurple
            "D2042D", 'Cherry Red
            "8DB600", 'Apple Green
            "87CEEB" 'Sky Blue
        }

        Dim colorPicker As String = colors(rand.Next(0, colors.Length))
        Dim colorInt As Integer = Integer.Parse(colorPicker, NumberStyles.HexNumber)
        Dim color As UInteger = Convert.ToInt32(colorInt)

        Return color
    End Function

#End Region

#Region "Misc"
    Public Function timeOut(g As SocketGuild)
        Dim afk As Integer = g.AFKTimeout
        Dim minutes As Integer = afk / 60
        Dim time As Integer = minutes
        Return time
    End Function

    Public Function CheckAPI(key As String) As Boolean
        If Not key.ToLower = "disable" Then
            Return True
        End If

        Return False
    End Function

#Region "status"
    Public sList() As String = {
                "Anime Music | /help",
                "Anime | /help",
                "Hentai | /help",
                "My owner at https://github.com/Projekt-Dev| /Help",
                "William being a cunt | /help",
                "Arknights is love, Arknights is life | /help",
                "If anyone is gonna boost the server | /help",
                "My Owner trying to debug me | /help"
             }
    Public sIndex As Integer = rand.Next(sList.Length)

#End Region

#Region "Memes"
    ReadOnly aniMemesReddits As String() = {
    "GoodAnimemes",
    "Animemes",
    "animememes",
    "AnimeMeme",
    "goodanimememes",
    "AnimemesHQ"
}
    ReadOnly memesReddits As String() = {
        "memes",
        "dankchristianmemes",
        "funny"
    }

    Public Function getAnimeMeme()
        Dim reddit As String = aniMemesReddits(rand.Next(0, aniMemesReddits.Length))
        Return reddit
    End Function
    Public Function getMeme()
        Dim reddit As String = memesReddits(rand.Next(0, memesReddits.Length))
        Return reddit
    End Function
#End Region

#End Region

End Class
