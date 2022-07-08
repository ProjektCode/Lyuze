Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Net.Http
Imports Discord.WebSocket
Imports ColorThiefDotNet
Imports System.Net
Imports System.Globalization

Public Class Images

    'Returns a image with the user's avatar and a welcome message
    Public Async Function CreateBannerImageAsync(user As SocketGuildUser, msg As String, submsg As String) As Task(Of String)
        Dim avatar = Await fetchImageAsync(If(user.GetAvatarUrl(Discord.ImageFormat.Png, 2048), user.GetDefaultAvatarUrl))
        Dim background = Await fetchImageAsync(Wallpapers.returnImage.Result)

        background = cropToBanner(background)
        Dim border = circleBorder(avatar)
        avatar = clipImageToCircle(avatar)

        Dim amap As Bitmap = TryCast(avatar, Bitmap)
        Dim bmap As Bitmap = TryCast(border, Bitmap)
        bmap?.MakeTransparent()

        Dim banner = copyRegionIntoBanner(bmap, amap, background)
        banner = drawTextToImage(banner, msg, submsg)

        Dim path As String = $"{Guid.NewGuid}.png"
        banner.Save(path)

        Return Await Task.FromResult(path)
    End Function

    'Returns a cropped image
    Public Async Function createImageAsync(width As Integer, height As Integer, url As String) As Task(Of String)
        Dim background = Await fetchImageAsync(If(url, Wallpapers.returnImage.Result))

        If background.Width < width Or background.Height < height Then
            background = Await fetchImageAsync(Wallpapers.returnImage.Result)
            background = cropToBanner(background)
            background = drawTextToImage(background, "Error has occurred.", "Either your height or width is larger than the image's original width/hieght.")
        Else
            background = cropToBanner(background, width, height)
        End If


        Dim path As String = $"{Guid.NewGuid}.png"
        background.Save(path)

        Return Await Task.FromResult(path)
    End Function

    Private Shared Function cropToBanner(img As Image, Optional width As Integer = 1100, Optional height As Integer = 450) As Bitmap
        Dim orgWidth = img.Width
        Dim orgHieght = img.Height

        Dim destinationSize = New Size(width, height)

        Dim heightRatio = Convert.ToDecimal(orgHieght / destinationSize.Height)
        Dim widthRatio = Convert.ToDecimal(orgWidth / destinationSize.Width)
        Dim ratio = MathF.Min(heightRatio, widthRatio)

        Dim heightScale = Convert.ToInt32(destinationSize.Height * ratio)
        Dim widthScale = Convert.ToInt32(destinationSize.Width * ratio)
        Dim startX = (orgWidth - widthScale) / 2
        Dim startY = (orgHieght - heightScale) / 2

        Dim sourceRectangle = New Rectangle(startX, startY, widthScale, heightScale)
        Dim bitmap = New Bitmap(destinationSize.Width, destinationSize.Height)
        Dim destinationRectangle = New Rectangle(0, 0, bitmap.Width, bitmap.Height)

        Using g = Graphics.FromImage(bitmap)
            g.InterpolationMode = InterpolationMode.HighQualityBicubic
            g.DrawImage(img, destinationRectangle, sourceRectangle, GraphicsUnit.Pixel)

            Return bitmap
        End Using

    End Function

    Private Function circleBorder(img As Image) As Image
        Dim destination As Image = New Bitmap(img.Width, img.Height, img.PixelFormat)
        Dim radius = img.Width / 2
        Dim x = img.Width / 2
        Dim y = img.Height / 2

        Using g = Graphics.FromImage(destination)
            Dim r = New Rectangle(x - radius, y - radius, radius * 2, radius * 2)
            Dim myPen = New Pen(Drawing.Color.Crimson, x)
            g.InterpolationMode = InterpolationMode.HighQualityBicubic
            g.CompositingQuality = CompositingQuality.HighQuality
            g.PixelOffsetMode = PixelOffsetMode.HighQuality
            g.SmoothingMode = SmoothingMode.AntiAlias

            Dim path = New GraphicsPath
            path.AddEllipse(r)
            g.SetClip(path)
            g.DrawEllipse(myPen, r)
            Return destination
        End Using

    End Function

    Private Function clipImageToCircle(img As Image) As Image
        Dim destination As Image = New Bitmap(img.Width, img.Height, img.PixelFormat)
        Dim radius = img.Width / 2
        Dim x = img.Width / 2
        Dim y = img.Height / 2


        Using g = Graphics.FromImage(destination)
            Dim r = New Rectangle(x - radius, y - radius, radius * 2, radius * 2)
            g.InterpolationMode = InterpolationMode.HighQualityBicubic
            g.CompositingQuality = CompositingQuality.HighQuality
            g.PixelOffsetMode = PixelOffsetMode.HighQuality
            g.SmoothingMode = SmoothingMode.AntiAlias

            Using brush As Brush = New SolidBrush(Drawing.Color.Transparent)
                g.FillRectangle(brush, 0, 0, destination.Width, destination.Height)
            End Using

            Dim path = New GraphicsPath
            path.AddEllipse(r)
            g.SetClip(path)
            g.DrawImage(img, 0, 0)

            Return destination
        End Using

    End Function

    Private Function copyRegionIntoBanner(circleBorder As Image, imgSource As Image, destination As Image) As Image
        Using g = Graphics.FromImage(destination)
            Dim x = (destination.Width / 2) - 110
            Dim y = (destination.Height / 2) - 155
            Dim _x = (destination.Width / 2) - 114
            Dim _y = (destination.Height / 2) - 159

            g.DrawImage(circleBorder, CInt(_x), CInt(_y), 233, 233)
            g.DrawImage(imgSource, CInt(x), CInt(y), 225, 225)

            Return destination
        End Using

    End Function

    Private Function drawTextToImage(img As Image, header As String, subHeader As String) As Image
        Dim head = New Font("Roboto", 30, FontStyle.Regular)
        Dim subH = New Font("Roboto", 23, FontStyle.Regular)

        Dim brushWhite = New SolidBrush(Drawing.Color.Crimson)
        Dim brushGrey = New SolidBrush(ColorTranslator.FromHtml("#B3B3B3"))

        Dim headerX = img.Width / 2
        Dim headerY = (img.Height / 2) + 115

        Dim subHeaderX = img.Width / 2
        Dim subHeaderY = (img.Height / 2) + 160

        Dim drawFormat = New StringFormat With {
            .LineAlignment = StringAlignment.Center,
            .Alignment = StringAlignment.Center
        }

        Using g = Graphics.FromImage(img)
            g.TextRenderingHint = Text.TextRenderingHint.AntiAliasGridFit
            g.DrawString(header, head, brushWhite, headerX, headerY, drawFormat)
            g.DrawString(subHeader, subH, brushGrey, subHeaderX, subHeaderY, drawFormat)
            Dim _img = New Bitmap(img)
            Return _img
        End Using

    End Function

    Private Async Function fetchImageAsync(url As String) As Task(Of Image)
        Dim client = New HttpClient
        Dim response = Await client.GetAsync(url)

        If Not response.IsSuccessStatusCode Then
            Dim backupResponse = Await client.GetAsync("https://images.unsplash.com/photo-1433259651738-0e74537aa8b5?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=1469&q=80")
            Dim backupStream = Await backupResponse.Content.ReadAsStreamAsync
            Return Image.FromStream(backupStream)
        End If

        Dim stream = Await response.Content.ReadAsStreamAsync
        Return Image.FromStream(stream)
    End Function

    'Returns 8 different hex codes from the given bitmap
    Private Async Function GenerateColors(bmap As Bitmap) As Task(Of IEnumerable(Of String))
        Dim thief = New ColorThief

        Dim colors = thief.GetPalette(bmap, 9)

        Return colors.Select(Function(x) x.Color.ToHexString()).ToList()
    End Function

    'Returns image URL as bitmap
    Private Async Function URLToBitmap(url As String) As Task(Of Bitmap)
        Dim client = New WebClient

        Dim stream = client.OpenRead(url)

        'If the stream can't be read set up a backup URL
        If Not stream.CanRead Then
            url = "https://images.unsplash.com/photo-1493514789931-586cb221d7a7?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=6512&q=80"
            stream = client.OpenRead(url)
        End If

        Dim bmap = New Bitmap(stream)

        Return bmap
    End Function

    'Return a UInteger for discord color when using a image URL
    Public Async Function RandomColorFromURL(url As String) As Task(Of UInteger)
        Dim newUrl As String

        If url.Contains(" ") Then
            newUrl = url.Replace(" ", "-")
        Else
            newUrl = url
        End If

        Dim rand As New Random
        Dim bmap = URLToBitmap(newUrl)
        Dim hex = Await GenerateColors(bmap.Result)
        Dim randomHex = hex(rand.Next(0, hex.Count))
        Dim newHex = randomHex.Replace("#", "")

        Dim colorInt As Integer = Integer.Parse(newHex, NumberStyles.HexNumber)
        Dim color As UInteger = Convert.ToInt32(colorInt)

        Return color
    End Function


End Class