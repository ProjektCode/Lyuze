Imports Discord
Imports Discord.Commands

<Name("Wallpapers")>
<Group("wall")>
<Summary("To get all your wallpaper needs.")>
Public Class Backgrounds
    Inherits ModuleBase(Of SocketCommandContext)

    ReadOnly _Utils As New MasterUtils

    <Command("search")>
    <Summary("Returns a link of wallpapers with your chosen keyword [Will be reworked since some websites don't have any results on a given keyword]")>
    <Remarks("\wall search league of legends")>
    Public Async Function wallpaperCmd(<Remainder> _text As String) As Task

        Dim text As String = Replace(_text, " ", "+")
        Await Context.Channel.SendMessageAsync(Wallpapers.getSite.Result + text)

    End Function

    <Command("rwall")>
    <Summary("Returns a random link with a random keyword")>
    Public Async Function randomWallpaperCmd() As Task
        Dim Text As String = Replace(Wallpapers.getWord.Result, " ", "+")
        Await Context.Channel.SendMessageAsync(Wallpapers.getSite.Result + Text)
    End Function

    <Command("list")>
    <Summary("Gives a list of all of our keywords for our wallpaper command")>
    Public Async Function cmdList() As Task 'Rework with pagination
        Dim m = Context.Message
        Dim u = Context.User
        Dim g = Context.Guild
        Dim c = Context.Client

        Dim embed As New EmbedBuilder With {
            .Title = $"Wallpaper keyword list",
            .ImageUrl = "https://i.imgur.com/vc241Ku.jpeg",
            .Description = "The full list of keywords in our random wallpaper list",
            .Color = New Color(_Utils.randomEmbedColor),
            .ThumbnailUrl = g.IconUrl,
            .Timestamp = Context.Message.Timestamp,
            .Footer = New EmbedFooterBuilder With {
                    .Text = "Keyword Data",
                    .IconUrl = g.IconUrl
                }
            }

        Dim row As Integer = 0
        Dim words As String = String.Empty

        For Each keyword As String In Wallpapers.keywords
            'If appending the keyword to the list of words exceeds 256
            'don't append, but instead add the existing words to a field.
            If words.Length + keyword.Length + 7 > 256 Then
                row += 1
                embed.AddField($"List #{row}", words, True) 'Add words to field

                'reset words
                words = String.Empty
            End If

            words = words + keyword + " **|** "
        Next

        'The add condition within the for loop is only entered when we are
        'about to exceed to field length. Any string under the max 
        'length would exit the loop without being added. Add it here
        embed.AddField($"List #{row + 1}", words, True)

        Await m.Channel.SendMessageAsync(embed:=embed.Build())
    End Function

End Class
