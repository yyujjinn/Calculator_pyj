using Calculator_pyj.Model;
using Calculator_pyj.ViewModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Calculator.ViewModel
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {

        #region [변수]
        string result;
        string expression;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region [필드]
        Stack<string> operatorStack = new Stack<string>();
        List<string> postfixTokens = new List<string>();
        private ObservableCollection<string> historyItems = new ObservableCollection<string>();
        private Visibility historyVisibility = Visibility.Collapsed;
        private bool isHistoryOpen;

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

        public ObservableCollection<string> HistoryItems
        {
            get { return historyItems; }
            set
            {
                historyItems = value;
                OnPropertyChanged("HistoryItems");
            }
        }

        public Visibility HistoryVisibility
        {
            get { return historyVisibility; }
            set
            {
                historyVisibility = value;
                OnPropertyChanged("HistoryVisibility");
            }
        }

        public bool IsHistoryOpen
        {
            get { return isHistoryOpen; }
            set
            {
                isHistoryOpen = value;
                OnPropertyChanged("IsHistoryOpen");

                HistoryVisibility = isHistoryOpen ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public ICommand NumberCommand { get; }
        public ICommand HistoryCommand { get; }
        public ICommand PlusMinusCommand { get; }
        public ICommand AcCommand { get; }
        public ICommand BracketCommand { get; }
        public ICommand PercentCommand { get; }
        public ICommand OperatorCommand { get; }
        public ICommand EqualCommand { get; }
        public ICommand DotCommand { get; }
        public ICommand CopyCommand { get; }
        public ICommand PasteCommand { get; }

        #endregion

        #region [생성자]
        public CalculatorViewModel()
        {
            NumberCommand = new RelayCommand<string>(executeNumberCommand);
            HistoryCommand = new RelayCommand<object>(executeHistoryCommand);
            AcCommand = new RelayCommand<string>(executeAcCommand);
            PlusMinusCommand = new RelayCommand<string>(executePlusMinusCommand);
            PercentCommand = new RelayCommand<string>(executePercentCommand);
            OperatorCommand = new RelayCommand<string>(executeOperatorCommand);
            EqualCommand = new RelayCommand<string>(executeEqualCommand);
            DotCommand = new RelayCommand<string>(executeDotCommand);
            CopyCommand = new RelayCommand<object>(executeCopyCommand);
            PasteCommand = new RelayCommand<object>(executePasteCommand);
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
        private bool IsOperator(string token)
        {
            return token == "+" || token == "-" || token == "x" || token == "/";
        }

        /**
        * @brief 연산을 수행하는 메서드
        * @param operand1: 첫 번째 피연산자, operand2: 두 번째 피연산자, operatorSymbol: 연산자
        * @note Patch-notes
        * 2023-08-10 | 박유진 |
        */
        private double PerformOperation(double operand1, double operand2, string operatorSymbol)
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
                        Result = "Error";
                        return 0;
                    }

            }
            return 0;

            #endregion
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
        * @brief History 버튼이 클릭되었을 때 호출되는 메서드
        * @note Patch-notes
        * 2023-08-10 | 박유진 |
        */
        private void executeHistoryCommand(object parameter)
        {
            if (IsHistoryOpen)
            {
                HistoryVisibility = Visibility.Visible;
            }
            else
            {
                HistoryVisibility = Visibility.Collapsed;
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
            operatorStack.Clear();
            postfixTokens.Clear();
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
        * 2023-08-10 | 박유진 | 입력된 연산자를 수식에 저장
        */
        private void executeOperatorCommand(object parameter)
        {
            if (parameter is string op)
            {
                switch (op)
                {
                    case "+":
                    case "-":
                    case "x":
                    case "/":
                        Expression += " " + op + " ";
                        Result = "";
                        break;
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
            CalculatorModel calModel = new CalculatorModel();
            if (!string.IsNullOrWhiteSpace(Expression))
            {
                string postfixExpression = ConvertToPostfix(Expression);
                string[] postfixTokens = postfixExpression.Split(' ');

                Stack<double> valueStack = new Stack<double>();



                foreach (string token in postfixTokens)
                {
                    if (double.TryParse(token, out double numericValue) || token == ".")
                    {
                        valueStack.Push(numericValue);
                    }
                    else if (IsOperator(token))
                    {
                        if (valueStack.Count >= 2)
                        {
                            double operand2 = valueStack.Pop();
                            double operand1 = valueStack.Pop();

                            if (operand2 != 0)
                            {
                                double result = PerformOperation(operand1, operand2, token);
                                valueStack.Push(result);
                            }
                            else
                            {
                                Result = "Error";
                            }

                        }
                        else
                        {
                            Result = "Error";
                            return;
                        }
                    }
                }
                if (valueStack.Count == 1)
                {
                    Result = valueStack.Pop().ToString();
                    string historyEntry = $"{Expression} = {Result}";
                    HistoryItems.Add(historyEntry);

                }
                else
                {
                    Result = "Error";
                }
            }
        }

        private void executeCopyCommand(object parameter)
        {
            Clipboard.SetText(Result);
        }

        private void executePasteCommand(object parameter)
        {
            if (Clipboard.ContainsText())
            {
                Result = Clipboard.GetText();
                Expression = Result;
            }
        }

    }
}
