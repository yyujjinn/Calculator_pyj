using Calculator_pyj.ViewModel;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Calculator.ViewModel
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        #region [변수]
        string result;
        string expression;
        string errorMessage;
        SelectedOperator selectedOperator;
        double lastNumber;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region [필드]
        public enum SelectedOperator
        {
            None,
            Addiction,
            Substraction,
            Multiplication,
            Division
        }

        #endregion

        #region [속성]
        public string Expression
        {
            get { return expression; }
            set
            {
                expression = value;
                OnPropertyChanged("Expression");
            }
        }

        public string Result
        {
            get { return result; }
            set
            {
                result = value;
                OnPropertyChanged("Result");
            }
        }

        public ICommand NumberCommand { get; }
        public ICommand AcCommand { get; }
        public ICommand PlusMinusCommand { get; }
        public ICommand PercentCommand { get; }
        public ICommand OperatorCommand { get; }
        public ICommand EqualCommand { get; }
        public ICommand DotCommand { get; }

        #endregion

        #region [생성자]
        public CalculatorViewModel()
        {
            NumberCommand = new RelayCommand<string>(executeNumberCommand);
            AcCommand = new RelayCommand<string>(executeAcCommand);
            PlusMinusCommand = new RelayCommand<string>(executePlusMinusCommand);
            PercentCommand = new RelayCommand<string>(executePercentCommand);
            OperatorCommand = new RelayCommand<string>(executeOperatorCommand);
            EqualCommand = new RelayCommand<string>(executeEqualCommand);
            DotCommand = new RelayCommand<string>(executeDotCommand);
            result = "0";
            expression = "";
        }



        #endregion

        #region [메서드]
        /**
        * @brief 속성 변경 이벤트 발생시키는 메서드
        * @param propertyName: 변경된 속성의 이름
        * @note Patch-notes
        * 2023-08-10 | 박유진 | 속성 값이 변경될 때마다 호출되어 속성 변경 이벤트 발생시킴
        */
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /**
        * @brief 숫자 버튼이 클릭되었을 때 호출되는 메서드
        * @param number: 입력된 숫자
        * @note Patch-notes
        * 2023-08-10 | 박유진 | 숫자 버튼이 클릭될 때마다 호출되어 숫자를 결과에 추가. 초기 값이 0일 경우 입력된 숫자를 결과로 대체
        */
        private void executeNumberCommand(object parameter)
        {
            if (parameter is string number)
            {
                if (Result == "0")
                {
                    Result = number;
                    Expression = number;
                }
                else
                {
                    Result += number;
                    Expression += number;
                }
            }
        }

        /**
        * @brief AC 버튼이 클릭되었을 때 호출되는 메서드
        * @note Patch-notes
        * 2023-08-10 | 박유진 | AC 버튼이 클릭될 때마다 호출되어 결과를 0으로 초기화, 마지막 숫자와 선택된 연산자를 초기화
        */
        private void executeAcCommand(object parameter)
        {
            Result = "0";
            Expression = "0";
            lastNumber = 0;
            selectedOperator = SelectedOperator.None;
        }

        /**
        * @brief +/- 버튼이 클릭되었을 때 호출되는 메서드
        * @param numericResult: 입력된 숫자
        * @note Patch-notes
        * 2023-08-10 | 박유진 | +/- 버튼이 클릭될 때마다 호출되어 입력된 숫자의 부호를 변경, 결과를 string으로 변경
        */
        private void executePlusMinusCommand(object parameter)
        {
            if (double.TryParse(Result, out double numericResult))
            {
                numericResult *= -1;
                Result = numericResult.ToString();
                Expression = numericResult.ToString();
            }
        }

        /**
        * @brief % 버튼이 클릭되었을 때 호출되는 메서드
        * @param numericResult: 입력된 숫자
        * @note Patch-notes
        * 2023-08-10 | 박유진 | % 버튼이 클릭될 때마다 호출되어 입력된 숫자에 0,01을 곱함, 결과를 string으로 변경
        */
        private void executePercentCommand(object parameter)
        {
            if (double.TryParse(Result, out double numericResult))
            {
                numericResult *= 0.01;
                Result = numericResult.ToString();
                Expression = numericResult.ToString();

            }
        }

        /**
        * @brief . 버튼이 클릭되었을 때 호출되는 메서드
        * @note Patch-notes
        * 2023-08-10 | 박유진 | . 버튼이 클릭될 때마다 호출되어 .을 입력. .이 이미 있는 경우 실행되지 않음
        */
        private void executeDotCommand(object parameter)
        {
            if (!Result.Contains("."))
            {
                Result += ".";
                Expression += ".";
            }
        }

        /**
        * @brief +, -, x, / 버튼이 클릭되었을 때 호출되는 메서드
        * @param op: 입력된 연산자
        * @note Patch-notes
        * 2023-08-10 | 박유진 | 입력된 연산자에 따라 selectedOperator를 설정하고 현재 표시된 숫자를 lastNumber로 저장
        */
        private void executeOperatorCommand(object parameter)
        {
            if (parameter is string op)
            {
                switch (op)
                {
                    case "+":
                        selectedOperator = SelectedOperator.Addiction;
                        break;
                    case "-":
                        selectedOperator = SelectedOperator.Substraction;
                        break;
                    case "x":
                        selectedOperator = SelectedOperator.Multiplication;
                        break;
                    case "/":
                        selectedOperator = SelectedOperator.Division;
                        break;
                    default:
                        selectedOperator = SelectedOperator.None;
                        break;
                }

                if (double.TryParse(Result, out double numericResult))
                {
                    lastNumber = numericResult;
                    Expression += op;
                    Result = "";
                }
            }
        }

        /**
        * @brief = 버튼이 클릭되었을 때 호출되는 메서드
        * @param op: 입력된 연산자
        * @note Patch-notes
        * 2023-08-10 | 박유진 | 설정된 연산자에 따라 lastNumber과 현재 입력된 숫자를 사용하여 계산을 수행하고 결과를 출력
        * @warning 설정된 연산자가 없거나 현재 표시된 숫자가 유효하지 않은 경우 계산을 수행하지 않음
        */
        private void executeEqualCommand(object parameter)
        {
            if (selectedOperator != SelectedOperator.None && double.TryParse(Result, out double numericResult))
            {
                switch (selectedOperator)
                {
                    case SelectedOperator.Addiction:
                        numericResult = SimpleMath.Add(lastNumber, numericResult);
                        break;
                    case SelectedOperator.Substraction:
                        numericResult = SimpleMath.Subtract(lastNumber, numericResult);
                        break;
                    case SelectedOperator.Multiplication:
                        numericResult = SimpleMath.Muliple(lastNumber, numericResult);
                        break;
                    case SelectedOperator.Division:
                        if (numericResult != 0)
                        {
                            numericResult = SimpleMath.Divide(lastNumber, numericResult);
                        }
                        else
                        {
                            errorMessage = "Error";
                        }
                        break;
                }
                if (errorMessage == null)
                {
                    Result = numericResult.ToString();
                    selectedOperator = SelectedOperator.None;
                    Expression += "=";
                }
                else
                {
                    Result = errorMessage;
                }
            }
        }

        #endregion

        #region [클래스]
        public class SimpleMath
        {
            /**
            * @brief 숫자를 더하는 함수
            * @param n1: 첫 번째 숫자, n2: 두 번째 숫자
            * @return n1 + n2: 두 숫자의 합
            * @note Patch-notes
            * 2023-08-10 | 박유진 | 첫 번째 입력 값과 두 번째 입력 값을 더해줌
            */
            public static double Add(double n1, double n2)
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
            public static double Subtract(double n1, double n2)
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
            public static double Muliple(double n1, double n2)
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
            public static double Divide(double n1, double n2)
            {
                return n1 / n2;
            }
        }
        #endregion

    }
}
