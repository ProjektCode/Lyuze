Imports Discord
Imports Discord.Commands
Imports Discord.WebSocket
Imports Microsoft.Extensions.DependencyInjection

Public Class LevelingSystem
    Private Shared ReadOnly _imgs As Images = serviceHandler.provider.GetRequiredService(Of Images)
    Private Shared ReadOnly _lvl As LevelingSystem = serviceHandler.provider.GetRequiredService(Of LevelingSystem)
    ' Private Shared ReadOnly _gold As GoldSystem = serviceHandler.provider.GetRequiredService(Of GoldSystem)

    Private Shared ReadOnly MsgCooldownTimer As New List(Of DateTimeOffset)()
    Private Shared ReadOnly MsgCooldownTarget As New List(Of SocketGuildUser)()
    'Private Shared ReadOnly CmdCooldownTimer As New List(Of DateTimeOffset)()
    'Private Shared ReadOnly CmdCooldownTarget As New List(Of SocketGuildUser)()

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
        _player.Level += 1
        _player.XP = 0

        Dim embed As New EmbedBuilder With {
            .Title = $"{user.Username} Has Reached Level {_player.Level}!",
            .Description = $"{LevelEquation(_player.Level)} XP needed for the next level!",
            .Color = New Color((Await _imgs.RandomColorFromURL(If(user.GetAvatarUrl, user.GetDefaultAvatarUrl)))),
            .ThumbnailUrl = If(user.GetAvatarUrl, user.GetDefaultAvatarUrl)
          }
        Await Player.UpdateUser(user, _player)
        Await ctx.Channel.SendMessageAsync(embed:=embed.Build)
    End Function

    Public Async Function GiveXP(user As SocketGuildUser, xp As Integer) As Task
        Dim _player As PlayerModel = Await Player.GetUser(user)
        _player.XP += xp
        Player.UpdateUser(user, _player)
    End Function

    Public Async Sub MsgAntiSpam(user As SocketGuildUser, ctx As SocketCommandContext, Optional xp As Integer = 1)
        'Anti-LevelSplan
        If MsgCooldownTarget.Contains(TryCast(ctx.User, SocketGuildUser)) Then
            'If they have used this command before, take the time the user last did something, add 3 seconds, and see if it's greater than this very moment.
            If MsgCooldownTimer(MsgCooldownTarget.IndexOf(TryCast(ctx.Message.Author, SocketGuildUser))).AddSeconds(3) >= DateTimeOffset.Now Then
                'If enough time hasn't passed
                Dim secondsLeft As Integer = Math.Truncate((MsgCooldownTimer(MsgCooldownTarget.IndexOf(TryCast(ctx.Message.Author, SocketGuildUser))).AddSeconds(3) - DateTimeOffset.Now).TotalSeconds)
            Else
                'If enough time has passed, set the time for the user to right now.
                MsgCooldownTimer(MsgCooldownTarget.IndexOf(TryCast(ctx.Message.Author, SocketGuildUser))) = DateTimeOffset.Now
                _lvl.GiveXP(user, xp)
                '_gold.AntiSpam(ctx)
                If Await _lvl.CanLevelUp(user) Then
                    _lvl.LevelUp(user, ctx)
                End If
            End If
        Else
            'If they've never used this command before, add their username and when they just used this command.
            MsgCooldownTarget.Add(TryCast(ctx.User, SocketGuildUser))
            MsgCooldownTimer.Add(DateTimeOffset.Now)
            _lvl.GiveXP(user, xp)
            '_gold.AntiSpam(ctx)
            If Await _lvl.CanLevelUp(user) Then
                _lvl.LevelUp(user, ctx)
            End If
        End If
    End Sub

    'Public Async Sub CmdAntiSpam(ctx As SocketCommandContext, Optional xp As Integer = 1)
    '    'Anti-LevelSplan
    '    If CmdCooldownTarget.Contains(TryCast(ctx.User, SocketGuildUser)) Then
    '        'If they have used this command before, take the time the user last did something, add 3 seconds, and see if it's greater than this very moment.
    '        If CmdCooldownTimer(CmdCooldownTarget.IndexOf(TryCast(ctx.Message.Author, SocketGuildUser))).AddSeconds(5) >= DateTimeOffset.Now Then
    '            'If enough time hasn't passed
    '            Dim secondsLeft As Integer = Math.Truncate((CmdCooldownTimer(CmdCooldownTarget.IndexOf(TryCast(ctx.Message.Author, SocketGuildUser))).AddSeconds(5) - DateTimeOffset.Now).TotalSeconds)
    '        Else
    '            'If enough time has passed, set the time for the user to right now.
    '            CmdCooldownTimer(CmdCooldownTarget.IndexOf(TryCast(ctx.Message.Author, SocketGuildUser))) = DateTimeOffset.Now
    '            _lvl.GiveXP(ctx, xp)
    '            '_gold.AntiSpam(ctx)
    '            If Await _lvl.CanLevelUp(ctx) Then
    '                _lvl.LevelUp(ctx)
    '            End If
    '        End If
    '    Else
    '        'If they've never used this command before, add their username and when they just used this command.
    '        CmdCooldownTarget.Add(TryCast(ctx.User, SocketGuildUser))
    '        CmdCooldownTimer.Add(DateTimeOffset.Now)
    '        _lvl.GiveXP(ctx, xp)
    '        '_gold.AntiSpam(ctx)
    '        If Await _lvl.CanLevelUp(ctx) Then
    '            _lvl.LevelUp(ctx)
    '        End If
    '    End If
    'End Sub

End Class
