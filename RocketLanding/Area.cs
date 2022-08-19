using System;
using System.Collections.Generic;

namespace RocketLanding
{
    public class Area: IArea
    {
        public readonly int?[,] _platform;
        private readonly int _platformFirstRow;
        private readonly int _platformFirstColumn;
        private readonly int _platformNumberOfRows;
        private readonly int _platformNumberOfColumns;

        private readonly Dictionary<int, (int, int)> rocketLastPlatformCheck;

        public Area(int numberOfRows, int numberOfColumns, int platformFirstRow, int platformFirstColumn, int platformNumberOfRows, int platformNumberOfColumns)
        {
            if (numberOfRows <= 0 || numberOfColumns <= 0)
            {
                throw new Exception("Invalid area size");
            }

            if (platformNumberOfRows <= 0 || platformNumberOfColumns <= 0)
            {
                throw new Exception("Invalid platform size");
            }

            if (platformFirstRow < 0 || platformFirstColumn < 0 || numberOfRows < platformFirstRow + platformNumberOfRows || numberOfColumns < platformFirstColumn + platformNumberOfColumns)
            {
                throw new Exception("Platform out of boundaries");
            }

            _platformFirstRow = platformFirstRow;
            _platformFirstColumn = platformFirstColumn;
            _platformNumberOfRows = platformNumberOfRows;
            _platformNumberOfColumns = platformNumberOfColumns;

            _platform = new int?[platformNumberOfRows, platformNumberOfColumns];
            rocketLastPlatformCheck = new Dictionary<int, (int, int)>();

        }

        private bool IsInPlatform(int row, int column)
        {
            return row >= _platformFirstRow &&
                row < _platformFirstRow + _platformNumberOfRows &&
                column >= _platformFirstColumn &&
                column < _platformFirstColumn + _platformNumberOfColumns;
        }

        public string CheckLanding(int rocketID, int row, int column)
        {
            if (rocketLastPlatformCheck.ContainsKey(rocketID))
            {
                (int, int) coords = rocketLastPlatformCheck[rocketID];
                rocketLastPlatformCheck.Remove(rocketID);
                _platform[coords.Item1, coords.Item2] = null;
            }

            if (!IsInPlatform(row, column))
            {
                return "out of platform";
            }

            int platformRow = row - _platformFirstRow;
            int platformColumn = column - _platformFirstColumn;

            for (int i = Math.Max(platformRow - 1, 0); i <= Math.Min(platformRow + 1, _platformNumberOfRows - 1); i++)
            {
                for (int j = Math.Max(platformColumn - 1, 0); j <= Math.Min(platformColumn + 1, _platformNumberOfColumns - 1); j++)
                {

                    if (_platform[i, j] != null)
                    {
                        return "clash";
                    }
                }
            }

            rocketLastPlatformCheck.Add(rocketID, (platformRow, platformColumn));
            _platform[platformRow, platformColumn] = rocketID;

            return "ok for landing";
        }


    }
}

