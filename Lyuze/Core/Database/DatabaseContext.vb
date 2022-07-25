Imports MongoDB.Driver

Public Class DatabaseContext
    Public _client As MongoClient
    Public _database As IMongoDatabase
    Public _playerCollection As IMongoCollection(Of PlayerModel)

    Public Sub New()
        Dim settings = Lyuze.Settings.Data
        _client = New MongoClient(settings.Database.MongoDb)
        _database = _client.GetDatabase("MyDatabase")
        _playerCollection = _database.GetCollection(Of PlayerModel)("Players")
    End Sub

End Class
