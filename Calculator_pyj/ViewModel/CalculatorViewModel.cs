using Calculator_pyj.ViewModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Calculator.ViewModel
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        #region [상수]
        string result;
        SelectedOperator selectedOperator;
        double lastNumber;

        #endregion

        public string Result
        {
            get { return result; }
            set
            {
                result = value;
                OnPropertyChanged("Result");
            }
        }

        public enum SelectedOperator
        {
            None,
            Addiction,
            Substraction,
            Multiplication,
            Division
        }

        public class SimpleMath
        {
            public static double Add(double n1, double n2)
            {
                return n1 + n2;
            }

            public static double Subtract(double n1, double n2)
            {
                return n1 - n2;
            }

            public static double Muliple(double n1, double n2)
            {
                return n1 * n2;
            }

            public static double Divide(double n1, double n2)
            {
                return n1 / n2;
            }
        }

        public ICommand NumberCommand { get; }
        public ICommand AcCommand { get; }
        public ICommand PlusMinusCommand { get; }
        public ICommand PercentCommand { get; }
        public ICommand OperatorCommand { get; }
        public ICommand EqualCommand { get; }
        public ICommand DotCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CalculatorViewModel()
        {
            NumberCommand = new RelayCommand<string>(executeNumberCommand);
            AcCommand = new RelayCommand<string>(executeAcCommand);
            PlusMinusCommand = new RelayCommand<string>(executePlusMinusCommand);
            PercentCommand = new RelayCommand<string>(executePercentCommand);
            OperatorCommand = new RelayCommand<string>(executeOperatorCommand);
            EqualCommand = new RelayCommand<string>(executeEqualCommand);
            DotCommand = new RelayCommand<string>(executeDotCommand);
        }

        // Command methods here
        private void executeNumberCommand(object parameter)
        {
            if (parameter is string number)
            {
                if (Result == "0")
                {
                    Result = number;
                }
                else
                {
                    Result += number;
                }
            }
        }


        private void executeAcCommand(object parameter)
        {
            Result = "0";
            lastNumber = 0;
            selectedOperator = SelectedOperator.None;
        }

        private void executePlusMinusCommand(object parameter)
        {
            if (double.TryParse(Result, out double numericResult))
            {
                numericResult *= -1;
                Result = numericResult.ToString();
            }

        }

        private void executePercentCommand(object parameter)
        {
            if (double.TryParse(Result, out double numericResult))
            {
                numericResult *= 0.01;
                Result = numericResult.ToString();
            }
        }

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
                    Result = " ";
                }
            }
        }



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
                        numericResult = SimpleMath.Divide(lastNumber, numericResult);
                        break;
                }

                Result = numericResult.ToString();
                selectedOperator = SelectedOperator.None;
            }
        }

        private void executeDotCommand(object parameter)
        {
            if (!Result.Contains("."))
            {
                Result += ".";
            }
        }
    }
}
