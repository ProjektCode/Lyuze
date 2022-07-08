Imports Discord.Commands
Imports Discord.Addons.Interactive
Imports System.IO
Imports Microsoft.Extensions.DependencyInjection

<Name("Image")>
<Summary("Manipulate, to a certain extent, any image.")>
Public Class imageManipulation

    Inherits InteractiveBase(Of SocketCommandContext)
    Private Shared ReadOnly _img As Images = serviceHandler.provider.GetRequiredService(Of Images)

    <Command("crop", RunMode.Async)>
    <Summary("Crops an image into a given dimension.")>
    <Remarks("\crop https://i.imgur.com/LdiD7hb.png | \crop <image attachment>")>
    Public Async Function imgCrop(width As Integer, height As Integer, Optional url As String = Nothing) As Task

#Region "Execution"

        Try

            'Checks if the URL in an attachment is a actual image. - Not added
            If Context.Message.Attachments.Count = 1 Then

                'Dim width = Convert.ToInt32(widthReply.ToString)
                'Dim height = Convert.ToInt32(heightReply.ToString)


                If width <= 0 Or height <= 0 Then
                    Await ReplyAndDeleteAsync("Please make sure your width and height are greater than zero.", timeout:=New TimeSpan(0, 0, 10))
                    Return
                End If

                url = Context.Message.Attachments.First.Url
                Dim ext = Path.GetExtension(url)
                If ext.EndsWith("png") Or ext.EndsWith("jpg") Then
                    Dim path = Await _img.createImageAsync(width, height, url)
                    Await Context.Channel.SendFileAsync(path)

                    File.Delete(path)
                    Return
                Else
                    Await ReplyAsync($"The given attachment was not a supported image format. use png or jpg.")
                    Return
                End If
            End If

            If url = Nothing Then
                Await ReplyAsync("Please provide a direct URL to am image.")
                Return
            Else
                Dim path = Await _img.createImageAsync(width, height, url)
                Await Context.Channel.SendFileAsync(path)
                File.Delete(path)
            End If

        Catch ex As Exception
            Dim _settings = Lyuze.Settings.Data

            If _settings.IDs.ErrorId = 0 Then
                loggingHandler.LogCriticalAsync("image", ex.Message)
            Else
                Dim chnl = Context.Guild.GetTextChannel(_settings.IDs.ErrorId)
                chnl.SendMessageAsync(embed:=embedHandler.errorEmbed("image - Crop", ex.Message).Result)
            End If

        End Try

#End Region

    End Function

    '<Command("masscrop", RunMode.Async)>
    '<[Alias]("mc")>
    '<Summary("mc | Owner only command - mass crops images from a list.")>
    'Public Async Function imgMassCrop(width As Integer, height As Integer) As Task
    '    Dim settings = Lyuze.Settings.Data

    '    Try
    '        If Context.User.Id = settings.IDs.OwnerId Then
    '            For Each wall As String In Wallpapers.images
    '                Dim path = Await _img.createImageAsync(width, height, wall)
    '                Await Context.Channel.SendFileAsync(path)
    '                File.Delete(path)
    '            Next
    '        Else
    '            Await Context.Channel.SendMessageAsync("This is only for the bot owner.")
    '        End If
    '    Catch ex As Exception
    '        Dim _settings = Lyuze.Settings.Data

    '        If _settings.IDs.ErrorId = 0 Then
    '            loggingHandler.LogCriticalAsync($"image", ex.Message)
    '        Else
    '            Dim chnl = Context.Guild.GetTextChannel(_settings.IDs.ErrorId)
    '            chnl.SendMessageAsync(embed:=embedHandler.errorEmbed($"Image - Masscrop", ex.Message).Result)
    '        End If
    '    End Try

    'End Function

End Class
