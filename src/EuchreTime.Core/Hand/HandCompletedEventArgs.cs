using System;

namespace EuchreTime.Core.Hand
{
    public class HandCompletedEventArgs : EventArgs
    {
        public int HandWinningTeamNumber { get; set; }
        public int PointsScored { get; set; }
        public int WinningTeamTricksWon { get; set; }
        public int TeamOnePoints { get; set; }
        public int TeamTwoPoints { get; set; }

        public HandCompletedEventArgs(int handWinningTeamNumber, int pointsScored, int winningTeamTricksWon, int teamOnePoints, int teamTwoPoints)
        {
            HandWinningTeamNumber = handWinningTeamNumber;
            PointsScored = pointsScored;
            WinningTeamTricksWon = winningTeamTricksWon;
            TeamOnePoints = teamOnePoints;
            TeamTwoPoints = teamTwoPoints;
        }
    }
}
