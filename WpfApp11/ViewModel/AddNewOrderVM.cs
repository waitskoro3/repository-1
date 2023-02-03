using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using WpfApp11.Model;
using WpfApp11.ViewModel;

namespace WpfApp11.ViewModel
{
    public class AddNewOrderVM : BaseVM
    {
        private Dish dish;
        public Dish Dish
        {
            get => dish;
            set
            {
                dish = value;
                OnPropertyChanged();
            }
        }

        //Вывод всех блюд

        public static ObservableCollection<Dish> listOfDishes = new(ServiceDB.db.Dish);
        public ObservableCollection<Dish> ListOfDishes
        {
            get { return listOfDishes; }
            set { listOfDishes = value; OnPropertyChanged(); }
        }

        //Создание заказа
        private static ObservableCollection<Dish> selectedDishes = new ObservableCollection<Dish>();
        public ObservableCollection<Dish> SelectedDishes
        {
            get { return selectedDishes; }
            set
            {
                selectedDishes = value;
                OnPropertyChanged();
            }
        }

        public static Dish InOrder { get; set; }

        private RelayCommand addNewDish;
        public RelayCommand AddNewDish
        {
            get
            {
                return addNewDish ?? new RelayCommand(obj =>
                {
                    if (InOrder != null)
                    {
                        selectedDishes.Add(InOrder);
                        ServiceDB.db.SaveChanges();
                        OnPropertyChanged();
                    }
                    else
                    {
                        MessageBox.Show("Блюдо не выбрано!");
                    }
                });
            }
        }

        private RelayCommand deleteDish;
        public RelayCommand DeleteDish
        {
            get
            {
                return deleteDish ?? new RelayCommand(obj =>
                {
                    if (Dish != null)
                    {
                        SelectedDishes.Remove(Dish);
                        OnPropertyChanged();
                    }
                    else
                    {
                        MessageBox.Show("Блюдо не выбрано");
                    }
                });
            }
        }

        private RelayCommand addNewOrder;
        public RelayCommand AddNewOrder
        {
            get
            {
                return addNewOrder ?? new RelayCommand(obj =>
                {
                    if (SelectedDishes.Count != 0)
                    {
                        Order order = new Order()
                        {
                            StatusReadyID = 1,
                            StatusPayID = 1,
                        };
                        ServiceDB.db.Order.Add(order);
                        ServiceDB.db.SaveChanges();

                        int orderCreatedId = ServiceDB.db.Order.OrderByDescending(q => q.ID).First().ID;
                        for (int i = 0; i < selectedDishes.Count; i++)
                        {
                            OrderDish ordersDish = new OrderDish()
                            {
                                DishID = SelectedDishes[i].ID,
                                OrderID = orderCreatedId,
                            };
                            ServiceDB.db.OrderDish.Add(ordersDish);
                            ServiceDB.db.SaveChanges();
                        }
                        new View.Waiter().Show();
                        foreach (Window item in Application.Current.Windows)
                        {
                            if (item.DataContext == this) item.Close();
                        }
                        OnPropertyChanged();
                    }
                    else
                    {
                        MessageBox.Show("Блюда не добавлены");
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
