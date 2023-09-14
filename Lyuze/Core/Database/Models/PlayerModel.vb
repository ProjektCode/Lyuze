Imports MongoDB.Driver
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports Discord.WebSocket
Imports Microsoft.Extensions.DependencyInjection

Public Class PlayerModel
    <BsonId>
    <BsonRepresentation(BsonType.Int64)>
    Public Property DiscordID() As ULong
    Public Property Level() As Integer
    Public Property XP() As Integer
    ' Public Property Gold() As Integer
    Public Property Background() As String
    Public Property FavChar() As String
    Public Property AboutMe() As String
    Public Property PublicProfile() As String
    Public Property LevelNotify() As Boolean
    Public Property InfractionCount() As Integer
    Public Property InfractionMessages() As List(Of String)
End Class

Public Class Player
    Private Shared ReadOnly db As DatabaseContext = serviceHandler.provider.GetRequiredService(Of DatabaseContext)
    'Shared ReadOnly db As New DatabaseContext
    Shared ReadOnly rand As New Random

    'Creates user's profile
    Public Shared Function CreateProfile(user As SocketGuildUser)
        Dim num As Integer = rand.Next(Settings.Data.ProfileBanners.Count)
        Dim InfractionList As New List(Of String)
        Dim player As New PlayerModel With {
            .DiscordID = user.Id,
            .Level = 1,
            .XP = 0,
            .Background = Settings.Data.ProfileBanners(num).AbsoluteUri,
            .AboutMe = "About me has not been set.",
            .FavChar = "Favorite Character has not been set.",
            .PublicProfile = "Private",
            .LevelNotify = True,
            .InfractionCount = 0,
            .InfractionMessages = InfractionList
        }
        db.playerCollection.InsertOneAsync(player)
    End Function

    'Checks if user has a profile
    Public Shared Function HasProfile(user As SocketGuildUser) As Boolean
        Dim result = db.playerCollection.Find(Function(x) x.DiscordID = user.Id)
        If result.CountDocuments = 1 Then
            Return True
        End If
        Return False
    End Function

    'Gets user from the database using a filter
    Public Shared Async Function GetUser(user As SocketGuildUser) As Task(Of PlayerModel)
        Dim filter = Builders(Of PlayerModel).Filter.Eq(Function(x) x.DiscordID, user.Id)
        Dim u = db.playerCollection.Find(filter).FirstOrDefault
        Return u
    End Function

    'Checks if user has profile if not goes to create
    Public Shared Function CheckProfile(user As SocketGuildUser)
        If Not HasProfile(user) Then
            CreateProfile(user)
        End If
    End Function

    'Updates user's profile
    Public Shared Async Function UpdateUser(user As SocketGuildUser, p As PlayerModel) As Task
        Dim filter = Builders(Of PlayerModel).Filter.Eq(Function(x) x.DiscordID, user.Id)
        Await db.playerCollection.ReplaceOneAsync(filter, p)
        Return
    End Function

End Class
