using System.Collections.Generic;

namespace Calculator_pyj.Model
{
    class CalculatorModel
    {
        Stack<string> operatorStack = new Stack<string>();
        List<string> postfixTokens = new List<string>();

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
    }

    public class BaseCalculator
    {
        /**
        * @brief 숫자를 더하는 함수
        * @param n1: 첫 번째 숫자, n2: 두 번째 숫자
        * @return n1 + n2: 두 숫자의 합
        * @note Patch-notes
        * 2023-08-10 | 박유진 | 첫 번째 입력 값과 두 번째 입력 값을 더해줌
        */
        public double Add(double n1, double n2)
        {
            return n1 + n2;
        }

        /**
        * @brief 숫자를 빼는 함수
        * @param n1: 첫 번째 숫자, n2: 두 번째 숫자
        * @return n1 - n2: 첫 번째 숫자에서 두 번째 숫자를 뺀 값
        * @note Patch-notes
        * 2023-08-10 | 박유진 | 첫 번째 입력 값에서 두 번째 입력 값을 빼줌
        */
        public double Subtract(double n1, double n2)
        {
            return n1 - n2;
        }

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
    }

    public class PerformCalculator : BaseCalculator
    {
        public double PerformAddition(double n1, double n2)
        {
            return Add(n1, n2);
        }

        public double PerformSubtraction(double n1, double n2)
        {
            return Subtract(n1, n2);
        }

        public double PerformMultiplication(double n1, double n2)
        {
            return Muliple(n1, n2);
        }

        public double PerformDivision(double n1, double n2)
        {
            return Divide(n1, n2);
        }
    }
}