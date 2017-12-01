using System.Security;
using System.Windows.Controls;
using System.Windows.Input;
using StockingSelector.Utility;
using StockingSelector.View.Command;

namespace StockingSelector.ViewModel
{
  /// <summary>
  /// @Document
  /// </summary>
  public class LoginViewModel : PropertyChangeNotifier
  {
    #region Fields

    private ICommand _cancelCommand;
    private ICommand _loginCommand;
    private ICommand _passwordChangedCommand;

    #endregion

    #region Properties
    
    /// <summary>
    /// @Document
    /// </summary>
    public string Title { get; } = $"{App.Name} [v{App.Version}]: Login";


    /// <summary>
    /// @Document
    /// </summary>
    public string Username { get; set; }


    /// <summary>
    /// @Document
    /// </summary>
    public SecureString Password { get; set; }


    /// <summary>
    /// @Document
    /// </summary>
    public bool DidUserLogin { get; private set; }

    #endregion

    #region Commands

    /// <summary>
    /// @Document
    /// </summary>
    public ICommand CancelCommand => _cancelCommand ?? (_cancelCommand = new DelegateCommand(OnCancel));


    /// <summary>
    /// @Document
    /// </summary>
    public ICommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(OnLogin, o => ValidateLogin()));


    /// <summary>
    /// @Document
    /// </summary>
    public ICommand PasswordChangedCommand => _passwordChangedCommand ?? (_passwordChangedCommand = new DelegateCommand(param =>
    {
      if (param is PasswordBox passwordBox)
        Password = passwordBox.SecurePassword;
    }));

    #endregion

    #region Private Functions
    
    /// <summary>
    /// @Document
    /// </summary>
    private void OnCancel() => DidUserLogin = false;


    /// <summary>
    /// @Document
    /// </summary>
    /// <returns></returns>
    private bool ValidateLogin() => !string.IsNullOrWhiteSpace(Username) && Password != null;


    /// <summary>
    /// @Document
    /// </summary>
    private void OnLogin() => DidUserLogin = true;

    #endregion
  }
}
