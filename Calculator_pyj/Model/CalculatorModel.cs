using System.Collections.Generic;

namespace Calculator_pyj.Model
{
    class CalculatorModel
    {
        string result;
        string expression;
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
        * @brief 연산을 수행하는 메서드
        * @param operand1: 첫 번째 피연산자, operand2: 두 번째 피연산자, operatorSymbol: 연산자
        * @note Patch-notes
        * 2023-08-10 | 박유진 |
        */
        public double PerformOperation(double operand1, double operand2, string operatorSymbol)
        {
            switch (operatorSymbol)
            {
                case "+":
                    return operand1 + operand2;
                case "-":
                    return operand1 - operand2;
                case "x":
                    return operand1 * operand2;
                case "/":
                    if (operand2 != 0)
                    {
                        return operand1 / operand2;
                    }
                    else
                    {
                        result = "Error";
                        return 0;
                    }

            }
            return 0;


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
}