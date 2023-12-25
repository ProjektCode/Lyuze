Imports Discord.Commands
Imports Discord.WebSocket
Imports Microsoft.Extensions.DependencyInjection

Public Class GoldSystem

    Private Shared ReadOnly _gold As GoldSystem = serviceHandler.provider.GetRequiredService(Of GoldSystem)

    Private Shared ReadOnly CmdCooldownTimer As New List(Of DateTimeOffset)()
    Private Shared ReadOnly CmdCooldownTarget As New List(Of SocketGuildUser)()

    Public Shared Async Function GiveGold(ctx As SocketCommandContext, gold As Integer) As Task
        Dim _player As PlayerModel = Await Player.GetUser(ctx.User)
        '_player.Gold += gold
        'Player.UpdateUser(ctx, _player)
    End Function

    Public Shared Async Sub AntiSpam(ctx As SocketCommandContext, Optional gold As Integer = 1)
        'Anti-Level Spam
        If CmdCooldownTarget.Contains(TryCast(ctx.User, SocketGuildUser)) Then
            'If they have used this command before, take the time the user last did something, add 3 seconds, and see if it's greater than this very moment.
            If CmdCooldownTimer(CmdCooldownTarget.IndexOf(TryCast(ctx.Message.Author, SocketGuildUser))).AddSeconds(10) >= DateTimeOffset.Now Then
                'If enough time hasn't passed
                Dim secondsLeft As Integer = Math.Truncate((CmdCooldownTimer(CmdCooldownTarget.IndexOf(TryCast(ctx.Message.Author, SocketGuildUser))).AddSeconds(5) - DateTimeOffset.Now).TotalSeconds)
            Else
                'If enough time has passed, set the time for the user to right now.
                CmdCooldownTimer(CmdCooldownTarget.IndexOf(TryCast(ctx.Message.Author, SocketGuildUser))) = DateTimeOffset.Now
                _gold.GiveGold(ctx, gold)
            End If
        Else
            'If they've never used this command before, add their username and when they just used this command.
            CmdCooldownTarget.Add(TryCast(ctx.User, SocketGuildUser))
            CmdCooldownTimer.Add(DateTimeOffset.Now)
            _gold.GiveGold(ctx, gold)
        End If
    End Sub


End Class
