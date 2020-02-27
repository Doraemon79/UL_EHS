using System;
using PeterO.Numbers;

namespace UlApi.Services.Helpers
{
    public class State
    {
        private readonly EDecimal _left;
        private readonly char _leftOperation;
        private readonly EDecimal _middle;
        private readonly char _rightOperation;
        private readonly EDecimal _right;
        private readonly bool _digitExpected = true;
        private readonly EContext _context;

        EDecimal Execute(EDecimal left, char operation, EDecimal right)
        {
            return operation switch
            {
                '+' => left.Add(right, _context),
                '-' => left.Subtract(right, _context),
                '*' => left.Multiply(right, _context),
                '/' => left.Divide(right, _context),
                _ => right
            };
        }

        public State(EContext context)
        {
            _left = EDecimal.Zero;
            _middle = EDecimal.Zero;
            _right = EDecimal.Zero;
            _context = context;
        }

        public State(EContext context, EDecimal left, char leftOperation, EDecimal middle, char rightOperation, EDecimal right, bool digitExpected)
        {
            _context = context;
            _left = left;
            _leftOperation = leftOperation;
            _middle = middle;
            _rightOperation = rightOperation;
            _right = right;
            _digitExpected = digitExpected;
        }

        public State Next(char c)
        {
            if (char.IsDigit(c))
            {
                return new State(
                    _context,
                    _left,
                    _leftOperation,
                    _middle,
                    _rightOperation,
                    10 * _right + (c - '0'),
                    false
                );
            }
            else
            {
                if (_digitExpected) throw new FormatException($"Misplaced character '{c}'");
                switch (c)
                {
                    case '+':
                    case '-':
                        return new State(
                            _context,
                            0,
                            default,
                            Execute(_left, _leftOperation, Execute(_middle, _rightOperation, _right)),
                            c,
                            0,
                            true
                        );
                    case '*':
                    case '/':
                        if (_rightOperation == '*' || _rightOperation == '/')
                            return new State(
                                _context,
                                _left,
                                _leftOperation,
                                Execute(_middle, _rightOperation, _right),
                                c,
                                0,
                                true
                            );
                        else
                            return new State(
                                _context,
                                _middle,
                                _rightOperation,
                                _right,
                                c,
                                0,
                                true
                            );
                    default:
                        throw new FormatException($"Unexpected character '{c}'");
                }
            }
        }

        public EDecimal Result
        {
            get
            {
                if (_digitExpected) throw new FormatException("Incomplete expression");
                return Next('+')._middle;
            }
        }
    }
}
