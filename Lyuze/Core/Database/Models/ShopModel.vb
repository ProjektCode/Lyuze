Imports Discord.Commands
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

Public Class ItemModel
    <BsonId>
    <BsonRepresentation(BsonType.Int64)>
    Public Property ID() As New ObjectId
    Public Property Name() As String
    Public Property Description() As String
    Public Property Cost() As Integer

    Public Property URL() As String

End Class

'Public Class ShopModel

'    Public Shared Async Function CheckGold(ctx As SocketCommandContext, i As ItemModel) As Task(Of Boolean)
'        Dim _player = Await Player.GetUser(ctx)
'        If _player.Gold >= i.Cost Then
'            Return True
'        End If
'        Return False
'    End Function

'    Public Shared Async Function BuyItem(ctx As SocketCommandContext, i As ItemModel) As Task
'        Dim _player As PlayerModel = Await Player.GetUser(ctx)
'        If Await CheckGold(ctx, i) Then
'            Player.AddItem(ctx, i)
'        End If
'    End Function

'End Class
