namespace Calculator_pyj.Model
{
    class CalculatorModel
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
}
