Imports Discord.Commands
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
End Class

Public Class Player
    Private Shared ReadOnly db As DatabaseContext = serviceHandler.provider.GetRequiredService(Of DatabaseContext)
    'Shared ReadOnly db As New DatabaseContext
    Shared ReadOnly rand As New Random

    Public Shared Function CreateProfile(user As SocketGuildUser)
        Dim num As Integer = rand.Next(1, 3)
        Dim player As New PlayerModel With {
            .DiscordID = user.Id,
            .Level = 1,
            .XP = 0,
            .Background = $"http://lyuze-api.projektcode.com/images/banners/info/profile/user-{num}",
            .AboutMe = "About me has not been set.",
            .FavChar = "Favorite Character has not been set.",
            .PublicProfile = "Private"
        }
        db.playerCollection.InsertOneAsync(player)
    End Function

    Public Shared Function HasProfile(user As SocketGuildUser) As Boolean
        Dim result = db.playerCollection.Find(Function(x) x.DiscordID = user.Id)
        If result.CountDocuments = 1 Then
            Return True
        End If
        Return False
    End Function

    Public Shared Async Function GetUser(user As SocketGuildUser) As Task(Of PlayerModel)
        Dim filter = Builders(Of PlayerModel).Filter.Eq(Function(x) x.DiscordID, user.Id)
        Dim u = db.playerCollection.Find(filter).FirstOrDefault
        Return u
    End Function

    Public Shared Function CheckProfile(user As SocketGuildUser)
        If Not HasProfile(user) Then
            CreateProfile(user)
        End If
    End Function

    Public Shared Async Function UpdateUser(user As SocketGuildUser, p As PlayerModel) As Task
        Dim filter = Builders(Of PlayerModel).Filter.Eq(Function(x) x.DiscordID, user.Id)
        'Dim user = db._collection.Find(filter).FirstOrDefault
        Await db.playerCollection.ReplaceOneAsync(filter, p)
        Return
    End Function

End Class
