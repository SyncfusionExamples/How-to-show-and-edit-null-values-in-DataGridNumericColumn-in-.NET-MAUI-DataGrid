# How-to-show-and-edit-null-values-in-DataGridNumericColumn-in-.NET-MAUI-DataGrid

This demo shows how to display null values as blank and still preserve an explicitly typed 0 in the `Syncfusion .NET MAUI SfDataGrid`'s numeric column. By using this sample, we ensure cells remain blank for nulls, while user-entered 0 is correctly saved and displayed.

You can bind the numeric column to a nullable property (for example, int? or double?) and enable `AllowNullValue` to let the grid hold nulls. We replace the default numeric cell renderer so the editor stays blank when the underlying value is null. We also apply a simple value converter through the column's `DisplayBinding` to show blank only for null (and not for 0). With this setup, null values appear empty in both display and edit states, and when users type 0, it is retained and shown as 0.

## C#
```
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

```