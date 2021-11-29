Imports Discord
Imports Discord.Commands
Imports Discord.Addons.Interactive
Imports System.IO
Imports Microsoft.Extensions.DependencyInjection

<Name("Image")>
<Group("image")>
<Summary("Sorta manipulate, to a certain extent, any image.")>
Public Class imageManipulation

    Inherits InteractiveBase(Of SocketCommandContext)
    Private Shared ReadOnly _img As Images = serviceHandler.provider.GetRequiredService(Of Images)

    <Command("crop", RunMode.Async)>
    <Summary("Crops an image into a given dimension.")>
    <Remarks("\crop https://i.imgur.com/LdiD7hb.png | \crop <image attachment>")>
    Public Async Function imgCrop(Optional url As String = Nothing) As Task

#Region "Execution"

        Try

            'Checks if the url in an attachment is a actual image. - Not added
            If Context.Message.Attachments.Count = 1 Then
                'Interactive response to get width with a 15sec timer
                Await ReplyAndDeleteAsync("What is your desired width?", timeout:=New TimeSpan(0, 0, 15))
                Dim widthReply = Await NextMessageAsync(True, timeout:=New TimeSpan(0, 0, 10))
                If widthReply Is Nothing Then
                    Await ReplyAndDeleteAsync($"{Context.Message.Author} you did not reply within the given timeframe.")
                    Return
                End If
                'Interactive response to get height with a 15sec timer
                Await ReplyAndDeleteAsync("What is your desired height?", timeout:=New TimeSpan(0, 0, 15))
                Dim heightReply = Await NextMessageAsync(timeout:=New TimeSpan(0, 0, 10))
                If heightReply Is Nothing Then
                    Await ReplyAndDeleteAsync($"{Context.Message.Author} you did not reply within the given timeframe.")
                    Return
                End If

                Dim width = Convert.ToInt32(widthReply.ToString)
                Dim height = Convert.ToInt32(heightReply.ToString)


                If width <= 0 Or height <= 0 Then
                    Await ReplyAndDeleteAsync("Please make sure your width and height are positive numbers.", timeout:=New TimeSpan(0, 0, 10))
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
                    Await ReplyAsync($"The given attachment was not a supported image format. if it is an image message admin to allow format.")
                    Return
                End If
            End If

            If url = Nothing Then
                Await ReplyAsync("Please provide a direct url to am image.")
                Return
            Else

                Await ReplyAndDeleteAsync("What is your desired width?", timeout:=New TimeSpan(0, 0, 15))
                Dim widthReply = Await NextMessageAsync(True, timeout:=New TimeSpan(0, 0, 10))
                If widthReply Is Nothing Then
                    Await ReplyAndDeleteAsync($"{Context.Message.Author} you did not reply within the given timeframe.")
                End If
                Await ReplyAndDeleteAsync("What is your desired height?", timeout:=New TimeSpan(0, 0, 15))
                Dim heightReply = Await NextMessageAsync(timeout:=New TimeSpan(0, 0, 10))
                If heightReply Is Nothing Then
                    Await ReplyAndDeleteAsync($"{Context.Message.Author} you did not reply within the given timeframe.")
                End If

                Dim width = Convert.ToInt32(widthReply.ToString)
                Dim height = Convert.ToInt32(heightReply.ToString)

                Dim path = Await _img.createImageAsync(width, height, url)
                Await Context.Channel.SendFileAsync(path)
                File.Delete(path)
            End If

        Catch ex As Exception
            loggingHandler.ErrorLog("Image", ex.Message)
        End Try

#End Region

    End Function

    <Command("masscrop", RunMode.Async)>
    <[Alias]("mc")>
    <Summary("mc | Owner only command - mass crops images from a list.")>
    Public Async Function imgMassCrop() As Task
        Dim settings = Lyuze.Settings.Data

        Try
            If Context.User.Id = settings.IDs.OwnerId Then
                For Each wall As String In Wallpapers.images
                    Dim path = Await _img.createImageAsync(1100, 450, wall)
                    Await Context.Channel.SendFileAsync(path)
                    File.Delete(path)
                Next
            Else
                Await Context.Channel.SendMessageAsync("This is only for the bot owner.")
            End If
        Catch ex As Exception
            loggingHandler.ErrorLog("Image", ex.Message)
        End Try

    End Function

End Class
