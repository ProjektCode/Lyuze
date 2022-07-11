Imports Discord.Commands
Imports MongoDB.Driver
Imports MongoDB.Bson
Imports MongoDB
Imports MongoDB.Bson.Serialization.Attributes

Public Class PlayerModel
    <BsonId>
    <BsonRepresentation(BsonType.Int64)>
    Public Property DiscordID() As ULong
    Public Property Level() As Integer
    Public Property XP() As Integer
    Public Property Coins() As Integer
    Public Property Background() As String
End Class

Public Class Player
    Shared ReadOnly db As New DatabaseContext

    Public Shared Function CreateProfile(ctx As SocketCommandContext)
        Dim player As New PlayerModel With {
            .DiscordID = ctx.User.Id,
            .Level = 1,
            .Coins = 100,
            .XP = 0,
            .Background = "http://lyuze-api.projektcode.com/images/banners/info/profile/user-3"
        }
        db._collection.InsertOneAsync(player)
    End Function

    Public Shared Function HasProfile(ctx As SocketCommandContext) As Boolean
        Dim result = db._collection.Find(Function(x) x.DiscordID = ctx.User.Id)
        If result.CountDocuments = 1 Then
            Return True
        End If
        Return False
    End Function

    Public Shared Async Function GetUser(ctx As SocketCommandContext) As Task(Of PlayerModel)
        Dim filter = Builders(Of PlayerModel).Filter.Eq(Function(x) x.DiscordID, ctx.User.Id)
        Dim user = db._collection.Find(filter).FirstOrDefault
        Return user
    End Function

    Public Shared Function CheckProfile(ctx As SocketCommandContext)
        If Not HasProfile(ctx) Then
            CreateProfile(ctx)
        End If
    End Function

    Public Shared Async Function UpdateUser(ctx As SocketCommandContext, u As PlayerModel) As Task
        Dim filter = Builders(Of PlayerModel).Filter.Eq(Function(x) x.DiscordID, ctx.User.Id)
        'Dim user = db._collection.Find(filter).FirstOrDefault
        Await db._collection.ReplaceOneAsync(filter, u)
        Return
    End Function

End Class
