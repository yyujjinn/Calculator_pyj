using System.Collections.Generic;

namespace Calculator_pyj.Model
{
    class CalculatorModel
    {
        #region [상수]
        Stack<string> operatorStack = new Stack<string>();
        List<string> postfixTokens = new List<string>();

        #endregion

        #region [메서드]
        /**
        * @brief 연산자의 우선순위를 나타내는 메서드
        * @param op: 입력 받은 연산자
        * @note Patch-notes
        * 2023-08-14 | 박유진 | 
        */
        public int GetPrecedence(string op)
        {
            switch (op)
            {
                case "(":
                case ")":
                    return 1;

                case "+":
                case "-":
                    return 3;

                case "x":
                case "/":
                    return 5;

                default:
                    return 0;
            }
        }

        /**
        * @brief 연산자 버튼인지 판별하는 메서드
        * @param token: 저장된 연산자
        * @note Patch-notes
        * 2023-08-10 | 박유진 |
        */
        public bool IsOperator(string token)
        {
            return token == "+" || token == "-" || token == "x" || token == "/";
        }

        /**
        * @brief 중위표기법으로 표현된 수식을 후위표기법으로 변환하는 메서드
        * @param expression: 중위표기법으로 입력 받은 수식, token: 수식을 띄어쓰기를 기준으로 나눈 값
        * @note Patch-notes
        * 2023-08-14 | 박유진 | 
        */
        public string ConvertToPostfix(string expression)
        {
            string[] tokens = expression.Split(' ');

            foreach (string token in tokens)
            {
                if (double.TryParse(token, out double num))
                {
                    postfixTokens.Add(token);
                }
                else
                {
                    while (operatorStack.Count != 0)
                    {
                        if (GetPrecedence(token) <= GetPrecedence(operatorStack.Peek()))
                        {
                            postfixTokens.Add(operatorStack.Pop().ToString());
                        }
                        else
                            break;
                    }
                    operatorStack.Push(token);
                }
            }

            while (operatorStack.Count > 0)
            {
                postfixTokens.Add(operatorStack.Pop().ToString());
            }

            return string.Join(" ", postfixTokens);
        }

        #endregion
    }

    public class BaseCalculator
    {
        #region [메서드]
        /**
        * @brief 숫자를 곱하는 함수
        * @param n1: 첫 번째 숫자, n2: 두 번째 숫자
        * @return n1 * n2: 두 숫자의 곱
        * @note Patch-notes
        * 2023-08-10 | 박유진 | 첫 번째 입력 값과 두 번째 입력 값을 곱해줌
        */
        public double Muliple(double n1, double n2)
        {
            return n1 * n2;
        }

        /**
        * @brief 숫자를 나누는 함수
        * @param n1: 첫 번째 숫자, n2: 두 번째 숫자
        * @return n1 / n2: 첫 번째 숫자에서 두 번째 숫자를 나눈 값
        * @note Patch-notes
        * 2023-08-10 | 박유진 | 첫 번째 입력 값에서 두 번째 입력 값을 나눠줌
        */
        public double Divide(double n1, double n2)
        {
            return n1 / n2;
        }

        #endregion
    }

    public class PerformCalculator : BaseCalculator
    {
        #region [속성]
        public double Value { get; set; }

        #endregion

        #region [생성자]
        public PerformCalculator(double value)
        {
            Value = value;
        }

        #endregion

        #region [메서드]
        /**
        * @brief + 연산자 재정의
        * @param num1: 첫 번째 PerformCalculator 인스턴스, num2: 두 번째 PerformCalculator 인스턴스
        * @return PerformCalculator 인스턴스: 두 인스턴스의 Value를 더한 결과를 가진 새로운 인스턴스 반환
        * @note Patch-notes
        * 2023-08-17 | 박유진 | 
        */
        public static PerformCalculator operator +(PerformCalculator num1, PerformCalculator num2)
        {
            return new PerformCalculator(num1.Value + num2.Value);
        }

        /**
        * @brief - 연산자 재정의
        * @param num1: 첫 번째 PerformCalculator 인스턴스, num2: 두 번째 PerformCalculator 인스턴스
        * @return PerformCalculator 인스턴스: 두 인스턴스의 Value를 뺀 결과를 가진 새로운 인스턴스 반환
        * @note Patch-notes
        * 2023-08-17 | 박유진 | 
        */
        public static PerformCalculator operator -(PerformCalculator num1, PerformCalculator num2)
        {
            return new PerformCalculator(num1.Value - num2.Value);
        }

        /**
        * @brief 두 숫자의 곱을 계산하는 메서드
        * @param n1: 첫 번째 숫자, n2: 두 번째 숫자
        * @return n1과 n2를 곱한 값
        * @note Patch-notes
        * 2023-08-17 | 박유진 |
        */

        public double PerformMultiplication(double n1, double n2)
        {
            return Muliple(n1, n2);
        }

        /**
        * @brief 첫 번째 숫자에서 두 번째 숫자의 나눗셈을 계산하는 메서드
        * @param n1: 첫 번째 숫자, n2: 두 번째 숫자
        * @return n1에서 n2를 나눈 값
        * @note Patch-notes
        * 2023-08-17 | 박유진 |
        */
        public double PerformDivision(double n1, double n2)
        {
            return Divide(n1, n2);
        }

        #endregion
    }
}