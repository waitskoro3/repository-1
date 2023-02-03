using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp11.Model;

namespace WpfApp11.ViewModel
{
    public class CookerVM : BaseVM
    {
        public static ObservableCollection<Order> allOrder = new (ServiceDB.db.Order.Include(x => x.StatusReady));
        public ObservableCollection<Order> AllOrder
        {
            get { return allOrder; }
            set { allOrder = value; OnPropertyChanged(); }
        }
        public static Order SelectedOrder { get; set; }

        private RelayCommand changeStatus;
        public RelayCommand ChangeStatus
        {
            get
            {
                return changeStatus ?? new RelayCommand(obj =>
                {
                    if(SelectedOrder != null)
                    {
                        Order newOrder = ServiceDB.db.Order.FirstOrDefault(f => f.ID == SelectedOrder.ID);
                        if (newOrder != null)
                        {
                            if (newOrder.StatusReadyID == 1)
                            {
                                newOrder.StatusReadyID = 2;
                                ServiceDB.db.SaveChanges(); //Сохранение изменений
                                AllOrder = new(ServiceDB.db.Order.Include(x => x.StatusReady)); //Для обновления DataGrid
                            }
                            else if (newOrder.StatusReadyID == 2)
                            {
                                newOrder.StatusReadyID = 3;
                                ServiceDB.db.SaveChanges(); //Сохранение изменений
                                AllOrder = new(ServiceDB.db.Order.Include(x => x.StatusReady)); //Для обновления DataGrid
                            }
                            else if (newOrder.StatusReadyID == 3)
                            {
                                MessageBox.Show("Заказ уже готов!");
                            }
                        }
                    }
                    else if (SelectedOrder == null)
                    {
                        MessageBox.Show("Заказ не выбран");
                    }
                });
            }
        }

        private RelayCommand exit;
        public RelayCommand Exit
        {
            get
            {
                return exit ?? new RelayCommand(obj =>
                {
                    new WpfApp11.WaiterVM().Show();

                    foreach (Window item in Application.Current.Windows)
                    {
                        if (item.DataContext == this) item.Close();
                    }
                });
            }
        }
    }
}
