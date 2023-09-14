Imports Discord.Commands
Imports Discord.WebSocket
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

'Abondon Shop Model - Might come back to it if I find a use for it.
Public Class ItemModel
    <BsonId>
    <BsonRepresentation(BsonType.Int64)>
    Public Property ID() As New ObjectId
    Public Property Name() As String
    Public Property Description() As String
    Public Property Cost() As Integer

    Public Property URL() As String

End Class

Public Class ShopModel

    Public Shared Async Function CheckGold(user As SocketGuildUser, i As ItemModel) As Task(Of Boolean)
        Dim _player = Await Player.GetUser(user)
        'If _player.Gold >= i.Cost Then
        '    Return True
        'End If
        Return False
    End Function

    Public Shared Async Function BuyItem(user As SocketGuildUser, i As ItemModel) As Task
        Dim _player As PlayerModel = Await Player.GetUser(user)
        If Await CheckGold(user, i) Then
            'Player.AddItem(ctx, i)
        End If
    End Function

End Class
