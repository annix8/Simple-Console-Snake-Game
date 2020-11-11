using System;
using System.Collections.Generic;

namespace Snake.Model
{
    public class Snake
    {
        private SnakeNode _head;
        private SnakeNode _tail;
        private Direction _currentDirection = Direction.Right;

        public SnakeNode Head => _head;
        public List<SnakeNode> BodyParts
        {
            get
            {
                var result = new List<SnakeNode>();
                var currentNode = _head;
                while (currentNode != null)
                {
                    result.Add(currentNode);
                    currentNode = currentNode.Prev;
                }

                return result;
            }
        }
        public int SnakeSpeedMilliSeconds { get; set; } = Constants.DEFAULT_SNAKE_SPEED_MILLISECONDS;

        public Snake()
        {
            _tail = new SnakeNode
            {
                Row = 0,
                Col = 0
            };

            SnakeNode currentNode = _tail;

            // -2 because of head and tail
            for (int i = 0; i < Constants.INITIAL_SNAKE_BODY_LENGTH - 2; i++)
            {
                var newNode = new SnakeNode
                {
                    Row = currentNode.Row,
                    Col = currentNode.Col + 1,
                    Prev = currentNode
                };

                currentNode.Next = newNode;
                currentNode = newNode;
            }

            _head = new SnakeNode
            {
                Row = currentNode.Row,
                Col = currentNode.Col + 1,
                Prev = currentNode,
                Symbol = GetSnakeHeadAccordingToDirection()
            };

            currentNode.Next = _head;
        }

        public void Grow()
        {
            GrowNewHead();
        }

        public void Move(Direction newDirection)
        {
            if (!IsNewDirectionValid(newDirection))
            {
                newDirection = _currentDirection;
            }

            _currentDirection = newDirection;

            SnakeNode newTail = _tail.Next;
            newTail.Prev = null;
            _tail = newTail;

            GrowNewHead();
        }

        private int GetNewHeadRow(int currentHeadRow)
        {
            switch (_currentDirection)
            {
                case Direction.Down: return currentHeadRow + 1;
                case Direction.Up: return currentHeadRow - 1;
                case Direction.Left:
                case Direction.Right: return currentHeadRow;
                default: return 0;
            }
        }

        private int GetNewHeadCol(int currentHeadCol)
        {
            switch (_currentDirection)
            {
                case Direction.Down:
                case Direction.Up: return currentHeadCol;
                case Direction.Left: return currentHeadCol - 1;
                case Direction.Right: return currentHeadCol + 1;
                default: return 0;
            }
        }

        private char GetSnakeHeadAccordingToDirection()
        {
            switch (_currentDirection)
            {
                case Direction.Down: return Constants.SNAKE_DOWN;
                case Direction.Up: return Constants.SNAKE_UP;
                case Direction.Left: return Constants.SNAKE_LEFT;
                case Direction.Right: return Constants.SNAKE_RIGHT;
                default: return Constants.SNAKE_RIGHT;
            }
        }

        private bool IsNewDirectionValid(Direction newDirection)
        {
            bool isValid = true;
            
            bool isLeftRightInvalid = (_currentDirection == Direction.Right && newDirection == Direction.Left) ||
                (_currentDirection == Direction.Left && newDirection == Direction.Right);

            bool isUpDownInvalid = (_currentDirection == Direction.Up && newDirection == Direction.Down) ||
                (_currentDirection == Direction.Down && newDirection == Direction.Up);

            if (isLeftRightInvalid || isUpDownInvalid)
            {
                isValid = false;
            }

            return isValid;
        }

        private void GrowNewHead()
        {
            SnakeNode newHead = new SnakeNode
            {
                Row = GetNewHeadRow(_head.Row),
                Col = GetNewHeadCol(_head.Col),
                Prev = _head,
                Symbol = GetSnakeHeadAccordingToDirection()
            };

            _head.Symbol = Constants.SNAKE_BODY;
            _head.Next = newHead;
            _head = newHead;
        }
    }
}
