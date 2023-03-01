Imports Discord
Imports Discord.Commands
Imports Discord.WebSocket
Imports Microsoft.Extensions.DependencyInjection

Public Class LevelingSystem
    Private Shared ReadOnly _imgs As Images = serviceHandler.provider.GetRequiredService(Of Images)
    Private Shared ReadOnly _lvl As LevelingSystem = serviceHandler.provider.GetRequiredService(Of LevelingSystem)
    ' Private Shared ReadOnly _gold As GoldSystem = serviceHandler.provider.GetRequiredService(Of GoldSystem)

    'Private Shared ReadOnly MsgCooldownTimer As New List(Of DateTimeOffset)()
    'Private Shared ReadOnly MsgCooldownTarget As New List(Of SocketGuildUser)()
    Private Shared ReadOnly MsgList As New List(Of Message)
    Private Shared ReadOnly AuthorList As New List(Of ULong)

    'f(x) = (10 * x^2) f(x) is xp needed - x is level
    Public Function LevelEquation(lvl As Integer) As Double
        Dim xp As Double = Math.Floor(Math.Round(25 * Math.Pow(lvl + 1, 2)))
        Return xp
    End Function

    Public Async Function CanLevelUp(user As SocketGuildUser) As Task(Of Boolean)
        Dim _player As PlayerModel = Await Player.GetUser(user)
        Dim needXP As Double = LevelEquation(_player.Level)

        If _player.XP >= needXP Then
            Return True
        End If

        Return False
    End Function

    Public Async Function LevelUp(user As SocketGuildUser, ctx As SocketCommandContext) As Task
        Dim _player As PlayerModel = Await Player.GetUser(user)
        Dim xp As Integer = 0
        'Untested
        If _player.XP > LevelEquation(_player.Level) Then
            xp = _player.XP - LevelEquation(_player.Level)
        End If

        _player.Level += 1
        _player.XP = xp

        If _player.LevelNotify = True Then
            Dim embed As New EmbedBuilder With {
            .Title = $"{user.Username} Has Reached Level {_player.Level}!",
            .Description = $"{LevelEquation(_player.Level)} XP needed for the next level!",
            .Color = New Color((Await _imgs.RandomColorFromURL(If(user.GetAvatarUrl, user.GetDefaultAvatarUrl)))),
            .ThumbnailUrl = If(user.GetAvatarUrl, user.GetDefaultAvatarUrl)
          }
            Await Player.UpdateUser(user, _player)

            Await ctx.Channel.SendMessageAsync(embed:=embed.Build)
            Return
        End If

        Await Player.UpdateUser(user, _player)
    End Function

    Public Async Function GiveXP(user As SocketGuildUser, xp As Integer) As Task
        Dim _player As PlayerModel = Await Player.GetUser(user)
        _player.XP += xp
        Player.UpdateUser(user, _player)
    End Function

    'Public Async Sub MsgAntiSpam(user As SocketGuildUser, ctx As SocketCommandContext, Optional xp As Integer = 1)
    '    'Anti-LevelSplan
    '    If MsgCooldownTarget.Contains(TryCast(ctx.User, SocketGuildUser)) Then
    '        'If they have used this command before, take the time the user last did something, add 3 seconds, and see if it's greater than this very moment.
    '        If MsgCooldownTimer(MsgCooldownTarget.IndexOf(TryCast(ctx.Message.Author, SocketGuildUser))).AddSeconds(3) >= DateTimeOffset.Now Then
    '            'If enough time hasn't passed
    '            Dim secondsLeft As Integer = Math.Truncate((MsgCooldownTimer(MsgCooldownTarget.IndexOf(TryCast(ctx.Message.Author, SocketGuildUser))).AddSeconds(3) - DateTimeOffset.Now).TotalSeconds)
    '        Else
    '            'If enough time has passed, set the time for the user to right now.
    '            MsgCooldownTimer(MsgCooldownTarget.IndexOf(TryCast(ctx.Message.Author, SocketGuildUser))) = DateTimeOffset.Now
    '            _lvl.GiveXP(user, xp)
    '            '_gold.AntiSpam(ctx)
    '            If Await _lvl.CanLevelUp(user) Then
    '                _lvl.LevelUp(user, ctx)
    '            End If
    '        End If
    '    Else
    '        'If they've never used this command before, add their username and when they just used this command.
    '        MsgCooldownTarget.Add(TryCast(ctx.User, SocketGuildUser))
    '        MsgCooldownTimer.Add(DateTimeOffset.Now)
    '        LevelHelper(user, xp, ctx)
    '    End If
    'End Sub

    Public Async Sub MsgCooldown(msg As IUserMessage, ctx As SocketCommandContext, Optional xp As Integer = 1)
        Dim newMsg As New Message With {
            .AuthorID = msg.Author.Id,
            .Timestamp = msg.Timestamp
        }

        If AuthorList.Contains(msg.Author.Id) Then
            'Check the current time and see if it's after 3 seconds of the last message.
            Dim AuthorMsg = MsgList.Find(Function(x) x.AuthorID = newMsg.AuthorID)
            If Not AuthorMsg.Timestamp.AddSeconds(3) >= DateTimeOffset.Now Then
                AuthorList.Remove(msg.Author.Id)
                MsgList.Remove(AuthorMsg)
                LevelHelper(msg.Author, xp, ctx)
            End If

        Else
            AuthorList.Add(msg.Author.Id)
            MsgList.Add(newMsg)
            LevelHelper(msg.Author, xp, ctx)
        End If

    End Sub

    Public Async Sub LevelHelper(user As SocketGuildUser, xp As Integer, ctx As SocketCommandContext)
        _lvl.GiveXP(user, xp)
        If Await _lvl.CanLevelUp(user) Then
            _lvl.LevelUp(user, ctx)
        End If
    End Sub

End Class

Partial Class Message
    Public Property AuthorID As ULong
    Public Property Timestamp As DateTimeOffset
End Class