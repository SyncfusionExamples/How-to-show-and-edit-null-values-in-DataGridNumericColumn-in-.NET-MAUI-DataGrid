using Syncfusion.Maui.Core.Internals;
using Syncfusion.Maui.DataGrid;
using Syncfusion.Maui.Inputs;
using Syncfusion.Maui.Themes;
using System.Collections.ObjectModel;
using System.Globalization;

namespace DataGridSample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            dataGrid.Columns.Add(new DataGridNumericColumn()
            {
                HeaderText = "Order ID",
                MappingName = "OrderID",
                Width = 150,
                AllowNullValue = true,
                Format = "0"
            });


            dataGrid.Columns.Add(new DataGridTextColumn()
            {
                HeaderText = "Customer ID",
                MappingName = "CustomerID",
                Width = 150
            });


            dataGrid.Columns.Add(new DataGridTextColumn()
            {
                HeaderText = "Ship Country",
                MappingName = "ShipCountry",
                Width = 150
            });


            dataGrid.Columns.Add(new DataGridNumericColumn()
            {
                HeaderText = "Quantity",
                MappingName = "Qty",
                Width = 150,
                AllowNullValue = true,
                Format = "0",
                DisplayBinding = new Binding
                {
                    Path = "Qty",
                    Converter = new DisplayBindingConverter(), 
                }
            });


            dataGrid.CellRenderers.Remove("Numeric");
            dataGrid.CellRenderers.Add("Numeric", new CustomTextRenderer());
        }

    }

    public class CustomTextRenderer : DataGridNumericCellRenderer
    {
        private bool _ignoreNextValueChanged = false;

        public override void OnInitializeEditView(DataColumnBase dataColumn, SfNumericEntry view)
        {
            base.OnInitializeEditView(dataColumn, view);

            if (view != null && view is SfNumericEntry entry)
            {
                var value = dataColumn.CellValue;

                if (value == null && dataColumn != null && dataColumn.DataGridColumn != null && dataColumn.DataGridColumn.MappingName == "Qty")
                {
                    entry.ValueChanged -= Entry_ValueChanged;
                    _ignoreNextValueChanged = true;
                    entry.Value = null;

                    entry.ValueChanged += Entry_ValueChanged;
                }

            }
        }

        private void Entry_ValueChanged(object? sender, NumericEntryValueChangedEventArgs e)
        {
            if (_ignoreNextValueChanged)
            {
                _ignoreNextValueChanged = false; 
                return; 
            }

            if (sender is SfNumericEntry entry)
            {
                if (e.NewValue == 0)
                {
                    entry.Value = 0;
                }
            }


        }
    }

    public class DisplayBindingConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? priceValue = (int?)value;
            if (priceValue == null)
            {
                return null;
            }
            else
            {
                if (parameter is int type)
                {
                    return string.Format(culture, (type == 4) ? "{0:N0}" : "{0:N2}", priceValue);
                }
                return string.Format(culture, "{0:N2}", priceValue);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    public class OrderInfo
    {
        private int orderID;
        private string customerID;
        private string customer;
        private string shipCity;
        private string shipCountry;
        private int? qty;

        public int OrderID
        {
            get { return orderID; }
            set { this.orderID = value; }
        }

        public string CustomerID
        {
            get { return customerID; }
            set { this.customerID = value; }
        }

        public string ShipCountry
        {
            get { return shipCountry; }
            set { this.shipCountry = value; }
        }

        public string Customer
        {
            get { return this.customer; }
            set { this.customer = value; }
        }

        public string ShipCity
        {
            get { return shipCity; }
            set { this.shipCity = value; }
        }

        public int? Qty
        {
            get { return qty; }
            set { qty = value; }
        }


        public OrderInfo(int orderId, string customerId, string country, string customer, string shipCity,int? qty)
        {
            this.OrderID = orderId;
            this.CustomerID = customerId;
            this.Customer = customer;
            this.ShipCountry = country;
            this.ShipCity = shipCity;
            this.Qty = qty;
        }
    }

    public class OrderInfoRepository
    {
        private ObservableCollection<OrderInfo> orderInfo;
        public ObservableCollection<OrderInfo> OrderInfoCollection
        {
            get { return orderInfo; }
            set { this.orderInfo = value; }
        }

        public OrderInfoRepository()
        {
            orderInfo = new ObservableCollection<OrderInfo>();
            this.GenerateOrders();
        }

        public void GenerateOrders()
        {
            orderInfo.Add(new OrderInfo(1001, "Maria Anders", "Germany", "ALFKI", "Berlin", null));
            orderInfo.Add(new OrderInfo(1002, "Ana Trujillo", "Mexico", "ANATR", "Mexico D.F.", 15));
            orderInfo.Add(new OrderInfo(1003, "Ant Fuller", "Mexico", "ANTON", "Mexico D.F.", 20));
            orderInfo.Add(new OrderInfo(1004, "Thomas Hardy", "UK", "AROUT", "London", 25));
            orderInfo.Add(new OrderInfo(1005, "Tim Adams", "Sweden", "BERGS", "London", 30));
            orderInfo.Add(new OrderInfo(1006, "Hanna Moos", "Germany", "BLAUS", "Mannheim", 12));
            orderInfo.Add(new OrderInfo(1007, "Andrew Fuller", "France", "BLONP", "Strasbourg", 18));
            orderInfo.Add(new OrderInfo(1008, "Martin King", "Spain", "BOLID", "Madrid", 22));
            orderInfo.Add(new OrderInfo(1009, "Lenny Lin", "France", "BONAP", "Marsiella", 16));
            orderInfo.Add(new OrderInfo(1010, "John Carter", "Canada", "BOTTM", "Lenny Lin", 14));
            orderInfo.Add(new OrderInfo(1011, "Laura King", "UK", "AROUT", "London", 19));
            orderInfo.Add(new OrderInfo(1012, "Anne Wilson", "Germany", "BLAUS", "Mannheim", 17));
            orderInfo.Add(new OrderInfo(1013, "Martin King", "France", "BLONP", "Strasbourg", 21));
            orderInfo.Add(new OrderInfo(1014, "Gina Irene", "UK", "AROUT", "London", 13));
        }

    }
}
