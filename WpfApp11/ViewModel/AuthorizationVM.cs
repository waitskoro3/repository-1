using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp11.Model;

namespace WpfApp11.ViewModel
{
    public class AuthorizationVM
    {
        private string login;
        public string Login
        {
            get { return login; }
            set
            {
                login = value;
            }
        }

        private RelayCommand loginCommand;
        public RelayCommand LoginCommand
        {
            get
            {
                return loginCommand ?? (loginCommand = new RelayCommand(o =>
                {
                    PasswordBox pb = o as PasswordBox;
                    string pass = pb.Password;

                    User LoginNPass = ServiceDB.db.User.FirstOrDefault(p => p.Login == Login && p.Password == pass);
                    if (LoginNPass != null)
                    {     
                        User RoleCooker = ServiceDB.db.User.FirstOrDefault(p => p.Login == Login && p.RoleID == 2);
                        User RoleWaiter = ServiceDB.db.User.FirstOrDefault(p => p.Login == Login && p.RoleID == 1);

                        if (RoleCooker != null)
                        {
                            new View.CookerV().Show();
                            foreach (Window item in Application.Current.Windows)
                            {
                                if (item.DataContext == this) item.Close();
                            }
                        }
                        if (RoleWaiter != null)
                        {
                            new View.Waiter().Show();

                            foreach (Window item in Application.Current.Windows)
                            {
                                if (item.DataContext == this) item.Close();
                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show("error");
                    }
                }));
            }
        }
    }
}
