using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public static class Constants
    {
        public const char SNAKE_BODY = '*';
        public const char SNAKE_RIGHT = '>';
        public const char SNAKE_LEFT = '<';
        public const char SNAKE_UP = '^';
        public const char SNAKE_DOWN = 'v';
        public const char FOOD = '@';
        public const char OBSTACLE = '#';
        public const char EMPTY_SPACE = ' ';

        public const int INITIAL_SNAKE_BODY_LENGTH = 10;
        public const int DEFAULT_SNAKE_SPEED_MILLISECONDS = 50;
        public const int OBSTACLES_COUNT = 15;
    }
}
