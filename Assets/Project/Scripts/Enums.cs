public enum PlayerType { PLAYER, AI }

public enum TrajectoryType { EASY, MEDIUM, HARD }

public enum CoinType { EASY, MEDIUM, HARD }

public enum TurnPhase { ActiveCoinSelection, PassiveTrajectorySelection, ActiveTrajectorySelection, PassiveThrow ,ActiveQuickTimeEvent, ActivePointAssign, VictoryChecks, NextPlayer }

public enum ItemUsePhase { ActiveTrajectorySelection, PassiveTrajectorySelection }

public enum ItemType { LuckyBullet, RustyCoin }

public enum DialogueType
{
    tailsDialogue,
    headsDialogue,
    easyThrowDialogue,
    mediumThrowDialogue,
    difficultThrowDialogue,
    failedShootDialogue,
    succesShootDialogue,
    playerWinDialogue,
    playerLoseDialogue,
    EnemyDifficultySelectionDialogue,
    EnemyEndTurnDialogue
}

public enum CommandType {none, shoot, flip }