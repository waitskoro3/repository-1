using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp11.Model;
using WpfApp11.View;

namespace WpfApp11.ViewModel
{
    public class WaiterVM : BaseVM
    {
        //Вывод всех блюд

        public static ObservableCollection<Dish> listOfDishes = new(ServiceDB.db.Dish);
        public ObservableCollection<Dish> ListOfDishes
        {
            get { return listOfDishes; }
            set { listOfDishes = value; OnPropertyChanged(); }
        }

        //Вывод всех заказов
        public static ObservableCollection<Order> allOrder = new(ServiceDB.db.Order);
        public ObservableCollection<Order> AllOrder
        {
            get { return allOrder; }
            set { allOrder = value; OnPropertyChanged(); }
        }

        public static Order SelectedOrder { get; set; }

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

        private RelayCommand changeStatus;
        public RelayCommand ChangeStatus
        {
            get
            {
                return changeStatus ?? new RelayCommand(obj =>
                {
                    if (SelectedOrder != null)
                    {
                        Order newOrder = ServiceDB.db.Order.FirstOrDefault(f => f.ID == SelectedOrder.ID);
                        if (newOrder != null)
                        {
                            if (newOrder.StatusPayID == 1)
                            {
                                newOrder.StatusPayID = 2;
                                ServiceDB.db.SaveChanges(); //Сохранение изменений
                                AllOrder = new(ServiceDB.db.Order); //Для обновления DataGrid
                            }
                            else if (newOrder.StatusPayID == 2)
                            {
                                MessageBox.Show("Заказ уже оплачен!");
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
        private RelayCommand addNewOrder;
        public RelayCommand AddNewOrder
        {
            get
            {
                return exit ?? new RelayCommand(obj =>
                {
                    new AddNewOrder_Waiter__VM().Show();
                    foreach (Window item in Application.Current.Windows)
                    {
                        if (item.DataContext == this) item.Close();
                    }
                });
            }
        }
    }
}
