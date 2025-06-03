public enum PlayerType { PLAYER, AI }

public enum TrajectoryType { EASY, MEDIUM, HARD }

public enum CoinType { EASY, MEDIUM, HARD }

public enum TurnPhase { ActiveCoinSelection, PassiveTrajectorySelection, ActiveTrajectorySelection, PassiveThrow ,ActiveQuickTimeEvent, ActivePointAssign, VictoryChecks, NextPlayer }

public enum ItemUsePhase { ActiveTrajectorySelection, PassiveTrajectorySelection }

public enum ItemType { LuckyBullet, RustyCoin }